﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Repositories.IRepositories
{
    public interface IUnitOfWork
    {
        IBranch Branch { get; }
        IRating Rating { get; }
        IVacancy Vacancy { get; }
        IProject Project { get; }
        IContact Contact { get; }
        IUserBranch UserBranch { get; }
        ICategory Category { get; }
        ICategoryBranch CategoryBranch { get; }               
        IService Service { get; }        
        ISession Session { get; }
        ITestimonial Testimonial { get; }
        IUser User { get; }
        IClientDetail ClientDetail { get; }
        IUserDetail UserDetail { get; }
        Task<int> Save();
        void ClearTracking();
    }
}