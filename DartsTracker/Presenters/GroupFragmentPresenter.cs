using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DartsTracker.Interfaces;
using DartsTracker.Models;
using Microcharts;

namespace DartsTracker.Presenters
{
    public class GroupFragmentPresenter: IGroupFragmentPresenter
    { 
        private string groupName;
        public GroupFragmentPresenter( string groupName)
        {
            this.groupName = groupName;
        }

        public async Task<Entry[]> GetGameModeEntries(string [] keys)
        {
           var c = await MainActivity.Database.GetGamesAsync();
           var lst = keys
                .Select(a => new { Key = a })
                .GroupJoin(c.Where(b => b.GroupName == groupName).ToList(),
                c => c.Key,
                d => d.GameType.ToString(),
                (e, f) => new Item(e.Key, f.Count()))
                .ToList();
            return Utils.MakeEntries(lst);
        }

        public async Task<Entry[]> GetWinsEntries()
        {
            var games = await MainActivity.Database.GetGamesAsync();
            var playersNames = await MainActivity.Database.GetPlayersAsync(groupName);
            var lst = playersNames
                 .Select(a => new 
                 {
                     a.Id,
                     a.Name 
                 })
                 .GroupJoin(games.Where(b => b.GroupName == groupName).ToList(),
                 c => c.Id,
                 d => d.WinnerId,
                 (e, f) => new Item(e.Name, f.Count()))
                 .ToList();
            return Utils.MakeEntries(lst);
        }

        public async Task<Entry[]> GetRoundsEntries()
        {
            var bounds = new List<Tuple<int, int>> 
            { 
                new Tuple<int, int> (1, 10),
                new Tuple<int, int> (11, 25),
                new Tuple<int, int> (26, 50),
                new Tuple<int, int> (51, 100),
                new Tuple<int, int> (101, 1000)
            }; 
            var games = await MainActivity.Database.GetGamesAsync();
            var g = games.Where(b => b.GroupName == groupName).ToList();
            var lst = bounds
                .Select(a => new Item(
                    a.Item1.ToString() + " - " + a.Item2.ToString(), 
                    g.Where(b => b.Rounds >= a.Item1 && b.Rounds <= a.Item2).Count()))
                .ToList();
            return Utils.MakeEntries(lst);
        }

    }
}