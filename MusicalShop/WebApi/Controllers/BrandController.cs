﻿using Microsoft.AspNetCore.Mvc;
using MusicalShop.Models;

namespace MusicalShop.Controllers
{
    
    [ApiController]
    [Route("/api/[controller]")]
    public class BrandController : Controller
    {
        private readonly ApplicationDbContext _context;

        public BrandController(ApplicationDbContext context) => _context = context;

        [HttpGet("GetBrands")]
        public ActionResult<IEnumerable<Brand>> GetBrands()
        {
            var brands = _context.Brands.ToList();
            return Ok(brands);
        }

        [HttpPost("AddBrand")]
        public IActionResult AddBrand(string name)
        {
            Brand brand = new Brand();
            brand.Name = name;

            var existingBrand = _context.Brands.FirstOrDefault(b => b.Name == name);

            if (existingBrand != null) return BadRequest("Такой бренд уже существует");

            _context.Brands.Add(brand);
            _context.SaveChanges();

            return Ok("Бренд успешно добавлен");
        }

        [HttpDelete("DeleteBrand")]
        public IActionResult DeleteBrand(long id) 
        {
            var brand = _context.Brands.Find(id);

            if (brand == null) 
            {
                return BadRequest("Нет бренда с таким id");
            }

            _context.Brands.Remove(brand);
            _context.SaveChanges();

            return Ok($"Бренд c id: {id} успешно удален");
        }

        [HttpPut("UpdateBrand")]
        public IActionResult UpdateBrand(long brandId, string? name = null)
        {
            var findBrand = _context.Brands.Find(brandId);

            if (findBrand == null)
            {
                return NotFound("Бренд не найден");
            }

            if (name != null)
            {
                findBrand.Name = name;
            }

            _context.Brands.Update(findBrand);
            _context.SaveChanges();

            return Ok($"Бренд с id: {brandId} обновлен ");
        }
    }
}
