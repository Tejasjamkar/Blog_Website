using Bloggie.Models.Domain;
using Microsoft.EntityFrameworkCore;

namespace Bloggie.Data
{
    public class Bloggiedbcontext : DbContext
    {
        public Bloggiedbcontext(DbContextOptions options) : base(options)
        {

        }

        public DbSet<Blogpost> blogposts { get; set; }
        public DbSet<Tag> Tags { get; set; } 
    }
}
