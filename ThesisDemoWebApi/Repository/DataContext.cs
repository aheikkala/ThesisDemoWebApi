using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using ThesisDemoWebApi.Models;

namespace ThesisDemoWebApi.Repository
{
    public class DataContext : DbContext
    {

        //public DataContext() : base(@"Data Source=DESKTOP-542OICS\SQLEXPRESS;Initial Catalog=ThesisDemoDb;Integrated Security=True") { }
        public DataContext() : base(@"Data Source=PII-PARTJUH\SQLEXPRESS;Initial Catalog=ThesisDemoDb;Integrated Security=True") {}

        //Entity set:
        public DbSet<Message> Messages { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<Group> Groups { get; set; }

        //Model:
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<User>().HasMany(x => x.Groups).WithMany(g => g.Users);
            //modelBuilder.Entity<Group>().HasMany(x => x.Users).WithMany();
            modelBuilder.Entity<Group>().HasMany(x => x.Messages).WithMany();
            //modelBuilder.Entity<Group>().HasMany(x => x.Messages).WithMany();
            modelBuilder.Entity<Message>().HasRequired(x => x.User).WithMany();
        }
    }
}