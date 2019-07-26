using System;
using System.Linq;
using System.Collections.Generic;
using System.Text;

namespace ChessDotNet.AI.Agents
{
    public class RandomAgent : IChessAgent
    {
        Random random;

        public RandomAgent()
        {
            random = new Random();
        }

        public Move GenerateMove(ChessGame game)
        {
            if (!game.HasAnyValidMoves(game.WhoseTurn))
                return null;

            var moves = game.GetValidMoves(game.WhoseTurn).ToList();
            var randomIndex = random.Next(0, moves.Count);
            return moves[randomIndex];
        }
    }
}
