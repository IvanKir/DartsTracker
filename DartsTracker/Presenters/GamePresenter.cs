using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DartsTracker.Interfaces;
using DartsTracker.Models;

namespace DartsTracker.Presenters
{
    public class GamePresenter : IGamePresenter
    {
        private IGameView view;

        private string gameName;
        private string groupName;
        private string[] invalidThrows = 
        {
            "23", "29", "31", "35", "37",
            "41", "43", "44", "46", "47", 
            "49", "52", "53", "55", "56", 
            "58", "59"
        };

        private int gameType;
        private int playerNumber;
        private int playingPlayer = 0;
        private int round = 0;
        private int winner = -1;

        private bool end = false;
        private Game game;
        private List<Player> players;
        private List<List<int>> playersScore = new List<List<int>>();
        private Dictionary<int, List<Throw>> playerThrows = new Dictionary<int, List<Throw>>();

        public GamePresenter(IGameView view)
        {
            this.view = view;
        }

        // Sets necesary parameters.
        private async Task SetParams(string groupName, string gameName, int gameType)
        {
            this.gameName = gameName;
            this.groupName = groupName;
            this.gameType = gameType;
            players = await MainActivity.Database.GetPlayersAsync(groupName);
            playerNumber = players.Count();
            var header = MakeHeader();
            view.SetAdapter(header);
        }
        public Throw GetThrow(int row, int column)
        {
            return playerThrows[column][row];
        }

        public async Task MakeNewGame(string groupName, string gameName, int GameType)
        {
            game = new Game(groupName, gameName, GameType);
            await MainActivity.Database.SaveGameAsync(game);
            var temp = await MainActivity.Database.GetGamesAsync();
            game = temp.LastOrDefault();
            await SetParams(groupName, gameName, gameType);
            playersScore.Add(new List<int>());
            view.AddTableRow();
            round = 1;
            for (int i = 0; i < players.Count; i++)
            {
                playersScore[0].Add(GameType);
                view.UpdateTableAt(0, i, GameType.ToString());
                playerThrows[i] = new List<Throw>();
            }
            view.SetPlayerText(players[playingPlayer].Name);
        }

        public async Task ContinueGame(string groupName, string gameName)
        {
            game = await MainActivity.Database.GetGameAsync(groupName, gameName);
            await SetParams(groupName, gameName, game.GameType);
            // Get players throws of current game.
            for (int i = 0; i < players.Count; i++)
                playerThrows[i] = await MainActivity.Database.GetThrowsAsync(players[i].Id, game.Id);

            // + 1 because of first row of maximum score (from gametype)
            var rowsNumber = playerThrows.Values.Max(a => a.Count) + 1;
            playingPlayer = playerThrows.Values.Where(a => a.Count == rowsNumber - 1).Count();
            round = rowsNumber - 1;
            if (playingPlayer >= playerNumber)
            {
                round++;
                playingPlayer = 0;
            }
            MakeTable(rowsNumber);
            //Winner might be set in Maktable.
            if (end)
                return;
            view.SetPlayerText(players[playingPlayer].Name);
        }

        public async Task<bool> OnBtnClicked(List<string> values)
        {
            if (end || !CheckValues(values))
                return false;

            byte[] val = values.Select(a => byte.Parse(a)).ToArray();
            if (!CheckByteValues(val))
                return false;

            var playerThrow = MakeThrow(val);
            playerThrows[playingPlayer].Add(playerThrow);
            await MainActivity.Database.SaveThrowAsync(playerThrow);

            //setting new row in table.
            if (playingPlayer == 0)
            {
                view.AddTableRow();
                playersScore.Add(new List<int>());
            }

            var throwSum = playerThrow.First + playerThrow.Second + playerThrow.Third;
            throwSum = CheckScore(round, playingPlayer, throwSum);
            // Update table inside this class.
            var newScore = playersScore[playersScore.Count - 2][playingPlayer] - throwSum;     
            playersScore.Last().Add(newScore);
            // Update table in Activity.
            view.UpdateTableAt(round, playingPlayer, newScore.ToString());
            view.ScrollToLast();

            if (end)
            {
                game.WinnerId = players[winner].Id;
                game.Rounds = (short)round;
                await MainActivity.Database.SaveGameAsync(game);
                return false;
            }

            if (++playingPlayer == playerNumber)
            {
                ++round;
                playingPlayer = 0;
            };
            view.SetPlayerText(players[playingPlayer].Name);
            return true;
        }

        public async Task<bool> OnOkButtonClicked(int gameRound, int player, List<string> values)
        {
            //if game is over player can not change the table. 
            if (end || playerThrows[player].Count > gameRound)
                return true;

            byte[] val = values.Select(a => byte.Parse(a)).ToArray();
            if (!CheckByteValues(val))
                return false;
            
            playerThrows[player][gameRound - 1].First = byte.Parse(values[0]);
            playerThrows[player][gameRound - 1].Second = byte.Parse(values[1]);
            playerThrows[player][gameRound - 1].Third = byte.Parse(values[2]);

            int throwSum = playerThrows[player][gameRound - 1].First
            + playerThrows[player][gameRound - 1].Second
            + playerThrows[player][gameRound - 1].Third;
            throwSum = CheckScore(gameRound, player, throwSum);
            var newScore = playersScore[gameRound - 1][player] - throwSum;
            playersScore[gameRound][player] = newScore;
            view.UpdateTableAt(gameRound, player, newScore.ToString());

            await MainActivity.Database.SaveThrowAsync(playerThrows[player][gameRound - 1]);
            return true;
        }

        private void MakeTable(int rowsNumber)
        {
            // adds rows to table
            for (int i = 0; i < rowsNumber; i++) {
                view.AddTableRow();
                playersScore.Add(new List<int>());
            }
            // adds starting score to table
            for (int i = 0; i < playerNumber; i++) {
                playersScore[0].Add(gameType);
                view.UpdateTableAt(0, i, gameType.ToString());
            }
            // computing and seting score to table
            for (int i = 0; i < playerThrows.Count; i++)
            {
                for (int j = 0; j < playerThrows[i].Count; j++)
                {
                    var throwSum = playerThrows[i][j].First + playerThrows[i][j].Second + playerThrows[i][j].Third;
                    throwSum = CheckScore(j + 1, i, throwSum);
                    var newScore = playersScore[j][i] - throwSum;
                    playersScore[j + 1].Add(newScore);
                    view.UpdateTableAt(j + 1, i, newScore.ToString());
                }
            }
        }

        private List<string> MakeHeader()
        {
            //first row of table.
            var header = new List<string>() { "" };
            foreach (var player in players)
            {
                header.Add(player.Name);
            }
            return header;
        }

        private Throw MakeThrow(byte[] values)
        {
           return new Throw
                (
                    players[playingPlayer].Id,
                    game.Id,
                    values[0],
                    values[1],
                    values[2]
                );
        }

        private bool CheckValues(List<string> values)
        {
            if(values.Any(a => string.IsNullOrEmpty(a) || invalidThrows.Contains(a)))
            {
                view.MakeToast(Resource.String.toast_game_throw);
                return false;
            }
            return true;
        }

        private bool CheckByteValues(byte[] arr) 
        {
            if (arr.Any(a => a > 60 || a < 0))
            {
                view.MakeToast(Resource.String.toast_game_throw);
                return false;
            }
            return true;
        }

        private int CheckScore(int round, int playingPlayer, int throwSum)
        {
            var newScore = playersScore[round - 1][playingPlayer] - throwSum;
            if (newScore < 0)
                return 0;
            //sets winner
            if (newScore == 0)
            {
                winner = playingPlayer;
                view.SetWinner(players[winner].Name);
                end = true;
            }
            return throwSum;
        }
    }
}
