using System;
using System.Collections.Generic;
using System.ServiceModel;
using System.Text;

namespace FundraisingandEngagement.DataverseSynchronization
{
    public class DataverseSynchronizationException : Exception
    {
        public DataverseSynchronizationException(string message) : base(message) { }

        public DataverseSynchronizationException(string message, Exception innerException) : base(message, innerException) { }
    }
}