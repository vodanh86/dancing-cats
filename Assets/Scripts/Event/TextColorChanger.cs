using TMPro;
using UnityEngine;

public class TextColorChanger
{
    public static void SetColor(TMP_Text text, int wordIndex, Color32 color32)
    {
        TMP_WordInfo info = text.textInfo.wordInfo[wordIndex];
        for (int i = 0; i < info.characterCount; ++i)
        {
            int charIndex = info.firstCharacterIndex + i;
            int meshIndex = text.textInfo.characterInfo[charIndex].materialReferenceIndex;
            int vertexIndex = text.textInfo.characterInfo[charIndex].vertexIndex;
   
            Color32[] vertexColors = text.textInfo.meshInfo[meshIndex].colors32;
            vertexColors[vertexIndex + 0] = color32;
            vertexColors[vertexIndex + 1] = color32;
            vertexColors[vertexIndex + 2] = color32;
            vertexColors[vertexIndex + 3] = color32;
        }
 
        text.UpdateVertexData(TMP_VertexDataUpdateFlags.All);
    }
}
