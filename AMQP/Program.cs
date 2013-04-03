using System;
using System.Text;
using System.Configuration;
using System.Net.Security;
using System.Reflection;
using RabbitMQ.Client;

[assembly: log4net.Config.XmlConfigurator(Watch = true)]

namespace AMQP
{
    public class CampusAMQPSender
    {
        private static string host;
        private static string exchange;
        private static string clientKey;
        private static string passPhrase;
        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        // In visual studio; app looks for App.config (exe file); as dll app look for AMQP.dll.config
        static CampusAMQPSender() {            
            log.Debug("CampusAMQPSender");
            Assembly ea = System.Reflection.Assembly.GetExecutingAssembly();            
            Console.WriteLine(ea.Location);
            Configuration config = ConfigurationManager.OpenExeConfiguration(ea.Location);            
            host = config.AppSettings.Settings["host"].Value.ToString();            
            log.Debug("host: " + host);            
            exchange = config.AppSettings.Settings["exchange"].Value.ToString();
            log.Debug("exchange: " + exchange);            
            clientKey = config.AppSettings.Settings["clientKey"].Value.ToString();
            log.Debug("clientKey: " + clientKey);            
            passPhrase = config.AppSettings.Settings["passPhrase"].Value.ToString();
            log.Debug("passPhrase: " + passPhrase);            
        }

        public static void publishMessage(string routingKey, string msg, bool persist)
        {
            try
            {
                ConnectionFactory cf = new ConnectionFactory();
                cf.Uri = host;     // this should be in some config file
                cf.Ssl.CertPath = clientKey; //"C:\\Users\\brippe\\Documents\\Visual Studio 2010\\Projects\\AMQP\\AMQP\\bin\\x64\\keycert.p12";
                cf.Ssl.CertPassphrase = passPhrase;
                cf.Ssl.Enabled = true;

                using (IConnection conn = cf.CreateConnection())
                {
                    log.Debug("Connection established");
                    using (IModel ch = conn.CreateModel())
                    {
                        IBasicProperties bProps = ch.CreateBasicProperties();
                        bProps.SetPersistent(persist);
                        log.Debug("Routing to binding " + routingKey);
                        log.Debug("Persist message: " + persist);
                        ch.BasicPublish(exchange, routingKey, bProps, Encoding.UTF8.GetBytes(msg));
                        log.Debug(msg + " sent to queue");
                    }
                }
            }
            catch (Exception ex)
            {
                log.Error(ex.ToString());
            }
        }
    }
}
