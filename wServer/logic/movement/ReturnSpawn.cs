﻿#region

using System;
using System.Collections.Generic;
using Mono.Game;
using wServer.realm;
using wServer.realm.entities;

#endregion

namespace wServer.logic.movement
{
    internal class ReturnSpawn : Behavior
    {
        private static readonly Dictionary<float, ReturnSpawn> instances = new Dictionary<float, ReturnSpawn>();
        private readonly float speed;
        private Random rand = new Random();

        private ReturnSpawn(float speed)
        {
            this.speed = speed;
        }

        public static ReturnSpawn Instance(float speed)
        {
            ReturnSpawn ret;
            if (!instances.TryGetValue(speed, out ret))
                ret = instances[speed] = new ReturnSpawn(speed);
            return ret;
        }

        protected override bool TickCore(RealmTime time)
        {
            if (Host.Self.HasConditionEffect(ConditionEffects.Paralyzed)) return true;
            float speed = this.speed*GetSpeedMultiplier(Host.Self);

            Position pos = (Host as Enemy).SpawnPoint;
            float tx = pos.X;
            float ty = pos.Y;
            if (Math.Abs(tx - Host.Self.X) > 1 || Math.Abs(ty - Host.Self.Y) > 1)
            {
                float x = Host.Self.X;
                float y = Host.Self.Y;
                Vector2 vect = new Vector2(tx, ty) - new Vector2(Host.Self.X, Host.Self.Y);
                vect.Normalize();
                vect *= (speed/1.5f)*(time.thisTickTimes/1000f);
                ValidateAndMove(Host.Self.X + vect.X, Host.Self.Y + vect.Y);
                Host.Self.UpdateCount++;
                return true;
            }
            return false;
        }
    }
}