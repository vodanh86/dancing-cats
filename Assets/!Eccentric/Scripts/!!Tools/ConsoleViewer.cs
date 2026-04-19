using System.Collections.Generic;
using System.Collections;
using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Eccentric
{
    public class ConsoleViewer : MonoBehaviour
    {
        private static bool _initialized;
        [SerializeField] private GameObject _logPanel;
        [SerializeField] private TextMeshProUGUI _logText;
        [Space(10)] [SerializeField] private Toggle _log;
        [SerializeField] private Toggle _warning;
        [SerializeField] private Toggle _error;
        [Space(5)] [SerializeField] private Button _closeButton;
        [Space(10)] [SerializeField] private int _touchToSwitchCount = 4;

        private Coroutine _coroutineSwitchActivate;
        private List<message> _logsList = new();
        private int _touchCount;


        private void Awake()
        {
            if (_initialized)
            {
                Destroy(gameObject);
                return;
            }

            Application.logMessageReceived += LogCallback;
            DontDestroyOnLoad(gameObject);
            _initialized = true;
        }

        private void SetActivate(bool enabled)
        {
            _logPanel.SetActive(enabled);
            _touchCount = 0;

            if (enabled)
                _closeButton.interactable = false;
        }

        public void SetTextSize(string size)
        {
            if (size == string.Empty) return;

            _logText.fontSize = int.Parse(size);
        }

        public void TouchToSwitch()
        {
            _touchCount++;
            if (_coroutineSwitchActivate == null)
                _coroutineSwitchActivate = StartCoroutine(SwitchActivate());
        }

        private void LogCallback(string logString, string stackTrace, LogType type)
        {
            string color;
            switch (type)
            {
                case LogType.Error:
                {
                    color = "CD0303";
                    break;
                }
                case LogType.Warning:
                {
                    color = "FFD800";
                    break;
                }
                default:
                {
                    color = "FFFFFF";
                    break;
                }
            }

            string coloredText = "<color=#" + color + ">" + logString + "</color>\r\n";
            ;
            _logsList.Add(new message(type, coloredText));

            if (gameObject.activeSelf)
                UpdateLogs();
        }

        public void UpdateLogs()
        {
            string logsText = string.Empty;

            for (int i = _logsList.Count - 1; i >= 0; i--)
            {
                switch (_logsList[i].Type)
                {
                    case LogType.Error:
                    {
                        if (_error.isOn)
                            logsText += _logsList[i].Text;

                        break;
                    }
                    case LogType.Warning:
                    {
                        if (_warning.isOn)
                            logsText += _logsList[i].Text;

                        break;
                    }
                    default:
                    {
                        if (_log.isOn)
                            logsText += _logsList[i].Text;

                        break;
                    }
                }
            }

            _logText.text = logsText;
        }

        private IEnumerator SwitchActivate()
        {
            float timer = 0;
            while (true)
            {
                if (_touchCount >= _touchToSwitchCount)
                {
                    SetActivate(!_logPanel.activeSelf);

                    if (_logPanel.activeSelf)
                    {
                        yield return new WaitForSeconds(1);
                        _closeButton.interactable = true;
                    }

                    break;
                }

                timer += Time.deltaTime;
                if (timer > 3)
                    break;

                yield return null;
            }

            _coroutineSwitchActivate = null;
        }

        private void OnDestroy()
        {
            Application.logMessageReceived -= LogCallback;
        }


        [Serializable]
        struct message
        {
            public LogType Type;
            public string Text;

            public message(LogType type, string text)
            {
                Type = type;
                Text = text;
            }
        }
    }
}
