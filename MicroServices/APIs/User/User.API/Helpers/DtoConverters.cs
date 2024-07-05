using LendStuff.Shared.DTOs;

namespace User.API.Helpers;

public static class DtoConverters
{ public static UserDto UserToDto(DataAccess.Models.User user) =>
        new(user.UserName, user.Email, [.. user.ActiveOrders], user.Rating);
}