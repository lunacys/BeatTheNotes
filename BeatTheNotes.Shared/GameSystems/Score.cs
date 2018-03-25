using Microsoft.Xna.Framework;

namespace BeatTheNotes.GameSystems
{
    public class Score
    {
        public long Position { get; }
        public int MsBeforeExpire { get; private set; }
        public string HitName { get; }

        public bool IsExpired => MsBeforeExpire <= 0;

        public Score(long position, int msBeforeExpire, string hit)
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
