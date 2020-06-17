using SimplePipeline.Domain;
using SimplePipeline.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace SimplePipeline.Blocks
{
    /// <summary>
    /// A pipeline step
    /// </summary>
    public abstract class PipelineStep : IPipelineStep
    {
        /// <summary>
        /// List of output queues names
        /// </summary>
        protected List<string> _outputQNames;

        /// <summary>
        /// Degree of parallelism of this step
        /// </summary>
        public int ParallelismDegree { get; protected set; }

        /// <summary>
        /// Name of the input queue
        /// </summary>
        public string InputQName { get; protected set; }

        /// <summary>
        /// List of ouput queues names
        /// </summary>
        /// <remarks>Returns a copy</remarks>
        public string[] OutputQNames
        {
            get
            {
                string[] ret = new string[_outputQNames.Count];
                _outputQNames.CopyTo(ret);
                return ret;
            }
        }

        /// <summary>
        /// Status of the step
        /// </summary>
        public StepStatus Status { get; protected set; }

        /// <summary>
        /// Updates the status of the step
        /// </summary>
        /// <param name="status">Status to set</param>
        internal void UpdateStatus(StepStatus status)
        {
            Status = status;
        }

        /// <summary>
        /// Executes the pipeline step
        /// </summary>
        /// <param name="item">The pipeline item</param>
        /// <returns>The output queue to use</returns>
        internal abstract string Run(IPipelineItem item);
    }
}
