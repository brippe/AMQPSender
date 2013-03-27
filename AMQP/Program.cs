using System;
using System.Text;
using System.Configuration;
using RabbitMQ.Client;

namespace AMQP
{
    public class CampusAMQPSender
    {
        
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
