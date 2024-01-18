using Duende.IdentityServer.Models;
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
	public OrderService(IRepository<Order> orderRepository, UnitOfWork unitOfWork,
		UserManager<ApplicationUser> userManager, IRepository<InternalMessage> messageRepository)
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
		var result = await _orderRepository.FindByKey(o => o.Owner.Id == userId || o.BorrowerId == userId);

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

	public async Task<ServiceResponse<OrderDto>> GetOrderById(int orderId)
	{
		var result = (await _orderRepository.FindByKey(o => o.OrderId == orderId)).FirstOrDefault();

		if (result is not null)
		{
			return new ServiceResponse<OrderDto>()
			{
				Data = ConvertOrderToDto(result),
				Message = $"Order with id: {orderId} found.",
				Success = true
			};
		}

		return new ServiceResponse<OrderDto>()
		{
			Message = $"Order with id: {orderId} not found",
			Success = false
		};
	}

	public async Task<ServiceResponse<string>> AddOrder(OrderDto newOrderDto)
	{
		//newOrderDto.LentDate = DateTime.UtcNow; //TODO Bara för test ta bort
		//newOrderDto.ReturnDate = DateTime.UtcNow.AddDays(7); //TODO: Bara för test ta bort
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
			Data = $"Order with nr: {result.OrderId} added",
			Message = $"Order with nr: {result.OrderId} added",
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
			BorrowerId = newOrder.BorrowerUserId,
			LentDate = newOrder.LentDate,
			Owner = await _userManager.FindByNameAsync(newOrder.OwnerUserName),
			ReturnDate = newOrder.ReturnDate,
			Status = newOrder.Status,
			//OrderMessages = await FindMessages(newOrder.OrderMessageDtos) //Kopplat till Funktion 1:
			OrderMessages = await Task.WhenAll(newOrder.OrderMessageDtos.Select(FindOneMessage)), //Kopplat till Funktion 2:
			OrderId = newOrder.OrderId
		};
	}


	//Funktion 1:
	private async Task<List<InternalMessage>> FindMessages(List<MessageDto> messageDtosToFind)
	{
		var listWithMessages = new List<InternalMessage>();

		foreach (var messageDto in messageDtosToFind)
		{
			var foundMessage = (await _messageRepository.FindByKey(m => m.MessageId == messageDto.MessageId)).FirstOrDefault();

			if (foundMessage is null)
			{
				var newAddedMessage = await _messageRepository.AddItem(new InternalMessage()
				{
					Message = messageDto.Message,
					IsRead = messageDto.IsRead,
					MessageSent = messageDto.MessageSent,
					SentFromUserName = messageDto.SentFromUserName,
					SentToUser = (await _userManager.FindByNameAsync(messageDto.SentToUserName))!
				});

				listWithMessages.Add(newAddedMessage);
			}

			else
			{
				listWithMessages.Add(foundMessage);
			}
		}

		return listWithMessages;
	}

	//Funktion 2:
	private async Task<InternalMessage> FindOneMessage(MessageDto messageToFind)
	{
		var internalMessage = (await _messageRepository.FindByKey(m => m.MessageId == messageToFind.MessageId))
			.FirstOrDefault();

		if (internalMessage is not null) 
			return internalMessage;

		var newMessage = await _messageRepository.AddItem(new InternalMessage()
		{
			Message = messageToFind.Message,
			IsRead = messageToFind.IsRead,
			MessageSent = messageToFind.MessageSent,
			SentFromUserName = messageToFind.SentFromUserName,
			SentToUser = (await _userManager.FindByNameAsync(messageToFind.SentToUserName))!
		});
		
		return newMessage;
	}


	private OrderDto ConvertOrderToDto(Order o)
	{
		return new OrderDto()
		{
			BoardGameId =
				o.BoardGame.Id,
			BorrowerUserId =
				o.BorrowerId,
			LentDate = o.LentDate,
			OrderId = o.OrderId,
			OwnerUserName = o.Owner.UserName!,
			ReturnDate = o.ReturnDate,
			Status = o.Status,
			OrderMessageDtos = o.OrderMessages.Select(ConvertMessageToDto).ToList()
		};
	}

	//TODO: Denna ska flyttas ihop till DtoConvertServicen, förkommer två ggr. Och i MessageService.
	private MessageDto ConvertMessageToDto(InternalMessage messageToConvert)
	{
		return new MessageDto()
		{
			MessageId = messageToConvert.MessageId,
			Message = messageToConvert.Message!,
			MessageSent = messageToConvert.MessageSent,
			SentFromUserName = messageToConvert.SentFromUserName!,
			SentToUserName = messageToConvert.SentToUser.UserName!,
			IsRead = messageToConvert.IsRead
		};
	}
}