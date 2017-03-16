#region Header

// +===========================================================================+
// Visual Grading Source Code
// 
// Copyright (C) 2016-2017 Piotr Mikolajczyk
// 
// 2017-03-15
// PasswordBoxBindingBehavior.cs
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

using System.Security;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Interactivity;

#endregion

namespace VisualGrading.Settings
{
    public class PasswordBoxBindingBehavior : Behavior<PasswordBox>
    {
        #region Fields

        public static readonly DependencyProperty PasswordProperty =
            DependencyProperty.Register("Password", typeof(SecureString), typeof(PasswordBoxBindingBehavior),
                new PropertyMetadata(null));

        #endregion

        #region Properties

        public SecureString Password
        {
            get { return (SecureString) GetValue(PasswordProperty); }
            set { SetValue(PasswordProperty, value); }
        }

        #endregion

        #region Private Methods

        protected override void OnAttached()
        {
            AssociatedObject.PasswordChanged += OnPasswordBoxValueChanged;
        }

        private void OnPasswordBoxValueChanged(object sender, RoutedEventArgs e)
        {
            var binding = BindingOperations.GetBindingExpression(this, PasswordProperty);
            if (binding != null)
            {
                var property = binding.DataItem.GetType().GetProperty(binding.ParentBinding.Path.Path);
                if (property != null)
                    property.SetValue(binding.DataItem, AssociatedObject.SecurePassword, null);
            }
        }

        #endregion
    }
}