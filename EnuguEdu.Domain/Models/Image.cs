﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EnuguEdu.Domain.Models
{
    public class Image
    {
       public long Id { get; set; }
        public string? ImageUrl { get; set; }
        public string? Key { get; set; }
        public string? Description { get; set; }
        //public long PropertyId { get; set; }
        //public Property Property { get; set; }      
        public bool Active { get; set; }
        public bool Default { get; set; }
        public DateTime Date { get; set; }
    }
}
