using System.Threading.Tasks;

namespace SimplePipeline.Interfaces
{
    public interface IAwaitablePipeline<TContent>
    {
        /// <summary>
        /// Executes the pipeline
        /// </summary>
        /// <param name="input">The input item of the pipeline</param>
        /// <returns>An awaitable task</returns>
        Task<TContent> Execute(TContent input);
        /// <summary>
        /// Indicates if the pipeline is complete, all queues needed by the steps are created
        /// </summary>
        bool isComplete { get; }
    }
}
