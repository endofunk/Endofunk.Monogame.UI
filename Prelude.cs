//
// Prelude.cs
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

  public static class PointExtensions {
    public static bool GreaterThan(this Point @this, Point @that) => (@this.X > that.X && @this.Y > that.Y);
    public static Point AddX(this Point @this, int x) => new Point(@this.X + x, @this.Y);
    public static Point AddX(this Point @this, Point @that) => new Point(@this.X + that.X, @this.Y);
    public static Point AddY(this Point @this, int y) => new Point(@this.X, @this.Y + y);
    public static Point AddY(this Point @this, Point @that) => new Point(@this.X, @this.Y + @that.Y);
  }

  public static class SpriteBatchExtensions {
    public static void DrawString(this SpriteBatch spriteBatch, SpriteFont font, string text, Rectangle bounds, Align align, Color color, Vector2 shadowOffset, Color shadowColor) {
      var size = font.MeasureString(text);
      if (shadowOffset != Vector2.Zero) spriteBatch.DrawString(font, text, align.Offset(size, bounds) + shadowOffset, shadowColor, 0, Vector2.Zero, 1f, SpriteEffects.None, 0f);
      spriteBatch.DrawString(font, text, align.Offset(size, bounds), color, 0, Vector2.Zero, 1f, SpriteEffects.None, 0f);
    }

    public static void DrawSprite(this SpriteBatch spriteBatch, Texture2D texture, Rectangle rect, Rectangle bounds, Align align, Color color, float rotation, Vector2 scale, Vector2 origin, Vector2 shadowOffset, Color shadowColor) {
      var size = new Vector2(texture.Width, texture.Height) * scale;
      if (shadowOffset != Vector2.Zero) spriteBatch.Draw(texture, align.Offset(size, bounds, origin, scale) + shadowOffset, rect, shadowColor, rotation, origin, scale, SpriteEffects.None, 0f);
      spriteBatch.Draw(texture, align.Offset(size, bounds, origin, scale), rect, color, rotation, origin, scale, SpriteEffects.None, 0f);
    }

    public static void DrawSpriteFrame(this SpriteBatch spriteBatch, Texture2D texture, Rectangle bounds, Align align, Color color, float rotation, float scale, Vector2 origin, int frameCount, int frameIndex, Vector2 shadowOffset, Color shadowColor) {
      var size = new Vector2(texture.Width / frameCount, texture.Height) * scale;
      if (shadowOffset != Vector2.Zero) spriteBatch.Draw(texture, align.Offset(size, bounds, origin, scale) + shadowOffset, new Rectangle(new Vector2(texture.Width / frameCount * frameIndex, 0).ToPoint(), new Vector2(texture.Width / frameCount, texture.Height).ToPoint()), shadowColor, rotation, origin, scale, SpriteEffects.None, 0f);
      spriteBatch.Draw(texture, align.Offset(size, bounds, origin, scale), new Rectangle(new Vector2(texture.Width / frameCount * frameIndex, 0).ToPoint(), new Vector2(texture.Width / frameCount, texture.Height).ToPoint()), color, rotation, origin, scale, SpriteEffects.None, 0f);
    }
  }

  public static class GDMExtensions {
    public static Rectangle Bounds(this GraphicsDeviceManager graphics) => new Rectangle(0, 0, graphics.PreferredBackBufferWidth, graphics.PreferredBackBufferHeight);
    public static Vector2 Center(this GraphicsDeviceManager graphics) => graphics.Bounds().Center.ToVector2();
  }

  public static class MouseStateExtensions {
    public static bool HasMouseEntered(this MouseState mouse, GameTime gameTime, bool hover, Rectangle area, Action<GameTime, MouseState> onMouseHover, Action<GameTime, MouseState> onMouseLeave) {
      if (hover == false && area.Contains(mouse.X, mouse.Y)) {
        hover = true;
        onMouseHover(gameTime, mouse);
      } else if (hover && !area.Contains(mouse.X, mouse.Y)) {
        hover = false;
        onMouseLeave(gameTime, mouse);
      }
      return hover;
    }
  }
}
