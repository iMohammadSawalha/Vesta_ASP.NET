namespace Vesta.Helpers
{
    public static class CustomCodeGenerator
    {
        public static string GenerateSimpleCode(int Length)
        {
            const string chars = "ABCDEFGHJKLMNPQRSTUVWXYZabcdefghkmnpqrstuvwxyz123456789";
            var random = new Random();

            // Generate a random string of length  8
            string randomString = new(Enumerable.Repeat(chars,  Length)
            .Select(s => s[random.Next(s.Length)]).ToArray());

            return randomString;
        }
    }
}