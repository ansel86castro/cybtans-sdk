using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cybtans.Expressions
{
    public class TokenMismatchException : ParsingException
    {

        public TokenMismatchException()
        {
        }

        public TokenMismatchException(String detailMessage, Exception throwable)
            : base(detailMessage, throwable)
        {

        }

        public TokenMismatchException(String detailMessage)
            : base(detailMessage)
        {

        }

        public TokenMismatchException(Exception throwable)
            : base(null, throwable)
        {

        }

    }

}
