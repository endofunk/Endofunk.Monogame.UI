//
// SpriteSheet.cs
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
  public class SpriteSheet : IElement {
    public readonly Texture2D Texture;
    public readonly Color Color;
    public readonly Align Align;
    public float Rotation;
    public readonly float Scale;
    public readonly Vector2 Origin;
    public readonly int FrameCount;
    internal readonly int FrameIndex;
    public readonly Vector2 ShadowOffset = Vector2.Zero;
    public readonly Color ShadowColor = Color.Black;
    public Vector2 Bounding { get; }
    private bool IsMouseEnter;
    private Rectangle Area = Rectangle.Empty;
    public SpriteSheet(Texture2D texture, Color color, Align align, Vector2 bounding, float rotation, float scale, Vector2 origin, int frameCount) : this(texture, color, align, bounding, rotation, scale, origin, frameCount, 0, Vector2.Zero, Color.Black) { }
    public SpriteSheet(Texture2D texture, Color color, Align align, Vector2 bounding, float rotation, float scale, Vector2 origin, int frameCount, int frameIndex, Vector2 shadowOffset, Color shadowColor) {
      (Texture, Color, Align, Bounding, Rotation, Scale, Origin, FrameCount, FrameIndex, ShadowOffset, ShadowColor) = (texture, color, align, bounding, rotation, scale, origin, frameCount, frameIndex, shadowOffset, shadowColor);
    }
    private SpriteSheet(SpriteSheet @this, int frameIndex) : this(@this.Texture, @this.Color, @this.Align, @this.Bounding, @this.Rotation, @this.Scale, @this.Origin, @this.FrameCount, frameIndex, @this.ShadowOffset, @this.ShadowColor) { }
    public SpriteSheet this[int frameIndex] => new SpriteSheet(this, frameIndex);
    public void Draw(SpriteBatch spriteBatch, Vector2 position) {
      if (Area.IsEmpty || Area.Location != position.ToPoint()) Area = new Rectangle(position.ToPoint(), Bounding.ToPoint());
      spriteBatch.DrawSpriteFrame(Texture, new Rectangle(position.ToPoint(), Bounding.ToPoint()), Align, Color, Rotation, Scale, Origin, FrameCount, FrameIndex, ShadowOffset, ShadowColor);
    }

    public event MouseEventHandler MouseEnter;
    public event MouseEventHandler MouseLeave;
    public event MouseEventHandler MouseMove;
    public event UpdateHandler UpdateEvent;
    protected virtual void OnMouseEnter(GameTime g, MouseState m) => MouseEnter?.Invoke(this, g, m);
    protected virtual void OnMouseLeave(GameTime g, MouseState m) => MouseLeave?.Invoke(this, g, m);
    protected virtual void OnMouseMove(GameTime g, MouseState m) => MouseMove?.Invoke(this, g, m);
    protected virtual void OnUpdateEvent(GameTime gameTime) => UpdateEvent?.Invoke(this, gameTime);

    public void Update(GameTime gameTime) {
      MouseState mouse = Mouse.GetState();
      IsMouseEnter = mouse.HasMouseEntered(gameTime, IsMouseEnter, Area, OnMouseEnter, OnMouseLeave);
      OnUpdateEvent(gameTime);
      if (IsMouseEnter) OnMouseMove(gameTime, mouse);
    }
  }

  public static partial class Prelude {
    #region Syntactic Sugar
    public static SpriteSheet SpriteSheet(Texture2D texture, Color color, Align align, Vector2 bounding, float rotation, float scale, Vector2 origin, int frameCount) => new SpriteSheet(texture, color, align, bounding, rotation, scale, origin, frameCount);
    public static SpriteSheet SpriteSheet(Texture2D texture, Color color, Align align, Vector2 bounding, float rotation, float scale, Vector2 origin, int frameCount, Vector2 shadowOffset, Color shadowColor) => new SpriteSheet(texture, color, align, bounding, rotation, scale, origin, frameCount, 0, shadowOffset, shadowColor);
    public static SpriteSheet SpriteSheet(Texture2D texture, Color color, Align align, Vector2 bounding, float rotation, float scale, Vector2 origin, int frameCount, int frameIndex) => new SpriteSheet(texture, color, align, bounding, rotation, scale, origin, frameCount, frameIndex, Vector2.Zero, Color.Black);
    public static SpriteSheet SpriteSheet(Texture2D texture, Color color, Align align, Vector2 bounding, float rotation, float scale, Vector2 origin, int frameCount, int frameIndex, Vector2 shadowOffset, Color shadowColor) => new SpriteSheet(texture, color, align, bounding, rotation, scale, origin, frameCount, frameIndex, shadowOffset, shadowColor);
    #endregion
  }
}
