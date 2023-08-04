namespace LendStuff.Shared.DTOs;

public class OrderDto
{ 
	public int OrderId { get; set; }
	public string OwnerUserName { get; set; } = string.Empty;
	public string BorrowerUserId { get; set; } = string.Empty;
	public string BoardGameId { get; set; }
	public DateTime LentDate { get; set; }
	public DateTime ReturnDate { get; set; }
	public OrderStatus Status { get; set; }
	public List<MessageDto> OrderMessageDtos { get; set; } = new(); 
}