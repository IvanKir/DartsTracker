using System;
using System.Collections.Generic;
using System.Linq;
using Android.App;
using Android.Content;
using Android.Content.PM;
using Android.OS;
using Android.Support.Design.Widget;
using Android.Support.V7.Widget;
using Android.Text;
using Android.Views;
using Android.Widget;
using DartsTracker.Interfaces;
using DartsTracker.Presenters;
using Toolbar = Android.Widget.Toolbar;

namespace DartsTracker
{
    [Activity(Label = "GroupActivity", Theme = "@style/GameTheme", ScreenOrientation = ScreenOrientation.Portrait)]
    public class GroupActivity : Activity, IGroupView
    {
        IGroupPresenter presenter;
        MainRecViewAdapter recAdapter;
        string groupName = "";
        int gametype = 301;
        
        protected override async void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.group_activity);
            Xamarin.Essentials.Platform.Init(this, savedInstanceState);
            
            presenter = new GroupPresenter(this);
            var toolbar = FindViewById<Toolbar>(Resource.Id.toolbar);
            var fab = FindViewById<FloatingActionButton>(Resource.Id.group_fab);
            var playersNumber = 0;
            // Gets data from previous activity
            var extras = Intent.Extras;
            if (extras != null)
            {
                groupName = extras.GetString("GroupName");
                if (extras.GetBoolean("Continue"))
                {
                    playersNumber = extras.GetInt("PlayersNumber");
                    showAlertDialog(playersNumber);
                }
            }
            toolbar.Title = groupName;
            SetActionBar(toolbar);
            var gameNames = await presenter.GetGamesNames(groupName);
            recAdapter = new MainRecViewAdapter(gameNames, true);
            recAdapter.ItemClick += OnItemClick;
            recAdapter.ItemLongClick += OnItemLongClick;

            var recView = FindViewById<RecyclerView>(Resource.Id.group_rec);
            var layoutManager = new LinearLayoutManager(this);
            recView.SetAdapter(recAdapter);
            recView.SetLayoutManager(layoutManager);

            DividerItemDecoration dividerItemDecoration = new DividerItemDecoration(recView.Context,
            layoutManager.Orientation);
            recView.AddItemDecoration(dividerItemDecoration);

            fab.Click += fabClicked;
        }

        public override bool OnCreateOptionsMenu(IMenu menu)
        {
            MenuInflater.Inflate(Resource.Menu.toolbar_menu, menu);
            return base.OnCreateOptionsMenu(menu);
        }

        // Takes care of menu on the toolbar.
        public override bool OnOptionsItemSelected(IMenuItem item)
        {
            Intent i = new Intent(this, typeof(StatisticsActivity));
            i.PutExtra("GroupName", groupName);
            StartActivity(i);
            return base.OnOptionsItemSelected(item);
        }

        private void showAlertDialog(int playersNumber)
        {
            var editTextList = new List<EditText>();
            var alertDialog = new AlertDialog.Builder(this);
            var par = new LinearLayout.LayoutParams(
                LinearLayout.LayoutParams.MatchParent,
                LinearLayout.LayoutParams.WrapContent);
            var inflater = this.LayoutInflater;
            var childLayout = inflater.Inflate(Resource.Layout.dialog_group_players, null);
            var layout = childLayout.FindViewById<LinearLayout>(Resource.Id.dialog_players_layout);

            alertDialog.SetTitle("Select names");
            par.SetMargins(8, 8, 8, 8);
            addEditTexts(playersNumber, editTextList, par, layout);
            
            alertDialog.SetView(childLayout)
            .SetPositiveButton(Resource.String.ok, (EventHandler<DialogClickEventArgs>)null)
            .SetNegativeButton(Resource.String.cancel, (obj, args) => Finish());
            var dialog = alertDialog.Create();
            dialog.Show();

            dialog.GetButton((int)DialogButtonType.Positive).Click += async delegate
            {
                if ((await presenter.AlertDialogOkClicked(groupName, editTextList.Select(a => a.Text).ToList())))
                    dialog.Dismiss();
            };

        }

        private void addEditTexts(int playersNumber, List<EditText> editTextList, 
            LinearLayout.LayoutParams pars, LinearLayout layout)
        {
            for (int i = 0; i < playersNumber; i++)
            {
                var editText = new EditText(this);
                //Maximum chars in EditTexts.
                editText.SetFilters(new IInputFilter[] { new InputFilterLengthFilter(12)});
                editText.LayoutParameters = pars;
                editText.Hint = Resources.GetString(Resource.String.hint_player_name);
                layout.AddView(editText);
                editTextList.Add(editText);
            }
        }

        private void fabClicked(object obj, EventArgs args)
        {
            var alertDialog = new AlertDialog.Builder(this);
            var inflater = LayoutInflater;
            var childLayout = inflater.Inflate(Resource.Layout.dialog_group_game, null);
            var spinner = Utils.AddSpinner(childLayout, Resource.Id.dialog_spinner, 
                Resources.GetStringArray(Resource.Array.mods_array));
            spinner.ItemSelected += new EventHandler<AdapterView.ItemSelectedEventArgs>(spinner_ItemSelected);
            
            alertDialog.SetView(childLayout)
                //set event to null so alertdialog won't be destroyed on button click
                .SetPositiveButton(Resource.String.ok, (EventHandler<DialogClickEventArgs>)null)
                .SetNegativeButton(Resource.String.cancel, (EventHandler<DialogClickEventArgs>)null);
            var dialog = alertDialog.Create();
            dialog.Show();

            var positiveButton = dialog.GetButton((int)DialogButtonType.Positive);
            positiveButton.Click += async delegate
            {
                var gameName = childLayout.FindViewById<EditText>(Resource.Id.dialog_group_name).Text;
                if(await presenter.FabClicked(groupName, gameName, gametype))
                    dialog.Dismiss();
                // other settings are checked by view settings
            };
        }

        private void spinner_ItemSelected(object sender, AdapterView.ItemSelectedEventArgs e)
        {
            var spinner = (Spinner)sender;
            gametype = int.Parse(spinner.GetItemAtPosition(e.Position).ToString());
        }

        private void OnItemClick(object sender, string gameName)
        {
            NavigateToGameActivity(groupName, gameName, true, gametype);
        }
        private async void OnItemLongClick(object sender, string gameName)
        {
            await presenter.DeleteGame(groupName, gameName);
            recAdapter.ItemList = (await MainActivity.Database.GetGamesAsync(groupName))
                .Select(a => a.Name)
                .ToList(); ;
            recAdapter.NotifyDataSetChanged();
        }

        public void OnGamesLoaded(List<string> games)
        {
            recAdapter.ItemList = games;
            recAdapter.NotifyDataSetChanged();
        }

        public void MakeToast(int text)
        {
            Toast.MakeText(this, text, ToastLength.Short).Show();
        }

        public void NavigateToGameActivity(string groupName, string gameName, bool alreadyPlayed, int gameType)
        {
            Intent i = new Intent(this, typeof(GameActivity));
            i.PutExtra("GroupName", groupName);
            i.PutExtra("GameName", gameName);
            i.PutExtra("AlreadyPlayed", alreadyPlayed);
            i.PutExtra("GameType", gameType);
            StartActivity(i);
        }

        public void AddToAdapter(string item)
        {
            recAdapter.ItemList.Add(item);
            recAdapter.NotifyItemInserted(recAdapter.ItemList.Count - 1);

        }

        public override void Finish()
        {
            Intent returnIntent = new Intent();
            SetResult(0, returnIntent);
            base.Finish();
        }
    }
}