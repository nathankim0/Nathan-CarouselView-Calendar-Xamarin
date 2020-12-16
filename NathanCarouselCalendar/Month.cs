using System;
using System.Collections.Generic;
using Xamarin.Forms;

namespace NathanCarouselCalendar
{
    public class Month
    {
        public int ThisMonth { get; set; }
        //  public Dictionary<int, Color> Day { get; set; } = new Dictionary<int, Color>();
        //  public int[] Day { get; set; } = new int[49];
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
        public string ThisDay { get; set; } 
        public Color Color { get; set; }
    }
}
