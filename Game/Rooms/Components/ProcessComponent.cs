﻿using AuroraEmu.Game.Rooms.Pathfinder;
using AuroraEmu.Network.Game.Packets.Composers.Users;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks.Dataflow;

namespace AuroraEmu.Game.Rooms.Components
{
    public class ProcessComponent : IDisposable
    {
        private Room room;
        private CancellationTokenSource _wtoken;
        private ActionBlock<DateTimeOffset> _task;

        public ProcessComponent(Room room)
        {
            this.room = room;
        }

        public void SetupRoomLoop()
        {
            _wtoken = new CancellationTokenSource();

            _task = Engine.MainDI.TaskController.ExecutePeriodic(now => Loop(), _wtoken.Token, 500);
            _task.Post(DateTimeOffset.Now);
        }

        private void Loop()
        {
            room.Loop();

            List<RoomActor> toUpdate = new List<RoomActor>();
            foreach (RoomActor actor in room.Actors.Values)
            {
                #region walking related

                if (actor.SetStep)
                {
                    actor.Position = actor.NextTile;

                    actor.UpdateNeeded = true;

                    actor.SetStep = false;
                }

                if (actor.CalcPath)
                {
                    if (actor.IsWalking)
                        actor.Path.Clear();

                    Grid grid = new Grid(room.Map, room.Map.MapSize.Item1, room.Map.MapSize.Item2);
                    actor.Path = grid.GetPath(actor.Position, actor.TargetPoint, MovementPatterns.Full);

                    if (actor.IsWalking)
                    {
                        actor.StepsOnPath = 1;
                        actor.CalcPath = false;
                    }
                    else
                    {
                        actor.CalcPath = false;
                        actor.Path.Clear();
                    }
                }

                if (actor.IsWalking)
                {
                    if ((actor.StepsOnPath >= actor.Path.Count))
                    {
                        actor.Path.Clear();
                        actor.CalcPath = false;
                        actor.Statusses.Remove("mv");
                    }
                    else
                    {
                        Point2D nextStep = actor.Path[(actor.Path.Count - actor.StepsOnPath) - 1];
                        actor.StepsOnPath++;
                        actor.Statusses.Remove("mv");

                        actor.Statusses.Add("mv", $"{nextStep.X},{nextStep.Y},0.00");
                        actor.SetStep = true;
                        actor.Rotation = PathFinder.CalculateRotation(actor.Position.X, actor.Position.Y, nextStep.X,
                            nextStep.Y);
                        actor.NextTile = nextStep;
                        actor.UpdateNeeded = true;
                    }
                }
                else
                {
                    if (actor.Statusses.ContainsKey("mv"))
                    {
                        actor.Statusses.Remove("mv");
                        actor.UpdateNeeded = true;
                    }
                }

                #endregion

                if (!actor.UpdateNeeded || toUpdate.Contains(actor))
                    continue;

                actor.UpdateNeeded = false;
                toUpdate.Add(actor);
            }
            if (toUpdate.Count > 0)
                room.SendComposer(new UserUpdateMessageComposer(toUpdate));
        }

        public void Dispose()
        {
            _wtoken.Cancel();
            _task = null;
            _wtoken = null;
        }
    }
}