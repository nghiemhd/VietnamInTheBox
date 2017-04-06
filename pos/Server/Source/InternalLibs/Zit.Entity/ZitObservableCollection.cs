using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Zit.Entity
{
    [Serializable]
    public class ZitObservableCollection<T> : ObservableCollection<T> where T : INotifyPropertyChanged
    {
        public ZitObservableCollection()
            :base()
        {
            
        }

        public ZitObservableCollection(IEnumerable<T> init)
	    {
            foreach (var item in init)
                this.Add(item);
	    }

        protected override void OnCollectionChanged(System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {            

            if (e.OldItems != null)
            {
                foreach (var item in e.OldItems)
                {
                    //Unscribe
                    var i = item as INotifyPropertyChanged;
                    i.PropertyChanged -= i_PropertyChanged;
                }
            }

            if (e.NewItems != null)
            {
                foreach (var item in e.NewItems)
                {
                    //Unscribe
                    var i = item as INotifyPropertyChanged;
                    i.PropertyChanged += i_PropertyChanged;
                }
            }

            base.OnCollectionChanged(e);
        }

        void i_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            NotifyCollectionChangedEventArgs ee = new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Reset);
            OnCollectionChanged(ee);
        }
    }
}
