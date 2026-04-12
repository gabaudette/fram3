using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace DocExtractor
{
    internal static class MdxEmitter
    {
        public static void Emit(IReadOnlyList<DocEntry> entries, string outputDir)
        {
            Directory.CreateDirectory(outputDir);

            var typeEntries = entries
                .Where(e => e.MemberKind == "T")
                .OrderBy(e => e.TypeName)
                .ToList();

            foreach (var typeEntry in typeEntries)
            {
                var memberEntries = entries
                    .Where(e => e.TypeName == typeEntry.TypeName && e.MemberKind != "T")
                    .ToList();

                var mdx = BuildMdx(typeEntry, memberEntries);
                var fileName = SimpleTypeName(typeEntry.TypeName) + ".mdx";
                var filePath = Path.Combine(outputDir, fileName);
                File.WriteAllText(filePath, mdx, Encoding.UTF8);
            }

            var metaJs = BuildMetaJs(typeEntries);
            File.WriteAllText(Path.Combine(outputDir, "_meta.js"), metaJs, Encoding.UTF8);
        }

        private static string BuildMetaJs(IReadOnlyList<DocEntry> typeEntries)
        {
            var sb = new StringBuilder();
            sb.AppendLine("export default {");
            foreach (var entry in typeEntries)
            {
                var name = SimpleTypeName(entry.TypeName);
                sb.AppendLine($"  '{name}': '{name}',");
            }
            sb.AppendLine("}");
            return sb.ToString();
        }

        private static string BuildMdx(DocEntry typeEntry, IReadOnlyList<DocEntry> members)
        {
            var sb = new StringBuilder();
            var simpleName = SimpleTypeName(typeEntry.TypeName);

            sb.AppendLine("---");
            sb.AppendLine($"title: {simpleName}");
            sb.AppendLine($"description: \"{EscapeYaml(EscapeMdx(typeEntry.Summary))}\"");
            sb.AppendLine("---");
            sb.AppendLine();
            sb.AppendLine($"# {simpleName}");
            sb.AppendLine();

            if (!string.IsNullOrWhiteSpace(typeEntry.Summary))
            {
                sb.AppendLine(EscapeMdx(typeEntry.Summary));
                sb.AppendLine();
            }

            var constructors = members
                .Where(m => m.MemberKind == "M" && m.MemberName == "#ctor")
                .ToList();

            if (constructors.Count > 0)
            {
                sb.AppendLine("## Constructor");
                sb.AppendLine();
                foreach (var ctor in constructors)
                {
                    AppendMemberSection(sb, ctor, simpleName);
                }
            }

            var properties = members
                .Where(m => m.MemberKind == "P")
                .OrderBy(m => m.MemberName)
                .ToList();

            if (properties.Count > 0)
            {
                sb.AppendLine("## Properties");
                sb.AppendLine();
                foreach (var prop in properties)
                {
                    AppendMemberSection(sb, prop, prop.MemberName);
                }
            }

            var fields = members
                .Where(m => m.MemberKind == "F")
                .OrderBy(m => m.MemberName)
                .ToList();

            if (fields.Count > 0)
            {
                sb.AppendLine("## Fields");
                sb.AppendLine();
                foreach (var field in fields)
                {
                    AppendMemberSection(sb, field, field.MemberName);
                }
            }

            var methods = members
                .Where(m => m.MemberKind == "M" && m.MemberName != "#ctor")
                .OrderBy(m => m.MemberName)
                .ToList();

            if (methods.Count > 0)
            {
                sb.AppendLine("## Methods");
                sb.AppendLine();
                foreach (var method in methods)
                {
                    AppendMemberSection(sb, method, method.MemberName);
                }
            }

            return sb.ToString();
        }

        private static void AppendMemberSection(StringBuilder sb, DocEntry entry, string heading)
        {
            sb.AppendLine($"### {heading}");
            sb.AppendLine();

            if (!string.IsNullOrWhiteSpace(entry.Summary))
            {
                sb.AppendLine(EscapeMdx(entry.Summary));
                sb.AppendLine();
            }

            if (entry.Params.Count > 0)
            {
                sb.AppendLine("| Parameter | Description |");
                sb.AppendLine("| --- | --- |");
                foreach (var param in entry.Params)
                {
                    sb.AppendLine($"| `{param.Name}` | {EscapeMdx(param.Description)} |");
                }
                sb.AppendLine();
            }

            if (!string.IsNullOrWhiteSpace(entry.Returns))
            {
                sb.AppendLine($"**Returns:** {EscapeMdx(entry.Returns)}");
                sb.AppendLine();
            }
        }

        private static string SimpleTypeName(string fullTypeName)
        {
            var lastDot = fullTypeName.LastIndexOf('.');
            var name = lastDot >= 0 ? fullTypeName.Substring(lastDot + 1) : fullTypeName;
            var backtick = name.IndexOf('`');
            return backtick >= 0 ? name.Substring(0, backtick) : name;
        }

        private static string EscapeYaml(string value)
        {
            return value.Replace("\"", "\\\"");
        }

        private static string EscapeMdx(string value)
        {
            return value
                .Replace("<", "&lt;")
                .Replace(">", "&gt;")
                .Replace("{", "&#123;")
                .Replace("}", "&#125;");
        }
    }
}
