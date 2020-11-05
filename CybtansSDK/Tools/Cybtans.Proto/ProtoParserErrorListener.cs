using Antlr4.Runtime;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Cybtans.Proto
{
    public class ProtoParserErrorListener : BaseErrorListener
    {
        public List<string> Errors { get; } = new List<string>();

        public List<RecognitionException> Exceptions { get; } = new List<RecognitionException>();
        public override void SyntaxError(TextWriter output, IRecognizer recognizer, IToken offendingSymbol, int line, int charPositionInLine, string msg, RecognitionException e)
        {
            Errors.Add($"{msg}, LINE:{line}, Char:{charPositionInLine}, Token:{offendingSymbol?.Text}");
            Exceptions.AddRange(Exceptions);            
        }

        public void EnsureNoErrors(string filename)
        {
            if (Errors.Any())
            {
                var errors = new List<string>();
                errors.Add(filename);
                errors.AddRange(Errors);

                throw new Proto3RecognitionException(errors);
            }
        }
    }
    
    

}
