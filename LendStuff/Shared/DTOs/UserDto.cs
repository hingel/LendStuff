namespace LendStuff.Shared.DTOs;

public record UserDto(string UserName, string Email, Guid[] ActiveOrderGuids, int? Rating);