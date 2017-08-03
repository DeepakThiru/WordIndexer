using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Search
{
    class Program
    {
        static Dictionary<string, Dictionary<string, int>> wordHash;

        static void Main(string[] args)
        {
            Debug.WriteLine("Search app started!");
            
            string indexFileName = "index.txt";

            Debug.WriteLine("Reading the Index file..");
            readHashFromFile(indexFileName);

            if (args.Length == 3 && String.Equals(args[1], "AND", StringComparison.Ordinal))
            {
                processAndCondition(args[0], args[2]);
            }
            else
            {
                processAllWords(args);
            }

            Debug.WriteLine("Search app completed successfully!");
        }

        private static void processAndCondition(string v1, string v2)
        {
            Console.WriteLine("Searching for '{0}' AND '{1}' ...", v1, v2);

            if (wordHash.ContainsKey(v1) && wordHash.ContainsKey(v2))
            {
                Dictionary<string, int> fileNames = wordHash[v1].Keys.Intersect(wordHash[v2].Keys).ToDictionary(x => x, x => wordHash[v1][x]);
                if (fileNames.Count == 0)
                {
                    Console.WriteLine("No matches found.");
                }
                else
                {
                    Console.WriteLine("Found in:");

                    foreach (string fileName in fileNames.Keys)
                    {
                        Console.WriteLine("\t{0}", fileName);
                    }
                }
            }
            else
            {
                Console.WriteLine("No matches found.");
            }
        }

        private static void processAllWords(string[] args)
        {
            foreach (string word in args)
            {
                Console.WriteLine("Searching for '{0}' ...", word);

                if (wordHash.ContainsKey(word))
                {
                    Dictionary<string, int> fileNames = wordHash[word];
                    Console.WriteLine("Found in:");

                    // Grade search results based on number of occurrences
                    var sortedFileNames = from pair in fileNames
                                      orderby pair.Value descending
                                      select pair.Key;

                    foreach (string fileName in sortedFileNames)
                    {
                        Console.WriteLine("\t{0}", fileName);
                    }
                }
                else
                {
                    Console.WriteLine("No matches found.");
                }

                Console.WriteLine();

            }
        }

        private static void readHashFromFile(string indexFileName)
        {
            if (File.Exists(indexFileName) && File.ReadAllText(indexFileName).Length != 0)
            {
                wordHash = JsonConvert.DeserializeObject<Dictionary<string, Dictionary<string, int>>>(File.ReadAllText(indexFileName));
            }
            else
            {
                throw new FileNotFoundException("Index file is missing or doesn't hold any data!");
            }
        }
    }
}
