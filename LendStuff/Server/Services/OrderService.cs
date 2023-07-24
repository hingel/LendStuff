﻿using Duende.IdentityServer.Models;
using LendStuff.DataAccess.Models;
using LendStuff.DataAccess.Repositories.Interfaces;
using LendStuff.Server.Models;
using LendStuff.Shared;
using LendStuff.Shared.DTOs;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace LendStuff.DataAccess.Services;

public class OrderService
{
	private readonly IRepository<Order> _orderRepository;
	private readonly UnitOfWork _unitOfWork;
	private readonly UserManager<ApplicationUser> _userManager;
	private readonly IRepository<InternalMessage> _messageRepository;
	public OrderService(IRepository<Order> orderRepository, UnitOfWork unitOfWork, UserManager<ApplicationUser> userManager, IRepository<InternalMessage> messageRepository)
	{
		_orderRepository = orderRepository;
		_unitOfWork = unitOfWork;
		_userManager = userManager;
		_messageRepository = messageRepository;
	}

	public async Task<ServiceResponse<IEnumerable<OrderDto>>> GetAllOrders()
	{
		var result = await _orderRepository.GetAll();

		var response = new ServiceResponse<IEnumerable<OrderDto>>()
		{
			Data = result.Select(ConvertOrderToDto),
			Message = "All orders",
			Success = true
		};

		return response;
	}

	public async Task<ServiceResponse<IEnumerable<OrderDto>>> GetAllUserOrders(string userId)
	{
		var result = await _orderRepository.FindByKey(o => o.Owner.Id == userId || o.Borrower.Id == userId);

		if (result.FirstOrDefault() is null)
		{
			return new ServiceResponse<IEnumerable<OrderDto>>()
			{
				Message = "Orders not found",
				Success = false
			};
		}

		return new ServiceResponse<IEnumerable<OrderDto>>()
		{
			Data = result.Select(ConvertOrderToDto),
			Message = $"order for {userId}",
			Success = true
		};
	}

	public async Task<ServiceResponse<string>> AddOrder(OrderDto newOrderDto)
	{
		newOrderDto.LentDate = DateTime.UtcNow; //Bara för test 
		newOrderDto.ReturnDate = DateTime.UtcNow.AddDays(7); //Bara för test
		var newOrder = await ConvertDtoToOrder(newOrderDto);
		
		if (!newOrder.BoardGame.Available)
		{
			return new ServiceResponse<string>()
			{
				Message = $"{newOrder.BoardGame.Title} is not available for lending",
				Success = false
			};
		}

		var result = await _orderRepository.AddItem(newOrder);

		return new ServiceResponse<string>()
		{
			Data = $"{result.OrderId} added",
			Message = $"{result.OrderId} added",
			Success = true
		};
	}

	public async Task<ServiceResponse<OrderDto>> UpdateOrder(OrderDto updatedOrderDto)
	{
		var updatedOrder = await ConvertDtoToOrder(updatedOrderDto);

		var result = await _orderRepository.Update(updatedOrder);

		return new ServiceResponse<OrderDto>()
		{
			Data = ConvertOrderToDto(result),
			Message = "Order sent",
			Success = true
		};
	}

	public async Task<ServiceResponse<string>> DeleteOrder(string orderId)
	{
		var result = await _orderRepository.Delete(orderId);

		return new ServiceResponse<string>()
		{
			Message = result,
			Success = true
		};
	}

	private async Task<Order> ConvertDtoToOrder(OrderDto newOrder)
	{
		return new Order()
		{
			BoardGame = (await _unitOfWork.BoardGameRepository.FindByKey(b => b.Id == newOrder.BoardGameId)).FirstOrDefault(),
			Borrower = await _userManager.FindByIdAsync(newOrder.BorrowerUserId),
			LentDate = newOrder.LentDate,
			Owner = await _userManager.FindByIdAsync(newOrder.OwnerUserId),
			ReturnDate = newOrder.ReturnDate,
			Status = newOrder.Status,
			OrderMessages = await Task.WhenAll(newOrder.OrderMessageDtos.Select(FindMessage).ToList()) //TODO: Funkar detta?
		};
	}

	private async Task<InternalMessage> FindMessage(MessageDto messageToFind) =>
		(await _messageRepository.FindByKey(m => m.MessageId == messageToFind.MessageId)).FirstOrDefault();

	private OrderDto ConvertOrderToDto(Order o)
	{
		return new OrderDto()
		{
			BoardGameId =
				o.BoardGame.Id, // (await _boardGameService.FindById(o.BoardGame.Id.ToString())).Data.Title,
			BorrowerUserId =
				o.Borrower.Id, // (await _userService.FindUserById(o.Borrower.Id)).Data, //hitta användaren o konvertera
			LentDate = o.LentDate,
			OrderId = o.OrderId,
			OwnerUserId = o.Owner.Id, //(await _userService.FindUserById(o.Owner.Id)).Data, //Hitta ägaren.
			ReturnDate = o.ReturnDate,
			Status = o.Status,
			OrderMessageDtos = o.OrderMessages.Select(ConvertMessageToDto).ToList()
		};
	}

	//TODO: Denna ska flyttas ihop till DtoConvertServicen.
	private MessageDto ConvertMessageToDto(InternalMessage messageToConvert)
	{
		return new MessageDto()
		{
			MessageId = messageToConvert.MessageId,
			Message = messageToConvert.Message,
			MessageSent = messageToConvert.MessageSent,
			SentFromUserName = messageToConvert.SentFromUserName,
			SentToUserName = messageToConvert.SentToUser.UserName,
			IsRead = messageToConvert.IsRead
		};
	}
}