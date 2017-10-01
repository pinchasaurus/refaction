using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace Refaction.Service.Models
{
    public class Product
    {
        public Product()
        {
        }

        public Guid Id { get; set; }

        [Required]
        [StringLength(100)]
        public string Name { get; set; }

        [StringLength(500)]
        public string Description { get; set; }

        public decimal Price { get; set; }

        public decimal DeliveryPrice { get; set; }

        // Copy constructor
        public Product(Product other)
        {
            Id = other.Id;
            DeliveryPrice = other.DeliveryPrice;
            Description = other.Description;
            Name = other.Name;
            Price = other.Price;
        }
    }

}
