namespace SchemesConstructor
{
    partial class FormLSA
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
            if (disposing && (components != null)) {
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
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.tabPage1 = new System.Windows.Forms.TabPage();
            this.dataGridViewS1 = new System.Windows.Forms.DataGridView();
            this.tabPage2 = new System.Windows.Forms.TabPage();
            this.dataGridViewS2 = new System.Windows.Forms.DataGridView();
            this.tabPage3 = new System.Windows.Forms.TabPage();
            this.dataGridViewS3 = new System.Windows.Forms.DataGridView();
            this.tabPage4 = new System.Windows.Forms.TabPage();
            this.dataGridViewS3_Minimized = new System.Windows.Forms.DataGridView();
            this.tabPage5 = new System.Windows.Forms.TabPage();
            this.textBoxLSA = new System.Windows.Forms.TextBox();
            this.groupBoxInformation = new System.Windows.Forms.GroupBox();
            this.labelTime = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.labelNumberErrors = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.progressBar = new System.Windows.Forms.ProgressBar();
            this.label3 = new System.Windows.Forms.Label();
            this.labelCondition = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.buttonCheckLSA = new System.Windows.Forms.Button();
            this.tabControl1.SuspendLayout();
            this.tabPage1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewS1)).BeginInit();
            this.tabPage2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewS2)).BeginInit();
            this.tabPage3.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewS3)).BeginInit();
            this.tabPage4.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewS3_Minimized)).BeginInit();
            this.tabPage5.SuspendLayout();
            this.groupBoxInformation.SuspendLayout();
            this.SuspendLayout();
            // 
            // tabControl1
            // 
            this.tabControl1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tabControl1.Controls.Add(this.tabPage1);
            this.tabControl1.Controls.Add(this.tabPage2);
            this.tabControl1.Controls.Add(this.tabPage3);
            this.tabControl1.Controls.Add(this.tabPage4);
            this.tabControl1.Controls.Add(this.tabPage5);
            this.tabControl1.Font = new System.Drawing.Font("Calibri", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.tabControl1.Location = new System.Drawing.Point(12, 12);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(1240, 577);
            this.tabControl1.TabIndex = 0;
            // 
            // tabPage1
            // 
            this.tabPage1.BackColor = System.Drawing.Color.LightGray;
            this.tabPage1.Controls.Add(this.dataGridViewS1);
            this.tabPage1.Font = new System.Drawing.Font("Calibri", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.tabPage1.Location = new System.Drawing.Point(4, 32);
            this.tabPage1.Name = "tabPage1";
            this.tabPage1.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage1.Size = new System.Drawing.Size(1232, 541);
            this.tabPage1.TabIndex = 0;
            this.tabPage1.Text = "Система формул переходов S1";
            // 
            // dataGridViewS1
            // 
            this.dataGridViewS1.AllowUserToAddRows = false;
            this.dataGridViewS1.AllowUserToDeleteRows = false;
            this.dataGridViewS1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.dataGridViewS1.AutoSizeRowsMode = System.Windows.Forms.DataGridViewAutoSizeRowsMode.AllHeaders;
            this.dataGridViewS1.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridViewS1.Location = new System.Drawing.Point(7, 6);
            this.dataGridViewS1.Name = "dataGridViewS1";
            this.dataGridViewS1.RowHeadersWidthSizeMode = System.Windows.Forms.DataGridViewRowHeadersWidthSizeMode.AutoSizeToAllHeaders;
            this.dataGridViewS1.Size = new System.Drawing.Size(1219, 529);
            this.dataGridViewS1.TabIndex = 1;
            this.dataGridViewS1.CellEndEdit += new System.Windows.Forms.DataGridViewCellEventHandler(this.dataGridViewS1_CellEndEdit);
            this.dataGridViewS1.EditingControlShowing += new System.Windows.Forms.DataGridViewEditingControlShowingEventHandler(this.dataGridViewS1_EditingControlShowing);
            this.dataGridViewS1.KeyUp += new System.Windows.Forms.KeyEventHandler(this.dataGridViewS1_KeyUp);
            // 
            // tabPage2
            // 
            this.tabPage2.BackColor = System.Drawing.Color.LightGray;
            this.tabPage2.Controls.Add(this.dataGridViewS2);
            this.tabPage2.Font = new System.Drawing.Font("Calibri", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.tabPage2.Location = new System.Drawing.Point(4, 32);
            this.tabPage2.Name = "tabPage2";
            this.tabPage2.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage2.Size = new System.Drawing.Size(1232, 541);
            this.tabPage2.TabIndex = 1;
            this.tabPage2.Text = "Система скобочных формул S2";
            // 
            // dataGridViewS2
            // 
            this.dataGridViewS2.AllowUserToAddRows = false;
            this.dataGridViewS2.AllowUserToDeleteRows = false;
            this.dataGridViewS2.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.dataGridViewS2.AutoSizeRowsMode = System.Windows.Forms.DataGridViewAutoSizeRowsMode.AllHeaders;
            this.dataGridViewS2.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridViewS2.Location = new System.Drawing.Point(6, 6);
            this.dataGridViewS2.Name = "dataGridViewS2";
            this.dataGridViewS2.RowHeadersWidthSizeMode = System.Windows.Forms.DataGridViewRowHeadersWidthSizeMode.AutoSizeToAllHeaders;
            this.dataGridViewS2.Size = new System.Drawing.Size(1220, 529);
            this.dataGridViewS2.TabIndex = 1;
            this.dataGridViewS2.CellEndEdit += new System.Windows.Forms.DataGridViewCellEventHandler(this.dataGridViewS2_CellEndEdit);
            this.dataGridViewS2.EditingControlShowing += new System.Windows.Forms.DataGridViewEditingControlShowingEventHandler(this.dataGridViewS2_EditingControlShowing);
            this.dataGridViewS2.KeyUp += new System.Windows.Forms.KeyEventHandler(this.dataGridViewS2_KeyUp);
            // 
            // tabPage3
            // 
            this.tabPage3.BackColor = System.Drawing.Color.LightGray;
            this.tabPage3.Controls.Add(this.dataGridViewS3);
            this.tabPage3.Font = new System.Drawing.Font("Calibri", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.tabPage3.Location = new System.Drawing.Point(4, 32);
            this.tabPage3.Name = "tabPage3";
            this.tabPage3.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage3.Size = new System.Drawing.Size(1232, 541);
            this.tabPage3.TabIndex = 2;
            this.tabPage3.Text = "Система схемных формул S3";
            // 
            // dataGridViewS3
            // 
            this.dataGridViewS3.AllowUserToAddRows = false;
            this.dataGridViewS3.AllowUserToDeleteRows = false;
            this.dataGridViewS3.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.dataGridViewS3.AutoSizeRowsMode = System.Windows.Forms.DataGridViewAutoSizeRowsMode.AllHeaders;
            this.dataGridViewS3.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridViewS3.Location = new System.Drawing.Point(7, 6);
            this.dataGridViewS3.Name = "dataGridViewS3";
            this.dataGridViewS3.RowHeadersWidthSizeMode = System.Windows.Forms.DataGridViewRowHeadersWidthSizeMode.AutoSizeToAllHeaders;
            this.dataGridViewS3.Size = new System.Drawing.Size(1219, 529);
            this.dataGridViewS3.TabIndex = 1;
            this.dataGridViewS3.CellEndEdit += new System.Windows.Forms.DataGridViewCellEventHandler(this.dataGridViewS3_CellEndEdit);
            this.dataGridViewS3.EditingControlShowing += new System.Windows.Forms.DataGridViewEditingControlShowingEventHandler(this.dataGridViewS3_EditingControlShowing);
            this.dataGridViewS3.KeyUp += new System.Windows.Forms.KeyEventHandler(this.dataGridViewS3_KeyUp);
            // 
            // tabPage4
            // 
            this.tabPage4.BackColor = System.Drawing.Color.LightGray;
            this.tabPage4.Controls.Add(this.dataGridViewS3_Minimized);
            this.tabPage4.Font = new System.Drawing.Font("Calibri", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.tabPage4.Location = new System.Drawing.Point(4, 32);
            this.tabPage4.Name = "tabPage4";
            this.tabPage4.Size = new System.Drawing.Size(1232, 541);
            this.tabPage4.TabIndex = 3;
            this.tabPage4.Text = "S3 минимизированная";
            // 
            // dataGridViewS3_Minimized
            // 
            this.dataGridViewS3_Minimized.AllowUserToAddRows = false;
            this.dataGridViewS3_Minimized.AllowUserToDeleteRows = false;
            this.dataGridViewS3_Minimized.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.dataGridViewS3_Minimized.AutoSizeRowsMode = System.Windows.Forms.DataGridViewAutoSizeRowsMode.AllHeaders;
            this.dataGridViewS3_Minimized.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridViewS3_Minimized.Location = new System.Drawing.Point(7, 6);
            this.dataGridViewS3_Minimized.Name = "dataGridViewS3_Minimized";
            this.dataGridViewS3_Minimized.RowHeadersWidthSizeMode = System.Windows.Forms.DataGridViewRowHeadersWidthSizeMode.AutoSizeToAllHeaders;
            this.dataGridViewS3_Minimized.Size = new System.Drawing.Size(1219, 529);
            this.dataGridViewS3_Minimized.TabIndex = 2;
            this.dataGridViewS3_Minimized.CellEndEdit += new System.Windows.Forms.DataGridViewCellEventHandler(this.dataGridViewS3_Minimized_CellEndEdit);
            this.dataGridViewS3_Minimized.EditingControlShowing += new System.Windows.Forms.DataGridViewEditingControlShowingEventHandler(this.dataGridViewS3_Minimized_EditingControlShowing);
            this.dataGridViewS3_Minimized.KeyUp += new System.Windows.Forms.KeyEventHandler(this.dataGridViewS3_Minimized_KeyUp);
            // 
            // tabPage5
            // 
            this.tabPage5.BackColor = System.Drawing.Color.LightGray;
            this.tabPage5.Controls.Add(this.buttonCheckLSA);
            this.tabPage5.Controls.Add(this.textBoxLSA);
            this.tabPage5.Font = new System.Drawing.Font("Calibri", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.tabPage5.Location = new System.Drawing.Point(4, 32);
            this.tabPage5.Name = "tabPage5";
            this.tabPage5.Size = new System.Drawing.Size(1232, 541);
            this.tabPage5.TabIndex = 4;
            this.tabPage5.Text = "ЛСА";
            // 
            // textBoxLSA
            // 
            this.textBoxLSA.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.textBoxLSA.Font = new System.Drawing.Font("Calibri", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.textBoxLSA.Location = new System.Drawing.Point(3, 3);
            this.textBoxLSA.Multiline = true;
            this.textBoxLSA.Name = "textBoxLSA";
            this.textBoxLSA.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this.textBoxLSA.Size = new System.Drawing.Size(1226, 473);
            this.textBoxLSA.TabIndex = 4;
            this.textBoxLSA.KeyDown += new System.Windows.Forms.KeyEventHandler(this.textBoxLSA_KeyDown);
            this.textBoxLSA.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.textBoxLSA_KeyPress);
            // 
            // groupBoxInformation
            // 
            this.groupBoxInformation.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBoxInformation.BackColor = System.Drawing.Color.LightGray;
            this.groupBoxInformation.Controls.Add(this.labelTime);
            this.groupBoxInformation.Controls.Add(this.label6);
            this.groupBoxInformation.Controls.Add(this.labelNumberErrors);
            this.groupBoxInformation.Controls.Add(this.label4);
            this.groupBoxInformation.Controls.Add(this.progressBar);
            this.groupBoxInformation.Controls.Add(this.label3);
            this.groupBoxInformation.Controls.Add(this.labelCondition);
            this.groupBoxInformation.Controls.Add(this.label1);
            this.groupBoxInformation.Font = new System.Drawing.Font("Calibri", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.groupBoxInformation.Location = new System.Drawing.Point(12, 595);
            this.groupBoxInformation.Name = "groupBoxInformation";
            this.groupBoxInformation.Size = new System.Drawing.Size(1239, 74);
            this.groupBoxInformation.TabIndex = 4;
            this.groupBoxInformation.TabStop = false;
            this.groupBoxInformation.Text = "Информация о выполнении задания";
            // 
            // labelTime
            // 
            this.labelTime.AutoSize = true;
            this.labelTime.Font = new System.Drawing.Font("Calibri", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.labelTime.Location = new System.Drawing.Point(1153, 31);
            this.labelTime.Name = "labelTime";
            this.labelTime.Size = new System.Drawing.Size(80, 23);
            this.labelTime.TabIndex = 7;
            this.labelTime.Text = "00:00:00";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Font = new System.Drawing.Font("Calibri", 14.25F, System.Drawing.FontStyle.Underline, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.label6.Location = new System.Drawing.Point(977, 31);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(170, 23);
            this.label6.TabIndex = 6;
            this.label6.Text = "Время выполнения:";
            // 
            // labelNumberErrors
            // 
            this.labelNumberErrors.AutoSize = true;
            this.labelNumberErrors.Font = new System.Drawing.Font("Calibri", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.labelNumberErrors.Location = new System.Drawing.Point(870, 31);
            this.labelNumberErrors.Name = "labelNumberErrors";
            this.labelNumberErrors.Size = new System.Drawing.Size(20, 23);
            this.labelNumberErrors.TabIndex = 5;
            this.labelNumberErrors.Text = "0";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Font = new System.Drawing.Font("Calibri", 14.25F, System.Drawing.FontStyle.Underline, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.label4.Location = new System.Drawing.Point(667, 31);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(174, 23);
            this.label4.TabIndex = 4;
            this.label4.Text = "Количество ошибок:";
            // 
            // progressBar
            // 
            this.progressBar.Location = new System.Drawing.Point(417, 31);
            this.progressBar.Name = "progressBar";
            this.progressBar.Size = new System.Drawing.Size(155, 23);
            this.progressBar.Step = 1;
            this.progressBar.TabIndex = 3;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("Calibri", 14.25F, System.Drawing.FontStyle.Underline, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.label3.Location = new System.Drawing.Point(322, 31);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(89, 23);
            this.label3.TabIndex = 2;
            this.label3.Text = "Прогресс:";
            // 
            // labelCondition
            // 
            this.labelCondition.AutoSize = true;
            this.labelCondition.BackColor = System.Drawing.Color.Gold;
            this.labelCondition.Font = new System.Drawing.Font("Calibri", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.labelCondition.Location = new System.Drawing.Point(111, 31);
            this.labelCondition.Name = "labelCondition";
            this.labelCondition.Size = new System.Drawing.Size(126, 23);
            this.labelCondition.TabIndex = 1;
            this.labelCondition.Text = "Не выполнено";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Calibri", 14.25F, System.Drawing.FontStyle.Underline, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.label1.Location = new System.Drawing.Point(7, 31);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(98, 23);
            this.label1.TabIndex = 0;
            this.label1.Text = "Состояние:";
            // 
            // buttonCheckLSA
            // 
            this.buttonCheckLSA.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonCheckLSA.BackColor = System.Drawing.Color.LimeGreen;
            this.buttonCheckLSA.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.buttonCheckLSA.Location = new System.Drawing.Point(1092, 491);
            this.buttonCheckLSA.Name = "buttonCheckLSA";
            this.buttonCheckLSA.Size = new System.Drawing.Size(124, 36);
            this.buttonCheckLSA.TabIndex = 5;
            this.buttonCheckLSA.Text = "Проверить";
            this.buttonCheckLSA.UseVisualStyleBackColor = false;
            this.buttonCheckLSA.Click += new System.EventHandler(this.buttonCheckLSA_Click);
            // 
            // FormLSA
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1264, 681);
            this.Controls.Add(this.groupBoxInformation);
            this.Controls.Add(this.tabControl1);
            this.Name = "FormLSA";
            this.Text = "Логическая схема алгоритма";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.FormLSA_FormClosing);
            this.VisibleChanged += new System.EventHandler(this.FormLSA_VisibleChanged);
            this.tabControl1.ResumeLayout(false);
            this.tabPage1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewS1)).EndInit();
            this.tabPage2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewS2)).EndInit();
            this.tabPage3.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewS3)).EndInit();
            this.tabPage4.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewS3_Minimized)).EndInit();
            this.tabPage5.ResumeLayout(false);
            this.tabPage5.PerformLayout();
            this.groupBoxInformation.ResumeLayout(false);
            this.groupBoxInformation.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.TabPage tabPage1;
        private System.Windows.Forms.TabPage tabPage2;
        private System.Windows.Forms.TabPage tabPage3;
        private System.Windows.Forms.TabPage tabPage4;
        private System.Windows.Forms.TabPage tabPage5;
        private System.Windows.Forms.GroupBox groupBoxInformation;
        private System.Windows.Forms.Label labelTime;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label labelNumberErrors;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.ProgressBar progressBar;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label labelCondition;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.DataGridView dataGridViewS1;
        private System.Windows.Forms.DataGridView dataGridViewS2;
        private System.Windows.Forms.DataGridView dataGridViewS3;
        private System.Windows.Forms.TextBox textBoxLSA;
        private System.Windows.Forms.DataGridView dataGridViewS3_Minimized;
        private System.Windows.Forms.Button buttonCheckLSA;
    }
}