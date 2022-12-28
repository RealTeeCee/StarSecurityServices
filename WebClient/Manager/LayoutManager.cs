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
            if(localeId == null)
            {
                var categoryNoBranch = await unitOfWork.CategoryBranch.GetAll(x => x.BranchId ==1 ,includeProperties: "Category,Branch");
                return categoryNoBranch.ToList();
            }
            var categoryBranch = await unitOfWork.CategoryBranch.GetAll(x=>x.BranchId==localeId,includeProperties: "Category,Branch");
            return categoryBranch.ToList();
        }
    }
}
