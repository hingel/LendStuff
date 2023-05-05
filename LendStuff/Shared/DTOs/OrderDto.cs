namespace LendStuff.Shared.DTOs;

public class OrderDto
{ public int OrderId { get; set; }
	public string OwnerUserId { get; set; }
	public string BorrowerUserId { get; set; }
	public string BoardGameId { get; set; }
	public DateTime LentDate { get; set; }
	public DateTime ReturnDate { get; set; }
	public OrderStatus Status { get; set; }
}