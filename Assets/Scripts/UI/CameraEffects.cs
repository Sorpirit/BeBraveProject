using System;
using Cinemachine;
using DG.Tweening;
using UnityEngine;


namespace UI
{
    public class CameraEffects : MonoBehaviour, ICameraEffects
    {
        [SerializeField] private CinemachineVirtualCamera virtualCamera;
        [SerializeField] private float zoomedInSize = 2;
        [SerializeField] private float zoomDuration = 0.5f;

        private float _originalSize;
        
        private void Start()
        {
            _originalSize = virtualCamera.m_Lens.OrthographicSize;
        }

        public Tween ZoomIn()
        {
            return DOTween.To(() => virtualCamera.m_Lens.OrthographicSize, value => virtualCamera.m_Lens.OrthographicSize = value, zoomedInSize, zoomDuration);
        }

        public Tween ZoomOut()
        {
            return DOTween.To(() => virtualCamera.m_Lens.OrthographicSize, value => virtualCamera.m_Lens.OrthographicSize = value, _originalSize, zoomDuration);
        }
    }
}