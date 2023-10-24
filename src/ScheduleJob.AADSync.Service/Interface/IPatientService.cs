namespace  ScheduleJob.Service.Interface
{
    /// <summary>
    /// Interface for QMS Hl7 data service
    /// </summary>
    public interface IPatientService
    {
        /// <summary>
        /// Get QMS HL7 data from Azure file storage
        /// </summary>
        /// <returns></returns>
        Task GetHl7File();
    }
}
