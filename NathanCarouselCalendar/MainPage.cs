using System;
using System.Collections.ObjectModel;
using Xamarin.Forms;

namespace NathanCarouselCalendar
{
    public partial class MainPage : ContentPage
    {
        public event EventHandler<DateTime> DateSelectedEvent;

        private readonly TapGestureRecognizer _tapGestureRecognizer = new TapGestureRecognizer();
        private static string[] WeekDays = { "일", "월", "화", "수", "목", "금", "토" };

        public MainPage()
        {
            var viewModel =new MonthViewModel();

            var now = DateTime.Now;


            for (int i = 0; i < 6; i++)
            {
                viewModel.source.Add(SetDateGrid(now.AddMonths(i)));
            }

            viewModel.Months = new ObservableCollection<Month>(viewModel.source);

            BindingContext = viewModel;

            _tapGestureRecognizer.Tapped += DateSelected;

            var carouselView = new CarouselView
            {
                //HeightRequest = 300,
                //WidthRequest = 300,
                Margin=20,
                VerticalOptions = LayoutOptions.CenterAndExpand,
                HorizontalOptions = LayoutOptions.CenterAndExpand,
            };
            carouselView.SetBinding(ItemsView.ItemsSourceProperty, "Months");


            carouselView.ItemTemplate = new DataTemplate(() =>
              {
                  var dayGrid = new Grid
                  {
                      ColumnSpacing = 15,
                      RowSpacing = 15,
                      HorizontalOptions = LayoutOptions.FillAndExpand,
                      VerticalOptions = LayoutOptions.FillAndExpand,
                  };
                  dayGrid.RowDefinitions.Add(new RowDefinition { Height = new GridLength(1, GridUnitType.Auto) });
                  dayGrid.RowDefinitions.Add(new RowDefinition { Height = new GridLength(1, GridUnitType.Star) });
                  dayGrid.RowDefinitions.Add(new RowDefinition { Height = new GridLength(1, GridUnitType.Star) });
                  dayGrid.RowDefinitions.Add(new RowDefinition { Height = new GridLength(1, GridUnitType.Star) });
                  dayGrid.RowDefinitions.Add(new RowDefinition { Height = new GridLength(1, GridUnitType.Star) });
                  dayGrid.RowDefinitions.Add(new RowDefinition { Height = new GridLength(1, GridUnitType.Star) });
                  dayGrid.RowDefinitions.Add(new RowDefinition { Height = new GridLength(1, GridUnitType.Star) });
                  dayGrid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star) });
                  dayGrid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star) });
                  dayGrid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star) });
                  dayGrid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star) });
                  dayGrid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star) });
                  dayGrid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star) });
                  dayGrid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star) });


                  for (var row = 0; row < 7; row++)
                  {
                      for (var col = 0; col < 7; col++)
                      {
                          var cell = new CellStackLayout
                          {
                              VerticalOptions = LayoutOptions.FillAndExpand,
                              HorizontalOptions = LayoutOptions.FillAndExpand,
                          };
                          var label = new Label
                          {
                              VerticalOptions = LayoutOptions.FillAndExpand,
                              HorizontalOptions = LayoutOptions.FillAndExpand,
                              VerticalTextAlignment = TextAlignment.Center,
                              HorizontalTextAlignment = TextAlignment.Center,
                          };
                          var index = row * 7 + col;

                          cell.SetBinding(CellStackLayout.DateTimeInfoProperty, $"Day[{index}].ThisDateTime");
                          label.SetBinding(Label.TextProperty, $"Day[{index}].ThisDay");
                          label.SetBinding(Label.TextColorProperty, $"Day[{index}].Color");
                          cell.SetBinding(IsVisibleProperty, $"Day[{index}].IsEnabled");

                          cell.Children.Add(label);

                          AddTapGesture(cell);

                          dayGrid.Children.Add(cell, col, row);
                      }
                  }
                  var yearLabel = new Label
                  {
                      FontSize = 20,
                      FontAttributes = FontAttributes.Bold
                  };
                  yearLabel.SetBinding(Label.TextProperty, "ThisYear");

                  var monthLabel = new Label
                  {
                      FontSize = 20,
                      FontAttributes = FontAttributes.Bold
                  };
                  monthLabel.SetBinding(Label.TextProperty, "ThisMonth");

                  var yearAndMonthStackLayout = new StackLayout
                  {
                      Orientation = StackOrientation.Horizontal,
                      Children = {yearLabel, monthLabel}
                  };

                  return new StackLayout
                  {
                      Children =
                      {
                          yearAndMonthStackLayout,

                          new Frame
                          {
                              HasShadow=false,
                              Padding=0,
                              Margin=0,
                              Content= dayGrid
                          }
                      }
                  };

              });


            Content = new ScrollView
            {
                Content = carouselView
            };
        }

        private int GetWeeksInMonth(DateTime dateTime)
        {
            //extract the month
            int daysInMonth = DateTime.DaysInMonth(dateTime.Year, dateTime.Month);
            DateTime firstOfMonth = new DateTime(dateTime.Year, dateTime.Month, 1);
            //days of week starts by default as Sunday = 0
            int firstDayOfMonth = (int)firstOfMonth.DayOfWeek;
            int weeksInMonth = (int)Math.Ceiling((firstDayOfMonth + daysInMonth) / 7.0);

            return weeksInMonth;
        }

        private Month SetDateGrid(DateTime dateTime)
        {
            var nowMonthDaysInMonth = DateTime.DaysInMonth(dateTime.Year, dateTime.Month); // 지정한 연도, 달의 날짜 수
            var startingDay = (int)new DateTime(dateTime.Year, dateTime.Month, 1).DayOfWeek; // 시작 요일 (월요일==1) 

            var previousMonthDateTime = dateTime.AddMonths(-1);
            var previousMonthDaysInMonth = DateTime.DaysInMonth(previousMonthDateTime.Year, previousMonthDateTime.Month);

            var nextMonthDateTime= dateTime.AddMonths(1);

            var counter = 0;
            var nextMonthCounter = 1;

            var thisDateTime = new DateTime();

            var isEnabled = false;

            var month = new Month
            {
                ThisYear = dateTime.Year.ToString()+"년",
                ThisMonth = dateTime.Month.ToString()+"월"
            };

            for (int row = 0; row < 7; row++)
            {
                for (int col = 0; col < 7; col++)
                {
                    if (row == 0)
                    {
                        var color = Color.Black;
                        if (col == 0)
                        {
                            color = Color.Red;
                        }
                        else if (col == 6)
                        {
                            color = Color.Blue;
                        }

                        month.Day[row * 7 + col].ThisDay = WeekDays[col];
                        month.Day[row * 7 + col].Color = color;
                    }
                    else
                    {
                        Color color = Color.Default;
                        int date=-1;

                        if (counter < startingDay)
                        {
                            date = previousMonthDaysInMonth - (startingDay - counter - 1);
                            color = Color.Gray;

                            thisDateTime = previousMonthDateTime;
                            isEnabled = false;
                        }

                        else if (counter >= startingDay && (counter - startingDay) < nowMonthDaysInMonth)
                        {
                            date = counter + 1 - startingDay;
                            color = Color.Black;

                            thisDateTime = dateTime;
                            isEnabled = true;
                        }

                        else if (counter >= (nowMonthDaysInMonth + startingDay))
                        {
                            date = nextMonthCounter++;
                            color = Color.Gray;

                            thisDateTime = nextMonthDateTime;
                            isEnabled = false;
                        }

                        try
                        {
                            month.Day[row * 7 + col].ThisDateTime = new DateTime(thisDateTime.Year, thisDateTime.Month, date);
                        }
                        catch (Exception e)
                        {
                            Console.WriteLine(e.Message);
                        }

                        month.Day[row * 7 + col].ThisDay = date.ToString();
                        month.Day[row * 7 + col].Color = color;
                        month.Day[row * 7 + col].IsEnabled = isEnabled;


                        counter++;
                    }

                }
            }
            
            return month;
        }
        private void AddTapGesture(StackLayout cell)
        {
            cell.GestureRecognizers.Clear();
            cell.GestureRecognizers.Add(_tapGestureRecognizer);
        }

        private async void DateSelected(object s, EventArgs e)
        {
            var cell = s as CellStackLayout;

            var dateString = $"{cell.DateTimeInfo.Year}-{cell.DateTimeInfo.Month}-{cell.DateTimeInfo.Day}";

            bool answer = await App.Current.MainPage.DisplayAlert("Date Selected!", dateString + "로 변경하시겠습니까?", "OK", "Cancel");

            if (answer) { DateSelectedEvent?.Invoke(this, cell.dateTime); }

        }
    }
}



