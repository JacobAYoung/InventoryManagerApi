using System;
using System.Collections.Generic;
using System.Text;

namespace Domain.Product.Business
{
    public interface IInventoryModel
    {
        public List<DataContract.Product> Products { get; set; }
    }
}
