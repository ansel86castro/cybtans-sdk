using Cybtans.Expressions.Ast;
using System.Linq;
using System.Reflection;

namespace Cybtans.Expressions
{
    public class QueryASTContext : ASTContext
    {
        public QueryASTContext()
        {
            AddFunction("all", typeof(Enumerable).GetMethod("All"));
            AddFunction("any", (MethodInfo)typeof(Enumerable).GetMember("Any")[1]);
            AddFunction("count", (MethodInfo)typeof(Enumerable).GetMember("Count")[1]);
            AddFunction("size", (MethodInfo)typeof(Enumerable).GetMember("Count")[0]);

        }
    }
}
