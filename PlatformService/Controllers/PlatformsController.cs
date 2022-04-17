using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using PlatformService.Data;
using PlatformService.Dtos;
using PlatformService.Models;
using PlatformService.SyncDataServices.Http;

namespace PlatformService.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class PlatformsController : ControllerBase
	{
		private readonly IMapper _mapper;
		private readonly IPlatformRepo _repository;
		private readonly ICommandDataClient _commandsDataClient;

		public PlatformsController(IPlatformRepo repository, IMapper mapper, ICommandDataClient commandDataClient)
		{
			_mapper = mapper;
			_repository = repository;
			_commandsDataClient = commandDataClient;
		}

		[HttpGet]
		public ActionResult<IEnumerable<PlatformReadDto>> getPlatforms()
		{
			Console.WriteLine("--> Getting Platforms...");
			var platformList = _repository.GetAllPlatforms();
			return Ok(_mapper.Map<IEnumerable<PlatformReadDto>>(platformList));
		}

		[HttpGet("{id}", Name = "GetPlatformById")]
		public ActionResult<PlatformReadDto> GetPlatformById(int id)
		{
			Console.WriteLine("--> Getting Platform:" + "id = " + id);
			var platformItem = _repository.GetPlatformById(id);

			if (platformItem != null)
			{
				return Ok(_mapper.Map<PlatformReadDto>(platformItem));
			}
			else
			{
				return NotFound();
			}
		}

		[HttpPost]
		public async Task<ActionResult<PlatformReadDto>> CreatePlatform(PlatformCreateDto platform)
		{
			var platformModel = _mapper.Map<Platform>(platform);
			_repository.CreatePlatform(platformModel);
			_repository.SaveChanges();

			var platformReadDto = _mapper.Map<PlatformReadDto>(platformModel);

			try
			{
				await _commandsDataClient.SendPlatformToCommand(platformReadDto);
			}
			catch (Exception ex)
			{
				Console.WriteLine($"--> Couldn't send synchronously {ex.Message}");
			}

			return CreatedAtRoute(nameof(GetPlatformById), new { Id = platformReadDto.Id }, platformReadDto);
		}
	}
}