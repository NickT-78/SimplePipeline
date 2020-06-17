using SimplePipeline.Interfaces;
using System;
using System.Collections.Concurrent;

namespace SimplePipeline.Blocks
{
    public class PipelineQueue : BlockingCollection<IPipelineItem>
    {
        public PipelineQueue(string name, int boundCapacity = 1000):
            base(boundCapacity)
        {
            Name = name;
        }

        public string Name { get; private set; }
        public int FillRatio => (int)Math.Floor((decimal)100 * this.Count / this.BoundedCapacity);
    }
}
