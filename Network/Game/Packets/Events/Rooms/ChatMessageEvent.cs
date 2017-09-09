﻿using AuroraEmu.Game.Clients;
using AuroraEmu.Network.Game.Packets.Composers.Rooms;

namespace AuroraEmu.Network.Game.Packets.Events.Rooms
{
    class ChatMessageEvent : IPacketEvent
    {
        public void Run(Client client, MessageEvent msgEvent)
        {
            if (client.CurrentRoomId < 1)
                return;
            
            string input = msgEvent.ReadString();

            if (input.StartsWith(":") && Engine.MainDI.CommandController.TryHandleCommand(client, input.Substring(1)))
                return;

            client.CurrentRoom.SendComposer(new ChatMessageComposer(client.UserActor.VirtualId, Engine.MainDI.WorldfilterController.CheckString(input)));
        }
    }
}
