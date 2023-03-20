using System.Reflection;
using System.Text.RegularExpressions;

namespace CsGOStateEmitter.Helper
{
    public class HtmlMessageHelper
    {
        /// <summary>
        /// Processa html de acordo com dados de modelo
        /// </summary>
        /// <param name="model"></param>
        /// <param name="path"></param>
        /// <param name="nameFile"></param>
        /// <returns></returns>
        public static string SetValuesContent(object model, string path, string nameFile)
        {
            var textHtml = GetFileLayout(path, nameFile);

            var allPropertiesModel = model.GetType().GetProperties();

            var regex = new Regex("\\{{(.*?)\\}}");
            var processedHtml = regex.Replace(textHtml, (match) =>
            {
                var nameProperty = match.Groups[1].Value;
                var property = allPropertiesModel.Single(prop => prop.Name == nameProperty);
                var value = property.GetValue(model);
                return value?.ToString();
            });
            return processedHtml;
        }

        public static string SetValuesContent(string html, Dictionary<string, string> values)
        {
            foreach (var item in values)
            {
                if (html.Contains(item.Key))
                    html = html.Replace(item.Key, item.Value);
            }
            return html;
        }

        public static string ReduceTextWithButton(string text, int numberCharacters, string button)
        {
            if (text.Length <= numberCharacters || numberCharacters == 0)
                return text;
            return $"{text.Substring(0, numberCharacters)}...{button}";
        }

        /// <summary>
        /// Get layout HTML para email (Diretorio wwwroot padrão)
        /// </summary>
        /// <param name="path"></param>
        /// <param name="nameFile"></param>
        /// <returns></returns>
        public static string GetFileLayout(string path, string nameFile)
        {
            //var rootPath = ConfigHelper.Get<string>($"Pathwwwroot");
            //if (string.IsNullOrWhiteSpace(rootPath) || string.IsNullOrWhiteSpace(nameFile) || string.IsNullOrWhiteSpace(path))
            //    return string.Empty;

            //string applicationPath = Path.Combine(Directory.GetCurrentDirectory(), @$"{rootPath}");

            var pathCurrentDirectory = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);

#if DEBUG
            pathCurrentDirectory = Directory.GetCurrentDirectory();
#endif

            return FileHelper.ReadFile(Path.Combine(pathCurrentDirectory, path), nameFile);
        }
    }
}
