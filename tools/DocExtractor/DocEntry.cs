using System.Collections.Generic;

namespace DocExtractor
{
    internal sealed class DocEntry
    {
        public string MemberId { get; }
        public string MemberKind { get; }
        public string TypeName { get; }
        public string MemberName { get; }
        public string Summary { get; }
        public IReadOnlyList<ParamDoc> Params { get; }
        public string Returns { get; }
        public string Since { get; }
        public string Status { get; }

        public DocEntry(
            string memberId,
            string memberKind,
            string typeName,
            string memberName,
            string summary,
            IReadOnlyList<ParamDoc> @params,
            string returns,
            string since = "",
            string status = "live")
        {
            MemberId = memberId;
            MemberKind = memberKind;
            TypeName = typeName;
            MemberName = memberName;
            Summary = summary;
            Params = @params;
            Returns = returns;
            Since = since;
            Status = status;
        }
    }

    internal sealed class ParamDoc
    {
        public string Name { get; }
        public string Description { get; }

        public ParamDoc(string name, string description)
        {
            Name = name;
            Description = description;
        }
    }
}
