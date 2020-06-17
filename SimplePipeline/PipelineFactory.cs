using SimplePipeline.Blocks;
using SimplePipeline.Domain;
using SimplePipeline.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SimplePipeline
{
    public class PipelineFactory
    {
        /// <summary>
        /// Creates a new pipeline
        /// </summary>
        /// <param name="inputQName">The input queue name that the requestor will use to post the items</param>
        /// <param name="outputQName">The output queue name that the requestor will use to read the output</param>
        /// <param name="steps">The steps of the pipeline</param>
        /// <param name="queues">The queues between the steps of the pipeline</param>
        /// <param name="logger">The logger function for all log traces</param>
        /// <returns>The pipeline</returns>
        public static IAwaitablePipeline<IPipelineItem> CreatePipeline(string inputQName, string outputQName, IEnumerable<IPipelineStep> steps, IEnumerable<PipelineQueue> queues, Func<IPipelineItem, bool> logger)
        {
            Pipeline p = new Pipeline(inputQName, outputQName, logger);
            p.AddQueues(queues);
            p.AddSteps(steps);
            return p.GetPipeline();
        }

    }
}
