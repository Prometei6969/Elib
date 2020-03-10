using ELib.Models;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using System.Security.Cryptography;
using System;
using System.Linq;
using System.Data.Entity;
using System.Collections.Generic;
using System.IO;
using System.Web.Hosting;

namespace ELib.Controllers
{
    [Authorize]
    public class PublicationController : Controller
    {
        private ApplicationSignInManager _signInManager;
        private ApplicationUserManager _userManager;

        private ApplicationDbContext db = new ApplicationDbContext();
        public PublicationController()
{
}

        public PublicationController(ApplicationUserManager userManager, ApplicationSignInManager signInManager)
        {
            UserManager = userManager;
            SignInManager = signInManager;
        }

        public ApplicationSignInManager SignInManager
        {
            get
            {
                return _signInManager ?? HttpContext.GetOwinContext().Get<ApplicationSignInManager>();
            }
            private set
            {
                _signInManager = value;
            }
        }

        public ApplicationUserManager UserManager
        {
            get
            {
                return _userManager ?? HttpContext.GetOwinContext().GetUserManager<ApplicationUserManager>();
            }
            private set
            {
                _userManager = value;
            }
        }
        // GET: /Publication/Index
        public async Task<ActionResult> Index(ManageMessageId? message)
        {
            ViewBag.StatusMessage =
                message == ManageMessageId.CreatePublicationSuccess ? "Публикация добавлена"
                : message == ManageMessageId.DeletePublicationSuccess ? "Публикация удалена"
                : message == ManageMessageId.ChangePublicationSuccess ? "Публикация изменена"
                : "";


            var user = await UserManager.FindByIdAsync(User.Identity.GetUserId());
            if (user != null)
            {
                var Publications = from m in db.Publications
                                   select m;

                Publications = Publications.Where(p => p.Nickname.Contains(user.Nickname));
                Publications = Publications.OrderByDescending(p => p.Year);

                List<PublicationModel> Pub = Publications.ToList();

                return View(Pub);
            }

            return View();
    }

   
        //
        // GET: /Publication/Create
        public ActionResult Create()
        {
            return View();
        }

        //
        // POST: /Publication/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create(PublicationViewModel model)
        {
            
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var user = await UserManager.FindByIdAsync(User.Identity.GetUserId());
            if (user != null)
            {
                await SignInManager.SignInAsync(user, isPersistent: false, rememberBrowser: false);
                byte[] buffer = new byte[4];
                RNGCryptoServiceProvider rng = new RNGCryptoServiceProvider();
                rng.GetBytes(buffer);
                int Token = BitConverter.ToInt32(buffer, 0);
 

                var upload = model.Files[0];
                if (upload == null)
                {
                    return RedirectToAction("Index");
                }

                // получаем имя файла
                string fileName = System.IO.Path.GetFileName(upload.FileName);
                // сохраняем файл в папку Files в проекте
                var subPath = Server.MapPath("~/Files/" + user.Nickname);
                bool subExists = System.IO.Directory.Exists(subPath);
                if (!subExists)
                {
                    System.IO.Directory.CreateDirectory(subPath);
                }

 
                upload.SaveAs(Server.MapPath("~/Files/" + user.Nickname + "/" + Token + fileName));

                var Publication = new PublicationModel
                {
                    Name = model.Name,
                    Author = model.Author,
                    Year = model.Year,
                    Theme = model.Theme,
                    Annotation = model.Annotation,
                    Nickname = user.Nickname,
                    Token = Token,
                    FileName = upload.FileName,
                    Path = Server.MapPath("~/Files/" + user.Nickname + "/" + Token + fileName),
                };
                db.Publications.Add(Publication);
                db.SaveChanges();
                return RedirectToAction("Index", new { Message = ManageMessageId.CreatePublicationSuccess });
            }

            return View(model);
        }

 

        public async Task<ActionResult> Delete(int? id)
        {
            var Publication = await db.Publications.FindAsync(id);
            if (Publication == null)
            {
                return HttpNotFound();
            }
            return View(Publication);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int? id)
        {
            var Publication = await db.Publications.FindAsync(id);

            string Path = Publication.Path;
            bool Exists = System.IO.File.Exists(Path);

            if (Exists)
            {
                System.IO.File.Delete(Path);
                db.Publications.Remove(Publication);
                await db.SaveChangesAsync();
                return RedirectToAction("Index", new { Message = ManageMessageId.DeletePublicationSuccess });
            }

            return RedirectToAction("Index", new { Message = ManageMessageId.DeletePublicationSuccess });
        }

        //
        // GET: /Publication/Show
        public async Task<ActionResult> Show(int? id)
        {
            var Publication = await db.Publications.FindAsync(id);
            var model = new ShowPublicationModel
            {
                Id = id,
                IsUser = IsUser(id),
                Name = Publication.Name,
                Author = Publication.Author,
                Year = Publication.Year,
                Theme = Publication.Theme,
                Annotation = Publication.Annotation,

            };

            return View(model);
        }

        //
        // POST: /Publication/Show
        [HttpPost]
        [ValidateAntiForgeryToken]
        public  ActionResult Show()
        {
            
            return View();

        }


        public async Task<ActionResult> Download(int? id)
        {
            var Publication = await db.Publications.FindAsync(id);
            
            string file_path = Publication.Path;
            byte[] mas = System.IO.File.ReadAllBytes(file_path);
            string file_type = "application/pdf";
            string file_name = Publication.Name;
            return File(mas, file_type, file_name);
        }

        //
        // GET: /Publication/Change
        [HttpGet]
        public async Task<ActionResult> Change(int? id)
        {
            if(id == null)
    {
                return HttpNotFound();
            }
            var Publication = await db.Publications.FindAsync(id);
            if (Publication != null)
            {
                var PublicationViewModel = new PublicationViewModel
                {
                    Id = Publication.Id,
                    Name = Publication.Name,
                    Author = Publication.Author,
                    Year = Publication.Year,
                    Theme = Publication.Theme,
                    Annotation = Publication.Annotation,
                    Nickname = Publication.Nickname,
                    Token = Publication.Token,
                    FileName = Publication.FileName,
                    Path = Publication.Path,
                };
                return View(PublicationViewModel);
            }
            return HttpNotFound();
        }

        // POST: /Publication/Change
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Change(PublicationViewModel publication)
        {
            if (!ModelState.IsValid)
            {
                return View(publication);
            }
            var upload = publication.Files[0];
            var user = await UserManager.FindByIdAsync(User.Identity.GetUserId());
            if (upload != null)
            {
                
                if (user != null)
                {
                    await SignInManager.SignInAsync(user, isPersistent: false, rememberBrowser: false);
                    string fileName = System.IO.Path.GetFileName(upload.FileName);


                    var Publication = new PublicationModel
                    {
                        Id = publication.Id,
                        Name = publication.Name,
                        Author = publication.Author,
                        Year = publication.Year,
                        Theme = publication.Theme,
                        Annotation = publication.Annotation,
                        Nickname = publication.Nickname,
                        Token = publication.Token,
                        FileName = publication.FileName,
                        Path = publication.Path,
                    };

                    string Path = Publication.Path;
                    bool Exists = System.IO.File.Exists(Path);

                    if (Exists)
                    {
                        System.IO.File.Delete(Path);

                        var subPath = Server.MapPath("~/Files/" + user.Nickname);
                        bool subExists = System.IO.Directory.Exists(subPath);
                        if (!subExists)
                        {
                            System.IO.Directory.CreateDirectory(subPath);
                        }

                        upload.SaveAs(Server.MapPath("~/Files/" + user.Nickname + "/" + Publication.Token + fileName));

                        Publication.Path = Server.MapPath("~/Files/" + user.Nickname + "/" + Publication.Token + fileName);
                        Publication.FileName = fileName;
                        db.Entry(Publication).State = EntityState.Modified;
                        await db.SaveChangesAsync();

                        return RedirectToAction("Index", new { Message = ManageMessageId.ChangePublicationSuccess });
                    }
                }

                return RedirectToAction("Index");
            }

            
            if (user != null)
            {
                await SignInManager.SignInAsync(user, isPersistent: false, rememberBrowser: false);
                var Publication = new PublicationModel
                {
                    Id = publication.Id,
                    Name = publication.Name,
                    Author = publication.Author,
                    Year = publication.Year,
                    Theme = publication.Theme,
                    Annotation = publication.Annotation,
                    Nickname = publication.Nickname,
                    Token = publication.Token,
                    FileName = publication.FileName,
                    Path = publication.Path,
                };

                db.Entry(Publication).State = EntityState.Modified;
                await db.SaveChangesAsync();

                return RedirectToAction("Index", new { Message = ManageMessageId.DeletePublicationSuccess });


            }

            return RedirectToAction("Index");



        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (_userManager != null)
                {
                    _userManager.Dispose();
                    _userManager = null;
                }

                if (_signInManager != null)
                {
                    _signInManager.Dispose();
                    _signInManager = null;
                }
            }

            base.Dispose(disposing);
        }

        #region

        public bool IsUser(int? id)
        {
            var Publication = db.Publications.Find(id);
            var user = UserManager.FindById(User.Identity.GetUserId());
            if (user.Nickname != Publication.Nickname)
            {
                return false;
            }
            return true;
        }
        public enum ManageMessageId
        {
            CreatePublicationSuccess,
            DeletePublicationSuccess,
            ChangePublicationSuccess
        }
        #endregion
    }
}