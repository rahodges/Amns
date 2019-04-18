using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;

namespace Amns.GreyFox.Imaging
{
	/// <summary>
	/// Summary description for ImageWatermark.
	/// </summary>
	public class ImageWatermark
	{
		private static Graphics getCanvas(Bitmap bitmap)
		{
			Graphics canvas;
			
			try
			{
				canvas = Graphics.FromImage(bitmap);
			}
			catch
			{
				Bitmap bmpNew = new Bitmap(bitmap.Width, bitmap.Height);
				ExifProperties.CopyProperties(bitmap, bmpNew);
				canvas = Graphics.FromImage(bmpNew);
				canvas.DrawImage(bmpNew, new Rectangle(0, 0, bmpNew.Width, bmpNew.Height), 0, 0, bitmap.Width, bitmap.Height, GraphicsUnit.Pixel);
			}

			return canvas;
		}

		public static void AddWatermark(Bitmap bitmap, string text, Font font, Brush brush, PointF point)
		{
			AddWatermark(bitmap, text, font, brush, point.X, point.Y);
		}

		public static void AddWatermark(Bitmap bitmap, string text, Font font, Brush brush, float x, float y)
		{
			Graphics canvas = getCanvas(bitmap);	
			canvas.DrawString(text, font, brush, x, y);
			canvas.Dispose();
		}		

		public static void AddWatermark(Bitmap bitmap, string text, Font font, Brush brush, Rectangle layoutRectangle)
		{
			Graphics canvas = getCanvas(bitmap);
			canvas.DrawString(text, font, brush, layoutRectangle);
			canvas.Dispose();
		}

		public static void AddWatermark(Bitmap bitmap, Bitmap watermark, WatermarkPosition position)
		{
			switch(position)
			{
				case WatermarkPosition.TopLeft:
					AddWatermark(bitmap, watermark, 5, 5);
					break;
				case WatermarkPosition.TopRight:
					AddWatermark(bitmap, watermark, bitmap.Width - watermark.Width - 5, 5);
					break;
				case WatermarkPosition.Center:
					AddWatermark(bitmap, watermark, bitmap.Width / 2 - watermark.Width / 2, bitmap.Height / 2 - watermark.Height / 2);
					break;
				case WatermarkPosition.BottomLeft:
					AddWatermark(bitmap, watermark, 5, bitmap.Height - watermark.Height - 5);
					break;
				case WatermarkPosition.BottomRight:
					AddWatermark(bitmap, watermark, bitmap.Width - watermark.Width - 5, bitmap.Height - watermark.Height - 5);
					break;
			}
		}

		public static void AddWatermark(Bitmap bitmap, Bitmap watermark, PointF point)
		{
			AddWatermark(bitmap, watermark, point.X, point.Y);
		}

		public static void AddWatermark(Bitmap bitmap, Bitmap watermark, float x, float y)
		{
			Graphics canvas = getCanvas(bitmap);
			canvas.InterpolationMode = InterpolationMode.HighQualityBicubic;
			canvas.SmoothingMode = SmoothingMode.AntiAlias;
            canvas.DrawImage(watermark, x, y, new Rectangle(0, 0, watermark.Width, watermark.Height), GraphicsUnit.Pixel);
			canvas.Dispose();
		}
	}
}
