using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cybtans.Expressions.Ast
{
    public abstract class ASTNode
    {
        public int Col;

        public int Row;

        public abstract void CheckSemantic(IASTContext context);

        public abstract void GenSQL(IASTContext context, StringBuilder sb, int tabOffset);

        public abstract void GenOData(IASTContext context, StringBuilder sb, int tabOffset);
    }

}
