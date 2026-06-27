using System.Collections;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

public class TorchPerformanceTests
{
    private GameObject playerGo;
    private PlayerInteractor playerInteractor;

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
    }

    [TearDown]
    public void Teardown()
    {
        Object.DestroyImmediate(playerGo);
    }

    // --- Load Behavior Test ---
    [UnityTest]
    public IEnumerator SpawnMultipleTorches_LoadBehaviorTest()
    {
        int numTorches = 1000;
        GameObject[] torches = new GameObject[numTorches];
        Material testLitMaterial = new Material(Shader.Find("Standard"));

        float startTime = Time.realtimeSinceStartup;

        // Instantiate 1000 torches
        for (int i = 0; i < numTorches; i++)
        {
            torches[i] = new GameObject("Torch_" + i);
            torches[i].transform.position = new Vector3(i, 0, 0); // Spread them out
            Torch torch = torches[i].AddComponent<Torch>();
            
            var torchCollider = torches[i].AddComponent<SphereCollider>();
            torchCollider.isTrigger = true;
            var renderer = torches[i].AddComponent<MeshRenderer>();
            
            // Set required private fields
            var torchType = torch.GetType();
            torchType.GetField("meshRenderer", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance).SetValue(torch, renderer);
            torchType.GetField("litMaterial", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance).SetValue(torch, testLitMaterial);
        }

        yield return null; // Wait for a frame

        float timeTaken = Time.realtimeSinceStartup - startTime;
        
        Assert.IsTrue(timeTaken < 5.0f, $"Spawning {numTorches} torches took too long: {timeTaken}s");

        // Clean up
        for (int i = 0; i < numTorches; i++)
        {
            Object.DestroyImmediate(torches[i]);
        }
        Object.DestroyImmediate(testLitMaterial);
    }

    // --- Stability Test ---
    [UnityTest]
    public IEnumerator RapidInteraction_StabilityTest()
    {
        // Setup a single torch
        GameObject torchGo = new GameObject("StabilityTorch");
        Torch torch = torchGo.AddComponent<Torch>();
        var torchCollider = torchGo.AddComponent<SphereCollider>();
        torchCollider.isTrigger = true;
        var renderer = torchGo.AddComponent<MeshRenderer>();
        Material testLitMaterial = new Material(Shader.Find("Standard"));
        
        var torchType = torch.GetType();
        torchType.GetField("meshRenderer", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance).SetValue(torch, renderer);
        torchType.GetField("litMaterial", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance).SetValue(torch, testLitMaterial);

        // Move player to torch
        playerGo.transform.position = torchGo.transform.position;
        yield return new WaitForFixedUpdate();

        int interactionCount = 1000;
        
        // Rapidly interact in a loop, ensuring no exceptions are thrown
        Assert.DoesNotThrow(() => 
        {
            for (int i = 0; i < interactionCount; i++)
            {
                playerInteractor.Interact();
            }
        }, "Rapid interaction caused an exception.");

        // Clean up
        Object.DestroyImmediate(torchGo);
        Object.DestroyImmediate(testLitMaterial);
    }
}
