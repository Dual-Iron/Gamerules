using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;

namespace Gamerules
{
    [Serializable]
    internal class MakeshiftAggregateException : Exception
    {
        public ICollection<Exception> Exceptions { get; } = new List<Exception>();

        public MakeshiftAggregateException() : base("An aggregate exception occurred.")
        {
        }

        public MakeshiftAggregateException(ICollection<Exception> exceptions) : base(exceptions.SingleOrDefault() is Exception e ? e.Message : "A total of " + exceptions.Count + " exceptions occurred.")
        {
            Exceptions = exceptions;
        }

        public MakeshiftAggregateException(string message) : base(message)
        {
        }

        public MakeshiftAggregateException(string message, Exception innerException) : base(message, innerException)
        {
        }

        protected MakeshiftAggregateException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }

        public override string ToString()
        {
            int i = 0;
            string ret = base.ToString();
            foreach (var exc in Exceptions)
            {
                ret += $"\n{++i}    {exc}";
            }
            return ret;
        }
    }
}