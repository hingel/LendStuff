namespace LendStuff.Shared.DTOs;

public class UserDto
{
	public string UserName { get; set; }
	public string Street { get; set; }
	public string City { get; set; }
	public string PostalCode { get; set; }
	public IEnumerable<BoardGameDto> BoardGameDtos { get; set; } = new List<BoardGameDto>() { };
	public IEnumerable<MessageDto> MessageDtosDtos { get; set; } = new List<MessageDto>() { };

}