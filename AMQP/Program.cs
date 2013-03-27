using System;
using System.Text;
using System.Configuration;
using RabbitMQ.Client;

namespace AMQP
{
    public class CampusAMQPSender
    {
        //private static string server = ConfigurationManager.AppSettings["host"];
        //private static string exchange = ConfigurationManager.AppSettings["exchange"];
        //static string exchangeType = "topic";
        //static string routingKey = "fc"; // cc, fc, or sce        
        // can't get appsettings to work in Win 2008 -- had to move info to powershell script
        
        /*static void Main(string[] args)
        {
            CampusAMQPSender.publishMessage("fc", "Brad sent message from C#");
            Console.Out.WriteLine("Sent message to amqp server");            
        }*/

        public static void publishMessage(string host, string exchange, string routingKey, string msg)
        {
            ConnectionFactory cf = new ConnectionFactory();            
            cf.Uri = host;     // this should be in some config file            

            using (IConnection conn = cf.CreateConnection())
            {
                using (IModel ch = conn.CreateModel())
                {
                    ch.BasicPublish(exchange, routingKey, null, Encoding.UTF8.GetBytes(msg));
                }
            }
        }
    }
}
