using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Index
{
    class Program
    {
        static Dictionary<string, Dictionary<string, int>> wordHash;

        static void Main(string[] args)
        {
            Console.WriteLine("Index app started!");
            string fileExtension = Path.GetExtension(args[0]);

            if (String.Equals(fileExtension, ".txt", StringComparison.OrdinalIgnoreCase) || String.Equals(fileExtension, ".log", StringComparison.OrdinalIgnoreCase))
            {
                string indexFileName = "index.txt";

                Console.WriteLine("Reading the Index file..");
                readHashFromFile(indexFileName);

                Console.WriteLine("Reading data from file - '{0}'", args[0]);
                string[] wordsInFile = File.ReadAllText(args[0]).Split(' ');
                updateWordHash(wordsInFile, args[0]);

                Console.WriteLine("Updating the Index file..");
                writeHashToFile(indexFileName); 
            }
            else
            {
                throw new ArgumentException("Cannot process files other than .txt and .log!");
            }

            Console.WriteLine("Index app completed successfully!");
        }

        private static void readHashFromFile(string indexFileName)
        {
            if (File.Exists(indexFileName) && File.ReadAllText(indexFileName).Length != 0)
            {
                wordHash = JsonConvert.DeserializeObject<Dictionary<string, Dictionary<string, int>>>(File.ReadAllText(indexFileName));
            }
            else
            {
                wordHash = new Dictionary<string, Dictionary<string, int>>();
            }
        }

        private static void updateWordHash(string[] wordsInFile, string filePath)
        {
            string fileName = Path.GetFileName(filePath);

            foreach (string word in wordsInFile)
            {
                if (wordHash.ContainsKey(word))
                {
                    Dictionary<string, int> fileNames = wordHash[word];

                    if (!fileNames.ContainsKey(fileName))
                    {
                        fileNames.Add(fileName, 1);
                    }
                    else
                    {
                        fileNames[fileName] = fileNames[fileName] + 1;
                    }

                    wordHash[word] = fileNames;
                }
                else
                {
                    wordHash.Add(word, new Dictionary<string, int> { { fileName, 1 } });
                }
            }
        }

        private static void writeHashToFile(string indexFileName)
        {
            File.WriteAllText(indexFileName, JsonConvert.SerializeObject(wordHash));
        }
    }
}
