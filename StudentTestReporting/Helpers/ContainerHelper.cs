using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Practices.Unity;
using StudentTestReporting.DataAccess;
using VisualGrading.Grades;
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

            _container.RegisterInstance<IDataManager>(DataManager.Instance);

            _container.RegisterInstance<ITestManager>(TestManager.Instance);

            _container.RegisterInstance<IStudentManager>(StudentManager.Instance);

            if (GradeManager.Instance != null)
            _container.RegisterInstance<IGradeManager>(GradeManager.Instance);




            //_container.RegisterInstance<IDataManager>(DataManager.Instance, new ContainerControlledLifetimeManager());
            //_container.RegisterInstance<ITestManager>(TestManager.Instance, new ContainerControlledLifetimeManager());
            //_container.RegisterInstance<IStudentManager>(StudentManager.Instance, new ContainerControlledLifetimeManager());

            //_container.RegisterInstance<IDataManager>(DataManager.Instance, new ContainerControlledLifetimeManager());

            //_container.RegisterType<IDataManager, DataManager>(new ContainerControlledLifetimeManager());
            //_container.RegisterType<ITestManager, TestManager>(new ContainerControlledLifetimeManager());
            //_container.RegisterType<IStudentManager, StudentManager>(new ContainerControlledLifetimeManager());
            //_container.RegisterType<IGradeManager, GradeManager>(new ContainerControlledLifetimeManager());

        }

        public static IUnityContainer Container
        {
            get { return _container; }
        }
    }
}
