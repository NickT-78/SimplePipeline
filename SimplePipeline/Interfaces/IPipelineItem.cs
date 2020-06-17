using SimplePipeline.Domain;
using System;
using System.Threading.Tasks;

namespace SimplePipeline.Interfaces
{
    public interface IPipelineItem
    {
        /// <summary>
        /// Item status, updated by the step functions
        /// </summary>
        ItemStatus Status { get; set; }
        /// <summary>
        /// The item's processing information
        /// </summary>
        ItemTreatment Treatment { get; set; }
        /// <summary>
        /// Content of the item (this holds the data the function will process)
        /// </summary>
        object Content { get; set; }
        /// <summary>
        /// The content type
        /// </summary>
        Type ContentType { get; set; }
        /// <summary>
        /// The item's task completion information
        /// </summary>
        TaskCompletionSource<IPipelineItem> TaskCompletionSource { get; set; }
    }
}
