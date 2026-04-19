using TMPro;
using UnityEngine;

[System.Serializable]
public struct GradientColors
{
    public Color ColorTop;
    public Color ColorBottom;
}

public class PerfectAnimation : MonoBehaviour
{

    [SerializeField] private Animator _animator;
    [SerializeField] private AnimationCurve _scaleCurve;
    [SerializeField] private GradientColors[] _gradientColorsArray;
    [SerializeField] private TextMeshProUGUI _text;

    private int _colorIndex;

    public void Play(int value)
    {
        gameObject.SetActive(true);
        _animator.SetTrigger("Hit");
        float scale = _scaleCurve.Evaluate((float)value / 30f);
        transform.localScale = Vector3.one * scale;

        _colorIndex = value % _gradientColorsArray.Length;

        Color top = _gradientColorsArray[_colorIndex].ColorTop;
        Color bottom = _gradientColorsArray[_colorIndex].ColorBottom;
        VertexGradient vertexGradient = new VertexGradient();
        vertexGradient.topLeft = top;
        vertexGradient.topRight = top;
        vertexGradient.bottomLeft = bottom;
        vertexGradient.bottomRight = bottom;
        _text.colorGradient = vertexGradient;
        _text.ForceMeshUpdate();
    }

    public void Hide()
    {
        gameObject.SetActive(false);
    }

}
