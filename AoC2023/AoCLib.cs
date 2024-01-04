namespace AoC2023
{
    internal class AoCLib
    {
        public static void FetchInputs(string year, string session)
        {
            var handler = new HttpClientHandler();
            handler.CookieContainer.Add(new System.Net.Cookie("session", session) { Domain = "adventofcode.com" });

            var client = new HttpClient(handler);

            for (int day = 1; day <= 25; day++)
            {
                var task = client.GetStringAsync($"https://adventofcode.com/{year}/day/{day}/input");
                task.Wait();
                File.WriteAllText("input" + day.ToString("D2") + ".txt", task.Result);
            }
        }
    }
}
