using System.Collections.Generic;
using Android.Views;
using Android.Widget;
using DartsTracker.Models;
using Microcharts;
using SkiaSharp;

namespace DartsTracker
{
    public static class Utils
    {
        public static readonly string BackgroundColor = "#FAFAFA";
        public static readonly string[] ChartsColors = new string [] { "#5c6bc0", "#ab47bc", "#ef5350", "#66bb6a", "#ff7043", "#ffee58" };

        public static Spinner AddSpinner(View view, int layout, string[] arr) 
        {
            var spinner = view.FindViewById<Spinner>(layout);
            var adapter = new ArrayAdapter<string>(view.Context,
                Android.Resource.Layout.SimpleSpinnerItem, arr);
            adapter.SetDropDownViewResource(Android.Resource.Layout.SimpleSpinnerDropDownItem);
            spinner.Adapter = adapter;
            return spinner;
        }

        public static Entry[] MakeEntries(List<Item> items)
        {
            int i = 0;
            var entries = new Entry[items.Count];
            foreach (var item in items)
            {
                entries[i] = new Entry(item.Value)
                {
                    Label = item.Type.ToString(),
                    ValueLabel = item.Value.ToString(),
                    Color = SKColor.Parse(ChartsColors[i++])
                };
            }
            return entries;
        }

    }
}