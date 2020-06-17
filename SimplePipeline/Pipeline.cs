using SimplePipeline.Blocks;
using SimplePipeline.Domain;
using SimplePipeline.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SimplePipeline
{
    public class Pipeline : IAwaitablePipeline<IPipelineItem>
    {
        private const int LOG_QUEUE_LENGTH = 1000;
        private List<PipelineStep> Steps = new List<PipelineStep>();
        private readonly string InputQName;
        private readonly string OutputQName;
        private readonly Func<IPipelineItem, bool> Logger;
        private const string LOG_QUEUE_NAME = "logQueue";
        private Dictionary<string, PipelineQueue> PipelineQueues { get; set; }

        /// <summary>
        /// Indicates if the pipeline is complete, all queues needed by the steps are created
        /// </summary>
        public bool isComplete { get; private set; }

        internal Pipeline(string inputQName, string outputQName, Func<IPipelineItem, bool> logger)
        {
            InputQName = inputQName;
            OutputQName = outputQName;
            Logger = logger;
            PipelineQueues = new Dictionary<string, PipelineQueue>();
            PipelineQueues.Add(LOG_QUEUE_NAME, new PipelineQueue(LOG_QUEUE_NAME, LOG_QUEUE_LENGTH));
            isComplete = true;
        }

        /// <summary>
        /// Executes the pipeline
        /// </summary>
        /// <param name="input">The input item of the pipeline</param>
        /// <returns>An awaitable task</returns>
        public Task<IPipelineItem> Execute(IPipelineItem input)
        {
            PipelineQueues[InputQName].Add(input);
            input.TaskCompletionSource = new TaskCompletionSource<IPipelineItem>();
            return input.TaskCompletionSource.Task;
        }

        internal void AddStep(IPipelineStep step)
        {
            Steps.Add((PipelineStep)step);
            bool isComplete = step.OutputQNames.All(s => PipelineQueues.ContainsKey(s));
            isComplete &= PipelineQueues.ContainsKey(step.InputQName);
            if (isComplete)
            {
                ((PipelineStep)step).UpdateStatus(StepStatus.Complete);
            }
        }

        internal void AddSteps(IEnumerable<IPipelineStep> steps)
        {
            foreach(var s in steps)
            {
                this.AddStep(s);
            }
        }

        internal void AddQueue(PipelineQueue queue)
        {
            PipelineQueues.Add(queue.Name, queue);
        }

        internal void AddQueues(IEnumerable<PipelineQueue> queues)
        {
            foreach (var q in queues)
            {
                this.AddQueue(q);
            }
        }

        internal IAwaitablePipeline<IPipelineItem> GetPipeline()
        {
            Task.Run(() => { foreach (var l in PipelineQueues[LOG_QUEUE_NAME].GetConsumingEnumerable()) { Logger.Invoke(l); } });

            foreach (var step in Steps)
            {
                bool isComplete = step.OutputQNames.All(s => PipelineQueues.ContainsKey(s));
                isComplete &= PipelineQueues.ContainsKey(step.InputQName);
                if (isComplete)
                {
                    step.UpdateStatus(StepStatus.Complete);
                }
                else
                {
                    step.UpdateStatus(StepStatus.InError);
                    isComplete &= false;
                }
            }

            if (isComplete)
            {
                foreach (var step in Steps)
                {
                    for (int i = 0; i < step.ParallelismDegree; i++)
                    {
                        Task.Run(() => { StartStep(step); });
                    }
                }
            }
            return this;
        }

        #region privates
        private void StartStep(PipelineStep step)
        {
            Parallel.ForEach(
                PipelineQueues[step.InputQName].GetConsumingEnumerable(),
                item =>
                {
                    string outputQueueName;
                    try
                    {
                        outputQueueName = step.Run(item);
                    }
                    catch (Exception e)
                    {
                        item.TaskCompletionSource.SetException(e);
                        item.Status = ItemStatus.Abandoned;
                        return;
                    }

                    if (item.Status == ItemStatus.Abandoned)
                    {
                        PipelineQueues[LOG_QUEUE_NAME].Add(item);
                        PipelineQueues[OutputQName].Add(item);
                    }
                    else
                    {
                        PipelineQueues[outputQueueName].Add(item);
                    }
                });
        }

        #endregion

    }
}
