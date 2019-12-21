using MediatR;
using SqlStreamStore.Streams;
using System.Threading.Tasks;

namespace Darts.QueueProcessing
{
    public static class MessageConverter
    {
        public static Task<INotification> ToNotification(this StreamMessage message)
        {
            return Task.FromResult((INotification)new RegistrationRequested());
        }
    }
}
