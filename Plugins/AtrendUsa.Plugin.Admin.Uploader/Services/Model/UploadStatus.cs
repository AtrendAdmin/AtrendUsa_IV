using System.Collections.Generic;
using System.Linq;

namespace AtrendUsa.Plugin.Admin.Uploader.Services.Model
{
    public class UploadStatus
    {
        private UploadStage _currentUploadStage;

        public UploadStatus()
        {
            UploadStageTrace = new List<UploadStage>();
        }

        public UploadStatus(string traceToken)
        {
            TraceToken = traceToken;
            UploadStageTrace = new List<UploadStage>();
        }

        public string TraceToken { get; private set; }

        public UploadStage CurrentUploadStage
        {
            get { return _currentUploadStage; }
            set
            {
                _currentUploadStage = value;
                if (UploadStageTrace.Any(x => x.Stage == _currentUploadStage.Stage)) return;
                UploadStageTrace.Add(_currentUploadStage);
            }
        }

        public List<UploadStage> UploadStageTrace { get; set; }
    }
}