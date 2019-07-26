using System;
using System.Collections.Generic;
using System.Text;

namespace ChessDotNet.AI.Scoring
{
    public class ChessBoardScoreCard
    {
        public int[,] Data;

        static int[,] CreateZeroScoreCard() => new int[8, 8]
        {
            { 0, 0, 0, 0, 0, 0, 0, 0 },
            { 0, 0, 0, 0, 0, 0, 0, 0 },
            { 0, 0, 0, 0, 0, 0, 0, 0 },
            { 0, 0, 0, 0, 0, 0, 0, 0 },
            { 0, 0, 0, 0, 0, 0, 0, 0 },
            { 0, 0, 0, 0, 0, 0, 0, 0 },
            { 0, 0, 0, 0, 0, 0, 0, 0 },
            { 0, 0, 0, 0, 0, 0, 0, 0 },
        };

        public ChessBoardScoreCard()
            => Data = CreateZeroScoreCard();

        public ChessBoardScoreCard(int[,] data)
            => Data = data;

        public ChessBoardScoreCard Clone()
            => new ChessBoardScoreCard((int[,])Data.Clone());

        public static void DisplayScoreCard(int[,] scoreCard)
        {
            for (int rank = scoreCard.GetUpperBound(0); rank >= 0; rank--)
            {
                for (int file = 0; file < scoreCard.GetLength(1); file++)
                    Console.Write(scoreCard[rank, file].ToString().PadLeft(4));

                Console.WriteLine();
            }

            Console.WriteLine();
        }
    }
}
