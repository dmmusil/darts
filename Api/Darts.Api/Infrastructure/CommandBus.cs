using System;
using System.Threading.Tasks;
using Darts.Api.Users;
using Darts.Infrastructure;
using Darts.Players;
using SqlStreamStore.Infrastructure;
using static Darts.Api.Users.Login;

namespace Darts.Api.Infrastructure
{
    public static class CommandBus
    {
        private static StreamStoreBase Store;

        public static void Initialize(StreamStoreBase store)
        {
            Store = store;
        }

        public static async Task<Result> Send(Command command)
        {
            var repository = new SqlStreamStoreRepository(Store);
            try
            {
                switch (command)
                {
                    case RegisterUser.Request c:
                        await new RegisterUser.Handler(repository).Handle(c);
                        return Result.Success();
                    case Authenticate.Request c:
                        await new Authenticate.Handler(repository).Handle(c);
                        return Result.Success(new { Token = (SecureUsername)c.Username });
                    default:
                        throw new ArgumentOutOfRangeException(
                            $"Can't handle command of type ${command.GetType().Name}");
                }
            }
            catch (DomainException e)
            {
                return Result.Failure(e.FailureReason, e.StatusCode);
            }
        }
    }
}
