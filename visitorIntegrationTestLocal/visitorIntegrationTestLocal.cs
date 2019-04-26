using System;
using System.Text;
using System.Xml;
using System.Xml.Schema;

namespace visitorIntegrationTestLocal
{
    class VisitorIntegrationTestLocal
    {
        public static void Main()
        {
            Console.WriteLine("local test");
            XmlDocument doc1 = new XmlDocument();
            doc1.Load("../../../XML/VisitorV2.xml");

            doc1.Schemas.Add(null, "../../../XSD/VisitorV2.xsd");


            doc1.Validate(ValidationCallBack);


            XmlDocument doc2 = new XmlDocument();
            doc2.Load("../../../XML/VisitorV2Invalid.xml");
            

            doc2.Schemas.Add(null, "../../../XSD/VisitorV2.xsd");

            doc2.Validate(ValidationCallBack);


            Console.ReadKey();
        }
        private static void ValidationCallBack(object sender, ValidationEventArgs args)
        {
            if (args.Severity == XmlSeverityType.Warning)
                Console.WriteLine("\tWarning: Matching schema not found.  No validation occurred." + args.Message);
            else
                Console.WriteLine("\tValidation error: " + args.Message);
        }
    }
}