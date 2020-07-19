using System.Threading.Tasks;
using Microcharts;

namespace DartsTracker.Interfaces
{
    public interface IGroupFragmentPresenter
    {
        /*
         * Gets data of played game modes for statistics from database
         */
        public Task<Entry[]> GetGameModeEntries(string [] keys);

        /*
         * Gets data of players wins for statistics from database
         */
        public Task<Entry[]> GetWinsEntries();

        /*
         * Gets data of winning rounds for statistics from database
         */
        public Task<Entry[]> GetRoundsEntries();
    }
}