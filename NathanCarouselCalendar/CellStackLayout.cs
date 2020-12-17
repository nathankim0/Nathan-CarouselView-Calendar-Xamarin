using System;
using Xamarin.Forms;

namespace NathanCarouselCalendar
{
    public class CellStackLayout : StackLayout
    {
        public DateTime dateTime;

        public CellStackLayout()
        {
            Padding = 0;
            Margin = 0;
            HeightRequest = 40;
            WidthRequest = 40;
            HorizontalOptions = LayoutOptions.Start;
            VerticalOptions = LayoutOptions.Start;
        }

        public static readonly BindableProperty DateTimeInfoProperty =
  BindableProperty.Create("DateTimeInfo", typeof(DateTime), typeof(CellStackLayout), null);

        public DateTime DateTimeInfo
        {
            get { return (DateTime)GetValue(DateTimeInfoProperty); }
            set { SetValue(DateTimeInfoProperty, value); }
        }
    }
}
