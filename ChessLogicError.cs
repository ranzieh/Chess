using System;
using System.Runtime.Serialization;

namespace Chess
{
    [Serializable]
    internal class ChessLogicError : Exception
    {
        public ChessLogicError()
        {
        }

        public ChessLogicError(string message) : base(message)
        {
        }

        public ChessLogicError(string message, Exception innerException) : base(message, innerException)
        {
        }

        protected ChessLogicError(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}