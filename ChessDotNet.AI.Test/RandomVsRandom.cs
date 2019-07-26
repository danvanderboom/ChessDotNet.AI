using System.Collections.Generic;
using NUnit.Framework;
using ChessDotNet.AI.Agents;
using ChessDotNet.AI.Test.Artifacts;
using System;
using ChessDotNet;

namespace Tests
{
    public class RandomVsRandom
    {
        //[SetUp]
        //public void Setup()
        //{
        //}

        [Test]
        public void RandomVsRandom_CheckmateBalance()
        {
            var randomWhiteAgent = new RandomAgent();
            var randomBlackAgent = new RandomAgent();

            var stats = new GameStats();

            for (int i = 0; i < 1000; i++)
                PlayGame(stats, randomWhiteAgent, randomBlackAgent);

            var epsilon = 0.01;
            var low = 0.075 - epsilon;
            var high = 0.075 + epsilon;

            // expect 7.5% checkmate for black
            var blackCheckmatePercent = stats.BlackCheckmateCount / (double)stats.GameCount;
            Assert.IsTrue(low <= stats.BlackCheckmateCount && stats.BlackCheckmateCount <= high);

            // expect 7.5% checkmate for white
            var whiteCheckmatePercent = stats.WhiteCheckmateCount / (double)stats.GameCount;
            Assert.IsTrue(low <= stats.WhiteCheckmateCount && stats.WhiteCheckmateCount <= high);
        }

        public void PlayGame(GameStats stats, IChessAgent whiteAgent, IChessAgent blackAgent)
        {
            var game = new ChessGame();
            var gameResult = new GameResult();

            stats.GameCount += 1;

            while (game.WhoseTurn != Player.None)
            {
                if (game.DrawCanBeClaimed)
                {
                    gameResult.Result = "Draw";
                    game.ClaimDraw("Boredom");

                    stats.DrawCount += 1;

                    break;
                }

                Move move = game.WhoseTurn == Player.White
                    ? whiteAgent.GenerateMove(game)
                    : blackAgent.GenerateMove(game);

                stats.MoveCount += 1;
                gameResult.MoveCount += 1;

                game.MakeMove(move, true);

                if (game.IsInCheck(game.WhoseTurn))
                {
                    stats.CheckCount += 1;
                    gameResult.CheckCount += 1;
                }

                if (game.IsStalemated(game.WhoseTurn))
                {
                    stats.StalemateCount += 1;
                    gameResult.Result = "Stalemate";

                    break;
                }

                if (game.IsCheckmated(game.WhoseTurn))
                {
                    gameResult.Result = "Checkmate";
                    gameResult.Winner = game.WhoseTurn == Player.White ? Player.Black : Player.White;

                    if (gameResult.Winner == Player.White)
                        stats.WhiteCheckmateCount += 1;
                    else
                        stats.BlackCheckmateCount += 1;

                    break;
                }
            }
        }

        
    }
}