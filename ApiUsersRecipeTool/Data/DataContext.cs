using ApiUsersRecipeTool.Models;
using Microsoft.EntityFrameworkCore;

namespace ApiUsersRecipeTool.Data
{
    public class DataContext : DbContext
    {
        public DataContext(DbContextOptions<DataContext> options) : base(options) { }

        public DbSet<User> Users { get; set; }
        public DbSet<Recipe> Recipes { get; set; }
        public DbSet<BuyList> BuyLists { get; set; }
        public DbSet<Ingredient> Ingredients { get; set; }
    }   
}
