using System.Collections.Generic;
using System.Threading.Tasks;
using DartsTracker.Models;
using SQLite;

namespace DartsTracker
{
    public class DartDatabase
    {
        readonly SQLiteAsyncConnection database;
       
        public DartDatabase(string dbPath)
        {
            database = new SQLiteAsyncConnection(dbPath);
            database.CreateTableAsync<Group>().Wait();
            database.CreateTableAsync<Player>().Wait();
            database.CreateTableAsync<Game>().Wait();
            database.CreateTableAsync<Throw>().Wait();
        }
        public Task<List<Group>> GetGroupAsync()
        {
            return database.Table<Group>().ToListAsync();
        }


        public Task<Group> GetGroupAsync(int id)
        {
            return database.Table<Models.Group>()
                            .Where(i => i.Id == id)
                            .FirstOrDefaultAsync();
        }
        public Task<Group> GetGroupAsync(string name)
        {
            return database.Table<Group>()
                            .Where(i => i.Name == name)
                            .FirstOrDefaultAsync();
        }

        public Task<int> SaveGroupAsync(Group group)
        {
            if (group.Id != 0)
            {
                return database.UpdateAsync(group);
            }
            else
            {
                return database.InsertAsync(group);
            }
        }

        public Task<int> DeleteGroupAsync(Models.Group group)
        {
            return database.DeleteAsync(group);
        }


        public Task<List<Player>> GetPlayersAsync(string groupName)
        {
            return database.Table<Player>()
                .Where(a => a.GroupName == groupName)
                .ToListAsync();
        }

        public Task<List<Player>> GetPlayersAsync()
        {
            return database.Table<Player>().ToListAsync();
        }


        public Task<Player> GetPlayerAsync(int id)
        {
            return database.Table<Player>()
                            .Where(i => i.Id == id)
                            .FirstOrDefaultAsync();
        }

        public Task<Player> GetPlayerAsync(string groupName, string playerName)
        {
            return database.Table<Player>()
                            .Where(i => i.GroupName == groupName && i.Name == playerName)
                            .FirstOrDefaultAsync();
        }

        public Task<int> SavePlayerAsync(Player player)
        {
            if (player.Id != 0)
            {
                return database.UpdateAsync(player);
            }
            else
            {
                return database.InsertAsync(player);
            }
        }

        public Task<int> DeletePlayerAsync(Player player)
        {
            return database.DeleteAsync(player);
        }

        public Task<List<Throw>> GetThrowsAsync()
        {
            return database.Table<Throw>().ToListAsync();
        }

        public Task<List<Throw>> GetThrowsAsync(int playerId, int gameId)
        {
            return database.Table<Throw>()
                  .Where(a => a.PlayerId == playerId && a.GameId == gameId)
                  .ToListAsync();
        }

        public Task<List<Throw>> GetThrowsAsync(int playerId)
        {
            return database.Table<Throw>()
                  .Where(a => a.PlayerId == playerId)
                  .ToListAsync();
        }
        public Task<List<Throw>> GetThrowsGameAsync(int gameId)
        {
            return database.Table<Throw>()
                  .Where(a => a.GameId == gameId)
                  .ToListAsync();
        }


        public Task<Throw> GetThrowAsync(int id)
        {
            return database.Table<Throw>()
                            .Where(i => i.Id == id)
                            .FirstOrDefaultAsync();

        }

        public Task<int> SaveThrowAsync(Throw t)
        { 
            if (t.Id != 0)
            {
                return database.UpdateAsync(t);
            }
            else
            {
                return database.InsertAsync(t);
            }
        }

        public Task<int> DeleteThrowAsync(Throw t)
        {
            return database.DeleteAsync(t);
        }

        public Task<List<Game>> GetGamesAsync()
        {
            return database.Table<Game>().ToListAsync();
        }
        public Task<List<Game>> GetGamesAsync(string groupName)
        {
            return database.Table<Game>()
                .Where(a => a.GroupName == groupName)
                .ToListAsync();
        }


        public Task<Game> GetGameAsync(int id)
        {
            return database.Table<Game>()
                            .Where(i => i.Id == id)
                            .FirstOrDefaultAsync();
        }
        public Task<Game> GetGameAsync(string groupName, string gameName)
        {
            return database.Table<Game>()
                            .Where(i => i.GroupName == groupName && i.Name == gameName)
                            .FirstOrDefaultAsync();
        }

        public Task<int> SaveGameAsync(Game game)
        {
            if (game.Id != 0)
            {
                return database.UpdateAsync(game);
            }
            else
            {
                return database.InsertAsync(game);
            }
        }

        public Task<int> DeleteGameAsync(Game game)
        {
            return database.DeleteAsync(game);
        }
    }
}