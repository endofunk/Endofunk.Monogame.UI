//
// Image.cs
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
  public class Image : IElement {
    public readonly Texture2D Texture;
    public readonly Color Color;
    public readonly Align Align;
    public float Rotation;
    public readonly Vector2 Scale;
    public readonly Vector2 Origin;
    public readonly Vector2 ShadowOffset;
    public readonly Color ShadowColor;
    public Vector2 Bounding { get; }
    private bool IsMouseEnter;
    private int ClickedUpdates;
    private Rectangle Area = Rectangle.Empty;
    public Image(Texture2D texture, Color color, Align align, Vector2 bounding, float rotation, Vector2 scale, Vector2 origin) : this(texture, color, align, bounding, rotation, scale, origin, Vector2.Zero, Color.Black) { }
    public Image(Texture2D texture, Color color, Align align, Vector2 bounding, float rotation, Vector2 scale, Vector2 origin, Vector2 shadowOffset, Color shadowColor) {
      (Texture, Color, Align, Bounding, Rotation, Scale, Origin, ShadowOffset, ShadowColor, ClickedUpdates) = (texture, color, align, bounding, rotation, scale, origin, shadowOffset, shadowColor, 0);
    }
    public void Draw(SpriteBatch spriteBatch, Vector2 position) {
      if (Area.IsEmpty || Area.Location != position.ToPoint()) Area = new Rectangle(position.ToPoint(), Bounding.ToPoint());
      spriteBatch.DrawSprite(Texture, new Rectangle(Point.Zero, new Point(Texture.Width, Texture.Height)), new Rectangle(position.ToPoint(), Bounding.ToPoint()), Align, Color, Rotation, Scale, Origin, ShadowOffset, ShadowColor);
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
    public static Image Image(Texture2D texture, Color color, Align align, Vector2 bounding, float rotation, Vector2 scale, Vector2 origin) => new Image(texture, color, align, bounding, rotation, scale, origin);
    public static Image Image(Texture2D texture, Color color, Align align, Vector2 bounding, float rotation, Vector2 scale, Vector2 origin, Vector2 shadowOffset, Color shadowColor) => new Image(texture, color, align, bounding, rotation, scale, origin, shadowOffset, shadowColor);
    #endregion
  }
}
