using SimplePipeline.Domain;
using SimplePipeline.Interfaces;
using System;
using System.Collections.Generic;

namespace SimplePipeline.Blocks
{
    /// <summary>
    /// A pipeline step that generates work on an item and updates the item
    /// </summary>
    public class PipelineAction : PipelineStep
    {
        /// <summary>
        /// Function performed by the step
        /// </summary>
        public Func<IPipelineItem, IPipelineItem> Function { get; private set; }

        /// <summary>
        /// Creates an instance of an action step
        /// </summary>
        /// <param name="function">The action to perform on the PipelineItem, must return the modified item</param>
        /// <param name="inputQName">The input queue name, used by the function to get it's input data items</param>
        /// <param name="outputQName">The output queue name to which the updated item is posted</param>
        /// <param name="parallelismDegree">Number of instances of this step the pipeline can spawn to process data. All queues are in common between the instances</param>
        public PipelineAction(Func<IPipelineItem, IPipelineItem> function, string inputQName, string outputQName, int parallelismDegree = 1)
        {
            Function = function;
            InputQName = inputQName;
            _outputQNames = new List<string>();
            _outputQNames.Add(outputQName);
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
            item = Function.Invoke(item);
            return OutputQNames[0];
        }
    }
}
