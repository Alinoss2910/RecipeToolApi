using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace ApiUsersRecipeTool.Models
{
    public class BuyList
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required]
        public string Name { get; set; }

        [ForeignKey("UserId")]
        public User User { get; set; }

        public List<Ingredient> Ingredients { get; set; } = new List<Ingredient>();
    }
}

