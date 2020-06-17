using SimplePipeline.Domain;
using System.Collections.Generic;

namespace SimplePipeline.Interfaces
{
    public interface IPipelineStep
    {
        #region properties
        /// <summary>
        /// Degree of parallelism of this step
        /// </summary>
        int ParallelismDegree { get; }
        /// <summary>
        /// Name of the input queue
        /// </summary>
        string InputQName { get; }
        /// <summary>
        /// List of ouput queues names
        /// </summary>
        /// <remarks>Returns a copy</remarks>
        string[] OutputQNames { get; }
        /// <summary>
        /// Status of the step
        /// </summary>
        StepStatus Status { get; }
        #endregion

    }
}
