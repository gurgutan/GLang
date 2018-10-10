using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using DataModelLibrary;
using Interpreter;

namespace ParserWinForm
{

    public partial class ExecutionForm : Form
    {

        public ExecutionForm()
        {
            InitializeComponent();
        }

        private void ExecutionForm_SizeChanged(object sender, EventArgs e)
        {

        }

        private void ExecutionForm_Shown(object sender, EventArgs e)
        {
            var nodes = Storage.Output.
                ToList().
                Select<KeyValuePair<string, Конус>, TreeNode>(pair => ToTreeNode(pair.Value)).
                    ToArray();
            ExecutionGraphTreeView.Nodes.Clear();
            ExecutionGraphTreeView.Nodes.AddRange(nodes);
        }

        private TreeNode ToTreeNode(Конус w)
        {
            return new TreeNode(w.Имя, w.Кообласть.
                Select<KeyValuePair<string, Конус>, TreeNode>(
                linksPair => ToTreeNode(linksPair.Value)).
                    ToArray());
        }
    }
}
