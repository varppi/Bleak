namespace Bleak.Helpers
{
    public static class Web
    {
        public static string ChangeOrAddGetParameter(HttpRequest request, string key, string value)
            => "?" + string.Join("&", request.Query
                        .Where(kp => kp.Key != key)
                        .Concat([KeyValuePair.Create<string, Microsoft.Extensions.Primitives.StringValues>(key, new string[] { value })])
                        .Select(kp => $"{kp.Key}={kp.Value}"));

    }
}
