using ClassMate.Data.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClassMate.Data
{
    public class ClassMateDbContext : DbContext
    {
        public DbSet<User> Users { get; set; }
        public DbSet<Class> Classes { get; set; }
        public DbSet<Note> Notes { get; set; }
        public DbSet<ToDo> ToDos { get; set; }
        public DbSet<Tag> Tags { get; set; }
        public DbSet<UsersClasses> UsersClasses { get; set; }
        public DbSet<UsersNotes> UsersNotes { get; set; }
        public DbSet<UsersToDos> UsersToDos { get; set; }
        public DbSet<OAuthToken> OAuthTokens { get; set; }
        public DbSet<CalendarEvent> CalendarEvents { get; set; }

        public ClassMateDbContext(DbContextOptions<ClassMateDbContext> options) : base(options) { }

    }
}
