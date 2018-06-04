using System;
using BeatTheNotes.Framework.Entities;

namespace BeatTheNotes.Framework.Input
{
    public class InputCommandAlreadyRegisteredException : Exception
    {
        public Action InputCommand { get; }

        public InputCommandAlreadyRegisteredException(Action inputCommand)
        {
            InputCommand = inputCommand;
        }
    }
}
