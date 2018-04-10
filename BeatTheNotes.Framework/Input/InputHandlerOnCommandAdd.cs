using System;
using Microsoft.Xna.Framework.Input;

namespace BeatTheNotes.Framework.Input
{
    public class InputHandlerOnCommandAdd : EventArgs
    {
        public InputHandler InputHandler { get; }
        public Keys Key { get; }
        public IInputCommand InputCommand { get; }

        public InputHandlerOnCommandAdd(InputHandler inputHandler, Keys key, IInputCommand inputCommand)
        {
            InputHandler = inputHandler;
            Key = key;
            InputCommand = inputCommand;
        }
    }
}