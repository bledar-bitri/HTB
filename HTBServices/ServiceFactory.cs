using System.Collections.Generic;
using System.Reflection;
using Autofac;
using Autofac.Builder;
using Autofac.Core;
using HTB.Database;
using HTBServices.Mail;
using HTBUtilities;

namespace HTBServices
{
    public sealed class ServiceFactory : AbstractServiceFactory
    {
        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(MethodBase.GetCurrentMethod()?.DeclaringType);
        private static readonly tblServerSettings serverSettings = (tblServerSettings)HTBUtils.GetSqlSingleRecord("Select * from tblServerSettings", typeof(tblServerSettings));
        private static readonly tblControl control = HTBUtils.GetControlRecord();


        private static ServiceFactory _instance;
        private ServiceFactory()
        {
            InitializeServices();
        }
        public static ServiceFactory Instance => _instance ?? (_instance = new ServiceFactory());
        protected override IContainer ConfigureServices()
        {
            
            var builder = new ContainerBuilder();


            //builder.RegisterType<EmailSenderExchange>()
            //    .As<IEmailSender>()
            //    .WithParameter("defaultFromEmail", serverSettings.ServerSystemMail)
            //    .WithParameter("defaultFromName", HTBUtils.GetConfigValue("From_Office_Name"))
            //    .WithParameter("user", control.SMTPUser)
            //    .WithParameter("password", control.SMTPPW)
            //    .SingleInstance();

            builder.RegisterType<EmailSenderSmtp>()
                .As<IEmailSender>()
                .UsingConstructor(typeof(string), typeof(string), typeof(string), typeof(int), typeof(string), typeof(string))
                .WithParameters(
                    new [] {
                        new NamedParameter("defaultFromEmail",serverSettings.ServerSystemMail),
                        new NamedParameter("defaultFromName", HTBUtils.GetConfigValue("From_Office_Name")),
                        new NamedParameter("server", control.SMTPServer),
                        new NamedParameter("port", control.SMTPPort),
                        new NamedParameter("user", control.SMTPUser),
                        new NamedParameter("password", control.SMTPPW)
                    })
                .SingleInstance();

            builder.RegisterType<HTBEmail>().As<IHTBEmail>();


            return builder.Build(ContainerBuildOptions.None);
        }
    }
}
