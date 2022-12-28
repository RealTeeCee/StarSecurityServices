using DataAccess.Repositories.IRepositories;
using Microsoft.AspNetCore.Mvc;
using System.Xml.Linq;

namespace WebClient.ViewComponents
{
    [ViewComponent(Name = "BranchList")]

    public class BranchViewComponent : ViewComponent
    {
        private readonly IUnitOfWork unitOfWork;
        public BranchViewComponent(IUnitOfWork unitOfWork)
        {
            this.unitOfWork = unitOfWork;

        }
        public async Task<IViewComponentResult> InvokeAsync()
        {
            try
            {
                var List = await unitOfWork.Branch.GetAll();
                return View("BranchList", List);
            }
            catch (Exception)
            {

                throw;
            }
        }
    }

}
