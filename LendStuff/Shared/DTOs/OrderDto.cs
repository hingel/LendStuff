namespace LendStuff.Shared.DTOs;

public class OrderDto
{ 
	public Guid OrderId { get; set; }
	public Guid OwnerUserId { get; set; }
	public Guid BorrowerUserId { get; set; }
	public Guid BoardGameId { get; set; }
	public DateTime LentDate { get; set; }
	public DateTime ReturnDate { get; set; }
	public OrderStatus Status { get; set; }
	public List<Guid> OrderMessageGuids { get; set; } = new(); 
}