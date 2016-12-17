//https://codefizzle.wordpress.com/2012/07/26/correct-use-of-repository-and-unit-of-work-patterns-in-asp-net-mvc/

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VisualGrading.Model.Data;

namespace VisualGrading.Model.Repositories
{
    public interface IUnitOfWork : IDisposable
    {
        IRepository<Test> TestRepository { get; }
        IRepository<Student> StudentRepository { get; }
        IRepository<Grade> GradeRepository { get; }

        void Commit();
    }
}
