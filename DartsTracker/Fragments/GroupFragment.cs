using System;
using Android.OS;
using Android.Views;
using Android.Widget;
using DartsTracker.Interfaces;
using DartsTracker.Presenters;
using Microcharts;
using Microcharts.Droid;
using SkiaSharp;
using Fragment = Android.Support.V4.App.Fragment;


namespace DartsTracker
{
    public class GroupFragment : Fragment
    {
        IGroupFragmentPresenter presenter;
        Spinner spinner;
        ChartView chartView;
        string groupName;

        public GroupFragment(string groupName)
        {
            this.groupName = groupName;
        }

        public override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
        }

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            //Use this to return your custom view for this Fragment
            base.OnCreateView(inflater, container, savedInstanceState);
            var view = inflater.Inflate(Resource.Layout.fragment_group, container, false);
            presenter = new GroupFragmentPresenter(groupName);
            spinner = Utils.AddSpinner(view, Resource.Id.group_spinner, 
                Resources.GetStringArray(Resource.Array.categories_array));

            spinner.ItemSelected += new EventHandler<AdapterView.ItemSelectedEventArgs>(spinner_ItemSelected);
            chartView = view.FindViewById<ChartView>(Resource.Id.group_chartView);
            return view;
        }

        private async void spinner_ItemSelected(object sender, AdapterView.ItemSelectedEventArgs e)
        {
            var spinner = (Spinner)sender;
            // array is in values/string.xml
            var categories = Resources.GetStringArray(Resource.Array.categories_array);
            // gets current category.
            var category = spinner.GetItemAtPosition(e.Position).ToString();
            var chart = new BarChart() 
            {
                LabelTextSize = 30,
                BackgroundColor = SKColor.Parse(Utils.BackgroundColor)
            };
            //lower resolutions.
            if (Resources.DisplayMetrics.Density <= 1.5)
                chart.LabelTextSize = 20;

            var entries = new Entry[0];
            if (category == categories[0])
            {
                entries = await presenter.GetWinsEntries();
            }
            else if (category == categories[1])
            {
                var gameModes = Resources.GetStringArray(Resource.Array.mods_array);
                entries = await presenter.GetGameModeEntries(gameModes);
            }
            else
            {
                entries = await presenter.GetRoundsEntries();

            }
            chart.Entries = entries;
            chartView.Chart = chart;

        }
    }
}