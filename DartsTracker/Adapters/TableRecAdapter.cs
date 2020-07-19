using System;
using System.Collections.Generic;
using System.Linq;
using Android.Graphics;
using Android.Support.V7.Widget;
using Android.Views;
using Android.Widget;
using DartsTracker.ViewHolders;

namespace DartsTracker.Adapters
{
    public class TableRecAdapter : RecyclerView.Adapter
    {
        public List<List<string>> lst = new List<List<string>>();
        public event EventHandler<Tuple<int, int>> ItemClick;

        public TableRecAdapter(List<string> header)
        {
            lst.Add(header);
        }

        public override int ItemCount => lst.Count;

        public override void OnBindViewHolder(RecyclerView.ViewHolder holder, int position)
        {
            TableViewHolder vh = holder as TableViewHolder;
            // Sets text from lst.
            for (int i = 0; i < vh.textViewList.Count; i++)
                vh.textViewList[i].Text = lst[position][i];
        }

        public override RecyclerView.ViewHolder OnCreateViewHolder(ViewGroup parent, int viewType)
        {
            List<TextView> newLst = new List<TextView>();
            View itemView = LayoutInflater.From(parent.Context).
                   Inflate(Resource.Layout.item_table, parent, false);
            // Parameters for first column.
            var par = new LinearLayout.LayoutParams(
                LinearLayout.LayoutParams.WrapContent,
                LinearLayout.LayoutParams.MatchParent);
            var layout = itemView.FindViewById<LinearLayout>(Resource.Id.table_layout);

            for (int i = 0; i < lst.First().Count; i++)
            {
                // Textview parameters.
                var txtv = new TextView(itemView.Context);
                txtv.TextAlignment = TextAlignment.Center;
                txtv.TextSize = 24;
                txtv.SetPadding(24, 24, 24, 24);

                // First columns is blue
                if (i == 0)
                {
                    txtv.SetBackgroundColor(Color.ParseColor(itemView.Context.GetString(Resource.Color.colorPrimary)));
                    // Ems is width of 3 'M' chars
                    txtv.SetMinEms(3);
                    txtv.SetMaxEms(3);
                }
                else
                {
                    txtv.SetMinEms(5);
                    txtv.SetMaxEms(5);
                }
                txtv.LayoutParameters = par;
                layout.AddView(txtv);
                newLst.Add(txtv);
            }
            return new TableViewHolder(itemView, newLst, OnClick);
        }

        public void UpdateTable(int row, int column, string newScore)
        {
            lst[row][column] = newScore;
            NotifyItemChanged(row);
        }

        public void AddNewRow() 
        {
            List<string> l = new List<String>();
            l.Add((lst.Count - 1).ToString());
            for (int i = 0; i < lst[0].Count - 1; i++)
                l.Add("");
            lst.Add(l);
            NotifyItemChanged(lst.Count - 1);
        }

        void OnClick(int posRow, int posCol)
        {
            var pos = new Tuple<int, int>(posRow, posCol);
            ItemClick?.Invoke(this, pos);
        }
    }
}