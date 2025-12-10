using Microsoft.AspNetCore.Mvc;
using UserPortalValdiationsDBContext.Constants;
using UserPortalValdiationsDBContext.Filters;
using UserPortalValdiationsDBContext.Interfaces;
using UserPortalValdiationsDBContext.Models;

namespace UserPortalValdiationsDBContext.Controllers
{
    [ServiceFilter(typeof(LoggingActionFilter))]
    [ServiceFilter(typeof(AuditingFilter))]
    [ServiceFilter(typeof(ErrorHandlingFilter))]
    public class ContactController : Controller
    {
        private readonly IContactService ContactService;
        private readonly IEmailService EmailService;

        public ContactController(IContactService contactService, IEmailService emailService)
        {
            ContactService = contactService;
            EmailService = emailService;
        }

        [ServiceFilter(typeof(ResultCacheFilter))]   // Cache contact list for performance
        public IActionResult Index()
        {
            ViewBag.Contacts = ContactService.GetAllContacts();
            return View();
        }

        [HttpPost]
        [ServiceFilter(typeof(ActionValidationFilter))]
        public IActionResult Index(ContactModel model)
        {
            ContactService.AddContact(model);

            try
            {
                EmailService.SendEmail(
                    model.Email,
                    "Thank you for contacting us!",
                    $"<p>Hi,</p><p>Your message was received:</p><p><b>{model.Message}</b></p><p>We will respond soon.</p>"
                );
            }
            catch (Exception ex)
            {
                ViewBag.Message = string.Format(Messages.ContactEmailFail, ex.Message);
                ViewBag.Contacts = ContactService.GetAllContacts();
                return View();
            }

            ViewBag.Message = Messages.ContactSuccess;
            ViewBag.Contacts = ContactService.GetAllContacts();
            return View();
        }
    }
}

/*
 * using Microsoft.AspNetCore.Mvc;
using UserPortalValdiationsDBContext.Constants; 
using UserPortalValdiationsDBContext.Interfaces;
using UserPortalValdiationsDBContext.Models;

namespace UserPortalValdiationsDBContext.Controllers
{
    public class ContactController : Controller
    {
        private readonly IContactService ContactService;
        private readonly IEmailService EmailService;

        public ContactController(IContactService contactService, IEmailService emailService)
        {
            ContactService = contactService;
            EmailService = emailService;
        }

        public IActionResult Index()
        {
            ViewBag.Contacts = ContactService.GetAllContacts();
            return View();
        }

        [HttpPost]
        public IActionResult Index(ContactModel model)
        {
            if (!ModelState.IsValid)
            {
                ViewBag.Contacts = ContactService.GetAllContacts();
                return View(model);
            }

            // Save contact message in DB
            ContactService.AddContact(model);

            try
            {
                EmailService.SendEmail(
                    model.Email,
                    "Thank you for contacting us!",
                    $"<p>Hi,</p><p>Your message was received:</p><p><b>{model.Message}</b></p><p>We will respond soon.</p>"
                );
            }
            catch (Exception ex)
            {
                ViewBag.Message = string.Format(Messages.ContactEmailFail, ex.Message);         //printing error message from the constant file.
                ViewBag.Contacts = ContactService.GetAllContacts();
                return View();
            }

            ViewBag.Message = Messages.ContactSuccess;              //printing success messge using constants.
            ViewBag.Contacts = ContactService.GetAllContacts();
            return View();
        }
    }
}







/*using Microsoft.AspNetCore.Mvc;
using UserPortalValdiationsDBContext.Interfaces;
using UserPortalValdiationsDBContext.Models;
using UserPortalValdiationsDBContext.Constants;
namespace UserPortalValdiationsDBContext.Controllers
{
    public class ContactController : Controller
    {
        private readonly IContactService ContactService;
        private readonly IEmailService EmailService;

        public ContactController(IContactService contactService, IEmailService emailService)
        {
            ContactService = contactService;
            EmailService = emailService;
        }

        public IActionResult Index()
        {
            ViewBag.Contacts = ContactService.GetAllContacts();
            return View();
        }

        [HttpPost]
        public IActionResult Index(ContactModel model)
        {
            if (!ModelState.IsValid)
            {
                ViewBag.Contacts = ContactService.GetAllContacts();
                return View(model);
            }

            // Save contact message in DB
            ContactService.AddContact(model);

            try
            {
                EmailService.SendEmail(
                    model.Email,
                    "Thank you for contacting us!",
                    $"<p>Hi,</p><p>Your message was received:</p><p><b>{model.Message}</b></p><p>We will respond soon.</p>"
                );
            }
            catch (Exception ex)
            {
                ViewBag.Message = "Message saved but failed to send email: " + ex.Message;
                ViewBag.Contacts = ContactService.GetAllContacts();
                return View();
            }

            ViewBag.Message = "Message sent successfully! A confirmation email has been sent.";
            ViewBag.Contacts = ContactService.GetAllContacts();
            return View();
        }
    }
}
*/