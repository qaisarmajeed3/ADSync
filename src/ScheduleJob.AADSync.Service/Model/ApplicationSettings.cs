using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;

namespace  ScheduleJob.AADSync.Service.Model
{
   /// <summary>
   /// Azure AD configuration class
   /// </summary>
    public class AzureAdImportApp
    {
        /// <summary>
        /// ClientId of Azure AD Import Application
        /// </summary>
        public string? ClientId { get; set; }
        /// <summary>
        /// Client Secret of Azure AD Import application
        /// </summary>
        public string? ClientSecret { get; set; }
        /// <summary>
        /// Scope of Azure AD import application
        /// </summary>
        public string? Scope { get; set; }
        /// <summary>
        /// TenantId of Azure AD 
        /// </summary>
        public string? TenantId { get; set; }

        /// <summary>
        /// Get allowed domain list 
        /// </summary>
        public string? AllowedDomains { get; set; }
    }

   
}
