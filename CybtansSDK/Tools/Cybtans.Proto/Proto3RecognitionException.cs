using System;
using System.Collections.Generic;

namespace Cybtans.Proto
{
    public class Proto3RecognitionException : Exception
    {
        public List<string> Errors { get; }
        public Proto3RecognitionException(List<string> errors):base(string.Join(Environment.NewLine, errors))
        {
            this.Errors = errors;
        }
    }
    

}
