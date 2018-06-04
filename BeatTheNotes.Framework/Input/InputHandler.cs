using System;
using System.Collections.Generic;
using BeatTheNotes.Framework.Entities;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using MonoGame.Extended;

namespace BeatTheNotes.Framework.Input
{
    public class InputHandler : GameComponent
    {
        public Vector2 MousePosition { get; private set; }
        public Vector2 MouseVelocity { get; private set; }
        public RectangleF MouseRectangle
            => new RectangleF(MousePosition, new Size2(1, 1));

        public Func<Keys, bool> KeyboardHandler { get; set; }
        public Func<MouseButton, bool> MouseHandler { get; set; }

        private KeyboardState _keyboardState, _oldKeyboardState;
        private MouseState _mouseState, _oldMouseState;
        private Vector2 _oldMousePosition;

        private readonly GameWindow _window;

        private readonly Dictionary<Keys, Action> _inputKeyCommands;
        private readonly Dictionary<MouseButton, Action> _inputMouseButtonCommands;

        public event EventHandler<InputHandlerOnCommandAdd> OnCommandAdded;

        public InputHandler(Game game)
            : base(game)
        {
            _inputKeyCommands = new Dictionary<Keys, Action>();
            _inputMouseButtonCommands = new Dictionary<MouseButton, Action>();

            KeyboardHandler = WasKeyPressed;
            MouseHandler = WasMouseButtonPressed;

            _window = game.Window;
        }

        public Action this[Keys key]
        {
            get => _inputKeyCommands[key];
            set => RegisterKeyCommand(key, value);
        }

        public Action this[MouseButton mouseButton]
        {
            get => _inputMouseButtonCommands[mouseButton];
            set => RegisterMouseButtonCommand(mouseButton, value);
        }

        public void RegisterKeyCommand(Keys key, Action command)
        {
            if (_inputKeyCommands.ContainsValue(command))
                throw new InputCommandAlreadyRegisteredException(command);

            _inputKeyCommands[key] = command;

            OnCommandAdded?.Invoke(this, new InputHandlerOnCommandAdd(this, key, null, command));
        }

        public void RegisterMouseButtonCommand(MouseButton mouseButton, Action command)
        {
            if (_inputMouseButtonCommands.ContainsValue(command))
                throw new InputCommandAlreadyRegisteredException(command);

            _inputMouseButtonCommands[mouseButton] = command;

            OnCommandAdded?.Invoke(this, new InputHandlerOnCommandAdd(this, null, mouseButton, command));
        }

        /// <summary>
        /// Handle all the input provided by IInputCommand interface. Works with both keyboard and mouse.
        /// This method returns a IEnumerable collection in order to process simultaneous pressed keys.
        /// </summary>
        /// <param name="keyboardHandler">Functor for keyboard handling process, if null, set WasKeyPressed method as the handler</param>
        /// <param name="mouseHandler">Functor for mouse handling process, if null set WasMouseButtonPressed as the handler</param>
        /// <returns>Command interface Enumerable</returns>
        public IEnumerable<Action> HandleInput(Func<Keys, bool> keyboardHandler = null, Func<MouseButton, bool> mouseHandler = null)
        {
            if (keyboardHandler == null)
                keyboardHandler = WasKeyPressed;
            if (mouseHandler == null)
                mouseHandler = WasMouseButtonPressed;

            // Process keyboard input
            foreach (var inputCommand in _inputMouseButtonCommands)
                if (mouseHandler(inputCommand.Key))
                    yield return inputCommand.Value;
            //yield return inputCommand.Value;

            // Process mouse input
            foreach (var inputCommand in _inputKeyCommands)
                if (keyboardHandler(inputCommand.Key))
                    yield return inputCommand.Value;

            // TODO: Process touch input

            yield return () => { }; // do nothing
        }

        /// <summary>
        /// Update both the Keyboard and Mouse states
        /// </summary>
        /// <param name="gameTime"><see cref="GameTime"/></param>
        public override void Update(GameTime gameTime)
        {
            _oldKeyboardState = _keyboardState;
            _oldMouseState = _mouseState;
            _oldMousePosition = _oldMouseState.Position.ToVector2();

            _keyboardState = Keyboard.GetState();
            _mouseState = Mouse.GetState(_window);
            MousePosition = _mouseState.Position.ToVector2();

            MouseVelocity = MousePosition - _oldMousePosition;

            foreach (var inputCommand in _inputKeyCommands)
                if (KeyboardHandler(inputCommand.Key))
                    inputCommand.Value.Invoke();

            foreach (var inputCommand in _inputMouseButtonCommands)
                if (MouseHandler(inputCommand.Key))
                    inputCommand.Value();
        }

        /// <summary>
        /// Gets whether was keyboard key pressed once
        /// </summary>
        /// <param name="key">Keyboard <see cref="Keys"/> to be handled</param>
        /// <returns>true if key was pressed, false otherwise</returns>
        public bool WasKeyPressed(Keys key)
        {
            return _oldKeyboardState.IsKeyUp(key)
                   && _keyboardState.IsKeyDown(key);
        }

        /// <summary>
        /// Gets whether keyboard key was released after being pressed
        /// </summary>
        /// <param name="key">Keyboard <see cref="Keys"/> to be handled</param>
        /// <returns>true if key was released, false otherwise</returns>
        public bool WasKeyReleased(Keys key)
        {
            return _oldKeyboardState.IsKeyDown(key)
                   && _keyboardState.IsKeyUp(key);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public bool IsKeyDown(Keys key)
        {
            return _keyboardState.IsKeyDown(key);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
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
                case MouseButton.X1:
                    return _oldMouseState.XButton1 == ButtonState.Released
                           && _mouseState.XButton1 == ButtonState.Pressed;
                case MouseButton.X2:
                    return _oldMouseState.XButton2 == ButtonState.Released
                           && _mouseState.XButton2 == ButtonState.Pressed;
            }
            return false;
        }

        /// <summary>
        /// Gets whether a mouse button was released after a press
        /// </summary>
        /// <param name="button"><see cref="MouseButton"/></param>
        /// <returns>Whether a mouse button was released after a press</returns>
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
                case MouseButton.X1:
                    return _oldMouseState.XButton1 == ButtonState.Pressed
                           && _mouseState.XButton1 == ButtonState.Released;
                case MouseButton.X2:
                    return _oldMouseState.XButton2 == ButtonState.Pressed
                           && _mouseState.XButton2 == ButtonState.Released;
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
                case MouseButton.X1:
                    return _mouseState.XButton1 == ButtonState.Pressed;
                case MouseButton.X2:
                    return _mouseState.XButton2 == ButtonState.Pressed;
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
                case MouseButton.X1:
                    return _mouseState.XButton1 == ButtonState.Released;
                case MouseButton.X2:
                    return _mouseState.XButton2 == ButtonState.Released;
            }
            return false;
        }
    }
}