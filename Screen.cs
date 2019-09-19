//
// Screen.cs
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
using static Endofunk.Monogame.UI.Prelude;

namespace Endofunk.Monogame.UI {

  public class Screen {
    public readonly IElement View;
    internal Segue Segue;
    private Screen() { }

    internal Screen(IElement view, Transition transition = Transition.EaseOut, Direction direction = Direction.Left) {
      View = view;
      Segue = Segue(this, transition, direction);
      view.UpdateEvent += View_UpdateEvent;
    }

    private void View_UpdateEvent(object sender, GameTime gameTime) {
      Segue.Update(gameTime);
    }
  }

  public static class ScreenExtension {
    public static void Draw(this Screen screen, SpriteBatch spriteBatch) {
      screen.View.Draw(spriteBatch, screen.Segue.Position);
    }

    public static void Update(this Screen screen, GameTime gameTime) {
      screen.View.Update(gameTime);
    }
  }

  public static partial class Prelude {
    public static Screen Screen(IElement view, Transition transition = Transition.EaseOut, Direction direction = Direction.Left) => new Screen(view, transition, direction);
  }
}
