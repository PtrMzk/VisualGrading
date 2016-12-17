using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.Practices.Unity;
using VisualGrading.DataAccess;
using VisualGrading.Grades;
using VisualGrading.Model.Data;
using VisualGrading.Model.Repositories;
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

            _container.RegisterInstance<ITestRepository>(TestRepository.Instance);

            _container.RegisterInstance<IStudentRepository>(StudentRepository.Instance);

            _container.RegisterInstance<IGradeRepository>(GradeRepository.Instance);



            //_container.RegisterInstance<IDataManager>(DataManager.Instance, new ContainerControlledLifetimeRepository());
            //_container.RegisterInstance<ITestRepository>(TestRepository.Instance, new ContainerControlledLifetimeRepository());
            //_container.RegisterInstance<IStudentRepository>(StudentRepository.Instance, new ContainerControlledLifetimeRepository());

            //_container.RegisterInstance<IDataManager>(DataManager.Instance, new ContainerControlledLifetimeRepository());

            //_container.RegisterType<IDataManager, DataManager>(new ContainerControlledLifetimeRepository());
            //_container.RegisterType<ITestRepository, TestRepository>(new ContainerControlledLifetimeRepository());
            //_container.RegisterType<IStudentRepository, StudentRepository>(new ContainerControlledLifetimeRepository());
            //_container.RegisterType<IGradeRepository, GradeRepository>(new ContainerControlledLifetimeRepository());

        }

        public static IUnityContainer Container
        {
            get { return _container; }
        }

    }
}
