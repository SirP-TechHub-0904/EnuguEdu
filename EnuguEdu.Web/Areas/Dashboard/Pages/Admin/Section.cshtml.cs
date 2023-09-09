using EnuguEdu.Domain.Dtos;
using EnuguEdu.Domain.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace EnuguEdu.Web.Areas.Dashboard.Pages.Admin
{
      [Microsoft.AspNetCore.Authorization.Authorize]

    public class SectionModel : PageModel
    {
        private readonly EnuguEdu.Domain.Data.ApplicationDbContext _context;

        public SectionModel(EnuguEdu.Domain.Data.ApplicationDbContext context)
        {
            _context = context;
        }

        public IList<CategoryDto> Category { get; set; } = default!;
        public Category1 Category1 { get; set; } = default!;


        public async Task<IActionResult> OnGetAsync(long id)
        {
            if (id == null)
            {
                return NotFound();
            }

            Category1 = await _context.Category1s.FirstOrDefaultAsync(m => m.Id == id);

            
            if (Category1 == null)
            {
                return RedirectToPage("/");
            }

            var Category2s = _context.Category2s
                 .Include(x => x.Appointments)
                 .ThenInclude(x => x.Profile)
                 .Include(x => x.Category3s)
                 .Where(x => x.Category1Id == Category1.Id)
                 .AsQueryable();
            //


            var output = Category2s
                .Select(d => new CategoryDto
                {
                    Id = d.Id,
                    Title = d.Title,
                    Fullname = d.Appointments.AsQueryable().OrderBy(x => x.Order).FirstOrDefault().Profile.Fullname ?? "",
                    Color = d.Color
                });
            Category = await output.ToListAsync();

            return Page();
        }
    }
}
