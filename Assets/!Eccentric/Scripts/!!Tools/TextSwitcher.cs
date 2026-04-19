using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Eccentric
{
    public class TextSwitcher : MonoBehaviour
    {
        private TextSwitcher _instance;
        private List<TextMeshProUGUI> _textsMeshPro = new();
        private List<Text> _textsLegacy = new();
        private bool _isEnabled = true;
        [SerializeField] private KeyCode _buttonSwitchInUI = KeyCode.T;
        [SerializeField] private KeyCode _buttonSwitchAll = KeyCode.R;

        private void Awake()
        {
            if (_instance != null) return;
            _instance = this;
            DontDestroyOnLoad(_instance);
        }

        private void Update()
        {
            if (Input.GetKeyDown(_buttonSwitchInUI))
                TextSwitchInUI(!_isEnabled);
            if (Input.GetKeyDown(_buttonSwitchAll))
                TextSwitchAll(!_isEnabled);
        }

        private void TextSwitchAll(bool enable)
        {
            _textsMeshPro.Clear();
            _textsLegacy.Clear();
            _textsMeshPro = FindObjectsOfType<TextMeshProUGUI>(true).ToList();
            _textsLegacy = FindObjectsOfType<Text>(true).ToList();
            _isEnabled = enable;
            foreach (var item in _textsMeshPro)
            {
                item.enabled = enable;
            }

            foreach (var item in _textsLegacy)
            {
                item.enabled = enable;
            }
        }

        void TextSwitchInUI(bool enable)
        {
            _textsMeshPro.Clear();
            _textsLegacy.Clear();
            var canvases = FindObjectsOfType<Canvas>(true);

            foreach (var item in canvases)
            {
                if (item.renderMode != RenderMode.WorldSpace)
                {
                    var textsMeshPro = item.GetComponentsInChildren<TextMeshProUGUI>();
                    _textsMeshPro.AddRange(textsMeshPro);
                    var textsLegacy = item.GetComponentsInChildren<Text>();
                    _textsLegacy.AddRange(textsLegacy);
                }
            }

            foreach (var item in _textsMeshPro)
                item.enabled = enable;

            foreach (var item in _textsLegacy)
                item.enabled = enable;

            _isEnabled = enable;
        }
    }
}