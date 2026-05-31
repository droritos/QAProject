using NUnit.Framework;
using UnityEngine;
using System;

public class TorchUnitTests
{
    private GameObject torchGameObject;
    private Torch torch;

    [SetUp]
    public void Setup()
    {
        torchGameObject = new GameObject("Torch");
        torch = torchGameObject.AddComponent<Torch>();
    }

    [TearDown]
    public void Teardown()
    {
        UnityEngine.Object.DestroyImmediate(torchGameObject);
    }

    [Test]
    public void Torch_StartsUnlit()
    {
        // Assert
        Assert.IsFalse(torch.IsLit, "Torch should start unlit.");
    }

    [Test]
    public void LightUp_SetsIsLitToTrue()
    {
        // Act
        torch.LightUp();

        // Assert
        Assert.IsTrue(torch.IsLit, "Calling LightUp should set IsLit to true.");
    }

    [Test]
    public void LightUp_FiresOnLitEvent()
    {
        // Arrange
        bool eventFired = false;
        UnityEngine.Events.UnityAction<bool> action = (state) => eventFired = state;
        Managers.EventBusManager.OnChangeLightState += action;

        // Act
        torch.LightUp();

        // Assert
        Assert.IsTrue(eventFired, "Calling LightUp should fire the EventBusManager.OnChangeLightState event.");
        
        // Cleanup
        Managers.EventBusManager.OnChangeLightState -= action;
    }
}
