//
// CheckBox.cs
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
using static Endofunk.Monogame.UI.Prelude;

namespace Endofunk.Monogame.UI {
  public class CheckBox : IElement {
    private Stack _Stack;
    private readonly Label _Label;
    private SpriteSheet _SpriteSheet;
    private int FrameIndex;
    public Vector2 Bounding => _Stack.Bounding;
    private bool IsMouseEnter;
    private bool IsChecked;
    private Rectangle Area = Rectangle.Empty;
    public CheckBox(string text, SpriteFont font, Texture2D texture, Color color, Vector2 bounding, float rotation, float scale, Vector2 origin, int frameCount) : this(text, font, texture, color, bounding, rotation, scale, origin, frameCount, Vector2.Zero, Color.Black) { }
    public CheckBox(string text, SpriteFont font, Texture2D texture, Color color, Vector2 bounding, float rotation, float scale, Vector2 origin, int frameCount, Vector2 shadowOffset, Color shadowColor) {
      _SpriteSheet = SpriteSheet(texture, color, Align.Left | Align.Top, new Vector2(48f, 48f), rotation, scale, origin, frameCount, shadowOffset, shadowColor);
      _Label = Label(text, color, Align.Left | Align.Top, font, bounding, shadowOffset, shadowColor);
      _Stack = HStack(_SpriteSheet, Spacer(new Vector2(10, 10)), _Label);
      FrameIndex = 0;
    }

    public void Draw(SpriteBatch spriteBatch, Vector2 position) {
      _SpriteSheet = _SpriteSheet[FrameIndex];
      _Stack = HStack(_SpriteSheet, Spacer(new Vector2(10, 10)), _Label);
      if (Area.IsEmpty) Area = new Rectangle(position.ToPoint(), Bounding.ToPoint());
      _Stack.Draw(spriteBatch, position);
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
      IsMouseEnter = mouse.HasMouseEntered(gameTime, IsMouseEnter, Area, OnMouseEnter, OnMouseLeave);

      // 0, 1 => unchecked (normal / mousehover)
      // 2, 3 => checked (normal / mousehover)
      FrameIndex = (IsChecked ? 2 : 0) + (IsMouseEnter ? 1 : 0);
      if (mouse.LeftButton == ButtonState.Pressed && IsMouseEnter) {
        IsChecked = !IsChecked;
        OnClicked(EventArgs.Empty);
      }
      OnUpdateEvent(gameTime);
      if (IsMouseEnter) OnMouseMove(gameTime, mouse);
    }
  }
}
