using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DartsTracker.Interfaces
{
    public interface IGroupPresenter
    {
        /*
         * Checks if input is valid. If so new group and players are created in database. 
         */
        public Task<bool> AlertDialogOkClicked(string groupName, List<string> playersNames);

        /*
         * Returns list of  games names in specific group from database.
         */
        public Task<List<String>> GetGamesNames(string groupName);

        /*
         * Checks if input is valid. If so, starts Game Activity.
         */
        public Task<bool> FabClicked(string groupName, string gameName, int gameType);

        public Task DeleteGame(string groupName, string gameName);
    }
}