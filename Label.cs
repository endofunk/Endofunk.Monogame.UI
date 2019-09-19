//
// Label.cs
//
// Author:  endofunk
//
// Copyright (c) 2019 
//
// Permission is hereby granted, free of charge, to any person obtaining a copy
// of this software and associated documentation files (the "Software"), to deal
// in the Software without restriction, including without limitation the rights
// to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
// copies of the Software, and to permit persons to whom the Software is
// furnished to do so, subject to the following conditions:
//
// The above copyright notice and this permission notice shall be included in
// all copies or substantial portions of the Software.
//
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
// IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
// FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
// AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
// LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
// OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN
// THE SOFTWARE.
using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Endofunk.Monogame.UI {
  public class Label : IElement {
    public string Text;
    public readonly Color Color;
    public readonly Align Align;
    public readonly SpriteFont Font;
    public readonly Vector2 ShadowOffset = Vector2.Zero;
    public readonly Color ShadowColor = Color.Black;
    public Vector2 Bounding { get; }
    private bool IsMouseEnter;
    private int ClickedUpdates;
    private Rectangle Area = Rectangle.Empty;
    public Label(string text, Color color, Align align, SpriteFont font, Vector2 bounding) : this(text, color, align, font, bounding, Vector2.Zero, Color.Black) { }
    public Label(string text, Color color, Align align, SpriteFont font, Vector2 bounding, Vector2 shadowOffset, Color shadowColor) {
      (Text, Color, Align, Font, Bounding, ShadowOffset, ShadowColor, ClickedUpdates) = (text, color, align, font, bounding, shadowOffset, shadowColor, 0);
    }
    public void Draw(SpriteBatch spriteBatch, Vector2 position) {
      if (Area.IsEmpty || Area.Location != position.ToPoint()) Area = new Rectangle(position.ToPoint(), Bounding.ToPoint());
      spriteBatch.DrawString(Font, Text, new Rectangle(position.ToPoint(), Bounding.ToPoint()), Align, Color, ShadowOffset, ShadowColor);
    }

    public event EventHandler Clicked;
    public event MouseEventHandler MouseEnter;
    public event MouseEventHandler MouseLeave;
    public event MouseEventHandler MouseMove;
    public event UpdateHandler UpdateEvent;
    protected virtual void OnClicked(EventArgs e) => Clicked?.Invoke(this, e);
    protected virtual void OnMouseEnter(GameTime g, MouseState m) => MouseEnter?.Invoke(this, g, m);
    protected virtual void OnMouseLeave(GameTime g, MouseState m) => MouseLeave?.Invoke(this, g, m);
    protected virtual void OnMouseMove(GameTime g, MouseState m) => MouseMove?.Invoke(this, g, m);
    protected virtual void OnUpdateEvent(GameTime gameTime) => UpdateEvent?.Invoke(this, gameTime);

    public void Update(GameTime gameTime) {
      MouseState mouse = Mouse.GetState();
      ClickedUpdates = (ClickedUpdates > 0 && mouse.LeftButton != ButtonState.Pressed) ? ClickedUpdates - 1 : ClickedUpdates;
      IsMouseEnter = mouse.HasMouseEntered(gameTime, IsMouseEnter, Area, OnMouseEnter, OnMouseLeave);
      if (mouse.LeftButton == ButtonState.Pressed && IsMouseEnter && ClickedUpdates < 1) {
        ClickedUpdates = 4;
        OnClicked(EventArgs.Empty);
      }
      OnUpdateEvent(gameTime);
      if (IsMouseEnter) OnMouseMove(gameTime, mouse);
    }
  }

  public static partial class Prelude {
    #region Syntactic Sugar
    public static Label Label(string text, Color color, Align align, SpriteFont font, Vector2 bounding) => new Label(text, color, align, font, bounding);
    public static Label Label(string text, Color color, Align align, SpriteFont font, Vector2 bounding, Vector2 shadowOffset, Color shadowColor) => new Label(text, color, align, font, bounding, shadowOffset, shadowColor);
    #endregion
  }
}
