#region Header

// +===========================================================================+
// Visual Grading Source Code
// 
// Copyright (C) 2016-2017 Piotr Mikolajczyk
// 
// 2017-03-15
// SettingsViewModel.cs
// 
// Permission is hereby granted, free of charge, to any person obtaining a
// copy of this software and associated documentation files (the
// "Software"), to deal in the Software without restriction, including
// without limitation the rights to use, copy, modify, merge, publish,
// distribute, sublicense, and/or sell copies of the Software, and to
// permit persons to whom the Software is furnished to do so, subject to
// the following conditions:
// 
// The above copyright notice and this permission notice shall be included
// in all copies or substantial portions of the Software.
// 
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS
// OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF
// MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT.
// IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY
// CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT,
// TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH THE
// SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.
//  +===========================================================================+

#endregion

#region Namespaces

using System;
using Microsoft.Practices.Unity;
using VisualGrading.Business;
using VisualGrading.DataAccess;
using VisualGrading.Helpers;
using VisualGrading.Presentation;

#endregion

namespace VisualGrading.Settings
{
    public class SettingsViewModel : BaseViewModel
    {
        #region Fields

        private readonly IBusinessManager _businessManager;

        private IDataManager _dataManager;

        #endregion

        #region Constructors

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

        public SettingsProfile SettingsProfile { get; set; }

        public RelayCommand CancelCommand { get; private set; }

        public RelayCommand SendTestEmailCommand { get; private set; }

        public RelayCommand SaveCommand { get; }

        #endregion

        #region Private Methods

        private bool CanSave()
        {
            return !SettingsProfile.HasErrors;
        }

        private void OnCancel()
        {
            Done();
        }

        private async void OnSave()
        {
            await _businessManager.UpdateSettingsProfileAsync(SettingsProfile);

            //force update so ID is set for new entries
            if (SettingsProfile.ID == 0)
                SettingsProfile = _businessManager.GetSettingsProfileWithoutPassword();

            Done();
        }

        private void OnSendTestEmail()
        {
            _businessManager.SendTestEmail(SettingsProfile);
        }

        private void RaiseCanExecuteChanged(object sender, EventArgs e)
        {
            SaveCommand.RaiseCanExecuteChanged();
        }

        #endregion

        public event Action Done = delegate { };
    }
}