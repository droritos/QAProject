using NUnit.Framework;

namespace UnitTests
{
    public class PlayerHealthSystemTests
    {
        [Test]
        public void TakeDamage_ReducesHealthCorrectly()
        {
            PlayerHealthSystem playerHealthSystem = new PlayerHealthSystem(100f);
            playerHealthSystem.TakeDamage(10f);
            
            Assert.AreEqual(90f, playerHealthSystem.CurrentHealth);
        }
        
    }
}
