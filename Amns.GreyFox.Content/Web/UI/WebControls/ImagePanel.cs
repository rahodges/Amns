using System;
using System.Collections;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.IO;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.ComponentModel;
using Amns.GreyFox.Imaging;

namespace Amns.GreyFox.Content.Web.UI.WebControls
{
	/// <summary>
	/// Summary description for ImagePanel.
	/// </summary>
	[DefaultProperty("Text"), 
		ToolboxData("<{0}:ImagePanel runat=server></{0}:ImagePanel>")]
	public class ImagePanel : System.Web.UI.WebControls.WebControl
	{
		private bool controlValidated			= false;

		bool enablePreviews						= false;
		string sourceDirectory					= "./root/";
		string previewDirectory					= "./thumbs/";
		bool resetPreviews						= false;

		Unit cellPadding						= 3;
		Unit cellSpacing						= 0;
			
		string pathCssClass						= "iGpath";
		bool enableExifDetails					= true;

		string rootName;
		string imageUrl							= string.Empty;	
				
		int sharpenWeight						= 15;
		string watermarkUrl						= string.Empty;	
		WatermarkPosition watermarkLocation		= WatermarkPosition.BottomRight;
		string borderTopUrl						= string.Empty;	
		string borderLeftUrl					= string.Empty;	
		string borderRightUrl					= string.Empty;	
		string borderBottomUrl					= string.Empty;	
		string borderTopLeftUrl					= string.Empty;	
		string borderTopRightUrl				= string.Empty;
		string borderBottomLeftUrl				= string.Empty;	
		string borderBottomRightUrl				= string.Empty;	
		int	maxPreviewHeight					= 400;
		int maxPreviewWidth						= 400;	
		int previewQuality						= 85;

		#region Preview Properties

		[Bindable(true),
		Category("Preview"),
		DefaultValue(false)]
		public bool EnablePreviews
		{
			get
			{
				return enablePreviews;
			}
			set
			{
				enablePreviews = value;
			}
		}
		
		[Bindable(true),
		Category("Preview"),
		DefaultValue("./thumbs/")]
		public string PreviewDirectory
		{
			get
			{
				return previewDirectory;
			}
			set
			{
				previewDirectory = value;
			}
		}		

		[Bindable(true),
		Category("Preview"),
		DefaultValue(false)]
		public bool ResetPreviews
		{
			get
			{
				return resetPreviews;
			}
			set
			{
				resetPreviews = value;
			}
		}
        
		#endregion

		#region CSS Properties

		private string navLinkCssClass = "ignavlink";
		[Bindable(true), 
		Category("Appearance"), 
		DefaultValue("ignavlink")] 
		public string NavLinkCssClass
		{
			get
			{
				return navLinkCssClass;
			}
			set
			{
				navLinkCssClass = value;
			}
		}

		private string fileNameCssClass = "iGfile";
		[Bindable(true), 
		Category("Appearance"), 
		DefaultValue("iGfile")] 
		public string FileNameCssClass
		{
			get
			{
				return fileNameCssClass;
			}
			set
			{
				fileNameCssClass = value;
			}
		}

		private string imageCssClass = "iGimage";
		[Bindable(true), 
		Category("Appearance"), 
		DefaultValue("iGimage")] 
		public string ImageCssClass
		{
			get
			{
				return imageCssClass;
			}
			set
			{
				imageCssClass = value;
			}
		}

		private string fileDateCssClass = "iGdate";
		[Bindable(true), 
		Category("Appearance"), 
		DefaultValue("iGdate")] 
		public string FileDateCssClass
		{
			get
			{
				return fileDateCssClass;
			}
			set
			{
				fileDateCssClass = value;
			}
		}

		private string exifTagCssClass = "igexiftag";
		[Bindable(true), 
		Category("Appearance"), 
		DefaultValue("igexiftag")] 
		public string ExifTagCssClass
		{
			get
			{
				return exifTagCssClass;
			}
			set
			{
				exifTagCssClass = value;
			}
		}

		private string exifValueCssClass = "igexifvalue";
		[Bindable(true), 
		Category("Appearance"), 
		DefaultValue("igexifvalue")] 
		public string ExifValueCssClass
		{
			get
			{
				return exifValueCssClass;
			}
			set
			{
				exifValueCssClass = value;
			}
		}

		private string exifHeaderCssClass = "iGexifheader";
		[Bindable(true),
		Category("Appearance"),
		DefaultValue("iGexifheader")]
		public string ExifHeaderCssClass
		{
			get
			{
				return exifHeaderCssClass;
			}
			set
			{
				exifHeaderCssClass = value;
			}
		}

		#endregion

		#region Appearance Properties

		[Bindable(true), 
		Category("Appearance"), 
		DefaultValue("3")]
		public Unit CellPadding
		{
			get
			{
				return cellPadding;
			}
			set
			{
				cellPadding = value;
			}
		}

		[Bindable(true), 
		Category("Appearance"), 
		DefaultValue("0")] 
		public Unit CellSpacing
		{
			get
			{
				return cellSpacing;
			}
			set
			{
				cellSpacing = value;
			}
		}

		[Bindable(true), 
		Category("Appearance"), 
		DefaultValue("iGpath")] 
		public string PathCssClass
		{
			get
			{
				return pathCssClass;
			}

			set
			{
				pathCssClass = value;
			}
		}


		[Bindable(true), 
		Category("Appearance"), 
		DefaultValue(true)] 
		public bool EnableExifDetails 
		{
			get
			{
				return enableExifDetails;
			}

			set
			{
				enableExifDetails = value;
			}
		}

		[Bindable(true),
		Category("Appearance"),
		DefaultValue("")]
		public string RootName
		{
			get
			{
				return rootName;
			}
			set
			{
				rootName = value;
			}
		}

		#endregion		

		#region Image Source Properties

		/// <summary>
		/// Root directory to access images. Use this in addition to ImageProtect to protect images on the server from
		/// direct access.
		/// </summary>
		[Bindable(true), 
		Category("Image Source"), 
		DefaultValue("./root/")] 
		public string SourceDirectory 
		{
			get
			{
				return sourceDirectory;
			}

			set
			{
				sourceDirectory = value;
			}
		}

		/// <summary>
		/// Source image url.
		/// </summary>
		[Bindable(true), 
		Category("Image Source"), 
		DefaultValue("")] 
		public string ImageUrl 
		{
			get
			{
				return imageUrl;
			}

			set
			{
				controlValidated = true;
				imageUrl = value;
			}
		}

		/// <summary>
		/// Protects images from direct access by client by disabling the enlargement feature.
		/// </summary>
		private bool imageProtect = true;	
		[Bindable(true), 
		Category("Image Source"), 
		DefaultValue(true)] 
		public bool ImageProtect 
		{
			get
			{
				return imageProtect;
			}

			set
			{
				imageProtect = value;
			}
		}

		#endregion

		#region Preview Post Processing

			#region Sharpening Properties

		[Bindable(true), 
		Category("Preview Post Processing"), 
		DefaultValue(15)] 
		public int SharpenWeight
		{
			get
			{
				return sharpenWeight;
			}

			set
			{
				sharpenWeight = value;
			}
		}

		#endregion

			#region Preview Border Properties

		[Bindable(true), 
		Category("Preview Borders"), 
		DefaultValue("")] 
		public string BorderTopUrl 
		{
			get { return borderTopUrl; }
			set { borderTopUrl = value; }
		}

		[Bindable(true), 
		Category("Preview Borders"), 
		DefaultValue("")] 
		public string BorderLeftUrl 
		{
			get { return borderLeftUrl; }
			set { borderLeftUrl = value; }
		}

		[Bindable(true), 
		Category("Preview Borders"), 
		DefaultValue("")] 
		public string BorderRightUrl 
		{
			get { return borderRightUrl; }
			set { borderRightUrl = value; }
		}

		[Bindable(true), 
		Category("Preview Borders"), 
		DefaultValue("")] 
		public string BorderBottomUrl 
		{
			get { return borderBottomUrl; }
			set { borderBottomUrl = value; }
		}

		[Bindable(true), 
		Category("Preview Borders"), 
		DefaultValue("")] 
		public string BorderTopLeftUrl 
		{
			get { return borderTopLeftUrl; }
			set { borderTopLeftUrl = value; }
		}

		[Bindable(true), 
		Category("Preview Borders"), 
		DefaultValue("")] 
		public string BorderTopRightUrl 
		{
			get { return borderTopRightUrl; }
			set { borderTopRightUrl = value; }
		}

		[Bindable(true), 
		Category("Preview Borders"), 
		DefaultValue("")] 
		public string BorderBottomLeftUrl 
		{
			get { return borderBottomLeftUrl; }
			set { borderBottomLeftUrl = value; }
		}

		[Bindable(true), 
		Category("Preview Borders"), 
		DefaultValue("")] 
		public string BorderBottomRightUrl 
		{
			get { return borderBottomRightUrl; }
			set { borderBottomRightUrl = value; }
		}

		#endregion

			#region Watermark Properties

		/// <summary>
		/// Protects images with a watermark.
		/// </summary>
		[Bindable(true), 
		Category("Preview Post Processing"), 
		DefaultValue("")] 
		public string WatermarkUrl 
		{
			get
			{
				return watermarkUrl;
			}

			set
			{
				watermarkUrl = value;
			}
		}

		/// <summary>
		/// Protects images with a watermark.
		/// </summary>
		[Bindable(true), 
		Category("Preview Post Processing"), 
		DefaultValue("Center")] 
		public WatermarkPosition WatermarkLocation 
		{
			get
			{
				return watermarkLocation;
			}

			set
			{
				watermarkLocation = value;
			}
		}

		#endregion

			#region Size Properties

		[Bindable(true), 
		Category("Preview Size"), 
		DefaultValue(400)] 
		public int MaxPreviewHeight
		{
			get
			{
				return maxPreviewHeight;
			}

			set
			{
				maxPreviewHeight = value;
			}
		}

		[Bindable(true), 
		Category("Preview Size"), 
		DefaultValue(400)] 
		public int MaxPreviewWidth
		{
			get
			{
				return maxPreviewWidth;
			}

			set
			{
				maxPreviewWidth = value;
			}
		}

		#endregion

			#region	Preview Quality Properties

		[Bindable(true),
		Category("Preview"),
		DefaultValue(85)]
		public int PreviewQuality
		{
			get
			{
				return previewQuality;
			}
			set
			{
				previewQuality = value;
			}
		}

		#endregion

		#endregion		

		#region Preview Generator

		private string mapBorder(string url)
		{
			if(url == string.Empty)
				return string.Empty;
			else
				return(HttpContext.Current.Server.MapPath(url));
		}

		/// <summary>
		/// Makes a preview from the file specified and then stores this into the path
		/// specified in thumbPath.
		/// </summary>
		/// <param name="file">File information.</param>
		/// <param name="thumbPath">Preview path.</param>
		/// <returns></returns>
		protected bool makeThumb(FileInfo file, string thumbPath)
		{			
			if(!File.Exists(thumbPath) | this.ResetPreviews)
			{	
				HttpServerUtility server = HttpContext.Current.Server;

				//get the original pictures
				Bitmap imgIn = new Bitmap(file.FullName); // System.Drawing.Image.FromFile(file.FullName);
        
				//set the format to be the same as the original picture
				System.Drawing.Imaging.ImageFormat format = imgIn.RawFormat;
				System.Drawing.Size newSize = new Size(this.MaxPreviewWidth, this.MaxPreviewHeight);

				System.Drawing.Bitmap imgOut = Amns.GreyFox.Imaging.BitmapTransform.ProportionalResize(imgIn, format, newSize);
				
				if(watermarkUrl != string.Empty)
				{
					string watermarkImageUrl = HttpContext.Current.Server.MapPath(watermarkUrl);
					Bitmap wbmp = new Bitmap(watermarkImageUrl);
					ImageWatermark.AddWatermark(imgOut, wbmp, watermarkLocation);
					wbmp.Dispose();
				}

				string borderTop = mapBorder(borderTopUrl);
				string borderLeft = mapBorder(borderLeftUrl);
				string borderRight = mapBorder(borderRightUrl);
				string borderBottom = mapBorder(borderBottomUrl);
				string borderTopLeft = mapBorder(borderTopLeftUrl);
				string borderTopRight = mapBorder(borderTopRightUrl);
				string borderBottomLeft = mapBorder(borderBottomLeftUrl);
				string borderBottomRight = mapBorder(borderBottomRightUrl);

				BorderFilter borderFilter = new BorderFilter(borderTop, borderLeft, borderRight, borderBottom,
					borderTopLeft, borderTopRight, borderBottomLeft, borderBottomRight);		
				borderFilter.ApplyFilter(imgOut);

				Amns.GreyFox.Imaging.ExifProperties.CopyProperties(imgIn, imgOut);

				ImageCodecInfo[] codecArray = ImageCodecInfo.GetImageEncoders();
							
				EncoderParameter[] parameterArray = new EncoderParameter[1];
				parameterArray[0] = new EncoderParameter(Encoder.Quality, previewQuality);
                
				EncoderParameters codecParameters = new EncoderParameters();
				codecParameters.Param = parameterArray;

				imgOut.Save(thumbPath, codecArray[1], codecParameters);

				imgIn.Dispose();
				imgOut.Dispose();

				return true;
			}

			return false;
		}

		#endregion

		protected override void OnLoad(EventArgs e)
		{
			base.OnLoad (e);

			if(Page.IsPostBack)
			{
				string cmd = Page.Request.Form[this.ClientID + "_update"];
				string desc = Page.Request.Form[this.ClientID + "_description"];
				string artist = Page.Request.Form[this.ClientID + "_artist"];
				string copyright = Page.Request.Form[this.ClientID + "_copyright"];

				if(cmd == "Update")
				{
					if(imageUrl == string.Empty)
						throw(new Exception("Empty ImageUrl; cannot update EXIF data."));

					string mappedImageUrl = HttpContext.Current.Server.MapPath(sourceDirectory + "/" + imageUrl);
					ExifProperties.WriteDescriptionInImage(mappedImageUrl, artist, copyright, desc);

					if(this.EnablePreviews)
					{
						FileInfo file = new FileInfo(mappedImageUrl);
						string mappedThumbDirectory = HttpContext.Current.Server.MapPath(previewDirectory);
						string thumbHash = UniqueID + "_" + file.FullName.GetHashCode().ToString() + file.Extension;
						ExifProperties.WriteDescriptionInImage(mappedThumbDirectory + "\\" + thumbHash, artist, copyright, desc);
					}
				}
			}
		}


		/// <summary> 
		/// Render this control to the output parameter specified.
		/// </summary>
		/// <param name="output"> The HTML writer to write out to </param>
		protected override void Render(HtmlTextWriter output)
		{
			if(!controlValidated)
			{
				output.Write("An exception has occured, the URL specified may be incorrect.");
				return;
			}
			if(imageUrl.IndexOf("..") > -1)
			{
				output.Write("Illegal characters in url.");
				return;
			}

			string mappedImageUrl = HttpContext.Current.Server.MapPath(sourceDirectory + "/" + imageUrl);

			if(File.Exists(mappedImageUrl))
			{
				ExifProperties exif;
				FileInfo file = new FileInfo(mappedImageUrl);

				// get the path of the image by removing the filename from the image url
				string imagePath = this.ImageUrl.Replace(file.Name, "");

				if(imagePath.StartsWith("/"))
					imagePath = imagePath.Substring(1, imagePath.Length - 1);
				if(imagePath.EndsWith("/"))
					imagePath = imagePath.Substring(0, imagePath.Length - 1);

				// get the displayed path
				string displayPath = string.Empty;
				if(rootName != string.Empty)
				{
					if(rootName.EndsWith("/"))
						displayPath = rootName.Substring(0, rootName.Length - 1);
					else
						displayPath = rootName;
				}

				displayPath += imagePath;

				// Find Next and Previous Images
				
				string nextImagePath = string.Empty;
				string previousImagePath = string.Empty;
				string extension;
				bool isFound = false;
				FileInfo[] files = file.Directory.GetFiles();
				for(int i = 0; i < files.Length; i++)
				{
					if(file.FullName == files[i].FullName)
					{
						isFound = true;
						continue;
					}

					extension = files[i].Extension.ToLower();

					if(extension == ".gif" | extension == ".jpg" | extension == ".jpeg" | extension == ".png")
					{
						if(!isFound) 
						{
							previousImagePath = imagePath + "/" + files[i].Name;
						}
						else if(isFound)
						{
							nextImagePath = imagePath + "/" + files[i].Name;
							break;
						}
					}
				}

				output.Write("\n<table id=\"{0}\"", UniqueID);
				if(CssClass != "")
					output.Write(" class=\"{0}\"", CssClass);
				if(!cellPadding.IsEmpty)
					output.Write(" cellPadding=\"{0}\"", cellPadding.ToString());
				if(!cellSpacing.IsEmpty)
					output.Write(" cellPadding=\"{0}\"", cellSpacing.ToString());
				if(!BorderWidth.IsEmpty)
					output.Write(" border=\"{0}\"", BorderWidth.ToString());
				if(!Width.IsEmpty)
					output.Write(" width=\"{0}\"", Width.ToString());
				if(!Height.IsEmpty)
					output.Write(" height=\"{0}\"", Height.ToString());
				output.Write(">\n");
				
				output.Indent++;
				output.WriteFullBeginTag("tr");
				output.WriteLine();
				output.Indent++;
				output.WriteBeginTag("td");
				output.WriteAttribute("colspan", "2");
				output.WriteAttribute("class", pathCssClass);
				output.WriteAttribute("width", "100%");
				output.Write(HtmlTextWriter.TagRightChar);
				output.WriteLine();
				output.Indent++;
				output.WriteBeginTag("span");
				output.WriteAttribute("style", "float:right;");
				output.Write(HtmlTextWriter.TagRightChar);
				
				if(Page.Request.UrlReferrer != null)
				{
					output.WriteBeginTag("a");
					output.WriteAttribute("class", this.navLinkCssClass);
					output.WriteAttribute("href", "./?subfolder=" + imagePath);
					output.Write(HtmlTextWriter.TagRightChar);
					output.Write("Back");
					output.WriteEndTag("a");
				}

				if(previousImagePath != string.Empty)
				{
					output.Write(" ");
					output.WriteBeginTag("a");
					output.WriteAttribute("class", this.navLinkCssClass);
					output.WriteAttribute("href", "?src=" + previousImagePath.Replace(" ", "%20"));
					output.Write(HtmlTextWriter.TagRightChar);
					output.Write("Previous");
					output.WriteEndTag("a");
				}
					
				if(nextImagePath != string.Empty)
				{
					output.Write(" ");
					output.WriteBeginTag("a");
					output.WriteAttribute("class", this.navLinkCssClass);
					output.WriteAttribute("href", "?src=" + nextImagePath.Replace(" ", "%20"));
					output.Write(HtmlTextWriter.TagRightChar);
					output.Write("Next");
					output.WriteEndTag("a");
				}

				output.WriteEndTag("span");	
				output.WriteLine(this.RootName + "/" + imagePath);
				output.WriteLine();			
				output.Indent--;
				output.WriteEndTag("td");
				output.Indent--;
				output.WriteEndTag("tr");
				output.WriteLine();

				output.Write("\t<tr>\n");
				output.Write("\t\t<td colspan=\"2\" class=\"{0}\">\n", imageCssClass);

				bool previewUpdate = false;
				
				if(enablePreviews)
				{
					string mappedThumbDirectory = HttpContext.Current.Server.MapPath(previewDirectory);
					string thumbName = UniqueID + "_" + file.FullName.GetHashCode().ToString() + file.Extension;
					string previewUrl = Page.ResolveUrl(previewDirectory + "/" + thumbName);
					output.Write("\t\t\t<img src=\"{0}\" border=\"0\">\n", previewUrl);
					previewUpdate = makeThumb(file, mappedThumbDirectory + "/" + thumbName);
					exif = new ExifProperties(mappedThumbDirectory + "/" + thumbName);
				}
				else
				{
					//					output.Write("\t\t\t<img src=\"{0}\" border=\"0\" width=\"{1}\" height=\"{2}\">\n", imageUrl,  scaleSize.Width, scaleSize.Height);
					output.Write("\t\t\t<img src=\"{0}\" border=\"0\">\n", imageUrl);
					exif = new ExifProperties(mappedImageUrl);
				}
				output.Write("\t\t</td>\n");
				output.Write("\t</tr>\n");

				// Display File Name
				output.Write("\t<tr>\n");
				output.Write("\t\t<td colspan=\"2\" class=\"{0}\">{1}</td>\n", fileNameCssClass, file.Name);
				output.Write("\t</tr>\n");

				// Display File Information				
				renderExifDetail(output, "File Size", string.Empty, (file.Length/1024).ToString(), "KB");
				renderExifDetail(output, "File Updated", string.Empty, file.LastWriteTime.ToString(), string.Empty);
				renderExifDetail(output, "Dimensions", exif.Width.ToString(), " x ", exif.Height.ToString());
				
				if(this.EnablePreviews)
				{
					if(previewUpdate)
						renderExifDetail(output, "Preview", string.Empty, "Updated", string.Empty);
					else
						renderExifDetail(output, "Preview", string.Empty, "Cached", string.Empty);
				}

				// Display EXIF Information
				if(enableExifDetails)
				{
					output.WriteFullBeginTag("tr");
					output.WriteBeginTag("td");
					output.WriteAttribute("colspan", "2");
					output.WriteAttribute("class", exifHeaderCssClass);
					output.WriteLine(HtmlTextWriter.TagRightChar);
					output.Write("EXIF Details");
					output.WriteEndTag("td");
					output.WriteEndTag("tr");
					output.WriteLine();

					renderExifDetail(output, "Description", string.Empty, exif.Description, string.Empty, 3);
					renderExifDetail(output, "Artist", string.Empty, exif.Artist, string.Empty, 1);
					renderExifDetail(output, "Copyright", string.Empty, exif.Copyright, string.Empty, 1);
					renderExifDetail(output, "Software", string.Empty, exif.Software, string.Empty);

					renderExifDetail(output, "Date Picture Taken", string.Empty, exif.DateTimeTaken.ToString(), string.Empty);
					renderExifDetail(output, "Focal Length", string.Empty, exif.FocalLength.ToString(), " mm.");
					renderExifDetail(output, "Exposure Time", string.Empty, exif.ExposureTime.ToString(), string.Empty);
					renderExifDetail(output, "F-Number", "F/", exif.FNumber.ToString(), string.Empty);
					renderExifDetail(output, "ISO Speed Rating", "ISO-", exif.ISOSpeedRating.ToString(), string.Empty);
					renderExifDetail(output, "Manufacturer", string.Empty, exif.Make, string.Empty);
					renderExifDetail(output, "Model", string.Empty, exif.Model, string.Empty);
				}

				if(Page.User.IsInRole("Administrator"))
				{
					output.WriteFullBeginTag("tr");
					output.WriteFullBeginTag("td");
					output.Write("&nbsp;");
					output.WriteEndTag("td");
					output.WriteFullBeginTag("td");
					output.Write("<input type=\"submit\" name=\"" + this.ClientID + "_update\" value=\"Update\">");
					output.WriteEndTag("td");
					output.WriteEndTag("tr");
					output.WriteLine();
				}

				output.Write("</table>");
				output.WriteLine();
			}
			else
			{
				output.Write(mappedImageUrl);
				output.Write("<br />");
				output.Write("File not found!");
			}
		}

		private void renderExifDetail(HtmlTextWriter output, string title, string prefix, string value, string suffix)
		{
			renderExifDetail(output, title, prefix, value, suffix, 0);
		}

		private void renderExifDetail(HtmlTextWriter output, string title, string prefix, string value, string suffix, int updateRows)
		{
			output.WriteFullBeginTag("tr");
			output.WriteBeginTag("td");
			output.WriteAttribute("class", exifTagCssClass);
			output.WriteLine(HtmlTextWriter.TagRightChar);
			output.Write(title);
			output.WriteEndTag("td");
			output.WriteBeginTag("td");
			output.WriteAttribute("class", exifValueCssClass);
			output.WriteLine(HtmlTextWriter.TagRightChar);
			output.Write(prefix);
			
			if(Page.User.IsInRole("Administrator") && updateRows > 0)
			{
				if(updateRows > 1)
				{
					output.WriteBeginTag("textarea");
					output.WriteAttribute("name", this.ClientID + "_" + title.Replace(" ","").ToLower());
					output.WriteAttribute("width", "350px");
					output.WriteAttribute("rows", updateRows.ToString());
					output.Write(HtmlTextWriter.TagRightChar);
					output.Write(value);
					output.WriteEndTag("textarea");
				}
				else if(updateRows == 1)
				{
					output.WriteBeginTag("input");
					output.WriteAttribute("name", this.ClientID + "_" + title.Replace(" ","").ToLower());
					output.WriteAttribute("width", "350px");
					output.WriteAttribute("value", value);
					output.Write(HtmlTextWriter.TagRightChar);
				}
			}
			else
			{
				output.Write(value);
			}
			
			output.Write(suffix);
			output.WriteEndTag("td");
			output.WriteEndTag("tr");
			output.WriteLine();
		}
	}
}