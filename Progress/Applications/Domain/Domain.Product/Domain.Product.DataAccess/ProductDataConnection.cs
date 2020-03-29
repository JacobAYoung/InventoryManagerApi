using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using Dapper;
using Domain.Log.Logger;
using Domain.Product.DataContract;
using Microsoft.Extensions.Logging;

namespace Domain.Product.DataAccess
{
    public class ProductDataConnection : IProductDataConnection
    {
        private readonly string ConnectionString = "Server=(localdb)\\localdb;Database=Product;Trusted_Connection=True;";

        private ILogger _logger = new Logger(nameof(ProductDataConnection));

        #region Get Procedures
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
            catch (Exception ex)
            {
                _logger.LogCritical(ex, ex.Message);
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
                    product.Cost = cost;
                    if (product != null)
                    {
                        product.Cost = cost;
                        product.Quantity = quantity;
                    }
                }
            }
            catch (Exception ex)
            {
                _logger.LogCritical(ex, ex.Message);
                throw;
            }

            return product;
        }
        #endregion

        #region Insert Procedures
        #endregion

        #region Update Procedures
        public bool UpdateProductQuantityBySku(DataContract.Product product)
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(ConnectionString))
                {
                    var parameters = new { Quantity = product.Quantity.Quantity, Sku = product.Sku };
                    connection.Execute("[dbo].[ptsp_UpdateProductQuantityBySku]",
                        parameters,
                        commandType: System.Data.CommandType.StoredProcedure);
                }
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogCritical(ex, ex.Message);
                return false;
            }
        }

        public bool UpdateProductCostBySku(DataContract.Product product)
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(ConnectionString))
                {
                    var parameters = new { Cost = product.Cost.Cost, Sku = product.Sku };
                    connection.Execute("[dbo].[ptsp_UpdateProductCostBySku]",
                        parameters,
                        commandType: System.Data.CommandType.StoredProcedure);
                }
            }
            catch (Exception ex)
            {
                _logger.LogCritical(ex, ex.Message);
                return false;
            }
            
            return true;
        }

        public bool UpdateProductMsrpBySku(DataContract.Product product)
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(ConnectionString))
                {
                    var parameters = new { Msrp = product.Cost.Msrp, Sku = product.Sku };
                    connection.Execute("[dbo].[ptsp_UpdateProductMsrpBySku]",
                        parameters,
                        commandType: System.Data.CommandType.StoredProcedure);
                }
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogCritical(ex, ex.Message);
                return false;
            }
        }
        #endregion
    }
}
