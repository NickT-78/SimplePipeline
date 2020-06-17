using SimplePipeline.Domain;
using SimplePipeline.Interfaces;
using System;
using System.Collections.Generic;

namespace SimplePipeline.Blocks
{
    /// <summary>
    /// A pipeline step that routes an item based on a function, nothing is performed on the item
    /// </summary>
    public class PipelineRouter : PipelineStep
    {
        /// <summary>
        /// Function performed by the step
        /// </summary>
        public Func<IPipelineItem, string> Function { get; private set; }

        /// <summary>
        /// Creates an instance of a router step
        /// </summary>
        /// <param name="function">The routing decision to perform on the PipelineItem, must return the name of the queue upon which to post the item </param>
        /// <param name="inputQName">The input queue name, used to get the item for the function</param>
        /// <param name="outputQNames">The different output queues names to which the item is posted based on function result</param>
        /// <param name="parallelismDegree">Number of instances of this step the pipeline can spawn to process data. All queues are in common between the instances</param>
        public PipelineRouter(Func<IPipelineItem, string> function, string inputQName, string[] outputQNames, int parallelismDegree = 1)
        {
            Function = function;
            InputQName = inputQName;
            _outputQNames = new List<string>();
            foreach (var o in outputQNames)
            {
                _outputQNames.Add(o);
            }
            ParallelismDegree = parallelismDegree;
            Status = StepStatus.Created;
        }

        /// <summary>
        /// Executes the pipeline step
        /// </summary>
        /// <param name="item">The pipeline item</param>
        /// <returns>The output queue to use</returns>
        internal override string Run(IPipelineItem item)
        {
            return Function.Invoke(item);
        }
    }
}
