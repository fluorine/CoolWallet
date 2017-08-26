using CoolWallet.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoolWallet.Tool
{
    class Program
    {
        const string Version = "v0.0.1-alpha";

        static void Main(string[] args)
        {
            try
            {
                if (args.Length < 1) return;

                switch (args[0].Trim())
                {
                    case "-g": // Generate wallet
                        GenerateSharedWallet(args[1].Trim(), args[2].Trim(), args[3].Trim());
                        break;
                    case "-h": // Display help
                        PrintUsage();
                        break;
                    case "-r": // Recover wallet using parts
                        if (args.Length > 1)
                        {
                            // Process parts provided as arguments
                            var rawParts = args.Reverse().Take(args.Length - 1);
                            RecoverPrivateKey(rawParts);
                        }
                        else
                        {
                            // Get parts by prompting
                        }
                        break;
                    default:
                        break;
                }
            } catch(ArgumentException e)
            {
                Console.WriteLine("Error:");
                Console.WriteLine("   " + e.Message);
            }
        }

        static void GenerateSharedWallet(string thresholdStr, string totalStr, string privateKey)
        {
            // Validate and convert values
            var valid = !string.IsNullOrWhiteSpace(thresholdStr)
                && !string.IsNullOrWhiteSpace(totalStr)
                && !string.IsNullOrWhiteSpace(privateKey);

            int threshold = -1;
            valid = valid && int.TryParse(thresholdStr, out threshold);

            int total = -1;
            valid = valid && int.TryParse(totalStr, out total);
            
            var signature = new WalletSignature()
            {
                PartsThreshold = threshold,
                PartsTotal = total
            };

            if (!valid || !signature.IsValid())
            {
                // Show instructions if parametes are invalid.
                PrintUsage();
            }
            else
            {
                // Generate wallet
                var wallet = new Wallet(signature, privateKey);

                if(!wallet.IsValid())
                {
                    PrintUsage();
                    return;
                }

                // Print parts
                Console.WriteLine("Parts generated: ");
                foreach(var part in wallet.Parts)
                {
                    Console.WriteLine(" - " +  part.GetShortNotation());
                }
            }

        }

        static void PrintUsage()
        {
            Console.WriteLine($"Cool Wallet generator {Version}.");
            Console.WriteLine(" - Generate a cool wallet's parts:");
            Console.WriteLine("     -g <thresholdQuantity> <totalSharesQuantity> \"<privateKey>\"");
            Console.WriteLine(" - Recover a Private Key from parts:");
            Console.WriteLine("     -r \"<part1>\" \"<part2>\" [...]");
            Console.WriteLine(" - Show this help:");
            Console.WriteLine("     -h");
        }

        static void RecoverPrivateKey(IEnumerable<string> rawParts)
        {
            var deserializedParts = new List<WalletPart>();
            
            foreach(var rawPart in rawParts)
            {
                var part = new WalletPart(rawPart);
                deserializedParts.Add(part);
            }

            var wallet = new Wallet(deserializedParts);

            Console.WriteLine("Recovered Private Key:\n  " + wallet.PrivateKey);
        }
    }
}
