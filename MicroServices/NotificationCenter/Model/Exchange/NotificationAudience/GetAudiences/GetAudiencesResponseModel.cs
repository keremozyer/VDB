using System;
using System.Collections.Generic;

namespace VDB.MicroServices.NotificationCenter.Model.Exchange.NotificationAudience.GetAudiences
{
    public record GetAudiencesResponseModel
    {
        /// <summary>
        /// List of audiences.
        /// </summary>
        public IEnumerable<AudienceData> Audiences { get; set; }

        public GetAudiencesResponseModel(IEnumerable<AudienceData> audiences)
        {
            Audiences = audiences;
        }
    }
    
    public record AudienceData
    {
        /// <summary>
        /// ID of audience record.
        /// </summary>
        public Guid ID { get; set; }
        /// <summary>
        /// Receiver tag of audience record.
        /// </summary>
        public string Receiver { get; set; }

        public AudienceData(Guid iD, string receiver)
        {
            ID = iD;
            Receiver = receiver;
        }
    }
}
