using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Cybtans.Expressions.Ast
{
    public class RecognitionException : ParsingException
    {
        
        private int Col;
        private int Row;

        public RecognitionException(string message) : base(message) { }

        public RecognitionException(string message, int Col, int Row)
            :base(message)
        {        
            this.Col = Col;
            this.Row = Row;
        }
    }
}
