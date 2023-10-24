using System.ComponentModel;

namespace  ScheduleJob.WebApi.Model
{
    /// <summary>
    /// Model class to initiate the job execution
    /// </summary>
    public class ExecuteJobModel
    {
        public enum Job
        {
            [Description("AADImportJob")]
            AADImportJob,
            [Description("EmailTaskJob")]
            EmailTaskJob,
            [Description("QMSImportJob")]
            QMSImportJob
        }
        /// <summary>
        /// Job name to be executed
        /// </summary>
        public string JobName { get; set; }
    }
}
