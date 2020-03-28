using System.Collections.Generic;

namespace Domain.Product.DataContract
{
    public class Product
    {
        public int Sku { get; set; }

        public string Websku { get; set; }

        public string Description { get; set; }

        public ProductCost Cost { get; set; }

        public ProductQuantity Quantity { get; set; }
    }
}
