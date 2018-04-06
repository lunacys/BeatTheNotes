using System;
using System.Collections.Generic;
using System.Text;
using BeatTheNotes.Framework.GameSystems;
using BeatTheNotes.GameSystems;
using Microsoft.Xna.Framework;

namespace BeatTheNotes.Shared.GameSystems
{
    public class HealthSystem : GameSystem
    {
        public float MaxHealth => 100.0f;
        public float MinHealth => 0.0f;

        public float HpDrainRate { get; set; }
        public float Health { get; set; }

        public bool IsLose => Health <= MinHealth;

        public HealthSystem(float hpDrainRate)
        {
            HpDrainRate = hpDrainRate;

            Health = MaxHealth;
        }

        public override void Initialize()
        {
            base.Initialize();

            var scoreSys = FindSystem<ScoreSystem>();
            scoreSys.OnScoreGet += OnScoreGet;
        }
        
        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);

            Health -= 0.25f * HpDrainRate * gameTime.ElapsedGameTime.Milliseconds / 100f;
            Health = MathHelper.Clamp(Health, MinHealth, MaxHealth);
        }

        public override void Draw(GameTime gameTime)
        {
            base.Draw(gameTime);
        }

        public override void Reset()
        {
            base.Reset();

            Health = MaxHealth;
        }

        private void OnScoreGet(object sender, OnScoreGetEventHandler eh)
        {
            var scoreSys = FindSystem<ScoreSystem>();

            if (eh.HitValueName == scoreSys.ScoreMarvelous)
                Health += 2 * HpDrainRate;
            else if (eh.HitValueName == scoreSys.ScorePerfect)
                Health += 2 * HpDrainRate;
            else if (eh.HitValueName == scoreSys.ScoreGreat)
                Health += 0.0f;
            else if (eh.HitValueName == scoreSys.ScoreGood)
                Health -= 0.5f * HpDrainRate;
            else if (eh.HitValueName == scoreSys.ScoreBad)
                Health -= 1 * HpDrainRate;
            else if (eh.HitValueName == scoreSys.ScoreMiss)
                Health -= 2 * HpDrainRate;
        }
    }
}
