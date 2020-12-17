using System;
using System.Collections.Generic;
using Xamarin.Forms;

namespace NathanCarouselCalendar
{
    public class Month
    {

        public string ThisYear { get; set; }
        public string ThisMonth { get; set; }
        public Color Color { get; set; }

        public Day[] Day { get; set; } = new Day[49];

        public Month()
        {
            for(int i=0; i < 49; i++)
            {
                Day[i] = new Day();
            }
        }
    }
    public class Day
    {
        public DateTime ThisDateTime { get; set; }

        public string ThisDay { get; set; } 
        public Color Color { get; set; }

        public bool IsEnabled { get; set; }
        public bool IsPastDay { get; set; }
        public Color SelectedBackgroundColor { get; set; }
    }
}
