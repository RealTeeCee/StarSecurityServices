using DataAccess.Repositories.IRepositories;
using Microsoft.AspNetCore.Mvc;
using Models;

namespace WebClient.Areas.Client.Controllers
{
    [Area("Client")]
    public class TestimonialController : Controller
    {
        private readonly IUnitOfWork unitOfWork;

        public TestimonialController(IUnitOfWork unitOfWork)
        {
            this.unitOfWork = unitOfWork;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Edit(Testimonial model)
        {
            return View(model);
        }
    }
}
