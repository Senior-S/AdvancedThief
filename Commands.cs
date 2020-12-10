using Rocket.API;
using System.Collections.Generic;
using Rocket.Unturned.Player;
using SDG.Unturned;
using Rocket.Unturned.Chat;

namespace AdvancedThief
{
    public class CommandRob : IRocketCommand
    {
        Items vinventory = new Items(7);
        public AllowedCaller AllowedCaller => AllowedCaller.Player;
        public string Name => "rob";
        public string Help => "Rob a player inventory!";
        public string Syntax => string.Empty;
        public List<string> Aliases => new List<string>();
        public List<string> Permissions => new List<string> { "SS.Rob" };

        public void Execute(IRocketPlayer caller, string[] command)
        {
            var user = (UnturnedPlayer)caller;
            Player victim = RaycastHelper.GetPlayerFromHits(user.Player, 8f);
            if (victim != null)
            {
                if (victim.animator.gesture == EPlayerGesture.SURRENDER_START || victim.animator.gesture == EPlayerGesture.SURRENDER_STOP)
                {
                    List<ItemJar> items = new List<ItemJar>();
                    UpdateList(victim, items);
                    vinventory.resize(7, 9);
                    foreach (ItemJar item in items)
                    {
                        vinventory.tryAddItem(item.item);
                    }
                    user.Player.inventory.updateItems(7, vinventory);
                    user.Player.inventory.sendStorage();

                    victim.inventory.onInventoryRemoved = (page, index, jar) =>
                    {
                        user.Player.inventory.closeDistantStorage();
                        user.Player.inventory.closeStorage();
                        user.Player.inventory.closeStorageAndNotifyClient();
                        UnturnedChat.Say(user, "The victim try to remove a item from it inventory!");
                    };
                    vinventory.onItemRemoved += (page, index, jar) => OnItemRemoved(victim, page, index, jar);
                    vinventory.onItemAdded += (page, index, jar) =>
                    {
                        victim.inventory.tryAddItem(jar.item, true);
                    };

                }
                else
                {
                    ChatManager.say(user.CSteamID, "The player need to have the arms up!", UnityEngine.Color.red, true);
                }
            }
            else
            {
                ChatManager.say(user.CSteamID, "You need to see a player to use this command!", UnityEngine.Color.red, true);
            }
        }

        //private void AvoidDuplications(Items vinventory, byte page, byte index, ItemJar jar)
        //{
        //    byte ic = vinventory.getItemCount();
        //    if (ic > 0)
        //    {
        //        for (byte p1 = 0; p1 < ic; p1++)
        //        {
        //            var s = vinventory.getItem(p1);
        //            if (s != null && s.item == jar.item) vinventory.removeItem(p1);
        //        }
        //    }
        //}

        //private void OnItemAdded(Player victim, byte page, byte index, ItemJar jar)
        //{
        //    victim.inventory.tryAddItem(jar.item, true);
        //}

        private void OnItemRemoved(Player victim, byte page, byte index, ItemJar jar)
        {
            for (byte pag = 0; pag < 7; pag++)
            {
                byte ic = victim.inventory.getItemCount(pag);
                if (ic > 0)
                {
                    for (byte p1 = 0; p1 < ic; p1++)
                    {
                        var s = victim.inventory.getItem(pag, p1);
                        if (s != null && s.item == jar.item) victim.inventory.removeItem(pag, p1);
                    }
                }
            }
        }

        private void UpdateList(Player player, List<ItemJar> list)
        {
            for (byte page = 0; page < 7; page++)
            {
                byte ic = player.inventory.getItemCount(page);
                if (ic > 0)
                {
                    for (byte p1 = 0; p1 < ic; p1++)
                    {
                        list.Add(player.inventory.getItem(page, p1));
                    }
                }
            }
        }
    }
}
