using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace ChessDotNet.AI.Search
{
    public class SearchTreeNode : INotifyPropertyChanged
    {
        public ChessGame Game;
        public Move Move;

        public int Depth => Parent == null ? 0 : Parent.Depth + 1;

        public int PreviousScore;

        int _Score;
        public int Score
        {
            get => _Score;
            set
            {
                // save current score as previous
                PreviousScore = _Score;
                // update current score to new value
                _Score = value;
                // report update
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Score)));
            }
        }

        public SearchTreeNode Parent;
        public SortedSet<SearchTreeNode> Children;
        public Dictionary<string, int> Properties;

        public event PropertyChangedEventHandler PropertyChanged;

        public SearchTreeNode(ChessGame game)
        {
            Game = new ChessGame(game.GetGameCreationData());

            Move = null;

            Children = new SortedSet<SearchTreeNode>(new SearchTreeNodeComparer());
            Properties = new Dictionary<string, int>();
        }

        public SearchTreeNode(ChessGame game, Move move, SearchTreeNode parent) : this(game)
        {
            Move = move;
            Parent = parent;

            parent?.Children.Add(this);

            Game.MakeMove(move, true);
        }

        public override string ToString() => $"{Move?.Player}: {Move} (Score: {Score}, Depth: {Depth})";

        public class SearchTreeNodeComparer : Comparer<SearchTreeNode>
        {
            public override int Compare(SearchTreeNode x, SearchTreeNode y) => x.Score - y.Score;
        }
    }
}
