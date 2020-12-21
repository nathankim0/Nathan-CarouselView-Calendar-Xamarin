using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using Xamarin.Forms;

namespace NathanCarouselCalendar
{
    public class MainPage : ContentPage
    {
        public event EventHandler<DateTime> DateSelectedEvent;

        private readonly TapGestureRecognizer _dayTapGestureRecognizer = new TapGestureRecognizer();
        private readonly TapGestureRecognizer _yearTapGestureRecognizer = new TapGestureRecognizer();

        private static readonly string[] WeekDays = {"일", "월", "화", "수", "목", "금", "토"};

        private readonly MonthViewModel viewModel = new MonthViewModel();

        private readonly List<CellStackLayout> _cellList = new List<CellStackLayout>();

        public MainPage()
        {
            BackgroundColor = Color.DarkOliveGreen;
            var now = DateTime.Now;


            for (var i = 0; i < 3; i++)
            {
                viewModel.source.Add(SetDateGrid(now.AddMonths(i)));
            }

            viewModel.Months = new ObservableCollection<Month>(viewModel.source);

            BindingContext = viewModel;

            _dayTapGestureRecognizer.Tapped += OnDateSelected;

            var carouselView = new CarouselView
            {
                //HeightRequest = 300,
                //WidthRequest = 300,
                Margin = 20,
                VerticalOptions = LayoutOptions.CenterAndExpand,
                HorizontalOptions = LayoutOptions.CenterAndExpand,
            };
            carouselView.SetBinding(ItemsView.ItemsSourceProperty, "Months");


            carouselView.ItemTemplate = new DataTemplate(() =>
            {
                var dayGrid = new Grid
                {
                    //ColumnSpacing = 15,
                    //RowSpacing = 15,
                    HorizontalOptions = LayoutOptions.FillAndExpand,
                    VerticalOptions = LayoutOptions.FillAndExpand,
                };
                dayGrid.RowDefinitions.Add(new RowDefinition {Height = new GridLength(1, GridUnitType.Auto)});

                for (var i = 0; i < 6; i++)
                {
                    dayGrid.RowDefinitions.Add(new RowDefinition {Height = new GridLength(1, GridUnitType.Star)});
                }

                for (var i = 0; i < 7; i++)
                {
                    dayGrid.ColumnDefinitions.Add(new ColumnDefinition {Width = new GridLength(1, GridUnitType.Star)});
                }

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
                            VerticalOptions = LayoutOptions.CenterAndExpand,
                            HorizontalOptions = LayoutOptions.CenterAndExpand,
                            VerticalTextAlignment = TextAlignment.Center,
                            HorizontalTextAlignment = TextAlignment.Center,
                        };
                        var index = row * 7 + col;

                        cell.SetBinding(CellStackLayout.DateTimeInfoProperty, $"Day[{index}].ThisDateTime");
                        cell.SetBinding(IsVisibleProperty, $"Day[{index}].IsEnabled");
                       // cell.SetBinding(BackgroundColorProperty, $"Day[{index}].SelectedBackgroundColor");

                        label.SetBinding(Label.TextProperty, $"Day[{index}].ThisDay");
                        label.SetBinding(Label.TextColorProperty, $"Day[{index}].Color");

                        cell.Children.Add(label);


                  
                        if (row != 0)
                        {
                            AddDayTapGesture(cell);

                            _cellList.Add(cell);
                        }


                        var cellFrame = new Frame
                        {
                            Content = cell,
                            Margin = 0,
                            Padding = 0,
                            BorderColor = Color.White,
                            HasShadow=false,
                        //    CornerRadius=20,
                            IsClippedToBounds=true
                        };
                        

                        dayGrid.Children.Add(cellFrame, col, row);
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
                monthLabel.SetBinding(Label.TextColorProperty, "Color");

                var yearAndMonthStackLayout = new StackLayout
                {
                    Orientation = StackOrientation.Horizontal,
                    Children = {yearLabel, monthLabel}
                };

                return new StackLayout
                {
                    Padding = 10,
                    Children =
                    {
                        yearAndMonthStackLayout,

                        new Frame
                        {
                            BorderColor = Color.DeepPink,
                            HasShadow = false,
                            Padding = 0,
                            Margin = 0,
                            Content = dayGrid
                        }
                    }
                };
            });

            var month = viewModel.Months.FirstOrDefault(x => x.ThisMonth == DateTime.Now.Month.ToString());
            var position = viewModel.source.ToList().FindIndex(x => x.ThisMonth == DateTime.Now.Month.ToString());
            //carouselView.CurrentItem = month;
            //carouselView.Position = position;
            carouselView.ScrollTo(position, animate: false);

            var button = new Button()
            {
                Text = "Go to Today",
                FontSize=30
            };
            button.Clicked += (s, e) =>
            {
                //carouselView.CurrentItem = month;
                //carouselView.Position = position;
                carouselView.ScrollTo(position, animate: false);
            };

            Content = new StackLayout
            {
                Padding=20,
                Children = { carouselView, button }
            };
        }

        private void AddDayTapGesture(CellStackLayout cell)
        {
            cell.GestureRecognizers.Clear();
            cell.GestureRecognizers.Add(_dayTapGestureRecognizer);
        }

        private void AddYearTapGesture(CellStackLayout cell)
        {
            cell.GestureRecognizers.Clear();
            cell.GestureRecognizers.Add(_dayTapGestureRecognizer);
        }

        private async void OnDateSelected(object s, EventArgs e)
        {
            if (!(s is CellStackLayout cell)) return;

            var dateTime = cell.DateTimeInfo;

            if (DateTime.Now.Year > dateTime.Year ||
               (DateTime.Now.Year == dateTime.Year && DateTime.Now.Month > dateTime.Month) ||
               (DateTime.Now.Year == dateTime.Year && DateTime.Now.Month == dateTime.Month && DateTime.Now.Day >= dateTime.Day))
            {
                return;
            }

            var dateString = $"{dateTime.Year}-{dateTime.Month}-{dateTime.Day}";

            _cellList.ForEach(x => x.BackgroundColor = Color.White);
            cell.BackgroundColor = Color.Aqua;

            //var answer =
            //    await Application.Current.MainPage.DisplayAlert("Date Selected!", dateString + "로 변경하시겠습니까?", "OK",
            //        "Cancel");

            //if (answer)
            //{
            //    //viewModel.Months.ToList().ForEach(x => x.Day.ToList().ForEach(day => day.SelectedBackgroundColor = Color.Yellow));
               
            //    DateSelectedEvent?.Invoke(this, cell.dateTime);
            //}
        }

        private int GetWeeksInMonth(DateTime dateTime)
        {
            var daysInMonth = DateTime.DaysInMonth(dateTime.Year, dateTime.Month);
            var firstOfMonth = new DateTime(dateTime.Year, dateTime.Month, 1);
            var firstDayOfMonth = (int) firstOfMonth.DayOfWeek;
            var weeksInMonth = (int) Math.Ceiling((firstDayOfMonth + daysInMonth) / 7.0);

            return weeksInMonth;
        }

        private Month SetDateGrid(DateTime dateTime)
        {
            var nowMonthDaysInMonth = DateTime.DaysInMonth(dateTime.Year, dateTime.Month); // 지정한 연도, 달의 날짜 수
            var startingDay = (int) new DateTime(dateTime.Year, dateTime.Month, 1).DayOfWeek; // 시작 요일 (월요일==1) 

            var previousMonthDateTime = dateTime.AddMonths(-1);
            var previousMonthDaysInMonth =
                DateTime.DaysInMonth(previousMonthDateTime.Year, previousMonthDateTime.Month);

            var nextMonthDateTime = dateTime.AddMonths(1);

            var counter = 0;
            var nextMonthCounter = 1;

            var thisDateTime = new DateTime();

            var isEnabled = false;
            var isPastDay = false;

            var month = new Month
            {
                ThisYear = dateTime.Year.ToString() + "년",
                ThisMonth = dateTime.Month.ToString()
            };

            if (DateTime.Now.Month == dateTime.Month)
            {
                month.Color = Color.Accent;
            }
            else
            {
                month.Color = Color.Black;
            }

            for (var row = 0; row < 7; row++)
            {
                for (var col = 0; col < 7; col++)
                {
                    if (row == 0)
                    {
                        var color = Color.Black;
                        switch (col)
                        {
                            case 0:
                                color = Color.Red;
                                break;
                            case 6:
                                color = Color.Blue;
                                break;
                        }

                        month.Day[row * 7 + col].ThisDay = WeekDays[col];
                        month.Day[row * 7 + col].Color = color;
                        month.Day[row * 7 + col].IsEnabled = true;
                    }
                    else
                    {
                        var color = Color.Default;
                        var date = -1;

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

                            if (DateTime.Now.Year == dateTime.Year &&
                                       DateTime.Now.Month == dateTime.Month &&
                                       DateTime.Now.Day == date)
                            {
                                color = Color.Accent;
                                isPastDay = false;
                            }
                            else if (DateTime.Now.Year > dateTime.Year ||
                                     (DateTime.Now.Year == dateTime.Year && DateTime.Now.Month > dateTime.Month) ||
                                    (DateTime.Now.Year == dateTime.Year && DateTime.Now.Month == dateTime.Month && DateTime.Now.Day > date))
                            {
                                color = Color.Gray;
                                isPastDay = true;
                            }
                            else
                            {
                                color = Color.Black;
                                isPastDay = false;
                            }

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
                            month.Day[row * 7 + col].ThisDateTime =
                                new DateTime(thisDateTime.Year, thisDateTime.Month, date);
                        }
                        catch (Exception e)
                        {
                            Console.WriteLine(e.Message);
                        }

                        month.Day[row * 7 + col].ThisDay = date.ToString();
                        month.Day[row * 7 + col].Color = color;
                        month.Day[row * 7 + col].IsEnabled = isEnabled;
                        month.Day[row * 7 + col].IsPastDay = isPastDay;

                        counter++;
                    }
                }
            }

            return month;
        }
    }
}