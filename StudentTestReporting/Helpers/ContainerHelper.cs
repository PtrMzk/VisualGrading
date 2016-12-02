using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Practices.Unity;
using StudentTestReporting.Students;
using StudentTestReporting.Tests;

namespace StudentTestReporting.Helpers
{
    public static class ContainerHelper
    {
        private static IUnityContainer _container;
        static ContainerHelper()
        {
            _container = new UnityContainer();
            _container.RegisterType<ITestManager, TestManager>(new ContainerControlledLifetimeManager());
            _container.RegisterType<IStudentManager, StudentManager>(new ContainerControlledLifetimeManager());
        }

        public static IUnityContainer Container
        {
            get { return _container; }
        }
    }
}
