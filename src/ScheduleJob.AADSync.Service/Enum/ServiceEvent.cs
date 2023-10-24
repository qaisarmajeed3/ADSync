namespace  ScheduleJob.AADSync.Service.Enum
{
    /// <summary>
    /// Class for listing service events.
    /// </summary>
    public class ServiceEvent : BaseEnum
    {
        private ServiceEvent(string value) : base(value) { }
        /// <summary>
        /// Gets or sets create event.
        /// </summary>
        public static readonly ServiceEvent Create = new("create");

        /// <summary>
        /// Gets or sets update event.
        /// </summary>
        public static readonly ServiceEvent Update = new("update");

        /// <summary>
        /// Gets or sets delete event.
        /// </summary>
        public static readonly ServiceEvent Delete = new("delete");

    }
}