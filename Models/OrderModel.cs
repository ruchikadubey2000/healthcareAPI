using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace WebApplication1.Models
{
    public class OrderModel
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public double TotalPrice { get; set; }
        [Required]
        public DateTime OrderedDate { get; set; }
        [Required]
        [ForeignKey("User")]
        public int UserId { get; set; }
        public virtual UserModel User { get; set; }
        [Required]
        [ForeignKey("Address")]
        public int AddressID { get; set; }
        public virtual AddressModel Address { get; set; }
        [Required]
        [ForeignKey("Status")]
        public int StatusId { get; set; }
        public virtual StatusModel Status { get; set; }


    }
}

