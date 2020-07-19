using System;
using System.Collections.Generic;
using System.Linq;
using Android.Support.V7.Widget;
using Android.Views;

namespace DartsTracker
{
    public class MainRecViewAdapter : RecyclerView.Adapter
    {
        public List<string> ItemList { get; set; }
        public event EventHandler<string> ItemClick;
        public event EventHandler<string> ItemLongClick;
        private bool showPopupMenu;

        public MainRecViewAdapter(List<string> itemList, bool showPopupMenu)
        {
            this.showPopupMenu = showPopupMenu;
            ItemList = itemList;
        }

        public override int ItemCount => ItemList.Count();

        public override void OnBindViewHolder(RecyclerView.ViewHolder holder, int position)
        {
            MainRecViewHolder vh = holder as MainRecViewHolder;
            vh.TxtView.Text = ItemList[position];
        }

        public override RecyclerView.ViewHolder OnCreateViewHolder(ViewGroup parent, int viewType)
        {
            View itemView = LayoutInflater.From(parent.Context)
                .Inflate(Resource.Layout.main_rec_item, parent, false);
            return new MainRecViewHolder(itemView, onClick, onLongClick, showPopupMenu);
        }

        void onClick(string name) 
        {
            ItemClick?.Invoke(this, name);
        }

        void onLongClick(string name)
        {
            ItemLongClick?.Invoke(this, name);
        }
    }
}