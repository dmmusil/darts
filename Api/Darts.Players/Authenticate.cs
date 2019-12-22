using System.Threading;
using System.Threading.Tasks;
using JetBrains.Annotations;
using MediatR;
using OneOf;

namespace Darts.Players
{
    public static class Authenticate
    {
        [UsedImplicitly]
        public class Request : IRequest<OneOf<Success, Failure>>
        {
            public string Username { get; set; }
            public string Password { get; set; }
        }

        [UsedImplicitly]
        public class Handler : IRequestHandler<Request, OneOf<Success, Failure>>
        {
            private readonly PlayerRepository _repository;

            public Handler(PlayerRepository repository)
            {
                _repository = repository;
            }

            public async Task<OneOf<Success, Failure>> Handle(Request command,
                CancellationToken token)
            {
                var user =
                    await _repository.Load(x => x.Username == command.Username);
                return user.Match(
                    some => some.Authenticate(command.Password)
                        .Match<OneOf<Success, Failure>>(
                            authToken => new Success(authToken),
                            failed => new Failure()),
                    none => new Failure());
            }
        }

        public class Failure
        {
        }

        public class Success
        {
            public AuthToken AuthToken { get; }

            public Success(AuthToken authToken)
            {
                AuthToken = authToken;
            }
        }
    }
}
