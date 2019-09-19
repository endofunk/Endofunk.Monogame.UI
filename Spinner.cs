//
// Spinner.cs
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
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using static Endofunk.Monogame.UI.Prelude;

namespace Endofunk.Monogame.UI {
  public class Spinner : IElement {
    private readonly SpriteSheet _SpriteSheet;
    private int FrameIndex;
    public Vector2 Bounding => _SpriteSheet.Bounding;
    private bool IsMouseEnter;
    private Rectangle Area = Rectangle.Empty;
    public Spinner(Texture2D texture, Color color, Align align, Vector2 bounding, float rotation, float scale, Vector2 origin, int frameCount) : this(texture, color, align, bounding, rotation, scale, origin, frameCount, Vector2.Zero, Color.Black) { }
    public Spinner(Texture2D texture, Color color, Align align, Vector2 bounding, float rotation, float scale, Vector2 origin, int frameCount, Vector2 shadowOffset, Color shadowColor) {
      _SpriteSheet = SpriteSheet(texture, color, align, bounding, rotation, scale, origin, frameCount, shadowOffset, shadowColor);
      FrameIndex = 0;
    }

    public void Draw(SpriteBatch spriteBatch, Vector2 position) {
      if (Area.IsEmpty) Area = new Rectangle(position.ToPoint(), Bounding.ToPoint());
      spriteBatch.DrawSpriteFrame(_SpriteSheet.Texture, new Rectangle(position.ToPoint(), _SpriteSheet.Bounding.ToPoint()), _SpriteSheet.Align, _SpriteSheet.Color, _SpriteSheet.Rotation, _SpriteSheet.Scale, _SpriteSheet.Origin, _SpriteSheet.FrameCount, FrameIndex, _SpriteSheet.ShadowOffset, _SpriteSheet.ShadowColor);
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
      FrameIndex = FrameIndex <= _SpriteSheet.FrameCount ? FrameIndex++ : 0;
      OnUpdateEvent(gameTime);
      if (IsMouseEnter) OnMouseMove(gameTime, mouse);
    }
  }

  public static partial class Prelude {
    public static Spinner Spinner(Texture2D texture, Color color, Align align, Vector2 bounding, float rotation, float scale, Vector2 origin, int frameCount) => new Spinner(texture, color, align, bounding, rotation, scale, origin, frameCount, Vector2.Zero, Color.Black);
    public static Spinner Spinner(Texture2D texture, Color color, Align align, Vector2 bounding, float rotation, float scale, Vector2 origin, int frameCount, Vector2 shadowOffset, Color shadowColor) => new Spinner(texture, color, align, bounding, rotation, scale, origin, frameCount, shadowOffset, shadowColor);
  }
}
