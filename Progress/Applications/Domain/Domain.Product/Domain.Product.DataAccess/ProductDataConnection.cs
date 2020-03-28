using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using Dapper;
using Domain.Product.DataContract;

namespace Domain.Product.DataAccess
{
    public class ProductDataConnection : IProductDataConnection
    {
        private readonly string ConnectionString = "Server=(localdb)\\localdb;Database=Product;Trusted_Connection=True;";
        
        public List<DataContract.Product> GetProducts()
        {
            List<DataContract.Product> products = new List<DataContract.Product>();
            List<ProductCost> costs = new List<ProductCost>();
            List<ProductQuantity> quantities = new List<ProductQuantity>();
            try
            {
                using (SqlConnection connection = new SqlConnection(ConnectionString))
                {
                    using (var product = connection.QueryMultiple("[dbo].[ptsp_GetAllProductInfo]", commandType: System.Data.CommandType.StoredProcedure))
                    {
                        products = product.Read<DataContract.Product>().ToList();
                        costs = product.Read<ProductCost>().ToList();
                        quantities = product.Read<ProductQuantity>().ToList();
                    }

                    List<DataContract.Product> selectedProducts = (from Product in products
                                                                   join ProductCost in costs on Product.Sku equals ProductCost.Sku
                                                                   join ProductQuantity in quantities on Product.Sku equals ProductQuantity.Sku
                                                                   select new DataContract.Product { Sku = Product.Sku, Cost = ProductCost, Quantity = ProductQuantity, Description = Product.Description, Websku = Product.Websku }).ToList();

                    products = selectedProducts;
                }
            }
            catch (System.Exception)
            {
                throw;
            }
            
            return products;
        }

        public DataContract.Product GetProductBySku(int sku)
        {
            DataContract.Product product = new DataContract.Product();
            ProductCost cost = new ProductCost();
            ProductQuantity quantity = new ProductQuantity();
            try
            {
                using (SqlConnection connection = new SqlConnection(ConnectionString))
                {
                    using (var query = connection.QueryMultiple("[dbo].[ptsp_GetAllProductInfoBySku]", new { Sku = sku }, commandType: System.Data.CommandType.StoredProcedure))
                    {
                        product = query.ReadFirstOrDefault<DataContract.Product>();
                        cost = query.ReadFirstOrDefault<ProductCost>();
                        quantity = query.ReadFirstOrDefault<ProductQuantity>();
                    }

                    if (product != null)
                    {
                        product.Cost = cost;
                        product.Quantity = quantity;
                    }
                }
            }
            catch (System.Exception)
            {
                throw;
            }

            return product;
        }
    }
}
