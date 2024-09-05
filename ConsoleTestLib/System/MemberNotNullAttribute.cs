using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace System.Diagnostics.CodeAnalysis;

#if NET46 || NETSTANDARD2_1
[AttributeUsage(AttributeTargets.Method)]
internal class MemberNotNullAttribute : Attribute
{
    public MemberNotNullAttribute(string args){}

    public MemberNotNullAttribute(params string[] args){}
}
#endif
