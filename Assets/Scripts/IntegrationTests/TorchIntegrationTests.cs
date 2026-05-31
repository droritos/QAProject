using System.Collections;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

public class TorchIntegrationTests
{
    private GameObject playerGo;
    private PlayerInteractor playerInteractor;
    private GameObject torchGo;
    private Torch torch;
    private Material testLitMaterial;

    [SetUp]
    public void Setup()
    {
        // Setup Player
        playerGo = new GameObject("Player");
        playerInteractor = playerGo.AddComponent<PlayerInteractor>();
        var playerCollider = playerGo.AddComponent<SphereCollider>();
        playerCollider.isTrigger = false;
        var rb = playerGo.AddComponent<Rigidbody>();
        rb.isKinematic = true;

        // Setup Torch
        torchGo = new GameObject("Torch");
        torchGo.transform.position = new Vector3(0, 0, 5); // Start out of range
        torch = torchGo.AddComponent<Torch>();
        
        var torchCollider = torchGo.AddComponent<SphereCollider>();
        torchCollider.isTrigger = true;
        
        var renderer = torchGo.AddComponent<MeshRenderer>();
        testLitMaterial = new Material(Shader.Find("Standard"));
        
        // Use reflection to assign private serialize fields for the test
        var torchType = torch.GetType();
        torchType.GetField("meshRenderer", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance).SetValue(torch, renderer);
        torchType.GetField("litMaterial", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance).SetValue(torch, testLitMaterial);
        
        // Re-enable so OnEnable binds events since we just set the torch reference
        torch.enabled = false;
        torch.enabled = true;
    }

    [TearDown]
    public void Teardown()
    {
        UnityEngine.Object.DestroyImmediate(playerGo);
        UnityEngine.Object.DestroyImmediate(torchGo);
        UnityEngine.Object.DestroyImmediate(testLitMaterial);
    }

    // --- Smoke Test ---
    [Test]
    public void TorchPrefab_InstantiatesWithoutErrors()
    {
        Assert.IsNotNull(torch);
        Assert.IsNotNull(playerInteractor);
    }

    // --- Integration Test ---
    [UnityTest]
    public IEnumerator PlayerPressesE_NearTorch_LightsUpTorch()
    {
        // Move player to torch
        playerGo.transform.position = torchGo.transform.position;
        // Wait a fixed frame for OnTriggerEnter to process
        yield return new WaitForFixedUpdate(); 
        
        // Simulate 'E' key press by calling our Interact method
        playerInteractor.Interact(); 

        Assert.IsTrue(torch.IsLit, "Torch should be lit after player interacts near it.");
    }

    // --- Regression Test ---
    [UnityTest]
    public IEnumerator PlayerLeavesTorch_StaysLit()
    {
        // Move near and interact
        playerGo.transform.position = torchGo.transform.position;
        yield return new WaitForFixedUpdate();
        
        playerInteractor.Interact();
        Assert.IsTrue(torch.IsLit);

        // Move away
        playerGo.transform.position = new Vector3(100, 100, 100);
        yield return new WaitForFixedUpdate();
        
        // Assert it's still lit
        Assert.IsTrue(torch.IsLit, "Torch should stay lit after player leaves.");
    }

    // --- Functional Test ---
    [UnityTest]
    public IEnumerator PlayerWalksToTorch_PressesE_TorchVisualsTurnYellow()
    {
        var renderer = torchGo.GetComponent<MeshRenderer>();

        // Walk to torch
        playerGo.transform.position = torchGo.transform.position;
        yield return new WaitForFixedUpdate();

        // Interact
        playerInteractor.Interact();

        // The material should now be the yellow/lit material
        Assert.AreEqual(testLitMaterial, renderer.sharedMaterial, "Torch visuals did not update to lit material.");
    }
}
