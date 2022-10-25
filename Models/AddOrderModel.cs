using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace WebApplication1.Models
{
    public class AddOrderModel
    {
        [Required]
        public double TotalPrice { get; set; }
        [Required]
        public DateTime OrderedDate { get; set; }
        [Required]
        public string UserName { get; set; }
        [Required]
        public int AddressID { get; set; }
        [Required]
        public int StatusId { get; set; }

        public ProductDetails[] ProductDetails  { get; set; }


    }
    public class ProductDetails
    {
        public int Quantity { get; set; }

        public int ProductId { get; set; }
    }
}

