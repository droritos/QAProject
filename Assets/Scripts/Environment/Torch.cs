using System;
using Managers;
using UnityEngine;

public class Torch : MonoBehaviour
{
    [SerializeField] private MeshRenderer meshRenderer;
    [SerializeField] private Material litMaterial;

    public bool IsLit { get; private set; }

    private void OnEnable()
    {
        EventBusManager.OnChangeLightState += HandleTorchLit;
    }

    private void OnDisable()
    {
        EventBusManager.OnChangeLightState -= HandleTorchLit;
    }

    public void LightUp()
    {
        if (IsLit) return;
        IsLit = true;
        //EventBusManager.RaiseLightChange(true);
    }

    private void HandleTorchLit(bool state)
    {
        // Only change material if the event is true and THIS torch has been lit
        if (state && IsLit && meshRenderer != null && litMaterial != null)
        {
            meshRenderer.material = litMaterial;
        }
    }
}
