using System.Windows.Forms;

namespace WindowsFormsApp1
{
    partial class Form1
    {
        /// <summary>
        /// 設計工具所需的變數。
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// 清除任何使用中的資源。
        /// </summary>
        /// <param name="disposing">如果應該處置受控資源則為 true，否則為 false。</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form 設計工具產生的程式碼

        /// <summary>
        /// 此為設計工具支援所需的方法 - 請勿使用程式碼編輯器修改
        /// 這個方法的內容。
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Form1));
            this.toolStrip1 = new System.Windows.Forms.ToolStrip();
            this.StartShapeButton = new System.Windows.Forms.ToolStripButton();
            this.TerminatorShapeButton = new System.Windows.Forms.ToolStripButton();
            this.DecisionShapeButton = new System.Windows.Forms.ToolStripButton();
            this.ProcessShapeButton = new System.Windows.Forms.ToolStripButton();
            this.toolStripButtonLine = new System.Windows.Forms.ToolStripButton();
            this.NormalModeButton = new System.Windows.Forms.ToolStripButton();
            this.toolStripButtonUndo = new System.Windows.Forms.ToolStripButton();
            this.toolStripButtonRedo = new System.Windows.Forms.ToolStripButton();
            this.saveButton = new System.Windows.Forms.ToolStripButton();
            this.loadButton = new System.Windows.Forms.ToolStripButton();
            this.toolStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // toolStrip1
            // 
            this.toolStrip1.ImageScalingSize = new System.Drawing.Size(20, 20);
            this.toolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.StartShapeButton,
            this.TerminatorShapeButton,
            this.DecisionShapeButton,
            this.ProcessShapeButton,
            this.toolStripButtonLine,
            this.NormalModeButton,
            this.toolStripButtonUndo,
            this.toolStripButtonRedo,
            this.saveButton,
            this.loadButton});
            this.toolStrip1.Location = new System.Drawing.Point(0, 0);
            this.toolStrip1.Name = "toolStrip1";
            this.toolStrip1.Size = new System.Drawing.Size(1924, 29);
            this.toolStrip1.TabIndex = 0;
            this.toolStrip1.Text = "toolStrip1";
            // 
            // StartShapeButton
            // 
            this.StartShapeButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.StartShapeButton.Image = ((System.Drawing.Image)(resources.GetObject("StartShapeButton.Image")));
            this.StartShapeButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.StartShapeButton.Name = "StartShapeButton";
            this.StartShapeButton.Size = new System.Drawing.Size(34, 24);
            this.StartShapeButton.Text = "Start";
            this.StartShapeButton.Click += new System.EventHandler(this.StartShapeButton_Click);
            // 
            // TerminatorShapeButton
            // 
            this.TerminatorShapeButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.TerminatorShapeButton.Image = ((System.Drawing.Image)(resources.GetObject("TerminatorShapeButton.Image")));
            this.TerminatorShapeButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.TerminatorShapeButton.Name = "TerminatorShapeButton";
            this.TerminatorShapeButton.Size = new System.Drawing.Size(34, 24);
            this.TerminatorShapeButton.Text = "Terminator";
            this.TerminatorShapeButton.Click += new System.EventHandler(this.TerminatorShapeButton_Click);
            // 
            // DecisionShapeButton
            // 
            this.DecisionShapeButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.DecisionShapeButton.Image = ((System.Drawing.Image)(resources.GetObject("DecisionShapeButton.Image")));
            this.DecisionShapeButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.DecisionShapeButton.Name = "DecisionShapeButton";
            this.DecisionShapeButton.Size = new System.Drawing.Size(34, 24);
            this.DecisionShapeButton.Text = "Decision";
            this.DecisionShapeButton.Click += new System.EventHandler(this.DecisionShapeButton_Click);
            // 
            // ProcessShapeButton
            // 
            this.ProcessShapeButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.ProcessShapeButton.Image = ((System.Drawing.Image)(resources.GetObject("ProcessShapeButton.Image")));
            this.ProcessShapeButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.ProcessShapeButton.Name = "ProcessShapeButton";
            this.ProcessShapeButton.Size = new System.Drawing.Size(34, 24);
            this.ProcessShapeButton.Text = "Process";
            this.ProcessShapeButton.Click += new System.EventHandler(this.ProcessShapeButton_Click);
            // 
            // toolStripButtonLine
            // 
            this.toolStripButtonLine.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.toolStripButtonLine.Image = ((System.Drawing.Image)(resources.GetObject("toolStripButtonLine.Image")));
            this.toolStripButtonLine.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButtonLine.Name = "toolStripButtonLine";
            this.toolStripButtonLine.Size = new System.Drawing.Size(34, 24);
            this.toolStripButtonLine.Text = "toolStripButton1";
            this.toolStripButtonLine.Click += new System.EventHandler(this.toolStripButtonLine_Click);
            // 
            // NormalModeButton
            // 
            this.NormalModeButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.NormalModeButton.Image = ((System.Drawing.Image)(resources.GetObject("NormalModeButton.Image")));
            this.NormalModeButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.NormalModeButton.Name = "NormalModeButton";
            this.NormalModeButton.Size = new System.Drawing.Size(34, 24);
            this.NormalModeButton.Text = "toolStripButton5";
            this.NormalModeButton.Click += new System.EventHandler(this.NormalModeButton_Click);
            // 
            // toolStripButtonUndo
            // 
            this.toolStripButtonUndo.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.toolStripButtonUndo.Image = ((System.Drawing.Image)(resources.GetObject("toolStripButtonUndo.Image")));
            this.toolStripButtonUndo.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButtonUndo.Name = "toolStripButtonUndo";
            this.toolStripButtonUndo.Size = new System.Drawing.Size(34, 24);
            this.toolStripButtonUndo.Text = "toolStripButtonUndo";
            this.toolStripButtonUndo.Click += new System.EventHandler(this.toolStripButtonUndo_Click);
            // 
            // toolStripButtonRedo
            // 
            this.toolStripButtonRedo.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.toolStripButtonRedo.Image = ((System.Drawing.Image)(resources.GetObject("toolStripButtonRedo.Image")));
            this.toolStripButtonRedo.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButtonRedo.Name = "toolStripButtonRedo";
            this.toolStripButtonRedo.Size = new System.Drawing.Size(34, 24);
            this.toolStripButtonRedo.Text = "toolStripButtonRedo";
            this.toolStripButtonRedo.Click += new System.EventHandler(this.toolStripButtonRedo_Click);
            // 
            // saveButton
            // 
            this.saveButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.saveButton.Image = ((System.Drawing.Image)(resources.GetObject("saveButton.Image")));
            this.saveButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.saveButton.Name = "saveButton";
            this.saveButton.Size = new System.Drawing.Size(34, 24);
            this.saveButton.Text = "toolStripButton1";
            this.saveButton.Click += new System.EventHandler(this.saveButton_Click);
            // 
            // loadButton
            // 
            this.loadButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.loadButton.Image = ((System.Drawing.Image)(resources.GetObject("loadButton.Image")));
            this.loadButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.loadButton.Name = "loadButton";
            this.loadButton.Size = new System.Drawing.Size(34, 24);
            this.loadButton.Text = "toolStripButton1";
            this.loadButton.Click += new System.EventHandler(this.loadButton_Click_1);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 18F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1924, 984);
            this.Controls.Add(this.toolStrip1);
            this.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.Name = "Form1";
            this.Text = "Form1";
            this.Load += new System.EventHandler(this.Form1_Load);
            this.toolStrip1.ResumeLayout(false);
            this.toolStrip1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ToolStrip toolStrip1;
        private System.Windows.Forms.ToolStripButton StartShapeButton;
        private System.Windows.Forms.ToolStripButton TerminatorShapeButton;
        private System.Windows.Forms.ToolStripButton DecisionShapeButton;
        private System.Windows.Forms.ToolStripButton ProcessShapeButton;
        private System.Windows.Forms.ToolStripButton NormalModeButton;
        private System.Windows.Forms.ToolStripButton toolStripButtonLine;
        private System.Windows.Forms.ToolStripButton toolStripButtonUndo;
        private System.Windows.Forms.ToolStripButton toolStripButtonRedo;
        private ToolStripButton saveButton;
        private ToolStripButton loadButton;
    }
}

