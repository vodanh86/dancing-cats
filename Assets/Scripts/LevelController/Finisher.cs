using System;
using UnityEngine;

public class Finisher : MonoBehaviour
{
    public event Action LevelEnded;

    public static Finisher Instance;

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(this);
    }

    private void OnTriggerEnter(Collider other)
    {
        LevelEnded?.Invoke();
        this.gameObject.SetActive(false);
    }
}
