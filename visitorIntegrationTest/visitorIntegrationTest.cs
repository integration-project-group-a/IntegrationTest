using System;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Text;
using System.Xml;
using System.Xml.Schema;
using System.Xml.XPath;
using System.IO;
using System.Net;
using Newtonsoft.Json;

class VisitorIntegrationTest
{
    private static String recivedMessage;



    public static void Main()
    {
        var factory = new ConnectionFactory() { HostName = "10.3.56.27", UserName = "manager", Password = "ehb" };
        using (var connection = factory.CreateConnection())
        using (var channel = connection.CreateModel())
        {
            channel.ExchangeDeclare(exchange: "logs", type: "fanout");

            var queueName = channel.QueueDeclare().QueueName;
            channel.QueueBind(queue: queueName, exchange: "logs", routingKey: "");

            Console.WriteLine("Waiting for Tests.");

            var consumer = new EventingBasicConsumer(channel);
            consumer.Received += (model, ea) =>
            {
                Console.WriteLine("Received message");
                XmlDocument doc = new XmlDocument();
                doc.LoadXml(Encoding.UTF32.GetString(ea.Body));
                recivedMessage = Encoding.UTF32.GetString(ea.Body);

                doc.Schemas.Add(null, "../../../XSD/VisitorV2.xsd");

                doc.Validate(ValidationCallBack);

                
            };
            channel.BasicConsume(queue: queueName, autoAck: true, consumer: consumer);
            Console.ReadLine();
        }
      

        Console.ReadKey();
    }
    private static void ValidationCallBack(object sender, ValidationEventArgs args)
    {
        if (args.Severity == XmlSeverityType.Warning)
            Console.WriteLine("\tWarning: Matching schema not found.  No validation occurred." + args.Message);
        else
            Console.WriteLine("\tValidation error: " + args.Message);

        HttpWebRequest httpWebRequest = (HttpWebRequest)WebRequest.Create("http://10.3.56.26:9200/visitortest/invalidxml"); //url
        httpWebRequest.ContentType = "application/json"; //ContentType
        httpWebRequest.Method = "POST"; //Methode

        //body
        using (var streamWriter = new StreamWriter(httpWebRequest.GetRequestStream()))
        {
            streamWriter.Write("{\"message\": \""+ recivedMessage.Replace("\"", "'") + "\"}");
            Console.WriteLine("{\"message\": \"" + recivedMessage.Replace("\"","'") + "\"}");
            streamWriter.Flush();
            streamWriter.Close();
        }

        httpWebRequest.GetResponse(); //sending request
    }
}