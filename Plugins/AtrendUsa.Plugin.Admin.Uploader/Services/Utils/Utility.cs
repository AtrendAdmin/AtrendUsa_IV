using System;
using System.IO;
using System.Net;

namespace AtrendUsa.Plugin.Admin.Uploader.Services.Utils
{
    public static class Utility
    {
        public static string GetFilePath(string directoryPath, string fileName)
        {
            string path = Path.Combine(directoryPath, fileName);
            string directoryName = Path.GetDirectoryName(path);
            if (!string.IsNullOrWhiteSpace(directoryName) && !Directory.Exists(directoryName))
            {
                Directory.CreateDirectory(directoryName);
            }

            return path;
        }

        public static string ToUniqueFileName(string fileName)
        {
            return string.Format("{0}_{1}", DateTime.Now.ToString("ddMMyyyyhhmmssffss"), Path.GetFileName(fileName));
        }

        public static string GenerateErrorIdentifier()
        {
            string machineName = Dns.GetHostName();

            return string.Format("WU-{0}-{1}-{2}", DateTime.Now.ToString("yyyyMMdd"), DateTime.Now.Ticks,
                MachineNameToCode(machineName));
        }

        public static string MachineNameToCode(string serverName)
        {
            if (string.IsNullOrWhiteSpace(serverName)) return "00";
            serverName = serverName.ToLowerInvariant();
            if (serverName.Contains("prd"))
            {
                return serverName.Replace("www", string.Empty).Replace("prd", string.Empty);
            }
            return serverName.Contains("qa") ? serverName : "00";
        }
    }
}