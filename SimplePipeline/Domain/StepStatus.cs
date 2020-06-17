namespace SimplePipeline.Domain
{
    /// <summary>
    /// Step stauts
    /// </summary>
    public enum StepStatus
    {
        /// <summary>
        /// Step is created
        /// </summary>
        Created = 0,
        /// <summary>
        /// Step is complete and ready to run
        /// </summary>
        Complete = 1,
        /// <summary>
        /// Step has an error and is not usable
        /// </summary>
        InError = 99
    }
}
