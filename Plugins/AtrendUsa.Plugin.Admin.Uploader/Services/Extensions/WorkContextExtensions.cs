using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AtrendUsa.Plugin.Admin.Uploader.Services.Model;
using Nop.Core;

namespace AtrendUsa.Plugin.Admin.Uploader.Services.Extensions
{
   public static class WorkContextExtensions
    {
       public static UserContext GetUserContext(this IWorkContext workContext)
       {
           return new UserContext
           {
               Id = workContext.CurrentCustomer.CustomerGuid,
               UserName = workContext.CurrentCustomer.Username
           };
       }
    } 
}
