using System;
using BeatTheNotes.Framework.Entities;
using Microsoft.Xna.Framework.Input;

namespace BeatTheNotes.Framework.Input
{
    /// <summary>
    /// Event args for on command add event
    /// </summary>
    public class InputHandlerOnCommandAdd : EventArgs
    {
        public InputHandler InputHandler { get; }
        public Keys? Key { get; }
        public MouseButton? MouseButton { get; }
        public Action InputCommand { get; }

        public InputHandlerOnCommandAdd(InputHandler inputHandler, Keys? key, MouseButton? mouseButton, Action inputCommand)
        {
            InputHandler = inputHandler;
            Key = key;
            MouseButton = mouseButton;
            InputCommand = inputCommand;
        }
    }
}