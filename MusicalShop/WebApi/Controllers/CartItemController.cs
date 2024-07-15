using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MusicalShop.Models;

namespace MusicalShop.Controllers
{
    
    [ApiController]
    [Route("/api/[controller]")]
    public class CartItemController : Controller
    {
        private readonly ApplicationDbContext _context;

        public CartItemController(ApplicationDbContext context) => _context = context;

        [HttpGet("GetCartItems")]
        public ActionResult<IEnumerable<CartItem>> GetCartItems()
        {
            var cartItems = _context.CartItems.Include(s => s.Cart).Include(s => s.MusicalInstruments).ToList();
            return Ok(cartItems);
        }

        [HttpPost("AddCartItem")]
        public IActionResult AddCartItem(long cartId, long musicalInstrumentId, int quantity)
        {
            CartItem cartItem = new CartItem();
            cartItem.CartId = cartId;
            cartItem.MusicalInstrumentId = musicalInstrumentId;
            cartItem.Quantity = quantity;

            

            var musicalInstrument = _context.MusicalInstruments.Find(musicalInstrumentId);

            if (musicalInstrument.Quantity < quantity) return BadRequest("Обуви не достаточно");

            musicalInstrument.Quantity -= quantity;
            _context.CartItems.Add(cartItem);
            _context.SaveChanges();

            return Ok("Обувь успешно добавлена в корзину");
        }

        [HttpDelete("DeleteCartItem")]
        public IActionResult DeleteCartItem(long id)
        {
            var cartItem = _context.CartItems.Include(ci => ci.MusicalInstruments).FirstOrDefault(ci => ci.Id == id);

            if (cartItem == null) return BadRequest("Нет записи в корзине с таким id");

            var musicalInstrument = cartItem.MusicalInstruments;
            musicalInstrument.Quantity += cartItem.Quantity;

            _context.CartItems.Remove(cartItem);
            _context.SaveChanges();

            return Ok($"Элемент корзины успешно удален. Возвращено {cartItem.Quantity} обуви.");
        }

        [HttpPut("UpdateCartItem")]
        public IActionResult UpdateCartItem(long cartItemId, long? cartId = null, long? musicalInstrumentId = null, int? quantity = null)
        {
            var findCartItem = _context.CartItems.Include(ci => ci.MusicalInstruments).FirstOrDefault(ci => ci.Id == cartItemId);

            if (findCartItem == null) return BadRequest("Запись в корзине не найдена");

            if (quantity != null)
            {
                var musicalInstrument = findCartItem.MusicalInstruments;
                int oldQuantity = findCartItem.Quantity;
                int newQuantity = quantity.Value;
                int quantityDifference = newQuantity - oldQuantity;

                if (quantityDifference > 0 && musicalInstrument.Quantity < quantityDifference)
                {
                    return BadRequest("Обуви недостаточно на складе для увеличения количества");
                }

                musicalInstrument.Quantity -= quantityDifference;
                findCartItem.Quantity = newQuantity;
            }

            if (musicalInstrumentId != null && findCartItem.MusicalInstrumentId != musicalInstrumentId.Value)
            {
                var oldMusicalInstrument = findCartItem.MusicalInstruments;
                var newMusicalInstrument = _context.MusicalInstruments.Find(musicalInstrumentId.Value);

                if (newMusicalInstrument == null) return BadRequest("Новая обувь не найдена");

                oldMusicalInstrument.Quantity += findCartItem.Quantity;

                if (newMusicalInstrument.Quantity < findCartItem.Quantity)
                {
                    return BadRequest("Новой обуви недостаточно на складе");
                }

                newMusicalInstrument.Quantity -= findCartItem.Quantity;
                findCartItem.MusicalInstrumentId = musicalInstrumentId.Value;
                findCartItem.MusicalInstruments = newMusicalInstrument;
            }

            if (cartId != null) findCartItem.CartId = cartId.Value;

            _context.CartItems.Update(findCartItem);
            _context.SaveChanges();

            return Ok($"Объект корзины с id: {cartItemId} успешно обновлен");
        }
    }
}
