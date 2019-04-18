using System;
using ComponentArt.Web.UI;

namespace Amns.GreyFox.Content.Support
{
	/// <summary>
	/// Summary description for CssInfo.
	/// </summary>
	public class CssInfo
	{
		bool _empty					= true;					// marks the cssInfo as empty
		string _default				= string.Empty;			// item
		string _defaultHover		= string.Empty;			// hover
		string _defaultActive		= string.Empty;			// active
		string _select				= string.Empty;			// select
		string _selectHover			= string.Empty;			// selectover
		string _selectActive		= string.Empty;			// selectactive

		public string Default
		{
			get { return _default; }
			set { _default = value; }
		}

		public string DefaultHover
		{
			get { return _defaultHover; }
			set { _defaultHover = value; }
		}

		public string DefaultActive
		{
			get { return _defaultActive; }
			set { _defaultActive = value; }
		}

		public string Select
		{
			get { return _select; }
			set { _select = value; }
		}

		public string SelectHover
		{
			get { return _selectHover; }
			set { _selectHover = value; }
		}

		public string SelectActive
		{
			get { return _selectActive; }
			set { _selectActive = value; }
		}

		public bool IsEmpty
		{
			get { return _empty; }
		}

		public CssInfo(string text)
		{
			if(text != null && text != string.Empty)
			{
				_empty = false;
				string[] vars = text.Split(',', ';');
				int varCount = vars.Length;
				if(varCount > 0)
					_default = vars[0];
				if(varCount > 1)
					_defaultHover = vars[1];
				if(varCount > 2)
					_defaultActive = vars[2];
				if(varCount > 3)
					_select = vars[3];
				if(varCount > 4)
					_selectHover = vars[4];
				if(varCount > 5)
					_selectActive = vars[5];
			}
		}

		public void SetLook(MenuItem item)
		{
			if(_empty)
				return;

            item.Look.CssClass = _default;
			item.Look.HoverCssClass = _defaultHover;
			item.Look.ActiveCssClass = _defaultActive;

			if(_select != string.Empty)
				item.SelectedLook.CssClass = _select;
			else if(_defaultActive != string.Empty)
				item.SelectedLook.CssClass = _defaultActive;
			else
				item.SelectedLook.CssClass = _defaultHover;
			
			if(_selectHover != string.Empty)
                item.SelectedLook.HoverCssClass = _selectHover;
			else
				item.SelectedLook.HoverCssClass = _defaultHover;

			if(_selectActive != string.Empty)
				item.SelectedLook.ActiveCssClass = _selectActive;
			else
				item.SelectedLook.ActiveCssClass = _defaultActive;

			item.TextWrap = true;
		}

		public void SetLook(Menu menu)
		{
			if(_empty)
				return;

			menu.DefaultItemLook.CssClass = _default;
			menu.DefaultItemLook.HoverCssClass = _defaultHover;
			menu.DefaultItemLook.ActiveCssClass = _defaultActive;

			if(_select != string.Empty)
				menu.DefaultSelectedItemLook.CssClass = _select;
			else if(_defaultActive != string.Empty)
				menu.DefaultSelectedItemLook.CssClass = _defaultActive;
			else
				menu.DefaultSelectedItemLook.CssClass = _defaultHover;
			
			if(_selectHover != string.Empty)
				menu.DefaultSelectedItemLook.HoverCssClass = _selectHover;
			else
				menu.DefaultSelectedItemLook.HoverCssClass = _defaultHover;

			if(_selectActive != string.Empty)
				menu.DefaultSelectedItemLook.ActiveCssClass = _selectActive;
			else
				menu.DefaultSelectedItemLook.ActiveCssClass = _defaultActive;

//			menu.DefaultGroupCssClass = _default;
		}
	}
}