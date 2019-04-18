using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;

namespace Amns.GreyFox.Imaging
{
	/// <summary>
	/// Summary description for BitmapTransform.
	/// </summary>
	public class BitmapTransform
	{
		public BitmapTransform()
		{
			//
			// TODO: Add constructor logic here
			//
		}

		public static Size ProportionalResize(int destWidth, int destHeight, int srcWidth, int srcHeight)
		{
			float multiplier;

			// this means you want to shrink an image that is already shrunken!
			if (srcHeight <= destHeight && srcWidth <= destWidth)
				return new Size(srcWidth, srcHeight);

			// check to see if we can shrink it width first
			multiplier = (float)((float)destWidth / (float)srcWidth);
			if ((srcHeight * multiplier)<= destHeight)
			{
				srcHeight = (int)(srcHeight * multiplier);
				return new Size(destWidth, srcHeight);
			}

			// if we can't get our dest width, then use the dest height
			multiplier = (float)destHeight / (float)srcHeight;
			srcWidth = (int)(srcWidth * multiplier);
			return new Size(srcWidth, destHeight);
		}

		public static Bitmap ProportionalResize(Bitmap imgIn, ImageFormat format, Size newSize)
		{
			Size outSize = ProportionalResize(newSize.Width, newSize.Height, imgIn.Width, imgIn.Height);

			return Resize(imgIn, format, outSize);
		}

		public static Bitmap Resize(Bitmap imgIn, ImageFormat format, Size newSize)
		{
			double y = imgIn.Height;
			double x = imgIn.Width;

			Bitmap imgOut = new Bitmap(newSize.Width, newSize.Height);
			
			Graphics g = Graphics.FromImage(imgOut);
			
			g.InterpolationMode = InterpolationMode.HighQualityBicubic;
			g.SmoothingMode = SmoothingMode.AntiAlias;

			Rectangle dstRect = new Rectangle(0, 0, imgOut.Width, imgOut.Height);
			Rectangle srcRect = new Rectangle(0, 0, imgIn.Width, imgIn.Height);

			g.DrawImage(imgIn, dstRect, srcRect, GraphicsUnit.Pixel);

			return imgOut;
		}
	}
}
