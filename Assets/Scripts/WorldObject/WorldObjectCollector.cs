using System;
using UnityEngine;
using AssetKits.ParticleImage;

public class WorldObjectCollector : MonoBehaviour
{
    [SerializeField] private Wallet _wallet;
    [SerializeField] private ParticleImage _particle;

    public event Action PointsSphereCollected;

    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent(out WorldObject worldObject))
        {
            if (worldObject is Coin coin)
            {
                _wallet.Add(coin.Value);
                _particle.Play();
                coin.DestroyItself();
            }
            else if (worldObject is GamePointsSphere points)
            {
                PointsSphereCollected?.Invoke();
                points.DestroyItself();
            }
        }
    }
}
