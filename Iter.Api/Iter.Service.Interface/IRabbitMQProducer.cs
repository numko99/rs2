namespace Iter.Services.Interface
{
    public interface IRabbitMQProducer
    {
        public void SendMessage<T>(T message);
    }
}
