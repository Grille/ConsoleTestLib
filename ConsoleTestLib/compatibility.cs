using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

#if !NETCOREAPP2_1_OR_GREATER
namespace System.Diagnostics.CodeAnalysis
{
    [AttributeUsage(AttributeTargets.Method)]
    internal class DoesNotReturnAttribute: Attribute { }
}
#endif