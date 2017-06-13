﻿using AuroraEmu.Game.Clients;
using AuroraEmu.Game.Rooms;

namespace AuroraEmu.Network.Game.Packets.Composers.Rooms
{
    public class RoomForwardMessageComposer : MessageComposer
    {
        public RoomForwardMessageComposer(Room room, Client client)
            : base(286)
        {
            AppendVL64(true);
            AppendVL64(client.CurrentRoom.Id);
        }
    }
}
