using System.Collections.Generic;
using Darts.Games.Persistence;
using Darts.Infrastructure;
using ValueOf;

namespace Darts.Games
{
    public class _301
    {
    }

    public class Cricket : Aggregate<CricketState>
    {
        private bool Completed { get; set; }
        private bool Started { get; set; }
        private readonly CricketScoreCalculator _calculator;
        public Cricket()
        {
            _calculator = new CricketScoreCalculator();
        }

        public void AddPlayer(PlayerId player)
        {
            if (!Started)
            {
                _calculator.AddPlayer(player);
            }
        }

        public void AddTurn(Turn turn)
        {
            if (!Completed)
            {
                _calculator.AddTurn(turn);
            }
        }

        public override void Load(CricketState state)
        {
            throw new System.NotImplementedException();
        }

        public override CricketState Memoize()
        {
            throw new System.NotImplementedException();
        }
    }

    internal class CricketScoreCalculator
    {
        private readonly Dictionary<PlayerId, CricketScore> _playerScores = new Dictionary<PlayerId, CricketScore>();
        public bool GameCompleted = false;

        public void AddTurn(Turn turn)
        {
            _playerScores[turn.Player].Apply(turn);
        }

        public void AddPlayer(PlayerId player)
        {
            _playerScores.Add(player, new CricketScore());
        }
    }

    internal class CricketScore
    {
        private readonly Dictionary<int, int> Scores = new Dictionary<int, int>
        {
            {20, 0},
            {19, 0},
            {18, 0},
            {17, 0},
            {16, 0},
            {15, 0},
            {25, 0}
        };

        public int _20s => Scores[20];
        public int _19s => Scores[19];
        public int _18s => Scores[18];
        public int _17s => Scores[17];
        public int _16s => Scores[16];
        public int _15s => Scores[15];
        public int Bulls => Scores[25];

        public void Apply(Turn turn)
        {
            Scores[turn.First.Value.score] += turn.First.Value.multiplier;
            Scores[turn.Second.Value.score] += turn.Second.Value.multiplier;
            Scores[turn.Third.Value.score] += turn.Third.Value.multiplier;
        }
    }

    public class Turn
    {
        public Turn(PlayerId player, Dart first, Dart second, Dart third)
        {
            Player = player;
            First = first;
            Second = second;
            Third = third;
        }

        public PlayerId Player { get; }
        public Dart First { get; }
        public Dart Second { get; }
        public Dart Third { get; }
    }

    public class Dart : ValueOf<(int score, int multiplier), Dart>
    {
    }
}
