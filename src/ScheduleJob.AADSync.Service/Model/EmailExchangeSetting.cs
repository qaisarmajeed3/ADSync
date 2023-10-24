using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace  ScheduleJob.AADSync.Service.Model
{
    /// <summary>
    /// Class that handles email exchange settings.
    /// </summary>
    public class EmailExchangeSetting
    {
        /// <summary>
        /// Username of email exchange.
        /// </summary>
        public string? UserName { get; set; }

        /// <summary>
        /// Password of email exchange.
        /// </summary>
        public string? Password { get; set; }
    }
}
