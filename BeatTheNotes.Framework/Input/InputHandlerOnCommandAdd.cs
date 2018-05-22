using System;
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
        public IInputCommand InputCommand { get; }

        public InputHandlerOnCommandAdd(InputHandler inputHandler, Keys? key, MouseButton? mouseButton, IInputCommand inputCommand)
        {
            InputHandler = inputHandler;
            Key = key;
            MouseButton = mouseButton;
            InputCommand = inputCommand;
        }
    }
}