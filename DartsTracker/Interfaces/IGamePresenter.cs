using System.Collections.Generic;
using System.Threading.Tasks;
using DartsTracker.Models;

namespace DartsTracker.Interfaces
{

    public interface IGamePresenter
    {
        /*
         * Get throw from table.
         */
        public Throw GetThrow(int row, int column);

        /*
         * Creates and set parameters of new game. Game is also saved into a database.
         */
        public Task MakeNewGame(string groupName, string gameName, int GameType);
        /*
         *  Check if input is valid. If yes, create and save new Throw and calls view method
         *  to print it into a score table.
         */
        public Task<bool> OnBtnClicked(List<string> values);
        /*
         * Continues already played game. Sets params and create table.
         */
        public Task ContinueGame(string groupName, string gameName);

        /*
         * Checks input and set changed throw values on table. Throw is also changed in database.
         */
        public Task<bool> OnOkButtonClicked(int row, int column, List<string> values);

    }
}