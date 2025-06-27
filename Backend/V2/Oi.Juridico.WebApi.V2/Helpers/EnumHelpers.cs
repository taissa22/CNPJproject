namespace Oi.Juridico.WebApi.V2.Helpers
{
    public static class EnumHelpers
    {
        public static T ParseOrDefault<T>(string value, T defaultValue) where T : struct
        {
            if (Enum.TryParse<T>(value, true, out T result))
            {
                return result;
            }
            return defaultValue;
        }
    }
}
