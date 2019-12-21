using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Darts.Infrastructure;
using Darts.Players;
using Darts.Api.Infrastructure;
using MediatR;
using System.Threading;
using Darts.Players.Persistence;
using OneOf;
using Darts.Players.Persistence.SQL;

namespace Darts.Api.Users
{
    public class Login
    {
        private readonly IMediator _mediator;

        public Login(IMediator mediator)
        {
            _mediator = mediator;
        }

        [FunctionName("Login")]
        public async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = "login")] HttpRequest req,
            ILogger log)
        {
            var loginInfo = await req.ReadJsonBody<Authenticate.Request>();

            var result = await _mediator.Send(loginInfo);

            return result.Match<IActionResult>(
                success => new OkObjectResult(SecureUsername.From(loginInfo.Username)),
                failure => new BadRequestObjectResult("Incorrect username or password.")
            );
        }
    }

    public class Authenticate
    {
        public class Request : IRequest<OneOf<Success, Failure>>
        {
            public string Username { get; set; }
            public string Password { get; set; }
        }

        public class Handler : IRequestHandler<Request, OneOf<Success, Failure>>
        {
            private readonly PlayerRepository _repository;

            public Handler(PlayerRepository repository)
            {
                _repository = repository;
            }

            public async Task<OneOf<Success, Failure>> Handle(Request command, CancellationToken token)
            {
                var user = await _repository.Load(x => x.Username == command.Username);
                return user.Match(
                    some => some.Authenticate(command.Password)
                            ? new Success()
                            : (OneOf<Success, Failure>)new Failure(),
                    none => new Failure());
            }
        }

        public class Failure { }

        public class Success { }
    }


}
