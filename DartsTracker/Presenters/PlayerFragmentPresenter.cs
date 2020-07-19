using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DartsTracker.Interfaces;
using DartsTracker.Models;
using Microcharts;

namespace DartsTracker.Presenters
{
    class PlayerFragmentPresenter : IPlayerFragmentPresenter
    {
        private string groupName;

        public PlayerFragmentPresenter(string groupName)
        {
            this.groupName = groupName;
        }

        public async Task<Entry[]> GetFiveThrows(string playerName)
        {
            var player = await MainActivity.Database.GetPlayerAsync(groupName, playerName);
            var throws = await MainActivity.Database.GetThrowsAsync(player.Id);
            var lst = throws
                .Select(a => new List<int>() { a.First, a.Second, a.Third })
                .SelectMany(a => a, (b, c) => c)
                .GroupBy(a => a)
                .Select(a => new Item(a.Key.ToString(), a.Count()))
                .OrderByDescending(a => a.Value)
                .Take(5)
                .ToList();
            return Utils.MakeEntries(lst);
        }

        public async Task<Entry[]> GetFiveAverages(string playerName)
        {
            var player = await MainActivity.Database.GetPlayerAsync(groupName, playerName);
            var throws = await MainActivity.Database.GetThrowsAsync(player.Id);
            var lst = throws
                .Select(a => a.First + a.Second + a.Third)
                .GroupBy(a => a)
                .Select(a => new Item(a.Key.ToString(), a.Count()))
                .OrderByDescending(a => a.Value)
                .Take(5)
                .ToList();
            return Utils.MakeEntries(lst);
        }
    }
}