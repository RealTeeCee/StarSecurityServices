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
            var categoryBranch = await unitOfWork.CategoryBranch.GetAll(x => x.BranchId == localeId && x.Category.Slug != "vacancy-service", includeProperties: "Category,Branch");
            return categoryBranch.ToList();
        }

        public async Task<bool> HasVacancy(int? localeId)
        {
            var categoryBranch = await unitOfWork.CategoryBranch.GetAll(x => x.BranchId == localeId, includeProperties: "Category,Branch");
            foreach (var item in categoryBranch)
            {
                if(item.Category.Slug == "vacancy-service")
                {
                    return true;
                }
            }
            return false;
        }

        public async Task<bool> IsRated(long projectId)
        {
            var rating = await unitOfWork.Rating.GetFirstOrDefault(x => x.ProjectId == projectId);
            if(rating != null)
            {
                return true;
            }
            return false;
        }
    }
}
