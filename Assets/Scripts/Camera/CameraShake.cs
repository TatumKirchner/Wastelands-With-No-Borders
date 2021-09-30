using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraShake : MonoBehaviour
{
    [SerializeField] private Transform camTransform;

    public float shakeDuration = 0f;
    [SerializeField] private float shakeAmount = 0.7f;
    [SerializeField] private float decreaseFactor = 1.0f;

    private ProtectCameraFromWallClip protectCamera;

    private Vector3 originalPos;

    private void Awake()
    {
        if (camTransform == null)
            camTransform = GetComponent<Transform>();

        protectCamera = GetComponentInParent<ProtectCameraFromWallClip>();
    }

    private void OnEnable()
    {
        originalPos = camTransform.localPosition;
    }

    private void Update()
    {
        // When shake duration is set disable ProtectCameraFromWallClip class and move the camera to a random point in a sphere around the camera.
        if (shakeDuration > 0)
        {
            protectCamera.enabled = false;
            camTransform.localPosition = originalPos + Random.insideUnitSphere * shakeAmount;
            shakeDuration -= Time.deltaTime * decreaseFactor;
        }
        else
        {
            shakeDuration = 0f;
            camTransform.localPosition = originalPos;
            protectCamera.enabled = true;
        }
    }
}
