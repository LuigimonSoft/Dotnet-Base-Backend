using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Dotnet_Base_Backend.Models;

namespace Dotnet_Base_Backend.Repositories.Context
{
    public class MessagesDbContext : DbContext
    {
        public MessagesDbContext() { }
        public MessagesDbContext(DbContextOptions<MessagesDbContext> options) : base(options) { }
        public virtual DbSet<Message> Messages { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Message>().HasKey(x => x.Id);
            modelBuilder.Entity<Message>().Property(x => x.Id).ValueGeneratedOnAdd();
            modelBuilder.Entity<Message>().Property(x => x.Content).IsRequired();
            modelBuilder.Entity<Message>().HasData();
        }
    }
}
