//
// ScreenManager.cs
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
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Endofunk.Monogame.UI {
  public class ScreenManager {
    private Screen CurrentScreen;
    private readonly Queue<Screen> Queue; 
    public ScreenManager(params Screen[] screens) {
      Queue = new Queue<Screen>();
      screens.ToList().ForEach(s => Queue.Enqueue(s));
      CurrentScreen = Queue.Any() ? Queue.Dequeue() : throw new ArgumentException("ScreenManager requires at least 1 screen to start");
    }

    public void EnQueue(Screen screen) {
      Queue.Enqueue(screen);
    }

    public void Draw(SpriteBatch spriteBatch) {
      CurrentScreen.Draw(spriteBatch);
      if (Queue.Any()) Queue.Peek().Draw(spriteBatch);
    }

    public void Update(GameTime gameTime) {
      if (Queue.Any()) {
        var screen = Queue.Peek();
        if (screen.Segue.Position.X >= 0) {
          CurrentScreen = Queue.Dequeue();
        } else {
          screen.Update(gameTime);
        }
      }
      CurrentScreen.Update(gameTime);
    }
  }

  public static partial class Prelude {
    public static ScreenManager ScreenManager(params Screen[] screens) => new ScreenManager(screens);
  }
}
