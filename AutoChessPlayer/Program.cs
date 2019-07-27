using System;
using System.Linq;
using ChessDotNet;
using ChessDotNet.AI.Agents;

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

            var gamePlayer = new AutoChessGamePlayer
            {
                WhiteAgent = new RandomAgent(),
                BlackAgent = new RandomAgent() // BlackAgent = new SpatialControlMaximizerAgent()
            };

            //var stats = gamePlayer.GameStats;

            if (showGameMoves)
            {
                gamePlayer.MoveMade += (before, move, after) =>
                {
                    var text = $"{before.FullMoveNumber}. {move}";

                    if (Console.CursorLeft + text.Length > Console.WindowWidth - 6)
                        Console.WriteLine();

                    Console.Write($"{text}\t");
                };
            }

            if (showGameResults)
            { 
                gamePlayer.GameConcluded += (game, result) =>
                {
                    if (showGameMoves)
                        Console.Write("\n\nGame ");

                    Console.Write($"{gamePlayer.GameStats.GameCount}. {result.Result}");

                    if (result.Result == "Checkmate")
                        Console.Write($" ({result.Winner})");

                    Console.WriteLine($" - {result.MoveCount} moves");

                    if (showGameMoves)
                        Console.WriteLine();
                };
            }

            var batchCount = gamesToPlay / gameBatchSize;
            for (int i = 0; i < batchCount; i++)
            {
                gamePlayer.PlayGames(gameBatchSize);

                Console.WriteLine();
                gamePlayer.GameStats.Display();
                Console.WriteLine();
            }
        }
    }
}
