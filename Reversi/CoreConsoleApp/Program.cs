using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Core;

namespace CoreConsoleApp
{
    class MainClass
    {
        const string usageText = "Usage: ConsoleApp inputfile.txt outputfile.txt";

        public static int Main(string[] args)
        {
            StreamWriter writer = null;

            args = new string[] { "input.txt" };
            if (args.Length < 1)
            {
                Console.WriteLine(usageText);
                return 1;
            }

            try
            {
                if (args.Length > 1)
                {
                    writer = new StreamWriter(args[1]);
                    Console.SetOut(writer);
                }

                if (args.Length > 0)
                {
                    Console.SetIn(new StreamReader(args[0]));
                }
            }
            catch (IOException e)
            {
                TextWriter errorWriter = Console.Error;
                errorWriter.WriteLine(e.Message);
                errorWriter.WriteLine(usageText);
                return 1;
            }

            var lines = new List<string>();
            char piece;
            while (true)
            {
                var line = Console.ReadLine().Trim();
                if (line.Length == 1)
                {
                    piece = line[0];
                    break;
                }

                lines.Add(line);
            }

            var game = new Game(lines.ToArray());
            game.GeneratePossibleMove(piece);
            Console.WriteLine(game.ToString());
            Console.WriteLine(piece);

            writer?.Close();

            StreamWriter standardOutput = new StreamWriter(Console.OpenStandardOutput());
            standardOutput.AutoFlush = true;
            Console.SetOut(standardOutput);

            StreamReader standardInput = new StreamReader(Console.OpenStandardInput());
            Console.SetIn(standardInput);

            Console.ReadKey();

            return 0;
        }
    }
}