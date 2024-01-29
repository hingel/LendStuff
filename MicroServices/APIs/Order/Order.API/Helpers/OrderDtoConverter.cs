using LendStuff.Shared.DTOs;

namespace Order.API.Helpers;

public static class OrderDtoConverter
{
	public static async Task<DataAccess.Models.Order> ConvertDtoToOrder(OrderDto newOrder)
	{
		return new DataAccess.Models.Order()
		{
			BoardGameId = newOrder.BoardGameId, // (await _unitOfWork.BoardGameRepository.FindByKey(b => b.Id == newOrder.BoardGameId)).FirstOrDefault(), //måste hämt
			BorrowerId = newOrder.BorrowerUserId,
			LentDate = newOrder.LentDate,
			OwnerId = newOrder.OwnerUserId,
			ReturnDate = newOrder.ReturnDate,
			Status = newOrder.Status,
			//OrderMessages = await FindMessages(newOrder.OrderMessageDtos) //Kopplat till Funktion 1:
			OrderMessages = newOrder.OrderMessageGuids, //Kopplat till Funktion 2:
			OrderId = newOrder.OrderId
		};
	}

	public static OrderDto ConvertOrderToDto(DataAccess.Models.Order o)
	{
		return new OrderDto()
		{
			BoardGameId =
				o.BoardGameId,
			BorrowerUserId =
				o.BorrowerId,
			LentDate = o.LentDate,
			OrderId = o.OrderId,
			OwnerUserId = o.OwnerId,
			ReturnDate = o.ReturnDate,
			Status = o.Status,
			OrderMessageGuids  = o.OrderMessages.ToList()
		};
	}
}