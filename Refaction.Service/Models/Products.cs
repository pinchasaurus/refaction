using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using Newtonsoft.Json;

namespace Refaction.Service.Models
{
    public class Products
    {
        public IEnumerable<Product> Items { get; private set; }

        public Products(IEnumerable<Product> items)
        {
            Items = items;
        }
    }
}