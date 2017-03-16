#region Header

// +===========================================================================+
// Visual Grading Source Code
// 
// Copyright (C) 2016-2017 Piotr Mikolajczyk
// 
// 2017-03-15
// RelayCommand.cs
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
using System.Windows.Input;

#endregion

namespace VisualGrading.Presentation
{
    public class RelayCommand : ICommand
    {
        #region Fields

        private readonly Func<bool> _TargetCanExecuteMethod;
        private readonly Action _TargetExecuteMethod;

        #endregion

        #region Constructors

        public RelayCommand(Action executeMethod)
        {
            _TargetExecuteMethod = executeMethod;
        }

        public RelayCommand(Action executeMethod, Func<bool> canExecuteMethod)
        {
            _TargetExecuteMethod = executeMethod;
            _TargetCanExecuteMethod = canExecuteMethod;
        }

        #endregion

        #region Public Methods

        public bool CanExecute(object parameter)
        {
            if (_TargetCanExecuteMethod != null)
                return _TargetCanExecuteMethod();

            if (_TargetExecuteMethod != null)
                return true;

            return false;
        }

        public void Execute(object parameter)
        {
            if (_TargetExecuteMethod != null)
                _TargetExecuteMethod();
        }

        public void RaiseCanExecuteChanged()
        {
            CanExecuteChanged(this, EventArgs.Empty);
        }

        #endregion

        #region Interface Implementations

        public event EventHandler CanExecuteChanged = delegate { };

        #endregion
    }

    //generic implementation
    public class RelayCommand<T> : ICommand
    {
        #region Fields

        private readonly Func<T, bool> _TargetCanExecuteMethod;
        private readonly Action<T> _TargetExecuteMethod;

        #endregion

        #region Constructors

        public RelayCommand(Action<T> executeMethod)
        {
            _TargetExecuteMethod = executeMethod;
        }

        public RelayCommand(Action<T> executeMethod, Func<T, bool> canExecuteMethod)
        {
            _TargetExecuteMethod = executeMethod;
            _TargetCanExecuteMethod = canExecuteMethod;
        }

        #endregion

        #region Public Methods

        public void RaiseCanExecuteChanged()
        {
            CanExecuteChanged(this, EventArgs.Empty);
        }

        #endregion

        #region Private Methods

        bool ICommand.CanExecute(object parameter)
        {
            if (_TargetCanExecuteMethod != null)
            {
                var tparm = (T) parameter;
                return _TargetCanExecuteMethod(tparm);
            }
            if (_TargetExecuteMethod != null)
                return true;
            return false;
        }

        void ICommand.Execute(object parameter)
        {
            if (_TargetExecuteMethod != null)
                _TargetExecuteMethod((T) parameter);
        }

        #endregion

        #region Interface Implementations

        // Beware - should use weak references if command instance lifetime is longer than lifetime of UI objects that get hooked up to command
        // Prism commands solve this in their implementation
        public event EventHandler CanExecuteChanged = delegate { };

        #endregion
    }
}