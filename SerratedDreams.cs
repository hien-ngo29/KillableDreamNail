using Modding;
using SFCore;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using UnityEngine;
using UObject = UnityEngine.Object;

namespace SerratedDreams
{
    internal class SerratedDreams : Mod, ILocalSettings<Settings>
    {
        public EasyCharm serratedDreamCharm;

        public override string GetVersion()
        {
            return Assembly.GetExecutingAssembly().GetName().Version.ToString();
        }

        public override void Initialize()
        {
            Log("Initializing");

            serratedDreamCharm = new SerratedDreamCharm();

            ModHooks.AfterSavegameLoadHook += OnGameLoaded;
            On.EnemyDreamnailReaction.RecieveDreamImpact += OnEnemyDreamnailed;

            Log("Initialized");
        }

        private void OnGameLoaded(SaveGameData data) 
        {
            if (!serratedDreamCharm.GotCharm)
                serratedDreamCharm.GiveCharm(true);
        }

        public void OnLoadLocal(Settings settings)
        {
            if (settings.serratedDreamCharmState != null)
            {
                serratedDreamCharm.RestoreCharmState(settings.serratedDreamCharmState);
            }
        }

        private void OnEnemyDreamnailed(On.EnemyDreamnailReaction.orig_RecieveDreamImpact orig, EnemyDreamnailReaction self)
        {
            orig(self);

            if (PlayerData.instance.GetBool($"equippedCharm_{serratedDreamCharm.Id}"))
            {
                HitInstance hitInstance = new()
                {
                    DamageDealt = PlayerData.instance.dreamNailUpgraded ? 120 : 60,
                    Multiplier = 1,
                    AttackType = AttackTypes.Nail,
                    Direction = HeroController.instance.transform.localScale.x,
                    CircleDirection = false,
                    Source = self.gameObject
                };

                var enemyHp = self.gameObject.GetComponent<HealthManager>();
                enemyHp.Hit(hitInstance);
            }
        }

        public Settings OnSaveLocal()
        {
            Log("Charm saved!");
            Settings settings = new();
            settings.serratedDreamCharmState = serratedDreamCharm.GetCharmState();
            return settings;
        }
    }
}