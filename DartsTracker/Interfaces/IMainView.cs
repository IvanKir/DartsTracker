using System.Collections.Generic;

namespace DartsTracker.Interfaces
{
    public interface IMainView
    {
        /*
         * Sets recycler view's adapter when groups names are loaded. 
         */
        public void OnGroupsLoaded(List<string> groups);
        /*
         * Creates and performs toast. 
         */
        public void MakeToast(int text);
       
        /*
         * Creates intent of new Activity and starts it.
         */
        public void NavigateToGroupActivity(string groupName, int playersNumber, bool continueGame);
    }
}