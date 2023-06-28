using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using ValheimLegends;



namespace ValheimLegends
{
    internal class VL_Localization
    {
        public static void InitializeConfig()
        {
            string currentLanguage = Localization.instance.GetSelectedLanguage();
            string externalFilePath = Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), $"Localized_{currentLanguage}.json");
            var assembly = Assembly.GetExecutingAssembly();
            string defaultNamespace = assembly.GetName().Name;
            string embeddedResourceName = $"{defaultNamespace}.Localized_{currentLanguage}.json";
            string defaultEmbeddedResourceName = $"{defaultNamespace}.Localized_English.json";

            if (File.Exists(externalFilePath))
            {
                // 加载外部文件
                string json = File.ReadAllText(externalFilePath);
                IDictionary<string, object> translations = JsonConvert.DeserializeObject<IDictionary<string, object>>(json);
                LoadTranslations(translations);
                Debug.Log($"Successfully loaded external translation file：Localized_{currentLanguage}.json");
            }
            else if (assembly.GetManifestResourceNames().Contains(embeddedResourceName))
            {
                // 加载嵌入资源
                using (Stream stream = assembly.GetManifestResourceStream(embeddedResourceName))
                {
                    using (StreamReader reader = new StreamReader(stream))
                    {
                        string json = reader.ReadToEnd();
                        IDictionary<string, object> translations = JsonConvert.DeserializeObject<IDictionary<string, object>>(json);
                        LoadTranslations(translations);
                    }
                }
            }
            else
            {
                // 加载默认嵌入资源
                using (Stream stream = assembly.GetManifestResourceStream(defaultEmbeddedResourceName))
                {
                    if (stream == null)
                    {
                        Debug.LogError($"No localization file named {defaultEmbeddedResourceName} found!");
                        return;
                    }
                    using (StreamReader reader = new StreamReader(stream))
                    {
                        string json = reader.ReadToEnd();
                        IDictionary<string, object> translations = JsonConvert.DeserializeObject<IDictionary<string, object>>(json);
                        LoadTranslations(translations);
                    }
                }
            }
        }



        public static void LoadTranslations(IDictionary<string, object> translations)
        {
            const string translationPrefix = "Legends_";

            if (translations == null)
            {
                Debug.LogError("Unable to parse translation file!");
                return;
            }

            var localizationType = typeof(Localization);
            var translationsField = localizationType.GetField("m_translations", BindingFlags.NonPublic | BindingFlags.Instance);
            var translationsDict = (Dictionary<string, string>)translationsField.GetValue(Localization.instance);

            var oldEntries = translationsDict
                .Where(entry => entry.Key.StartsWith(translationPrefix))
                .ToList();

            foreach (var entry in oldEntries)
            {
                translationsDict.Remove(entry.Key);
            }

            foreach (var translation in translations)
            {
                translationsDict.Add(translation.Key, translation.Value.ToString());
            }
        }




        public static void LoadEmbeddedAssembly(Assembly assembly, string assemblyName)
        {
            var stream = assembly.GetManifestResourceStream($"{assembly.GetName().Name}.{assemblyName}");
            if (stream == null)
            {
                Debug.LogError($"Could not load embedded assembly ({assemblyName})!");
                return;
            }

            using (stream)
            {
                var data = new byte[stream.Length];
                stream.Read(data, 0, data.Length);
                Assembly.Load(data);
            }
        }
    }
}
