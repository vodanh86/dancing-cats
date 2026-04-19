using System;
using UnityEngine;

public class StartButtonUI : MonoBehaviour
{
    public event Action GameStarted;

    public void SendAction()
    {
        GameStarted?.Invoke();
    }
}
