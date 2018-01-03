using System.Collections.Generic;
using System.Linq;

namespace SportsStore.Models
{
    public class Cart
    {
        private readonly List<Line> _lines = new List<Line>();

        public virtual void AddItem(Product product, int quantity)
        {
            var cartLine = _lines.FirstOrDefault(p => p.Product.ProductID == product.ProductID);
            if (cartLine == null)
            {
                _lines.Add(new Line {Product = product, Quantity = quantity});
            }
            else
            {
                cartLine.Quantity += quantity;
            }
        }

        public virtual void RemoveLine(Product product)
            => _lines.RemoveAll(l => l.Product.ProductID == product.ProductID);

        public virtual decimal ComputeTotalValue()
            => _lines.Sum(e => e.Product.Price * e.Quantity);

        public virtual void Clear() => _lines.Clear();

        public virtual IEnumerable<Line> Lines => _lines;

        public class Line
        {
            public int ID { get; set; }
            public Product Product { get; set; }
            public int Quantity { get; set; }
        }
    }
}
