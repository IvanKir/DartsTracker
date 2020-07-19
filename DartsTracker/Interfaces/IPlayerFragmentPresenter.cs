using System.Threading.Tasks;
using Microcharts;

namespace DartsTracker.Interfaces
{
    interface IPlayerFragmentPresenter
    {
        /*
         * Gets data of most five throws for statistics from database.
         */
        public Task<Entry[]> GetFiveThrows(string playerName);

        /*
         * Gets data of most five throws for statistics from database.
         */
        public Task<Entry[]> GetFiveAverages(string playerName);
    }
}