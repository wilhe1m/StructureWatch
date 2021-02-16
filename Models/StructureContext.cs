using System;
using Microsoft.EntityFrameworkCore;

namespace wilhe1m.StructureWatch.Models{

    public class StructureContext: DbContext{
        public DbSet<Structure> Structures{get;set;}
        public DbSet<Notification> Notifications{get;set;}
        public DbSet<Character> Characters{get;set;}
       
        public StructureContext(){
            this.Database.EnsureCreated(); 
        }
        public StructureContext(DbContextOptions<StructureContext> options):base(options){
	        this.Database.EnsureCreated();           
        }

        protected override void OnConfiguring(DbContextOptionsBuilder options)
            => options.UseSqlite("Data Source=StructureWatch.db");

        protected override void OnModelCreating(ModelBuilder modelBuilder){
            modelBuilder.Entity<Notification>().HasKey(n => n.Id);
            modelBuilder.Entity<Notification>().HasIndex(s => s.NotificationId) .IsUnique();

            modelBuilder.Entity<Structure>().HasKey(s => s.Id);
            modelBuilder.Entity<Structure>().HasIndex(s => s.StructureId) .IsUnique();
    
            modelBuilder.Entity<Character>().HasKey(s => s.Id);
            modelBuilder.Entity<Character>().HasIndex(s => s.CharacterID).IsUnique();
        }

    }

}