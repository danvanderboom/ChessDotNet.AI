using System;
using System.Collections.Generic;
using System.Text;

namespace ChessDotNet.AI.Agents
{
    public interface IChessAgent
    {
        Move GenerateMove(ChessGame game);
    }
}
