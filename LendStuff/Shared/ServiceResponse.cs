namespace LendStuff.Shared;

public record ServiceResponse <T>
{
	public T? Data { get; init; }
	public bool Success { get; init; }
	public string? Message { get; init; }
}