namespace bobotalents;

internal static class Program
{
    private static void Main(string[] args)
    {
        const string url = "https://www.bobo-talents.com/?c=priest&t=vhvMvNvOvPvrvvvqvwvxvivIvJvKvLvgvCvDvpvyvuDeDfDgDhDiC-DaDbDcpvvYv4vQv0v1v2v3C8C9C_C.C~wgvRwXIVIWwYDdvz";
        // translates to: https://www.wowhead.com/classic/talent-calc/priest/505230033005151-205051032
        
        // "https://www.bobo-talents.com/?c=mage&t=pZsqsrssstpYrirjrkrlI8p2q3q4p3rpp5rnpXrmp0rarbrcp~rdwdwewfC5p.pFqTqUqVqWpJpKsusvswp6pBqRpGqPqQpDsIsJ";
        // translates to: https://www.wowhead.com/classic/talent-calc/mage/2500052310211531-0550003123
        
        try
        {
            var talents = TalentDecoder.GetTalents(url);

            if (talents.Count == 0)
            {
                Console.WriteLine("No talents found.");
                return;
            }

            foreach (var talent in talents)
            {
                Console.WriteLine($"Talent ID {talent.Id}: Tab = {talent.Tab}, Index = {talent.Index}, Rank = {talent.Rank}, Level = {talent.Level}");
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"An error occurred: {ex.Message}");
        }
    }
}