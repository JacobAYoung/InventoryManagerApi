using Domain.Product.DataAccess;
using System.Linq;
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

        public bool UpdateProduct(int sku)
        {
            var product = Products.SingleOrDefault(product => product.Sku == sku);
            if (product == null)
            {
                return ProductDataConnection.UpdateProductQuantityBySku(product);
            }
            return false;
        }
    }
}
