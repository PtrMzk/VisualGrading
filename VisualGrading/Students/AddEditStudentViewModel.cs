﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Practices.Unity;
using VisualGrading.Business;
using VisualGrading.DataAccess;
using VisualGrading.Helpers;
using VisualGrading.Presentation;

namespace VisualGrading.Students
{
    public class AddEditStudentViewModel : BaseViewModel
    {
        #region Constructor

        public AddEditStudentViewModel()
        {
            _dataManager = ContainerHelper.Container.Resolve<IDataManager>();
            _businessManager = ContainerHelper.Container.Resolve<IBusinessManager>();
            CancelCommand = new RelayCommand(OnCancel);
            SaveCommand = new RelayCommand(OnSave, CanSave);
        }

        #endregion

        #region Properties

        private IBusinessManager _businessManager;


        private IDataManager _dataManager;

        private Student _editingStudent;

        public SimpleEditableStudent EditingStudent
        {
            get { return _Student; }
            set { SetProperty(ref _Student, value); }
        }

        private SimpleEditableStudent _Student;

        private bool _editMode;

        public bool EditMode
        {
            get { return _editMode; }
            set { SetProperty(ref _editMode, value); }
        }

        public RelayCommand CancelCommand { get; private set; }

        public RelayCommand SaveCommand { get; }

        public event Action Done = delegate { };

        #endregion

        #region Methods

        public void SetStudent(Student student)
        {
            _editingStudent = student;
            if (EditingStudent != null) EditingStudent.ErrorsChanged -= RaiseCanExecuteChanged;
            EditingStudent = new SimpleEditableStudent();
            EditingStudent.ErrorsChanged += RaiseCanExecuteChanged;
            CopyStudent(student, EditingStudent);
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

        private void CopyStudent(Student source, SimpleEditableStudent destination)
        {
            destination.ID = source.ID;

            if (EditMode)
            {
                destination.FirstName = source.FirstName;
                destination.LastName = source.LastName;
                destination.Nickname = source.Nickname;
                destination.EmailAddress = source.EmailAddress;
                destination.ParentEmailAddress = source.ParentEmailAddress;

            }
        }

        private bool CanSave()
        {
            return !EditingStudent.HasErrors;
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

        private void OnCancel()
        {
            Done();
        }

        #endregion
    }
}