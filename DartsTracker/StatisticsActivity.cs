using System.Linq;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Support.V4.View;
using DartsTracker.Adapters;
using Android.Support.Design.Widget;
using Android.Support.V4.App;
using Android.Content.PM;
using Android.Util;

namespace DartsTracker
{
    [Activity(Label = "StatisticsActivity", ScreenOrientation = ScreenOrientation.Portrait)]
    public class StatisticsActivity : FragmentActivity
    {
        TabLayout tabs;
        ViewPager pager;
        protected async override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.activity_statistics);

            var bundle = Intent.Extras;
            string groupName = "";
            if (bundle != null)
            {
                groupName = bundle.GetString("GroupName");
            }

            pager = FindViewById<ViewPager>(Resource.Id.pager);
            tabs = FindViewById<TabLayout>(Resource.Id.stats_tablayout);
            pager.CurrentItem = 0;

            string[] titles = new string[]
                {
                    Resources.GetString(Resource.String.stats_group),
                   Resources.GetString(Resource.String.stats_player)
                };

            var players = await MainActivity.Database.GetPlayersAsync(groupName);
            var playersNames = players
                .Select(a => a.Name)
                .ToArray();

            var adapter = new TabsFragmentAdapter(SupportFragmentManager, groupName, titles, playersNames);
            pager.Adapter = adapter;
            pager.OffscreenPageLimit = titles.Length;
            tabs.SetupWithViewPager(pager);
        }
    }
}