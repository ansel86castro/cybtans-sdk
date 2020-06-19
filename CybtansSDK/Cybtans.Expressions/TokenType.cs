using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cybtans.Expressions
{
    public enum TokenType
    {
        UNKNOW,
        ID,
        LBRACK,
        RBRACK,
        COLON,
        LCURLY,
        RCURLY,
        SEMICOLON,
        TRUE,
        FALSE,
        LPARENT,
        RPARENT,
        DOT,
        EQUAL,
        NEQUAL,
        LESS,
        LEQUAL,
        GREATER,
        GEQUAL,
        LIKE,
        OR,
        AND,
        INT,
        DOUBLE,
        STRING,
        NULL,
        PLUS,
        SUB,
        MUL,
        DIV,
        QUESTION,    
        EOF
    }
}
