using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using WebApplication1.Bd_Context;
using WebApplication1.Login;
using WebApplication1.Models;

namespace WebApplication1.Pages
{
    public class IndexModel : PageModel
    {
        private readonly ILogger<IndexModel> _logger;

        [BindProperty]
        public Users user {  get; set; }

        [BindProperty]
        public string role { get; set; }

        public IndexModel(ILogger<IndexModel> logger)
        {
            _logger = logger;
        }

        public void OnGet()
        {

        }

        public IActionResult OnPost()
        {
            if (!string.IsNullOrEmpty(user.name) && !string.IsNullOrEmpty(user.password) && !string.IsNullOrEmpty(role))
            {
                var fulname = $"{role}_{user.name}";

                var connstring =
                    $"Host=localhost;Database=postgres;Username={fulname};Password=\"{user.password}\";Port=5432";

                try
                {
                    var context = new Context(connstring);
                    using var conn = context.GetConnection(); // просто проверка подключени€

                    // если дошли сюда Ч подключение успешно
                    Response.Cookies.Append("username", fulname);
                    Response.Cookies.Append("password", user.password);

                    return RedirectToPage("/Main");
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError(string.Empty, $"ќшибка подключени€: {ex.Message}");
                }
            }

            return Page();
        }

    }
}
