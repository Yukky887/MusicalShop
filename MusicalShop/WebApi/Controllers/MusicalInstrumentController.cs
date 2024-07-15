using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MusicalShop.Models;
using System.Linq;

namespace MusicalShop.Controllers
{
    
    [ApiController]
    [Route("/api/[controller]")]
    public class MusicalInstrumentController : Controller
    {
        private readonly ApplicationDbContext _context;

        public MusicalInstrumentController(ApplicationDbContext context) => _context = context;

        [HttpGet("GetMusicalInstrument")]
        public ActionResult<IEnumerable<MusicalInstrument>> GetMusicalInstrument()
        {
            var musicalInstrument = _context.MusicalInstruments.Include(s => s.Brand).Include(s => s.Category).ToList();
            return Ok(musicalInstrument);
        }

        [HttpPost("AddMusicalInstrument")]
        public IActionResult AddMusicalInstrument(string name, int price, string color, int quantity, long brandId, long categoryId)
        {
            MusicalInstrument musicalInstrument = new MusicalInstrument();
            musicalInstrument.Name = name;
            musicalInstrument.Price = price;
            musicalInstrument.Color = color;
            musicalInstrument.Quantity = quantity;
            musicalInstrument.BrandId = brandId;
            musicalInstrument.CategoryId = categoryId;

            var existingMusicalInstrument = _context.MusicalInstruments.FirstOrDefault(s => s.Name == musicalInstrument.Name 
                                                            && s.Price == musicalInstrument.Price
                                                            && s.Color == musicalInstrument.Color
                                                            && s.Quantity == musicalInstrument.Quantity);

            if (existingMusicalInstrument != null) 
            {
                return BadRequest("Такой товар уже существует");
            }

            _context.MusicalInstruments.Add(musicalInstrument);
            _context.SaveChanges();

            return Ok("Товар успешно добавлен");
        }

        [HttpDelete("DeleteMusicalInstrument")]
        public IActionResult DeleteMusicalInstrument(long id)
        {
            var musicalInstrument = _context.MusicalInstruments.Find(id);

            if (musicalInstrument == null)
            {
                return BadRequest("Нет товара с таким id");
            }

            _context.MusicalInstruments.Remove(musicalInstrument);
            _context.SaveChanges();

            return Ok($"Товар c id: {id} успешно удален");
        }

        [HttpPut("UpdateMusicalInstrument")]
        public IActionResult UpdateMusicalInstrument(long musicalInstrumentId, string? name = null, int? price = null, string color = null, int? quantity = null, 
            long? brandId = null, long? categoryId = null)
        {
            var findMusicalInstrument = _context.MusicalInstruments.Find(musicalInstrumentId);

            if (findMusicalInstrument == null)
            {
                return BadRequest("Товар не найден");
            }
            if (name != null)
            {
                findMusicalInstrument.Name = name;
            }
            if (price != null)
            {
                findMusicalInstrument.Price = price.Value;
            }
            if (color != null)
            {
                findMusicalInstrument.Color = color;
            }
            if (quantity != null)
            {
                findMusicalInstrument.Quantity = quantity.Value;
            }
            if (brandId != null)
            {
                findMusicalInstrument.BrandId = brandId.Value;
            }
            if (categoryId != null)
            {
                findMusicalInstrument.CategoryId = categoryId.Value;
            }

            _context.MusicalInstruments.Update(findMusicalInstrument);
            _context.SaveChanges();

            return Ok($"Товар с id: {musicalInstrumentId} успешно обновлен");
        }
    }
}
