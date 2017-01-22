using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net.Mail;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Practices.Unity;
using VisualGrading.Business;
using VisualGrading.DataAccess;
using VisualGrading.Helpers;
using VisualGrading.Students;
using VisualGrading.Presentation;

namespace VisualGrading.Settings
{
    public class SettingsViewModel : BaseViewModel
    {
        #region Constructor

        public SettingsViewModel()
        {
            _dataManager = ContainerHelper.Container.Resolve<IDataManager>();
            _businessManager = ContainerHelper.Container.Resolve<IBusinessManager>();
            CancelCommand = new RelayCommand(OnCancel);
            SendTestEmailCommand = new RelayCommand(OnSendTestEmail);

            SaveCommand = new RelayCommand(OnSave, CanSave);
            SettingsProfile = _businessManager.GetSettingsProfileWithoutPassword();
        }

        #endregion

        #region Properties

        private IBusinessManager _businessManager;

        private IDataManager _dataManager;

        public SettingsProfile SettingsProfile { get; set; }

        public RelayCommand CancelCommand { get; private set; }

        public RelayCommand SendTestEmailCommand { get; private set; }
        
        public RelayCommand SaveCommand { get; }

        public event Action Done = delegate { };

        #endregion

        #region Methods
        

        private void RaiseCanExecuteChanged(object sender, EventArgs e)
        {
            SaveCommand.RaiseCanExecuteChanged();
        }

        private bool CanSave()
        {
            return !SettingsProfile.HasErrors;
        }

        private async void OnSave()
        {
            await _businessManager.UpdateSettingsProfileAsync(SettingsProfile);

            //force update so ID is set for new entries
            if (SettingsProfile.ID == 0)
            SettingsProfile = _businessManager.GetSettingsProfileWithoutPassword();

            Done();
        }

        private void OnCancel()
        {
            Done();
        }

        private void OnSendTestEmail()
        {
            _businessManager.SendEmail(SettingsProfile);
        }

        #endregion
    }
}
