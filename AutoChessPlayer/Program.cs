using ChessDotNet;
using ChessDotNet.AI.Agents;
using System;
using System.Linq;

namespace AutoChessPlayer
{
    class Program
    {
        static void Main(string[] args)
        {
            var showGameMoves = false; // true to show all moves
            var showGameResults = true;
            var gamesToPlay = 10000;
            var gameBatchSize = 100;

            Console.WriteLine("Auto playing chess...\n");

            Console.WriteLine("Random vs Random\n");

            var stats = new GameStats();

            var gamePlayer = new AutoChessGamePlayer
            {
                WhitePlayer = new RandomAgent(),
                BlackPlayer = new RandomAgent(),
                GameStats = stats
            };

            if (showGameMoves)
            {
                gamePlayer.MoveMade += (before, move, after) =>
                {
                    var text = $"{before.FullMoveNumber}. {move}";

                    if (Console.CursorLeft + text.Length > Console.WindowWidth)
                        Console.Write($"\n{text}\t");
                    else
                        Console.Write($"{text}\t");
                };
            }

            if (showGameResults)
            { 
                gamePlayer.GameConcluded += (game, result) =>
                {
                    if (showGameMoves)
                        Console.Write("\n\n");

                    Console.Write($"{stats.GameCount}. {result.Result}");

                    if (result.Result == "Checkmate")
                        Console.WriteLine($" ({result.Winner})");
                    else
                        Console.WriteLine();

                    if (showGameMoves)
                        Console.WriteLine();
                };
            }

            for (int i = 0; i < (gamesToPlay / gameBatchSize); i++)
            {
                gamePlayer.PlayGames(gameBatchSize);

                Console.WriteLine();
                stats.Display();

                Console.WriteLine();
            }
        }
    }
}
