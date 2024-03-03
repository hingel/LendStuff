﻿using LendStuff.Shared;

namespace BoardGame.DataAccess.Models;

public class UserBoardGame : IEntity
{
	public Guid Id { get; init; }
	public BoardGame BoardGame { get; set; } = null!;
	public Guid Owner { get; set; }
	public int Condition { get; set; }
	public string? Comment { get; set; } = string.Empty;
	public bool ForLending { get; set; }
}