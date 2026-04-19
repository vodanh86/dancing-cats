using System;
using UnityEngine;

public class PhaseChanger : MonoBehaviour
{
    [SerializeField] private Phase _phace;

    public event Action FinalChange;
    public Phase Phace => _phace;

    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent(out Phase phase))
            FinalChange?.Invoke();
    }
}