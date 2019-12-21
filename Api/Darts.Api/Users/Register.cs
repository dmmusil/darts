using System;
using System.Threading;
using System.Threading.Tasks;
using Darts.Api.Infrastructure;
using Darts.Infrastructure;
using Darts.Players;
using Darts.Players.Persistence;
using Darts.Players.Persistence.SQL;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Extensions.Logging;
using OneOf;

namespace Darts.Api.Users
{
    public class Register
    {
        private readonly IMediator _mediator;

        public Register(IMediator mediator)
        {
            _mediator = mediator;
        }

        [FunctionName("Register")]
        public async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = "users/register")] HttpRequest req,
            ILogger log)
        {
            var command = await req.ReadJsonBody<RegisterUser.Request>();

            var result = await _mediator.Send(command);

            return result.Match<IActionResult>(
                success => new OkResult(),
                failed => new BadRequestObjectResult(failed.Reason));
        }
    }

    public static class RegisterUser
    {
        public class Request : IRequest<OneOf<RegistrationSuccess, RegistrationFailed>>
        {
            public string Username { get; set; }
            public string Password { get; set; }
            public string Email { get; set; }
        }

        public class Handler : IRequestHandler<Request, OneOf<RegistrationSuccess, RegistrationFailed>>
        {
            private readonly PlayerRepository _repository;

            public Handler(PlayerRepository repository)
            {
                _repository = repository;
            }

            public async Task<OneOf<RegistrationSuccess, RegistrationFailed>> Handle(
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
                    return new RegistrationFailed(e.FailureReason);
                }
                try
                {
                    await _repository.Save(user);
                }
                catch (UniqueConstraintViolationException)
                {
                    return new RegistrationFailed("This username or Email already exists.");
                }

                return new RegistrationSuccess();
            }
        }
    }

    public class RegistrationFailed
    {
        public RegistrationFailed(string reason)
        {
            Reason = reason;
        }

        public string Reason { get; }
    }

    public class RegistrationSuccess
    {
    }
}
