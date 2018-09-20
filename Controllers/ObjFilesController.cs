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
using Graduation.Models;
using Graduation.Data.ViewModels;
using Microsoft.AspNetCore.Hosting;
using System.IO;
using System.Net.Http.Headers;
using Microsoft.AspNetCore.Authorization;
using System.Runtime.InteropServices;

namespace Graduation.Controllers
{
    //[Authorize]
    public class ObjFilesController : Controller
    {
        [DllImport(@"C:\Users\Reda khamis\Desktop\CGAL\CodeFighter\PhotoCad\yarab\x64\Debug\PhotoDesk.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern int ModelBuilder(string XYZFile);

        private readonly ApplicationDbContext _context;
        private readonly ObjFileRepository repo;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly IHostingEnvironment _hostingEnvironment;
        private readonly UserManager<ApplicationUser> _userManager;

        public ObjFilesController(ApplicationDbContext context, SignInManager<ApplicationUser> signInManager, IHostingEnvironment hostingEnvironment, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            repo = new ObjFileRepository(context);
            _signInManager = signInManager;
            _hostingEnvironment = hostingEnvironment;
            _userManager = userManager;
        }

        // GET: ObjFiles
        public async Task<IActionResult> Index()
        {
            var applicationDbContext = repo.GetAllFiles();
            return View(await applicationDbContext.ToListAsync());
        }

        // GET: ObjFiles/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var objFile = await repo.GetAllFiles()
                .Include(o => o.ApplicationUser)
                .SingleOrDefaultAsync(m => m.Id == id);
            if (objFile == null)
            {
                return NotFound();
            }

            return View(objFile);
        }

        // GET: ObjFiles/Create
        public IActionResult Create()
        {
            ViewData["FK_ApplicatioUserId"] = new SelectList(_context.Users, "Id", "Id");
            return View();
        }

        // POST: ObjFiles/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create([Bind("Id,FK_ApplicatioUserId,StaticFilePath,RelativeFilePath,UploadedTime")] ObjFile objFile)
        {
            if (ModelState.IsValid)
            {
                repo.Add(objFile);
                return RedirectToAction(nameof(Index));
            }
            ViewData["FK_ApplicatioUserId"] = new SelectList(_context.Users, "Id", "Id", objFile.FK_ApplicatioUserId);
            return View(objFile);
        }

        // GET: ObjFiles/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var objFile = await repo.GetAllFiles().SingleOrDefaultAsync(m => m.Id == id);
            if (objFile == null)
            {
                return NotFound();
            }
            ViewData["FK_ApplicatioUserId"] = new SelectList(_context.Users, "Id", "Id", objFile.FK_ApplicatioUserId);
            return View(objFile);
        }

        // POST: ObjFiles/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(int id, [Bind("Id,FK_ApplicatioUserId,StaticFilePath,RelativeFilePath,UploadedTime")] ObjFile objFile)
        {
            if (id != objFile.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    repo.Update(objFile);
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ObjFileExists(objFile.Id))
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
            ViewData["FK_ApplicatioUserId"] = new SelectList(_context.Users, "Id", "Id", objFile.FK_ApplicatioUserId);
            return View(objFile);
        }

        // GET: ObjFiles/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var objFile = await repo.GetAllFiles()
                .Include(o => o.ApplicationUser)
                .SingleOrDefaultAsync(m => m.Id == id);
            if (objFile == null)
            {
                return NotFound();
            }

            return View(objFile);
        }

        // POST: ObjFiles/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var objFile = await repo.GetAllFiles().SingleOrDefaultAsync(m => m.Id == id);
            repo.Delete(objFile);
            return RedirectToAction(nameof(Index));
        }

        private bool ObjFileExists(int id)
        {
            return repo.GetAllFiles().Any(e => e.Id == id);
        }


        public IActionResult ModelBuilderForC()
        {
            var ExctionStatus = ModelBuilder(@"C:\Users\Reda khamis\Desktop\CGAL\CodeFighter\PhotoCad\yarab\yarab\data\cube");
            return View();
        }
        public IActionResult Upload()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Upload(FileData fileData)
        {
            if (ModelState.IsValid)
            {
                var CurrentDate = DateTime.Now;
                var UploadDate = CurrentDate.ToString("yyyyMMdd_hhmmss");
                var userfileName = User.Identity.Name.ToString();
                var RootFolder = _hostingEnvironment.WebRootPath;
                var UsersFileLocation = "UsersDirectory";
                //var RootFfolderString = "wwwroot";
                var UserDirectory = $"{_hostingEnvironment.WebRootPath}/{UsersFileLocation}/{userfileName}/{UploadDate}";
                //var UserFilesDirectoryRelative = $"{RootFfolderString}/{UsersFileLocation}/{userfileName}/{UploadDate}";
                var UserFilesDirectoryRelative = $"{UsersFileLocation}/{userfileName}/{UploadDate}";
                var ImageFileLocationPaths = new List<String>();
                //Check if user Directory exsists , if not creates new directory 
                var exsits = Directory.Exists(UserDirectory);
                if (!exsits)
                {
                    Directory.CreateDirectory(UserDirectory);
                }
                foreach (var ObjFile in fileData.ObjFile)
                {
                    var filePathName = ContentDispositionHeaderValue.Parse(ObjFile.ContentDisposition).FileName.Trim('"');
                    var fileExtention = Path.GetExtension(filePathName);
                    if (!(fileExtention == ".obj" || fileExtention == ".OBJ"))
                    {
                        return RedirectToAction("ErrorPage");
                    }
                    var uploads = Path.Combine(RootFolder, UsersFileLocation, userfileName, UploadDate);
                    var fileName = Guid.NewGuid().ToString("N").Substring(0, 10);
                    var FileNameFull = fileName + fileExtention;
                    var StaticPath = Path.Combine(uploads, FileNameFull);
                    var StaticPathNameNoExtension = Path.Combine(uploads, fileName);
                    var RelativePaths = $"{UserFilesDirectoryRelative}/{FileNameFull}";
                    //the image will be saved with a unique filename
                    FileStream DestinationStream = new FileStream(StaticPath, FileMode.CreateNew);
                    //ImageFileLocationPaths.Add(PathObjRelative);
                    //the image will be saved with a unique filename
                    // ImageFile.CopyToAsync(DestinationStream);
                    ObjFile.CopyTo(DestinationStream);

                    var currentUser = await _userManager.FindByNameAsync(User.Identity.Name);
                    var ObjectFile = new ObjFile
                    {
                        FileName = FileNameFull,
                        StaticFilePath = StaticPath,
                        RelativeFilePath = RelativePaths,
                        FK_ApplicatioUserId = currentUser.Id,
                        UploadedTime = CurrentDate
                    };
                    repo.Add(ObjectFile);
                }
                //var ImageFileLocationArray = ImageFileLocationPaths.ToArray();
                return RedirectToAction("UploadCompelete");
            }
            else
            {
                return View("Error");
            }

        }
        public IActionResult UploadPointCloud()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> UploadPointCloud(PointCloud pointCloud)
        {
            if (ModelState.IsValid)
            {
                var CurrentDate = DateTime.Now;
                var UploadDate = CurrentDate.ToString("yyyyMMdd_hhmmss");
                var userfileName = User.Identity.Name.ToString();
                var RootFolder = _hostingEnvironment.WebRootPath;
                var UsersFileLocation = "UsersDirectory";
               // var RootFfolderString = "wwwroot";
                var ObjFileExtension = ".obj";
                var UserDirectory = $"{_hostingEnvironment.WebRootPath}/{UsersFileLocation}/{userfileName}/{UploadDate}";
                //var UserFilesDirectoryRelative = $"{RootFfolderString}/{UsersFileLocation}/{userfileName}/{UploadDate}";
                var UserFilesDirectoryRelative = $"{UsersFileLocation}/{userfileName}/{UploadDate}";
                //var ImageFileLocationPaths = new List<String>();
                //Check if user Directory exsists , if not creates new directory 
                var exsits = Directory.Exists(UserDirectory);
                if (!exsits)
                {
                    Directory.CreateDirectory(UserDirectory);
                }
                var filePathName = ContentDispositionHeaderValue.Parse(pointCloud.PointCloudFile.ContentDisposition).FileName.Trim('"');
                var fileExtention = Path.GetExtension(filePathName);
                if (!(fileExtention == ".xyz" || fileExtention == ".XYZ"))
                {
                    return RedirectToAction("ErrorPage");
                }
                var uploads = Path.Combine(RootFolder, UsersFileLocation, userfileName, UploadDate);
                var fileName = Guid.NewGuid().ToString("N").Substring(0, 10);
                var FileNameFull = fileName + fileExtention;
                var FileNameObj = fileName + ObjFileExtension;
                var StaticPath = Path.Combine(uploads, FileNameFull);
                var StaticPathNameNoExtension = Path.Combine(uploads, fileName);
                var RelativePaths = $"{UserFilesDirectoryRelative}/{fileName}" + $"{ObjFileExtension}";
                //the image will be saved with a unique filename
                FileStream DestinationStream = new FileStream(StaticPath, FileMode.CreateNew);
                //ImageFileLocationPaths.Add(PathObjRelative);
                //the image will be saved with a unique filename
                // ImageFile.CopyToAsync(DestinationStream);
                pointCloud.PointCloudFile.CopyTo(DestinationStream);
                var ExcuationStatus = ModelBuilder(StaticPathNameNoExtension);
                var currentUser = await _userManager.FindByNameAsync(User.Identity.Name);
                if (ExcuationStatus == 0)
                {
                    var ObjectFile = new ObjFile
                    {
                        FileName = FileNameObj,
                        StaticFilePath = StaticPathNameNoExtension + ObjFileExtension,
                        RelativeFilePath = RelativePaths,
                        FK_ApplicatioUserId = currentUser.Id,
                        UploadedTime = CurrentDate
                    };
                    repo.Add(ObjectFile);
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
        [HttpPost]
        public async Task<IActionResult> UploadPointCloudGeneric(PointCloud pointCloud)
        {
            if (ModelState.IsValid)
            {
                var CurrentDate = DateTime.Now;
                var UploadDate = CurrentDate.ToString("yyyyMMdd_hhmmss");
                var userfileName = User.Identity.Name.ToString();
                var RootFolder = _hostingEnvironment.WebRootPath;
                var UsersFileLocation = "UsersDirectory";
              //  var RootFfolderString = "wwwroot";
                var ObjFileExtension = ".obj";
                var UserDirectory = $"{_hostingEnvironment.WebRootPath}/{UsersFileLocation}/{userfileName}/{UploadDate}";
                //var UserFilesDirectoryRelative = $"{RootFfolderString}/{UsersFileLocation}/{userfileName}/{UploadDate}";
                var UserFilesDirectoryRelative = $"{UsersFileLocation}/{userfileName}/{UploadDate}";
                //var ImageFileLocationPaths = new List<String>();
                //Check if user Directory exsists , if not creates new directory 
                var exsits = Directory.Exists(UserDirectory);
                if (!exsits)
                {
                    Directory.CreateDirectory(UserDirectory);
                }
                var filePathName = ContentDispositionHeaderValue.Parse(pointCloud.PointCloudFile.ContentDisposition).FileName.Trim('"');
                var fileExtention = Path.GetExtension(filePathName);
                if (!(fileExtention == ".xyz" || fileExtention == ".XYZ"))
                {
                    return RedirectToAction("ErrorPage");
                }
                var uploads = Path.Combine(RootFolder, UsersFileLocation, userfileName, UploadDate);
                var fileName = Guid.NewGuid().ToString("N").Substring(0, 10);
                var FileNameFull = fileName + fileExtention;
                var FileNameObj = fileName + ObjFileExtension;
                var StaticPath = Path.Combine(uploads, FileNameFull);
                var StaticPathNameNoExtension = Path.Combine(uploads, fileName);
                var RelativePaths = $"{UserFilesDirectoryRelative}/{fileName}" + $"{ObjFileExtension}";
                //the image will be saved with a unique filename
                FileStream DestinationStream = new FileStream(StaticPath, FileMode.CreateNew);
                //ImageFileLocationPaths.Add(PathObjRelative);
                //the image will be saved with a unique filename
                // ImageFile.CopyToAsync(DestinationStream);
                pointCloud.PointCloudFile.CopyTo(DestinationStream);
                var ExcuationStatus = ModelBuilder(StaticPathNameNoExtension);
                var currentUser = await _userManager.FindByNameAsync(User.Identity.Name);
                if (ExcuationStatus == 0)
                {
                    var ObjectFile = new ObjFile
                    {
                        FileName = FileNameObj,
                        StaticFilePath = StaticPathNameNoExtension + ObjFileExtension,
                        RelativeFilePath = RelativePaths,
                        FK_ApplicatioUserId = currentUser.Id,
                        UploadedTime = CurrentDate
                    };
                    repo.Add(ObjectFile);
                }
                else
                {
                    return RedirectToAction("ErrorPage");
                }
                return RedirectToAction("UploadCompelete");
            }
            else
            {
                return View("Error");
            }

        }
        [HttpPost]
        public async Task<IActionResult> UploadImagesCloud(ImageData imageData)
        {
            if (ModelState.IsValid)
            {
                var CurrentDate = DateTime.Now;
                var UploadDate = CurrentDate.ToString("yyyyMMdd_hhmmss");
                var userfileName = User.Identity.Name.ToString();
                var RootFolder = _hostingEnvironment.WebRootPath;
                var UsersFileLocation = "UsersDirectory";
               // var RootFfolderString = "wwwroot";
                var ObjFileExtension = ".obj";

                var UserDirectory = $"{_hostingEnvironment.WebRootPath}/{UsersFileLocation}/{userfileName}/{UploadDate}";
                //var UserFilesDirectoryRelative = $"{RootFfolderString}/{UsersFileLocation}/{userfileName}/{UploadDate}";
                var UserFilesDirectoryRelative = $"{UsersFileLocation}/{userfileName}/{UploadDate}";
                var ImageFileLocationPaths = new List<String>();
                var uploads = Path.Combine(RootFolder, UsersFileLocation, userfileName, UploadDate);
                //Check if user Directory exsists , if not creates new directory 
                var exsits = Directory.Exists(UserDirectory);
                if (!exsits)
                {
                    Directory.CreateDirectory(UserDirectory);
                }
                foreach (var Image in imageData.ImageFile)
                {
                    var filePathName = ContentDispositionHeaderValue.Parse(Image.ContentDisposition).FileName.Trim('"');
                    var fileExtention = Path.GetExtension(filePathName);
                    if (!(fileExtention == ".jpg" || fileExtention == ".JPG" || fileExtention == ".PNG" || fileExtention == ".png"))
                    {
                        return RedirectToAction("ErrorPage");
                    }
                    var fileName = Guid.NewGuid().ToString("N").Substring(0, 10);
                    var FileNameFull = fileName + fileExtention;
                    //var FileNameObj = fileName + ObjFileExtension;
                    var StaticPath = Path.Combine(uploads, FileNameFull);
                    var StaticPathNameNoExtension = Path.Combine(uploads, fileName);
                    var RelativePaths = $"{UserFilesDirectoryRelative}/{fileName}" + $"{fileExtention}";
                    //the image will be saved with a unique filename
                    FileStream DestinationStream = new FileStream(StaticPath, FileMode.CreateNew);
                    ImageFileLocationPaths.Add(StaticPath);
                    Image.CopyTo(DestinationStream);
                }
                var ImageFileLocationArray = ImageFileLocationPaths.ToArray();
                var ObjFileName = Guid.NewGuid().ToString("N").Substring(0, 10);
                var ObjFileStaticPath = Path.Combine(uploads, ObjFileName);
                var ArraySize = ImageFileLocationArray.Length;
                var ObjRelativePaths = $"{UserFilesDirectoryRelative}/{ObjFileName}" + $"{ObjFileExtension}";
                //var ExcuationStatus = ModelBuilder(ObjFileStaticPath);
                var currentUser = await _userManager.FindByNameAsync(User.Identity.Name);
                //if (ExcuationStatus == 0)
                //{
                //    var ObjectFile = new ObjFile
                //    {
                //        FileName = FileNameObj,
                //        StaticFilePath = ObjFileStaticPath + ObjFileExtension,
                //        RelativeFilePath = ObjRelativePaths,
                //        FK_ApplicatioUserId = currentUser.Id,
                //        UploadedTime = CurrentDate
                //    };
                //    repo.Add(ObjectFile);
                // }
                //else
                //{
                //    return RedirectToAction("ErrorPage");
                //}
                return RedirectToAction("UploadCompelete");
            }
            else
            {
                return View("Error");
            }

        }
        [HttpPost]
        public async Task<IActionResult> UploadImagesCloudGeneric(ImageData imageData)
        {
            if (ModelState.IsValid)
            {
                var CurrentDate = DateTime.Now;
                var UploadDate = CurrentDate.ToString("yyyyMMdd_hhmmss");
                var userfileName = User.Identity.Name.ToString();
                var RootFolder = _hostingEnvironment.WebRootPath;
                var UsersFileLocation = "UsersDirectory";
               // var RootFfolderString = "wwwroot";
                var ObjFileExtension = ".obj";

                var UserDirectory = $"{_hostingEnvironment.WebRootPath}/{UsersFileLocation}/{userfileName}/{UploadDate}";
                //var UserFilesDirectoryRelative = $"{RootFfolderString}/{UsersFileLocation}/{userfileName}/{UploadDate}";
                var UserFilesDirectoryRelative = $"{UsersFileLocation}/{userfileName}/{UploadDate}";
                var ImageFileLocationPaths = new List<String>();
                var uploads = Path.Combine(RootFolder, UsersFileLocation, userfileName, UploadDate);
                //Check if user Directory exsists , if not creates new directory 
                var exsits = Directory.Exists(UserDirectory);
                if (!exsits)
                {
                    Directory.CreateDirectory(UserDirectory);
                }
                foreach (var Image in imageData.ImageFile)
                {
                    var filePathName = ContentDispositionHeaderValue.Parse(Image.ContentDisposition).FileName.Trim('"');
                    var fileExtention = Path.GetExtension(filePathName);
                    if (!(fileExtention == ".jpg" || fileExtention == ".JPG" || fileExtention == ".PNG" || fileExtention == ".png"))
                    {
                        return RedirectToAction("ErrorPage");
                    }
                    var fileName = Guid.NewGuid().ToString("N").Substring(0, 10);
                    var FileNameFull = fileName + fileExtention;
                    //var FileNameObj = fileName + ObjFileExtension;
                    var StaticPath = Path.Combine(uploads, FileNameFull);
                    var StaticPathNameNoExtension = Path.Combine(uploads, fileName);
                    var RelativePaths = $"{UserFilesDirectoryRelative}/{fileName}" + $"{fileExtention}";
                    //the image will be saved with a unique filename
                    FileStream DestinationStream = new FileStream(StaticPath, FileMode.CreateNew);
                    ImageFileLocationPaths.Add(StaticPath);
                    Image.CopyTo(DestinationStream);
                }
                var ImageFileLocationArray = ImageFileLocationPaths.ToArray();
                var ObjFileName = Guid.NewGuid().ToString("N").Substring(0, 10);
                var ObjFileStaticPath = Path.Combine(uploads, ObjFileName);
                var ArraySize = ImageFileLocationArray.Length;
                var ObjRelativePaths = $"{UserFilesDirectoryRelative}/{ObjFileName}" + $"{ObjFileExtension}";
                //var ExcuationStatus = ModelBuilder(ObjFileStaticPath);
                var currentUser = await _userManager.FindByNameAsync(User.Identity.Name);
                //if (ExcuationStatus == 0)
                //{
                //    var ObjectFile = new ObjFile
                //    {
                //        FileName = FileNameObj,
                //        StaticFilePath = ObjFileStaticPath + ObjFileExtension,
                //        RelativeFilePath = ObjRelativePaths,
                //        FK_ApplicatioUserId = currentUser.Id,
                //        UploadedTime = CurrentDate
                //    };
                //    repo.Add(ObjectFile);
                // }
                //else
                //{
                //    return RedirectToAction("ErrorPage");
                //}
                return RedirectToAction("UploadCompelete");
            }
            else
            {
                return View("Error");
            }

        }
        [HttpPost]
        public IActionResult UploadImageCad(ImageInput imageInput)
        {
            if (ModelState.IsValid)
            {
                var CurrentDate = DateTime.Now;
                var UploadDate = CurrentDate.ToString("yyyyMMdd_hhmmss");
                var userfileName = User.Identity.Name.ToString();
                var RootFolder = _hostingEnvironment.WebRootPath;
                var UsersFileLocation = "UsersDirectory";
                //var RootFfolderString = "wwwroot";
                var DxfFileExtension = ".dxf";

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
                TempData["DxfFileStaticPath"] = DxfFileStaticPath;
                TempData["DxFRelativePaths"] = DxFRelativePaths;
                TempData["ImagePath"] = RelativePaths;
                TempData["StaticImagePath"] = StaticPath;
                TempData["FileNameFull"] = FileNameFull;
                return RedirectToAction("CanvasDrawer");

            }
            else
            {
                return View("Error");
            }

        }

        public IActionResult ObjViewer()
        {
            return View();
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
          //  var DxfFileExtension = ".dxf";
            //Function Here
           // var ExcuationStatus = 0;
            //var ExcuationStatus = ModelBuilder(StaticPathNameNoExtension);
            var currentUser = await _userManager.FindByNameAsync(User.Identity.Name);
            //if (ExcuationStatus == 0)
            //{
            //    var DxfFile = new DxfFile
            //    {
            //        FileName = FileNameFull,
            //        StaticFilePath = DxfFileStaticPath + DxfFileExtension,
            //        RelativeFilePath = DxfFileExtension,
            //        FK_ApplicatioUserId = currentUser.Id,
            //        UploadedTime = CurrentDate
            //    };
            //    repo.Add(DxfFile);
            //}
                return View();
        }

        public async Task<IActionResult> Download(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var objFile = await repo.GetAllFiles().SingleOrDefaultAsync(m => m.Id == id);
            var path = objFile.StaticFilePath;

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

        public IActionResult UploadCompelete()
        {
            return View();
        }

        public IActionResult ErrorPage()
        {
            ViewBag.Error = "Please upload an Image file with Proper Extension";
            return View();

        }
    }
}
