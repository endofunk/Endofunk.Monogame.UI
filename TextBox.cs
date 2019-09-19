//
// TextBox.cs
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
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using static Endofunk.Monogame.UI.Prelude;

namespace Endofunk.Monogame.UI {
  public class TextBox : IElement {
    private Label _Label;
    private readonly SpriteSheet _SpriteSheet;
    private int FrameIndex;
    public Vector2 Bounding => _Label.Bounding;
    private bool IsMouseEnter;
    private bool HasFocus;
    private Rectangle Area = Rectangle.Empty;
    public TextBox(Texture2D texture, Color color, Align align, Vector2 bounding, float rotation, float scale, Vector2 origin, int frameCount) : this("", null, texture, color, align, bounding, rotation, scale, origin, frameCount, Vector2.Zero, Color.Black) { }
    public TextBox(string text, SpriteFont font, Texture2D texture, Color color, Align align, Vector2 bounding, float rotation, float scale, Vector2 origin, int frameCount) : this(text, font, texture, color, align, bounding, rotation, scale, origin, frameCount, Vector2.Zero, Color.Black) { }
    public TextBox(string text, SpriteFont font, Texture2D texture, Color color, Align align, Vector2 bounding, float rotation, float scale, Vector2 origin, int frameCount, Vector2 shadowOffset, Color shadowColor) {
      _SpriteSheet = SpriteSheet(texture, color, align, bounding, rotation, scale, origin, frameCount, shadowOffset, shadowColor);
      _Label = Label(text, color, align, font, bounding, shadowOffset, shadowColor);
      FrameIndex = 0;
    }

    public void Draw(SpriteBatch spriteBatch, Vector2 position) {
      if (Area.IsEmpty) Area = new Rectangle(position.ToPoint(), Bounding.ToPoint());
      spriteBatch.DrawSpriteFrame(_SpriteSheet.Texture, new Rectangle(Vector2.Zero.ToPoint(), _SpriteSheet.Bounding.ToPoint()), _SpriteSheet.Align, _SpriteSheet.Color, _SpriteSheet.Rotation, _SpriteSheet.Scale, _SpriteSheet.Origin, _SpriteSheet.FrameCount, FrameIndex, _SpriteSheet.ShadowOffset, _SpriteSheet.ShadowColor);
      if (!(_Label.Text.Length < 1) && _Label.Font != null) {
        spriteBatch.DrawString(_Label.Font, _Label.Text, new Rectangle(position.ToPoint(), _Label.Bounding.ToPoint()), _Label.Align, _Label.Color, _Label.ShadowOffset, _Label.ShadowColor);
      }
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
      KeyboardState keyb = Keyboard.GetState();
      MouseState mouse = Mouse.GetState();
      IsMouseEnter = mouse.HasMouseEntered(gameTime, IsMouseEnter, Area, OnMouseEnter, OnMouseLeave);

      // capture keystrokes if focused.
      if (HasFocus) {
        keyb.GetPressedKeys().Take(1).ToList().ForEach(k => {
          if ((int)k >= 0X20 && (int)k <= 0X7E) {
            _Label = Label(_Label.Text + (char)k, _Label.Color, _Label.Align, _Label.Font, _Label.Bounding, _Label.ShadowOffset, _Label.ShadowColor);
          }
        });
      }

      //FrameIndex = _MouseHover ? 1 : 0;
      HasFocus |= (mouse.LeftButton == ButtonState.Pressed && IsMouseEnter);
      OnUpdateEvent(gameTime);
      if (IsMouseEnter) OnMouseMove(gameTime, mouse);
    }
  }
}
