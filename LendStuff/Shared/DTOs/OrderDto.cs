namespace LendStuff.Shared.DTOs;

public class OrderDto
{ public int OrderId { get; set; }
	public UserDto Owner { get; set; }
	public UserDto Borrower { get; set; }
	public BoardGameDto BoardGame { get; set; }
	public DateTime LentDate { get; set; }
	public DateTime ReturnDate { get; set; }
	public OrderStatus Status { get; set; }
}