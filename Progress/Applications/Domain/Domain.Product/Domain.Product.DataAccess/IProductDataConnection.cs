using System.Collections.Generic;

namespace Domain.Product.DataAccess
{
    public interface IProductDataConnection
    {
        List<DataContract.Product> GetProducts();

        DataContract.Product GetProductBySku(int sku);
    }
}