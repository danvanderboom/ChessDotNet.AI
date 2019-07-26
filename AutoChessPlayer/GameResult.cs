using System;
using System.Collections.Generic;
using System.Text;
using ChessDotNet;

namespace AutoChessPlayer
{
    public class GameResult
    {
        public int MoveCount;
        public int CheckCount;

        public string Result;
        public Player Winner = Player.None;

        public int MinScore = int.MaxValue;
        public int MaxScore = int.MinValue;

        public override string ToString()
        {
            var winner = Winner == Player.None ? string.Empty : $" ({Winner})";
            return $"{Result}{winner}, {MoveCount} moves, squares controlled (from {MinScore} to {MaxScore})";
        }
    }
}
