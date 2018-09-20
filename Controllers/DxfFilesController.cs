using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Graduation.Data;
using Graduation.Data.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Hosting;
using Graduation.Models;
using System.IO;
using Graduation.Data.ViewModels;
using System.Net.Http.Headers;

namespace Graduation.Controllers
{
    public class DxfFilesController : Controller
    {
        private readonly ApplicationDbContext _context;

        private readonly DxfFileRepository repo;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly IHostingEnvironment _hostingEnvironment;
        private readonly UserManager<ApplicationUser> _userManager;

        public DxfFilesController(ApplicationDbContext context, SignInManager<ApplicationUser> signInManager, IHostingEnvironment hostingEnvironment, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            repo = new DxfFileRepository(context);
            _signInManager = signInManager;
            _hostingEnvironment = hostingEnvironment;
            _userManager = userManager;
        }

        // GET: DxfFiles
        public async Task<IActionResult> Index()
        {
            var applicationDbContext = repo.GetAllFiles();
            return View(await applicationDbContext.ToListAsync());
        }

        // GET: DxfFiles/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var dxfFile = await repo.GetAllFiles()
                .Include(d => d.ApplicationUser2)
                .SingleOrDefaultAsync(m => m.Id == id);
            if (dxfFile == null)
            {
                return NotFound();
            }

            return View(dxfFile);
        }

        // GET: DxfFiles/Create
        public IActionResult Create()
        {
            ViewData["FK_ApplicatioUserId"] = new SelectList(_context.Users, "Id", "Id");
            return View();
        }

        // POST: DxfFiles/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create([Bind("Id,FK_ApplicatioUserId,FileName,StaticFilePath,RelativeFilePath,UploadedTime")] DxfFile dxfFile)
        {
            if (ModelState.IsValid)
            {
                repo.Add(dxfFile);

                return RedirectToAction(nameof(Index));
            }
            ViewData["FK_ApplicatioUserId"] = new SelectList(_context.Users, "Id", "Id", dxfFile.FK_ApplicatioUserId);
            return View(dxfFile);
        }

        // GET: DxfFiles/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var dxfFile = await repo.GetAllFiles().SingleOrDefaultAsync(m => m.Id == id);
            if (dxfFile == null)
            {
                return NotFound();
            }
            ViewData["FK_ApplicatioUserId"] = new SelectList(_context.Users, "Id", "Id", dxfFile.FK_ApplicatioUserId);
            return View(dxfFile);
        }

        // POST: DxfFiles/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(int id, [Bind("Id,FK_ApplicatioUserId,FileName,StaticFilePath,RelativeFilePath,UploadedTime")] DxfFile dxfFile)
        {
            if (id != dxfFile.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    repo.Update(dxfFile);

                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!DxfFileExists(dxfFile.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            ViewData["FK_ApplicatioUserId"] = new SelectList(_context.Users, "Id", "Id", dxfFile.FK_ApplicatioUserId);
            return View(dxfFile);
        }

        // GET: DxfFiles/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var dxfFile = await repo.GetAllFiles()
                .Include(d => d.ApplicationUser2)
                .SingleOrDefaultAsync(m => m.Id == id);
            if (dxfFile == null)
            {
                return NotFound();
            }

            return View(dxfFile);
        }

        // POST: DxfFiles/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var dxfFile = await repo.GetAllFiles().SingleOrDefaultAsync(m => m.Id == id);
            repo.Delete(dxfFile);
            return RedirectToAction(nameof(Index));
        }

        private bool DxfFileExists(int id)
        {
            return repo.GetAllFiles().Any(e => e.Id == id);
        }
        public async Task<IActionResult> Download(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var dxfFile = await repo.GetAllFiles().SingleOrDefaultAsync(m => m.Id == id);
            var path = dxfFile.StaticFilePath;

            var memory = new MemoryStream();
            using (var stream = new FileStream(path, FileMode.Open))
            {
                await stream.CopyToAsync(memory);
            }
            memory.Position = 0;
            return File(memory, GetContentType(path), Path.GetFileName(path));
        }

        private string GetContentType(string path)
        {
            var types = GetMimeTypes();
            var ext = Path.GetExtension(path).ToLowerInvariant();
            return types[ext];
        }
        private Dictionary<string, string> GetMimeTypes()
        {
            return new Dictionary<string, string>
            {
                {".txt", "text/plain"},
                {".pdf", "application/pdf"},
                {".doc", "application/vnd.ms-word"},
                {".docx", "application/vnd.ms-word"},
                {".xls", "application/vnd.ms-excel"},
                {".xlsx", "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet"},
                {".png", "image/png"},
                {".jpg", "image/jpeg"},
                {".jpeg", "image/jpeg"},
                {".gif", "image/gif"},
                {".csv", "text/csv"},
                {".obj" ,"application/obj"},
                {".xyz" ,"application/xyz"},
                {".dxf","application/dxf" },
                {".ifc","application/ifc" }
            };
        }
        public IActionResult CanvasDrawer()
        {
            var DxfFileStaticPath = TempData["DxfFileStaticPath"];
            var DxFRelativePaths = TempData["DxFRelativePaths"];
            var RelativePaths = TempData["ImagePath"];
            var StaticPath = TempData["StaticImagePath"];
            var FileNameFull = TempData["FileNameFull"];

            TempData["DxfFileStaticPath1"] = DxfFileStaticPath;
            TempData["DxFRelativePaths1"] = DxFRelativePaths;
            TempData["StaticImagePath1"] = StaticPath;
            TempData["FileNameFull1"] = FileNameFull;
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> CanvasDrawer([Bind("TopLeft,TopRight,BottomRight,BottomLeft,Precision")] Polygon polygon)
        {
            var CurrentDate = DateTime.Now;
            var UploadDate = CurrentDate.ToString("yyyyMMdd_hhmmss");
            var DxfFileStaticPath = TempData["DxfFileStaticPath1"];
            var DxFRelativePaths = TempData["DxFRelativePaths1"];
            var StaticPath = TempData["StaticImagePath1"];
            var FileNameFull = TempData["FileNameFull1"].ToString();
            var DxfFileExtension = ".dxf";
            //Function Here
            //var ExcuationStatus = ModelBuilder(StaticPathNameNoExtension);
            var ExcuationStatus = 0;
            var currentUser = await _userManager.FindByNameAsync(User.Identity.Name);
            if (ExcuationStatus == 0)
            {
                var DxfFile = new DxfFile
                {
                    FileName = FileNameFull,
                    StaticFilePath = DxfFileStaticPath + DxfFileExtension,
                    RelativeFilePath = DxfFileExtension,
                    FK_ApplicatioUserId = currentUser.Id,
                    UploadedTime = CurrentDate
                };
                repo.Add(DxfFile);
                RedirectToAction("UploadCompelete");
            }
            else
            {
                RedirectToAction("ErrorPage");
            }
            return Ok();
        }
        [HttpPost]
        public async Task<IActionResult> UploadImageCad(ImageInput imageInput)
        {
            if (ModelState.IsValid)
            {
                var CurrentDate = DateTime.Now;
                var UploadDate = CurrentDate.ToString("yyyyMMdd_hhmmss");
                var userfileName = User.Identity.Name.ToString();
                var RootFolder = _hostingEnvironment.WebRootPath;
                var UsersFileLocation = "UsersDirectory";
                var DxfFileExtension = ".dxf";
                //var RootFfolderString = "wwwroot";

                var UserDirectory = $"{_hostingEnvironment.WebRootPath}/{UsersFileLocation}/{userfileName}/{UploadDate}";
                //var UserFilesDirectoryRelative = $"{RootFfolderString}/{UsersFileLocation}/{userfileName}/{UploadDate}";
                var UserFilesDirectoryRelative = $"{UsersFileLocation}/{userfileName}/{UploadDate}";
                //   var ImageFileLocationPaths = new List<String>();
                var uploads = Path.Combine(RootFolder, UsersFileLocation, userfileName, UploadDate);
                //Check if user Directory exsists , if not creates new directory 
                var exsits = Directory.Exists(UserDirectory);
                if (!exsits)
                {
                    Directory.CreateDirectory(UserDirectory);
                }

                var filePathName = ContentDispositionHeaderValue.Parse(imageInput.ImageFile.ContentDisposition).FileName.Trim('"');
                var fileExtention = Path.GetExtension(filePathName);
                if (!(fileExtention == ".jpg" || fileExtention == ".JPG" || fileExtention == ".PNG" || fileExtention == ".png"))
                {
                    return RedirectToAction("ErrorPage");
                }
                var fileName = Guid.NewGuid().ToString("N").Substring(0, 10);
                var FileNameFull = fileName + fileExtention;
                var StaticPath = Path.Combine(uploads, FileNameFull);
                var StaticPathNameNoExtension = Path.Combine(uploads, fileName);
                var RelativePaths = $"{UserFilesDirectoryRelative}/{fileName}" + $"{fileExtention}";
                //the image will be saved with a unique filename
                FileStream DestinationStream = new FileStream(StaticPath, FileMode.CreateNew);
                imageInput.ImageFile.CopyTo(DestinationStream);
                var DxfFileName = Guid.NewGuid().ToString("N").Substring(0, 10);
                var DxfFileStaticPath = Path.Combine(uploads, DxfFileName);
                var DxFRelativePaths = $"{UserFilesDirectoryRelative}/{DxfFileName}" + $"{DxfFileExtension}";
                //Function Here
                //var ExcuationStatus = ModelBuilder(StaticPathNameNoExtension);
                var ExcuationStatus = 0;
                var currentUser = await _userManager.FindByNameAsync(User.Identity.Name);
                if (ExcuationStatus == 0)
                {
                    var DxfFile = new DxfFile
                    {
                        FileName = FileNameFull,
                        StaticFilePath = DxfFileStaticPath + DxfFileExtension,
                        RelativeFilePath = DxfFileExtension,
                        FK_ApplicatioUserId = currentUser.Id,
                        UploadedTime = CurrentDate
                    };
                    repo.Add(DxfFile);
                }
                else
                {
                    return RedirectToAction("ErrorPage");
                }
                    //var ImageFileLocationArray = ImageFileLocationPaths.ToArray();
                    return RedirectToAction("UploadCompelete");
                }
                else
                {
                    return View("Error");
                }
            }
        public IActionResult UploadCompelete()
        {
            return View();
        }

        public IActionResult ErrorPage()
        {
            ViewBag.Error = "Sorry Try Again";
            return View();
        }
    }
}
