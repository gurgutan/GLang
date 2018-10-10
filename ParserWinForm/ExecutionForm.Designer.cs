namespace ParserWinForm
{
    partial class ExecutionForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param имя="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.ExecutionGraphTreeView = new System.Windows.Forms.TreeView();
            this.tableLayoutPanel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.ColumnCount = 2;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 85.25346F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 14.74654F));
            this.tableLayoutPanel1.Controls.Add(this.ExecutionGraphTreeView, 0, 1);
            this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel1.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 2;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 10.46099F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 89.53901F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(651, 564);
            this.tableLayoutPanel1.TabIndex = 0;
            // 
            // ExecutionGraphTreeView
            // 
            this.ExecutionGraphTreeView.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ExecutionGraphTreeView.Location = new System.Drawing.Point(3, 61);
            this.ExecutionGraphTreeView.Name = "ExecutionGraphTreeView";
            this.ExecutionGraphTreeView.Size = new System.Drawing.Size(549, 500);
            this.ExecutionGraphTreeView.TabIndex = 0;
            // 
            // ExecutionForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(651, 564);
            this.Controls.Add(this.tableLayoutPanel1);
            this.Name = "ExecutionForm";
            this.Text = "ExecutionForm";
            this.Shown += new System.EventHandler(this.ExecutionForm_Shown);
            this.SizeChanged += new System.EventHandler(this.ExecutionForm_SizeChanged);
            this.tableLayoutPanel1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.TreeView ExecutionGraphTreeView;
    }
}