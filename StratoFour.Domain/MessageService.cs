using System.Reactive.Subjects;

namespace StratoFour.Domain
{
    public class MessageService
    {
        private readonly ISubject<string> _messageStream = new ReplaySubject<string>(1);

        public IObservable<string> MessageStream => _messageStream;

        public void Publish(string message)
        {
            _messageStream.OnNext(message);
        }
    }
}
