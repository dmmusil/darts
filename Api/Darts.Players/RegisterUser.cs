using System.Threading;
using System.Threading.Tasks;
using Darts.Infrastructure;
using JetBrains.Annotations;
using MediatR;
using OneOf;

namespace Darts.Players
{
    public static class RegisterUser
    {
        [UsedImplicitly]
        public class Request : IRequest<OneOf<Success, Failure>>
        {
            public string Username { get; set; }
            public string Password { get; set; }
            public string Email { get; set; }
        }

        [UsedImplicitly]
        public class Handler : IRequestHandler<Request, OneOf<Success, Failure>>
        {
            private readonly PlayerRepository _repository;

            public Handler(PlayerRepository repository)
            {
                _repository = repository;
            }

            public async Task<OneOf<Success, Failure>> Handle(
                Request request,
                CancellationToken cancellationToken)
            {
                var user = new Player();
                try
                {
                    user.Register(request.Username, request.Password, request.Email);
                }
                catch (DomainException e)
                {
                    return new Failure(e.FailureReason);
                }
                try
                {
                    await _repository.Save(user);
                }
                catch (UniqueConstraintViolationException)
                {
                    return new Failure("This username or Email already exists.");
                }

                return new Success();
            }
        }

        public class Failure
        {
            public Failure(string reason)
            {
                Reason = reason;
            }

            public string Reason { get; }
        }

        public class Success
        {
        }
    }

    
}
