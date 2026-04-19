using System;
using System.Collections.Generic;

namespace Eccentric
{
    public class LocalisationManager
    {
        private List<LocalisationDataSO> _localisationDatas;
        private readonly Language _language;
        private Dictionary<string, LocalisationData> _dictionary = new();

        public LocalisationManager(Language language, List<LocalisationDataSO> localisationDatas)
        {
            _language = language;
            _localisationDatas = localisationDatas;
        }

        public void CreateDictionary()
        {
            foreach (var itemLocalisationData in _localisationDatas)
            {
                foreach (var itemInData in itemLocalisationData.LocalisationList)
                {
                    _dictionary.TryAdd(itemInData.Key.ToUpper(), itemInData);
                }
            }
        }

        public string GetText(string key, params object[] tokens)
        {
            var text = _language switch
            {
                Language.Russian => _dictionary[key.ToUpper()].RussianText,
                Language.Spanish => _dictionary[key.ToUpper()].SpanishText,
                Language.German => _dictionary[key.ToUpper()].GermanText,
                Language.Turkish => _dictionary[key.ToUpper()].TurkishText,

                _ => _dictionary[key.ToUpper()].EnglishText
            };
            
            if (string.IsNullOrEmpty(text)) text = _dictionary[key.ToUpper()].EnglishText;
            
            for (int i = 0; i < tokens.Length; i++)
            {
                text = text.Replace($"{{T{i}}}", tokens[i].ToString());
            }

            return text;
        }
    }
}

[Serializable]
public struct LocalisationData
{
    public string Key;
    public string RussianText;
    public string EnglishText;
    public string SpanishText;
    public string GermanText;
    public string TurkishText;
}