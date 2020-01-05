using System.Threading;
using System.Threading.Tasks;
using Darts.Infrastructure;
using MediatR;
using OneOf;
using OneOf.Types;

namespace Darts.Games
{
    public static class CreateCricketGame
    {
        public class Command : IRequest<OneOf<Id, None>>
        {
        }

        public class Handler : IRequestHandler<Command, OneOf<Id, None>>
        {
            private readonly CricketRepository Repo;

            public Handler(CricketRepository repo)
            {
                Repo = repo;
            }
            public async Task<OneOf<Id, None>> Handle(Command request, CancellationToken cancellationToken)
            {
                var game = new Cricket();
                game.Create();
                var state = await Repo.Save(game);
                return Id.From(state.Id);
            }
        }
    }
}
