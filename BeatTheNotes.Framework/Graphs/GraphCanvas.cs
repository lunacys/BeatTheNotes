using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using MonoGame.Extended;

namespace BeatTheNotes.Framework.Graphs
{
    public class GraphCanvas : DrawableGameComponent
    {
        public Vector2 Position { get; set; }
        public Size2 Size { get; set; }
        public Size2 CellSize { get; set; }

        public float MinValue { get; set; }
        public float MaxValue { get; set; }

        public SpriteFont Font { get; set; }

        public bool ShouldDrawBackground { get; set; } = true;
        public bool ShouldDrawBars { get; set; } = true;

        public readonly int MaxNodes = 100;

        public float MinPushedValue => _nodes.Count > 0 ? _nodes.Min(node => node.Value) : 0;
        public float MaxPushedValue => _nodes.Count > 0 ? _nodes.Max(node => node.Value) : 0;

        public GraphNode LastNode => _nodes.Count > 0 ? _nodes.Last() : null;

        //public float X { get; private set; }

        private SpriteBatch _spriteBatch;

        private List<GraphNode> _nodes = new List<GraphNode>();

        private int _xOffset;

        public GraphCanvas(Game game)
            : base(game)
        {
            CellSize = new Size2(10, 20);
        }

        public override void Initialize()
        {
            base.Initialize();

            _spriteBatch = new SpriteBatch(GraphicsDevice);
        }

        public void PushValue(float value)
        {
            var node = new GraphNode(LastNode, value, LastNode == null ? 0 : LastNode.XOffset + CellSize.Width);

            //Console.WriteLine($"Node: {node.Value}, {node.XOffset}, {LastNode}");

            _nodes.Add(node);
        }

        public override void Update(GameTime gameTime)
        {
            if (_nodes.Count > 0)
            {
                if (LastNode.XOffset > Size.Width - _xOffset)
                {
                    _xOffset -= (int)CellSize.Width;
                    //_nodes.Remove(LastNode);
                }
            }

            MinValue = MinPushedValue;
            MaxValue = MaxPushedValue;

            base.Update(gameTime);
        }

        public override void Draw(GameTime gameTime)
        {
            _spriteBatch.Begin();

            if (ShouldDrawBackground)
            {
                var posOffset = new Vector2(Position.X - 4, Position.Y - 4);
                var sizeOffset = new Size2(Size.Width + 8, Size.Height + 8);

                _spriteBatch.FillRectangle(posOffset, sizeOffset, Color.Gray);
                _spriteBatch.DrawRectangle(Position, Size, Color.Black);
            }
            else
            {
                _spriteBatch.DrawLine(new Vector2(Position.X, Position.Y + Size.Height),
                    new Vector2(Position.X + Size.Width, Position.Y + Size.Height), Color.Black);
            }

            if (_nodes.Count > 0)
            {
                foreach (var node in _nodes)
                {
                    var pos = new Vector2(
                        Position.X + node.XOffset + _xOffset,
                        Position.Y - MathHelperExtensions.InBetween(Size.Height, node.Value, MinValue, MaxValue) + Size.Height
                        );

                    if (pos.X >= Position.X)
                    {
                        var rect = new RectangleF(pos - Vector2.UnitX * (CellSize.Width / 2),
                            new Size2(CellSize.Width, Size.Height * node.Value / (MaxValue - MinValue)));

                        var mousePos = Mouse.GetState().Position.ToVector2();
                        var mouseRect = new RectangleF(mousePos, new Size2(1, 1));

                        if (new RectangleF(new Point2(Position.X + node.XOffset + _xOffset, Position.Y), new Size2(CellSize.Width, Size.Height)).Intersects(mouseRect))
                        {
                            if (ShouldDrawBars)
                            {
                                _spriteBatch.FillRectangle(rect, Color.Red);
                                _spriteBatch.DrawRectangle(rect, Color.Green);
                            }

                            if (Font != null)
                            {
                                _spriteBatch.FillRectangle(new Vector2(pos.X, Position.Y + Size.Height),
                                    new Size2(node.Value.ToString(CultureInfo.CurrentCulture).Length * 10, 16), Color.Yellow);
                                _spriteBatch.DrawString(Font, node.Value.ToString(),
                                    new Vector2(pos.X, Position.Y + Size.Height), Color.Black);
                            }

                            _spriteBatch.DrawPoint(pos, Color.Orange, 4f);
                        }
                        else
                        {
                            if (ShouldDrawBars)
                            {
                                _spriteBatch.FillRectangle(rect, Color.LightCyan);
                                _spriteBatch.DrawRectangle(rect, Color.Green);
                            }
                        }

                        if (node.Next != null)
                        {
                            var nextPos = new Vector2(
                                Position.X + node.Next.XOffset + _xOffset,
                                Position.Y - MathHelperExtensions.InBetween(Size.Height, node.Next.Value, MinValue, MaxValue) + Size.Height
                                );
                            _spriteBatch.DrawLine(pos, nextPos, Color.White);
                        }
                        _spriteBatch.DrawPoint(pos, Color.Red, 2f);
                    }
                }

                if (Font != null)
                {
                    _spriteBatch.DrawString(Font, $"Avg: {_nodes.Average(n => n.Value):F1}\nMin: {MinPushedValue:F1}\nMax: {MaxPushedValue:F1}",
                        Position + Vector2.UnitY * Size.Height, Color.Black);
                }
            }

            _spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}