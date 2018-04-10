using System;
using System.Collections.Generic;
using BeatTheNotes.Framework.Settings;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace BeatTheNotes.Framework.Input
{
    public class InputHandler : GameComponent
    {
        public Vector2 MousePosition { get; private set; }
        public Vector2 MouseVelocity { get; private set; }
        public Rectangle MouseRect
            => new Rectangle((int)MousePosition.X, (int)MousePosition.Y, 1, 1);

        private KeyboardState _keyboardState, _oldKeyboardState;
        private MouseState _mouseState, _oldMouseState;
        private Vector2 _oldMousePosition;

        private readonly GameWindow Window;

        private readonly IInputCommand _nullCommand;

        // input commands for the keys from 0 to 9 (0 is 1k mode, 1 is 2k and so on)
        private readonly Dictionary<Keys, IInputCommand> _inputCommands;

        public event EventHandler<InputHandlerOnCommandAdd> OnCommandAdded; 

        public InputHandler(Game game) : base(game)
        {
            _inputCommands = new Dictionary<Keys, IInputCommand>();

            _nullCommand = new InputNullCommand();

            Window = game.Window;
        }

        public IInputCommand this[Keys key]
        {
            get => _inputCommands[key];
            set => RegisterKeyCommand(key, value);
        }

        public void RegisterKeyCommand(Keys key, IInputCommand command)
        {
            if (_inputCommands.ContainsValue(command))
                throw new InputCommandAlreadyRegisteredException(command);

            _inputCommands[key] = command;

            OnCommandAdded?.Invoke(this, new InputHandlerOnCommandAdd(this, key, command));
        }

        /// <summary>
        /// Handle all the input provided by IInputCommand interface. Only works with keyboard.
        /// This method returns a IEnumerable collection in order to process simultaneous pressed keys.
        /// </summary>
        /// <param name="keyboardHandler">Functor, if null, set WasKeyPressed method as the handler</param>
        /// <returns>Command interface Enumerable</returns>
        public IEnumerable<IInputCommand> HandleInput(Func<Keys, bool> keyboardHandler)
        {
            if (keyboardHandler == null)
                keyboardHandler = WasKeyPressed;

            foreach (var inputCommand in _inputCommands)
                if (keyboardHandler(inputCommand.Key))
                    yield return inputCommand.Value;

            yield return _nullCommand;
        }

        public override void Update(GameTime gameTime)
        {
            _oldKeyboardState = _keyboardState;
            _oldMouseState = _mouseState;
            _oldMousePosition = _oldMouseState.Position.ToVector2();

            _keyboardState = Keyboard.GetState();
            _mouseState = Mouse.GetState(Window);
            MousePosition = _mouseState.Position.ToVector2();

            MouseVelocity = MousePosition - _oldMousePosition;
        }

        public bool WasKeyPressed(Keys key)
        {
            return _oldKeyboardState.IsKeyUp(key)
                   && _keyboardState.IsKeyDown(key);
        }

        public bool WasKeyReleased(Keys key)
        {
            return _oldKeyboardState.IsKeyDown(key)
                   && _keyboardState.IsKeyUp(key);
        }
        public bool IsKeyDown(Keys key)
        {
            return _keyboardState.IsKeyDown(key);
        }

        public bool IsKeyUp(Keys key)
        {
            return _keyboardState.IsKeyUp(key);
        }

        public bool WasMouseButtonPressed(MouseButton button)
        {
            switch (button)
            {
                case MouseButton.Left:
                    return _oldMouseState.LeftButton == ButtonState.Released
                           && _mouseState.LeftButton == ButtonState.Pressed;
                case MouseButton.Right:
                    return _oldMouseState.RightButton == ButtonState.Released
                           && _mouseState.RightButton == ButtonState.Pressed;
                case MouseButton.Middle:
                    return _oldMouseState.MiddleButton == ButtonState.Released
                           && _mouseState.MiddleButton == ButtonState.Pressed;
            }
            return false;
        }

        public bool WasMouseButtonReleased(MouseButton button)
        {
            switch (button)
            {
                case MouseButton.Left:
                    return _oldMouseState.LeftButton == ButtonState.Pressed
                           && _mouseState.LeftButton == ButtonState.Released;
                case MouseButton.Right:
                    return _oldMouseState.RightButton == ButtonState.Pressed
                           && _mouseState.RightButton == ButtonState.Released;
                case MouseButton.Middle:
                    return _oldMouseState.MiddleButton == ButtonState.Pressed
                           && _mouseState.MiddleButton == ButtonState.Released;
            }
            return false;
        }

        public bool IsMouseButtonDown(MouseButton button)
        {
            switch (button)
            {
                case MouseButton.Left:
                    return _mouseState.LeftButton == ButtonState.Pressed;
                case MouseButton.Right:
                    return _mouseState.RightButton == ButtonState.Pressed;
                case MouseButton.Middle:
                    return _mouseState.MiddleButton == ButtonState.Pressed;
            }
            return false;
        }

        public bool IsMouseButtonUp(MouseButton button)
        {
            switch (button)
            {
                case MouseButton.Left:
                    return _mouseState.LeftButton == ButtonState.Released;
                case MouseButton.Right:
                    return _mouseState.RightButton == ButtonState.Released;
                case MouseButton.Middle:
                    return _mouseState.MiddleButton == ButtonState.Released;
            }
            return false;
        }
    }
}