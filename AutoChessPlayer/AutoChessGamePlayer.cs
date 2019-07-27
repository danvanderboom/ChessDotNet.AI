using System;
using System.Collections.Generic;
using System.Text;
using ChessDotNet;
using ChessDotNet.AI.Agents;

namespace AutoChessPlayer
{
    public class AutoChessGamePlayer
    {
        public IChessAgent WhiteAgent { get; set; }
        public IChessAgent BlackAgent { get; set; }

        public GameStats GameStats { get; set; }

        public event Action<ChessGame, Move, ChessGame> MoveMade;
        public event Action<ChessGame, GameResult> GameConcluded;

        public AutoChessGamePlayer()
        {
            GameStats = new GameStats();
        }

        public GameStats PlayGames(int gameCount)
        {
            if (WhiteAgent == null)
                throw new ArgumentNullException(nameof(WhiteAgent));

            if (BlackAgent == null)
                throw new ArgumentNullException(nameof(BlackAgent));

            for (int i = 0; i < gameCount; i++)
                PlayGame();

            return GameStats;
        }

        public GameResult PlayGame(ChessGame startingPosition = null)
        {
            var game = startingPosition ?? new ChessGame();

            var gameResult = new GameResult();

            GameStats.GameCount += 1;

            while (game.WhoseTurn != Player.None)
            {
                if (game.DrawCanBeClaimed)
                {
                    gameResult.Result = "Draw";
                    game.ClaimDraw("Boredom");

                    GameStats.DrawCount += 1;

                    break;
                }

                var move = (game.WhoseTurn == Player.White ? WhiteAgent : BlackAgent).GenerateMove(game);

                GameStats.MoveCount += 1;
                gameResult.MoveCount += 1;

                var gameBeforeMove = new ChessGame(game.GetGameCreationData());
                game.MakeMove(move, true);
                var gameAfterMove = new ChessGame(game.GetGameCreationData());

                // raise event
                MoveMade?.Invoke(gameBeforeMove, move, gameAfterMove);

                if (game.IsInCheck(game.WhoseTurn))
                {
                    GameStats.CheckCount += 1;
                    gameResult.CheckCount += 1;
                }

                if (game.IsStalemated(game.WhoseTurn))
                {
                    GameStats.StalemateCount += 1;
                    gameResult.Result = "Stalemate";

                    break;
                }

                if (game.IsCheckmated(game.WhoseTurn))
                {
                    gameResult.Result = "Checkmate";
                    gameResult.Winner = game.WhoseTurn == Player.White ? Player.Black : Player.White;

                    if (gameResult.Winner == Player.White)
                        GameStats.WhiteCheckmateCount += 1;
                    else
                        GameStats.BlackCheckmateCount += 1;

                    break;
                }
            }

            // raise event
            GameConcluded?.Invoke(new ChessGame(game.GetGameCreationData()), gameResult);

            return gameResult;
        }
    }
}
