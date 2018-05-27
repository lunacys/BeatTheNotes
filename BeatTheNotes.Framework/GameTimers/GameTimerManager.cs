using System.Collections.Generic;
using Microsoft.Xna.Framework;

namespace BeatTheNotes.Framework.GameTimers
{
    /// <summary>
    /// <see cref="GameTimer"/> manager
    /// </summary>
    public static class GameTimerManager
    {
        private static readonly List<GameTimer> GameTimerList = new List<GameTimer>();

        /// <summary>
        /// Add a <see cref="GameTimer"/>
        /// </summary>
        /// <param name="gameTimer"><see cref="GameTimer"/> to add</param>
        public static void Add(GameTimer gameTimer)
        {
            GameTimerList.Add(gameTimer);
        }

        /// <summary>
        /// Get <see cref="GameTimer"/> by index
        /// </summary>
        /// <param name="index">Index of <see cref="GameTimer"/></param>
        /// <returns><see cref="GameTimer"/></returns>
        public static GameTimer Get(int index)
        {
            return GameTimerList[index];
        }

        /// <summary>
        /// Stop all timers
        /// </summary>
        public static void StopAll()
        {
            foreach (var gameTimer in GameTimerList)
            {
                gameTimer.Stop();
            }
        }

        /// <summary>
        /// Start all timers
        /// </summary>
        public static void StartAll()
        {
            foreach (var gameTimer in GameTimerList)
            {
                gameTimer.Start();
            }
        }

        /// <summary>
        /// Clear the timer collection
        /// </summary>
        public static void Clear()
        {
            GameTimerList.Clear();
        }

        /// <summary>
        /// Update the state of all the timers
        /// </summary>
        /// <param name="gameTime"><see cref="GameTime"/></param>
        public static void Update(GameTime gameTime)
        {
            foreach (var gameTimer in GameTimerList)
            {
                gameTimer.Update(gameTime);
            }
        }
    }
}