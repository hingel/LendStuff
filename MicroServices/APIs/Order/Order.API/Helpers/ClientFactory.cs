using System.Net.Http.Headers;
using LendStuff.Shared;
using LendStuff.Shared.DTOs;
using Microsoft.AspNetCore.Authentication;

namespace Order.API.Helpers;

public class ClientFactory(IHttpClientFactory httpClientFactory, IHttpContextAccessor contextAccessor)
{
    public async Task<BoardGameDto?> Call(Guid boardGameId)
    {
        //TODO: Detta får uppdateras. Skriv en extern metod, kommer behöva göra likannde på andra ställen
        var uri = $"http://boardgame.api:8080/getGameById/{boardGameId}";
        var authenticationArray = contextAccessor.HttpContext.Request.Headers.Authorization.FirstOrDefault().Split(' ');

        var httpRequestMessage = new HttpRequestMessage(HttpMethod.Get, uri)
        {
            Headers = { Authorization = new AuthenticationHeaderValue(authenticationArray[0], authenticationArray[1]) }
        };

        using var httpClient = httpClientFactory.CreateClient();
        if (authenticationArray.Length <= 1)
        {
            throw new AuthenticationFailureException("Not authenticated");
        }
        
        var response = await httpClient.SendAsync(httpRequestMessage);

        if (!response.IsSuccessStatusCode)
        {
            throw new BadHttpRequestException("bad request");
        }

        var result = await response.Content.ReadFromJsonAsync<ServiceResponse<BoardGameDto>>();

        return result!.Data;
    }
}