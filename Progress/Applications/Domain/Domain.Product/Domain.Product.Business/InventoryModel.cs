using Domain.Product.DataAccess;
using System;
using System.Collections.Generic;

namespace Domain.Product.Business
{
    public class InventoryModel : IInventoryModel
    {
        public List<DataContract.Product> Products { get; set; } = new List<DataContract.Product>();

        private IProductDataConnection ProductDataConnection { get; set; } = new ProductDataConnection();

        public InventoryModel()
        {
            GetAllProducts();
        }

        public InventoryModel(int sku)
        {
            GetProductBySku(sku);
        }

        private void GetAllProducts()
        {
            Products = ProductDataConnection.GetProducts();
        }

        private void GetProductBySku(int sku)
        {
            Products.Add(ProductDataConnection.GetProductBySku(sku));
        }
    }
}
