using System;
using System.Threading.Tasks;
using AutoMapper;
using HomeApi.Contracts.Models.Devices;
using HomeApi.Contracts.Models.Rooms;
using HomeApi.Data.Models;
using HomeApi.Data.Queries;
using HomeApi.Data.Repos;
using Microsoft.AspNetCore.Mvc;

namespace HomeApi.Controllers
{
    /// <summary>
    /// Контроллер комнат
    /// </summary>
    [ApiController]
    [Route("[controller]")]
    public class RoomsController : ControllerBase
    {
        private IRoomRepository _repository;
        private IMapper _mapper;
        
        public RoomsController(IRoomRepository repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        /// <summary>
        /// Получение всех существующих комнат
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route("")]
        public async Task <IActionResult> GetRooms()
        {
            var rooms = await _repository.GetRooms();

            var result = new GetRoomsResponse
            {
                RoomAmount = rooms.Length,
                Rooms = _mapper.Map<Room[], RoomView[]>(rooms)
            };

            return StatusCode(200, result);
        }
        
        
        /// <summary>
        /// Добавление комнаты
        /// </summary>
        [HttpPost] 
        [Route("")] 
        public async Task<IActionResult> Add([FromBody] AddRoomRequest request)
        {
            var existingRoom = await _repository.GetRoomByName(request.Name);
            if (existingRoom == null)
            {
                var newRoom = _mapper.Map<AddRoomRequest, Room>(request);
                await _repository.AddRoom(newRoom);
                return StatusCode(201, $"Комната {request.Name} добавлена!");
            }
            
            return StatusCode(409, $"Ошибка: Комната {request.Name} уже существует.");
        }

        [HttpPut]
        [Route("{id}")]
        public async Task<IActionResult> Edit(
            [FromRoute] Guid id,
            [FromBody] EditRoomRequest request)
        {
            var room = await _repository.GetRoomById(id);
            
            if (room == null)
                return StatusCode(400, $"Ошибка: Комната {id} не подключена. Сначала подключите комнату!");

            var newRoom = await _repository.GetRoomByName(request.NewName);
            if (newRoom != null)
                return StatusCode(400, $"Ошибка: Комната с именем {request.NewName} уже существует. Выберите другое имя!");

            _repository.UpdateRoom(room, new UpdateRoomQuery(request.NewName, request.NewArea, request.NewGasConnected, request.NewVoltage));

            return StatusCode(200, $"Комната обновлена! Имя - {room.Name}");

        }
    }
}