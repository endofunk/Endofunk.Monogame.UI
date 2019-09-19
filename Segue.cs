//
// Segue.cs
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

namespace Endofunk.Monogame.UI {
  public enum Transition {
    EaseIn,    // slow at the beginning, fast at the end
    EaseOut,   // fast at the beginning, slow at the end
    EaseInOut, // slow start, then fast, then end slowly
    Linear,    // same speed from start to end
    None
  }

  public enum Direction {
    Up, Down, Left, Right, None
  }

  public class Segue {
    private Vector2 increment;
    public Vector2 Position { get; private set; }
    public Transition Transition;
    public Direction Direction;
    public Screen Screen;
    private bool IsSegueComplete;
    public Segue(Screen screen, Transition transition, Direction direction) {
      Screen = screen;
      Transition = transition;
      Direction = direction;
      increment = InitialIncrement();
      Position = InitialPosition();
    }

    public void Update(GameTime gameTime) {
      if (Direction.HasFlag(Direction.None) || Transition.HasFlag(Transition.None)) return;
      if (IsSegueComplete) return;
      if (IsTransitionInProgress()) {
        Position += increment;
        increment = AdjustIncrement();
      } else {
        IsSegueComplete = true;
        Position = Vector2.Zero;
        increment = Vector2.Zero;
      }
    }

    private bool IsTransitionInProgress() {
      switch (Direction) {
        case Direction.Up: return Position.Y > 0;
        case Direction.Down: return Position.Y < 0;
        case Direction.Left: return Position.X < 0;
        case Direction.Right: return Position.X > 0;
        default: return false;
      }
    }

    private Vector2 InitialPosition() {
      switch (Direction) {
        case Direction.Left: return new Vector2(-Screen.View.Bounding.X, 0);
        case Direction.Up: return new Vector2(0, Screen.View.Bounding.Y);
        case Direction.Down: return new Vector2(0, -Screen.View.Bounding.Y);
        case Direction.Right: return new Vector2(Screen.View.Bounding.X, 0);
        default: return new Vector2(0, 0);
      }
    }

    private Vector2 InitialIncrement() {
      switch (Transition) {
        case Transition.EaseIn: return Increment(new Vector2(59, 20));
        case Transition.EaseOut: return Increment(new Vector2(1, 1));
        case Transition.EaseInOut: return Increment(new Vector2(1, 1));
        case Transition.Linear: return Increment(new Vector2(15, 10));
        default: return new Vector2(0, 0);
      }
    }

    private Vector2 Increment(Vector2 initialIncrement) {
      switch (Direction) {
        case Direction.Left: return new Vector2(initialIncrement.X, 0);
        case Direction.Up: return new Vector2(0, -initialIncrement.Y);
        case Direction.Down: return new Vector2(0, initialIncrement.Y);
        case Direction.Right: return new Vector2(-initialIncrement.X, 0);
        default: return new Vector2(0, 0);
      }
    }

    private Vector2 AdjustIncrement() {
      switch (Transition) {
        case Transition.EaseIn: return increment * 1 / 1.1f;
        case Transition.EaseOut: return increment * 1.1f;
        case Transition.EaseInOut: return increment * 1.1f;
        case Transition.Linear: return increment;
        default: return increment;
      }
    }
  }

  public static partial class Prelude {
    public static Segue Segue(Screen screen, Transition transition, Direction direction) => new Segue(screen, transition, direction);
  }
}
