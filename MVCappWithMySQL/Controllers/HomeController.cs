using System;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin.Security;
using MVCappWithMySQL.Models;
using AspNet.Identity.MySQL;
using Template.Data;
using Repository.Pattern.DataContext;
using Repository.Pattern.UnitOfWork;
using Repository.Pattern.Infrastructure;
using Template.Data.Models;
using Template.Data.Service;
using Repository.Pattern.Ef6;
using AkiraSocial;
using Repository.Pattern.Repositories;

namespace MVCappWithMySQL.Controllers
{
    public class HomeController : Controller
    {
        private ApplicationSignInManager _signInManager;
        private ApplicationUserManager _userManager;
        private ApplicationRoleManager _roleManager;
        private UserRolesTableRepository _userRolesRepository;
        public HomeController()
        {
        }

        public HomeController(ApplicationUserManager userManager, ApplicationSignInManager signInManager, ApplicationRoleManager roleManager, UserRolesTableRepository userRolesRepository)
        {
            UserManager = userManager;
            SignInManager = signInManager;
            RoleManager = roleManager;
            UserRolesRepository = userRolesRepository;


        }
        public UserRolesTableRepository UserRolesRepository
        {
            get
            {
                return new UserRolesTableRepository(new IdentityDbContext("DefaultConnection"));
            }
            set
            {
                _userRolesRepository = value; 
            }
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
        public ApplicationRoleManager RoleManager
        {
            get
            {
                return _roleManager ?? HttpContext.GetOwinContext().Get<ApplicationRoleManager>();
            }
            private set
            {
                _roleManager = value;
            }
        }

        


        public ActionResult Index()
        {
            //Add category with unitofwork
            using (IDataContextAsync applicationdb = new ApplicationDbContext())
            using (IUnitOfWorkAsync unitOfWork = new UnitOfWork(applicationdb))
            {
                if (!unitOfWork.Repository<Category>().Query(x=>x.CategoryName =="Cat1").Select().Any())
                {
                    unitOfWork.Repository<Category>().Insert(new Category { CategoryName = "Cat1", ObjectState = ObjectState.Added });
                    unitOfWork.SaveChanges();
                }
            }
            //Using custom repository by service
            using (IDataContextAsync context = new ApplicationDbContext())
            using (IUnitOfWorkAsync unitOfWork = new UnitOfWork(context))
            {
                IRepositoryAsync<Category> categoryRepository = new Repository<Category>(context, unitOfWork);
                //IService<Category> categoryservice = new CategoryService(categoryRepository);
                ICategoryService categoryservice = new CategoryService(categoryRepository);
                var category = categoryservice.CategoriesByName("Cat1");
            }
         


        

                return View();
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";
            var user = UserManager.FindById(User.Identity.GetUserId());
            //Using custom repository by service

            //using (IDataContextAsync context = new ApplicationDbContext())
            //using (IUnitOfWorkAsync unitOfWork = new UnitOfWork(context))
            //{
            //    IRepositoryAsync<UserProfile> userprofileRepository = new Repository<UserProfile>(context, unitOfWork);
            //    IUserProfileService userprofileservice = new UserProfileService(userprofileRepository);
            //    userprofileservice.Insert(user, new UserProfile { Surname = "PROVA" });
            //    unitOfWork.SaveChanges();
            //}


            using (IDataContextAsync context = new ApplicationDbContext())
            using (IUnitOfWorkAsync unitOfWork = new UnitOfWork(context))
            {
                IRepositoryAsync<UserProfile> categoryRepository = new Repository<UserProfile>(context, unitOfWork);
                unitOfWork.Repository<UserProfile>().Insert(new UserProfile { UserId = user.Id, Surname = "PROVA", ObjectState = ObjectState.Added });
                unitOfWork.SaveChanges();
            }
            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
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
    }
}