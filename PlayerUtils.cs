using HarmonyLib;
using Steamworks;
using System.Linq;

namespace MuckMenu
{
    public static class PlayerUtils
    {
        private static float originalJumpForce = float.NaN;

        public static void AddCoins(int amount)
        {
            var item = ItemManager.Instance.allScriptableItems[3];
            item.amount = amount;
            InventoryUI.Instance.AddItemToInventory(item);
        }

        public static void DropCoins(int amount)
        {
            ClientSend.DropItem(3, amount);
        }

        public static void SetFullStamina() => PlayerStatus.Instance.stamina = PlayerStatus.Instance.maxStamina;
        public static void SetFullHealth() => PlayerStatus.Instance.hp = PlayerStatus.Instance.maxHp;
        public static void SetFullHunger() => PlayerStatus.Instance.hunger = PlayerStatus.Instance.maxHunger;

        public static void UnlockAllAchievements()
        {
            SteamUserStats.Achievements.Do(a => a.Trigger(true));
            SteamUserStats.StoreStats();
        }

        public static void ReviveAllPlayers()
        {
            GameManager.players.Values.Where(m => m != null && m.dead && !m.disconnected).Do(m => ClientSend.RevivePlayer(m.id, -1, false));
        }
    }
}