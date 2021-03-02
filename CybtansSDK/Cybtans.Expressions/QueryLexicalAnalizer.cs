using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using boolean = System.Boolean;

namespace Cybtans.Expressions
{
    /// <summary>
    /// LL-K lexical analizer
    /// </summary>
    public class QueryLexicalAnalizer
    {
        String _string;
        String _lower;
        int index;
        int len;
        int simbolWidth;
        int Col;
        int Row;

        Dictionary<String, TokenType> keywords = new Dictionary<String, TokenType>();

        // lookahead list	
        Queue<Token> tokens = new Queue<Token>(4);

        Token current;

        public QueryLexicalAnalizer()
        {
            keywords.Add("and", TokenType.AND);
            keywords.Add("or", TokenType.OR);
            keywords.Add("null", TokenType.NULL);
            keywords.Add("like", TokenType.LIKE);
            keywords.Add("true", TokenType.TRUE);
            keywords.Add("false", TokenType.FALSE);

            keywords.Add("plus", TokenType.PLUS);
            keywords.Add("sub", TokenType.SUB);
            keywords.Add("mul", TokenType.MUL);
            keywords.Add("div", TokenType.DIV);
            keywords.Add("eq", TokenType.EQUAL);
            keywords.Add("lt", TokenType.LESS);
            keywords.Add("gt", TokenType.GREATER);
            keywords.Add("ge", TokenType.GEQUAL);
            keywords.Add("le", TokenType.LEQUAL);
            keywords.Add("ne", TokenType.NEQUAL);
            keywords.Add("qt", TokenType.QUESTION);
        }

        public QueryLexicalAnalizer(String source):this()
        {
            Source = source;
        }

        public string Source
        {
            get
            {
                return _string;
            }
            set
            {
                this._string = value;
                this._lower = value?.ToLowerInvariant();
                len = value?.Length ?? 0;
                tokens.Clear();
                current = null;
                Row = 0;
                Col = 0;
                index = 0;
                simbolWidth = 0;
            }
        }

        /// <summary>
        /// returns the current token in the stream
        /// </summary>
        /// <returns></returns>
        public Token CurrentToken
        {
            get
            {
                if (current == null)
                {
                    current = GetNextTokenFromStream();
                    if (current != null)
                    {
                        Col = current.Col;
                        Row = current.Row;
                    }
                }
                return current;
            }
        }

        /// <summary>
        /// Lookahead the next token in the stream ,maintaining the old current token
        /// </summary>
        /// <returns></returns>
        public Token PeekNextToken()
        {
            Token t = GetNextTokenFromStream();
            if (t != null)
            {
                Col = t.Col;
                Row = t.Row;
                tokens.Enqueue(t);
            }
            return t;
        }

        /// <summary>
        /// Get the next token in the stream ,making it the current token
        /// </summary>
        /// <returns></returns>
        public Token GetNextToken()
        {
            if (tokens.Count > 0)
            {
                current = tokens.Dequeue();
            }
            else
            {
                current = GetNextTokenFromStream();
                if (current != null)
                {
                    Col = current.Col;
                    Row = current.Row;
                }
            }
            return current;
        }

        /// <summary>
        /// remove the tokens in the lookahead
        /// </summary>
        public Token ConsumeLookAhead()
        {
            if (tokens.Count > 0)
            {
                return tokens.Dequeue();
            }
            return null;
        }

        protected Token GetNextTokenFromStream()
        {
            SkipSpaces();
            int simbol = Peek();

            if (simbol == -1)
                return null;

            String value;

            TokenType tokenType = IsSymbol(simbol);

            if (tokenType != TokenType.UNKNOW)
            {
                value = _string.Substring(index, simbolWidth);
                Skip(simbolWidth);
                return new Token(tokenType, value);
            }

            if (simbol == '\'')
            {
                value = ReadString();
                return new Token(TokenType.STRING, value);

            }
            else if (char.IsDigit((char)simbol))
            {
                value = ReadInteger();
                simbol = Peek();
                if (simbol == -1 || IsWhiteSpace(simbol))
                    return new Token(TokenType.INT, value);

                TokenType token = IsSymbol(simbol);
                if (token == TokenType.DOT)
                {
                    Read();
                    return new Token(TokenType.DOUBLE, value + "." + ReadInteger());
                }
                else if (token != TokenType.UNKNOW)
                {
                    return new Token(TokenType.INT, value);
                }
                else
                    throw new TokenMismatchException(String.Format("expecting number at COL: {0} ROW: {1}", Col, Row));

            }
            else if (simbol == '_' || Char.IsLetter((char)simbol))
            {
                value = ReadWord(simbol);
                TokenType keyword = GetKeyword(value);
                if (keyword != TokenType.UNKNOW)
                {
                    return new Token(keyword, value);
                }
                return new Token(TokenType.ID, value);
            }

            return null;

        }

        private Token GetTokenSymbol(int index)
        {
            char c = _string[index];
            Token token = null;
            char nextSimbol = index < (len - 1) ? _string[index + 1] : (char)0;
            switch (c)
            {
                case '{':
                    token = new Token(TokenType.LCURLY, "{");
                    simbolWidth = 1;
                    break;
                case '}':
                    token = new Token(TokenType.RCURLY, "}");
                    simbolWidth = 1;
                    break;
                case '[':
                    token = new Token(TokenType.LBRACK, "[");
                    simbolWidth = 1;
                    break;
                case ']':
                    token = new Token(TokenType.RBRACK, "]");
                    simbolWidth = 1;
                    break;
                case ':':
                    token = new Token(TokenType.COLON, ":");
                    simbolWidth = 1;
                    break;
                case ',':
                    token = new Token(TokenType.SEMICOLON, ",");
                    simbolWidth = 1;
                    break;
                case '.':
                    token = new Token(TokenType.DOT, ".");
                    simbolWidth = 1;
                    break;
                case '<':
                    if (nextSimbol == '=')
                    {
                        token = new Token(TokenType.LEQUAL, "<=");
                        simbolWidth = 2;
                    }
                    else
                    {
                        token = new Token(TokenType.LESS, "<");
                        simbolWidth = 1;
                    }
                    break;
                case '>':
                    if (nextSimbol == '=')
                    {
                        token = new Token(TokenType.GEQUAL, ">=");
                        simbolWidth = 2;
                    }
                    else
                    {
                        token = new Token(TokenType.GREATER, ">");
                        simbolWidth = 1;
                    }
                    break;
                case '=':
                    token = new Token(TokenType.EQUAL, "=");
                    simbolWidth = 1;
                    break;
                case '!':
                    if (nextSimbol == '=')
                    {
                        token = new Token(TokenType.NEQUAL, "!=");
                        simbolWidth = 2;
                    }
                    else
                        throw new TokenMismatchException("only ! found ,expected !=");

                    break;
                case '(':
                    token = new Token(TokenType.LPARENT, "(");
                    simbolWidth = 1;
                    break;
                case ')':
                    token = new Token(TokenType.LPARENT, "(");
                    simbolWidth = 1;
                    break;
            }
            //if (token == null)
            //{
            //    if (match(index, "and"))
            //    {
            //        simbolWidth += 3;
            //        token = new Token(TokenType.AND, "AND");
            //    }
            //    else if (match(index, "or"))
            //    {
            //        simbolWidth += 3;
            //        token = new Token(TokenType.OR, "OR");
            //    }
            //    else if (match(index, "NULL"))
            //    {
            //        simbolWidth += 4;
            //        token = new Token(TokenType.NULL, "NULL");
            //    }
            //    else if (match(index, "LIKE"))
            //    {
            //        simbolWidth += 4;
            //        token = new Token(TokenType.LIKE, "LIKE");
            //    }

            //}
            return token;
        }

        private bool Match(int start, String value)
        {
            int valueLen = value.Length;
            char end = (char)0;
            if (start + valueLen < len)
            {
                end = _lower[start + valueLen];
                if (end != ' ' || end != '\t' || end != '\r' || end != '\n')
                    return false;
            }

            for (int i = 0; i < valueLen && (start + i) < len; i++)
            {
                if (value[i] != _lower[start + i])
                {
                    return false;
                }
            }

            return true;
        }

        private TokenType IsSymbol(int simbol)
        {
            simbolWidth = 0;
            TokenType token = TokenType.UNKNOW;
            int nextSimbol = -1;
            if ((index + 1) < len)
                nextSimbol = _string[index + 1];

            switch (simbol)
            {
                case '{':
                    token = TokenType.LCURLY;
                    simbolWidth = 1;
                    break;
                case '}':
                    token = TokenType.RCURLY;
                    simbolWidth = 1;
                    break;
                case '[':
                    token = TokenType.LBRACK;
                    simbolWidth = 1;
                    break;
                case ']':
                    token = TokenType.RBRACK;
                    simbolWidth = 1;
                    break;
                case ':':
                    token = TokenType.COLON;
                    simbolWidth = 1;
                    break;
                case ',':
                    token = TokenType.SEMICOLON;
                    simbolWidth = 1;
                    break;
                case '.':
                    token = TokenType.DOT;
                    simbolWidth = 1;
                    break;
                case '<':
                    if (nextSimbol == '=')
                    {
                        token = TokenType.LEQUAL;
                        simbolWidth = 2;
                    }
                    else
                    {
                        token = TokenType.LESS;
                        simbolWidth = 1;
                    }
                    break;
                case '>':
                    if (nextSimbol == '=')
                    {
                        token = TokenType.GEQUAL;
                        simbolWidth = 2;
                    }
                    else
                    {
                        token = TokenType.GREATER;
                        simbolWidth = 1;
                    }
                    break;
                case '=':
                    token = TokenType.EQUAL;
                    simbolWidth = 1;
                    break;
                case '!':
                    if (nextSimbol == '=')
                    {
                        token = TokenType.NEQUAL;
                        simbolWidth = 2;
                    }
                    else
                        throw new TokenMismatchException("only ! found ,expected !=");

                    break;
                case '(':
                    token = TokenType.LPARENT;
                    simbolWidth = 1;
                    break;
                case ')':
                    token = TokenType.RPARENT;
                    simbolWidth = 1;
                    break;
                case '-':
                    token = TokenType.SUB;
                    simbolWidth = 1;
                    break;
                case '+':
                    token = TokenType.PLUS;
                    simbolWidth = 1;
                    break;
                case '*':
                    token = TokenType.MUL;
                    simbolWidth = 1;
                    break;
                case '/':
                    token = TokenType.DIV;
                    simbolWidth = 1;
                    break;
                case '?':
                    token = TokenType.QUESTION;
                    simbolWidth = 1;
                    break;
            }
            return token;
        }

        private int Read()
        {
            if (index < len)
            {
                char c = _string[index++];
                if (c == '\n')
                {
                    Row++;
                    Col = 1;
                }
                else
                {
                    Col++;
                }
                return c;
            }
            return -1;
        }

        private void Skip(int characters)
        {
            while (characters > 0)
            {
                Read();
                characters--;
            }
        }

        private int Peek()
        {
            if (index < len)
                return _string[index];
            return -1;
        }

        private string ReadString()
        {
            StringBuilder sb = new StringBuilder();
            boolean scape = false;
            int c = Read();
            if (c == '\'')
                c = Read();

            while (c > 0)
            {
                if (scape)
                {
                    sb.Append((char)c);
                    scape = false;
                }
                else if (c == '\\')
                {
                    scape = true;
                }
                else if (c == '\'')
                {
                    break;
                }
                else
                {
                    sb.Append((char)c);
                }
                c = Read();
            }
            return sb.ToString();
        }

        private boolean IsWhiteSpace(int simbol)
        {
            return simbol == ' ' || simbol == '\t' || simbol == '\r' || simbol == '\n';
        }

        private String ReadWord(int simbol)
        {
            StringBuilder sb = new StringBuilder();
            int c = Peek();
            while (c > 0)
            {
                c = Peek();
                if (c == -1)
                {
                    break;
                }

                if (IsWhiteSpace(c) || IsSymbol(c) != TokenType.UNKNOW || c == '\'')
                {
                    break;
                }
                else if (!Char.IsLetterOrDigit((char)c) && c != '_')
                {
                    throw new TokenMismatchException(String.Format("Expecting ID '{0}' at COL {1} ROW {2}", sb.ToString(), Col, Row));
                }

                sb.Append((char)c);
                Read();
            }
            return sb.ToString();
        }

        private String ReadInteger()
        {
            StringBuilder sb = new StringBuilder();
            int c = Peek();

            while (c > 0)
            {
                c = Peek();
                if (c == -1)
                    break;
                if (IsWhiteSpace(c) || IsSymbol(c) != TokenType.UNKNOW || c == '\'')
                {
                    break;
                }
                else if (!Char.IsDigit((char)c))
                {
                    throw new TokenMismatchException(String.Format("Expecting Integer '{0}' at COL:{1} ROW: {2}", sb.ToString(), Col, Row));
                }

                sb.Append((char)c);
                Read();
            }
            return sb.ToString();
        }

        private String ReadNumber()
        {
            String intPart = ReadInteger();
            char c = (char)Peek();
            TokenType token = IsSymbol(c);
            if (token == TokenType.DOT)
            {
                return intPart + "." + ReadInteger();
            }
            else if (token != TokenType.UNKNOW)
            {
                return intPart;
            }
            else
                throw new TokenMismatchException(String.Format("expecting number at COL: {0} ROW: {1}", Col, Row));
        }

        private TokenType GetKeyword(String word)
        {
            TokenType tokenType = TokenType.UNKNOW;
            keywords.TryGetValue(word.ToLowerInvariant(), out tokenType);
            return tokenType;
        }

        private void SkipSpaces()
        {
            while (true)
            {
                int simbol = Peek();
                if (!IsWhiteSpace(simbol))
                    break;

                Read();
            }
        }
    }
}
