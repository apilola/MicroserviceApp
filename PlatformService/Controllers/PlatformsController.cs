using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using PlatformService.Data;
using PlatformService.Dtos;
using PlatformService.Models;
using PlatformService.SyncDataService.Http;

namespace PlatformService.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PlatformsController : ControllerBase
    {
        private readonly IPlatformRepo _repository;
        private readonly IMapper _mapper;
        private readonly ICommandDataClient _commandClient;

        public PlatformsController(
            IPlatformRepo repository,
            IMapper mapper,
            ICommandDataClient commandClient)
        {
            _repository = repository;
            _mapper = mapper;
            _commandClient = commandClient;
        }

        [HttpGet]
        public ActionResult<IEnumerable<PlatformReadDto>> GetPlatforms()
        {
            Console.WriteLine("--> GettingPlatforms....");

            var platformItems = _repository.GetAllPlatforms();

            return Ok(_mapper.Map<IEnumerable<PlatformReadDto>>(platformItems));
        }

        [HttpGet("{id}", Name = nameof(GetPlatformByID))]
        public ActionResult<PlatformReadDto> GetPlatformByID(int id)
        {
            Console.WriteLine($"--> GettingPlatform for Id {id}");

            var platformItem = _repository.GetPlatformByID(id);
            if(platformItem == null)
            {
                return NotFound();
            }
            return Ok(_mapper.Map<PlatformReadDto>(platformItem));
        }

        [HttpPost]
        public async Task<ActionResult<PlatformReadDto>> CreatePlatform(PlatformCreateDto platformCreateDto)
        {            
            Console.WriteLine($"--> CreatingPlatform for with name {platformCreateDto.Name}");
            var platformModel = _mapper.Map<Platform>(platformCreateDto);
            _repository.CreatePlatform(platformModel);
            _repository.SaveChanges();

            var platformReadDto = _mapper.Map<PlatformReadDto>(platformModel);

            try
            {
                await _commandClient.SendPlatformToCommand(platformReadDto);
            }
            catch(Exception e)
            {
                Console.WriteLine($"-- Could not send syncronously: {e.Message}");
            }

            Console.WriteLine($"--> CreatingPlatform for with ID {platformReadDto.Id}");
            return CreatedAtRoute(nameof(GetPlatformByID), new {Id = platformReadDto.Id}, platformReadDto);
        }
    }
}