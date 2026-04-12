using System;
using System.IO;

namespace DocExtractor
{
    internal static class EntryPoint
    {
        private static int Main(string[] args)
        {
            if (args.Length < 2)
            {
                Console.Error.WriteLine("Usage: DocExtractor <path-to-xml> <output-dir> [--namespace-prefix <prefix>]");
                return 1;
            }

            var xmlPath = args[0];
            var outputDir = args[1];
            var namespacePrefix = ParseNamespacePrefix(args);

            if (!File.Exists(xmlPath))
            {
                Console.Error.WriteLine($"XML file not found: {xmlPath}");
                return 1;
            }

            Console.WriteLine($"Parsing: {xmlPath}");
            var entries = XmlDocParser.Parse(xmlPath, namespacePrefix);
            Console.WriteLine($"Found {entries.Count} doc entries");

            Console.WriteLine($"Emitting MDX to: {outputDir}");
            MdxEmitter.Emit(entries, outputDir);
            Console.WriteLine("Done.");

            return 0;
        }

        private static string? ParseNamespacePrefix(string[] args)
        {
            for (var i = 2; i < args.Length - 1; i++)
            {
                if (args[i] == "--namespace-prefix")
                {
                    return args[i + 1];
                }
            }
            return null;
        }
    }
}
