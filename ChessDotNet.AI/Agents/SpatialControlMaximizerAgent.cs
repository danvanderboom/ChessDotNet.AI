using System;
using System.Linq;
using System.Collections.Generic;
using System.Text;
using ChessDotNet.AI.Scoring;

namespace ChessDotNet.AI.Agents
{
    public class SpatialControlMaximizerAgent : IChessAgent
    {
        Random random = new Random();

        public Move GenerateMove(ChessGame game)
        {
            if (!game.HasAnyValidMoves(game.WhoseTurn))
                return null;

            var gameData = game.GetGameCreationData();

            var ScoredMoves = new Dictionary<Move, double>();

            foreach (var move in game.GetValidMoves(game.WhoseTurn))
            {
                var continuedGame = new ChessGame(gameData);
                continuedGame.MakeMove(move, true);

                var score = Score(continuedGame);
                ScoredMoves.Add(move, score);
            }

            List<KeyValuePair<Move, double>> moveScores = ScoredMoves.ToList();

            moveScores.Sort(
                delegate (KeyValuePair<Move, double> pair1, KeyValuePair<Move, double> pair2)
                {
                    return pair1.Value.CompareTo(pair2.Value);
                }
            );

            var kvp = game.WhoseTurn == Player.White ? moveScores.Last() : moveScores.First();
            var bestScore = kvp.Value;

            var bestScoreMoves = moveScores.Where(s => s.Value == bestScore).ToList();

            var randomIndex = random.Next(0, bestScoreMoves.Count);

            return bestScoreMoves[randomIndex].Key;
        }

        public ChessBoardScoreCard CreateScoreCard(ChessGame game)
        {
            var scoreCard = new ChessBoardScoreCard();
            //var scoreCard = CloneScoreCard(CreateZeroScoreCard());

            foreach (var move in game.GetValidMoves(game.WhoseTurn))
            {
                var pos = PositionToTuple(move.NewPosition);
                var playerFactor = game.WhoseTurn == Player.White ? 1 : -1;
                scoreCard.Data[pos.rank, pos.file] += playerFactor;
            }

            var gameData = game.GetGameCreationData();
            gameData.WhoseTurn = (gameData.WhoseTurn == Player.White) ? Player.Black : Player.White;

            var switchedGame = new ChessGame(gameData);

            foreach (var move in switchedGame.GetValidMoves(switchedGame.WhoseTurn))
            {
                var pos = PositionToTuple(move.NewPosition);
                var playerFactor = game.WhoseTurn == Player.White ? -1 : +1;
                scoreCard.Data[pos.rank, pos.file] += playerFactor;
            }

            return scoreCard;
        }

        public int Score(ChessGame game)
        {
            var scoreCard = CreateScoreCard(game);

            var score = 0;

            for (int rank = 0; rank <= scoreCard.Data.GetUpperBound(0); rank++)
                for (int file = 0; file <= scoreCard.Data.GetUpperBound(1); file++)
                    score += scoreCard.Data[rank, file];

            return score;
        }

        public static (int rank, int file) PositionToTuple(Position position)
        {
            return (position.Rank - 1, (int)position.File);
        }

        int Minimax(ChessGame game, int depth)
        {
            if (game.IsCheckmated(game.WhoseTurn))
                return game.WhoseTurn == Player.White ? int.MaxValue : int.MinValue;

            if (game.IsStalemated(game.WhoseTurn))
                return 0;

            if (depth == 0)
                return Score(game);

            if (game.WhoseTurn == Player.White)
            {
                var score = int.MinValue;

                foreach (var move in game.GetValidMoves(game.WhoseTurn))
                {
                    var childGame = new ChessGame(game.GetGameCreationData());
                    childGame.MakeMove(move, true);
                    //movesConsidered++;

                    score = Math.Max(score, Minimax(childGame, depth - 1));
                }

                return score;
            }
            else
            {
                var score = int.MaxValue;

                foreach (var move in game.GetValidMoves(game.WhoseTurn))
                {
                    var childGame = new ChessGame(game.GetGameCreationData());
                    childGame.MakeMove(move, true);
                    //movesConsidered++;

                    score = Math.Min(score, Minimax(childGame, depth - 1));
                }

                return score;
            }
        }
    }
}
