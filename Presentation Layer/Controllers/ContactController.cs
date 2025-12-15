using Microsoft.AspNetCore.Mvc;
using UserPortalValdiationsDBContext.Constants;
using UserPortalValdiationsDBContext.Filters;
using UserPortalValdiationsDBContext.Models;
using UserPortalValdiationsDBContext.Services.Interfaces;
<<<<<<< HEAD:Controllers/ContactController.cs

namespace UserPortalValdiationsDBContext.Controllers
{
    [ServiceFilter(typeof(LoggingActionFilter))]
    [ServiceFilter(typeof(AuditingFilter))]
    [ServiceFilter(typeof(ErrorHandlingFilter))]
    public class ContactController : Controller
    {
        private readonly IContactService _contactService;
        private readonly IEmailService _emailService;

        public ContactController(
            IContactService contactService,
            IEmailService emailService)
        {
            _contactService = contactService;
            _emailService = emailService;
        }

        [ServiceFilter(typeof(ResultCacheFilter))]
        public IActionResult Index()
        {
            ViewBag.Contacts = _contactService.GetAllContacts();
            return View();
        }

        [HttpPost]
        [ServiceFilter(typeof(ActionValidationFilter))]
        public async Task<IActionResult> Index(ContactModel model)
        {
            _contactService.AddContact(model);

            try
            {
                await _emailService.SendEmailAsync(
                    model.Email,
                    "Thank you for contacting us!",
                    $"""
                    <p>Hi,</p>
                    <p>Your message was received:</p>
                    <p><b>{model.Message}</b></p>
                    <p>We will respond soon.</p>
                    """
                );
            }
            catch (Exception ex)
            {
                ViewBag.Message = string.Format(Messages.ContactEmailFail, ex.Message);
                ViewBag.Contacts = _contactService.GetAllContacts();
                return View();
            }

            ViewBag.Message = Messages.ContactSuccess;
            ViewBag.Contacts = _contactService.GetAllContacts();
            return View();
        }
    }
}















/*using Microsoft.AspNetCore.Mvc;
using UserPortalValdiationsDBContext.Constants;
using UserPortalValdiationsDBContext.Filters;
using UserPortalValdiationsDBContext.Interfaces;
using UserPortalValdiationsDBContext.Models;
using UserPortalValdiationsDBContext.Services.Interfaces;
=======
>>>>>>> 434e4f68b63bd11e3b7b38789add7bc53265a4dc:Presentation Layer/Controllers/ContactController.cs

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

*/