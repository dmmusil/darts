using System;
using System.ComponentModel;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Darts.Games.Persistence;
using Darts.Infrastructure;
using Microsoft.EntityFrameworkCore;

namespace Darts.Games
{
    public class Cricket : Aggregate<CricketState>
    {
        private bool Completed => _calculator.GameCompleted;
        private bool Started => _calculator.GameStarted;
        private readonly CricketScoreCalculator _calculator;
        public Cricket()
        {
            _calculator = new CricketScoreCalculator();
        }

        public void Create()
        {
            if (Id == 0)
            {
                Apply(new GameCreated());
            }
        }

        public void AddPlayer(PlayerId player)
        {
            if (!Started)
            {
                Apply(new PlayerAdded(player));
            }
        }

        public void AddTurn(Turn turn)
        {
            if (!Completed)
            {
                Apply(new TurnAdded(turn));
            }
        }

        protected override void ApplyEvent(Event e)
        {
            switch(e)
            {
                case GameCreated _:
                    State = new CricketState(Id);
                    break;
                case PlayerAdded p: 
                    _calculator.AddPlayer(p.Player);
                    break;
                case TurnAdded t:
                    _calculator.AddTurn(t.Turn);
                    break;
                default:
                    throw new InvalidEnumArgumentException($"{e.GetType()} can't be handled");
            }
        }

        protected override void LoadInternal(CricketState state)
        {
            Id = state.Id;
            foreach (var player in state.Players)
            {
                AddPlayer(PlayerId.From(player.PlayerId));
            }

            foreach (var turn in state.Turns)
            {
                var loading = Turn.ForPlayer(PlayerId.From(turn.PlayerId));
                foreach (var score in turn.Scores)
                {
                    loading.WithScores(Segment.From(score.Segment),
                        TurnMultiplier.From(score.Count));
                }

                loading.WithId(turn.Id);
                AddTurn(loading);
            }
        }
    }

    public class GameCreated : Event
    {
    }

    public class TurnAdded : Event
    {
        public TurnAdded(Turn turn)
        {
            Turn = turn;
        }

        public Turn Turn { get; }
    }

    public class PlayerAdded : Event
    {
        public PlayerId Player { get; }

        public PlayerAdded(PlayerId player)
        {
            Player = player;
        }
    }

    public class CricketRepository : EntityFrameworkRepository<Cricket, CricketState, GamesContext>
    {
        private readonly GamesContext DbContext;

        public CricketRepository(GamesContext dbContext) : base(dbContext)
        {
            DbContext = dbContext;
        }

        protected override Task<CricketState> LoadWithIncludes(Expression<Func<CricketState, bool>> selector)
        {
            return DbContext.CricketGames
                .Include(x => x.Players)
                .Include(x => x.Turns)
                .ThenInclude(x => x.Scores)
                .SingleOrDefaultAsync(selector);
        }
    }
}
