using System;
using System.Collections.Generic;
using System.Linq;
using Domain.Product.Business;
using Domain.Product.DataContract;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace InventoryManagerApi.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class InventoryManagerController : ControllerBase
    {
        private ILogger _logger;

        public InventoryManagerController(ILogger<InventoryManagerController> logger)
        {
            _logger = logger;
        }

        [HttpGet]
        [Route("Products/{sku}")]
        public IEnumerable<Product> GetProducts(int sku)
        {
            var model = new InventoryModel(sku);
            var products = model.Products;
            _logger.LogInformation("Products");
            _logger.LogWarning("Products");
            return products;
        }
    }
}
