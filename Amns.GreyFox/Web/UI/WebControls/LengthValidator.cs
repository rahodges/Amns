using System;
using System.Text;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Amns.GreyFox.Web.UI.WebControls
{

	public class LengthValidator : BaseValidator 
	{

		protected int _LowerLength = 1;
		protected int _UpperLength = 2147483647;

		public int LowerLength 
		{

			set { _LowerLength = value; }

		}

		public int UpperLength 
		{

			set { _UpperLength = value; }

		}

		protected override bool EvaluateIsValid( ) 
		{

			string val = this . GetControlValidationValue ( this . ControlToValidate );

			if ( val . Length >= _LowerLength && val . Length <= _UpperLength ) 
			{

				return true;

			} 
			else 
			{

				return false;

			}

		}

	}

}
