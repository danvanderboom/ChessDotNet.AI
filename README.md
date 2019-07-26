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
    BlackPlayer = new RandomAgent(),
    GameStats = stats
};
```

You can switch out one or both of the agents, try the first version of the SpatialControlMaximizerAgent, or build your own IChessAgent and play it against another agent--or itself.

For example:
```
var gamePlayer = new AutoChessGamePlayer
{
    WhitePlayer = new RandomAgent(),
    BlackPlayer = new SpatialControlMaximizerAgent(),
    GameStats = stats
};
```
