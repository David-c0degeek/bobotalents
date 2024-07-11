namespace bobotalents;

internal static class TalentDecoder
{
    private const string Charset = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789_.~-";

    private static readonly Dictionary<char, int> CharToIndexMap = Charset
        .Select((c, i) => new { c, i })
        .ToDictionary(x => x.c, x => x.i);

    public static List<Talent> GetTalents(string url)
    {
        var talentString = GetUrlParameter(url, "t");
        if (string.IsNullOrEmpty(talentString))
        {
            throw new ArgumentException("Talent string not found in URL", nameof(url));
        }

        var talents = new List<Talent>();
        var level = 10;

        for (var i = 0; i < talentString.Length; i += 2)
        {
            if (i + 1 >= talentString.Length) break;

            var encodedId = talentString.Substring(i, 2);
            var decodedId = DecodeId(encodedId);

            if (TalentMap.Talents.TryGetValue(decodedId, out var talentInfo))
            {
                talents.Add(new Talent
                {
                    Tab = talentInfo.Tab,
                    Index = talentInfo.Index,
                    Rank = talentInfo.Rank,
                    Level = level++,
                    Id = decodedId
                });
            }
            else
            {
                Console.WriteLine($"Warning: Talent ID {decodedId} not found in talent map.");
            }
        }

        return talents;
    }

    private static string? GetUrlParameter(string url, string paramName)
    {
        var uri = new Uri(url);
        var query = uri.Query.TrimStart('?').Split('&');
        return query
            .Select(param => param.Split('='))
            .FirstOrDefault(keyValue => keyValue[0] == paramName)?[1];
    }

    private static int DecodeId(string encodedId)
    {
        if (encodedId.Length != 2)
            throw new ArgumentException("Encoded ID must be 2 characters long", nameof(encodedId));

        return CharToIndexMap[encodedId[0]] * Charset.Length + CharToIndexMap[encodedId[1]];
    }
}