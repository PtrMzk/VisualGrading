// From: https://github.com/ovidiaconescu/ObservableCollectionEx/blob/master/ObservableCollectionEx/ObservableCollectionEx/ObservableCollectionEx.cs
// Extends ObservableCollection to have events that fire on properties
// Modified to support additional constructors, and to fire the event

using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using StudentTestReporting.Annotations;

namespace VisualGrading.Presentation
{
    public class ObservableCollectionExtended<T> : ObservableCollection<T> where T : INotifyPropertyChanged
    {
        #region Constructors
        public ObservableCollectionExtended()
        {
        }

        public ObservableCollectionExtended(List<T> list) : this()
        {
           foreach (var x in list)
                this.Add(x);
        }

        public ObservableCollectionExtended(IEnumerable<T> collection) : this()
        {
            foreach (var x in collection)
                this.Add(x);
        }
        #endregion

        protected override void OnCollectionChanged(NotifyCollectionChangedEventArgs e)
        {
            Unsubscribe(e.OldItems);
            Subscribe(e.NewItems);
            base.OnCollectionChanged(e);
        }

        protected override void ClearItems()
        {
            foreach (T element in this)
                element.PropertyChanged -= ContainedElementChanged;

            base.ClearItems();
        }

        private void Subscribe(IList iList)
        {
            if (iList != null)
            {
                foreach (T element in iList)
                    element.PropertyChanged += ContainedElementChanged;
            }
        }

        private void Unsubscribe(IList iList)
        {
            if (iList != null)
            {
                foreach (T element in iList)
                    element.PropertyChanged -= ContainedElementChanged;
            }
        }

        private void ContainedElementChanged(object sender, PropertyChangedEventArgs e)
        {
            //var ex = new PropertyChangedEventArgsExtended(e.PropertyName, sender);
            OnCollectionPropertyChanged(sender, sender.GetType().ToString());
        }

        public event PropertyChangedEventHandler CollectionPropertyChanged;

        [NotifyPropertyChangedInvocator]
        protected virtual void OnCollectionPropertyChanged(object sender, [CallerMemberName] string propertyName = null)
        {
            CollectionPropertyChanged?.Invoke(sender, new PropertyChangedEventArgs(propertyName));
        }
    }
}