using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Xml.Linq;

namespace DocExtractor
{
    internal static class XmlDocParser
    {
        public static IReadOnlyList<DocEntry> Parse(string xmlPath, string? namespacePrefix = null)
        {
            var doc = XDocument.Load(xmlPath);
            var members = doc.Descendants("member");
            var entries = new List<DocEntry>();

            foreach (var member in members)
            {
                var nameAttr = member.Attribute("name")?.Value;
                if (nameAttr == null) { continue; }

                var kind = nameAttr.Length > 1 ? nameAttr[0].ToString() : string.Empty;
                if (kind != "T" && kind != "P" && kind != "M" && kind != "F") { continue; }

                var fullName = nameAttr.Substring(2);

                if (namespacePrefix != null && !fullName.StartsWith(namespacePrefix, StringComparison.Ordinal))
                {
                    continue;
                }
                var typeName = ExtractTypeName(fullName, kind);
                var memberName = ExtractMemberName(fullName, kind);
                var summary = NormalizeText(member.Element("summary"));
                var returns = NormalizeText(member.Element("returns"));

                var paramDocs = member.Elements("param")
                    .Select(p => new ParamDoc(
                        p.Attribute("name")?.Value ?? string.Empty,
                        NormalizeText(p)))
                    .ToList();

                entries.Add(new DocEntry(nameAttr, kind, typeName, memberName, summary, paramDocs, returns));
            }

            return entries;
        }

        private static string ExtractTypeName(string fullName, string kind)
        {
            if (kind == "T") { return fullName; }

            var parenIndex = fullName.IndexOf('(');
            var baseName = parenIndex >= 0 ? fullName.Substring(0, parenIndex) : fullName;
            var lastDot = baseName.LastIndexOf('.');
            if (lastDot < 0) { return baseName; }
            return baseName.Substring(0, lastDot);
        }

        private static string ExtractMemberName(string fullName, string kind)
        {
            if (kind == "T") { return fullName; }

            var parenIndex = fullName.IndexOf('(');
            var baseName = parenIndex >= 0 ? fullName.Substring(0, parenIndex) : fullName;
            var lastDot = baseName.LastIndexOf('.');
            if (lastDot < 0) { return baseName; }
            return baseName.Substring(lastDot + 1);
        }

        private static string NormalizeText(XElement? element)
        {
            if (element == null) { return string.Empty; }
            var sb = new System.Text.StringBuilder();
            foreach (var node in element.Nodes())
            {
                if (node is XText textNode)
                {
                    sb.Append(textNode.Value);
                }
                else if (node is XElement childElement)
                {
                    var cref = childElement.Attribute("cref")?.Value;
                    if (cref != null)
                    {
                        sb.Append(SimpleNameFromCref(cref));
                    }
                    else
                    {
                        sb.Append(childElement.Value);
                    }
                }
            }
            return NormalizeWhitespace(sb.ToString());
        }

        private static string SimpleNameFromCref(string cref)
        {
            var name = cref.Length > 2 && cref[1] == ':' ? cref.Substring(2) : cref;
            var backtick = name.IndexOf('`');
            if (backtick >= 0) { name = name.Substring(0, backtick); }
            var lastDot = name.LastIndexOf('.');
            return lastDot >= 0 ? name.Substring(lastDot + 1) : name;
        }

        private static string NormalizeWhitespace(string input)
        {
            return Regex.Replace(input.Trim(), @"\s+", " ");
        }
    }
}
