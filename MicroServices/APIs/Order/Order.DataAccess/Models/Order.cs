using System.ComponentModel.DataAnnotations;
using LendStuff.Shared;

namespace Order.DataAccess.Models;

public class Order
{
	[Key]
	public Guid OrderId { get; set; }
	public Guid OwnerId { get; set; }
	public Guid BorrowerId { get; set; }
	public Guid BoardGameId { get; set; }
	public DateTime LentDate { get; set; }
	public DateTime ReturnDate { get; set; }
	public OrderStatus Status { get; set; }
	public ICollection<Guid> OrderMessages { get; set; } = new List<Guid>();
}