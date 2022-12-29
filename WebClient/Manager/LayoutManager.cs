using DataAccess.Repositories.IRepositories;
using Models;

namespace WebClient.Manager
{
    public class LayoutManager
    {
        private readonly IUnitOfWork unitOfWork;

        public LayoutManager(IUnitOfWork unitOfWork)
        {
            this.unitOfWork = unitOfWork;
        }
        public async Task<List<CategoryBranch>> CategoryBranchList(int? localeId)
        {
            var categoryBranch = await unitOfWork.CategoryBranch.GetAll(x => x.BranchId == localeId, includeProperties: "Category,Branch");
            return categoryBranch.ToList();
        }
    }
}
