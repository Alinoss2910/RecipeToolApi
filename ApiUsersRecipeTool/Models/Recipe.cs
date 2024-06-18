using System.ComponentModel.DataAnnotations.Schema;

namespace ApiUsersRecipeTool.Models
{
    public class Recipe
    {
        public int Id { get; set; }
        public string Url { get; set; }

        [ForeignKey("UserId")]
        public User User { get; set; }
    }
}
