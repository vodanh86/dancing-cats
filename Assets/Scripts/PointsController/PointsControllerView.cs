using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class PointsControllerView : MonoBehaviour
{
    [SerializeField] private TMP_Text _goodText;
    [SerializeField] private TMP_Text _perfecText;
    [SerializeField] private PerfectAnimation _perfectAnimation;
    [SerializeField] private TMP_Text _multiplierText;
    [SerializeField] private WordsData _words;

    private const string Perfect = nameof(Perfect);
    private const string Good = nameof(Good);

    private void Awake()
    {
        Hide();
    }

    public void SetStatus(bool isPerfectHit, int multiplier)
    {
        if (isPerfectHit)
        {
            if (multiplier > 1)
            {
                _multiplierText.text = "x" + multiplier.ToString();
            }
            else
            {
                _multiplierText.text = null;
            }
        }

        SwitchObjects(isPerfectHit, multiplier);
    }

    private void SwitchObjects(bool isPerfectHit, int multiplier)
    {

        //_perfecText.gameObject.SetActive(isPerfectHit);
        if (isPerfectHit)
        {
            _perfectAnimation.Play(_words.GetID(multiplier));
            _goodText.gameObject.SetActive(false);
            _perfecText.text = _words.GetWord(multiplier);
        }
        else {
            _goodText.gameObject.SetActive(true);
            _perfectAnimation.Hide();
        }
        

        BlockManager.Instance.SetBlocksIconStatus(isPerfectHit);
    }

    public void Hide()
    {
        _goodText.gameObject.SetActive(false);
        _perfectAnimation.Hide();
    }
}


