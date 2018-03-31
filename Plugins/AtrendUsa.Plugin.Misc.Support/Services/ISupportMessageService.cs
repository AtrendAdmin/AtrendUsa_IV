using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AtrendUsa.Plugin.Misc.Support.Models;

namespace AtrendUsa.Plugin.Misc.Support.Services
{
    public interface ISupportMessageService
    {
        void SendFreightOrderClaimMessage(FreightOrderClaimModel model);
        void SendReturnAuthorizationRequestMessage(ReturnAuthorizationRequestModel model);
    }
}
