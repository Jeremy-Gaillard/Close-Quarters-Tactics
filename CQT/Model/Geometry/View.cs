using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CQT.Model.Geometry
{
    public class View
    {
        /*
        private void drawLine(int x1, int y1, int x2, int y2)
        {
            Texture2D SimpleTexture = new Texture2D(GraphicsDevice, 1, 1, false,
               SurfaceFormat.Color);

            Int32[] pixel = { 0xFFFFFF }; // White. 0xFF is Red, 0xFF0000 is Blue
            SimpleTexture.SetData<Int32>(pixel, 0, SimpleTexture.Width * SimpleTexture.Height);

            double length = Math.Sqrt(Math.Pow(x2 - x1, 2) + Math.Pow(y2 - y1, 2));
            float angle = (float)Math.Atan2(y2 - y1, x2 - x1);

            int width = 2;

            spriteBatch.Draw(SimpleTexture, new Rectangle(x1, y1, (int)(x1 + length), width), null,
                Color.Blue, angle, new Vector2(0f, 0f), SpriteEffects.None, 1f);
        }
        */
    }
}
