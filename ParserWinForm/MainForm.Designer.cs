namespace ParserWinForm
{
    partial class MainForm
    {
        /// <summary>
        /// Требуется переменная конструктора.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Освободить все используемые ресурсы.
        /// </summary>
        /// <param имя="disposing">истинно, если управляемый ресурс должен быть удален; иначе ложно.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Код, автоматически созданный конструктором форм Windows

        /// <summary>
        /// Обязательный метод для поддержки конструктора - не изменяйте
        /// содержимое данного метода при помощи редактора кода.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainForm));
            this.panel1 = new System.Windows.Forms.Panel();
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.ASTTreeView = new System.Windows.Forms.TreeView();
            this.splitContainer2 = new System.Windows.Forms.SplitContainer();
            this.splitContainer4 = new System.Windows.Forms.SplitContainer();
            this.panel2 = new System.Windows.Forms.Panel();
            this.label1 = new System.Windows.Forms.Label();
            this.GrammarTextBox = new System.Windows.Forms.RichTextBox();
            this.GrammarTreeView = new System.Windows.Forms.TreeView();
            this.toolStrip1 = new System.Windows.Forms.ToolStrip();
            this.ButtonExecute = new System.Windows.Forms.ToolStripButton();
            this.splitContainer3 = new System.Windows.Forms.SplitContainer();
            this.label2 = new System.Windows.Forms.Label();
            this.InputTextBox = new System.Windows.Forms.RichTextBox();
            this.panel3 = new System.Windows.Forms.Panel();
            this.label3 = new System.Windows.Forms.Label();
            this.OutputTextBox = new System.Windows.Forms.RichTextBox();
            this.panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).BeginInit();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer2)).BeginInit();
            this.splitContainer2.Panel1.SuspendLayout();
            this.splitContainer2.Panel2.SuspendLayout();
            this.splitContainer2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer4)).BeginInit();
            this.splitContainer4.Panel1.SuspendLayout();
            this.splitContainer4.Panel2.SuspendLayout();
            this.splitContainer4.SuspendLayout();
            this.panel2.SuspendLayout();
            this.toolStrip1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer3)).BeginInit();
            this.splitContainer3.Panel1.SuspendLayout();
            this.splitContainer3.Panel2.SuspendLayout();
            this.splitContainer3.SuspendLayout();
            this.panel3.SuspendLayout();
            this.SuspendLayout();
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.splitContainer1);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Margin = new System.Windows.Forms.Padding(2);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(1094, 639);
            this.panel1.TabIndex = 1;
            // 
            // splitContainer1
            // 
            this.splitContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer1.Location = new System.Drawing.Point(0, 0);
            this.splitContainer1.Margin = new System.Windows.Forms.Padding(2);
            this.splitContainer1.Name = "splitContainer1";
            // 
            // splitContainer1.Panel1
            // 
            this.splitContainer1.Panel1.Controls.Add(this.ASTTreeView);
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.Controls.Add(this.splitContainer2);
            this.splitContainer1.Size = new System.Drawing.Size(1094, 639);
            this.splitContainer1.SplitterDistance = 202;
            this.splitContainer1.SplitterWidth = 8;
            this.splitContainer1.TabIndex = 0;
            this.splitContainer1.SplitterMoved += new System.Windows.Forms.SplitterEventHandler(this.splitContainer1_SplitterMoved);
            // 
            // ASTTreeView
            // 
            this.ASTTreeView.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ASTTreeView.Location = new System.Drawing.Point(0, 0);
            this.ASTTreeView.Margin = new System.Windows.Forms.Padding(2);
            this.ASTTreeView.Name = "ASTTreeView";
            this.ASTTreeView.Size = new System.Drawing.Size(202, 639);
            this.ASTTreeView.TabIndex = 2;
            // 
            // splitContainer2
            // 
            this.splitContainer2.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.splitContainer2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer2.Location = new System.Drawing.Point(0, 0);
            this.splitContainer2.Margin = new System.Windows.Forms.Padding(2);
            this.splitContainer2.Name = "splitContainer2";
            this.splitContainer2.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // splitContainer2.Panel1
            // 
            this.splitContainer2.Panel1.Controls.Add(this.splitContainer4);
            this.splitContainer2.Panel1.Controls.Add(this.toolStrip1);
            this.splitContainer2.Panel1.RightToLeft = System.Windows.Forms.RightToLeft.No;
            // 
            // splitContainer2.Panel2
            // 
            this.splitContainer2.Panel2.Controls.Add(this.splitContainer3);
            this.splitContainer2.Panel2.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.splitContainer2.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.splitContainer2.Size = new System.Drawing.Size(884, 639);
            this.splitContainer2.SplitterDistance = 263;
            this.splitContainer2.SplitterWidth = 8;
            this.splitContainer2.TabIndex = 0;
            // 
            // splitContainer4
            // 
            this.splitContainer4.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.splitContainer4.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer4.Location = new System.Drawing.Point(0, 25);
            this.splitContainer4.Margin = new System.Windows.Forms.Padding(2);
            this.splitContainer4.Name = "splitContainer4";
            // 
            // splitContainer4.Panel1
            // 
            this.splitContainer4.Panel1.Controls.Add(this.panel2);
            this.splitContainer4.Panel1.Controls.Add(this.GrammarTextBox);
            // 
            // splitContainer4.Panel2
            // 
            this.splitContainer4.Panel2.Controls.Add(this.GrammarTreeView);
            this.splitContainer4.Size = new System.Drawing.Size(884, 238);
            this.splitContainer4.SplitterDistance = 621;
            this.splitContainer4.SplitterIncrement = 4;
            this.splitContainer4.SplitterWidth = 6;
            this.splitContainer4.TabIndex = 1;
            // 
            // panel2
            // 
            this.panel2.Controls.Add(this.label1);
            this.panel2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel2.Location = new System.Drawing.Point(0, 0);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(619, 32);
            this.panel2.TabIndex = 3;
            // 
            // label1
            // 
            this.label1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.label1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label1.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.label1.Location = new System.Drawing.Point(0, 0);
            this.label1.Name = "label1";
            this.label1.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.label1.Size = new System.Drawing.Size(619, 32);
            this.label1.TabIndex = 0;
            this.label1.Text = "Описание грамматики. Можно редактировать. Служебные символы - \"=\", \"|\", \";\", \"\'\"." +
    " Символ \' используется для отделения создания символов, совпадающих со служебным" +
    "и.";
            // 
            // GrammarTextBox
            // 
            this.GrammarTextBox.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.GrammarTextBox.Font = new System.Drawing.Font("Arial", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.GrammarTextBox.Location = new System.Drawing.Point(0, 32);
            this.GrammarTextBox.Margin = new System.Windows.Forms.Padding(2);
            this.GrammarTextBox.Name = "GrammarTextBox";
            this.GrammarTextBox.Size = new System.Drawing.Size(619, 204);
            this.GrammarTextBox.TabIndex = 2;
            this.GrammarTextBox.Text = "";
            // 
            // GrammarTreeView
            // 
            this.GrammarTreeView.Dock = System.Windows.Forms.DockStyle.Fill;
            this.GrammarTreeView.Location = new System.Drawing.Point(0, 0);
            this.GrammarTreeView.Margin = new System.Windows.Forms.Padding(2);
            this.GrammarTreeView.Name = "GrammarTreeView";
            this.GrammarTreeView.Size = new System.Drawing.Size(255, 236);
            this.GrammarTreeView.TabIndex = 1;
            this.GrammarTreeView.Leave += new System.EventHandler(this.GrammarTextBox_Leave);
            // 
            // toolStrip1
            // 
            this.toolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.ButtonExecute});
            this.toolStrip1.Location = new System.Drawing.Point(0, 0);
            this.toolStrip1.Name = "toolStrip1";
            this.toolStrip1.Size = new System.Drawing.Size(884, 25);
            this.toolStrip1.TabIndex = 0;
            this.toolStrip1.Text = "toolStrip1";
            // 
            // ButtonExecute
            // 
            this.ButtonExecute.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.ButtonExecute.Image = ((System.Drawing.Image)(resources.GetObject("ButtonExecute.Image")));
            this.ButtonExecute.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.ButtonExecute.Name = "ButtonExecute";
            this.ButtonExecute.Size = new System.Drawing.Size(73, 22);
            this.ButtonExecute.Text = "Выполнить";
            this.ButtonExecute.Click += new System.EventHandler(this.ButtonExecute_Click);
            // 
            // splitContainer3
            // 
            this.splitContainer3.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer3.Location = new System.Drawing.Point(0, 0);
            this.splitContainer3.Margin = new System.Windows.Forms.Padding(2);
            this.splitContainer3.Name = "splitContainer3";
            // 
            // splitContainer3.Panel1
            // 
            this.splitContainer3.Panel1.Controls.Add(this.label2);
            this.splitContainer3.Panel1.Controls.Add(this.InputTextBox);
            // 
            // splitContainer3.Panel2
            // 
            this.splitContainer3.Panel2.Controls.Add(this.panel3);
            this.splitContainer3.Panel2.Controls.Add(this.OutputTextBox);
            this.splitContainer3.Size = new System.Drawing.Size(882, 366);
            this.splitContainer3.SplitterDistance = 488;
            this.splitContainer3.SplitterWidth = 8;
            this.splitContainer3.TabIndex = 0;
            // 
            // label2
            // 
            this.label2.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.label2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label2.Location = new System.Drawing.Point(0, 0);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(488, 37);
            this.label2.TabIndex = 2;
            this.label2.Text = "Текст программы - можно редактировать. Генерирует выходной текст в соответствии с" +
    " правилами грамматики и исполняющими классами.";
            // 
            // InputTextBox
            // 
            this.InputTextBox.AccessibleDescription = "Текст программы - можно редактировать. Генерирует выходной текст в соответствии с" +
    " правилами грамматики и исполняющими классами.";
            this.InputTextBox.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.InputTextBox.Font = new System.Drawing.Font("Arial", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.InputTextBox.Location = new System.Drawing.Point(0, 37);
            this.InputTextBox.Margin = new System.Windows.Forms.Padding(2);
            this.InputTextBox.Name = "InputTextBox";
            this.InputTextBox.Size = new System.Drawing.Size(488, 329);
            this.InputTextBox.TabIndex = 1;
            this.InputTextBox.TabStop = false;
            this.InputTextBox.Text = "";
            this.InputTextBox.DoubleClick += new System.EventHandler(this.ButtonExecute_Click);
            // 
            // panel3
            // 
            this.panel3.Controls.Add(this.label3);
            this.panel3.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel3.Location = new System.Drawing.Point(0, 0);
            this.panel3.Name = "panel3";
            this.panel3.Size = new System.Drawing.Size(386, 37);
            this.panel3.TabIndex = 1;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label3.Location = new System.Drawing.Point(0, 0);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(186, 13);
            this.label3.TabIndex = 2;
            this.label3.Text = "Результат выполнения программы";
            // 
            // OutputTextBox
            // 
            this.OutputTextBox.AccessibleDescription = "Результат выполнения программы";
            this.OutputTextBox.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.OutputTextBox.Font = new System.Drawing.Font("Arial", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.OutputTextBox.Location = new System.Drawing.Point(0, 37);
            this.OutputTextBox.Margin = new System.Windows.Forms.Padding(2);
            this.OutputTextBox.Name = "OutputTextBox";
            this.OutputTextBox.Size = new System.Drawing.Size(386, 329);
            this.OutputTextBox.TabIndex = 0;
            this.OutputTextBox.Text = "";
            this.OutputTextBox.ZoomFactor = 2F;
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1094, 639);
            this.Controls.Add(this.panel1);
            this.Margin = new System.Windows.Forms.Padding(2);
            this.MinimizeBox = false;
            this.Name = "MainForm";
            this.Text = "Грамматика";
            this.panel1.ResumeLayout(false);
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).EndInit();
            this.splitContainer1.ResumeLayout(false);
            this.splitContainer2.Panel1.ResumeLayout(false);
            this.splitContainer2.Panel1.PerformLayout();
            this.splitContainer2.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer2)).EndInit();
            this.splitContainer2.ResumeLayout(false);
            this.splitContainer4.Panel1.ResumeLayout(false);
            this.splitContainer4.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer4)).EndInit();
            this.splitContainer4.ResumeLayout(false);
            this.panel2.ResumeLayout(false);
            this.toolStrip1.ResumeLayout(false);
            this.toolStrip1.PerformLayout();
            this.splitContainer3.Panel1.ResumeLayout(false);
            this.splitContainer3.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer3)).EndInit();
            this.splitContainer3.ResumeLayout(false);
            this.panel3.ResumeLayout(false);
            this.panel3.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.SplitContainer splitContainer1;
        private System.Windows.Forms.SplitContainer splitContainer2;
        private System.Windows.Forms.ToolStrip toolStrip1;
        private System.Windows.Forms.ToolStripButton ButtonExecute;
        private System.Windows.Forms.SplitContainer splitContainer4;
        private System.Windows.Forms.RichTextBox GrammarTextBox;
        private System.Windows.Forms.TreeView GrammarTreeView;
        private System.Windows.Forms.TreeView ASTTreeView;
        private System.Windows.Forms.SplitContainer splitContainer3;
        private System.Windows.Forms.RichTextBox InputTextBox;
        private System.Windows.Forms.RichTextBox OutputTextBox;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Panel panel3;
        private System.Windows.Forms.Label label3;
    }
}

