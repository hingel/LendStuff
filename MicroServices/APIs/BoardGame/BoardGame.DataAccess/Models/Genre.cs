using System.ComponentModel.DataAnnotations;
using BoardGame.DataAccess.Repository;
using LendStuff.Shared;

namespace BoardGame.DataAccess.Models;

public class Genre : IEntity
{
	public Guid Id { get; init; }
	public string Name { get; set; } = string.Empty;
}