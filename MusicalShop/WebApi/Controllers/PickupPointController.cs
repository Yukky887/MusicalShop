﻿using Microsoft.AspNetCore.Mvc;
using MusicalShop.Models;

namespace MusicalShop.Controllers
{
    
    [ApiController]
    [Route("/api/[controller]")]
    public class PickupPointController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public PickupPointController(ApplicationDbContext context) => _context = context;

        [HttpGet("GetPickupPoints")]
        public ActionResult<IEnumerable<PickupPoint>> GetPickupPoints()
        {
            var pickupPoints = _context.PickupPoints.ToList();
            return Ok(pickupPoints);
        }

        [HttpPost("AddPickupPoint")]
        public IActionResult AddPickupPoint(string city, string address)
        {
            PickupPoint pickup = new PickupPoint();
            pickup.City = city;
            pickup.Address = address;

            var existingPickup = _context.PickupPoints.FirstOrDefault(b => b.City == city && b.Address == address);

            if (existingPickup != null) return BadRequest("Такой пункт выдачи уже существует");

            _context.PickupPoints.Add(pickup);
            _context.SaveChanges();

            return Ok("Пункт выдачи успешно добавлен");
        }

        [HttpDelete("DeletePickupPoint")]
        public IActionResult DeletePickupPoint(long id)
        {
            var pickup = _context.PickupPoints.Find(id);

            if (pickup == null)
            {
                return BadRequest("Нет пункта выдачи с таким id");
            }

            _context.PickupPoints.Remove(pickup);
            _context.SaveChanges();

            return Ok($"Пункт выдачи c id: {id} успешно создан");
        }

        [HttpPut("UpdateDeletePickupPoint")]
        public IActionResult UpdateDeletePickupPoint(long pickupId, string? city = null, string? address = null)
        {
            var findPickup = _context.PickupPoints.Find(pickupId);

            if (findPickup == null) return NotFound("Пункт выдачи не найден");
            if (city != null) findPickup.City = city;
            if (address != null) findPickup.Address = address;
            

            _context.PickupPoints.Update(findPickup);
            _context.SaveChanges();

            return Ok($"Пункт выдачи с id: {pickupId} обновлен ");
        }
    }
}
