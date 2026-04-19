using System;
using System.Collections.Generic;
using Eccentric;
using UnityEngine;

[CreateAssetMenu(menuName = nameof(WordsData), fileName = nameof(WordsData), order = 51)]
public class WordsData : ScriptableObject
{
    [SerializeField] private List<PointsWord> _words;
    
    public string GetWord(int value)
    {
        for (int i = _words.Count - 1; i > 0; i--)
        {
            if (value >= _words[i].Value)
                return EccentricInit.Instance.LocalisationManager.GetText(_words[i].Word);
        }

        return null;
    }

    public int GetID(int value)
    {
        for (int i = _words.Count - 1; i > 0; i--)
        {
            if (value >= _words[i].Value)
                return i;
        }

        return 0;
    }
}

[Serializable]
public struct PointsWord
{
    [SerializeField] private int _value;
    [SerializeField] private string _word;

    public int Value => _value;
    public string Word => _word;
}
