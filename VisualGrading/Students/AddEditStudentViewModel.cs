#region Header

// +===========================================================================+
// Visual Grading Source Code
// 
// Copyright (C) 2016-2017 Piotr Mikolajczyk
// 
// 2017-03-15
// AddEditStudentViewModel.cs
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

namespace VisualGrading.Students
{
    public class AddEditStudentViewModel : BaseViewModel
    {
        #region Fields

        private readonly IBusinessManager _businessManager;

        private IDataManager _dataManager;

        private Student _editingStudent;

        private bool _editMode;

        private SimpleEditableStudent _Student;

        #endregion

        #region Constructors

        public AddEditStudentViewModel()
        {
            _dataManager = ContainerHelper.Container.Resolve<IDataManager>();
            _businessManager = ContainerHelper.Container.Resolve<IBusinessManager>();
            CancelCommand = new RelayCommand(OnCancel);
            SaveCommand = new RelayCommand(OnSave, CanSave);
        }

        #endregion

        #region Properties

        public SimpleEditableStudent EditingStudent
        {
            get { return _Student; }
            set { SetProperty(ref _Student, value); }
        }

        public bool EditMode
        {
            get { return _editMode; }
            set { SetProperty(ref _editMode, value); }
        }

        public RelayCommand CancelCommand { get; private set; }

        public RelayCommand SaveCommand { get; }

        #endregion

        #region Public Methods

        public void SetStudent(Student student)
        {
            _editingStudent = student;
            if (EditingStudent != null) EditingStudent.ErrorsChanged -= RaiseCanExecuteChanged;
            EditingStudent = new SimpleEditableStudent();
            EditingStudent.ErrorsChanged += RaiseCanExecuteChanged;
            CopyStudent(student, EditingStudent);
        }

        #endregion

        #region Private Methods

        private bool CanSave()
        {
            return !EditingStudent.HasErrors;
        }

        private void CopyStudent(Student source, SimpleEditableStudent destination)
        {
            destination.ID = source.ID;

            destination.FirstName = source.FirstName;
            destination.LastName = source.LastName;
            destination.Nickname = source.Nickname;
            destination.EmailAddress = source.EmailAddress;
            destination.ParentEmailAddress = source.ParentEmailAddress;
        }

        private void OnCancel()
        {
            Done();
        }

        private async void OnSave()
        {
            UpdateStudent(EditingStudent, _editingStudent);
            if (EditMode)
                await _businessManager.UpdateStudentAsync(_editingStudent);
            else
                await _businessManager.InsertStudentAsync(_editingStudent);
            Done();
        }

        private void RaiseCanExecuteChanged(object sender, EventArgs e)
        {
            SaveCommand.RaiseCanExecuteChanged();
        }

        private void UpdateStudent(SimpleEditableStudent source, Student destination)
        {
            destination.ID = source.ID;
            destination.FirstName = source.FirstName;
            destination.LastName = source.LastName;
            destination.Nickname = source.Nickname;
            destination.EmailAddress = source.EmailAddress;
            destination.ParentEmailAddress = source.ParentEmailAddress;
        }

        #endregion

        public event Action Done = delegate { };
    }
}