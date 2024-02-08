using Core.Data.Items;
using Core.PlayerSystems;
using NUnit.Framework;

namespace Tests.CoreTest.Data
{
    public class BasicItemsTest
    {
        [Test]
        public void CoinsTest()
        {
            var player = CreatePlayer();
            var coins = new Coins(10);
            coins.Use(player);
            Assert.AreEqual(10, player.Inventory.Gold);
        }

        [Test]
        public void BasicTrapTest()
        {
            var player = CreatePlayer();
            var trap = new BasicTrap(10);
            trap.Use(player);
            Assert.AreEqual(5, player.HealthSystem.Health);
        }
        
        [Test]
        public void BasicSwordTest()
        {
            var player = CreatePlayer();
            var sword = new BasicSword(10);
            sword.Use(player);
            Assert.AreEqual(9, sword.Damage);
            
            var swordFlat = new BasicSword(1);
            swordFlat.Use(player);
            swordFlat.Use(player);
            Assert.AreEqual(1, swordFlat.Damage);
        }

        [Test]
        public void BasicShieldTest()
        {
            var player = CreatePlayer();
            var shield = new BasicShield(10);
            shield.Use(player);
            Assert.AreEqual(9, shield.Shield);
            var shieldFlat = new BasicShield(1);
            shieldFlat.Use(player);
            shieldFlat.Use(player);
            Assert.AreEqual(0, shieldFlat.Shield);
        }
        
        [Test]
        public void HealthPotionTest()
        {
            var player = CreatePlayer();
            player.HealthSystem.TakeDamage(new DamageInfo(5));
            var healthPotion = new HealthPotion(10);
            healthPotion.Use(player);
            Assert.AreEqual(15, player.HealthSystem.Health);
        }

        [Test]
        public void PickUpAbleItemTest()
        {
            var player = CreatePlayer();
            
            Assert.AreEqual(1, player.Inventory.Weapon.Damage);
            var sword = new BasicSword(10);
            var pickUpAbleItem = new PickUpAbleItem(sword);
            pickUpAbleItem.Use(player);
            Assert.AreEqual(10, player.Inventory.Weapon.Damage);
            
            Assert.AreEqual(0, player.Inventory.Shield.Shield);
            var shield = new BasicShield(10);
            pickUpAbleItem = new PickUpAbleItem(shield);
            pickUpAbleItem.Use(player);
            Assert.AreEqual(10, player.Inventory.Shield.Shield);
        }
        
        private PlayerPawn CreatePlayer()
        {
            return new PlayerPawn(new StandardHealthSystem(15));
        }
    }
}