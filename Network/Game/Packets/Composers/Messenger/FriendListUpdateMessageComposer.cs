﻿using AuroraEmu.Game.Clients;
using AuroraEmu.Game.Messenger;
using System.Collections.Generic;

namespace AuroraEmu.Network.Game.Packets.Composers.Messenger
{
    public class FriendListUpdateMessageComposer : MessageComposer
    {
        public FriendListUpdateMessageComposer(int updateCount, Dictionary<int, MessengerFriends> updatedFriends)
            : base(13)
        {
            AppendVL64(0);
            AppendVL64(updateCount);
            AppendVL64(0);

            foreach (MessengerFriends friend in updatedFriends.Values)
            {
                AppendVL64(friend.UserTwoId);
                AppendString(friend.Username);
                AppendVL64(1);
                AppendVL64(ClientManager.GetInstance().PlayerIsOnline(friend.UserTwoId));
                AppendVL64(true);
                AppendString(friend.Figure);
                AppendVL64(0);
                AppendString(friend.Motto);
                AppendString("");

                AppendVL64(false);
            }
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
