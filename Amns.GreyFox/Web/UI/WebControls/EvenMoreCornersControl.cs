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
    [ToolboxData("<{0}:EvenMoreCornersControl runat=server></{0}:EvenMoreCornersControl>")]
    public class EvenMoreCornersDialog : Control
    {
        private ControlCollection header;
        private ControlCollection body;
        private ControlCollection footer;
        private string cssClass;
        private DialogMode dialogMode;

        public enum DialogMode { Dialog, Downgrade };

        public string CssClass
        {
            get { return cssClass; }
            set { cssClass = value; }
        }

        public ControlCollection Header
        {
            get
            {
                if (header == null)
                    header = new ControlCollection(this);
                return header;
            }
            set { header = value; }
        }

        public ControlCollection Body
        {
            get
            {
                if (body == null)
                    body = new ControlCollection(this);
                return body;
            }
            set { body = value; }
        }

        public ControlCollection Footer
        {
            get
            {
                if (footer == null)
                    footer = new ControlCollection(this);
                return footer;
            }
            set { footer = value; }
        }

        public DialogMode Mode
        {
            get { return dialogMode; }
            set { dialogMode = value; }
        }

        public EvenMoreCornersDialog() : base()
        {
            dialogMode = DialogMode.Dialog;
        }
                
        protected override void Render(HtmlTextWriter output)
        {            
            output.WriteLine("<div class=\"" + cssClass  + "\">");
            output.Indent++;

            if (dialogMode == DialogMode.Dialog)
            {
                output.WriteLine("<div class=\"content\">");
                output.Indent++;

                output.WriteLine("<div class=\"wrapper\">");
                output.Indent++;

                output.WriteLine("<div class=\"t\"></div>");
                output.WriteLine();
            }

            if (header != null && header.Count > 0)
            {
                if (dialogMode == DialogMode.Dialog)
                {
                    output.WriteBeginTag("div");
                    output.WriteAttribute("class", "hd");
                    output.WriteLine(Html32TextWriter.TagRightChar);
                    output.Indent++;
                }
                foreach (Control control in header)
                    control.RenderControl(output);
                if (dialogMode == DialogMode.Dialog)
                {
                    output.Indent--;
                    output.WriteEndTag("div");
                    output.WriteLine();
                }
            }

            if (dialogMode == DialogMode.Dialog)
            {
                output.WriteBeginTag("div");
                output.WriteAttribute("class", "bd");
                output.WriteLine(Html32TextWriter.TagRightChar);
                output.Indent++;
            }
            if (body != null && body.Count > 0)
            {
                foreach (Control control in body)
                    control.RenderControl(output);
            }
            else
            {
                output.WriteLine("&nbsp;");
            }
            if (dialogMode == DialogMode.Dialog)
            {
                output.Indent--;
                output.WriteEndTag("div");
                output.WriteLine();
            }

            if (footer != null && footer.Count > 0)
            {
                if (dialogMode == DialogMode.Dialog)
                {
                    output.WriteBeginTag("div");
                    output.WriteAttribute("class", "ft");
                    output.WriteLine(Html32TextWriter.TagRightChar);
                    output.Indent++;
                }
                foreach (Control control in footer)
                    control.RenderControl(output);
                if (dialogMode == DialogMode.Dialog)
                {
                    output.Indent--;
                    output.WriteEndTag("div");
                    output.WriteLine();
                }
            }

            if (dialogMode == DialogMode.Dialog)
            {
                output.Indent--;
                output.WriteEndTag("div"); // wrapper
                output.WriteLine();

                output.Indent--;
                output.WriteEndTag("div"); // content
                output.WriteLine();

                output.WriteLine("<div class=\"b\"><div></div></div>");
            }

            output.Indent--;
            output.WriteEndTag("div"); // dialog
            output.WriteLine();
        }
    }
}
