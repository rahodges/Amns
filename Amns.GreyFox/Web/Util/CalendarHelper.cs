using System;
using ComponentArt.Web.UI;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;

namespace Amns.GreyFox.Web.Util
{
	/// <summary>
	/// Summary description for CalendarHelper.
	/// </summary>
	public class CalendarHelper
	{
		public CalendarHelper()
		{
			//
			// TODO: Add constructor logic here
			//
		}

		public static void RegisterCalendarPair(Control parentControl,
			string calendarID,
			out ComponentArt.Web.UI.Calendar picker, 
			out System.Web.UI.HtmlControls.HtmlImage button, 
			out ComponentArt.Web.UI.Calendar calendar, bool encapsulate)
		{
            if (encapsulate)
            {
                parentControl.Controls.Add(new LiteralControl("<table " +
                    "cellspacing=\"0\" cellpadding=\"0\" border=\"0\"><tr><td>"));
            }

			picker = new ComponentArt.Web.UI.Calendar();
			picker.ID = parentControl.ID + "_" + calendarID + "P";
			picker.ControlType = ComponentArt.Web.UI.CalendarControlType.Picker;
			picker.PickerFormat = ComponentArt.Web.UI.DateTimeFormatType.Custom;
			picker.PickerCustomFormat = "MMMM d yyyy";
			picker.MinDate = new DateTime(1800, 1, 1);
			picker.MaxDate = new DateTime(3000, 1, 1);
			picker.SelectedDate = new DateTime(1980, 1, 1);			
			picker.Width = Unit.Pixel(150);
			parentControl.Controls.Add(picker);

            if (encapsulate)
            {
                parentControl.Controls.Add(new LiteralControl("</td><td style=\"font-size:10px;\">&nbsp;</td><td>"));
            }

			button = new HtmlImage();
			button.ID = parentControl.ID + "_" + calendarID + "B";
			parentControl.Controls.Add(button);

            if(encapsulate)
            {
                parentControl.Controls.Add(new LiteralControl("</td></tr></table>"));
            }

			calendar = new ComponentArt.Web.UI.Calendar();
			calendar.ID = parentControl.ID + "_" + calendarID + "C";
            calendar.CssClass = "calendar";
            calendar.TitleCssClass = "calendartitle";
			calendar.ControlType = ComponentArt.Web.UI.CalendarControlType.Calendar;
			calendar.DayCssClass = "day";
			calendar.DayHeaderCssClass = "dayheader";
			calendar.DayHoverCssClass = "dayhover";
			calendar.DayNameFormat = System.Web.UI.WebControls.DayNameFormat.FirstTwoLetters;
			calendar.ImagesBaseUrl = "calendar_images/";
			calendar.MonthCssClass = "month";
			calendar.NextImageUrl = "cal_nextMonth.gif";
			calendar.NextPrevCssClass = "nextprev";
			calendar.OtherMonthDayCssClass = "othermonthday";
			calendar.PopUp = ComponentArt.Web.UI.CalendarPopUpType.Custom;			
			calendar.PrevImageUrl = "cal_prevMonth.gif";
			calendar.MinDate = new DateTime(1800, 1, 1);
			calendar.MaxDate = new DateTime(3000, 1, 1);
			calendar.SelectedDate = new DateTime(1980, 1, 1);
			calendar.SelectedDayCssClass = "selectedday";
			calendar.SelectMonthCssClass = "selector";
			calendar.SelectMonthText = "&curren;";
			calendar.SelectWeekCssClass = "selector";
			calendar.SelectWeekText = "&raquo;";
			calendar.SwapDuration = 300;
			calendar.SwapSlide = ComponentArt.Web.UI.SlideType.Linear;
			parentControl.Controls.Add(calendar);

			button.Alt = "";
			button.Attributes.Add("onclick", calendar.ClientObjectId + ".SetSelectedDate(" + 
				picker.ClientObjectId + ".GetSelectedDate());" + 
				calendar.ClientObjectId + ".Show();");
			button.Attributes.Add("class", "calbutton");
			button.Src = parentControl.Page.ResolveUrl("~/images/btn_calendar.gif");
			button.Width = 25;
			button.Height = 22;

			picker.ClientSideOnSelectionChanged = "on" + picker.ClientObjectId + "Change";
			calendar.ClientSideOnSelectionChanged = "on" + calendar.ClientObjectId + "Change";
			calendar.PopUpExpandControlId = button.ClientID;

			parentControl.Page.ClientScript.RegisterClientScriptBlock(typeof(CalendarHelper), picker.ID, 
				"<script type=\"text/javascript\">" +
				"function on" + picker.ClientObjectId + "Change(picker) {\r\n" +
				calendar.ClientObjectId + ".SetSelectedDate(picker.GetSelectedDate());\r\n" +
				"}\r\n" +
				"function on" + calendar.ClientObjectId + "Change(calendar) {\r\n" +
				picker.ClientObjectId + ".SetSelectedDate(calendar.GetSelectedDate());\r\n" +
				"}\r\n" +
				"</script>\r\n");
		}
	}
}
