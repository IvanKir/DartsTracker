using System.Collections.Generic;
using System.Threading.Tasks;

namespace DartsTracker.Interfaces
{
    public interface IMainPresenter
    {
        /*
         * Performs when OK button on AlertDialog from Floating Action Button is clicked.
         * If input from alertDialog is valid, method navigates app to the Group Activity.
         */
        public bool FabOkClicked(string groupName, string playersNumber);
        /*
         * Asynchronously Group Names from database. 
         */
        public Task<List<string>> GetGroupsNames();
        
        /*
         * Method removes whole group from databse - also all of its players, games and throws.
         */
        public Task DeleteFromDatabase(string groupName);
    }
}