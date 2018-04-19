using Microsoft.Xna.Framework;

namespace BeatTheNotes.GameSystems
{
    public class ScoremeterScore
    {
        public long Position { get; }
        public int MsBeforeExpire { get; private set; }
        public string HitName { get; }

        public bool IsExpired => MsBeforeExpire <= 0;

        public ScoremeterScore(long position, int msBeforeExpire, string hit)
        {
            Position = position;
            MsBeforeExpire = msBeforeExpire;
            HitName = hit;
        }

        public void Update(GameTime gameTime)
        {
            MsBeforeExpire -= gameTime.ElapsedGameTime.Milliseconds;
        }
    }
}
