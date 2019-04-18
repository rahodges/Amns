using System;
using System.Collections;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Configuration;
using System.Drawing;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.Design;


namespace Amns.GreyFox.Web.UI.WebControls {
	/// <summary>
	/// Use the ComboBox control to create a web control that behaves like a text box and select control. Specify items listings in the control by placing HTML &lt;option&gt; elements between the opening and closing &lt;tns:combobox&gt; tags. Each item is represented by a System.Web.UI.WebControls.ListItem.
	/// Unlike the HtmlSelect control, the ListItem.Value property cannot be used to associate a different value to the combox box.
	/// The Size property specifices the width of the text box in number of characters, much like the HtmlInputText control. If the size is not set, the width of the control will accommodate the largest ListItem.Text property value.
	/// The ComboBox Control implements databiding. The DataSource property specifies the data source to bind to. Use the DataMember property if you're DataSource impements ISourceList, such as the DataSet object, to specify which data member you want bound.
	/// The DataTextField is used to associate the ListItem.Text property.
	/// </summary>
	/// 
	[
	ControlBuilderAttribute(typeof(ComboBoxBuilder)),
	ParseChildren(false),
	ToolboxData(@"<{0}:ComboBox runat=server><asp:ListItem>ComboBox</asp:ListItem></{0}:ComboBox>"),
	ToolboxBitmap(typeof(Amns.GreyFox.Web.UI.WebControls.ComboBox)),
	Designer(typeof(Amns.GreyFox.Web.UI.WebControls.Design.ComboBoxDesigner)),
	]
	public class ComboBox: WebControl, IPostBackDataHandler {
		/// <summary>
		/// Initializes a new instance of the ComboBox class.
		/// </summary>
		public ComboBox() {
			EventServerChange = new Object();
		}
		#region Private Members
		private ListItemCollection items;
		private int cachedSelectedIndex;
		private string _value = String.Empty;
		private int _size = 20;
		private object _datasource = null;
		#endregion

		private object EventServerChange;

		/// <summary>
		/// The ServerChange event is raised when the selected items in the ComboBox control change between posts to the server.
		/// </summary>
		public event EventHandler ServerChange {
			add {this.Events.AddHandler(this.EventServerChange, value);}
			remove {this.Events.RemoveHandler(this.EventServerChange, value);}
		}

		#region Public Properties

		/// <summary>
		/// Gets a collection that contains the items listed in an ComboBox control.
		/// </summary>
		/// 
		[
		DesignerSerializationVisibility(DesignerSerializationVisibility.Content),
		PersistenceMode(PersistenceMode.InnerDefaultProperty),
		Editor(typeof(System.Web.UI.Design.WebControls.ListItemsCollectionEditor), typeof(System.Drawing.Design.UITypeEditor)),
		]
		public ListItemCollection Items {
			get {
				if (this.items == null) {
					this.items = new ListItemCollection();
				}
				return this.items;
			}
		}
		/// <summary>
		/// Gets or sets the zero-based index of the selected tab.
		/// The index is based on the order of list items.
		/// The default is -1, which indicates that nothing is selected 
		/// or the text value does not match any list items.
		/// </summary>
		/// 
		[Browsable(false)]
		public int SelectedIndex {
			get {
				for (int i=0; i< this.Items.Count; i++) {
					if (this.Items[i].Selected)
						return i;
				}
				return -1;
			}
			set {
				if (this.Items.Count == 0) {
					this.cachedSelectedIndex = value;
					return;
				}
				if (value < -1 || value >= this.Items.Count)
					throw new ArgumentOutOfRangeException("value");
				this.ClearSelection();
				if (value >= 0) {
					this.Items[value].Selected = true;
					_value = this.Items[value].Text;
				}
			}
		}

		/// <summary>
		/// Gets or sets the text value.  Also sets the selected index 
		/// if text value matches any list item in the drop down list
		/// </summary>
		/// 
		[Category("Behavior")]
		public string Value {
			get {
				string sValue = this.Attributes["value"];
				if (sValue == null) {
					return String.Empty; 
				}
				return sValue;
			}
			set {
				this.Attributes["value"] = value;
				int index = this.Items.IndexOf(this.Items.FindByText(value));
				if (index >= 0)
					this.SelectedIndex = index;
				else
					this.SelectedIndex = -1;
			}
		}

		/// <summary>
		/// Gets or sets the width size of the text firstName. 
		/// Defaults to match the size of dropdown list.
		/// </summary>
		/// 
		[Category("Appearance")]
		public int Size {
			get {return _size;}
			set {
				_size = value;
				this.Attributes["size"] = _size.ToString();
			}
		}
		/// <summary>
		/// Gets or sets a value indicating whether an automatic postback to the server 
		/// will occur whenever the user changes the selected index.
		/// </summary>
		/// 
		[Category("Behavior")]
		public bool AutoPostBack {
			get {
				Object obj = ViewState["AutoPostBack"];
				return ((obj == null) ? false : (bool)obj);
			}
			set { ViewState["AutoPostBack"] = value; }
		}
		/// <summary>
		/// Gets or sets the source of information to bind to the ComboBox control.
		/// </summary>
		/// 
		[Category("Data")]
		public object DataSource {
			get {return _datasource;}
			set {
				if (value == null || !(value is IListSource) || !(value is IEnumerable)) {
					_datasource = value;
					return; 
				}
				throw new ArgumentException("An invalid data source is being used for " + base.ID + ". A valid data source must implement either IListSource or IEnumerable. ");
			}
		}
		/// <summary>
		/// Gets or sets the firstName from the DataSource to bind to the ListItem.Text 
		/// property of each item in the ComboBox control.
		/// </summary>
		/// 
		[Category("Data")]
		public string DataTextField {
			get {
				string sDataTextField = this.Attributes["DataTextField"];
				if (sDataTextField != null) {
					return sDataTextField;
				}
				return string.Empty;
			}
			set {this.Attributes["DataTextField"] = value;}
		}

		/// <summary>
		/// Gets or sets the set of data to bind to the ComboBox control from a DataSource with multiple sets of data
		/// </summary>
		/// 
		[Category("Data")]
		public string DataMember {
			get {
				string sDataMember = this.Attributes["DataMember"];
				if (sDataMember != null) {
					return ((string) sDataMember); 
				}
				return string.Empty;
			}
			set { this.Attributes["DataMember"] = value;}
		}
		#endregion
		
		#region Public Constants
		/// <summary>
		/// The namespace for the ComboBox and its children.
		/// </summary>
		public const string TagNamespace = "CBNS";

		/// <summary>
		/// The ComboBox's tag name.
		/// </summary>
		public const string ComboBoxTagName = "ComboBox";

		/// <summary>
		/// The ComboBox default client location
		/// </summary>
		public const string DefaultCommonFilesRoot = "/webctrl_client/progstudios/";
		#endregion

		#region Internal Properties
		internal string RenderedNameAttribute {
			get {return this.UniqueID;}
		}

		#endregion

		#region Render Methods
		/// <summary>
		/// This member overrides Control.AddParsedSubObject.
		/// </summary>
		/// <param name="obj"></param>
		protected override void AddParsedSubObject(object obj) {
			if (obj is ListItem) {
				this.Items.Add((ListItem) obj);
				return;
			}
			throw new HttpException("ComboBox Cannot Have Children Of Type " + obj.GetType().Name);
		}
		/// <summary>
		/// This member overrides Control.Render.
		/// </summary>
		/// <param name="writer"></param>
		protected override void Render(HtmlTextWriter writer) {
			this.RenderBeginTag(writer);
			this.RenderChildren(writer);
			this.RenderEndTag(writer);
		}

		/// <summary>
		/// This member supports the .NET Framework infrastructure and is not intended to be used directly from your code.
		/// </summary>
		/// <param name="writer"></param>
		public override void RenderBeginTag(HtmlTextWriter writer) {
			writer.Write("<?XML:NAMESPACE PREFIX=\"" + TagNamespace
				+ "\" />\n<?IMPORT NAMESPACE=\"" + TagNamespace + "\" IMPLEMENTATION=\""
				+ this.GetFilePath() + "combobox.htc" + "\" />");
			writer.WriteLine();
			writer.WriteBeginTag(TagNamespace + ":" + ComboBoxTagName);
			this.RenderAttributes(writer);
			writer.Write(">");
		} 

		/// <summary>
		/// This member overrides Control.RenderChildren.
		/// </summary>
		/// <param name="writer"></param>
		protected override void RenderChildren(HtmlTextWriter writer) {
			ListItem item;
			writer.WriteLine();
			writer.Indent = writer.Indent + 1;
			if (this.Items.Count > 0) {
				for (int i=0; i < this.Items.Count; i++) {
					item = this.Items[i];
					writer.WriteBeginTag("option");
					if (item.Selected) {
						writer.WriteAttribute("selected", "selected");
					}
					writer.WriteAttribute("value", item.Value, true);
					item.Attributes.Remove("text");
					item.Attributes.Remove("value");
					item.Attributes.Remove("selected");
					item.Attributes.Render(writer);
					writer.Write('>');
					HttpUtility.HtmlEncode(item.Text, writer);
					writer.WriteEndTag("option");
					writer.WriteLine();
				} 
			}
			writer.Indent = writer.Indent - 1;
		}

		/// <summary>
		/// This member supports the .NET Framework infrastructure and is not intended to be used directly from your code.
		/// </summary>
		/// <param name="writer"></param>
		protected virtual void RenderAttributes(HtmlTextWriter writer) 
        {
			if(base.ID != null) 
            {
				writer.WriteAttribute("id", base.ClientID);
				writer.WriteAttribute("name", this.UniqueID);
			}

			if(this.AutoPostBack) 
            {
				string sOnChangeCmd = this.Attributes["onchange"];

				if (sOnChangeCmd != null) 
                {
					this.Attributes["onchange"] = (sOnChangeCmd.EndsWith(";")) ? sOnChangeCmd + 
                        Page.ClientScript.GetPostBackEventReference(this, String.Empty)+ ";" : sOnChangeCmd + ";" + 
                        Page.ClientScript.GetPostBackEventReference(this, String.Empty);
				} 
                else 
                {
					this.Attributes.Add("onchange", 
                        Page.ClientScript.GetPostBackEventReference(this, String.Empty));
				}
			}
			this.Attributes.Render(writer);
		} 

		/// <summary>
		/// This member supports the .NET Framework infrastructure and is not intended to be used directly from your code.
		/// </summary>
		/// <param name="writer"></param>
		public override void RenderEndTag(HtmlTextWriter writer) {
			writer.WriteEndTag(TagNamespace + ":" + ComboBoxTagName);
		}
		#endregion

		/// <summary>
		/// Overridden. Creates an EmptyControlCollection to prevent controls from
		/// being added to the ControlCollection.
		/// </summary>
		/// <returns>An EmptyControlCollection object.</returns>
		protected override ControlCollection CreateControlCollection() {
			return new EmptyControlCollection(this);
		}

		
		/// <summary>
		/// This member supports the .NET Framework infrastructure and is not intended to be used directly from your code.
		/// </summary>
		protected virtual void ClearSelection() {
			foreach (ListItem item in this.Items) {
				item.Selected = false;
			}
		}

		/// <summary>
		/// This member supports the .NET Framework infrastructure and is not intended to be used directly from your code.
		/// </summary>
		/// <param name="index"></param>
		protected virtual void Select(int index) {
			if (index < -1 || index >= this.Items.Count)
				throw new ArgumentOutOfRangeException("index");
			else {
				this.ClearSelection();
				this.Items[index].Selected = true;
			}
		}


		#region PostBack Methods
		public virtual void RaisePostDataChangedEvent() {
			this.OnServerChange(EventArgs.Empty);
		}
		public virtual bool LoadPostData(string postDataKey, NameValueCollection postCollection) {
			string sValue = this.Value;
			string sPostedValue = postCollection.GetValues(postDataKey)[0];
			if (!sValue.Equals(sPostedValue)) {
				this.Value = sPostedValue;
				return true; 
			}
			return false;
		}
		protected override void LoadViewState(object savedState) {
			if (savedState != null) {
				object[] State = (object[])savedState;
				this.Value = (string) State[0];
				ArrayList ItemsList = (ArrayList) State[1];
				foreach (object itemText in ItemsList) {
					if (this.Items.FindByText((string)itemText)==null) {
						ListItem item = new ListItem();
						item.Text = (string) itemText;
						this.Items.Add(item);
					}
				}
			}
		}
		protected override object SaveViewState() {
			ArrayList ItemsList = new ArrayList();
			foreach (ListItem item in this.Items) {
				ItemsList.Add(item.Text);
			}
			object[] savedState = new object[2];
			savedState[0] = this.Value;
			savedState[1] = ItemsList;
			return savedState;
		}
		#endregion

		/// <summary>
		/// This member overrides Control.OnPreRender
		/// </summary>
		/// <param name="e"></param>
		protected override void OnPreRender(EventArgs e) {
			if (base.Page != null) {
				base.Page.RegisterRequiresPostBack(this);
			}
		} 

		/// <summary>
		/// Raises the ServerChange event of the ComboBox control. This allows you to provide a custom handler for the event.
		/// </summary>
		/// <param name="e">A System.EventArgs that contains the event data.</param>
		protected virtual void OnServerChange(EventArgs e) {
			EventHandler handler = (EventHandler) this.Events[this.EventServerChange];
			if (handler != null)
				handler.Invoke(this, e);
		}

		/// <summary>
		/// This member overrides Control.OnDataBinding.
		/// </summary>
		/// <param name="e">A System.EventArgs that contains the event data.</param>
		protected override void OnDataBinding(EventArgs e) {
			base.OnDataBinding(e);
			IEnumerable eDataSource = GetResolvedDataSource();
			if (eDataSource != null) {
				this.Items.Clear();
				if (eDataSource is ICollection) {
					this.Items.Capacity = ((ICollection)eDataSource).Count;
				}
				try {
					for(IEnumerator ie = eDataSource.GetEnumerator() ; ie.MoveNext();) {					
						ListItem item = new ListItem();
						if (this.DataTextField.Length > 0) {
							item.Text = DataBinder.GetPropertyValue(ie.Current, this.DataTextField, null);
						} else {
							item.Text = ie.Current.ToString();
						}
						this.Items.Add(item);
					}
				} 
				catch {}
			}
			if (this.cachedSelectedIndex != -1) {
				this.SelectedIndex = this.cachedSelectedIndex;
				this.cachedSelectedIndex = -1;
			}
		}

		#region Private Methods
		private IEnumerable GetResolvedDataSource() {
			if (this.DataSource == null) {
				return null; 
			}
			if (this.DataSource is IListSource) {
				IListSource iListSource = (IListSource)this.DataSource;
				IList iList = iListSource.GetList();
				if (!iListSource.ContainsListCollection) {
					return iList; 
				}
				if (iList != null && iList is ITypedList) {
					ITypedList iTypeList = ((ITypedList) iList);
					PropertyDescriptorCollection collection = iTypeList.GetItemProperties(new PropertyDescriptor[0]);
					if ((collection != null) && (collection.Count != 0)) {
						PropertyDescriptor discriptor = null;
						if ((this.DataMember == null) || (this.DataMember.Length == 0)) {
							discriptor = collection[0];
						}
						else {
							discriptor = collection.Find(this.DataMember, true);
						}
						if (discriptor != null) {
							object key = iList[0];
							object val = discriptor.GetValue(key);
							if ((val != null) && val is IEnumerable) {
								return ((IEnumerable) val); 
							}
						}
						throw new HttpException("The datasource does not contain a member named " + this.DataMember);
					}
					throw new HttpException("Datasource does not contain any members.");
				}
			}
			if (this.DataSource is IEnumerable) {
				return ((IEnumerable) this.DataSource);
			}
			return null;
		}
		private string GetFilePath() {
			// Look at the current configuration for the path
			
			string sFilePath = ConfigurationManager.AppSettings["DHWEBCONTORLS_COMMONFILEPATH"];
			if (sFilePath != null) {
				if (sFilePath.Length > 0) {
					if (sFilePath[sFilePath.Length - 1] != '/') {
						sFilePath += "/";
					}
				}
				return sFilePath;
			}

			// Return the default path with version number
			System.Reflection.Assembly assembly = typeof(ComboBox).Assembly;
			Version version = assembly.GetName().Version;

			return DefaultCommonFilesRoot + version.Major.ToString() + "_" + version.Minor.ToString() + "/";
		}

		#endregion
	}
}
