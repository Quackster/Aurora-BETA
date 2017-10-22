﻿using System.Collections.Generic;
using System.Linq;
using AuroraEmu.DI.Database.DAO;
using AuroraEmu.DI.Game.Achievements;
using AuroraEmu.Game.Achievements.Models;
using AuroraEmu.Game.Clients;
using AuroraEmu.Network.Game.Packets.Composers.Achievements;
using AuroraEmu.Network.Game.Packets.Events.Achievements;

namespace AuroraEmu.Game.Achievements
{
    public class AchievementController : IAchievementController
    {
        public IAchievementsDao Dao { get; }
        public Dictionary<string, Achievement> Achievements { get; }

        public AchievementController(IAchievementsDao dao)
        {
            Dao = dao;
            Achievements = dao.GetAchievements();
            
            Engine.Logger.Info($"Loaded {Achievements.Count} Achievements.");
        }
        
        public void CheckAchievement(Client client, string achievementCode, int current)
        {
            if (!Achievements.TryGetValue(achievementCode, out Achievement achievement)) return;

            int checkLevel;
            bool hasAchievementBase;

            if (!client.Achievements.TryGetValue(achievement.Id, out int currentLevel))
            {
                checkLevel = 1;
                hasAchievementBase = false;
            }
            else
            {
                if (currentLevel >= achievement.Levels.Keys.Max())
                {
                    return; // No more levels...
                }
                
                checkLevel = currentLevel + 1;
                hasAchievementBase = true;
            }

            if (current < achievement.Levels[checkLevel].Required) return;

            Dao.AddOrUpdateUserAchievement(client.Player.Id, achievement.Id, checkLevel);

            if (hasAchievementBase)
            {
                client.Achievements[achievement.Id] = checkLevel;
            }
            else
            {
                client.Achievements.Add(achievement.Id, checkLevel);
            }

            client.Player.BadgesComponent.AddOrUpdateBadge(achievement.Badge, checkLevel);
            
            // Unlocked!
            client.SendComposer(new HabboAchievementNotificationMessageComposer(checkLevel, achievement));
            client.IncreasePixels(achievement.Levels[checkLevel].Pixels);
        }
    }
}