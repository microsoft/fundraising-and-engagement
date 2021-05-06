using System;
using System.Collections.Generic;
using System.Text;

namespace Data.DataSynchronization
{
    class EntityRepositoryException : Exception
    {
        public EntityRepositoryException(string message, Exception inner) : base(message, inner) { }
    }
}