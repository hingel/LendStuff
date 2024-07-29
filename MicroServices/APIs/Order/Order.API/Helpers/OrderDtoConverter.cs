using LendStuff.Shared;
using LendStuff.Shared.DTOs;

namespace Order.API.Helpers;

public static class OrderDtoConverter
{
	public static DataAccess.Models.Order ConvertDtoToOrder(OrderDto orderDto)
	{
		var order = new DataAccess.Models.Order
		{
			BoardGameId = orderDto.BoardGameId,
			BorrowerId = orderDto.BorrowerUserId,
			LentDate = orderDto.LentDate ?? DateTime.Now,
			OwnerId = orderDto.OwnerUserId,
			ReturnDate = orderDto.ReturnDate ?? DateTime.Now,
			Status = orderDto.Status ?? OrderStatus.Inquiry,
			OrderId = orderDto.OrderId ?? Guid.NewGuid()
		};

		order.OrderMessageGuids.AddRange(orderDto.OrderMessageGuids);

        return order;
    }

	public static OrderDto ConvertOrderToDto(DataAccess.Models.Order o)
	{
		return new OrderDto(o.OrderId, o.OwnerId, o.BorrowerId, o.BoardGameId, o.LentDate, o.ReturnDate, o.Status, o.OrderMessageGuids.ToArray());
	}
}