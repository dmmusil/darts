using System.Threading;
using System.Threading.Tasks;
using Darts.Games.Persistence;
using MediatR;

namespace Darts.Games
{
    public static class AddCricketPlayer
    {
        public class Command : IRequest
        {
            public Command(string newPlayer)
            {
                NewPlayer = newPlayer;
            }

            public PlayerId NewPlayer { get; }
            public int GameId { get; private set; }

            public void ForGame(int gameId) => GameId = gameId;
        }

        public class Handler : IRequestHandler<Command>
        {
            private readonly CricketRepository Repo;

            public Handler(CricketRepository repo)
            {
                Repo = repo;
            }

            public async Task<Unit> Handle(Command request, CancellationToken cancellationToken)
            {
                var game = await Repo.Load(request.GameId);
                await game.Match(some =>
                    {
                        some.AddPlayer(request.NewPlayer);
                        return Repo.Save(some);
                    },
                    none => Task.FromResult((CricketState)null));
                return Unit.Value;
            }
        }
    }
}
