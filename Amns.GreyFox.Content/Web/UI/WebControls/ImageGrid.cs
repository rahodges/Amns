using System;
using System.Collections;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.IO;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web;
using System.ComponentModel;

namespace Amns.GreyFox.Content.Web.UI.WebControls
{
	/// <summary>
	/// Summary description for ImageGrid.
	/// </summary>
	[DefaultProperty("Text"), 
	ToolboxData("<{0}:ImageGrid runat=server></{0}:ImageGrid>")]
	public class ImageGrid : System.Web.UI.WebControls.WebControl
	{
		private enum ImageGridFileType: byte {Directory, Image, Shortcut};

		#region Icon Properties

		private string folderImageUrl;
		[Bindable(true), 
		Category("Icons"), 
		DefaultValue("")] 
		public string FolderImageUrl
		{
			get
			{
				return folderImageUrl;
			}

			set
			{
				folderImageUrl = value;
			}
		}

		private string shortcutImageUrl;
		[Bindable(true), 
		Category("Icons"), 
		DefaultValue("")]
		public string ShortcutImageUrl
		{
			get
			{
				return shortcutImageUrl;
			}
			set
			{
				shortcutImageUrl = value;
			}
		}

		private string upFolderImageUrl;
		[Bindable(true), 
		Category("Icons"), 
		DefaultValue("")] 
		public string UpFolderImageUrl
		{
			get
			{
				return upFolderImageUrl;
			}

			set
			{
				upFolderImageUrl = value;
			}
		}

		private string newImageUrl = string.Empty;
		[Bindable(true),
		Category("Icons"),
		DefaultValue("")]
		public string NewImageUrl
		{
			get
			{
				return newImageUrl;
			}
			set
			{
				newImageUrl = value;
			}
		}

		#endregion

		#region Behavior Properties

		private string rootDirectory;
		[Bindable(true), 
		Category("Behavior"), 
		DefaultValue("")] 
		public string RootDirectory
		{
			get
			{
				return rootDirectory;
			}

			set
			{
				rootDirectory = value;
			}
		}

		private bool enableShortcuts;
		[Bindable(true), 
		Category("Behavior"), 
		DefaultValue("")]
		public bool EnableShortcuts
		{
			get
			{
				return enableShortcuts;
			}
			set
			{
				enableShortcuts = value;
			}
		}



		private bool enableViewer;
		[Bindable(true), 
		Category("Behavior"), 
		DefaultValue("false")]
		public bool EnableViewer
		{
			get
			{
				return enableViewer;
			}
			set
			{
				enableViewer = value;
			}
		}

		private string viewerUrl;
		[Bindable(true), 
		Category("Behavior"), 
		DefaultValue("")] 
		public string ViewerUrl
		{
			get
			{
				return viewerUrl;
			}

			set
			{
				viewerUrl = value;
			}
		}

		private bool enablePopups;
		[Bindable(true), 
		Category("Behavior"), 
		DefaultValue("false")]
		public bool EnablePopups
		{
			get
			{
				return enablePopups;
			}
			set
			{
				enablePopups = value;
			}
		}

		private bool useThumbfiles;
		[Bindable(true), 
		Category("Behavior"), 
		DefaultValue("false")] 
		public bool UseThumbfiles
		{
			get
			{
				return useThumbfiles;
			}

			set
			{
				useThumbfiles = value;
			}
		}

		private string thumbDirectory;
		[Bindable(true), 
		Category("Behavior"), 
		DefaultValue("")] 
		public string ThumbDirectory
		{
			get
			{
				return thumbDirectory;
			}

			set
			{
				thumbDirectory = value;
			}
		}

		private string thumbGenPath;
		[Bindable(true), 
		Category("Behavior"), 
		DefaultValue("")] 
		public string ThumbGenPath
		{
			get
			{
				return thumbGenPath;
			}

			set
			{
				thumbGenPath = value;
			}
		}



		#endregion

		#region Thumbnail Generator Properties

		private bool resetThumbnails;
		[Bindable(true), 
		Category("Thumb Generator"), 
		DefaultValue("False")] 
		public bool ResetThumbnails
		{
			get
			{
				return resetThumbnails;
			}

			set
			{
				resetThumbnails = value;
			}
		}

		private int thumbHeight;
		[Bindable(true), 
		Category("Thumb Generator"), 
		DefaultValue("")] 
		public int ThumbHeight
		{
			get
			{
				return thumbHeight;
			}

			set
			{
				thumbHeight = value;
			}
		}

		private int thumbWidth;
		[Bindable(true), 
		Category("Thumb Generator"), 
		DefaultValue("")] 
		public int ThumbWidth
		{
			get
			{
				return thumbWidth;
			}

			set
			{
				thumbWidth = value;
			}
		}

		private int thumbnailQuality = 80;
		[Bindable(true),
		Category("Thumb Generator"),
		DefaultValue("80")]
		public int ThumbnailQuality
		{
			get
			{
				return thumbnailQuality;
			}
			set
			{
				thumbnailQuality = value;
			}
		}

		#endregion

		#region Appearance Properties

		private Unit detailCellHeight = Unit.Pixel(25);
		[Bindable(true), 
		Category("Appearance"), 
		DefaultValue("25px")] 
		public Unit DetailCellHeight
		{
			get
			{
				return detailCellHeight;
			}
			set
			{
				detailCellHeight = value;
			}
		}

		private Unit iconCellHeight = Unit.Pixel(100);
		[Bindable(true), 
		Category("Appearance"), 
		DefaultValue("100px")] 
		public Unit IconCellHeight
		{
			get
			{
				return iconCellHeight;
			}
			set
			{
				iconCellHeight = value;
			}
		}

		private int columnCount;
		[Bindable(true), 
		Category("Appearance"), 
		DefaultValue("")] 
		public int ColumnCount
		{
			get
			{
				return columnCount;
			}
			set
			{
				columnCount = value;
			}
		}

		private int rowCount;
		[Bindable(true), 
		Category("Appearance"), 
		DefaultValue("")] 
		public int RowCount
		{
			get
			{
				return rowCount;
			}
			set
			{
				rowCount = value;
			}
		}

		private Unit cellPadding;
		[Bindable(true), 
		Category("Appearance"), 
		DefaultValue("")] 
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

		private Unit cellSpacing;
		[Bindable(true), 
		Category("Appearance"), 
		DefaultValue("")] 
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



		private TimeSpan newImageTime;
		[Bindable(true),
		Category("Appearance"),
		DefaultValue("")]
		public TimeSpan NewImageTime
		{
			get
			{
				return newImageTime;
			}
			set
			{
				newImageTime = value;
			}
		}


		private bool enableNewGraphic;
		[Bindable(true),
		Category("Appearance"),
		DefaultValue("")]
		public bool EnableNewGraphic
		{
			get
			{
				return enableNewGraphic;
			}
			set
			{
				enableNewGraphic = value;
			}
		}

		private string pathCssClass = "iGpath";
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


		private string filesizeCssClass = "iGfilesize";
		[Bindable(true), 
		Category("Appearance"), 
		DefaultValue("igSize")] 
		public string FilesizeCssClass
		{
			get
			{
				return filesizeCssClass;
			}

			set
			{
				filesizeCssClass = value;
			}
		}

		private string filenameCssClass = "iGfilename";
		[Bindable(true), 
		Category("Appearance"), 
		DefaultValue("iGfilename")] 
		public string FilenameCssClass
		{
			get
			{
				return filenameCssClass;
			}

			set
			{
				filenameCssClass = value;
			}
		}

		private string foldernameCssClass = "iGfoldername";
		[Bindable(true), 
		Category("Appearance"), 
		DefaultValue("iGfoldername")] 
		public string FoldernameCssClass
		{
			get
			{
				return foldernameCssClass;
			}

			set
			{
				foldernameCssClass = value;
			}
		}

		private string folderdateCssClass = "iGfolderdate";
		[Bindable(true), 
		Category("Appearance"), 
		DefaultValue("iGfolderdate")] 
		public string FolderdateCssClass
		{
			get
			{
				return folderdateCssClass;
			}

			set
			{
				folderdateCssClass = value;
			}
		}

		private string folderCssClass = "iGfolder";
		[Bindable(true), 
		Category("Appearance"), 
		DefaultValue("iGfolder")] 
		public string FolderCssClass
		{
			get
			{
				return folderCssClass;
			}

			set
			{
				folderCssClass = value;
			}
		}

		private string fileCssClass = "iGfile";
		[Bindable(true), 
		Category("Appearance"), 
		DefaultValue("iGfile")] 
		public string FileCssClass
		{
			get
			{
				return fileCssClass;
			}

			set
			{
				fileCssClass = value;
			}
		}

        private string rootName;
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

		private bool enableFileNames;
		[Bindable(true), 
		Category("Appearance"), 
		DefaultValue("false")] 
		public bool EnableFileNames
		{
			get
			{
				return enableFileNames;
			}

			set
			{
				enableFileNames = value;
			}
		}

		private bool enableFileSizes;
		[Bindable(true), 
		Category("Appearance"), 
		DefaultValue("false")] 
		public bool EnableFileSizes
		{
			get
			{
				return enableFileSizes;
			}

			set
			{
				enableFileSizes = value;
			}
		}

		#endregion

		#region Control Properties

		private bool IsRoot
		{
			get
			{
				if(subFolder != "")
					return false;
				return true;
			}
		}

		#endregion
		
		#region Thumbnail Generator

		/// <summary>
		/// Makes a thumbnail from the file specified and then stores this into the path
		/// specified in thumbPath.
		/// </summary>
		/// <param name="file">File information.</param>
		/// <param name="thumbPath">Thumbnail path.</param>
		/// <returns></returns>
		protected bool makeThumb(FileInfo file, string thumbPath)
		{			
			if(!File.Exists(thumbPath) | resetThumbnails)
			{						
				//get the original pictures
				Bitmap imgIn = new Bitmap(file.FullName); // System.Drawing.Image.FromFile(file.FullName);
        
				//set the format to be the same as the original picture
				System.Drawing.Imaging.ImageFormat format = imgIn.RawFormat;
				System.Drawing.Size newSize = new Size(this.ThumbWidth, this.ThumbHeight);

				System.Drawing.Image imgOut = Amns.GreyFox.Imaging.BitmapTransform.ProportionalResize(imgIn, format, newSize);

				Amns.GreyFox.Imaging.ExifProperties.CopyProperties(imgIn, imgOut);

				ImageCodecInfo[] codecArray = ImageCodecInfo.GetImageEncoders();
							
				EncoderParameter[] parameterArray = new EncoderParameter[1];
				parameterArray[0] = new EncoderParameter(Encoder.Quality, thumbnailQuality);
                
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

		private string parentFolder = string.Empty;
		private string subFolder;
		private string errorMessage = string.Empty;
		string fadeCss = string.Empty;

		private string getShortcut(string fileName)
		{
			Configuration.IniConfiguration conf = new Configuration.IniConfiguration(new StreamReader(fileName));
			return conf.GetValue("InternetShortcut", "URL");
		}

		protected override void OnLoad(EventArgs e)
		{
			base.OnLoad(e);

			if(HttpContext.Current.Request.QueryString["subfolder"] != null)
			{
				subFolder = HttpContext.Current.Request.QueryString["subfolder"];
				if(subFolder.IndexOf("..") > 0 | subFolder.IndexOf(".") > 0)
					errorMessage = "Cannot use '..' or '.' in subfolder query!";

				if(subFolder.IndexOf('/') > 0)
					parentFolder = subFolder.Substring(0, subFolder.LastIndexOf('/'));
			}
			else
				subFolder = string.Empty;
				
//			if(HttpContext.Current.Request.Browser.MajorVersion >= 6)
//			{
//				fadeCss = "style=\"filter:alpha(opacity=50);-moz-opacity:0.5\" onMouseover=\"makevisible(this,0)\" onMouseout=\"makevisible(this,1)\"";
//				string fadeScript = "\r\t<script language=\"JavaScript1.2\">\r" +
//					"\tfunction makevisible(cur,which){ \r" +
//					"\tstrength=(which==0)? 1 : 0.5 \r" +
//					"\tif (cur.style.MozOpacity) \r" +
//					"\tcur.style.MozOpacity=strength \r" +
//					"\telse if (cur.filters) \r" +
//					"\tcur.filters.alpha.opacity=strength*100 \r" +
//					"\t} \r" +
//					"\t</script> \r";
//            
//				Page.ClientScript.RegisterClientScriptBlock(this.GetType(), "fadescript", fadeScript);
//			}
		}

		#region Render Apply

		/// <summary> 
		/// Render this control to the output parameter specified.
		/// </summary>
		/// <param name="output"> The HTML writer to write out to </param>
		protected override void Render(HtmlTextWriter output)
		{
//			output.Write(subFolder);
//			output.WriteFullBeginTag("br");
//			output.Write(parentFolder);
//			output.WriteFullBeginTag("br");
//			output.Write(rootDirectory);
//			output.WriteFullBeginTag("br");

			if(!Enabled)
				return;

			if(errorMessage != string.Empty)
			{
				output.WriteLine(errorMessage);
				return;
			}
			
			EnsureChildControls();
			string tempPath;
		
			if(subFolder == string.Empty)
				tempPath = rootDirectory;
			else
				tempPath = rootDirectory + "/" + subFolder;

			FileInfo file;
			DirectoryInfo directory;

			string[] files;
			string[] directories;

			// This creates an array of files containing the filenames of all the files in your picture directroy
			
			try
			{
				directories = Directory.GetDirectories(Context.Server.MapPath(tempPath), "*.*");
				files = Directory.GetFiles(Context.Server.MapPath(tempPath), "*.*");
			}
			catch
			{
				output.Write("Cannot find the subfolder specified in query!");
				return;
			}

			int totalElements = files.Length + directories.Length;

			if(columnCount <= 0)
				columnCount = 3;
			if(rowCount <= 0)
				rowCount = 5;
			int colWidth = 100 / columnCount;

			output.WriteLine();
			output.Indent++;

			output.WriteBeginTag("table");
			output.WriteAttribute("id", UniqueID);
            if(CssClass != "")
				output.WriteAttribute("class", CssClass);
			if(!cellPadding.IsEmpty)
				output.WriteAttribute("cellPadding", cellPadding.ToString());
			if(!cellSpacing.IsEmpty)
				output.WriteAttribute("cellSpacing", cellSpacing.ToString());
			if(!BorderWidth.IsEmpty)
				output.WriteAttribute("border", BorderWidth.ToString());
			if(!Width.IsEmpty)
				output.WriteAttribute("width", Width.ToString());
			if(!Height.IsEmpty)
				output.WriteAttribute("height", Height.ToString());
			output.Write(HtmlTextWriter.TagRightChar);
			output.WriteLine();

			output.Indent++;
			output.WriteFullBeginTag("tr");
			output.WriteLine();

			output.Indent++;
			output.WriteBeginTag("td");
			output.WriteAttribute("class", pathCssClass);
			output.WriteAttribute("colspan", columnCount.ToString());
            output.WriteAttribute("width", "100%");
			output.Write(HtmlTextWriter.TagRightChar);
			output.WriteLine();

			output.Indent++;
			output.Write(rootName);
			
			if(!IsRoot)
				output.Write(string.Format("/{0}", subFolder));
			output.WriteLine();
			
			output.Indent--;
			output.WriteEndTag("td");
			output.WriteLine();
			
			output.Indent--;
			output.WriteEndTag("tr");
			output.WriteLine();

			//We want to loop through this created array and add only picture files to our Hashtable
			int i = 0;
			int x = 1;
			string thumbName;
			string mappedThumbDirectory = HttpContext.Current.Server.MapPath(thumbDirectory);
			string filenameWoExtension;			
			string colWidthFormatted = string.Format("{0}%", colWidth);

			string[] detailsBuffer = new string[columnCount+1];
			ImageGridFileType[] detailType = new ImageGridFileType[columnCount+1];
            
			for(int r = 1; r <= rowCount; r++)
			{
				output.WriteFullBeginTag("tr");
				output.WriteLine();
				
				// Output the up folder if this directory is a subdirectory of the root
				if(!IsRoot & r == 1)
				{
					output.Indent++;
					output.WriteBeginTag("td");
					output.WriteAttribute("class", folderCssClass);
					output.WriteAttribute("width", colWidthFormatted);
					output.WriteAttribute("height", iconCellHeight.ToString());
					output.WriteLine(HtmlTextWriter.TagRightChar);

					output.Indent++;
					output.WriteBeginTag("a");
//					if(parentFolder != "")
                        output.WriteAttribute("href", string.Format("?subfolder={0}", parentFolder.Replace(" ", "%20")));
//					else
//						output.WriteAttribute("href", "?subfolder=");

					output.WriteLine(HtmlTextWriter.TagRightChar);

					output.WriteBeginTag("img");
					output.WriteAttribute("src", Page.ResolveUrl(upFolderImageUrl));
					output.WriteAttribute("border", "0");
					output.Write(HtmlTextWriter.TagRightChar);
					output.WriteEndTag("a");
					

					// FILL DETAILS BUFFER WITH DIRECTORY DETAILS
					// ------------------------------------------
					detailsBuffer[1] = string.Format("<span class=\"{0}\">..</span>\n",
						foldernameCssClass);

					output.Indent--;
					
					output.WriteEndTag("td");
					output.WriteLine();

					output.Indent--;
					
					// Increment cell counter
					x = 2;
				}

				while(x <= columnCount)
				{				
					if(i < directories.Length)
					{
						directory = new DirectoryInfo(directories[i]);
						i++;

						// Do not display thumbnail directories or directories starting with an underscore
						// for FrontPage Webs.
						if(directory.Name != this.thumbDirectory & !directory.Name.StartsWith("_"))
						{
							output.Indent++;
							output.WriteBeginTag("td");
							output.WriteAttribute("class", folderCssClass);
							output.WriteAttribute("height", iconCellHeight.ToString());
							output.WriteAttribute("width", colWidthFormatted);
							output.WriteLine(HtmlTextWriter.TagRightChar);
							
							output.Indent++;
							output.WriteBeginTag("a");
							if(IsRoot)
								output.WriteAttribute("href",
									string.Format("?subfolder={0}", directory.Name.Replace(" ", "%20")));
							else
								output.WriteAttribute("href",
									string.Format("?subfolder={0}/{1}",
									subFolder.Replace(" ", "%20"), directory.Name.Replace(" ", "%20")));
							output.Write(HtmlTextWriter.TagRightChar);

							output.WriteLine();
							output.WriteBeginTag("img");
							output.WriteAttribute("src", Page.ResolveUrl(folderImageUrl));
							output.WriteAttribute("border", "0");
							output.Write(HtmlTextWriter.TagRightChar);
							output.WriteEndTag("a");
							output.WriteLine();

							// FILL DETAILS BUFFER WITH DIRECTORY DETAILS
							// ------------------------------------------
							detailsBuffer[x] = string.Format("<span class=\"{0}\">{1}</span>",
								foldernameCssClass, directory.Name);
							detailsBuffer[x] += string.Format("<br><span class=\"{0}\">{1}</span>",
								folderdateCssClass, directory.LastWriteTime.ToShortDateString());

							output.WriteLine();
							output.Indent--;

							output.WriteEndTag("td");
							output.WriteLine();
							output.Indent--;

							x++;
						}						
					}
					else if(i < totalElements)
					{						
						while(i < totalElements)
						{	
							detailsBuffer[x] = string.Empty;

							file = new FileInfo(files[i - directories.Length]);
							filenameWoExtension = file.Name.Substring(0, file.Name.Length - file.Extension.Length);

							i++;

							if(this.enableShortcuts && file.Extension.ToLower() == ".url")
							{
								output.Indent++;
								output.WriteBeginTag("td");
								output.WriteAttribute("class", fileCssClass);
								output.WriteAttribute("height", iconCellHeight.ToString());
								output.WriteAttribute("width", string.Format("{0}%", colWidth.ToString()));
								output.WriteLine(HtmlTextWriter.TagRightChar);

								output.Indent++;
                                output.WriteBeginTag("a");
								output.WriteAttribute("href", getShortcut(file.FullName));
								output.WriteLine(HtmlTextWriter.TagRightChar);

								output.WriteBeginTag("img");
								output.WriteAttribute("src", Page.ResolveUrl(shortcutImageUrl));
								output.WriteAttribute("border", "0");
								output.Write(HtmlTextWriter.TagRightChar);
								output.WriteEndTag("a");
								output.WriteLine();

								// FILL DETAILS BUFFER WITH SHORTCUT DETAILS
								// -------------------------------------
								detailsBuffer[x] = string.Format("{0}<br>{1}",
									filenameWoExtension, file.LastWriteTime.ToShortDateString());
								
								output.Indent--;
                                output.WriteEndTag("td");
								output.WriteLine();
								output.Indent--;
								
								x++;
								break;
							}
							else if(file.Extension.ToLower() == ".jpg" | file.Extension.ToLower() == ".gif")
							{
								output.Indent++;
								output.WriteBeginTag("td");
								output.WriteAttribute("class", fileCssClass);
								output.WriteAttribute("height", iconCellHeight.ToString());
								output.WriteAttribute("width", string.Format("{0}%", colWidth.ToString()));
								output.WriteLine(HtmlTextWriter.TagRightChar);

								output.Indent++;
								output.WriteBeginTag("a");

								if(enablePopups)
									output.WriteAttribute("href", 
										string.Format("javascript:window.open('ImageViewer.aspx?src={0}/{1}&alt={2}', '', 'status=yes'); void('');", 
										tempPath, file.Name.Replace(" ", "%20").Replace(this.RootDirectory, ""), file.Name));
								else if(enableViewer)
									output.WriteAttribute("href",
										string.Format("{0}?src={1}/{2}",
										viewerUrl, tempPath.Replace(" ", "%20").Replace(this.RootDirectory, ""), file.Name.Replace(" ", "%20").Replace(this.RootDirectory, "")));
								else
									output.WriteAttribute("href",
										string.Format("{0}/{1}",
										tempPath, file.Name.Replace(" ", "%20").Replace(this.RootDirectory, "")));

								output.Write(HtmlTextWriter.TagRightChar);
								
								if(this.useThumbfiles)
								{
									thumbName = UniqueID + "_" + file.FullName.GetHashCode().ToString() + file.Extension;
									output.WriteLine();
									output.WriteBeginTag("img");
									output.WriteAttribute("src", 
										string.Format("{0}/{1}", Page.ResolveUrl(thumbDirectory), thumbName));
									output.WriteAttribute("border", "0");
									if(fadeCss != string.Empty)
										output.WriteAttribute("class", fadeCss);
									output.Write(HtmlTextWriter.TagRightChar);
									output.WriteEndTag("a");
									
									if(makeThumb(file, mappedThumbDirectory + "/" + thumbName))
										output.Write("<br>thumbnail updated");									
								}
								else
								{
									if(IsRoot)
										output.Write("\n\t\t\t<img src=\"{0}?src=/{1}/{2}\" border=\"0\"></a>\n", thumbGenPath, rootDirectory, file.Name);
									else
										output.Write("\n\t\t\t<img src=\"{0}?src=/{1}/{2}/{3}\" border=\"0\"></a>\n", thumbGenPath, rootDirectory, subFolder, file.Name);
									output.WriteEndTag("a");
								}
							
								// FILL DETAILS BUFFER WITH FILE DETAILS
								// -------------------------------------
								if(enableFileNames)
								{
									detailsBuffer[x] += string.Format("<span class=\"{0}\">{1}</span>",
										filenameCssClass, filenameWoExtension);
								}
								if(enableFileSizes)
								{
									detailsBuffer[x] += string.Format("<br><span class=\"{0}\">{1}KB</span>",
										filesizeCssClass, file.Length / 1024);
								}
								if(enableNewGraphic && DateTime.Now - file.LastWriteTime < newImageTime)
								{
									if(newImageUrl != string.Empty)
										detailsBuffer[x] += string.Format("<br><img src=\"{0}\" border =\"0\" />", newImageUrl);
									else
										detailsBuffer[x] += string.Format("<br><strong>NEW</strong>");									
								}

								output.WriteLine();
								output.Indent--;

								output.WriteEndTag("td");
								output.WriteLine();
								output.Indent--;
								x++;
								break;
							}
						}
					}
					else
					{
						detailsBuffer[x] = "&nbsp; ";

						output.Indent++;
						output.WriteBeginTag("td");
						output.WriteAttribute("height", iconCellHeight.ToString());
						output.WriteAttribute("width", string.Format("{0}%", colWidth));
						output.Write(HtmlTextWriter.TagRightChar);
						output.Write("&nbsp; ");
						output.WriteEndTag("td");
						output.WriteLine();
						output.Indent--;

						detailsBuffer[x] = "&nbsp; ";
						
						x++;
					}
				}
				
				output.WriteEndTag("tr");
				output.WriteLine();
				//output.Indent--;

				// RENDER DETAILS BUFFER ROW
				// ------------------------------------------------------------
				output.WriteFullBeginTag("tr");
				output.WriteLine();
				output.Indent++;

				for(x = 1; x <= columnCount; x++)
				{
					output.WriteBeginTag("td");
					output.WriteAttribute("height", detailCellHeight.ToString());
					output.WriteAttribute("valign", "top");

					switch(detailType[x])
					{
						case ImageGridFileType.Directory:
							output.WriteAttribute("class", this.folderCssClass);
							break;
						default:
							output.WriteAttribute("class", this.fileCssClass);
							break;
					}
					output.WriteLine(HtmlTextWriter.TagRightChar);

					output.Indent++;
					output.Write(detailsBuffer[x]);
					output.WriteLine();
					output.Indent--;

					output.WriteEndTag("td");
					output.WriteLine();
				}
				output.Indent--;
				output.WriteEndTag("tr");
				output.WriteLine();
				
				// If the total elements are exceeded, break the loop
				if(i >= totalElements)
					break;

				x = 1;
			}
			

			output.Indent--;
			output.WriteEndTag("table");
		}

		#endregion

		#region ViewState Methods

		protected override void LoadViewState(object savedState) 
		{
			if(savedState != null)
			{
				// Load State from the array of objects that was saved at ;
				// SavedViewState.
				object[] myState = (object[])savedState;
				if (myState[0] != null)
					base.LoadViewState(myState[0]);
				if (myState[1] != null)
					subFolder = (string) myState[1];
			}
		}

		protected override object SaveViewState()
		{			
			object baseState = base.SaveViewState();
			object[] myState = new object[2];
			myState[0] = baseState;
			myState[1] = subFolder;
			return myState;
		}

		#endregion
	}
}