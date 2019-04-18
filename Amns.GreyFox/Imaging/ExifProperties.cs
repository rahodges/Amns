using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Resources;
using System.Reflection;
//using System.Web;

namespace Amns.GreyFox.Imaging
{
	public struct ExposureTimeStruct
	{
		public uint Numerator;
		public uint Denomenator;

		public ExposureTimeStruct(uint numerator, uint denomenator)
		{
			Numerator = numerator;
			Denomenator = denomenator;
		}

		public override string ToString()
		{
			if(Numerator == 0)
				return "unknown";
			if(Denomenator == 0)
				return "unknown";
            if(Numerator > Denomenator)
				return string.Format("{0} sec.", Numerator / Denomenator);
			return string.Format("{0}/{1} sec.", Numerator, Denomenator);
		}
	}

	/// <summary>
	/// Summary description for ExifProperties.
	/// </summary>
	public class ExifProperties
	{
		public int Width;
		public int Height;
        
		public string Make;
		public string Model;
		public string Description;

		public string Artist;
		public string Copyright;
		public string Software;

		public int WidthResolution = 72;
		public int HeightResolution = 72;

		public uint FocalLength;		

		public ExposureTimeStruct ExposureTime;
		
		public float FNumber
		{
			get
			{
				return (float) FNumberNumerator / (float) FNumberDenomenator;
			}
		}
		public uint FNumberNumerator;
		public uint FNumberDenomenator;
		
		public int ISOSpeedRating;

		public DateTime DateTimeTaken;

		public ExifProperties(string filePath)
		{
			Load(filePath);
		}

		/// <summary>
		/// Creates a new property item.
		/// </summary>
		/// <returns>A new instance of the <see cref="PropertyItem"/> class.</returns>
		public static PropertyItem CreatePropertyImage()
		{
			// Gets the current assembly, retrieves the stream for the resource and loads the associated image.
			Assembly currentAssembly = Assembly.GetExecutingAssembly();
			using(Stream propertyStream = currentAssembly.GetManifestResourceStream("Amns.GreyFox.Imaging.propertyitem.jpg"))
			using(Image resourceImage = (Image)Image.FromStream(propertyStream))
			{
				return resourceImage.PropertyItems[0];
			}
		} 

//		/// <summary>
//		/// Saves EXIF properties to the image.
//		/// </summary>
//		/// <param name="image">Image to save EXIF properties to.</param>
//		public void Save(Image image)
//		{
//			PropertyItem[] propertyItems = image.PropertyItems;
//			ExifProperties oldExif = ExifProperties(image);
//
//			foreach(PropertyItem item in propertyItems)
//			{
//				switch(item.Id)
//				{
//					default:
//						if(!setPropertyItem(item
//
//				}
//			}
//		}
//
//		private bool setPropertyItem(PropertyItem srcPropertyItem, PropertyItem[] propertyItems)
//		{
//			// Search for existing PropertyItem
//			for(int x = 0; x < propertyItems.Length; x++)
//				if(propertyItems[x].Id == srcPropertyItem.Id)
//				{
//					propertyItems[x].Len = srcPropertyItem.Len;
//					propertyItems[x].Type = srcPropertyItem.Type;
//					propertyItems[x].Value = srcPropertyItem.Value;
//					return true;
//				}
//
//			return false;
//		}

		/// <summary>
		/// A dangerous routine to copy property items since property item is a sealed class.
		/// </summary>
		/// <param name="src">The source image to copy properties from.</param>
		/// <param name="dest">The destination image to copy properties to.</param>
		public static void CopyProperties(Image src, Image dest)
		{
			PropertyItem srcItem;
//			PropertyItem newItem;

			for(int x = 0; x < src.PropertyItems.Length; x++)
			{
				srcItem = src.PropertyItems[x];
				dest.SetPropertyItem(srcItem);
			}
		}

		public static DateTime DateTimeFromString(string dt)
		{
			// Exif DateTime strings are formatted as
			//      "YYYY:MM:DD HH:MM:SS"
			
			string delimiters = " :";
			string[] dt_data = dt.Split ( delimiters.ToCharArray(), 6 );
			DateTime result;
			result = new DateTime (Int32.Parse(dt_data[0]), Int32.Parse(dt_data[1]), Int32.Parse(dt_data[2]),
				Int32.Parse(dt_data[3]), Int32.Parse(dt_data[4]), Int32.Parse(dt_data[5]));
			
			return result;
		}	

		/// <summary>
		/// Loads EXIF properties from a specified file.
		/// </summary>
		/// <param name="filePath">File path to specified file.</param>
		public void Load(string filePath)
		{
			Image img = Image.FromFile(filePath);
			Load(img);
			img.Dispose();
		}

		/// <summary>
		/// Loads EXIF properties from an image.
		/// </summary>
		/// <param name="img">Image to load EXIF properties from.</param>
		public void Load(Image img)
		{
			System.Text.ASCIIEncoding encoding = new System.Text.ASCIIEncoding();

			Width = img.Width;
			Height = img.Height;

			foreach(PropertyItem item in img.PropertyItems)
			{
				switch(item.Id)
				{					
					case 282:
						try { WidthResolution = BitConverter.ToInt16(item.Value, 0); }
                        catch { }
						break;
					case 283:
						try { HeightResolution = BitConverter.ToInt16(item.Value, 0); }
						catch { }
						break;
					case 271:
						try { Make = encoding.GetString(item.Value); }
						catch { }
						break;
					case 272:
						try { Model = encoding.GetString(item.Value); }
						catch { }
						break;
					case 306:
						try {DateTimeTaken = DateTimeFromString(encoding.GetString(item.Value)); }
						catch { }
						break;
					case 33434:
						try { ExposureTime = new ExposureTimeStruct(BitConverter.ToUInt32(item.Value, 0), BitConverter.ToUInt32(item.Value, 4)); }
						catch { }
						break;
					case 33437:
						try 
						{
							FNumberNumerator = BitConverter.ToUInt32(item.Value, 0);
							FNumberDenomenator = BitConverter.ToUInt32(item.Value, 4); 
						}
						catch { }
						break;
					case 34855:
						try { ISOSpeedRating = BitConverter.ToUInt16(item.Value, 0); }
						catch { }
						break;
					case 37386:
						try { FocalLength = (uint) ((float) BitConverter.ToUInt32(item.Value, 0) / (float) BitConverter.ToUInt32(item.Value, 4));}						
						catch { }
						break;
					case 0x010e:
						try { Description = encoding.GetString(item.Value); }
						catch { }
						break;
					case 0x013b:
						try { Artist = encoding.GetString(item.Value); }
						catch { }
						break;
					case 0x8298:
						try { Copyright = encoding.GetString(item.Value); }
						catch { }
						break;
					case 0x0131:
						try { Software = encoding.GetString(item.Value); }
						catch { }
						break;
				}
			}

			img.Dispose();
		}

		private static ImageCodecInfo GetEncoderInfo(String mimeType)
		{
			int j;
			ImageCodecInfo[] encoders;
			encoders = ImageCodecInfo.GetImageEncoders();
			for(j = 0; j < encoders.Length; ++j)
			{
				if(encoders[j].MimeType == mimeType)
					return encoders[j];
			} return null;
		}

		public static void WriteDescriptionInImage(string filename, string newArtist, string newCopyright, string newDescription)
		{
			// The save command will throw an exception if a property has a zero length string.
			// Make sure there is something in the strings to make sure the exception isn't thrown.

			if(newArtist.Length == 0)
				newArtist = "\0";
			if(newCopyright.Length == 0)
				newCopyright = "\0";
			if(newDescription.Length == 0)
				newDescription = "\0";

			writeDescriptionInImage(filename, newArtist, newCopyright, newDescription);
		}

		private static void writeDescriptionInImage(string filename, string newArtist, string newCopyright, string newDescription)
		{
			Image Pic;
			PropertyItem[] PropertyItems;
			byte[] bArtist=new Byte[newArtist.Length];
			byte[] bCopyright=new Byte[newCopyright.Length];
			byte[] bDescription=new Byte[newDescription.Length];
			int i;
			string FilenameTemp;
			Encoder Enc=Encoder.Transformation;
			EncoderParameters EncParms=new EncoderParameters(1);
			EncoderParameter EncParm;
			ImageCodecInfo CodecInfo=GetEncoderInfo("image/jpeg");

			// copy description into byte array
			for (i=0;i<newArtist.Length;i++) bArtist[i]=(byte)newArtist[i];
			for (i=0;i<newCopyright.Length;i++) bCopyright[i]=(byte)newCopyright[i];
			for (i=0;i<newDescription.Length;i++) bDescription[i]=(byte)newDescription[i];

			// load the image to change
			Pic=Image.FromFile(filename);

//			PropertyItem propertyAuthor = CreatePropertyImage();
//			propertyAuthor.Id = 0x013b;
//			propertyAuthor.Type = 2;
//			propertyAuthor.Len = newAuthor.Length;
//			propertyAuthor.Value = bAuthor;
//			Pic.SetPropertyItem(propertyAuthor);
//
//			PropertyItem propertyCopyright = CreatePropertyImage();
//			propertyCopyright.Id = 0x8298;
//			propertyCopyright.Type = 2;
//			propertyCopyright.Len = newCopyright.Length;
//			propertyCopyright.Value = bCopyright;
//			Pic.SetPropertyItem(propertyCopyright);
//
//			PropertyItem propertyDescription = CreatePropertyImage();	
//			propertyDescription.Id = 0x010e;
//			propertyDescription.Type = 2;
//			propertyDescription.Len = newDescription.Length;
//			propertyDescription.Value = bDescription;
//			Pic.SetPropertyItem(propertyDescription);

			// put the new description into the right property item
			PropertyItems=Pic.PropertyItems; 

			PropertyItems[0].Id=0x010e;										// 0x010e as specified in EXIF standard
			PropertyItems[0].Type=2;
			PropertyItems[0].Len=newDescription.Length;
			PropertyItems[0].Value=bDescription;
			Pic.SetPropertyItem(PropertyItems[0]);

			PropertyItems=Pic.PropertyItems; 
			PropertyItems[0].Id=0x8298;										
			PropertyItems[0].Type=2;
			PropertyItems[0].Len=newCopyright.Length;
			PropertyItems[0].Value=bCopyright;
			Pic.SetPropertyItem(PropertyItems[0]);

			PropertyItems=Pic.PropertyItems; 
			PropertyItems[0].Id=0x013b;									
			PropertyItems[0].Type=2;
			PropertyItems[0].Len=newArtist.Length;
			PropertyItems[0].Value=bArtist;
			Pic.SetPropertyItem(PropertyItems[0]);

			PropertyItems=Pic.PropertyItems; 			

			// we cannot store in the same image, so use a temporary image instead
			FilenameTemp=filename+".temp";

			// for lossless rewriting must rotate the image by 90 degrees!
			EncParm=new EncoderParameter(Enc,(long)EncoderValue.TransformRotate90);
			EncParms.Param[0]=EncParm;

			// now write the rotated image with new description
			Pic.Save(FilenameTemp,CodecInfo,EncParms);

			// for computers with low memory and large pictures: release memory now
			Pic.Dispose();
			Pic=null;
			GC.Collect();

			// delete the original file, will be replaced later
			System.IO.File.Delete(filename); 

			// now must rotate back the written picture
			Pic=Image.FromFile(FilenameTemp);
			EncParm=new EncoderParameter(Enc,(long)EncoderValue.TransformRotate270);
			EncParms.Param[0]=EncParm;
			Pic.Save(filename,CodecInfo,EncParms);

			// release memory now
			Pic.Dispose();
			Pic=null;
			GC.Collect();

			// delete the temporary picture
			System.IO.File.Delete(FilenameTemp); 
		}
	}
}