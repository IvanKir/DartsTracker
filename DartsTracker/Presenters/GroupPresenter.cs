using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DartsTracker.Interfaces;
using DartsTracker.Models;

namespace DartsTracker.Presenters
{
    public class GroupPresenter :IGroupPresenter
    {
        private readonly IGroupView view;
        public GroupPresenter(IGroupView view)
        {
            this.view = view;
        }

        public async Task<bool> AlertDialogOkClicked(string groupName, List<string> playersNames)
        {
            if (!CheckNames(playersNames))
                return false;
            var newGroup = new Group(groupName);
            await MainActivity.Database.SaveGroupAsync(newGroup);
            foreach (var item in playersNames)
            {
                await MainActivity.Database.SavePlayerAsync(new Player(item, groupName));
            }
            return true;
        }

        public async Task<bool> FabClicked(string groupName, string gameName, int gameType)
        {
            if (!(await CheckGameName(groupName, gameName)))
                return false;
            view.AddToAdapter(gameName);
            view.NavigateToGameActivity(groupName, gameName, false, gameType);
            return true;
        }

        private async Task<bool> CheckGameName(string groupName, string gameName)
        {
            if (string.IsNullOrEmpty(gameName))
            {
                view.MakeToast(Resource.String.toast_group_name_error);
                return false;
            }
            // Checks if database contains given game name in given group already.
            else if ((await GetGamesNames(groupName)).Contains(gameName))
            {
                view.MakeToast(Resource.String.toast_game_unique);
                return false;
            }
            return true;
        }

        public async Task<List<string>> GetGamesNames(string groupName)
        {
            var lst = await MainActivity.Database.GetGamesAsync();
            return lst
                .Where(a => a.GroupName == groupName)
                .Select(a => a.Name)
                .ToList();
        }


        private bool CheckNames(List<string> playerNames)
        {
            if(playerNames.Any(a => string.IsNullOrEmpty(a)))
            {
                view.MakeToast(Resource.String.toast_players_name_error);
                return false;
            }
            //Checks if all names are unique.
            if (playerNames.Distinct().Count() != playerNames.Count) {
                view.MakeToast(Resource.String.toast_players_unique_error);
                return false;
            }
            return true;
        }

        public async Task DeleteGame(string groupName, string gameName)
        {
            var game = await MainActivity.Database.GetGameAsync(groupName, gameName);
            var gameThrows = await MainActivity.Database.GetThrowsGameAsync(game.Id);
            foreach (var t in gameThrows)
                await MainActivity.Database.DeleteThrowAsync(t);
            await MainActivity.Database.DeleteGameAsync(game);
            var lst = (await MainActivity.Database.GetGamesAsync(groupName))
                .Select(a => a.Name)
                .ToList();
            view.OnGamesLoaded(lst);
        }
    }
}