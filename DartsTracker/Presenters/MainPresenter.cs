using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DartsTracker.Interfaces;
using DartsTracker.Models;

namespace DartsTracker.Presenters
{
    class MainPresenter : IMainPresenter
    {
        private IMainView view;
        public MainPresenter(IMainView view)
        {
            this.view = view;
        }

        public bool FabOkClicked(string groupName, string playersNumber)
        {
            if (!CheckValidGroupName(groupName) || !CheckPlayersNumber(playersNumber, out int players))
                return false;
            view.NavigateToGroupActivity(groupName, players, true);
            return true;

        }

        public async Task<List<string>> GetGroupsNames()
        {
            var lst = await MainActivity.Database.GetGroupAsync();
            return lst
                .Select(a => a.Name)
                .ToList();
        }

        private bool CheckValidGroupName(string groupName)
        {
            if (string.IsNullOrEmpty(groupName))
            {
                view.MakeToast(Resource.String.toast_group_name_error);
                return false;
            }
            //Checks if database already contains a group with this given name.
            if (MainActivity.Database.GetGroupAsync().Result
                .Select(a => a.Name)
                .ToList()
                .Contains(groupName))
            {
                view.MakeToast(Resource.String.toast_group_unique);
                return false;
            }
            return true;
        }

        private bool CheckPlayersNumber(string text, out int playersNumber)
        {
            if (string.IsNullOrEmpty(text))
            {
                view.MakeToast(Resource.String.toast_players_error);
                playersNumber = 0;
                return false;
            }
            if (int.TryParse(text, out playersNumber))
            {
                if (playersNumber < 0 && playersNumber > 7)
                    return false;
            }
            return true;
        }



        public async Task DeleteFromDatabase(string groupName)
        {
            var players = await MainActivity.Database.GetPlayersAsync(groupName);
            var games = await MainActivity.Database.GetGamesAsync(groupName);
            await DeletePlayersFromDatabase(players);
            await DeleteGamesFromDatabase(games);
            await MainActivity.Database.DeleteGroupAsync(await MainActivity.Database.GetGroupAsync(groupName));
            view.OnGroupsLoaded(MainActivity.Database.GetGroupAsync().Result.Select(a => a.Name).ToList());
            
        }
        
        /*
         * Method deletes player and all his throws from database.
         */
        private async Task DeletePlayersFromDatabase(IEnumerable<Player> players)
        {
            foreach (var player in players)
            {
                var playerThrow = MainActivity.Database.GetThrowsAsync().Result.Where(a => a.PlayerId == player.Id).ToList();
                foreach (var t in playerThrow)
                {
                    await MainActivity.Database.DeleteThrowAsync(t);
                }
                await MainActivity.Database.DeletePlayerAsync(player);
            }
        }

        
        private async Task DeleteGamesFromDatabase(IEnumerable<Game> games)
        {
            foreach (var game in games)
            {
                await MainActivity.Database.DeleteGameAsync(game);
            }
        }

    }
}