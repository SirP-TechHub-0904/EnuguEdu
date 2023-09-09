using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using EnuguEdu.Domain.Data;
using EnuguEdu.Domain.Models;

namespace EnuguEdu.Web.Areas.Secured.Pages.Categories.Level3
{
    [Microsoft.AspNetCore.Authorization.Authorize(Roles = "mSuperAdmin,Admin")]
    public class CreateModel : PageModel
    {
        private readonly EnuguEdu.Domain.Data.ApplicationDbContext _context;

        public CreateModel(EnuguEdu.Domain.Data.ApplicationDbContext context)
        {
            _context = context;
        }

        public IActionResult OnGet()
        {
            return Page();
        }

        [BindProperty]
        public Category3 Category3 { get; set; } = default!;
        

        // To protect from overposting attacks, see https://aka.ms/RazorPagesCRUD
        public async Task<IActionResult> OnPostAsync()
        {
            //if (!ModelState.IsValid || _context.Category1s == null || Category3 == null)
            //  {
            //      return Page();
            //  }
            var list = new List<string> { "bg-red", "bg-yellow", "bg-aqua", "bg-blue",
                    "bg-green", "bg-navy", "bg-teal", "bg-olive", "bg-lime", "bg-orange", "bg-fuchsia", "bg-purple",
                    "bg-maroon", "bg-gray" }; var random = new Random();
            Category3.Color = list[random.Next(list.Count)];
            Category3.Order = 0;

            _context.Category3s.Add(Category3);
            await _context.SaveChangesAsync();
            TempData["success"] = "Successful";

            return RedirectToPage("./Index");
        }
    }
}
