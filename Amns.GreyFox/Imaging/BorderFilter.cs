using System;
using System.Drawing;
using System.Drawing.Drawing2D;

namespace Amns.GreyFox.Imaging
{
	/// <summary>
	/// Summary description for BorderFilter.
	/// </summary>
	public class BorderFilter
	{
		private Image topLeft;
		private Image topRight;
		
		private Image top;
		private Image left;
		private Image right;
		private Image bottom;

		private Image bottomLeft;
		private Image bottomRight;
		
		public BorderFilter(string top, string left, string right, string bottom, 
			string topLeft, string topRight, string bottomLeft, string bottomRight)
		{
			if(top != string.Empty) this.top = Bitmap.FromFile(top);
			if(left != string.Empty) this.left = Bitmap.FromFile(left);
			if(right != string.Empty) this.right = Bitmap.FromFile(right);
			if(bottom != string.Empty) this.bottom = Bitmap.FromFile(bottom);
            if(topLeft != string.Empty) this.topLeft = Bitmap.FromFile(topLeft);
			if(topRight != string.Empty) this.topRight = Bitmap.FromFile(topRight);
			if(bottomLeft != string.Empty) this.bottomLeft = Bitmap.FromFile(bottomLeft);
			if(bottomRight != string.Empty) this.bottomRight = Bitmap.FromFile(bottomRight);			
		}

		~BorderFilter()
		{
			if(topLeft != null) topLeft.Dispose();
			if(topRight != null) topRight.Dispose();
			if(top != null) top.Dispose();
			if(left != null) left.Dispose();
			if(right != null) right.Dispose();
			if(bottom != null) bottom.Dispose();
			if(bottomLeft != null) bottomLeft.Dispose();
			if(bottomRight != null) bottomRight.Dispose();
		}

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

		/// <summary>
		/// Adds a border to a bitmap.
		/// </summary>
		/// <param name="b">Bitmap to apply borders to.</param>
		public void ApplyFilter(Bitmap b)
		{
			int x = 0;
			int y = 0;

			Graphics canvas = getCanvas(b);
			canvas.InterpolationMode = InterpolationMode.HighQualityBicubic;
			canvas.SmoothingMode = SmoothingMode.AntiAlias;

			#region Edges

			if(top != null)
			{
				while(x < b.Width)
				{
					canvas.DrawImageUnscaled(top, x, 0);
					x += top.Width;
				}
			}

			if(bottom != null)
			{
				x = 0;
				y = b.Height - bottom.Height;
				while(x < b.Width)
				{
					canvas.DrawImageUnscaled(bottom, x, y);
					x += bottom.Width;
				}
			}

			if(left != null)
			{
				y = 0;
				while(y < b.Height)
				{
					canvas.DrawImageUnscaled(left, 0, y);
					y += left.Height;
				}
			}

			if(right != null)
			{
				x = b.Width - right.Width;
				y = 0;
				while(y < b.Height)
				{
					canvas.DrawImageUnscaled(right, x, y);
					y += right.Height;
				}
			}

			#endregion

			#region Corners

			
			if(topLeft != null)
				canvas.DrawImageUnscaled(topLeft, 0, 0);
			if(topRight != null)
                canvas.DrawImageUnscaled(topRight, b.Width - topRight.Width, 0);
			if(bottomLeft != null)
				canvas.DrawImageUnscaled(bottomLeft, 0, b.Height - bottomLeft.Height);
			if(bottomRight != null)
				canvas.DrawImageUnscaled(bottomRight, b.Width - bottomRight.Width, b.Height - bottomRight.Height);

			#endregion

			canvas.Dispose();
		}
	}
}
