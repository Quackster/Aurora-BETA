﻿using System.Collections.Concurrent;
using System.Collections.Generic;
using AuroraEmu.Game.Clients;
using AuroraEmu.Game.Catalog.Models;
using AuroraEmu.Game.Items.Models;
using AuroraEmu.Game.Items.Models.Dimmer;
using AuroraEmu.Game.Players.Models;

namespace AuroraEmu.DI.Database.DAO
{
    public interface IItemDao
    {
        void ReloadTemplates(Dictionary<int, ItemDefinition> items);

        void GiveItem(Client client, CatalogProduct product, string extraData);
        
        void GiveItem(Client client, ItemDefinition template, string extraData);
        
        void GiveItem(Player targetUser, CatalogProduct product, string extraData);
        
        int GiveItem(Player targetUser, ItemDefinition template, string extraData);
        
        ConcurrentDictionary<int, Item> GetItemsInRoom(int roomId);

        Dictionary<int, Item> GetItemsFromOwner(int ownerId);

        void UpdateItem(int itemId, int x, int y, int rot, object roomId);

        void DeleteItem(int itemId);

        void AddFloorItem(int itemId, int x, int y, int rot, int roomId);

        void AddWallItem(int itemId, string wallposition, int roomId);

        void UpdateItemData(int itemId, string data);

        void UpdateDimmerPreset(DimmerData data);

        void CreatePresent(int definitionId, int playerId, int giftId, string data);
        
        (int, string) GetPresent(int presentId, int playerId);
        void DeletePresent(int presentId);
    }
}