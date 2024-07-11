namespace bobotalents
{
    internal static class Program
    {
        private const string Charset = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789_.~-";

        private static readonly Dictionary<char, int> ZeroIndexInSet = Charset
            .Select((c, i) => new { c, i })
            .ToDictionary(x => x.c, x => x.i);

        private static void Main(string[] args)
        {
            const string url =
               // "https://www.bobo-talents.com/?c=mage&t=pZsqsrssstpYrirjrkrlI8p2q3q4p3rpp5rnpXrmp0rarbrcp~rdwdwewfC5p.pFqTqUqVqWpJpKsusvswp6pBqRpGqPqQpDsIsJ";
                // translates to: https://www.wowhead.com/classic/talent-calc/mage/2500052310211531-0550003123


                "https://www.bobo-talents.com/?c=priest&t=vhvMvNvOvPvrvvvqvwvxvivIvJvKvLvgvCvDvpvyvuDeDfDgDhDiC-DaDbDcpvvYv4vQv0v1v2v3C8C9C_C.C~wgvRwXIVIWwYDdvz";
            // translates to: https://www.wowhead.com/classic/talent-calc/priest/505230033005151-205051032
            
            
            var talents = GetTalents(url);

            if (talents == null) return;

            foreach (var talent in talents)
            {
                Console.WriteLine(
                    $"Talent ID {talent.Id}: Tab = {talent.Tab}, Index = {talent.Index}, Rank = {talent.Rank}, Level = {talent.Level}");
            }
        }

        private static string? GetUrlParameter(string url, string paramName)
        {
            var uri = new Uri(url);
            var query = uri.Query.TrimStart('?').Split('&');
            return (from param in query select param.Split('=') into keyValue where keyValue[0] == paramName select keyValue[1]).FirstOrDefault();
        }

        private static List<Talent>? GetTalents(string url)
        {
            var talentString = GetUrlParameter(url, "t");
            if (string.IsNullOrEmpty(talentString)) return null;

            var talents = new List<Talent>();
            var talentStringLength = talentString.Length;
            var level = 10;
            for (var i = 0; i < talentStringLength; i += 2)
            {
                var encodedId = talentString.Substring(i, Math.Min(2, talentStringLength - i));
                if (encodedId.Length != 2) continue;

                var firstChar = encodedId[0];
                var secondChar = encodedId[1];
                var decodedId = ZeroIndexInSet[firstChar] * Charset.Length + ZeroIndexInSet[secondChar];

                if (TalentMap.Talents.TryGetValue(decodedId, out var talentInfo))
                {
                    talents.Add(new Talent
                    {
                        Tab = talentInfo.Tab,
                        Index = talentInfo.Index,
                        Rank = talentInfo.Rank,
                        Level = level,
                        Id = decodedId
                    });
                    level++;
                }
                else
                {
                    // Handle the case where decodedId is not found in the talentMap
                    Console.WriteLine($"Talent ID {decodedId} not found in talent map.");
                }
            }

            return talents;
        }
    }
}