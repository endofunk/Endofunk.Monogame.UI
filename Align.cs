//
// Align.cs
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

namespace Endofunk.Monogame.UI {
  [Flags] public enum Align { Center = 1, Left = 2, Right = 4, Top = 8, Bottom = 16 }

  public static class AlignExtensions {
    public static Vector2 Offset(this Align align, Vector2 size, Rectangle bounds) => align.Offset(size, bounds, Vector2.Zero, Vector2.One);
    public static Vector2 Offset(this Align align, Vector2 size, Rectangle bounds, Vector2 origin, float scale) => align.Offset(size, bounds, origin, new Vector2(scale, scale));

    public static Vector2 Offset(this Align align, Vector2 size, Rectangle bounds, Vector2 origin, Vector2 scale) {
      var offset = new Vector2(bounds.Width / 2 - size.X / 2 + origin.X * scale.X, bounds.Height / 2 - size.Y / 2 + origin.Y * scale.Y);
      if (align.HasFlag(Align.Left)) offset.X -= offset.X;
      if (align.HasFlag(Align.Right)) offset.X += offset.X;
      if (align.HasFlag(Align.Top)) offset.Y -= offset.Y;
      if (align.HasFlag(Align.Bottom)) offset.Y += offset.Y;
      return new Vector2(bounds.X + offset.X, bounds.Y + offset.Y);
    }
  }
}
