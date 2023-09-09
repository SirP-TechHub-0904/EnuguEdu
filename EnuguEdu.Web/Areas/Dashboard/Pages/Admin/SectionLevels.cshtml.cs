using EnuguEdu.Domain.Dtos;
using EnuguEdu.Domain.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace EnuguEdu.Web.Areas.Dashboard.Pages.Admin
{
      [Microsoft.AspNetCore.Authorization.Authorize]

    public class SectionLevelsModel : PageModel
    {
        private readonly EnuguEdu.Domain.Data.ApplicationDbContext _context;

        public SectionLevelsModel(EnuguEdu.Domain.Data.ApplicationDbContext context)
        {
            _context = context;
        }

        public IList<CategoryDto> Category { get; set; } = default!;
        public CategoryDto Category3 { get; set; } = default!;


        public async Task<IActionResult> OnGetAsync(long id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var category3 = await _context.Category3s.Include(x => x.Appointments).ThenInclude(x => x.Profile).FirstOrDefaultAsync(m => m.Id == id);
            
            var appoint = await _context.CategoryAppointments.Include(x => x.Profile).FirstOrDefaultAsync(x => x.Category3Id == id);
            if (appoint != null)
            {

                var mainoutput = new CategoryDto
                {
                    Id = category3.Id,
                    Title = category3.Title,
                    Fullname = appoint.Profile.Fullname ?? "",
                    Image = appoint.Profile.PhotoUrl ?? "/img/logo-1.png",
                    Email = appoint.Profile.Email ?? "",
                    PhoneNumbers = appoint.Profile.PhoneNumber ?? "",
                    Color = category3.Color,
                    Biography = appoint.Profile.Biography ?? "",

                }; Category3 = mainoutput;
            }
            else
            {

                var mainoutput = new CategoryDto
                {
                    Id = category3.Id,
                    Title = category3.Title,
                    
                    Color = category3.Color, 

                }; Category3 = mainoutput;
            }
            

            if (Category3 == null)
            {
                return RedirectToPage("/");
            }

            var Category4s = _context.Category4s
                 .Include(x => x.Appointments)
                 .ThenInclude(x => x.Profile)
                 .Include(x => x.Category5s)
                 .Where(x => x.Category3Id == category3.Id)
                 .AsQueryable();
            //


            var output = Category4s
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
