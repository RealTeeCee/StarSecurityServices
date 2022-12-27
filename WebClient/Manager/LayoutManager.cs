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
        public async Task<List<Category>> CategoryList()
        {
            var categoryList = await unitOfWork.Category.GetAll();
            return (List<Category>)categoryList;
        }
    }
}
