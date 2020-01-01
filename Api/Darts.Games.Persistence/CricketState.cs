using System;
using System.Collections.Generic;
using System.Linq;
using Darts.Infrastructure;
using ValueOf;

namespace Darts.Games.Persistence
{
    public class CricketState : State
    {
        protected bool Equals(CricketState other)
        {
            return Players.SequenceEqual(other.Players)&& Turns.SequenceEqual(other.Turns);
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj))
            {
                return false;
            }

            if (ReferenceEquals(this, obj))
            {
                return true;
            }

            if (obj.GetType() != this.GetType())
            {
                return false;
            }

            return Equals((CricketState) obj);
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(Players, Turns);
        }

        public List<Player> Players { get; set; } = new List<Player>();
        public List<Turn> Turns { get; set; } = new List<Turn>();

        public CricketState(int id) : base(id)
        {
        }
    }

    public class Turn : State
    {
        protected bool Equals(Turn other)
        {
            return Scores.SequenceEqual(other.Scores) &&
                   PlayerId.Equals(other.PlayerId) && Order == other.Order &&
                   GameId == other.GameId;
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj))
            {
                return false;
            }

            if (ReferenceEquals(this, obj))
            {
                return true;
            }

            if (obj.GetType() != this.GetType())
            {
                return false;
            }

            return Equals((Turn) obj);
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(Scores, PlayerId, Order, GameId);
        }

        /// <summary>
        /// A turn has a list of scores. Can be empty.
        /// </summary>
        public List<Score> Scores { get; set; } = new List<Score>();
     
        /// <summary>
        /// A turn belongs to a player.
        /// </summary>
        public Guid PlayerId { get; set; }
        public Player Player { get; set; }
        
        /// <summary>
        /// Track turn order so they can be presented in order for reporting.
        /// </summary>
        public int Order { get; set; }

        /// <summary>
        /// A turn only exists as part of a game.
        /// </summary>
        public int GameId { get; set; }
        public CricketState Game { get; set; }

        public Turn(int id) : base(id)
        {
        }
    }

    /// <summary>
    /// A row for a unique score hit during a turn.
    /// If a player hits 3x 20s and 2x 18s there would be two Score rows for that turn.
    /// There cannot be multiple of the same Score per turn. If multiples are hit during a turn,
    /// they should be coalesced into a single row.
    /// </summary>
    public class Score
    {
        protected bool Equals(Score other)
        {
            return Segment == other.Segment && Count == other.Count && TurnId == other.TurnId;
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj))
            {
                return false;
            }

            if (ReferenceEquals(this, obj))
            {
                return true;
            }

            if (obj.GetType() != this.GetType())
            {
                return false;
            }

            return Equals((Score) obj);
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(Segment, Count, TurnId);
        }

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
        protected bool Equals(Player other)
        {
            return PlayerId.Equals(other.PlayerId);
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj))
            {
                return false;
            }

            if (ReferenceEquals(this, obj))
            {
                return true;
            }

            if (obj.GetType() != this.GetType())
            {
                return false;
            }

            return Equals((Player) obj);
        }

        public override int GetHashCode()
        {
            return PlayerId.GetHashCode();
        }

        /// <summary>
        /// A reference to a player from the Player context.
        /// </summary>
        public Guid PlayerId { get; set; }
        
        /// <summary>
        /// Players only exist in the Games context as part of a game.
        /// </summary>
        public int GameId { get; set; }
        public CricketState Game { get; set; }
    }

    public class PlayerId : ValueOf<Guid, PlayerId>
    {
    }
}
