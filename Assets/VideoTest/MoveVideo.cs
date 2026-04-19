using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;

[ExecuteAlways]
public class MoveVideo : MonoBehaviour
{

    [SerializeField] private VideoPlayer _videoPlayer;
    [SerializeField] private Transform _quadTransform;
    [SerializeField] private int _bpm = 85;
    [SerializeField] private float _bitPerMeter = 2f;

    private float _timer;

    void Update()
    {
        _timer += Time.deltaTime;
        float bps = _bpm / 60f;
        if (Application.isPlaying) {
            _quadTransform.position = _timer * Vector3.forward * bps * _bitPerMeter;
        }
        _videoPlayer.time = _quadTransform.localPosition.z / (bps * _bitPerMeter);
        _videoPlayer.playbackSpeed = 0f;
        _videoPlayer.Play();
    }
}
