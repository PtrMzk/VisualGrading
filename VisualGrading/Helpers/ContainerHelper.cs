using Microsoft.Practices.Unity;
using VisualGrading.Business;
using VisualGrading.DataAccess;
using VisualGrading.Emails;
using VisualGrading.Model.Data;
using VisualGrading.Presentation;

namespace VisualGrading.Helpers
{
    public static class ContainerHelper
    {
        #region Constructors

        static ContainerHelper()
        {
            Container = new UnityContainer();

            Container.RegisterInstance<IUnitOfWork>(new EFUnitOfWork());

            Container.RegisterInstance<IDataManager>(DataManager.Instance);

            //Container.RegisterInstance<IEmailManager>(new EmailManager());

            Container.RegisterInstance<IBusinessManager>(new BusinessManager());

            Container.RegisterInstance<IFileDialog>(new FileDialog());
        }

        #endregion

        #region Properties

        public static IUnityContainer Container { get; }

        #endregion
    }
}