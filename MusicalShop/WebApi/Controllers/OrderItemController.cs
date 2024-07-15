using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MusicalShop.Models;

namespace MusicalShop.Controllers
{
    [ApiController]
    [Route("/api/[controller]")]
    public class OrderItemController : Controller
    {
        private readonly ApplicationDbContext _context;

        public OrderItemController(ApplicationDbContext context) => _context = context;

        [HttpGet("GetOrderItems")]
        public ActionResult<IEnumerable<OrderItem>> GetOrderItems()
        {
            var orderItems = _context.OrderItems.Include(s => s.Order).Include(s => s.MusicalInstrument).ToList();
            return Ok(orderItems);
        }

        [HttpPost("AddOrderItem")]
        public IActionResult AddOrderItem(long orderId, long musicalInstrumentId, int quantity, int price, bool isPickedUp)
        {
            OrderItem orderItem = new OrderItem();
            orderItem.OrderId = orderId;
            orderItem.MusicalInstrumentId = musicalInstrumentId;
            orderItem.Quantity = quantity;
            orderItem.Price = price;
            orderItem.IsPickedUp = isPickedUp;

            var existingOrderItem = _context.OrderItems.FirstOrDefault(s => s.OrderId == orderItem.OrderId
                                                                      && s.MusicalInstrumentId == orderItem.MusicalInstrumentId
                                                                      && s.Quantity == orderItem.Quantity
                                                                      && s.Price == orderItem.Price
                                                                      && s.IsPickedUp == orderItem.IsPickedUp);

            if (existingOrderItem != null)
            {
                return BadRequest("Такой заказ уже существует");
            }

            _context.OrderItems.Add(orderItem);
            _context.SaveChanges();

            return Ok("Заказ успешно добавлен");
        }

        [HttpDelete("DeleteOrderItem")]
        public IActionResult DeleteOrderItem(long id)
        {
            var orderItem = _context.OrderItems.Find(id);

            if (orderItem == null)
            {
                return BadRequest("Нет заказа с таким id");
            }

            _context.OrderItems.Remove(orderItem);
            _context.SaveChanges();

            return Ok($"Заказ c id: {id} успешно удален");
        }

        [HttpPut("UpdateOrderItem")]
        public IActionResult UpdateOrderItem(long orderItemId, long? orderId = null, long? musicalInstrumentId = null, int? qunatity = null, int? price = null,
            bool? isPickedUp = null)
        {
            var findOrderItem = _context.OrderItems.Find(orderId);

            if (findOrderItem == null)
            {
                return BadRequest("Заказ не найден");
            }

            if (orderId != null)
            {
                findOrderItem.OrderId = orderId.Value;
            }
            if (price != null)
            {
                findOrderItem.Price = price.Value;
            }
            if (musicalInstrumentId != null)
            {
                findOrderItem.MusicalInstrumentId = musicalInstrumentId.Value;
            }
            if (qunatity != null)
            {
                findOrderItem.Quantity = qunatity.Value;
            }
            if (isPickedUp != null)
            {
                findOrderItem.IsPickedUp = isPickedUp.Value;
            }

            _context.OrderItems.Update(findOrderItem);
            _context.SaveChanges();

            return Ok($"Заказ с id: {orderId} успешно обновлен");
        }
    }
}
