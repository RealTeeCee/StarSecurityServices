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
        public async Task<IActionResult> Index(int? localeId)
        {
            if (localeId == null)
            {
                CategoryViewModel branches = new CategoryViewModel();
                branches.Branches = (List<Branch>)await unitOfWork.Branch.GetAll();
                return View(branches);
            }
            CategoryViewModel model = new CategoryViewModel();
            model.Branch = await unitOfWork.Branch.GetFirstOrDefault(x => x.Id == localeId);
            model.Branches = (List<Branch>)await unitOfWork.Branch.GetAll();
            return View(model);
        }

        [Route("send-message")]
        public async Task<IActionResult> HandleReceivedMessage([FromBody] Contact contact)
        {
            if (contact == null)
            {
                return RedirectToAction("PageNotFound", "Error", new { area = "Client" });
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

        [Route("find-shortest-branch")]
        public async Task<IActionResult> FindShortestBranch(string latituteUser, string longitudeUser)
        {
            try
            {
                var branch = await unitOfWork.Branch.GetAll();
                if (branch != null)
                {
                    if (!string.IsNullOrEmpty(latituteUser) && !string.IsNullOrEmpty(longitudeUser))
                    {
                        Location currentUserLocation = new Location()
                        {
                            Latitude = double.Parse(latituteUser),
                            Longitude = double.Parse(longitudeUser)

                        };
                        List<Location> storeLocation = new List<Location>();
                        List<double> distances = new List<double>();
                        foreach (var item in branch)
                        {
                            Location branchLocation = new Location()
                            {
                                Latitude = double.Parse(item.Latitude),
                                Longitude = double.Parse(item.Longitude),
                                BranchId = (int)item.Id,
                                BranchName = item.Name
                            };
                            branchLocation.DistanceToUser = CalculateDistance(branchLocation, currentUserLocation);
                            storeLocation.Add(branchLocation);
                        }

                        var shortestDistance = storeLocation.Min(x => x.DistanceToUser);
                        var selectbranch = storeLocation.FirstOrDefault(x => x.DistanceToUser == shortestDistance);
                        if (selectbranch != null)
                        {
                            var shortestBranch = branch.FirstOrDefault(x => x.Id == selectbranch.BranchId);
                            if (shortestBranch != null)
                            {
                                return Json(new
                                {
                                    data = shortestBranch
                                });
                            }
                            else
                            {
                                return Json(new
                                {
                                    data = ""
                                });
                            }
                        }
                    }
                }
                
                return BadRequest();
            }
            catch (Exception)
            {

                return RedirectToAction("Index", "Error", new { area = "Client" });
            }
        }

        [Route("calculate-distance")]
        public double CalculateDistance(Location location1, Location currentUserLocation)
        {
            try
            {
                var d1 = location1.Latitude * (Math.PI / 180.0);
                var num1 = location1.Longitude * (Math.PI / 180.0);
                var d2 = currentUserLocation.Latitude * (Math.PI / 180.0);
                var num2 = currentUserLocation.Longitude * (Math.PI / 180.0) - num1;
                var d3 = Math.Pow(Math.Sin((d2 - d1) / 2.0), 2.0) +
                         Math.Cos(d1) * Math.Cos(d2) * Math.Pow(Math.Sin(num2 / 2.0), 2.0);
                return 6376500.0 * (2.0 * Math.Atan2(Math.Sqrt(d3), Math.Sqrt(1.0 - d3)));
            }
            catch (Exception)
            {
                return 0;
            }
        }
    }
}
