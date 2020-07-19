using System;
using System.Collections.Generic;
using System.Linq;
using Android.App;
using Android.Content;
using Android.Content.PM;
using Android.OS;
using Android.Support.V7.Widget;
using Android.Widget;
using DartsTracker.Adapters;
using DartsTracker.Interfaces;
using DartsTracker.Presenters;

namespace DartsTracker
{
    [Activity(Label = "GameActivity", ScreenOrientation = ScreenOrientation.Portrait)]
    public class GameActivity : Activity, IGameView
    {
        private IGamePresenter presenter;

        private int gameType = 301;

        private List<EditText> values = new List<EditText>();
        private TextView playingTxtView;
        private TextView playerNameTextView;

        RecyclerView recView;
        TableRecAdapter ad;
        protected async override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.activity_game);

            var btn = FindViewById<Button>(Resource.Id.game_btn);
            playerNameTextView = FindViewById<TextView>(Resource.Id.game_player_name);
            playingTxtView = FindViewById<TextView>(Resource.Id.game_playing);
            presenter = new GamePresenter(this);
            //used for getting the width of display.
            // get data from previous activity.
            Bundle extras = Intent.Extras;
            if (extras != null) {
                var groupName = extras.GetString("GroupName");
                var name = extras.GetString("GameName");
                gameType = extras.GetInt("GameType");
                if (extras.GetBoolean("AlreadyPlayed"))
                    await presenter.ContinueGame(groupName, name);
                else
                    await presenter.MakeNewGame(groupName, name, gameType);
            }
            var layoutManager = new LinearLayoutManager(this, LinearLayoutManager.Vertical, false);

            recView = FindViewById<RecyclerView>(Resource.Id.game_rec);
            recView.SetAdapter(ad);
            recView.SetLayoutManager(layoutManager);    

            values.Add(FindViewById<EditText>(Resource.Id.game_edittext_first));
            values.Add(FindViewById<EditText>(Resource.Id.game_edittext_second));
            values.Add(FindViewById<EditText>(Resource.Id.game_edittext_third));
            
            ad.ItemClick += OnItemClick;
            btn.Click += BtnClicked;

        }

        private async void BtnClicked(object obj, EventArgs args)
        {
            var valid =  await presenter.OnBtnClicked(values
                .Select(a => a.Text)
                .ToList());
            if (valid)
                values.ForEach(a => a.Text = "");
        }

        private void OnItemClick(object sender, Tuple<int, int> pos)
        {
            // only last row can be changed.
            if (pos.Item1 < 2 || pos.Item1 != ad.lst.Count - 1 || ad.lst[pos.Item1][pos.Item2] == "")
                return;
            // Builds dialog.
            AlertDialog.Builder alertDialog = new AlertDialog.Builder(this);
            var inflater = this.LayoutInflater;
            var childLayout = inflater.Inflate(Resource.Layout.dialog_change_throw, null);
            alertDialog.SetView(childLayout)
                //set event to null so alertdialog won't be destroyd on button click
                .SetPositiveButton(Resource.String.ok, (EventHandler<DialogClickEventArgs>)null)
                .SetNegativeButton(Resource.String.cancel, (EventHandler<DialogClickEventArgs>)null);
            var dialog = alertDialog.Create();
            dialog.Show();

            var edittexts = new List<EditText>();
            var t = presenter.GetThrow(pos.Item1 - 2, pos.Item2 - 1);

            edittexts.Add(childLayout.FindViewById<EditText>(Resource.Id.dialog_throw_first));
            edittexts.Add(childLayout.FindViewById<EditText>(Resource.Id.dialog_throw_second));
            edittexts.Add(childLayout.FindViewById<EditText>(Resource.Id.dialog_throw_third));
            edittexts[0].Text = t.First.ToString();
            edittexts[1].Text = t.Second.ToString();
            edittexts[2].Text = t.Third.ToString();

            var positiveButton = dialog.GetButton((int)DialogButtonType.Positive);
            positiveButton.Click += async delegate
            {
                if ((await presenter.OnOkButtonClicked(pos.Item1 - 1, pos.Item2 - 1,
                    edittexts
                    .Select(a => a.Text)
                    .ToList())))
                    dialog.Dismiss();    
            };
        }

        public void SetAdapter(List<string> header)
        {
            ad = new TableRecAdapter(header);
        }

        public void AddTableRow()
        {
            ad.AddNewRow();
        }

        public void UpdateTableAt(int round, int player, string sumThrow)
        {
            ad.UpdateTable(round + 1, player + 1, sumThrow);
        }

        public void MakeToast(int text)
        {
            Toast.MakeText(this, text, ToastLength.Short).Show();
        }

        public void SetPlayerText(string text)
        {
            playerNameTextView.Text = text;
        }

        public void SetWinner(string playerName)
        {
            playingTxtView.Text = Resources.GetString(Resource.String.winner);
            SetPlayerText(playerName);
        }

        public void ScrollToLast()
        {
            recView.ScrollToPosition(ad.lst.Count - 1);
        }
    }
}