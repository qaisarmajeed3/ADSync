namespace  ScheduleJob.AADSync.Service.Enum
{
    /// <summary>
    /// Base class for string Enumeration
    /// </summary>
    public class BaseEnum
    {
        /// <summary>
        /// Base enum class for constructor.
        /// </summary>
        protected BaseEnum(string value) { Value = value; }
        /// <summary>
        /// gets the value of the base enum.
        /// </summary>
        public string Value { get; }
        /// <summary>
        /// overriding the to string method.
        /// </summary>
        public override string ToString() => Value.ToLower();
    }
}