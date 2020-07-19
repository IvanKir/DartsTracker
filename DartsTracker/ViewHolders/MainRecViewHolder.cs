using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Support.V7.Widget;
using Android.Views;
using Android.Widget;
using PopupMenu = Android.Support.V7.Widget.PopupMenu;

namespace DartsTracker
{
    public class MainRecViewHolder : RecyclerView.ViewHolder
    {
        public TextView TxtView { get; set; }
        public MainRecViewHolder(View itemView, Action<string> listener, Action<string> longListener, bool menu) : base(itemView)
        {
            TxtView = ItemView.FindViewById<TextView>(Resource.Id.main_rec_txtview);
            TxtView.Click += (obj, sender) => listener(TxtView.Text);
            if (menu)
            {
                TxtView.LongClick += (obj, sened) =>
                {
                    PopupMenu popupMenu = new PopupMenu(ItemView.Context, itemView);
                    popupMenu.Inflate(Resource.Menu.popup_menu);
                    popupMenu.MenuItemClick += (s1, arg1) =>
                    {
                        longListener(TxtView.Text);
                    };
                    popupMenu.Show();
                };
            }
        }

        
    }
}