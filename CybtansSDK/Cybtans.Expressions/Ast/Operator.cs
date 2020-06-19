using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cybtans.Expressions.Ast
{
    public enum Operator
    {
        OP_NONE =0,
        OP_ADD = 1,
        OP_SUB = 2,
        OP_DIV = 3,
        OP_MUL = 4,
        OP_AND = 5,
        OP_OR = 6,
        OP_EQUAL = 7,
        OP_DISTINT = 8,
        OP_LIKE = 9,
        OP_LESS = 10,
        OP_GREATHER = 11,
        OP_GREATHER_EQ = 12,
        OP_LESS_EQ = 13,
    }
}
