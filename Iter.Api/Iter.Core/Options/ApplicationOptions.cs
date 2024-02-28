namespace Iter.Core.Options
{
    public class ApplicationOptions
    {
        public JwtConfiguration JwtConfiguration { get; set; } = new JwtConfiguration();

        public EmailSettings EmailSettings { get; set; } = new EmailSettings();

    }
}
