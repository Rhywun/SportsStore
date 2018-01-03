using System.Linq;
using Microsoft.AspNetCore.Mvc;
using SportsStore.Data;
using SportsStore.Models.ViewModels;

namespace SportsStore.Controllers
{
    public class ProductController : Controller
    {
        public int PageSize = 4;

        private readonly IProductRepository _repository;

        public ProductController(IProductRepository repository)
        {
            _repository = repository;
        }

        // GET
        public ViewResult List(string category, int productPage = 1) =>
            View(new ProductsListViewModel
            {
                Products =
                    _repository.Products.Where(p => category == null || p.Category == category)
                               .OrderBy(p => p.ProductID)
                               .Skip((productPage - 1) * PageSize)
                               .Take(PageSize),
                PagingInfo =
                    new PagingInfo
                    {
                        CurrentPage  = productPage,
                        ItemsPerPage = PageSize,
                        TotalItems   = category == null ?
                            _repository.Products.Count() :
                            _repository.Products.Count(e => e.Category == category)
                    },
                CurrentCategory = category
            });
    }
}
