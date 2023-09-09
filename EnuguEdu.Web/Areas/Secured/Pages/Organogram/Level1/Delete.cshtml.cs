using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using EnuguEdu.Domain.Data;
using EnuguEdu.Domain.Models;

namespace EnuguEdu.Web.Areas.Secured.Pages.Organogram.Level1
{
    public class DeleteModel : PageModel
    {
        private readonly EnuguEdu.Domain.Data.ApplicationDbContext _context;

        public DeleteModel(EnuguEdu.Domain.Data.ApplicationDbContext context)
        {
            _context = context;
        }

        [BindProperty]
      public OgLevel1 OgLevel1 { get; set; } = default!;

        public async Task<IActionResult> OnGetAsync(long? id)
        {
            if (id == null || _context.OgLevel1s == null)
            {
                return NotFound();
            }

            var oglevel1 = await _context.OgLevel1s.FirstOrDefaultAsync(m => m.Id == id);

            if (oglevel1 == null)
            {
                return NotFound();
            }
            else 
            {
                OgLevel1 = oglevel1;
            }
            return Page();
        }

        public async Task<IActionResult> OnPostAsync(long? id)
        {
            if (id == null || _context.OgLevel1s == null)
            {
                return NotFound();
            }
            var oglevel1 = await _context.OgLevel1s.FindAsync(id);

            if (oglevel1 != null)
            {
                OgLevel1 = oglevel1;
                _context.OgLevel1s.Remove(OgLevel1);
                await _context.SaveChangesAsync();
            }

            return RedirectToPage("./Index");
        }
    }
}
