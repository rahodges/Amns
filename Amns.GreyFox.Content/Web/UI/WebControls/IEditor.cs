using System;
using System.Collections.Generic;
using System.Text;

namespace Amns.GreyFox.Content.Web.UI.WebControls
{
    public interface IEditor
    {
        ComponentArt.Web.UI.Editor Editor { get; }
        string SkinPath { get; set; }
    }
}
