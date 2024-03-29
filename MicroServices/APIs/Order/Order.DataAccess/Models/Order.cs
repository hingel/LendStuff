using LendStuff.Shared;

namespace Order.DataAccess.Models;

public class Order
{
	public Guid OrderId { get; set; }
	public Guid OwnerId { get; set; }
	public Guid BorrowerId { get; set; }
	public Guid BoardGameId { get; set; }
	public DateTime LentDate { get; set; }
	public DateTime ReturnDate { get; set; }
	public OrderStatus Status { get; set; }
	public List<Guid> OrderMessagesGuid { get; } = new ();
}