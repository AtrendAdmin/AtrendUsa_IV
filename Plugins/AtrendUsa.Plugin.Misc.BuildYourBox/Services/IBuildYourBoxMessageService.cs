using AtrendUsa.Plugin.Misc.BuildYourBox.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AtrendUsa.Plugin.Misc.BuildYourBox.Services
{
    public interface IBuildYourBoxMessageService
    {
        void SendBuildYourBoxMessage(BuildYourBoxModel model);
    }
}
