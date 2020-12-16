using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace NathanCarouselCalendar
{
    public class MonthViewModel : INotifyPropertyChanged
    {
        public readonly IList<Month> source;
        public ObservableCollection<Month> Months { get; set; }

        public MonthViewModel()
        {
            source = new List<Month>();
        }

        public event PropertyChangedEventHandler PropertyChanged;

        void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
