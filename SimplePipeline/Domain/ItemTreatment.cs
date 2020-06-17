using System;
using System.Collections.Generic;

namespace SimplePipeline.Domain
{
    /// <summary>
    /// Item processing information
    /// </summary>
    public class ItemTreatment
    {
        /// <summary>
        /// Message list, each step can add to the list
        /// </summary>
        public List<string> Message { get; } = new List<string>();
        /// <summary>
        /// Last exception raised by the process
        /// </summary>
        public Exception TreatmentException { get; set; }
    }
}
