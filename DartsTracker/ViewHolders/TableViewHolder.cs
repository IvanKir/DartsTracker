using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Support.V7.Widget;
using Android.Util;
using Android.Views;
using Android.Widget;

namespace DartsTracker.ViewHolders
{
    class TableViewHolder : RecyclerView.ViewHolder
    {
        public List<TextView> textViewList { get; private set; }
        public TableViewHolder(View itemView, List<TextView> textViewList, Action<int, int> listener) : base(itemView)
        {
            this.textViewList = textViewList;

            for (int i = 1; i < textViewList.Count; i++) {
                int tempCol = i;
                textViewList[i].LongClick += (obj, args) => listener(base.LayoutPosition, tempCol);
            }
        }
        
    }
}