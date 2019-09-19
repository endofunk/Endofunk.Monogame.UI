//
// Layout.cs
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

namespace Endofunk.Monogame.UI {
  [Flags] public enum Layout { Horizontal = 1, Vertical = 2 }

  public static class LayoutExtensions {
    public static Vector2 Offset(this Layout l, Vector2 p, float by) => l.HasFlag(Layout.Horizontal) ? new Vector2(p.X + by, p.Y) : new Vector2(p.X, p.Y + by);
    public static float Increment(this Layout l, Vector2 b) => l.HasFlag(Layout.Horizontal) ? b.X : b.Y;
    public static Vector2 Bounding(this Layout l, IElement[] xs) => l.HasFlag(Layout.Horizontal) ? new Vector2(xs.Sum(e => e.Bounding.X), xs.Max(e => e.Bounding.Y)) : new Vector2(xs.Max(e => e.Bounding.X), xs.Sum(e => e.Bounding.Y));
  }
}
