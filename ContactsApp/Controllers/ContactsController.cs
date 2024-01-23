using Microsoft.AspNetCore.Mvc;
using ContactsApp.Models;
using System.Data;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Authorization;
using log4net;
using System.Net.Http;

namespace ContactsApp.Controllers
{
    [SessionCheck]
    public class ContactsController : Controller
    {
        
        private readonly ApiService _apiService;
        public ContactsController(ApiService apiService)
        {
            _apiService = apiService;
        }
        [HttpGet]
        public IActionResult Import()
        {
            return View();
        }
        [HttpPost]
        public IActionResult Import(IFormFile file)
        {
            
                
                    _apiService.ImportFromExcel(file);
                    ViewBag.Message = "Imported Sucessfully";
                    return View();
                    
        }
        [HttpGet]
        public IActionResult Export()
        {
            var userIdSession = HttpContext.Session.GetInt32("UserId");
            int userId = userIdSession.Value;

            _apiService.ExportToExcel(userId);
            ViewBag.Message = "Exported Sucessfully";
            return RedirectToAction("Index");

        }
        [HttpGet]
        public IActionResult Logout()
        {
            HttpContext.Session.Clear();
            return RedirectToAction("Login");
        }
        [HttpGet]
        public IActionResult Delete(int Id)
        {
            _apiService.DeleteContact(Id);
            return RedirectToAction("Index");

        }
        [HttpGet]
        public IActionResult Contact()
        {
            Contact model = _apiService.GetContactModel();
            return View(model);
        }

        [HttpPost]
        public IActionResult Contact(Contact contact)
        {
            if (ModelState.IsValid)
            {
                var userIdSession = HttpContext.Session.GetInt32("UserId");
                int userId = userIdSession.Value;
                AddContact addcontact = new AddContact
                {
                    UserId = userId,
                    Email = contact.Email,
                    FirstName = contact.FirstName,
                    LastName = contact.LastName,
                    Number = contact.Number,
                    TypeId = contact.TypeId,
                    Location = contact.Location
                };

                _apiService.AddContact(addcontact);
                return RedirectToAction("Index");
            }
            return View(contact);
        }

        [HttpGet]
        public ActionResult Edit(int Id)
        {
            Contact model = _apiService.GetEditContactModel(Id);
            return View(model);
        }

        [HttpPost]
        public IActionResult Edit(Contact contact)
        {
            _apiService.EditContact(contact);
            return RedirectToAction("Index");
        }
        [AllowAnonymous]
        [HttpGet]
        public IActionResult Login()
        {
            
            return View();
        }
        [HttpPost]
        [AllowAnonymous]
        public IActionResult Login(LoginModel user)
        {
            if (ModelState.IsValid)
            {
                if (_apiService.FindUser(user.Username))
                {
                    if (_apiService.TestPassword(user))
                    {
                        int userid = _apiService.GetUserId(user.Username);
                        HttpContext.Session.SetString("UserName", user.Username);
                        HttpContext.Session.SetInt32("UserId", userid);
                        return RedirectToAction("Index");

                    }
                    else
                    {
                        ModelState.AddModelError("Password", "Incorrect Password");
                        return View(user);
                    }
                }
                else
                {
                    ModelState.AddModelError("Username", "You Are Not Registered");
                    return View(user);
                }
            }
            return View(user);

        }
        [HttpGet]
        public IActionResult Index()
        {
            var userIdSession = HttpContext.Session.GetInt32("UserId");
            int userId = userIdSession.Value;
            List<ContactList> contacts = _apiService.GetContacts(userId);
            return View(contacts);
        }
    }
}
