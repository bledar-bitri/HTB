using System;
using System.IO;
using Renci.SshNet;

namespace HTBUtilities
{
    public static class HTBSftp
    {
        private static readonly string Host = HTBUtils.GetConfigValue("Mahnung_FTP_Host");
        private static readonly int Port = Convert.ToInt32(HTBUtils.GetConfigValue("Mahnung_FTP_Port"));
        private static readonly string User = HTBUtils.GetConfigValue("Mahnung_FTP_User");
        private static readonly string Password = HTBUtils.GetConfigValue("Mahnung_FTP_Password");
        private static readonly string Directory = HTBUtils.GetConfigValue("Mahnung_FTP_Directory");
        
        public static void SendFile(string from)
        {
            if (!HTBUtils.IsTestEnvironment)
            {
                using (var sftp = new SftpClient(Host, Port, User, Password))
                {
                    sftp.Connect();
                    using (var file = File.OpenRead(from))
                    {
                        sftp.UploadFile(file, (Directory != null ? "/" + Directory : "") + HTBUtils.GetJustFileName(from));
                    }
                    sftp.Disconnect();
                }
            }
        }
    }
}
