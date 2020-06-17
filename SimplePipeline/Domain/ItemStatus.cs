namespace SimplePipeline.Domain
{
    /// <summary>
    /// Status of a pipeline item
    /// </summary>
    public enum ItemStatus
    {
        /// <summary>
        /// Newly inserted into the pipeline, nothing done yet
        /// </summary>
        New = 0,
        /// <summary>
        /// Item is being processed
        /// </summary>
        OnGoing = 1,
        /// <summary>
        /// All processing is complete on the item
        /// </summary>
        Complete = 2,
        /// <summary>
        /// Item is abandoned (item will go direct to output)
        /// </summary>
        Abandoned = 99
    }
}
