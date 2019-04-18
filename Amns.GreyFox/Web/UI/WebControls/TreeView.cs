using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using ComponentArt.Web.UI;

namespace Amns.GreyFox.Web.UI.WebControls
{
    [DefaultProperty("Text")]
    [ToolboxData("<{0}:TreeView runat=server></{0}:TreeView>")]
    public class TreeView : Control
    {
        protected ComponentArt.Web.UI.TreeView tree;

        #region Properties

        private string treeCssClass;
        public string TreeCssClass
        {
            get { return treeCssClass; }
            set { treeCssClass = value; }
        }
        private string selectedNodeCssClass;
        public string SelectedNodeCssClass
        {
            get { return selectedNodeCssClass; }
            set { selectedNodeCssClass = value; }
        }

        private string hoverNodeCssClass;        
        public string HoverNodeCssClass
        {
            get { return hoverNodeCssClass; }
            set { hoverNodeCssClass = value; }
        }

        private string nodeEditCssClass;        
        public string NodeEditCssClass
        {
            get { return nodeEditCssClass; }
            set { nodeEditCssClass = value; }
        }
        
        private int lineImageWidth;
        public int LineImageWidth
        {
            get { return lineImageWidth; }
            set { lineImageWidth = value; }
        }
        
        private int lineImageHeight;
        public int LineImageHeight
        {
            get { return lineImageHeight; }
            set { lineImageHeight = value; }
        }
        
        private int defaultImageWidth;
        public int DefaultImageWidth
        {
            get { return defaultImageWidth; }
            set { defaultImageWidth = value; }
        }
        
        private int defaultImageHeight;
        public int DefaultImageHeight
        {
            get { return defaultImageHeight; }
            set { defaultImageHeight = value; }
        }
        
        private int itemSpacing;
        public int ItemSpacing
        {
            get { return itemSpacing; }
            set { itemSpacing = value; }
        }

        private int nodeLabelPadding;
        public int NodeLabelPadding
        {
            get
            {
                return nodeLabelPadding;
            }
            set
            {
                nodeLabelPadding = value;
            }
        }
        private string parentNodeImageUrl;
        public string ParentNodeImageUrl
        {
            get
            {
                return parentNodeImageUrl;
            }
            set
            {
                parentNodeImageUrl = value;
            }
        }
        private string leafNodeImageUrl;
        public string LeafNodeImageUrl
        {
            get
            {
                return leafNodeImageUrl;
            }
            set
            {
                leafNodeImageUrl = value;
            }
        }
        private bool showLines;
        public bool ShowLines
        {
            get
            {
                return showLines;
            }
            set
            {
                showLines = value;
            }
        }
        private string lineImagesFolderUrl;
        public string LineImagesFolderUrl
        {
            get
            {
                return lineImagesFolderUrl;
            }
            set
            {
                lineImagesFolderUrl = value;
            }
        }
        private string cssClass;
        public string CssClass
        {
            get
            {
                return cssClass;
            }
            set
            {
                cssClass = value;
            }
        }
        private string nodeCssClass;
        public string NodeCssClass
        {
            get
            {
                return nodeCssClass;
            }
            set
            {
                nodeCssClass = value;
            }
        }
        private Unit width;
        public Unit Width
        {
            get
            {
                return width;
            }
            set
            {
                width = value;
            }
        }
        private Unit height;
        public Unit Height
        {
            get
            {
                return height;
            }
            set
            {
                height = value;
            }
        }

        #endregion

        public TreeView()
        {
            cssClass = "TreeView";
            nodeCssClass = "TreeNode";
            selectedNodeCssClass = "SelectedTreeNode";
            hoverNodeCssClass = "HoverTreeNode";
            nodeEditCssClass = "NodeEdit";
            lineImageHeight = 19;
            lineImageWidth = 20;
            defaultImageWidth = 16;
            defaultImageHeight = 16;
            nodeLabelPadding = 3;
            parentNodeImageUrl = "images/tree/folders.gif";
            leafNodeImageUrl = "images/tree/folder.gif";
            showLines = true;
            lineImagesFolderUrl = "images/lines/";
            EnableViewState = false;
        }

        protected override void CreateChildControls()
        {
            Panel container = new Panel();
            container.CssClass = cssClass;
            Controls.Add(container);

            tree = new ComponentArt.Web.UI.TreeView();
            tree.ID = ID + "_Tree";
            tree.SelectedNodeCssClass = selectedNodeCssClass;
            tree.HoverNodeCssClass = hoverNodeCssClass;
            tree.NodeEditCssClass = nodeEditCssClass;
            tree.LineImageHeight = lineImageHeight;
            tree.LineImageWidth = lineImageWidth;
            tree.DefaultImageWidth = defaultImageWidth;
            tree.DefaultImageHeight = defaultImageHeight;
            tree.ItemSpacing = itemSpacing;
            tree.NodeLabelPadding = nodeLabelPadding;
            tree.ParentNodeImageUrl = parentNodeImageUrl;
            tree.LeafNodeImageUrl = leafNodeImageUrl;
            tree.ShowLines = showLines;
            tree.LineImagesFolderUrl = lineImagesFolderUrl;
            tree.CssClass = treeCssClass;
            tree.NodeCssClass = nodeCssClass;
            tree.EnableViewState = EnableViewState;
            tree.Width = width;
            tree.Height = height;
            container.Controls.Add(tree);

            ChildControlsCreated = true;
        }
    }
}

