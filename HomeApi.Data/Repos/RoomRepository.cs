using System;
using System.Linq;
using System.Threading.Tasks;
using HomeApi.Data.Models;
using HomeApi.Data.Queries;
using Microsoft.EntityFrameworkCore;

namespace HomeApi.Data.Repos
{
    /// <summary>
    /// Репозиторий для операций с объектами типа "Room" в базе
    /// </summary>
    public class RoomRepository : IRoomRepository
    {
        private readonly HomeApiContext _context;
        
        public RoomRepository (HomeApiContext context)
        {
            _context = context;
        }
        
        /// <summary>
        ///  Найти комнату по имени
        /// </summary>
        public async Task<Room> GetRoomByName(string name)
        {
            return await _context.Rooms.Where(r => r.Name == name).FirstOrDefaultAsync();
        }
        
        /// <summary>
        ///  Добавить новую комнату
        /// </summary>
        public async Task AddRoom(Room room)
        {
            var entry = _context.Entry(room);
            if (entry.State == EntityState.Detached)
                await _context.Rooms.AddAsync(room);
            
            await _context.SaveChangesAsync();
        }

        /// <summary>
        /// Получить все комнаты
        /// </summary>
        /// <returns></returns>
        public async Task<Room[]> GetRooms()
        {
            return await _context.Rooms.ToArrayAsync();
        }

        /// <summary>
        /// Получить комнату по Id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<Room> GetRoomById(Guid id)
        {
            return await _context.Rooms
                .Where(r => r.Id == id)
                .FirstOrDefaultAsync();
        }

        public async Task UpdateRoom(Room room, UpdateRoomQuery query)
        {
            if (!string.IsNullOrEmpty(query.NewName))
            {
                room.Name = query.NewName;
            }

            if (!(string.IsNullOrEmpty(query.NewArea) & int.TryParse(query.NewArea, out var number)))
            {
                room.Area = number;
            }

            if (!string.IsNullOrEmpty(query.NewGasConnected) & bool.TryParse(query.NewGasConnected, out var result))
            {
                room.GasConnected = result;
            }

            if (!string.IsNullOrEmpty(query.NewVoltage) & int.TryParse(query.NewVoltage, out var volt))
            {
                room.Voltage = volt;
            }

            // Добавляем в базу 
            var entry = _context.Entry(room);
            if (entry.State == EntityState.Detached)
                _context.Rooms.Update(room);

            // Сохраняем изменения в базе 
            await _context.SaveChangesAsync();
        }
    }
}