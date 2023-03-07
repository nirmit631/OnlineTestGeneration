using Microsoft.EntityFrameworkCore;
using OnlineTest.Model;
using OnlineTest.Models;
using System.Collections.Generic;
using System.Data;

namespace OnlineTest.Data
{
    public class OnlineTestContext : DbContext
    {
        public OnlineTestContext(DbContextOptions<OnlineTestContext> options) : base(options)
        { }
        public DbSet<User> Users { get; set; }
        public DbSet<Role> Roles { get; set; }
        public DbSet<UserRole> UserRoles { get; set; }
        public DbSet<RToken> RToken { get; set; }
    }
}
