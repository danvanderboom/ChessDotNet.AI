# ChessDotNet.AI

Building some AI agents on top of the ChessDotNet library, which can be found at:
https://github.com/ProgramFOX/Chess.NET

There are two projects:

### AutoChessPlayer

This is a simple Console application that plays chess games and outputs to the console window (stdout).

Update local variables in Program.Main to adjust how it plays:

* showChessMoves - false by default, set to true to show all chess moves played on both sides of every game. Algebraic chess notation.
* showGameResults - true by default, shows the summarized result of each game
* gamesToPlay - 10,000 by default
* gameBatchSize - 100 games / batch

The default agent for both white and black players is RandomAgent, which very simply picks a random valid move. When playing Random vs Random, I've found each to checkmate the other side 7.5% of the time.

```
var gamePlayer = new AutoChessGamePlayer
{
    WhitePlayer = new RandomAgent(),
    BlackPlayer = new RandomAgent()
};

// play a single game
var gameResult = gamePlayer.PlayGame();

// play a batch of 100 games
var stats = gamePlayer.PlayGames(100);
```

You can switch out one or both of the agents, try the first version of the SpatialControlMaximizerAgent, or build your own IChessAgent and play it against another agent--or itself.

For example:
```
var gamePlayer = new AutoChessGamePlayer
{
    WhitePlayer = new RandomAgent(),
    BlackPlayer = new SpatialControlMaximizerAgent()
};
```

### ChessDotNet.AI

This project is the beginning of a collection of tools for building AI agents on top of ChessDotNet, which provides a nice, simple set of abstractions and APIs for manipulating chess pieces on a board, cloning board positions, validating moves, enumerating valid moves, and much more.

So far these chess AI tools are organized into:
* Agents - IChessAgent, RandomAgent, SpatialControlMaximizerAgent, and additional agents
* Scoring - ChessBoardScoreCard, an 8x8 int matrix for scoring position-specific data
* Search - prioritized queues, solution search trees, and related tools to support prioritized Monte Carlo expansion toward goal

# Create A New AI Agent

Implementing a new agent is easy using the IChessAgent interface. 
```
public interface IChessAgent
{
    Move GenerateMove(ChessGame game);
}
```

For example, the random play agent looks like this:
```
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
```
