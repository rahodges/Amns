using System;
using System.Text;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Amns.GreyFox.Web.UI.WebControls
{
//	public enum ListValidatorType {RadioButtonList, CheckBoxList};

	public class RadioButtonListRequiredFieldValidator : BaseValidator 
	{
//		private ListValidatorType _listType = ListValidatorType.CheckBoxList;
		
//		public ListValidatorType ListType
//		{
//			get { return _listType; }
//			set { _listType = value; }
//		}

		protected override bool ControlPropertiesValid()
		{
			return true;
		}

		protected override bool EvaluateIsValid() 
		{
			return this.EvaluateIsChecked();
		}

		protected bool EvaluateIsChecked() 
		{
			object controlObject = this.FindControl(this.ControlToValidate);

			if(controlObject is System.Web.UI.WebControls.CheckBoxList)
			{
				CheckBoxList _cbl = (CheckBoxList) controlObject;

				foreach(ListItem li in _cbl.Items) 
				{
					if(li.Selected == true) 
					{
						return true;
					}
				}
				return false;
			}

			if(controlObject is System.Web.UI.WebControls.RadioButtonList)
			{
				RadioButtonList _rbl = (RadioButtonList) controlObject;

				foreach(ListItem li in _rbl.Items) 
				{
					if(li.Selected == true) 
					{
						return true;
					}
				}
				return false;
			}

			return false;
		}

		protected override void OnPreRender( EventArgs e )
		{
			if (this.EnableClientScript) { this.ClientScript(); }
			base.OnPreRender( e );
		}

		protected void ClientScript() 
		{
			object controlObject = this.FindControl(this.ControlToValidate);

			if(controlObject is System.Web.UI.WebControls.RadioButtonList)
			{
				this.Attributes["evaluationfunction"] = "rb_verify_" + this.ClientID;
				StringBuilder sb_Script = new StringBuilder();
				sb_Script.Append("<script language=\"javascript\">\r\n");
				sb_Script.Append("function rb_verify_");
				sb_Script.Append(this.ClientID);
				sb_Script.Append("(val) {");
				sb_Script.Append("var val = document.all[document.all[\"" );
				sb_Script.Append(this.ClientID );
				sb_Script.Append("\"].controltovalidate];" );
				sb_Script.Append("radioOption = -1;");
				sb_Script.Append("for (counter=0; counter<val.length; counter++) {");
				sb_Script.Append("if (val[counter].checked) {");
				sb_Script.Append("radioOption = counter;");
				sb_Script.Append("}\r\n");
				sb_Script.Append("}\r\n");
				sb_Script.Append("return radioOption != -1;\r\n");
				sb_Script.Append("}\r\n");
				sb_Script.Append("</script>");
				this.Page.ClientScript.RegisterClientScriptBlock(this.GetType(), "RBLScript_" + this.ClientID, sb_Script.ToString());
				
			}
			else if(controlObject is System.Web.UI.WebControls.CheckBoxList)
			{
				this.Attributes["evaluationfunction"] = "cb_verify_" + this.ClientID;

				StringBuilder sb_Script = new StringBuilder();
				sb_Script.Append("<script language=\"javascript\">" );
				sb_Script.Append("function cb_verify_" ) ;
				sb_Script.Append(this.ClientID ) ; 
				sb_Script.Append("(val) {" );
				sb_Script.Append("\r" );
				sb_Script.Append("var val = document.all[document.all[\"" );
				sb_Script.Append(this.ClientID );
				sb_Script.Append("\"].controltovalidate];" );
				sb_Script.Append("\r" );
				sb_Script.Append("var col = val.all;" );
				sb_Script.Append("\r" );
				sb_Script.Append("if ( col != null ) {" );
				sb_Script.Append("\r" );
				sb_Script.Append("for ( i = 0; i < col.length; i++ ) {" );
				sb_Script.Append("\r" );
				sb_Script.Append("if (col.item(i).tagName == \"INPUT\") {" );
				sb_Script.Append("\r" );
				sb_Script.Append("if ( col.item(i).checked ) {" );
				sb_Script.Append("\r" );
				sb_Script.Append("\r" );
				sb_Script.Append("return true;" );
				sb_Script.Append("\r" );
				sb_Script.Append("}" );
				sb_Script.Append("\r" );
				sb_Script.Append("}" );
				sb_Script.Append("\r" );
				sb_Script.Append("}" );
				sb_Script.Append("\r" );
				sb_Script.Append("\r" );
				sb_Script.Append("\r" );
				sb_Script.Append("return false;" );
				sb_Script.Append("\r" );
				sb_Script.Append("}" );
				sb_Script.Append("\r" );
				sb_Script.Append("}" );
				sb_Script.Append("\r" );
				sb_Script.Append("</script>" );
				this.Page.ClientScript.RegisterClientScriptBlock(this.GetType(), "RBLScript_" + this.ClientID, sb_Script.ToString());
			}
		}
	}
}