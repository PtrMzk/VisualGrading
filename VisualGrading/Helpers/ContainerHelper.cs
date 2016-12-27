using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.Practices.Unity;
using VisualGrading.Business;
using VisualGrading.DataAccess;
using VisualGrading.Grades;
using VisualGrading.Model.Data;
using VisualGrading.Students;
using VisualGrading.Tests;

namespace VisualGrading.Helpers
{
    public static class ContainerHelper
    {
        private static IUnityContainer _container;

        static ContainerHelper()
        {
            _container = new UnityContainer();
            
            _container.RegisterInstance<IUnitOfWork>(new EFUnitOfWork());

            _container.RegisterInstance<IDataManager>(DataManager.Instance);

            _container.RegisterInstance<IBusinessManager>(new BusinessManager());

        }

        public static IUnityContainer Container
        {
            get { return _container; }
        }

    }
}
