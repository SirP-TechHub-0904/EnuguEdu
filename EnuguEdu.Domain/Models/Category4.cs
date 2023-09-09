using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace EnuguEdu.Domain.Models
{
    public class Category4
    {
        public long Id { get; set; }
        public string Title { get; set; }
        public int Order { get; set; }

        public string Description { get; set; }
        [Display(Name = "Address")]
        public string? Address { get; set; }

        [Display(Name = "Show Address")]
        public bool ShowAddress { get; set; }

        [Display(Name = "Email Address")]
        public string? Email { get; set; }

        [Display(Name = "Show Email")]
        public bool ShowEmail { get; set; }

        [Display(Name = "Phone Numbers")]
        public string? PhoneNumbers { get; set; }

        [Display(Name = "Show Phone Numbers")]
        public bool ShowPhoneNumbers { get; set; }


        [Display(Name = "Website")]
        public string? Website { get; set; }
        [Display(Name = "Show Website")]
        public bool ShowWebsite { get; set; }
        [Display(Name = "Code")]
        public string? Code { get; set; }
        [Display(Name = "Show Code")]
        public bool ShowCode { get; set; }

        public long Category3Id { get; set; }
        public Category3 Category3 { get; set; }

        public ICollection<Category5> Category5s { get; set; }
        public ICollection<OgAppointment> Appointments { get; set; }

         public string Color { get; set; }
    }
}
