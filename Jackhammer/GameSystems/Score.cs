using Microsoft.Xna.Framework;

namespace Jackhammer.GameSystems
{
    public class Score
    {
        public int Position { get; set; }
        public int MsBeforeExpire { get; set; }

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
