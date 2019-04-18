using System;
using System.Web.UI.WebControls;
using ComponentArt.Web.UI;

namespace Amns.GreyFox.Content.Support
{
	/// <summary>
	/// Summary description for IconDescription.
	/// </summary>
	public class IconInfo
	{
		bool _empty					= true;

		string _defaultUrl			= string.Empty;			// item
		string _defaultHoverUrl		= string.Empty;			// hover
		string _defaultActiveUrl	= string.Empty;			// active
		string _selectUrl			= string.Empty;			// select
		string _selectHoverUrl		= string.Empty;			// selectover
		string _selectActiveUrl		= string.Empty;			// selectactive

		Unit _width			= Unit.Pixel(15);
		Unit _height		= Unit.Pixel(15);

		public string DefaultUrl
		{
			get { return _defaultUrl; }
			set { _defaultUrl = value; }
		}

		public string DefaultHover
		{
			get { return _defaultHoverUrl; }
			set { _defaultHoverUrl = value; }
		}

		public string DefaultActive
		{
			get { return _defaultActiveUrl; }
			set { _defaultActiveUrl = value; }
		}

		public string SelectUrl
		{
			get { return _selectUrl; }
			set { _selectUrl = value; }
		}

		public string SelectHoverUrl
		{
			get { return _selectHoverUrl; }
			set { _selectHoverUrl = value; }
		}

		public string SelectActiveUrl
		{
			get { return _selectActiveUrl; }
			set { _selectActiveUrl = value; }
		}

		public bool IsEmpty
		{
			get { return _empty; }
		}

		public Unit Width
		{
			get { return _width; }
			set { _width = value; }
		}

		public Unit Height
		{
			get { return _height; }
			set { _height = value; }
		}

		public IconInfo(string text)
		{
			if(text != null && text != string.Empty)
			{
				_empty = false;
				string[] vars = text.Split(',', ';');
				int varCount = vars.Length;
				if(varCount > 0)
					_defaultUrl = vars[0];
				if(varCount > 1)
					_width = Unit.Parse(vars[1]);
				if(varCount > 2)
					_height = Unit.Parse(vars[2]);
				if(varCount > 3)
					_defaultHoverUrl = vars[3];
				if(varCount > 4)
					_defaultActiveUrl = vars[4];
				if(varCount > 5)
					_selectUrl = vars[5];
				if(varCount > 6)
					_selectHoverUrl = vars[6];
				if(varCount > 7)
					_selectActiveUrl = vars[7];
			}
		}

		public void SetLeftLook(ComponentArt.Web.UI.MenuItem item)
		{
			if(_empty)
				return;

			item.Look.LeftIconUrl = _defaultUrl;
			item.Look.LeftIconHeight = _height;
			item.Look.LeftIconWidth = _width;
			item.Look.HoverLeftIconUrl = _defaultHoverUrl;
			item.Look.ActiveLeftIconUrl = _defaultActiveUrl;
            

			if(_selectUrl != string.Empty)
                item.SelectedLook.LeftIconUrl = _selectUrl;
			else
				item.SelectedLook.LeftIconUrl = _defaultUrl;

			item.SelectedLook.LeftIconWidth = _width;
			item.SelectedLook.LeftIconHeight = _height;

			if(_selectHoverUrl != string.Empty)
				item.SelectedLook.HoverLeftIconUrl = _selectHoverUrl;
			else
				item.SelectedLook.HoverLeftIconUrl = _defaultHoverUrl;

			if(_selectActiveUrl != string.Empty)
				item.SelectedLook.ActiveLeftIconUrl = _selectActiveUrl;
			else
				item.SelectedLook.ActiveLeftIconUrl = _defaultActiveUrl;
		}

		public void SetRightLook(ComponentArt.Web.UI.MenuItem item)
		{
			if(_empty)
				return;

			item.Look.RightIconUrl = _defaultUrl;
			item.Look.RightIconHeight = _height;
			item.Look.RightIconWidth = _width;
			item.Look.HoverRightIconUrl = _defaultHoverUrl;
			item.Look.ActiveRightIconUrl = _defaultActiveUrl;

			if(_selectUrl != string.Empty)
				item.SelectedLook.RightIconUrl = _selectUrl;
			else
				item.SelectedLook.RightIconUrl = _defaultUrl;

			item.SelectedLook.RightIconWidth = _width;
			item.SelectedLook.RightIconHeight = _height;

			if(_selectHoverUrl != string.Empty)
				item.SelectedLook.HoverRightIconUrl = _selectHoverUrl;
			else
				item.SelectedLook.HoverRightIconUrl = _defaultHoverUrl;

			if(_selectActiveUrl != string.Empty)
				item.SelectedLook.ActiveRightIconUrl = _selectActiveUrl;
			else
				item.SelectedLook.ActiveRightIconUrl = _defaultActiveUrl;
		}
	}
}
