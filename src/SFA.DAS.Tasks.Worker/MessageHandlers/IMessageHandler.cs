namespace SFA.DAS.Tasks.Worker.MessageHandlers
{
    public interface IMessageHandler<in T> where T : new()
    {
        void Handle(T message);
    }
}
