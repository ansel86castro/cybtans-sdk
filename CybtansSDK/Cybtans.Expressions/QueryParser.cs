using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Cybtans.Expressions.Ast;

namespace Cybtans.Expressions
{
    public class QueryParser
    {

        /*
         *  exp : or_exp (QUESTION exp COLON exp)?
         *         ;
         *         
         *  or_exp : and_exp (OR and_exp)*
         *  	   ;
         *  
         *  and_exp: rel_exp( AND rel_exp)*
         *  	   ;
         *  
         *  rel_exp: lvalue (
         *  	( EQUAL 
         *  	| NEQUAL
         *  	| LESS 
         *  	| LEQUAL
         *  	| GREATER
         *  	| GEQUAL
         *  	| LIKE ) lvalue )? 
         *  	;
         *  
         *  lvalue: ID (.ID)* (LPARENT exp_list RPARENT)?
         *  	| INT 
         *  	| DOUBLE 
         *  	| STRING 
         *  	| NULL
                | LPARENT exp RPARENT
                | SUB lvalue
                ;

          * exl_list: exp (SEMICOLON exp)* 
          * 
         * */

        QueryLexicalAnalizer lexicalAnalizer;

        public QueryParser()
        {
            lexicalAnalizer = new QueryLexicalAnalizer();
        }

        public Token Match(TokenType token)
        {
            Token t = lexicalAnalizer.CurrentToken;
            if (!(t?.Match(token) ?? false))
            {
                throw new RecognitionException("Expecting " + token);
            }
            return lexicalAnalizer.GetNextToken();
        }

        public Expression Parse(String query)
        {
            lexicalAnalizer.Source = query;
            Expression exp = or_exp();
            if (lexicalAnalizer.CurrentToken!= null)
            {
                throw new RecognitionException("Expecting EOF");
            }
            return exp;
        }

        public Expression exp()
        {
            var exp = or_exp();
            var token = lexicalAnalizer.CurrentToken;
            if(token == TokenType.QUESTION)
            {
                Match(TokenType.QUESTION);

                var onTrue = this.exp();

                Match(TokenType.COLON);

                var onFalse = this.exp();

                exp = new TernaryExpression(exp, onTrue, onFalse);
            }

            return exp;
        }

        public Expression or_exp()
        {
            Expression exp = and_expression();

            while (true)
            {
                Token t = lexicalAnalizer.CurrentToken;
                if (t == null)
                {
                    break;
                }

                if (t.Match(TokenType.OR))
                {
                    //throw new RecognitionException("Expecting OR",t.Col, t.Row);
                    lexicalAnalizer.GetNextToken();
                    Expression right = and_expression();
                    exp = new LogicalExpression(exp, right, Operator.OP_OR);
                }
                else
                {
                    return exp;
                }
            }


            return exp;
        }

        private Expression and_expression()
        {
            Expression exp = rel_expression();

            while (true)
            {
                Token t = lexicalAnalizer.CurrentToken;
                if (t == null)
                {
                    break;
                }

                if (t.Match(TokenType.AND))
                {
                    lexicalAnalizer.GetNextToken();
                    Expression right = rel_expression();
                    exp = new LogicalExpression(exp, right, Operator.OP_AND);

                    //throw new RecognitionException("Expecting AND",t.Col, t.Row);				
                }
                else
                {
                    return exp;
                }
            }


            return exp;
        }

        private Expression rel_expression()
        {
            Expression exp = lvalue();

            Token t = lexicalAnalizer.CurrentToken;
            if (t == null)
            {
                return exp;
            }

            Operator op = 0;
            if (t.Match(TokenType.EQUAL))
            {
                op = Operator.OP_EQUAL;
            }
            else if (t.Match(TokenType.NEQUAL))
            {
                op = Operator.OP_DISTINT;
            }
            else if (t.Match(TokenType.LESS))
            {
                op = Operator.OP_LESS;
            }
            else if (t.Match(TokenType.GREATER))
            {
                op = Operator.OP_GREATHER;
            }
            else if (t.Match(TokenType.GEQUAL))
            {
                op = Operator.OP_GREATHER_EQ;
            }
            else if (t.Match(TokenType.LEQUAL))
            {
                op = Operator.OP_LESS_EQ;
            }
            else if (t.Match(TokenType.LIKE))
            {
                op = Operator.OP_LIKE;
            }

            if (op > 0)
            {
                //throw new RecognitionException("Expecting operator (=,<,>,<=,>=,LIKE,!=)", t.Col, t.Row);
                //return exp;
                lexicalAnalizer.GetNextToken();
                Expression right = lvalue();
                exp = new RelationalExpression(exp, right, op);
            }
            return exp;
        }

        private Expression lvalue()
        {
            Token t = lexicalAnalizer.CurrentToken;
            if (t == null)
                throw new RecognitionException("EOF");

            Expression exp = null;

            if (t.Match(TokenType.ID))
            {
                //ID
                lexicalAnalizer.GetNextToken();

                string name = t.Value;             

                t = lexicalAnalizer.CurrentToken;
                while (t != null && t.Match(TokenType.DOT))
                {
                    if(exp == null)
                        exp = new VariableExpression(name);

                    //(.ID)*

                    t = lexicalAnalizer.GetNextToken();
                    if (t == null || !t.Match(TokenType.ID))
                    {
                        throw new RecognitionException("Expecting ID", t?.Col??0, t?.Row??0);
                    }

                    exp = new MemberExpression(t.Value, exp);
                    t = lexicalAnalizer.GetNextToken();
                }

                if(t != null && t == TokenType.LPARENT)
                {
                    //(LPARENT exp_list RPARENT)?
                    Match(TokenType.LPARENT);

                    var exp_list = this.exp_list();

                    Match(TokenType.RPARENT);

                    exp = new FunctionCall(name, exp_list.ToArray(), t.Col, t.Row);

                }
                else if(exp == null)
                {
                    exp = new VariableExpression(name);
                }
            }
            else if (t.Match(TokenType.DOUBLE))
            {
                lexicalAnalizer.GetNextToken();
                exp = new LiteralExpression(t.Value, ExpressionType.Double);
            }
            else if (t.Match(TokenType.INT))
            {
                lexicalAnalizer.GetNextToken();
                exp = new LiteralExpression(t.Value, ExpressionType.Integer);
            }
            else if (t.Match(TokenType.STRING))
            {
                lexicalAnalizer.GetNextToken();
                exp = new LiteralExpression(t.Value, ExpressionType.String);
            }
            else if (t.Match(TokenType.NULL))
            {
                lexicalAnalizer.GetNextToken();
                exp = new LiteralExpression(t.Value, ExpressionType.Null);
            }
            else if (t.Match(TokenType.TRUE) || t.Match(TokenType.FALSE))
            {
                lexicalAnalizer.GetNextToken();
                exp = new LiteralExpression(t.Value, ExpressionType.Bool, t.TokenType);
            }
            else if (t.Match(TokenType.LPARENT))
            {
                lexicalAnalizer.GetNextToken();
                exp = or_exp();
                Match(TokenType.RPARENT);
            }
            else if (t.Match(TokenType.SUB))
            {
                lexicalAnalizer.GetNextToken();
                Expression valueExp = lvalue();
                exp = new UnaryExpression(valueExp, Operator.OP_SUB);
            }

            return exp;

            //throw new RecognitionException("Unspected token "+t.Value, t.Col, t.Row);

        }

        public List<Expression> exp_list()
        {
            List<Expression> list = new List<Expression>();
            //exp
            var exp = this.exp();
            list.Add(exp);

            var t = lexicalAnalizer.CurrentToken;

            while (t != null && t.Match(TokenType.SEMICOLON))
            {
                // (SEMICOLON exp)* 

                Match(TokenType.SEMICOLON);
                
                exp = this.exp();

                list.Add(exp);

                t = lexicalAnalizer.CurrentToken;
            }
            return list;

        }

    }
}
