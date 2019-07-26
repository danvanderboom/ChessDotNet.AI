using System;
using System.Collections.Generic;
using System.Text;

namespace AutoChessPlayer
{
    public class GameStats
    {
        public int GameCount = 0;
        public int CheckCount = 0;
        public int MoveCount = 0;
        //public int CastleCount = 0;

        public int WhiteCheckmateCount = 0;
        public int BlackCheckmateCount = 0;

        public int DrawCount = 0;
        public int StalemateCount = 0;

        public int MinScore = int.MaxValue;
        public int MaxScore = int.MinValue;

        public int LowSquareControl = 0;
        public int HighSquareControl = 0;

        public int TotalRange = 0;
        public int MinRange = int.MaxValue;
        public int MaxRange = int.MinValue;

        string FormatPercent(int x, int y) => (x / (double)y).ToString("P1");

        public void Display()
        {
            Console.WriteLine($"Games: {GameCount}");
            Console.WriteLine($"Checks: {CheckCount} (avg {(CheckCount / (double)GameCount).ToString("F1")} per game)");
            Console.WriteLine($"Checkmates (White): {WhiteCheckmateCount} ({FormatPercent(WhiteCheckmateCount, GameCount)})");
            Console.WriteLine($"Checkmates (Black): {BlackCheckmateCount} ({FormatPercent(BlackCheckmateCount, GameCount)})");
            Console.WriteLine($"Stalemates: {StalemateCount} ({FormatPercent(StalemateCount, GameCount)})");
            Console.WriteLine($"Draws: {DrawCount} ({FormatPercent(DrawCount, GameCount)})");
            //Console.WriteLine();
            //Console.WriteLine($"Min Score: {MinScore}");
            //Console.WriteLine($"Max Score: {MaxScore}");
            //Console.WriteLine();
            //Console.WriteLine($"Average Low Square Control: {(LowSquareControl / (double)GameCount).ToString("F1")}");
            //Console.WriteLine($"Average High Square Control: {(HighSquareControl / (double)GameCount).ToString("F1")}");
            //Console.WriteLine();
            //Console.WriteLine($"Average Range: {(TotalRange / (double)GameCount).ToString("F1")}");

            //Console.WriteLine($"Min Range: {LowRange}");
            //Console.WriteLine($"Max Range: {HighRange}");
        }
    }
}
