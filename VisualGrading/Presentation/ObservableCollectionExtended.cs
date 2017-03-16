#region Header

// +===========================================================================+
// Visual Grading Source Code
// 
// Copyright (C) 2016-2017 Piotr Mikolajczyk
// 
// 2017-03-15
// ObservableCollectionExtended.cs
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

using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using StudentTestReporting.Annotations;

#endregion

namespace VisualGrading.Presentation
{
    // From: https://github.com/ovidiaconescu/ObservableCollectionEx/blob/master/ObservableCollectionEx/ObservableCollectionEx/ObservableCollectionEx.cs
    // Extends ObservableCollection to have events that fire on properties
    // Modified to support additional constructors, and to fire the event

    public class ObservableCollectionExtended<T> : ObservableCollection<T> where T : INotifyPropertyChanged
    {
        #region Constructors

        public ObservableCollectionExtended()
        {
        }

        public ObservableCollectionExtended(List<T> list) : this()
        {
            foreach (var x in list)
                Add(x);
        }

        public ObservableCollectionExtended(IEnumerable<T> collection) : this()
        {
            foreach (var x in collection)
                Add(x);
        }

        #endregion

        #region Private Methods

        protected override void ClearItems()
        {
            foreach (var element in this)
                element.PropertyChanged -= ContainedElementChanged;

            base.ClearItems();
        }

        private void ContainedElementChanged(object sender, PropertyChangedEventArgs e)
        {
            //var ex = new PropertyChangedEventArgsExtended(e.PropertyName, sender);
            OnCollectionPropertyChanged(sender, sender.GetType().ToString());
        }

        protected override void OnCollectionChanged(NotifyCollectionChangedEventArgs e)
        {
            Unsubscribe(e.OldItems);
            Subscribe(e.NewItems);
            base.OnCollectionChanged(e);
        }

        [NotifyPropertyChangedInvocator]
        protected virtual void OnCollectionPropertyChanged(object sender, [CallerMemberName] string propertyName = null)
        {
            CollectionPropertyChanged?.Invoke(sender, new PropertyChangedEventArgs(propertyName));
        }

        private void Subscribe(IList iList)
        {
            if (iList != null)
                foreach (T element in iList)
                    element.PropertyChanged += ContainedElementChanged;
        }

        private void Unsubscribe(IList iList)
        {
            if (iList != null)
                foreach (T element in iList)
                    element.PropertyChanged -= ContainedElementChanged;
        }

        #endregion

        public event PropertyChangedEventHandler CollectionPropertyChanged;
    }
}