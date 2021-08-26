using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel;

namespace GrabberClient.Internals
{
    public class ObservableQueue<T> : Queue<T>, INotifyCollectionChanged, INotifyPropertyChanged
    {
        #region Events

        public event NotifyCollectionChangedEventHandler CollectionChanged;
        protected event PropertyChangedEventHandler PropertyChanged;
        event PropertyChangedEventHandler INotifyPropertyChanged.PropertyChanged
        {
            add { this.PropertyChanged += value; }
            remove { this.PropertyChanged -= value; }
        }

        #endregion

        #region Constructors

        public ObservableQueue()
        {
        }

        public ObservableQueue(IEnumerable<T> collection)
        {
            foreach (var item in collection)
            {
                base.Enqueue(item);
            }
        }

        public ObservableQueue(List<T> list)
        {
            foreach (var item in list)
            {
                base.Enqueue(item);
            }
        }

        #endregion

        #region Base methods

        public new void Clear()
        {
            base.Clear();
            this.OnCollectionChanged(new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Reset));
        }

        public new void Enqueue(T element)
        {
            base.Enqueue(element);
            this.OnPropertyChanged(new PropertyChangedEventArgs(nameof(Count)));
            this.OnCollectionChanged(new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Add, element));
        }

        public new T Dequeue()
        {
            var item = base.Dequeue();
            this.OnPropertyChanged(new PropertyChangedEventArgs(nameof(Count)));
            this.OnCollectionChanged(new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Remove, item, default(int)));

            return item;
        }

        #endregion

        #region Event handlers

        protected virtual void OnCollectionChanged(NotifyCollectionChangedEventArgs e)
        {
            this.RaiseCollectionChanged(e);
        }

        protected virtual void OnPropertyChanged(PropertyChangedEventArgs e)
        {
            this.RaisePropertyChanged(e);
        }

        private void RaiseCollectionChanged(NotifyCollectionChangedEventArgs e)
        {
            if (this.CollectionChanged is not null)
                this.CollectionChanged(this, e);
        }

        private void RaisePropertyChanged(PropertyChangedEventArgs e)
        {
            if (this.PropertyChanged is not null)
                this.PropertyChanged(this, e);
        }

        #endregion
    }
}
