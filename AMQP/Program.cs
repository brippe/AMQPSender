using System;
using System.Text;
using System.Configuration;
using System.Net.Security;
using System.Security.Cryptography.X509Certificates;
using RabbitMQ.Client;

namespace AMQP
{
    public class CampusAMQPSender
    {
        // sso-shibboleth.nocccd.edu 10.200.7.236 - amqps://admin:<pass>@sso-shibboleth:5671
        static void Main(string[] args)
        {
            CampusAMQPSender.publishMessage("amqps://admin:Pass4Rabbit$@sso-shibboleth:5671", "nocccd_x", "fc", 
                "Brad sent message from C#", true);
            Console.Out.WriteLine("Sent message to amqp server"); 
        }

        public static void publishMessage(string host, string exchange, string routingKey, string msg, bool persist)
        {
            try
            {
                ConnectionFactory cf = new ConnectionFactory();
                cf.Uri = host;     // this should be in some config file
                cf.Ssl.CertPath = "C:\\Users\\brippe\\Documents\\Visual Studio 2010\\Projects\\AMQP\\AMQP\\bin\\x64\\keycert.p12";
                cf.Ssl.CertPassphrase = "Pass4Cert";
                cf.Ssl.Enabled = true;

                using (IConnection conn = cf.CreateConnection())
                {
                    using (IModel ch = conn.CreateModel())
                    {
                        IBasicProperties bProps = ch.CreateBasicProperties();
                        bProps.SetPersistent(persist);
                        ch.BasicPublish(exchange, routingKey, bProps, Encoding.UTF8.GetBytes(msg));
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
        }
    }
}
