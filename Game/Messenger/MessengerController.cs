﻿using AuroraEmu.DI.Game.Messenger;
using AuroraEmu.Game.Messenger.Models;
using AuroraEmu.Game.Players.Models;
using System.Collections.Generic;

namespace AuroraEmu.Game.Messenger
{
    public class MessengerController : IMessengerController
    {
        public void MessengerSearch(string searchString, Player player, List<MessengerSearch> friends, List<MessengerSearch> notFriends)
        {
            foreach (MessengerSearch searchResult in SearchForUsers(searchString))
            {
                if (player.MessengerComponent.IsFriends(searchResult.Id))
                {
                    friends.Add(searchResult);
                }
                else
                {
                    notFriends.Add(searchResult);
                }
            }
        }

        public List<MessengerSearch> SearchForUsers(string searchString) =>
            Engine.MainDI.MessengerDao.SearchForUsers(searchString);
    }
}