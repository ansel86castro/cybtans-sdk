using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cybtans.Expressions
{
    public class Token:IEquatable<Token>, IEquatable<TokenType>
    {
        public TokenType TokenType;
        public String Value;
        public int Col;
        public int Row;

        public Token(TokenType tokenType, String value)
        {
            TokenType = tokenType;
            Value = value;
        }

        public bool Match(TokenType t)
        {
            return this.TokenType == t;
        }

        public override string ToString()
        {
            return TokenType.ToString() + ":" + Value;
        }

        public static bool operator == (Token t, TokenType type)
        {
            return t?.TokenType == type;
        }

        public static bool operator != (Token t, TokenType type)
        {
            return t?.TokenType != type;
        }

        public override bool Equals(object obj)
        {
            if(obj is TokenType type)
            {
                return TokenType == type;
            }
            return base.Equals(obj);
        }

        public bool Equals(Token other)
        {
            return ReferenceEquals(this, other);
        }

        public bool Equals(TokenType other)
        {
            return TokenType == other;
        }
    }
}
