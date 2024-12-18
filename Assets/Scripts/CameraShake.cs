using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraShake : MonoBehaviour
{
    [SerializeField] private float dampingSpeed = 1.0f;
    [SerializeField] private float magnitude = 0.5f;
    [SerializeField] private float duration = 0.5f;

    private Transform cameraTransform;
    private Vector3 initialPosition;
    private float shakeMagnitude;
    private float shakeElapsedTime;

    void Awake()
    {
        if (cameraTransform == null)
        {
            cameraTransform = GetComponent<Transform>();
        }
    }

    void OnEnable()
    {
        GameEvents.OnFoodEaten += Shake;
        initialPosition = cameraTransform.localPosition;
    }

    void OnDisable()
    {
        GameEvents.OnFoodEaten -= Shake;
    }

    private void Shake()
    {
        if (SettingsManager.Instance.cameraShakeEnabled)
        {
            shakeMagnitude = magnitude;
            shakeElapsedTime = duration;
        }
    }

    public void TriggerShake(float _shakeDuration = 0.5f, float _shakeMagnitude = 0.5f)
    {
        if (SettingsManager.Instance.cameraShakeEnabled)
        {
            shakeMagnitude = _shakeMagnitude;
            shakeElapsedTime = _shakeDuration;
        }
    }

    void LateUpdate()
    {
        if (shakeElapsedTime > 0)
        {
            cameraTransform.localPosition = initialPosition + new Vector3(
                Mathf.PerlinNoise(Time.time * shakeMagnitude, 0f) * 2 - 1,
                Mathf.PerlinNoise(0f, Time.time * shakeMagnitude) * 2 - 1,
                0) * shakeMagnitude;

            shakeElapsedTime -= Time.deltaTime * dampingSpeed;
        }
        else
        {
            shakeElapsedTime = 0f;
            cameraTransform.localPosition = initialPosition;
        }
    }
}
