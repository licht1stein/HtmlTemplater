using DotLiquid;
using System;
using System.IO;

namespace HtmlTemplater
{
    class Program
    {

        /// <summary>
        /// YAML -> HTML Generator
        /// </summary>
        /// <param name="template" optional="false">Template HTML file name</param>
        /// <param name="text">YAML text file name</param>
        /// <param name="target">Output target HTML file name</param>
        static void Main(string template, string text, string target)
        {
            string templateText = "";
            string textDictionary = "";

            if (template is null || text is null || target is null)
            {
                WriteAndExit($"You must provide all arguments. Run with -h for help.");
            }

            try
            {
                templateText = File.ReadAllText(template);

            }
            catch (FileNotFoundException) {
                WriteAndExit($"File not found: {template}");
            }

            try
            {
                textDictionary = File.ReadAllText(text);
            }
            catch (FileNotFoundException)
            {
                WriteAndExit($"File not found: {template}");
            }

            var yaml = OpenYaml(text);


            Template liquidTemplate = Template.Parse(templateText);  // Parses and compiles the template
            var result = liquidTemplate.Render(yaml); // Renders the
            File.WriteAllText(target, result);
        }

        static void WriteAndExit(string text)
        {
            Console.WriteLine(text);
            Environment.Exit(1);
        }

        static Hash OpenYaml(string file)
        {
            var deserializer = new YamlDotNet.Serialization.DeserializerBuilder().Build();
            return deserializer.Deserialize<Hash>(File.OpenText(file));
        }
    }
}
