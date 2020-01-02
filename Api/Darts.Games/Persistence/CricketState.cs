using System;
using System.Collections.Generic;
using System.Linq;
using Darts.Infrastructure;
using ValueOf;

namespace Darts.Games.Persistence
{
    public class CricketState : State
    {
        public List<Player> Players { get; set; } = new List<Player>();
        public List<Turn> Turns { get; set; } = new List<Turn>();

        public CricketState(int id) : base(id)
        {
        }

        public override void Handle(IEnumerable<Event> events)
        {
            foreach (var @event in events)
            {
                switch (@event)
                {
                    case PlayerAdded p:
                        Players.Add(new Player {PlayerId = p.Player.Value});
                        break;
                    case TurnAdded t:
                        Turns.Add(new Turn
                        {
                            Order = Turns.Count,
                            PlayerId = t.Turn.PlayerId.Value,
                            Scores = t.Turn.Scores.Select(kvp => new Score
                            {
                                Count = kvp.Value.Value,
                                Segment = kvp.Key.Value
                            }).ToList()
                        });
                        break;
                }
            }
        }
    }

    public class Turn 
    { 
        public int Id { get; set; }
        /// <summary>
        /// A turn has a list of scores. Can be empty.
        /// </summary>
        public List<Score> Scores { get; set; } = new List<Score>();
     
        /// <summary>
        /// A turn belongs to a player.
        /// </summary>
        public Guid PlayerId { get; set; }
        
        /// <summary>
        /// Track turn order so they can be presented in order for reporting.
        /// </summary>
        public int Order { get; set; }

        /// <summary>
        /// A turn only exists as part of a game.
        /// </summary>
        public int GameId { get; set; }
        public CricketState Game { get; set; }
    }

    /// <summary>
    /// A row for a unique score hit during a turn.
    /// If a player hits 3x 20s and 2x 18s there would be two Score rows for that turn.
    /// There cannot be multiple of the same Score per turn. If multiples are hit during a turn,
    /// they should be coalesced into a single row.
    /// </summary>
    public class Score
    {
        /// <summary>
        ///  The section of the board that was hit.
        /// </summary>
        public int Segment { get; set; }
        /// <summary>
        /// The number of marks for the segment.
        /// </summary>
        public int Count { get; set; }

        /// <summary>
        /// Reference to the turn it was submitted with.
        /// </summary>
        public int TurnId { get; set; }
        public Turn Turn { get; set; }
    }

   

    public class Player
    {
        /// <summary>
        /// A reference to a player from the Player context.
        /// </summary>
        public Guid PlayerId { get; set; }
        public string Name { get; set; }
        /// <summary>
        /// Players only exist in the Games context as part of a game.
        /// </summary>
        public int GameId { get; set; }
        public CricketState Game { get; set; }
    }

    public class PlayerId : ValueOf<Guid, PlayerId>
    {
        public static implicit operator PlayerId(string guid) =>
            From(Guid.Parse(guid));
    }
}
