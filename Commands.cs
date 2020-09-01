using System;
using Microsoft.Extensions.Logging;
using Cysharp.Threading.Tasks;
using OpenMod.Unturned.Users;
using OpenMod.Core.Commands;
using System.Threading.Tasks;
using SDG.Unturned;
using Command = OpenMod.Core.Commands.Command;
using System.Collections.Generic;
using OpenMod.API.Commands;

namespace AdvancedThief
{
    [Command("rob")]
    [CommandDescription("Rob a player inventory!")]
    [CommandActor(typeof(UnturnedUser))]
    public class CommandRob : Command
    {

        public CommandRob(IServiceProvider serviceProvider) : base(serviceProvider)
        {
        }

        protected async override Task OnExecuteAsync()
        {
            await UniTask.SwitchToMainThread();
            var user = (UnturnedUser)Context.Actor;
            Player victim = RaycastHelper.GetPlayerFromHits(user.Player, 8f);
            if (victim != null)
            {
                if (victim.animator.gesture == EPlayerGesture.SURRENDER_START || victim.animator.gesture == EPlayerGesture.SURRENDER_STOP)
                {
                    Items vinventory = new Items(7);
                    List<ItemJar> items = new List<ItemJar>();
                    UpdateList(victim, items);
                    vinventory.resize(7, 9);
                    foreach (ItemJar item in items)
                    {
                        vinventory.tryAddItem(item.item);
                    }
                    user.Player.inventory.updateItems(7, vinventory);
                    user.Player.inventory.sendStorage();

                    victim.inventory.onInventoryRemoved += (page, index, jar) => AvoidDuplications(vinventory, page, index, jar);
                    vinventory.onItemRemoved += (page, index, jar) => OnItemRemoved(victim, page, index, jar);
                    vinventory.onItemAdded += (page, index, jar) => OnItemAdded(victim, page, index, jar);
                }
                else
                {
                    throw new UserFriendlyException("The player need to have the arms up!");
                }
            }
            else 
            {
                await user.PrintMessageAsync("Victim not found!");
                throw new UserFriendlyException("You need to see a player to use this command!");
            }
        }

        private void AvoidDuplications(Items vinventory, byte page, byte index, ItemJar jar)
        {
            byte ic = vinventory.getItemCount();
            if (ic > 0)
            {
                for (byte p1 = 0; p1 < ic; p1++)
                {
                    var s = vinventory.getItem(p1);
                    if (s != null && s.item == jar.item) vinventory.removeItem(p1);
                }
            }
        }

        private void OnItemAdded(Player victim, byte page, byte index, ItemJar jar)
        {
            victim.inventory.tryAddItem(jar.item, true);
        }

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
