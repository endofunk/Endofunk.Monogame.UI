//
// Canvas2.cs
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
using System.IO;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Endofunk.Monogame.UI {
  public class Canvas : IElement {
    public readonly Game Game;
    public readonly Point Size;
    private Texture2D Texture;
    private float Rotation;
    public readonly Vector2 Origin;
    private bool HasChanged;
    public Vector2 Bounding => Size.ToVector2();
    private bool IsMouseEnter;
    private Rectangle Area = Rectangle.Empty;
    public System.Drawing.Graphics Graphics;
    public System.Drawing.Bitmap Bitmap;
    public Canvas(Game game, Point size) {
      Game = game;
      Size = size;
      Bitmap = new System.Drawing.Bitmap(size.X, size.Y);
      Graphics = System.Drawing.Graphics.FromImage(Bitmap);
      Rotation = 0f;
      Origin = Vector2.Zero;
      UpdateTexture();
    }

    public void Save() {
      HasChanged = true;
    }

    public void Dispose() {
      Graphics.Dispose();
      Bitmap.Dispose();
    }

    public event MouseEventHandler MouseEnter;
    public event MouseEventHandler MouseLeave;
    public event MouseEventHandler MouseMove;
    public event UpdateHandler UpdateEvent;
    protected virtual void OnMouseEnter(GameTime g, MouseState m) => MouseEnter?.Invoke(this, g, m);
    protected virtual void OnMouseLeave(GameTime g, MouseState m) => MouseLeave?.Invoke(this, g, m);
    protected virtual void OnMouseMove(GameTime g, MouseState m) => MouseMove?.Invoke(this, g, m);
    protected virtual void OnUpdateEvent(GameTime gameTime) => UpdateEvent?.Invoke(this, gameTime);

    public void Draw(SpriteBatch spriteBatch, Vector2 position) {
      if (Area.IsEmpty) Area = new Rectangle(Point.Zero, Bounding.ToPoint());
      spriteBatch.Draw(Texture, position, Area, Color.White, Rotation, Origin, 1f, SpriteEffects.None, 0f);
    }

    public void UpdateTexture() {
      using (var stream = new MemoryStream()) {
        Bitmap.Save(stream, System.Drawing.Imaging.ImageFormat.Png);
        stream.Seek(0, SeekOrigin.Begin);
        Texture = Texture2D.FromStream(Game.GraphicsDevice, stream);
      }
    }

    public void Update(GameTime gameTime) {
      if (HasChanged) {
        UpdateTexture();
        HasChanged = false;
      }
      MouseState mouse = Mouse.GetState();
      IsMouseEnter = mouse.HasMouseEntered(gameTime, IsMouseEnter, Area, OnMouseEnter, OnMouseLeave);
      OnUpdateEvent(gameTime);
      if (IsMouseEnter) OnMouseMove(gameTime, mouse);
    }
  }

  public static class GraphicsExtensions {
    private static System.Drawing.Drawing2D.GraphicsPath RoundedRect(System.Drawing.Rectangle bounds, int radius) {
      int diameter = radius * 2;
      System.Drawing.Size size = new System.Drawing.Size(diameter, diameter);
      System.Drawing.Rectangle arc = new System.Drawing.Rectangle(bounds.Location, size);
      System.Drawing.Drawing2D.GraphicsPath path = new System.Drawing.Drawing2D.GraphicsPath();
      if (radius == 0) {
        path.AddRectangle(bounds);
        return path;
      }
      path.AddArc(arc, 180, 90);
      arc.X = bounds.Right - diameter - 1;
      path.AddArc(arc, 270, 90);
      arc.Y = bounds.Bottom - diameter - 1;
      path.AddArc(arc, 0, 90);
      arc.X = bounds.Left;
      path.AddArc(arc, 90, 90);
      path.CloseFigure();
      return path;
    }

    public static void DrawRoundedRectangle(this System.Drawing.Graphics graphics, System.Drawing.Pen pen, System.Drawing.Rectangle bounds, int cornerRadius) {
      if (graphics == null) throw new ArgumentNullException("graphics");
      if (pen == null) throw new ArgumentNullException("pen");
      using (System.Drawing.Drawing2D.GraphicsPath path = RoundedRect(bounds, cornerRadius)) {
        graphics.DrawPath(pen, path);
      }
    }

    public static void FillRoundedRectangle(this System.Drawing.Graphics graphics, System.Drawing.Brush brush, System.Drawing.Rectangle bounds, int cornerRadius) {
      if (graphics == null) throw new ArgumentNullException("graphics");
      if (brush == null) throw new ArgumentNullException("brush");
      using (System.Drawing.Drawing2D.GraphicsPath path = RoundedRect(bounds, cornerRadius)) {
        graphics.FillPath(brush, path);
      }
    }
  }

  public static partial class Prelude {
    #region Syntactic Sugar
    public static Canvas Canvas(Game game, Point size) => new Canvas(game, size);
    #endregion
  }
}
