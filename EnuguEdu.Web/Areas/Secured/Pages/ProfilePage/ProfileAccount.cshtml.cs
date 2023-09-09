using EnuguEdu.Domain.Data.Migrations;
using EnuguEdu.Domain.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace EnuguEdu.Web.Areas.Secured.Pages.ProfilePage
{
    [Microsoft.AspNetCore.Authorization.Authorize(Roles = "mSuperAdmin,Admin")]

    public class ProfileAccountModel : PageModel
    {
        private readonly UserManager<Profile> _userManager;
         private readonly ILogger<ProfileAccountModel> _logger;

        public ProfileAccountModel( 
            ILogger<ProfileAccountModel> logger,
            UserManager<Profile> userManager)
        {
            _userManager = userManager;
             _logger = logger;
        }

        [BindProperty]
        public Profile Profile { get; set; }

      
        public async Task<IActionResult> OnGetAsync(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var profile = await _userManager.FindByIdAsync(id);
            if (profile == null)
            {
                return NotFound();
            }
            Profile = profile;
            return Page();
        }

     }
}
