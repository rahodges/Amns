using System;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;
using System.Web.UI;
using System.ComponentModel;
using System.Collections;
using System.Collections.Specialized;

namespace Amns.GreyFox.WebControls
{
	/// <summary>
	/// This server control allows users to select a valid date and/or time trough selectors.
	/// </summary>
	[ToolboxData("<{0}:DateEditor runat=server></{0}:DateEditor>")]
	public class TimeEditor : System.Web.UI.Control, IPostBackDataHandler
	{
		private const string ScriptKey = "DateEditorScript";
		private bool _renderUplevel, _autoAdjust, _alertEnabled, _monthNamesDisabled;
		private int _minYear, _maxYear;
		private System.Web.UI.WebControls.Style _dateStyle, _timeStyle, _baseStyle;
		private string _format, _alertText;
		private string[] _months;
		private DateTime _oldDate;
		// TODO: Add a tab index to this control
//		private short tabIndex;

		/// <summary>
		/// The default constructor.
		/// </summary>
		public DateEditor()
		{
			_months = new string[] {"January", "February", "March", "April", "May", "June", "July", "August", "September", "October", "November", "December"};
			_format = "MONTH;/;DAY;/;YEAR;-;HOUR;:;MINUTE";
			_minYear = DateTime.Now.Year - 100;
			_maxYear = DateTime.Now.Year + 3;
			_dateStyle = new System.Web.UI.WebControls.Style();
			_timeStyle = new System.Web.UI.WebControls.Style();
			_baseStyle = new System.Web.UI.WebControls.Style();
			_alertText = "The date you selected is not valid and has been reset to the last day in the month.";
		}

		/// <summary>
		/// Register the client side validation script in the ASP page.
		/// </summary>
		protected void RegisterValidatorScript() 
		{
			// Register the script block is not allready done.
			if (!Page.IsClientScriptBlockRegistered(ScriptKey)) 
			{
				string includeScript = @"
				<script language='javascript'>
				function isValid(year, month, day)
				{
					syear = document.getElementById(year);
					smonth = document.getElementById(month);
					sday = document.getElementById(day);

					maximum = maxDays(syear[syear.selectedIndex].value, smonth[smonth.selectedIndex].value, sday[sday.selectedIndex].value);
					
					if (maximum <  sday[sday.selectedIndex].value)
					{
						sday[maximum-1].selected = true;";
				
				if (_alertEnabled)
				{
					includeScript += @"
						alert('" + _alertText + "');";
				}

				includeScript += @"
					}
				}

				// Returns the maximum day number in the specified month. Use leap year calculation.
				function maxDays(year, month, day)
				{
					if (month == 2)
						return (((year % 4 == 0) && (year % 100 != 0)) || (year % 400 == 0)) ? 29 : 28;
					else
						return (month == 4 || month == 6 || month == 9 || month == 11) ? 30 : 31;
				}
				</script>
				";

				// Create client script block.
				Page.RegisterClientScriptBlock(ScriptKey, includeScript);   
			}
		}                  

		/// <summary>
		/// Raises the PreRender event.
		/// </summary>
		/// <remarks>This method notifies the server control to perform any necessary prerendering steps prior to saving view state and rendering content.</remarks>
		/// <param name="e">An <see cref="EventArgs"/> object that contains the event data. </param>
		protected override void OnPreRender(EventArgs e) 
		{
			base.OnPreRender(e);
		
			// Determine if you can or want to use the client side validation.
			_renderUplevel = DetermineRenderUplevel();

			if (_renderUplevel) 
			{
				// Register the client side validation script.
				RegisterValidatorScript();
			}
		}

		/// <summary>
		/// Determine if we need to register the client side validation script.
		/// </summary>
		/// <returns>true if validation is needed; otherwise false.</returns>
		protected virtual bool DetermineRenderUplevel() 
		{
			// Must be on a page.
			Page page = Page;
			if (page == null || page.Request == null) 
				return false;

			// Check whether the client browser has turned off scripting and check
			// browser capabilities. Active DateTime needs the W3C DOM level 1 for
			// control manipulation and at least ECMAScript 1.2.
			return (EnableClientScript 
				//&& page.Request.Browser.W3CDomVersion.Major >= 1
				&& ((page.Request.Browser.Browser.ToUpper().IndexOf("IE") > -1 && page.Request.Browser.MajorVersion >= 4)
					|| (page.Request.Browser.Browser.ToUpper().IndexOf("NETSCAPE") > -1 && page.Request.Browser.MajorVersion >= 6)
					|| (page.Request.Browser.Browser.ToUpper().IndexOf("OPERA") > -1 && page.Request.Browser.MajorVersion >= 3))
				&& page.Request.Browser.EcmaScriptVersion.CompareTo(new Version(1, 2)) >= 0
				&& this._format.ToUpper().IndexOf("DAY") > -1);
		}

		/// <summary>
		/// Sends server control content to a provided HtmlTextWriter object, which writes the content to be rendered on the client.
		/// </summary>
		/// <param name="output">The HtmlTextWriter object that receives the server control content.</param>
		protected override void Render(HtmlTextWriter output)
		{
			base.Render(output);

			// A workaround before finding a solution
			output.AddAttribute(HtmlTextWriterAttribute.Type,"Hidden");
			output.AddAttribute(HtmlTextWriterAttribute.Name,UniqueID);
			output.AddAttribute(HtmlTextWriterAttribute.Value,Date.Ticks.ToString());
			output.RenderBeginTag(HtmlTextWriterTag.Input);
			output.RenderEndTag();

			// Check each element separated by semicolon char in the Format string.
			// If the element is the string representation of a specific date part,
			// the code render the selector. If not, the element is rendered as
			// text to the HtmlTextWriter.
			foreach(string element in Format.Split(';'))
			{
				// TODO: Write out a tabindex

				switch (element.ToUpper())
				{
					case "DAY":
						WriteSelector(output, "_day", DateStyle, "isValid('" + UniqueID + "_year', '" + UniqueID + "_month', '" + UniqueID + "_day')", 1, 31, 2, Date.Day);
						break;
					case "MONTH":
						WriteSelector(output, "_month", DateStyle, "isValid('" + UniqueID + "_year', '" + UniqueID + "_month', '" + UniqueID + "_day')", 1, 12, 2, Date.Month);
						break;
					case "YEAR":
						WriteSelector(output, "_year", DateStyle, "isValid('" + UniqueID + "_year', '" + UniqueID + "_month', '" + UniqueID + "_day')", Date.Year<_minYear && _autoAdjust ? Date.Year : _minYear, Date.Year>_maxYear && _autoAdjust ? Date.Year : _maxYear, 4, Date.Year);
						break;
					case "HOUR":
						WriteSelector(output, "_hour", TimeStyle, null, 0, 23, 2, Date.Hour);
						break;
					case "MINUTE":
						WriteSelector(output, "_minute", TimeStyle, null, 0, 59, 2, Date.Minute);
						break;
					case "SECOND":
						WriteSelector(output, "_second", TimeStyle, null, 0, 59, 2, Date.Second);
						break;
					case "MILLISECOND":
						WriteSelector(output, "_millisecond", TimeStyle, null, 0, 999, 3, Date.Millisecond);
						break;
					default:
						output.Write(element);
						break;
				}
			}
		}

		/// <summary>
		/// This method create a selector based on the parameters.
		/// </summary>
		/// <param name="output">The HtmlTextWriter to write.</param>
		/// <param name="suffix">The suffix to use to identify the selector with the LoadPostData method.</param>
		/// <param name="style">The style class to use.</param>
		/// <param name="onchange">The value of the OnChange attribute of the selector to use with the client side validator.</param>
		/// <param name="min">The minimum value of the selector.</param>
		/// <param name="max">The maximum value of the selector.</param>
		/// <param name="padding">The number of chars to use with padding.</param>
		/// <param name="selectedValue">The selected value.</param>
		private void WriteSelector(HtmlTextWriter output, string suffix, System.Web.UI.WebControls.Style style, string onchange, int min, int max, int padding, int selectedValue)
		{
			// Some variable we will use
			int index;

			// Check if the actual year value can be displayed in the selector
			if (selectedValue < min || selectedValue > max)
				throw new Exception("The year value (" + Date.Year.ToString() + ") of the Date property is greater than the maximum (" + max.ToString() + ") or less than the minimum (" + min.ToString() + "). Please adjust values or set AutoAdjust property to true.");

			// Write the selector
			output.AddAttribute(HtmlTextWriterAttribute.Name, UniqueID + suffix);
						
			// Render the validation action code only if needed
			if(_renderUplevel && onchange != null)
				output.AddAttribute(HtmlTextWriterAttribute.Onchange, onchange);
			
			// Add the styles to the selector after the merge with the base style.
			style.MergeWith(_baseStyle);
			style.AddAttributesToRender(output);

			output.RenderBeginTag(HtmlTextWriterTag.Select);
			
			// Write the option tags
			for(index=min;index<=max;index++)
			{
				output.AddAttribute(HtmlTextWriterAttribute.Value, index.ToString());

				// Set the selected value
				if (index == selectedValue)
					output.AddAttribute(HtmlTextWriterAttribute.Selected, null);

				output.RenderBeginTag(HtmlTextWriterTag.Option);
				if (suffix == "_month" && !_monthNamesDisabled)
					output.InnerWriter.Write(_months[index-1]);
				else
					output.InnerWriter.Write(index.ToString().PadLeft(padding, '0'));
				output.RenderEndTag();
			}

			// Write the selector end tag
			output.RenderEndTag();
		}

		/// <summary>
		/// Processes post back data for an the server control.
		/// </summary>
		/// <param name="postDataKey">The key identifier for the control.</param>
		/// <param name="postCollection">The collection of all incoming name values.</param>
		/// <returns>true if the server control's state changes as a result of the post back; otherwise false.</returns>
		public virtual bool LoadPostData(string postDataKey, NameValueCollection postCollection) 
		{
			int day, month, year, hour, minute, second, millisecond;

			_oldDate = Date;

			day = Convert.ToInt16(postCollection[UniqueID + "_day"]);
			month = Convert.ToInt16(postCollection[UniqueID + "_month"]);
			year = Convert.ToInt16(postCollection[UniqueID + "_year"]);
			hour = Convert.ToInt16(postCollection[UniqueID + "_hour"]);
			minute = Convert.ToInt16(postCollection[UniqueID + "_minute"]);
			second = Convert.ToInt16(postCollection[UniqueID + "_second"]);
			millisecond = Convert.ToInt16(postCollection[UniqueID + "_millisecond"]);

			try
			{
				Date = new DateTime(year == 0 ? DateTime.Now.Year : year, month == 0 ? DateTime.Now.Month : month, day == 0 ? DateTime.Now.Day : day, hour, minute, second, millisecond);
			}
			catch
			{
				throw new Exception("The selectors represent an invalid date. Please use client side validation to prevent this.");
			}

			if (!Date.Equals(_oldDate))
				return true;

			return false;
		}

		/// <summary>
		/// Signals the server control object to notify the ASP.NET application that the state of the control has changed.
		/// </summary>
		public virtual void RaisePostDataChangedEvent() 
		{
			OnDateChanged(new DateChangedEventArgs(_oldDate, Date));
		}

		/// <summary>
		/// Occurs when the Date property value changes.
		/// </summary>
		public event DateChangedEventHandler DateChanged;

		/// <summary>
		/// The DateChanged event handler.
		/// </summary>
		public delegate void DateChangedEventHandler(object sender, DateChangedEventArgs e);

		/// <summary>
		/// Raises the DateChanged event.
		/// </summary>
		/// <param name="e">An <see cref="DateChangedEventArgs"/> that contains the event data.</param>
		protected virtual void OnDateChanged(DateChangedEventArgs e) 
		{
			// Check if someone use our event.
			if (DateChanged != null)
				DateChanged(this,e);
		}

		/// <summary>
		/// Gets or sets a value indicating whether client-side validation is enabled.
		/// </summary>
		[Bindable(true),
		Category("Appearance"),
		Description("Gets or sets a value indicating whether client-side validation is enabled.")]
		public bool EnableClientScript 
		{
			get 
			{
				object o = ViewState["EnableClientScript"];
				return((o == null) ? true : (bool)o);
			}
			set 
			{
				ViewState["EnableClientScript"] = value;
			}
		}

		/// <summary>
		/// Gets or sets the date of the control.
		/// </summary>
		[Bindable(true),
		Category("Data"),
		Description("Gets or sets the date of the control.")]
		public DateTime Date
		{
			get
			{
				if (ViewState["_date"] == null)
					ViewState["_date"] = DateTime.Now;
				return (DateTime)ViewState["_date"];
			}
			set
			{
				ViewState["_date"] = value;
			}
		}

		/// <summary>
		/// Gets or sets the minimum year to display in the year selector.
		/// </summary>
		[Bindable(true),
		Category("Appearance"),
		Description("Gets or sets the minimum year to display in the year selector.")]
		public int MinYear
		{
			get
			{
				return _minYear;
			}
			set
			{
				_minYear = value;
			}
		}

		/// <summary>
		/// Gets or sets the maximum year to display in the year selector.
		/// </summary>
		[Bindable(true),
		Category("Appearance"),
		Description("Gets or sets the maximum year to display in the year selector.")]
		public int MaxYear
		{
			get
			{
				return _maxYear;
			}
			set
			{
				_maxYear = value;
			}
		}

		/// <summary>
		/// Lets you specify if you want the control to show an alert popup when a invalid date is selected.
		/// </summary>
		public bool AlertEnabled
		{
			get
			{
				return _alertEnabled;
			}
			set
			{
				_alertEnabled = value;
			}
		}

		/// <summary>
		/// Set to true is we want to auto adjust the maximum and/or minimum year with the data.
		/// </summary>
		/// <remarks>Setting a high value to <see cref="MaxYear"/> or setting a low value in <see cref="MinYear"/> can produce performance problem.
		/// For each year, more than 20 bytes are added to the browser HTML output.
		/// By setting this property to true, you can prevent from exception throws without having to set very high maximum year or very low minimum year.
		/// If the maximum year value is less than the actual date year value, the maximum year will be adjusted to the actual date year. Same for the minimum year.</remarks>
		[Bindable(true),
		Category("Appearance"),
		Description("Set to true is we want to auto adjust the maximum and/or minimum year with the data.")]
		public bool AutoAdjust
		{
			get
			{
				return _autoAdjust;
			}
			set
			{
				_autoAdjust = value;
			}
		}

		/// <summary>
		/// Gets the style properties of the date and time selectors base.
		/// </summary>
		[Bindable(true),
		Category("Appearance"),
		Description("Gets the style properties of the date and time selectors base.")]
		public System.Web.UI.WebControls.Style BaseStyle
		{
			get
			{
				return _baseStyle;
			}
			set
			{
				_baseStyle = value;
			}
		}

		/// <summary>
		/// Gets the style properties of the date selectors.
		/// </summary>
		[Bindable(true),
		Category("Appearance"),
		Description("Gets the style properties of the date selectors.")]
		public System.Web.UI.WebControls.Style DateStyle
		{
			get
			{
				return _dateStyle;
			}
			set
			{
				_dateStyle = value;
			}
		}

		/// <summary>
		/// Gets or sets the month names.
		/// </summary>
		/// <remarks>
		/// <code>
		/// // This line will replace the january month name by 'Janvier'
		/// MyDate.Months[0] = "Janvier";
		/// 
		/// // This line will replace the december month name by 'Décembre (noel)'
		/// MyDate.Months[11] = "Décembre (noel)";
		/// </code>
		/// </remarks>
		public string[] Months
		{
			get
			{
				return _months;
			}
			set
			{
				_months = value;
			}
		}

		/// <summary>
		/// Sets the month names.
		/// </summary>
		/// <remarks>
		/// This array of string contains the month names. You can set your own month names to match with your culture. If your website is in french, you will prefer use Février in replacment of February.
		/// By default, the months are in english. If the <see cref="MonthNamesDisabled"/> property is set to true, numbers will replace month names.
		/// <code>
		/// // Please verify that MonthNamesDisabled property is not set to true
		/// &lt;AU:ActiveDateTime runat="server" id="CompleteSelector" Format="day;/;month;/;year; ;hour;:;minute;:;second;:;millisecond" SetMonthNames="Janvier,F&amp;eacute;vrier,Mars,Avril,Mai,Juin,Juillet,Ao&amp;ucirc;t,Septembre,Octobre,Novembre,D&amp;eacute;cembre"&gt;&lt;/AU:ActiveDateTime&gt;
		/// </code>
		/// </remarks>
		public string SetMonthNames
		{
			set
			{
				string[] months = value.Split(',');
				int index;

				for(index=0;index<months.Length;index++)
				{
					_months[index] = months[index];
				}
			}
		}
	
		/// <summary>
		/// Specify if you want to display month names or month numbers. False by default.
		/// </summary>
		public bool MonthNamesDisabled
		{
			get
			{
				return _monthNamesDisabled;
			}
			set
			{
				_monthNamesDisabled = value;
			}
		}

		/// <summary>
		/// Gets the style properties of the time selectors.
		/// </summary>
		[Bindable(true),
		Category("Appearance"),
		Description("Gets the style properties of the time selectors.")]
		public System.Web.UI.WebControls.Style TimeStyle
		{
			get
			{
				return _timeStyle;
			}
			set
			{
				_timeStyle = value;
			}
		}

		/// <summary>
		/// Gets or sets the alert text to use when a invalid date is selected.
		/// </summary>
		public string AlertText
		{
			get
			{
				return _alertText;
			}
			set
			{
				_alertText = value;
			}
		}

		/// <summary>
		/// Gets or sets the format to use to render the selectors.
		/// </summary>
		/// <remarks>You can specify the display layout using the <c>format specifiers</c> like <c>"hour"</c> or <c>"month"</c>.<br></br>
		/// Fields must be separated by ; (semicolon) char. Non reconized fields are send to the HtmlTextWriter as literal text.<br></br>
		/// Here are some examples (<c>[</c> and <c>]</c> represent a selector:<br></br><br></br>
		///	<c>"month;/;day;/;year"</c> will display<br></br><c>[MONTH]/[DAY]/[YEAR]</c>.<br></br><br></br>
		///	<c>"hour;:;minute"</c> will display<br></br><c>[HOUR]:[MINUTE]</c>.<br></br><br></br>
		///	<c>"Date : ;day;/;month;/;year; Time : ;hour;:;minute;:;second"</c> will display<br></br><c>Date : [DAY]/[MONTH]/[YEAR] Time : [HOUR]:[MINUTE]:[SECOND]</c>.<br></br><br></br>
		///	<table><tr><td bgcolor="#F0F0F0">Format Specifier</td><td bgcolor="#F0F0F0">Name</td></tr>
		///	<tr><td><b>day</b></td><td>The day part (1 to 31).</td></tr>
		///	<tr><td><b>month</b></td><td>The month part (1 to 12).</td></tr>
		///	<tr><td><b>year</b></td><td>The year part (variable).</td></tr>
		///	<tr><td><b>hour</b></td><td>The hour part (0 to 23).</td></tr>
		///	<tr><td><b>minute</b></td><td>The minute part (0 to 59).</td></tr>
		///	<tr><td><b>second</b></td><td>The second part (0 to 59).</td></tr>
		///	<tr><td><b>millisecond</b></td><td>The millisecond part (0 to 999).</td></tr>
		///	<tr><td><i>other literal</i></td><td>Rendered as literal text.</td></tr>
		///	</table>
		/// </remarks>
		[Bindable(true),
		Category("Appearance"),
		Description("Gets or sets the format to use to render the selectors.")]
		public string Format
		{
			get
			{
				return _format;
			}
			set
			{
				_format = value;
			}
		}
	}
}
