using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace Jackhammer
{
    public enum MouseButton
    {
        Left,
        Right,
        Middle
    }

    public static class InputManager
    {
        public static Vector2 MousePosition { get; private set; }
        public static Vector2 MouseVelocity { get; private set; }
        public static Rectangle MouseRect
            => new Rectangle((int)MousePosition.X, (int)MousePosition.Y, 1, 1);

        private static KeyboardState _keyboardState, _oldKeyboardState;
        private static MouseState _mouseState, _oldMouseState;
        private static Vector2 _oldMousePosition;

        public static void Update(Game game)
        {
            _oldKeyboardState = _keyboardState;
            _oldMouseState = _mouseState;
            _oldMousePosition = _oldMouseState.Position.ToVector2();


            _keyboardState = Keyboard.GetState();
            _mouseState = Mouse.GetState(game.Window);
            MousePosition = _mouseState.Position.ToVector2();

            MouseVelocity = MousePosition - _oldMousePosition;
        }

        public static bool WasKeyPressed(Keys key)
        {
            return _oldKeyboardState.IsKeyUp(key)
                   && _keyboardState.IsKeyDown(key);
        }

        public static bool WasKeyReleased(Keys key)
        {
            return _oldKeyboardState.IsKeyDown(key)
                   && _keyboardState.IsKeyUp(key);
        }
        public static bool IsKeyDown(Keys key)
        {
            return _keyboardState.IsKeyDown(key);
        }

        public static bool WasMouseButtonPressed(MouseButton button)
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

        public static bool WasMouseButtonReleased(MouseButton button)
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

        public static bool IsMouseButtonDown(MouseButton button)
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

        public static bool IsMouseButtonUp(MouseButton button)
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
