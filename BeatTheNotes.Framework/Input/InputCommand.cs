using System;
using BeatTheNotes.Framework.Entities;

namespace BeatTheNotes.Framework.Input
{
    public class InputCommand
    {
        /// <summary>
        /// Gets or sets command that should be processed
        /// </summary>
        public Action<Entity> Command { get; set; }
        /// <summary>
        /// Gets or sets <see cref="Entity"/> that should be processed
        /// </summary>
        public Entity Entity { get; set; }
    }
}