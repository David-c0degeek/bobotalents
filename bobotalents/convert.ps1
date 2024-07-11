# Path to the talents.txt file
$inputFilePath = "talents.txt"

# Read the contents of the file
$contents = Get-Content -Path $inputFilePath

# Initialize variables
$dictionaryEntries = @()

# Parse the contents
foreach ($line in $contents) {
    # Skip empty lines and lines that are just comments
    if ($line -match '^\s*$' -or $line -match '^\s*--') {
        continue
    }

    # Extract the key and values using regex
    if ($line -match '\[(\d+)\] = {t = (\d+), i = (\d+), r = (\d+)},?') {
        $key = $matches[1]
        $t = $matches[2]
        $i = $matches[3]
        $r = $matches[4]
        $dictionaryEntries += "`t`  {$key, new Talent { Tab = $t, Index = $i, Rank = $r }},"
    }
}

# Remove the last comma
if ($dictionaryEntries.Count -gt 0) {
    $dictionaryEntries[-1] = $dictionaryEntries[-1].TrimEnd(',')
}

# Define the output file path
$outputFilePath = "TalentDictionary.cs"

# Write the dictionary entries to the C# file
@"
using System.Collections.Generic;

namespace YourNamespace
{
    public class Talent
    {
        public int Tab { get; set; }
        public int Index { get; set; }
        public int Rank { get; set; }
    }

    public static class TalentDictionary
    {
        public static Dictionary<int, Talent> Talents = new Dictionary<int, Talent>
        {
            $(($dictionaryEntries -join "`n            "))
        };
    }
}
"@ | Set-Content -Path $outputFilePath

Write-Output "C# dictionary seed file generated at $outputFilePath"
