//
// Stack.cs
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
  public class Panel : IElement {
    public readonly IElement Element;
    public readonly Image _Image;
    public Vector2 Bounding { get; }
    private bool IsMouseEnter;
    private Rectangle Area = Rectangle.Empty;
    public Panel(Texture2D texture, Color color, Align align, Vector2 origin, IElement element) {
      Element = element;
      Bounding = element.Bounding;
      _Image = Image(texture, color, align, new Vector2(texture.Width, texture.Height), 0f, new Vector2(Bounding.X / texture.Width, Bounding.Y / texture.Height), origin);
    }
    public void Draw(SpriteBatch spriteBatch, Vector2 position) {
      if (Area.IsEmpty || Area.Location != position.ToPoint()) Area = new Rectangle(position.ToPoint(), Bounding.ToPoint());
      _Image.Draw(spriteBatch, position);
      Element.Draw(spriteBatch, position);
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
      _Image.Update(gameTime);
      Element.Update(gameTime);
      OnUpdateEvent(gameTime);
      if (IsMouseEnter) OnMouseMove(gameTime, mouse);
    }
  }

  public static partial class Prelude {
    public static Panel Panel(Texture2D texture, Color color, Align align, Vector2 origin, IElement element) => new Panel(texture, color, align, origin, element);
  }
}
