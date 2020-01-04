using System.Threading;
using System.Threading.Tasks;
using JetBrains.Annotations;
using MediatR;
using OneOf;
using OneOf.Types;

namespace Darts.Games
{
    public static class AddCricketTurn
    {
        [UsedImplicitly]
        public class Command : IRequest<OneOf<Success, None>>
        {
            public Command(Turn turn, int gameId)
            {
                Turn = turn;
                GameId = gameId;
            }

            public Turn Turn { get; }
            public int GameId { get; }
        }

        [UsedImplicitly]
        public class Handler : IRequestHandler<Command, OneOf<Success, None>>
        {
            private readonly CricketRepository Repo;

            public Handler(CricketRepository repo)
            {
                Repo = repo;
            }

            public async Task<OneOf<Success, None>> Handle(Command request, CancellationToken cancellationToken)
            {
                var game = await Repo.Load(request.GameId);
                return await game.Match<Task<OneOf<Success, None>>>(async some =>
                {
                    some.AddTurn(request.Turn);
                    await Repo.Save(some);
                    return new Success();
                }, none => Task.FromResult<OneOf<Success, None>>(new None()));
            }
        }
    }
}
