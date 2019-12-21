using MediatR;

namespace Darts.QueueProcessing
{
    public class RegistrationRequested : INotification
    {
        public string Username { get; set; }
        public string Email { get; set; }
    }
}
