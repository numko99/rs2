using System.Reflection;
using Newtonsoft.Json;

namespace Iter.Infrastructure
{
    public static class Common
    {
        public static string GetManifestResourceName(string resourceName)
        {
            return Assembly.GetExecutingAssembly().GetManifestResourceNames()
                .Single(str =>
                str.EndsWith(resourceName, StringComparison.InvariantCulture));
        }

        public static string GetManifestResourceString(string resourceName)
        {
            string str = string.Empty;
            var a = GetManifestResourceName(resourceName);
            var manifestResourceStream = Assembly.GetExecutingAssembly().GetManifestResourceStream(a);
            if (manifestResourceStream != null)
            {
                using (StreamReader sr = new StreamReader(manifestResourceStream))
                {
                    str = sr.ReadToEnd().Replace(Environment.NewLine, " ");
                }
            }

            return str;
        }

        public static T? Deserialize<T>(string resourceName)
        {
            var a = GetManifestResourceString(resourceName);

            return JsonConvert.DeserializeObject<T>(a);
        }
    }
}
