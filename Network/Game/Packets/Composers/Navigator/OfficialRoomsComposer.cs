﻿using System.Collections.Generic;
using AuroraEmu.Game.Navigator.Models;

namespace AuroraEmu.Network.Game.Packets.Composers.Navigator
{
    class OfficialRoomsComposer : MessageComposer
    {
        public OfficialRoomsComposer(IDictionary<int, FrontpageItem> frontpageItems)
            : base(450)
        {
            AppendVL64(0);
            AppendVL64(frontpageItems.Count);

            foreach (FrontpageItem item in frontpageItems.Values)
            {
                AppendString(item.Name);
                AppendString(item.Description);
                AppendVL64(item.Size);
                AppendString(item.Name);
                AppendString(item.Image);
                AppendVL64(0);
                AppendVL64(item.Type);

                switch (item.Type)
                {
                    case 1:
                        AppendString(item.Tag);
                        break;
                    case 2:
                        
                        break;
                    case 3:
                        var room = Engine.Locator.RoomController.GetRoom(item.RoomId);
                        
                        AppendString("");
                        AppendVL64(room.Id);
                        AppendVL64(0);
                        AppendString(room.CCTs);
                        AppendVL64(room.PlayersMax);
                        AppendVL64(item.RoomId);
                        break;
                }
            }
        }
    }
}