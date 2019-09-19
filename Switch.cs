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
  public class Switch : IElement {
    private readonly string TextOn;
    private readonly string TextOff;
    public readonly Canvas _Canvas;
    public Vector2 Bounding => _Canvas.Bounding;
    private bool IsMouseEnter;
    private bool IsOn = true;
    private bool IsOff => !IsOn;
    private int ClickedUpdates;
    private Rectangle Area = Rectangle.Empty;
    public System.Drawing.Graphics Graphics => _Canvas.Graphics;
    public Switch(Game game, Vector2 bounding) : this("On", "Off", game, bounding) { }
    public Switch(string textOn, string textOff, Game game, Vector2 bounding) {
      _Canvas = new Canvas(game, bounding.ToPoint());
      TextOn = textOn;
      TextOff = textOff;
      DefaultLayout();
      ClickedUpdates = 0;
    }

    public void Save() {
      _Canvas.Save();
    }

    public void HoverLayout() {
      var rounding = 6;
      var buttonColor = System.Drawing.Color.Gray;
      Graphics.Clear(System.Drawing.Color.Transparent);
      Graphics.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;
      Graphics.DrawRoundedRectangle(new System.Drawing.Pen(System.Drawing.Color.FromArgb(255, 191, 191, 191), 4), new System.Drawing.Rectangle(2, 4, (int)Bounding.X - 4, (int)Bounding.Y - 5), rounding);
      var linearGradientBrush = new System.Drawing.Drawing2D.LinearGradientBrush(new System.Drawing.PointF(0f, 4f), new System.Drawing.PointF(0f, Bounding.Y + 14), System.Drawing.Color.FromArgb(255, 255, 255, 255), buttonColor);
      Graphics.FillRoundedRectangle(linearGradientBrush, new System.Drawing.Rectangle(1, 3, (int)Bounding.X - 2, (int)Bounding.Y - 7), rounding);
      Graphics.DrawRoundedRectangle(new System.Drawing.Pen(System.Drawing.Color.FromArgb(255, 200, 200, 200), 2), new System.Drawing.Rectangle(1, 3, (int)Bounding.X - 2, (int)Bounding.Y - 7), rounding);
      var strRect = new System.Drawing.Rectangle(new System.Drawing.Point(1, 3), new System.Drawing.Size((int)Bounding.X - 2, (int)Bounding.Y - 6));
      var strFormat = new System.Drawing.StringFormat(System.Drawing.StringFormatFlags.NoClip);
      strFormat.Alignment = System.Drawing.StringAlignment.Center;
      strFormat.LineAlignment = System.Drawing.StringAlignment.Center;
      Graphics.DrawString(TextOn, new System.Drawing.Font(System.Drawing.FontFamily.GenericSansSerif, 16, System.Drawing.FontStyle.Bold), System.Drawing.Brushes.Black, strRect, strFormat);
      Save();
    }

    private void DefaultLayout() {
      var rounding = 6;
      var buttonColor = System.Drawing.Color.Gray;
      Graphics.Clear(System.Drawing.Color.Transparent);
      Graphics.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;
      Graphics.DrawRoundedRectangle(new System.Drawing.Pen(System.Drawing.Color.FromArgb(255, 191, 191, 191), 4), new System.Drawing.Rectangle(2, 4, (int)Bounding.X - 4, (int)Bounding.Y - 5), rounding);
      var linearGradientBrush = new System.Drawing.Drawing2D.LinearGradientBrush(new System.Drawing.PointF(0f, 4f), new System.Drawing.PointF(0f, Bounding.Y + 14), System.Drawing.Color.FromArgb(255, 255, 255, 255), buttonColor);
      Graphics.FillRoundedRectangle(linearGradientBrush, new System.Drawing.Rectangle(1, 3, (int)Bounding.X - 2, (int)Bounding.Y - 7), rounding);
      Graphics.DrawRoundedRectangle(new System.Drawing.Pen(System.Drawing.Color.FromArgb(255, 35, 35, 35), 2), new System.Drawing.Rectangle(1, 3, (int)Bounding.X - 2, (int)Bounding.Y - 7), rounding);
      var strRect = new System.Drawing.Rectangle(new System.Drawing.Point(1, 3), new System.Drawing.Size((int)Bounding.X - 2, (int)Bounding.Y - 6));
      var strFormat = new System.Drawing.StringFormat(System.Drawing.StringFormatFlags.NoClip);
      strFormat.Alignment = System.Drawing.StringAlignment.Center;
      strFormat.LineAlignment = System.Drawing.StringAlignment.Center;
      Graphics.DrawString(TextOn, new System.Drawing.Font(System.Drawing.FontFamily.GenericSansSerif, 16, System.Drawing.FontStyle.Bold), System.Drawing.Brushes.Black, strRect, strFormat);
      Save();
    }

    public void ClickLayout() {
      var rounding = 6;
      var buttonColor = System.Drawing.Color.Gray;
      Graphics.Clear(System.Drawing.Color.Transparent);
      Graphics.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;
      Graphics.DrawRoundedRectangle(new System.Drawing.Pen(System.Drawing.Color.FromArgb(255, 0, 0, 0), 4), new System.Drawing.Rectangle(2, 2, (int)Bounding.X - 4, (int)Bounding.Y - 3), rounding);
      var linearGradientBrush = new System.Drawing.Drawing2D.LinearGradientBrush(new System.Drawing.PointF(0f, 0f), new System.Drawing.PointF(0f, Bounding.Y - 6), buttonColor, System.Drawing.Color.FromArgb(255, 253, 253, 253));
      Graphics.FillRoundedRectangle(linearGradientBrush, new System.Drawing.Rectangle(1, 3, (int)Bounding.X - 2, (int)Bounding.Y - 7), rounding);
      Graphics.DrawRoundedRectangle(new System.Drawing.Pen(System.Drawing.Color.FromArgb(255, 255, 255, 255), 2), new System.Drawing.Rectangle(1, 3, (int)Bounding.X - 2, (int)Bounding.Y - 7), rounding);
      var strRect = new System.Drawing.Rectangle(new System.Drawing.Point(1, 3), new System.Drawing.Size((int)Bounding.X - 2, (int)Bounding.Y - 6));
      var strFormat = new System.Drawing.StringFormat(System.Drawing.StringFormatFlags.NoClip);
      strFormat.Alignment = System.Drawing.StringAlignment.Center;
      strFormat.LineAlignment = System.Drawing.StringAlignment.Center;
      Graphics.DrawString(TextOn, new System.Drawing.Font(System.Drawing.FontFamily.GenericSansSerif, 16, System.Drawing.FontStyle.Bold), System.Drawing.Brushes.Black, strRect, strFormat);
      Save();
    }

    public void Draw(SpriteBatch spriteBatch, Vector2 position) {
      if (Area.IsEmpty) Area = new Rectangle(position.ToPoint(), Bounding.ToPoint());
      _Canvas.Draw(spriteBatch, position);
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
      ClickedUpdates = ClickedUpdates > 0 ? ClickedUpdates - 1 : ClickedUpdates;
      _Canvas.Update(gameTime);
      MouseState mouse = Mouse.GetState();
      IsMouseEnter = mouse.HasMouseEntered(gameTime, IsMouseEnter, Area, OnMouseEnter, OnMouseLeave);
      if (mouse.LeftButton == ButtonState.Pressed && IsMouseEnter && ClickedUpdates < 1) {
        ClickedUpdates = 10;
        //Current.Instance.Store.Fold(s => s.Sounds.Fold(a => a["Click1"].Play()));
        OnClicked(EventArgs.Empty);
      }
      if (IsMouseEnter) {
        if (ClickedUpdates > 0) {
          ClickLayout();
        } else {
          HoverLayout();
        }
      }
      if (!IsMouseEnter) {
        if (ClickedUpdates > 0) {
          ClickLayout();
        } else {
          DefaultLayout();
        }
      }
      OnUpdateEvent(gameTime);
      if (IsMouseEnter) OnMouseMove(gameTime, mouse);
    }
  }

  public static partial class Prelude {
    #region Syntactic Sugar
    //public static Button2 Button2(Game game, Vector2 bounding) => new Button2(game, bounding);
    //public static Button2 Button2(string text, Game game, Vector2 bounding) => new Button2(text, game, bounding);
    #endregion
  }
}
