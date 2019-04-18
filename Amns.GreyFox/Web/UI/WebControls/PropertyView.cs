using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Amns.GreyFox.Web.UI.WebControls
{
    [DefaultProperty("Text")]
    [ToolboxData("<{0}:PropertyView runat=server></{0}:PropertyView>")]
    public class PropertyView : WebControl
    {
        object selectedTag;

        public object SelectedTag { get { return selectedTag; } set { selectedTag = value; } }

        protected override void RenderContents(HtmlTextWriter output)
        {
            PropertyDescriptorCollection properties =
                TypeDescriptor.GetProperties(selectedTag);

            foreach (PropertyDescriptor descriptor in properties)
            {
                if (descriptor.IsBrowsable)
                {
                    output.WriteBeginTag("div");
                }
            }
        }
    }
}
