using System;
using System.Collections.Generic;

namespace VDB.Architecture.AppException.Model
{
    public abstract class BaseAppException : ApplicationException
    {
        public IEnumerable<BaseAppExceptionMessage> Messages { get; set; }

        public BaseAppException(params BaseAppExceptionMessage[] messages)
        {
            this.Messages = messages;
        }
    }
}
