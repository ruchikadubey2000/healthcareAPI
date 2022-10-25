using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebApplication1.Models
{
    public class HealthcareContext : DbContext
    {
        public HealthcareContext(DbContextOptions<HealthcareContext> options) : base(options)
        {
        }

        public DbSet<UserModel> User { get; set; }
        public DbSet<StatusModel> Status { get; set; }
        public DbSet<ProductModel> Product { get; set; }
        public DbSet<ProductOrderedModel> ProductOrdered { get; set; }
        public DbSet<OrderModel> Order { get; set; }
        public DbSet<CategoryModel> Category { get; set; }
        public DbSet<AddressModel> Address { get; set; }
    }
}
