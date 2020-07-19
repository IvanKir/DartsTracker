using Android.App;
using Android.OS;
using Android.Support.V7.App;
using Android.Runtime;
using Android.Widget;
using Android.Support.V7.Widget;
using System.Collections.Generic;
using Android.Support.Design.Widget;
using System;
using AlertDialog = Android.Support.V7.App.AlertDialog;
using System.Linq;
using Android.Media;
using Android.Content;
using System.IO;
using DartsTracker.Models;
using DartsTracker.Interfaces;
using DartsTracker.Presenters;

namespace DartsTracker
{

    [Activity(Label = "@string/app_name", Theme = "@style/AppTheme", MainLauncher = true, ScreenOrientation = Android.Content.PM.ScreenOrientation.Portrait)]
    public class MainActivity : AppCompatActivity, IMainView 
    {
        IMainPresenter mainPresenter;
        MainRecViewAdapter recAdapter;
        List<string> groupListNames;

        static DartDatabase database;
        public static DartDatabase Database
        {
            get
            {
                if (database == null)
                {
                    database = new DartDatabase(Path.Combine
                        (System.Environment.GetFolderPath
                        (System.Environment.SpecialFolder.Personal),
                        "DartTrackerDatabase.db3"));
                }
                return database;
            }
        }
        protected async override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            Xamarin.Essentials.Platform.Init(this, savedInstanceState);
            // Set our view from the "main" layout resource
            SetContentView(Resource.Layout.activity_main);

            var fab = FindViewById<FloatingActionButton>(Resource.Id.main_fab);
            var recView = FindViewById<RecyclerView>(Resource.Id.main_recview);
            var layoutManager = new LinearLayoutManager(this);
            mainPresenter = new MainPresenter(this);
            groupListNames = await mainPresenter.GetGroupsNames();
            recAdapter = new MainRecViewAdapter(groupListNames, true);
            
            recAdapter.ItemClick += OnItemClick;
            recAdapter.ItemLongClick += OnItemLongClick;
            recView.SetAdapter(recAdapter);
            recView.SetLayoutManager(layoutManager);

            DividerItemDecoration dividerItemDecoration = new DividerItemDecoration(recView.Context,
            layoutManager.Orientation);
            recView.AddItemDecoration(dividerItemDecoration);

            fab.Click += fabClicked;
        }

        public override void OnRequestPermissionsResult(int requestCode, string[] permissions, [GeneratedEnum] Android.Content.PM.Permission[] grantResults)
        {
            Xamarin.Essentials.Platform.OnRequestPermissionsResult(requestCode, permissions, grantResults);
            base.OnRequestPermissionsResult(requestCode, permissions, grantResults);
        }

        private void fabClicked(object sender, EventArgs args)
        {
            AlertDialog.Builder alertDialog = new AlertDialog.Builder(this);
            var inflater = this.LayoutInflater;
            var childLayout = inflater.Inflate(Resource.Layout.dialog_main, null);
            alertDialog.SetView(childLayout)
                //set event to null so alertdialog won't be destroyd on button click
                .SetPositiveButton(Resource.String.ok, (EventHandler<DialogClickEventArgs>)null)
                .SetNegativeButton(Resource.String.cancel, (EventHandler<DialogClickEventArgs>)null);
            var dialog = alertDialog.Create();
            dialog.Show();

            var positiveButton  = dialog.GetButton((int)DialogButtonType.Positive);
            positiveButton.Click += delegate
            {
                var groupName = childLayout.FindViewById<EditText>(Resource.Id.dialog_group).Text;
                var playersText = childLayout.FindViewById<EditText>(Resource.Id.dialog_players).Text;
                if (mainPresenter.FabOkClicked(groupName, playersText))
                    dialog.Dismiss();
            };
        }


        private void OnItemClick(object sender, string name)
        {
            NavigateToGroupActivity(name, 0, false);
        }

        private void OnItemLongClick(object sender, string name)
        {
            mainPresenter.DeleteFromDatabase(name);
        }

        public void OnGroupsLoaded(List<string> groups)
        {
            recAdapter.ItemList = groups;
            recAdapter.NotifyDataSetChanged();
        }

        public void NavigateToGroupActivity(string groupName, int playersNumber, bool continueGame)
        {
            Intent i = new Intent(this, typeof(GroupActivity));
            i.PutExtra("GroupName", groupName);
            i.PutExtra("PlayersNumber", playersNumber);
            i.PutExtra("Continue", continueGame);
            StartActivityForResult(i , 0);
        }

        public void MakeToast(int text)
        {
            Toast.MakeText(this, text, ToastLength.Short).Show();
        }

        protected async override void OnActivityResult(int requestCode, [GeneratedEnum] Result resultCode, Intent data)
        {

            if (requestCode == 0 && resultCode == 0)
            {
                if (recAdapter != null)
                {
                    recAdapter.ItemList = (await Database.GetGroupAsync()).Select(a => a.Name).ToList();
                    recAdapter.NotifyDataSetChanged();
                }
            }
            base.OnActivityResult(requestCode, resultCode, data);
        }
    }
}