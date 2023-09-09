using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using EnuguEdu.Domain.Data;
using EnuguEdu.Domain.Models;
using EnuguEdu.Domain.Dtos;

namespace EnuguEdu.Web.Areas.Secured.Pages.Categories.Level2
{
    [Microsoft.AspNetCore.Authorization.Authorize(Roles = "mSuperAdmin,Admin")]
    public class DetailsModel : PageModel
    {
        private readonly EnuguEdu.Domain.Data.ApplicationDbContext _context;

        public DetailsModel(EnuguEdu.Domain.Data.ApplicationDbContext context)
        {
            _context = context;
        }

        public Category2 Category2 { get; set; } = default!;

        public List<AppointmentDto> AppointmentDtos { get; set; }

        public async Task<IActionResult> OnGetAsync(long? id)
        {
            if (id == null || _context.Category2s == null)
            {
                return NotFound();
            }

            var category2 = await _context.Category2s
                .Include(x => x.Category1)
                .Include(x => x.Appointments)
                     .ThenInclude(x => x.Profile)
                     .Include(x => x.Category3s)
                     .ThenInclude(x => x.Appointments)
                      .Include(x => x.Category3s)
                     .ThenInclude(x => x.Category4s)

                .FirstOrDefaultAsync(m => m.Id == id);
            if (category2 == null)
            {
                return NotFound();
            }
            else
            {
                Category2 = category2;
            }

            var appointlist = _context.CategoryAppointments.Include(x => x.Profile).Where(x => x.Category2Id == id).OrderBy(x => x.Order).AsQueryable();
            var output = appointlist
                   .Select(d => new AppointmentDto
                   {
                       Id = d.Id,
                       Fullname = d.Profile.Fullname ?? "",
                       Position = d.Position ?? "",
                       EndOfAppointment = d.EndOfAppointment.ToString("yyyy") ?? "----",
                       StartOfAppointment = d.StartOfAppointment.ToString("yyyy") ?? "----",
                       UserId = d.ProfileId,
                       Photo = d.Profile.PhotoUrl
                   });
            AppointmentDtos = output.ToList();

            return Page();
        }
        [BindProperty]
        public long mid { get; set; }
        public async Task<IActionResult> OnPostDelete(long? id)
        {
            if (id == null)
            {
                TempData["error"] = "unable to delete";
                return RedirectToPage("./Details", new { id = mid });
            }
            var xpage = await _context.CategoryAppointments.FindAsync(id);

            if (xpage != null)
            {
                _context.CategoryAppointments.Remove(xpage);
                await _context.SaveChangesAsync();
            }
            TempData["success"] = "Successful";

            return RedirectToPage("./Details", new { id = mid });
        }

        public async Task<IActionResult> OnPostDeleteLevel(long? id)
        {
            if (id == null)
            {
                TempData["error"] = "unable to delete";
                return RedirectToPage("./Details", new { id = mid });
            }
            var xpage = await _context.Category3s.FindAsync(id);

            if (xpage != null)
            {
                if (xpage.Category4s != null)
                {
                    TempData["error"] = "Have casecading data";
                    return RedirectToPage("./Details", new { id = mid });
                }
                if (xpage.Appointments != null)
                {
                    TempData["error"] = "Have casecading data";
                    return RedirectToPage("./Details", new { id = mid });
                }
                _context.Category3s.Remove(xpage);
                await _context.SaveChangesAsync();
                TempData["success"] = "Successful";
            }

            return RedirectToPage("./Details", new { id = mid });
        }

        public async Task<IActionResult> OnPostRefreshColor(long? id)
        {
            if (id == null)
            {
                TempData["error"] = "unable to delete";
                return NotFound();
            }
            var xpage = await _context.Category3s.FindAsync(id);
            var list = new List<string> { "bg-red", "bg-yellow", "bg-aqua", "bg-blue",
                    "bg-green", "bg-navy", "bg-teal", "bg-olive", "bg-lime", "bg-orange", "bg-fuchsia", "bg-purple",
                    "bg-maroon", "bg-gray" }; var random = new Random();
            xpage.Color = list[random.Next(list.Count)];
            _context.Attach(xpage).State = EntityState.Modified;
            await _context.SaveChangesAsync();
            TempData["success"] = "Successful";


            return RedirectToPage("./Details", new { id = xpage.Category2Id });
        }

    }
}
