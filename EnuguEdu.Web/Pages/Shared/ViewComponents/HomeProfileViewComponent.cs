﻿using Amazon.S3;
using EnuguEdu.Domain.Data;
using EnuguEdu.Domain.Dtos.AwsDtos;
using EnuguEdu.Domain.Services.AWS;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace EnuguEdu.Pages.Shared.ViewComponents
{
    public class HomeProfileViewComponent : ViewComponent
    {
        private readonly UserManager<EnuguEdu.Domain.Models.Profile> _userManager;
         

        public HomeProfileViewComponent(
            UserManager<EnuguEdu.Domain.Models.Profile> userManager
            )
        {
            _userManager = userManager;
           
        }

        public string UserInfo { get; set; }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            string LoggedInUser = _userManager.GetUserId(HttpContext.User);
            var user = await _userManager.FindByIdAsync(LoggedInUser);

            return View(user);
        }
    }
}
