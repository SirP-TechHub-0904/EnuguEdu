using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using EnuguEdu.Domain.Data;
using EnuguEdu.Domain.Models;

namespace EnuguEdu.Web.Areas.Secured.Pages.Categories.Cat
{
    public class IndexModel : PageModel
    {
        private readonly EnuguEdu.Domain.Data.ApplicationDbContext _context;

        public IndexModel(EnuguEdu.Domain.Data.ApplicationDbContext context)
        {
            _context = context;
        }

        public IList<Category1> Category1 { get; set; }

        public async Task OnGetAsync()
        {
            var xCategory1 = await _context.OgLevel1s.ToListAsync();

//            bg-navy { background-color: #001F3F; }
//.bg-blue { background-color: #0074D9; }
//.bg-aqua { background-color: #7FDBFF; }
//.bg-teal { background-color: #39CCCC; }
//.bg-olive { background-color: #3D9970; }
//.bg-green { background-color: #2ECC40; }
//.bg-lime { background-color: #01FF70; }
//.bg-yellow { background-color: #FFDC00; }
//.bg-orange { background-color: #FF851B; }
//.bg-red { background-color: #FF4136; }
//.bg-fuchsia { background-color: #F012BE; }
//.bg-purple { background-color: #B10DC9; }
//.bg-maroon { background-color: #85144B; }
//.bg-white { background-color: #FFFFFF; }
//.bg-gray { background-color: #AAAAAA; }
//.bg-silver { background-color: #DDDDDD; }
//.bg-black { background-color: #111111; }



            foreach (var d in xCategory1)
            {
                var list = new List<string> { "bg-red", "bg-yellow", "bg-aqua", "bg-blue", 
                    "bg-green", "bg-navy", "bg-teal", "bg-olive", "bg-lime", "bg-orange", "bg-fuchsia", "bg-purple", 
                    "bg-maroon", "bg-gray" };
                var random = new Random();
                d.Color = list[random.Next(list.Count)];

                _context.Attach(d).State = EntityState.Modified;


            }
            var xCategory2 = await _context.OgLevel2s.ToListAsync();

            foreach (var d in xCategory2)
            {
                var list = new List<string> { "bg-red", "bg-yellow", "bg-aqua", "bg-blue",
                    "bg-green", "bg-navy", "bg-teal", "bg-olive", "bg-lime", "bg-orange", "bg-fuchsia", "bg-purple",
                    "bg-maroon", "bg-gray" }; var random = new Random();
                d.Color = list[random.Next(list.Count)];

                _context.Attach(d).State = EntityState.Modified;


            }

            await _context.SaveChangesAsync();
        }
    }
}
