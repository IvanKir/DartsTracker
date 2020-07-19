using System.Collections.Generic;

namespace DartsTracker.Interfaces
{
    public interface IGroupView
    {
        /*
         * Creates and performs toast.
         */
        public void MakeToast(int text);

        /*
         * Starts new Game activity. 
         */
        public void NavigateToGameActivity(string groupName, string gameName, bool alreadyPlayed, int gameType);
        
        /*
         * Adds Item to adapter and updates it.
         */
        public void AddToAdapter(string item);

        /*
         * Sets recycler view's adapter when games names are loaded. 
         */
        public void OnGamesLoaded(List<string> games);

    
    }
}