using System;
using System.Linq;
using BeatTheNotes.Framework.GameSystems;
using BeatTheNotes.GameSystems;
using Microsoft.Xna.Framework;

namespace BeatTheNotes.Shared.GameSystems
{
    public class GameTimeSystem : GameSystem
    {
        public double Time { get; set; }

        public GameTimeSystem()
        {
            Time = 0;
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);

            Time += gameTime.ElapsedGameTime.TotalMilliseconds * FindSystem<MusicSystem>().PlaybackRate;
        }

        public override void Reset()
        {
            base.Reset();

            var gm = FindSystem<GameplaySystem>();
            var pos = gm.Beatmap.HitObjects[0].Position;
            

            Time = -220;
            
        }
    }
}
