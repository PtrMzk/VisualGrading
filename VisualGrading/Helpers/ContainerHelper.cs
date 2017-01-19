using Microsoft.Practices.Unity;
using VisualGrading.Business;
using VisualGrading.DataAccess;
using VisualGrading.Model.Data;
using VisualGrading.ViewModelHelpers;

namespace VisualGrading.Helpers
{
    public static class ContainerHelper
    {
        #region Fields

        #endregion

        #region Constructors

        static ContainerHelper()
        {
            Container = new UnityContainer();

            Container.RegisterInstance<IUnitOfWork>(new EFUnitOfWork());

            Container.RegisterInstance<IDataManager>(DataManager.Instance);

            Container.RegisterInstance<IBusinessManager>(new BusinessManager());

            Container.RegisterInstance<IFileDialog>(new FileDialog());
        }

        #endregion

        #region Properties

        public static IUnityContainer Container { get; }

        #endregion
    }
}