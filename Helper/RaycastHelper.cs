﻿using SDG.Unturned;
using UnityEngine;
using SDG.Framework.Landscapes;
using System.Linq;

namespace AdvancedThief
{
    public class RaycastHelper
    {
        public static Player GetPlayerFromHits(Player caller, float maxDistance)
        {
            var hits = Physics.RaycastAll(new Ray(caller.look.aim.position, caller.look.aim.forward), maxDistance, RayMasks.PLAYER_INTERACT | RayMasks.PLAYER);
            Player player = null;
            for (int i = 0; i < hits.Length; i++)
            {
                Player suspect = hits[i].transform.GetComponentInParent<Player>();
                if (suspect != caller)
                {
                    player = suspect;
                    break;
                }
            }
            return player;
        }
    }
}
