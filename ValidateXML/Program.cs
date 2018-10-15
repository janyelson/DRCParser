using System;
using System.IO;
using System.Xml;
using System.Xml.Schema;

namespace ValidateXML
{
    class Program
    {

        static void Main(string[] args)
        {

            if (args.Length == 0)
            {
                Console.WriteLine("É necessário o argumento para o arquivo xml.");
                return;
            }

            try { 
                XmlReaderSettings drcSettings = new XmlReaderSettings();
                drcSettings.Schemas.Add("http://schemas.oceanehr.com/templates", "arquivos/FICHA_DRC.xsd");
                drcSettings.Schemas.Add("http://schemas.openehr.org/v1", "arquivos/Structure.xsd");
                drcSettings.Schemas.Add("http://schemas.openehr.org/v1", "arquivos/BaseTypes.xsd");
                drcSettings.ValidationType = ValidationType.Schema;
                drcSettings.ValidationFlags |= XmlSchemaValidationFlags.ProcessInlineSchema;
                drcSettings.ValidationFlags |= XmlSchemaValidationFlags.ProcessSchemaLocation;
                drcSettings.ValidationFlags |= XmlSchemaValidationFlags.ReportValidationWarnings;
                drcSettings.ValidationEventHandler += new ValidationEventHandler(drcSettingsValidationEventHandler);

                XmlReader drc = XmlReader.Create(args[0], drcSettings);

                while (drc.Read()) { }

                XMLParser xml = new XMLParser(args[0]);
                xml.Run();

            } catch(FileNotFoundException e) {
                Console.WriteLine("Argumento para o arquivo xml não encontrado. " + e.Message);
            }
        }

        static void drcSettingsValidationEventHandler(object sender, ValidationEventArgs e)
        {
            if (e.Severity == XmlSeverityType.Warning)
                Console.WriteLine("Aviso:" + e.Message);
            else
                Console.WriteLine("Erro [" + e.Exception.LineNumber + "]: " + e.Message);
        }
    }
}
