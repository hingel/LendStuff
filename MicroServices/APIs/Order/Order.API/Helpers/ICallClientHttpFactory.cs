using LendStuff.Shared.DTOs;

namespace Order.API.Helpers;

public interface ICallClientHttpFactory
{ 
    Task<BoardGameDto?> Call(Guid boardGameId);
}