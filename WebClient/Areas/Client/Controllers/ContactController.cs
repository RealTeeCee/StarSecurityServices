using DataAccess.Data;
using DataAccess.Repositories.IRepositories;
using Microsoft.AspNetCore.Mvc;
using Models;
using System.Threading.Tasks;

namespace WebClient.Areas.Client.Controllers
{
    [Area("Client")]
    [Route("contact")]
    public class ContactController : Controller
    {
        private readonly StarSecurityDbContext context;
        private readonly IUnitOfWork unitOfWork;
        private readonly IWebHostEnvironment env;

        public ContactController(IUnitOfWork unitOfWork, StarSecurityDbContext context, IWebHostEnvironment env)
        {
            this.unitOfWork = unitOfWork;
            this.context = context;
            this.env = env;
        }
        public async Task<IActionResult> Index()
        {
            var branches = await unitOfWork.Branch.GetAll();

            return View(branches);
        }

        [Route("sendmessage")]
        public async Task<IActionResult> HandleReceivedMessage([FromBody] Contact contact)
        {
            if (contact == null)
            {
                return RedirectToAction("Index", "Error", new { area = "Client" });
            }

            try
            {
                await unitOfWork.Contact.Add(contact);
                await unitOfWork.Save();
            }
            catch
            {
                return RedirectToAction("Index", "Error", new { area = "Client" });
            }

            return Ok();
        }
    }
}
