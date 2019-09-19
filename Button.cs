//
// Button.cs
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
  public class Button : IElement {
    private readonly string Text;
    private readonly Canvas Canvas;
    public Vector2 Bounding => Canvas.Bounding;
    private bool IsMouseEnter;
    private int ClickedUpdates;
    private readonly int CornerRadius;
    private readonly float EmSize;
    private readonly System.Drawing.Brush TextColor;
    private readonly System.Drawing.Brush ButtonColor;
    private Rectangle Area = Rectangle.Empty;
    private Action Layout;

    public Button(string text, Game game, float emSize, System.Drawing.Brush textColor, System.Drawing.Brush buttonColor) {
      CornerRadius = 10;
      EmSize = emSize;
      TextColor = textColor;
      ButtonColor = buttonColor;
      var b = new System.Drawing.Bitmap(1, 1);
      var g = System.Drawing.Graphics.FromImage(b);
      var f = new System.Drawing.Font(System.Drawing.FontFamily.GenericSansSerif, emSize);
      var s = g.MeasureString(text, f);
      var p = new Point(Math.Max((int)s.Width + 32, 120), Math.Max((int)s.Height, 60));
      Canvas = new Canvas(game, p);
      Text = text;
      Layout = DefaultLayout;
      ClickedUpdates = 0;
    }

    public void Save() => Canvas.Save();

    private void Render(string buttonPng) {
      Canvas.Graphics.Clear(System.Drawing.Color.Transparent);
      Canvas.Graphics.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;
      Canvas.Graphics.FillRoundedRectangle(ButtonColor, new System.Drawing.Rectangle(6, 6, (int)Bounding.X - 12, (int)Bounding.Y - 12), CornerRadius);
      Canvas.Graphics.TextRenderingHint = System.Drawing.Text.TextRenderingHint.AntiAlias;
      var strRect = new System.Drawing.Rectangle(new System.Drawing.Point(1, 3), new System.Drawing.Size((int)Bounding.X - 2, (int)Bounding.Y - 6));
      var strFormat = new System.Drawing.StringFormat(System.Drawing.StringFormatFlags.NoClip) {
        Alignment = System.Drawing.StringAlignment.Center,
        LineAlignment = System.Drawing.StringAlignment.Center
      };
      Canvas.Graphics.DrawString(Text, new System.Drawing.Font(System.Drawing.FontFamily.GenericSansSerif, EmSize, System.Drawing.FontStyle.Regular), TextColor, strRect, strFormat);
      var overlay = new System.Drawing.Bitmap(buttonPng);
      var prefix = new System.Drawing.Rectangle(0, 0, 15, overlay.Height);
      var suffix = new System.Drawing.Rectangle(overlay.Width - 16, 0, 16, overlay.Height);
      var midfix = new System.Drawing.Rectangle(16, 0, overlay.Width - 32, overlay.Height);
      Canvas.Graphics.DrawImage(overlay, new System.Drawing.Rectangle(0, 0, 16, (int)Bounding.Y), prefix, System.Drawing.GraphicsUnit.Pixel);
      Canvas.Graphics.DrawImage(overlay, new System.Drawing.Rectangle(16, 0, (int)Bounding.X - 32, (int)Bounding.Y), midfix, System.Drawing.GraphicsUnit.Pixel);
      Canvas.Graphics.DrawImage(overlay, new System.Drawing.Rectangle((int)Bounding.X - 16, 0, 16, (int)Bounding.Y), suffix, System.Drawing.GraphicsUnit.Pixel);
      Save();
    }

    private void DefaultLayout() => Render("./Content/Images/ButtonNormal.png");
    public void HoverLayout() => Render("./Content/Images/ButtonHover.png");
    public void ClickLayout() => Render("./Content/Images/ButtonClicked.png");

    public void Draw(SpriteBatch spriteBatch, Vector2 position) {
      if (Area.IsEmpty || Area.Location != position.ToPoint()) Area = new Rectangle(position.ToPoint(), Bounding.ToPoint());
      Canvas.Draw(spriteBatch, position);
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
    protected virtual void OnUpdateEvent(GameTime g) => UpdateEvent?.Invoke(this, g);

    public void Update(GameTime gameTime) {
      MouseState mouse = Mouse.GetState();
      ClickedUpdates = (ClickedUpdates > 0 && mouse.LeftButton != ButtonState.Pressed) ? ClickedUpdates - 1 : ClickedUpdates;
      Canvas.Update(gameTime);
      IsMouseEnter = mouse.HasMouseEntered(gameTime, IsMouseEnter, Area, OnMouseEnter, OnMouseLeave);
      if (mouse.LeftButton == ButtonState.Pressed && IsMouseEnter && ClickedUpdates < 1) {
        ClickedUpdates = 4;
        OnClicked(EventArgs.Empty);
      }
      Layout = DefaultLayout;
      if (ClickedUpdates > 0) Layout = ClickLayout;
      if (IsMouseEnter && ClickedUpdates < 1) Layout = HoverLayout;
      Layout();
      OnUpdateEvent(gameTime);
      if (IsMouseEnter) OnMouseMove(gameTime, mouse);
    }
  }

  public static partial class Prelude {
    #region Syntactic Sugar
    public static Button Button(string text, Game game, float emSize, System.Drawing.Brush textColor, System.Drawing.Brush buttonColor) => new Button(text, game, emSize, textColor, buttonColor);
    #endregion
  }
}
