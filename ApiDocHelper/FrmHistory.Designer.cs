namespace ApiDocHelper
{
    partial class FrmHistory
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
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
            this.DataView = new System.Windows.Forms.DataGridView();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.DocName = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Author = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.DocType = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.DocSaveFile = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.CreateTime = new System.Windows.Forms.DataGridViewTextBoxColumn();
            ((System.ComponentModel.ISupportInitialize)(this.DataView)).BeginInit();
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // DataView
            // 
            this.DataView.AllowUserToAddRows = false;
            this.DataView.AllowUserToDeleteRows = false;
            this.DataView.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.DataView.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.DocName,
            this.Author,
            this.DocType,
            this.DocSaveFile,
            this.CreateTime});
            this.DataView.Dock = System.Windows.Forms.DockStyle.Fill;
            this.DataView.Location = new System.Drawing.Point(3, 21);
            this.DataView.Name = "DataView";
            this.DataView.ReadOnly = true;
            this.DataView.RowTemplate.Height = 27;
            this.DataView.Size = new System.Drawing.Size(977, 334);
            this.DataView.TabIndex = 0;
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.DataView);
            this.groupBox1.Location = new System.Drawing.Point(5, 4);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(983, 358);
            this.groupBox1.TabIndex = 1;
            this.groupBox1.TabStop = false;
            // 
            // DocName
            // 
            this.DocName.DataPropertyName = "DocName";
            this.DocName.HeaderText = "文档名称";
            this.DocName.Name = "DocName";
            this.DocName.ReadOnly = true;
            // 
            // Author
            // 
            this.Author.DataPropertyName = "Author";
            this.Author.HeaderText = "文档作者";
            this.Author.Name = "Author";
            this.Author.ReadOnly = true;
            // 
            // DocType
            // 
            this.DocType.DataPropertyName = "DocType";
            this.DocType.HeaderText = "文档类型";
            this.DocType.Name = "DocType";
            this.DocType.ReadOnly = true;
            // 
            // DocSaveFile
            // 
            this.DocSaveFile.DataPropertyName = "DocSaveFile";
            this.DocSaveFile.HeaderText = "文件路径";
            this.DocSaveFile.Name = "DocSaveFile";
            this.DocSaveFile.ReadOnly = true;
            // 
            // CreateTime
            // 
            this.CreateTime.DataPropertyName = "CreateTime";
            this.CreateTime.HeaderText = "创建时间";
            this.CreateTime.Name = "CreateTime";
            this.CreateTime.ReadOnly = true;
            // 
            // FrmHistory
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(995, 364);
            this.Controls.Add(this.groupBox1);
            this.Name = "FrmHistory";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Api文档生成历史";
            this.Load += new System.EventHandler(this.FrmHistory_Load);
            ((System.ComponentModel.ISupportInitialize)(this.DataView)).EndInit();
            this.groupBox1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.DataGridView DataView;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.DataGridViewTextBoxColumn DocName;
        private System.Windows.Forms.DataGridViewTextBoxColumn Author;
        private System.Windows.Forms.DataGridViewTextBoxColumn DocType;
        private System.Windows.Forms.DataGridViewTextBoxColumn DocSaveFile;
        private System.Windows.Forms.DataGridViewTextBoxColumn CreateTime;
    }
}