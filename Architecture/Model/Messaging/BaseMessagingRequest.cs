using System;

namespace VDB.Architecture.Model.Messaging
{
    public abstract record BaseMessagingRequest
    {
        public Guid MessageID { get; set; }

        protected BaseMessagingRequest()
        {
            this.MessageID = Guid.NewGuid();
        }
    }
}
