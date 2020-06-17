using NUnit.Framework;
using SimplePipeline.Blocks;
using SimplePipeline.Domain;
using SimplePipeline.Interfaces;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SimplePipeline.TestCore
{
    public class Tests
    {
        [SetUp]
        public void Setup()
        {
        }

        [Test]
        public void Test1()
        {
            string inQ = "in", outQ = "out";

            var steps = new List<PipelineStep>() {
                new PipelineAction(item=> { item.Content = (int)item.Content + 5; item.Status = ItemStatus.OnGoing; return item; }, inQ, "has5", 2),
                new PipelineRouter(item => ((int)item.Content%2 ==0?"pair":"impair"), "has5", new[] {"pair", "impair" }, 2),
                new PipelineAction(item => {Assert.AreEqual(0, (int)item.Content%2); item.Status = ItemStatus.Complete; return item; }, "pair", outQ, 2),
                new PipelineAction(item => {Assert.AreEqual(1, (int)item.Content%2); item.Status = ItemStatus.Complete; return item; }, "impair", outQ, 2)
            };
            var outP = new PipelineQueue(outQ);
            var queues = new List<PipelineQueue>()
            {
                new PipelineQueue(inQ),
                outP,
                new PipelineQueue("has5"),
                new PipelineQueue("pair"),
                new PipelineQueue("impair")
            };

            Task.Run(() => {
                foreach (var o in outP.GetConsumingEnumerable()) 
                { 
                    Console.WriteLine(string.Concat(o.Treatment.Message)); 
                } 
            });


            var p = PipelineFactory.CreatePipeline(
                inQ,
                outQ,
                steps,
                queues,
                item => { 
                    Console.WriteLine(string.Concat(item.Treatment.Message)); 
                    return true; 
                }
            );

            p.Execute(new item() { Content = 5 });
        }
    }

    public class item : IPipelineItem
    {
        public ItemStatus Status { get; set; } = ItemStatus.New;
        public ItemTreatment Treatment { get; set; } = new ItemTreatment();
        public object Content { get; set; }
        public Type ContentType { get { return typeof(int); } set => throw new NotImplementedException(); }
        public TaskCompletionSource<IPipelineItem> TaskCompletionSource { get; set; } = new TaskCompletionSource<IPipelineItem>();
    }

}