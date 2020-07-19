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
    public class PlayerFragment : Fragment
    {
        private ChartView throwChartView;
        private ChartView averageChartView;
        private string groupName;
        private string[] playersNames;

        private IPlayerFragmentPresenter presenter;
        public PlayerFragment(string groupName, string[] playersNames)
        {
            this.groupName = groupName;
            this.playersNames = playersNames;
        }

        public override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
        }

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            base.OnCreateView(inflater, container, savedInstanceState);
            var view =  inflater.Inflate(Resource.Layout.fragment_player, container, false);
            var spinner = view.FindViewById<Spinner>(Resource.Id.player_spinner);
            var adapter = new ArrayAdapter<string>(view.Context,
                Android.Resource.Layout.SimpleSpinnerItem, playersNames);
            adapter.SetDropDownViewResource(Android.Resource.Layout.SimpleSpinnerDropDownItem);
            spinner.Adapter = adapter;
            spinner.ItemSelected += new EventHandler<AdapterView.ItemSelectedEventArgs>(spinner_ItemSelected);
            presenter = new PlayerFragmentPresenter(groupName);
            throwChartView = view.FindViewById<ChartView>(Resource.Id.player_throw_chartView);
            averageChartView = view.FindViewById<ChartView>(Resource.Id.player_average_chartView);
            return view;
        }

        private async void spinner_ItemSelected(object sender, AdapterView.ItemSelectedEventArgs e)
        {
            var spinner = (Spinner)sender;
            var playerName = spinner.GetItemAtPosition(e.Position).ToString();
            
            var throwEntries = await presenter.GetFiveThrows(playerName);
            var throwChart = new DonutChart()
            {
                Entries = throwEntries,
                LabelTextSize = 30
            };
            //lower resolutions.
            if (Resources.DisplayMetrics.Density <= 1.5)
                throwChart.LabelTextSize = 20;
            throwChart.BackgroundColor = SKColor.Parse(Utils.BackgroundColor);
            throwChartView.Chart = throwChart;

            var averageEntries = await presenter.GetFiveAverages(playerName);
            var averageChart = new DonutChart()
            {
                Entries = averageEntries,
                LabelTextSize = 30
            };
            //lower resolutions.
            if (Resources.DisplayMetrics.Density <= 1.5)
                averageChart.LabelTextSize = 20;
            averageChart.BackgroundColor = SKColor.Parse(Utils.BackgroundColor);
            averageChartView.Chart = averageChart;
        }
    }
}