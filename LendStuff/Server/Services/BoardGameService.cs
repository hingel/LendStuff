﻿using LendStuff.DataAccess.Models;
using LendStuff.DataAccess.Repositories;
using LendStuff.DataAccess.Repositories.Interfaces;
using LendStuff.Server.Models;
using LendStuff.Shared;
using LendStuff.Shared.DTOs;

namespace LendStuff.DataAccess.Services;

public class BoardGameService
{
	private readonly UnitOfWork _unitOfWork;

	public BoardGameService(UnitOfWork unitOfWork)
	{
		_unitOfWork = unitOfWork;
	}

	public async Task<ServiceResponse<IEnumerable<BoardGameDto>>> FindByTitle(string searchWord)
	{
		var result = await _unitOfWork.BoardGameRepository.FindByKey((game => game.Title.ToLower().Contains(searchWord.ToLower())));
		
		return new ServiceResponse<IEnumerable<BoardGameDto>>()
		{
			Data = result.Select(DtoConvert.ConvertBoardGameToDto),
			Message = $"{result.Count()} Boardgame{(result.Count() != 1 ? "s": "")} found.",
			Success = true
		};
	}

	public async Task<ServiceResponse<BoardGameDto>> FindById(string id)
	{
		var result = await _unitOfWork.BoardGameRepository.FindByKey((game => game.Id == id));

		if (result.Count() > 0)
		{
			return new ServiceResponse<BoardGameDto>()
			{
				Data = result.Select(DtoConvert.ConvertBoardGameToDto).First(),
				Message = $"Boardgame found.",
				Success = true
			};
		}

		return new ServiceResponse<BoardGameDto>()
		{
			Message = "No board game found",
			Success = false
		};
	}

	public async Task<ServiceResponse<string>> DeleteBoardGame(string id)
	{
		var result = await _unitOfWork.BoardGameRepository.Delete(id);
		var saveResult = await _unitOfWork.SaveChanges();

		return new ServiceResponse<string>()
		{
			Message = result,
			Success = true
		};
	}

	private async Task<BoardGame> ConvertDtoToBoardGame(BoardGameDto dtoToConvert)
	{
		return new BoardGame
		{
			Available = dtoToConvert.Available,
			BggLink = dtoToConvert.BggLink,
			Description = dtoToConvert.Description,
			Genres = await FindGenres(dtoToConvert.Genres),
			Id = dtoToConvert.Id == "" ? Guid.NewGuid().ToString() : dtoToConvert.Id, 
			ReleaseYear = dtoToConvert.ReleaseYear,
			Title = dtoToConvert.Title
		};
	}

	private async Task<List<Genre>> FindGenres(List<string> genreStrings)
	{
		var listOfGenres = new List<Genre>();

		foreach (var genre in genreStrings)
		{
			var search =
				await _unitOfWork.GenreRepository.FindByKey(g => g.Name.ToLower().Equals(genre.ToLower())); //TODO: mer check på detta. Kapa tomutrymme etc.
			//Eller ge förslag på när användaren skriver i de ord som är liknande.

			if (search.Count() > 0)
			{
				listOfGenres.AddRange(search);
				continue;
			}

			//Här skapas en ny genre om den inte redan finns.
			listOfGenres.Add(await _unitOfWork.GenreRepository.AddItem(new Genre() { Name = genre }));
		}

		return listOfGenres;
		
	}
}