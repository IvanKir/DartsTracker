using System.Collections.Generic;

namespace DartsTracker.Interfaces
{
    public interface IGameView
    {
        /*
         * Set recyclerView Adapter
         */
        public void SetAdapter(List<string> header);
        public void AddTableRow();
        /*
         * Updates table at position with new value.
         */
        public void UpdateTableAt(int row, int column, string sumThrow);
        /*
         * Creates and performs toast
         */
        public void MakeToast(int text);
        /*
         * Changes Textviews.
         */
        public void SetWinner(string playerName);
        /*
         * Change TextView.
         */
        public void SetPlayerText(string text);

        /*
         * Scrolls to last row in table.
         */
        public void ScrollToLast();

    }
}