namespace Iter.Core.Options
{
    public class ApplicationOptions
    {
        public JwtConfiguration JwtConfiguration { get; set; } = new JwtConfiguration();

        public EmailSettings EmailSettings { get; set; } = new EmailSettings();

        public RabbitMqSettings RabbitMqSettings { get; set; } = new RabbitMqSettings();

        public ApplicationSettings ApplicationSettings { get; set; } = new ApplicationSettings();
    }
}
