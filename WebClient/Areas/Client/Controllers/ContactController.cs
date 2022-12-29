using DataAccess.Data;
using DataAccess.Repositories.IRepositories;
using Microsoft.AspNetCore.Mvc;

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
        public IActionResult HandleReceivedMessage([FromForm] string data)
        {
            return Ok();
        }
    }
}
