using System.Linq;
using Microsoft.AspNetCore.Mvc;
using SportsStore.Data;
using SportsStore.Models;

namespace SportsStore.Controllers
{
    public class OrderController : Controller
    {
        private readonly IOrderRepository _repository;
        private readonly Cart _cart;

        public OrderController(IOrderRepository repository, Cart cart)
        {
            _repository = repository;
            _cart       = cart;
        }

        public ViewResult List() => View(_repository.Orders.Where(o => !o.IsShipped));

        [HttpPost]
        public IActionResult MarkShipped(int orderID)
        {
            Order order = _repository.Orders.FirstOrDefault(o => o.OrderID == orderID);
            if (order != null)
            {
                order.IsShipped = true;
                _repository.SaveOrder(order);
            }
            return RedirectToAction(nameof(List));
        }

        public ViewResult Checkout() => View(new Order());

        [HttpPost]
        public IActionResult Checkout(Order order)
        {
            if (!_cart.Lines.Any())
            {
                ModelState.AddModelError("", "Sorry, your cart is empty!");
            }

            if (ModelState.IsValid)
            {
                order.Lines = _cart.Lines.ToArray();
                _repository.SaveOrder(order);
                return RedirectToAction(nameof(Completed));
            }
            else
            {
                return View(order);
            }
        }

        public ViewResult Completed()
        {
            _cart.Clear();
            return View();
        }
    }
}
