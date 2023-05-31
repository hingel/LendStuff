namespace LendStuff.Shared.DTOs;

public class UserDto
{
	public string UserName { get; set; } = string.Empty;
	public string Street { get; set; } = string.Empty;
	public string City { get; set; } = string.Empty;
	public string PostalCode { get; set; } = string.Empty;
	public string Email { get; set; } = string.Empty;
	public List<UserBoardGameDto> BoardGameDtos { get; set; } = new () { };
	public IEnumerable<MessageDto> MessageDtosDtos { get; set; } = new List<MessageDto>() { };

}