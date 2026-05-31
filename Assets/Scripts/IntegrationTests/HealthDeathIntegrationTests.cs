using System.Collections;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;
namespace IntegrationTests
{
    public class HealthDeathIntegrationTests
    {
        //[cite_start]
        [UnityTest]
        public IEnumerator Player_TakingLethalDamage_TriggersDeathHandler()
        {
            GameObject playerObject = new GameObject("Player");
            PlayerHealth healthComponent = playerObject.AddComponent<PlayerHealth>();
            DeathHandler deathComponent = playerObject.AddComponent<DeathHandler>();
            
            healthComponent.TakeDamage(100f);
            yield return null;
            
            Assert.IsTrue(deathComponent.PlayerHasDied);
            Assert.AreEqual(1, deathComponent.DeathCount);
            Object.Destroy(playerObject);
        }
        
    }
}
