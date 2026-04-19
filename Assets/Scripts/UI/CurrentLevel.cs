using UnityEngine;
using Eccentric;
using TMPro;

public class CurrentLevel : MonoBehaviour
{
    [SerializeField] private TMP_Text _level;

    private const string LocalizationParameter = "LEVEL";

    private void Start()
    {
        _level.text = EccentricInit.Instance.LocalisationManager.GetText(LocalizationParameter, SaveSystemWithData.PlayerData.CurrentLevel.ToString());
    }
}
