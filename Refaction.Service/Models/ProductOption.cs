using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

using Newtonsoft.Json;

namespace Refaction.Service.Models
{
    public class ProductOption
    {
        public ProductOption()
        {
        }

        public Guid Id { get; set; }

        // by design, this property should never be serialized by the json formatter
        [JsonIgnore]
        public Guid ProductId { get; set; }

        [Required]
        [StringLength(100)]
        public string Name { get; set; }

        [StringLength(500)]
        public string Description { get; set; }

        // Copy constructor
        public ProductOption(ProductOption other)
        {
            Id = other.Id;
            Description = other.Description;
            Name = other.Name;
            ProductId = other.ProductId;
        }
    }
}