using ApiUsersRecipeTool.Data;
using ApiUsersRecipeTool.DTO;
using ApiUsersRecipeTool.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace ApiUsersRecipeTool.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class UserController : ControllerBase
    {
        private readonly DataContext _context;
        private readonly UserService _userService;
        private readonly TokenService _tokenService;

        public UserController(DataContext context, UserService userService, TokenService tokenService)
        {
            _context = context;
            _userService = userService;
            _tokenService = tokenService;
        }

        [HttpPost("Register")]
        public async Task<IActionResult> Register([FromBody] AuthDTO dto)
        {
            var user = new User
            {
                Username = dto.Username,
                Password = _userService.HashPassword(dto.Password)
            };

            await _context.Users.AddAsync(user);
            await _context.SaveChangesAsync();

            return Ok("Usuario registrado correctamente");
        }

        [HttpPost("Login")]
        public async Task<IActionResult> Login([FromBody] AuthDTO dto)
        {
            var user = await _context.Users.FirstOrDefaultAsync(u => u.Username == dto.Username);

            if (user == null || !_userService.VerifyPassword(dto.Password, user.Password))
            {
                return Unauthorized();
            }

            var token = _tokenService.GenerateToken(user);
            return Ok(new { Token = token });
        }

        [HttpGet("GetUser")]
        [Authorize]
        public async Task<IActionResult> GetUser()
        {
            int userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));

            if (userId == 0)
            {
                return BadRequest("LogIn first!");
            }

            var query = await _context.Users.FirstOrDefaultAsync(u => u.Id == userId);

            if (query == null)
            {
                return NotFound("User not found");
            }

            var user = new
            {
                Id = query.Id,
                Username = query.Username
            };


            return Ok(user);
        }

        [HttpPut("EditUser")]
        [Authorize]
        public async Task<IActionResult> EditUser([FromBody] AuthDTO dto)
        {
            int userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));

            if (userId == 0)
            {
                return BadRequest("LogIn first!");
            }

            var user = await _context.Users.FirstOrDefaultAsync(u => u.Id == userId);

            if (user == null)
            {
                return NotFound("User not found");
            }

            user.Username = dto.Username != "" ? dto.Username : user.Username;
            user.Password = dto.Password != "" ? _userService.HashPassword(dto.Password) : user.Password;

            await _context.SaveChangesAsync();

            return Ok("Usuario editado con exito");
        }

        [HttpGet("FavoriteRecipes")]
        [Authorize]
        public async Task<IActionResult> FavoriteRecipes()
        {
            int userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));

            var recipes = await _context.Recipes
                .Where(r => r.User.Id == userId)
                .Include(r => r.User)
                .Select(r => new
                {
                    r.Id,
                    r.Url,
                    User = new
                    {
                        r.User.Id,
                        r.User.Username
                    }
                })
                .ToListAsync();
                

            return Ok(recipes);
        }

        [HttpPost("AddFavoriteRecipe")]
        [Authorize]
        public async Task<IActionResult> AddFavoriteRecipe([FromBody] RecipeDTO dto)
        {

            int userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));

            if (userId == 0)
            {
                return BadRequest("Seleccione un Usuario Valido");
            }

            var url = await _context.Recipes.FirstOrDefaultAsync(r => r.Url == dto.Url && r.User.Id == userId);

            if (url != null)
            {
                await DeleteFavoriteRecipe(url.Id);
                return Ok("Receta eliminada de favoritos");
            }

            var recipe = new Recipe
            {
                Url = dto.Url,
                User = await _context.Users.FirstOrDefaultAsync(u => u.Id == userId)
            };

            await _context.Recipes.AddAsync(recipe);
            await _context.SaveChangesAsync();

            return Ok("Receta guardada con exito");
        }

        [HttpGet("GetFavoriteRecipe/{id}")]
        public async Task<IActionResult> GetFavoriteRecipe(int id)
        {
            var recipe = await _context.Recipes.FirstOrDefaultAsync(r => r.Id == id);

            if (recipe == null)
            {
                return NotFound();
            }

            return Ok(recipe);
        }

        [HttpDelete("DeleteFavoriteRecipe/{id}")]
        [Authorize]
        public async Task<IActionResult> DeleteFavoriteRecipe(int id)
        {
            var recipe = await _context.Recipes.FirstOrDefaultAsync(r => r.Id == id);

            if (recipe == null)
            {
                return NotFound();
            }

            _context.Recipes.Remove(recipe);
            await _context.SaveChangesAsync();

            return Ok("Receta eliminada con exito");
        }

        [HttpPost("CreateBuyList")]
        [Authorize]
        public async Task<IActionResult> CreateBuyList([FromBody] BuyListDTO dto)
        {

            int userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));

            if (userId == 0)
            {
                return BadRequest("LogIn first!");
            }

            var user = await _context.Users.FirstOrDefaultAsync(u => u.Id == userId);

            var buyList = new BuyList();
            buyList.Name = dto.Name;
            buyList.User = user;

            if ( dto.Ingredients == null || dto.Ingredients.Count == 0)
            {
                await _context.BuyLists.AddAsync(buyList);
                await _context.SaveChangesAsync();
                return Ok("Lista creada con exito sin ingredientes");
            }

            foreach (var ingredient in dto.Ingredients)
            {
                buyList.Ingredients.Add(new Ingredient 
                {
                    Name = ingredient,
                    BuyList = buyList
                });
            }

            await _context.BuyLists.AddAsync(buyList);
            await _context.SaveChangesAsync();

            return Ok("Lista de compra creada con exito");
        }

        [HttpGet("GetBuyLists")]
        public async Task<IActionResult> GetBuyList()
        {
            int userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));

            if (userId == 0)
            {
                return BadRequest("LogIn first!");
            }

            var buyList = await _context.BuyLists
                .Include(bl => bl.User)
                .Where(bl => bl.User.Id == userId)
                .Select(bl => new
                {
                    bl.Id,
                    bl.Name,
                    Ingredients = _context.Ingredients
                        .Where(i => i.BuyList.Id == bl.Id)
                        .Select(i => new
                        {
                            i.Id,
                            i.Name
                        })
                        .ToList()
                    
                })
                .ToListAsync();

            if (buyList == null)
            {
                return NotFound();
            }

            return Ok(buyList);
        }

        [HttpDelete("DeleteBuyList/{id}")]
        [Authorize]
        public async Task<IActionResult> DeleteBuyList(int id)
        {
            var buyList = await _context.BuyLists.FirstOrDefaultAsync(b => b.Id == id);

            if (buyList == null)
            {
                return NotFound();
            }

            _context.BuyLists.Remove(buyList);
            await _context.SaveChangesAsync();

            return Ok("Lista de compra eliminada con exito");
        }

        [HttpPost("AddIngredient")]
        [Authorize]
        public async Task<IActionResult> AddIngredient([FromBody] IngredientDTO dto)
        {
            var buyList = await _context.BuyLists.FirstOrDefaultAsync(b => b.Id == dto.BuyListId);

            if (buyList == null)
            {
                return NotFound("Lista de compra no encontrada");
            }

            var ingredient = new Ingredient
            {
                Name = dto.Name,
                BuyList = buyList
            };

            await _context.Ingredients.AddAsync(ingredient);
            await _context.SaveChangesAsync();

            return Ok("Ingrediente añadido con exito");
        }

        [HttpDelete("RemoveIngredient")]
        [Authorize]
        public async Task<IActionResult> RemoveIngredient(int idList, int idIngredient)
        {
            var ingredient = await _context.Ingredients
                .Include(idList => idList.BuyList)
                .FirstOrDefaultAsync(i => i.Id == idIngredient && i.BuyList.Id == idList);

            if (ingredient == null)
            {
                return NotFound("Ingrediente no encontrado");
            }

            _context.Ingredients.Remove(ingredient);
            await _context.SaveChangesAsync();

            return Ok("Ingrediente eliminado con exito");
        }
    }
}
