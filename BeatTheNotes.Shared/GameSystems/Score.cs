using Microsoft.Xna.Framework;

namespace BeatTheNotes.GameSystems
{
    public class Score
    {
        public int Position { get; }
        public int MsBeforeExpire { get; private set; }

        public bool IsExpired => MsBeforeExpire <= 0;

        public Score(int position, int msBeforeExpire)
        {
            Position = position;
            MsBeforeExpire = msBeforeExpire;
        }

        public void Update(GameTime gameTime)
        {
            MsBeforeExpire -= gameTime.ElapsedGameTime.Milliseconds;
        }
    }
}
