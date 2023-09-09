using EnuguEdu.Domain.Dtos.AwsDtos;
using EnuguEdu.Domain.Models;
using EnuguEdu.Domain.Services.AWS;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace EnuguEdu.Web.Areas.Secured.Pages.Categories.Level2
{
    [Microsoft.AspNetCore.Authorization.Authorize(Roles = "mSuperAdmin,Admin")]
    public class NewOfficialModel : PageModel
    {
        private readonly EnuguEdu.Domain.Data.ApplicationDbContext _context;
        private readonly UserManager<Profile> _userManager;
        private readonly IConfiguration _config;
        private readonly IStorageService _storageService;

        public NewOfficialModel(EnuguEdu.Domain.Data.ApplicationDbContext context, UserManager<Profile> userManager, IConfiguration config, IStorageService storageService)
        {
            _context = context;
            _userManager = userManager;
            _config = config;
            _storageService = storageService;
        }

        public async Task<IActionResult> OnGetAsync(long? id)
        {
            if (id == null)
            {
                return RedirectToPage("/Result", new { error = "Invalid Data" });
            }

            Category2 = await _context.Category2s.FirstOrDefaultAsync(m => m.Id == id);
            if (Category2 == null)
            {
                return RedirectToPage("/Result", new { error = "Invalid Level" });
            }

            var list = _context.Users.Where(x => x.Email != "admin@abia.com").OrderBy(x => x.Title).AsQueryable();
            ViewData["ProfileId"] = new SelectList(list, "Id", "Fullname");

            RouteId = id;
            return Page();
        }

        [BindProperty]
        public CategoryAppointment CategoryAppointment { get; set; } = default!;
        public Category2 Category2 { get; set; } = default!;

        [BindProperty]
        public long? RouteId { get; set; }

        [BindProperty]
        public bool NewAccount { get; set; }

        [BindProperty]
        public Profile Profile { get; set; } = default!;

        [BindProperty]
        public IFormFile? file { get; set; }
        // To protect from overposting attacks, see https://aka.ms/RazorPagesCRUD
        public async Task<IActionResult> OnPostAsync()
        {
            if (String.IsNullOrEmpty(Profile.SurName))
            {
                var checkacc = await _userManager.FindByIdAsync(CategoryAppointment.ProfileId);
                if (checkacc == null)
                {
                    TempData["error"] = "Unable to fine account";
                    return Page();
                }
            }
            else
            {
                var user = new Profile
                {
                    UserName = Profile.Email,
                    Email = Profile.Email,
                    PhoneNumber = Profile.PhoneNumber,
                    FirstName = Profile.FirstName,
                    Title = Profile.Title,
                    SurName = Profile.SurName,
                    LastName = Profile.LastName,
                    Gender = Profile.Gender,
                    MaritalStatus = Profile.MaritalStatus,
                    ReligionStatus = Profile.ReligionStatus,
                    AltPhone = Profile.AltPhone,
                    Address = Profile.Address,
                    Biography = Profile.Biography


                };

               

                user.Id = Guid.NewGuid().ToString();
                var result = await _userManager.CreateAsync(user, "Abia@2023");
                if (result.Succeeded)
                {
                    CategoryAppointment.ProfileId = user.Id;
                    CategoryAppointment.Category2Id = RouteId;
                    _context.CategoryAppointments.Add(CategoryAppointment);
                    await _context.SaveChangesAsync();


                    try
                    {

                        if (file != null)
                        {
                            // Process file
                            await using var memoryStream = new MemoryStream();
                            await file.CopyToAsync(memoryStream);

                            var fileExt = Path.GetExtension(file.FileName);
                            var docName = $"{Guid.NewGuid()}{fileExt}";
                            // call server

                            var s3Obj = new Domain.Dtos.AwsDtos.S3Object()
                            {
                                BucketName = "abiastate",
                                InputStream = memoryStream,
                                Name = docName
                            };

                            var cred = new AwsCredentials()
                            {
                                AccessKey = _config["AwsConfiguration:AWSAccessKey"],
                                SecretKey = _config["AwsConfiguration:AWSSecretKey"]
                            };

                            var xresult = await _storageService.UploadFileReturnUrlAsync(s3Obj, cred, "");
                            // 
                            var updateuser = await _userManager.FindByIdAsync(user.Id);
                            if (xresult.Message.Contains("200"))
                            {
                                updateuser.PhotoUrl = xresult.Url;
                                updateuser.Key = xresult.Key;
                                await _userManager.UpdateAsync(updateuser);
                            }
                            else
                            {
                                TempData["error"] = "unable to upload image";
                                //return Page();
                            }
                        }
                    }
                    catch (Exception c)
                    {

                    }

                    return RedirectToPage("./Details", new { id = RouteId });
                }
                string messages = string.Join("; ", ModelState.Values
                                        .SelectMany(x => x.Errors)
                                        .Select(x => x.ErrorMessage));
                TempData["error"] = messages;
                return RedirectToPage("./Details", new { id = RouteId });
            }

            try
            {
                CategoryAppointment.Category2Id = RouteId;
                _context.CategoryAppointments.Add(CategoryAppointment);
                
                await _context.SaveChangesAsync();
                TempData["success"] = "successful";
            }
            catch(Exception c)
            {
                TempData["error"] = c.ToString();
            }
            return RedirectToPage("./Details", new { id = RouteId });
        }
    }
}
