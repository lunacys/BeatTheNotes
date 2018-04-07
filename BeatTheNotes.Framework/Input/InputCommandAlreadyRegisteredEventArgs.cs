using System;

namespace BeatTheNotes.Framework.Input
{
    public class InputCommandAlreadyRegisteredException : Exception
    {
        public IInputCommand InputCommand { get; }

        public InputCommandAlreadyRegisteredException(IInputCommand inputCommand)
        {
            InputCommand = inputCommand;
        }
    }
}
