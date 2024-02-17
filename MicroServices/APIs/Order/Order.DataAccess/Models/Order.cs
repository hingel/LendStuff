using System.ComponentModel.DataAnnotations;
using LendStuff.Shared;

namespace Order.DataAccess.Models;

public class Order
{
	[Key]
	public Guid OrderId { get; set; }
	[Required]
	public Guid OwnerId { get; set; }
	[Required]
	public Guid BorrowerId { get; set; }
	[Required]
	public Guid BoardGameId { get; set; }
	[Required]
	public DateTime LentDate { get; set; }
	[Required]
	public DateTime ReturnDate { get; set; }
	[Required]
	public OrderStatus Status { get; set; }
	public ICollection<Guid> OrderMessages { get; set; } = new List<Guid>();
}