// <copyright file="Spinner.cs" company="Adam Ralph">
//  Copyright (c) Adam Ralph. (adam@adamralph.com)
// </copyright>

namespace JabbR.History.Console
{
    using System;
    using System.Globalization;

    internal class Spinner
    {
        private readonly double minFrameMilliseconds;
        private readonly string[] frameFormats;

        private DateTime lastSpin = DateTime.MinValue;
        private int position;
        private int maxFrameLength;

        public Spinner(uint maxFramesPerSecond, params string[] frameFormats)
        {
            if (frameFormats.Length == 0)
            {
                throw new ArgumentException("No frame formats provided.", "frameFormats");
            }

            this.minFrameMilliseconds = maxFramesPerSecond == 0 ? int.MaxValue : (int)(1000m / Math.Abs(maxFramesPerSecond));
            this.frameFormats = frameFormats;
        }

        public void Spin(params object[] formatArgs)
        {
            var now = DateTime.Now;
            if ((now - this.lastSpin).TotalMilliseconds < this.minFrameMilliseconds)
            {
                return;
            }

            this.lastSpin = now;
            var frame = string.Format(CultureInfo.CurrentCulture, this.frameFormats[this.position], formatArgs);
            this.maxFrameLength = Math.Max(this.maxFrameLength, frame.Length);
            
            Console.CursorVisible = false;
            Console.Write(frame.PadRight(this.maxFrameLength));
            Console.SetCursorPosition(Console.CursorLeft - this.maxFrameLength, Console.CursorTop);
            
            ++this.position;
            this.position = this.position % this.frameFormats.Length;
        }

        public void Hide()
        {
            Console.Write(string.Empty.PadRight(this.maxFrameLength));
            Console.SetCursorPosition(Console.CursorLeft - this.maxFrameLength, Console.CursorTop);
            Console.CursorVisible = true;

            this.position = 0;
        }
    }
}
