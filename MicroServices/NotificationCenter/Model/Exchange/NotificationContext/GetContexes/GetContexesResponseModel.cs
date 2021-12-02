using System;
using System.Collections.Generic;

namespace VDB.MicroServices.NotificationCenter.Model.Exchange.NotificationContext.GetContexes
{
    public record GetContexesResponseModel
    {
        /// <summary>
        /// List of notification contexes.
        /// </summary>
        public IEnumerable<NotificationContextData> Contexes { get; set; }

        public GetContexesResponseModel(IEnumerable<NotificationContextData> contexes)
        {
            Contexes = contexes;
        }
    }

    public record NotificationContextData
    {
        /// <summary>
        /// ID of notification context.
        /// </summary>
        public Guid ID { get; set; }
        /// <summary>
        /// Type of notification context.
        /// </summary>
        public string Type { get; set; }

        public NotificationContextData(Guid iD, string type)
        {
            ID = iD;
            Type = type;
        }
    }
}
