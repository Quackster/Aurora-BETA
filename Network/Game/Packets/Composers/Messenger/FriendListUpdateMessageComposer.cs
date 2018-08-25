﻿using System.Collections.Generic;
using AuroraEmu.Game.Clients;
using AuroraEmu.Game.Messenger.Models;
using AuroraEmu.Game.Players.Models;

namespace AuroraEmu.Network.Game.Packets.Composers.Messenger
{
    public class FriendListUpdateMessageComposer : MessageComposer
    {
        public FriendListUpdateMessageComposer(Dictionary<int, MessengerFriend> updatedFriends)
            : base(13)
        {
            AppendVL64(0);
            AppendVL64(updatedFriends.Count);
            AppendVL64(0);

            foreach (MessengerFriend friend in updatedFriends.Values)
            {
                AppendVL64(friend.UserTwoId);
                AppendString(friend.Username);
                AppendVL64(1);
                AppendVL64(Engine.Locator.ClientController.TryGetPlayer(friend.UserTwoId, out Player player));
                AppendVL64(true);
                AppendString(friend.Figure);
                AppendVL64(0);
                AppendString(friend.Motto);
                AppendString("");

                AppendVL64(false);
            }
        }

        public FriendListUpdateMessageComposer(Player player)
            : base(13)
        {
            Client client = Engine.Locator.ClientController.GetClientByHabbo(player.Id);

            AppendVL64(0);
            AppendVL64(1);
            AppendVL64(0);
            AppendVL64(player.Id);
            AppendString(player.Username);
            AppendVL64(1);
            AppendVL64(player.Online);
            AppendVL64(client != null && client.CurrentRoomId != 0);
            AppendString(player.Figure);
            AppendVL64(0);
            AppendString(player.Motto);
            AppendString("");

            AppendVL64(false);
        }

        public FriendListUpdateMessageComposer(int friendId)
            : base(13)
        {
            AppendVL64(0);
            AppendVL64(1);
            AppendVL64(-1);
            AppendVL64(friendId);
        }
    }
}
