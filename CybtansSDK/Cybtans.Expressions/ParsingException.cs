using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cybtans.Expressions
{
    public class ParsingException:Exception
    {
        public ParsingException()
        {
        }

        public ParsingException(String detailMessage, Exception throwable)
            : base(detailMessage, throwable)
        {

        }

        public ParsingException(String detailMessage)
            : base(detailMessage)
        {

        }

        public ParsingException(Exception throwable)
            : base(null, throwable)
        {

        }

    }
}
