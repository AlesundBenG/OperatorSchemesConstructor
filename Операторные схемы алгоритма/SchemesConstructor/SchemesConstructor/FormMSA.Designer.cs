namespace SchemesConstructor
{
    partial class FormMSA
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
            this.groupBoxMatrix = new System.Windows.Forms.GroupBox();
            this.dataGridViewMSA = new System.Windows.Forms.DataGridView();
            this.groupBoxInformation = new System.Windows.Forms.GroupBox();
            this.labelTime = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.labelNumberErrors = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.progressBar = new System.Windows.Forms.ProgressBar();
            this.label3 = new System.Windows.Forms.Label();
            this.labelCondition = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.groupBoxMatrix.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewMSA)).BeginInit();
            this.groupBoxInformation.SuspendLayout();
            this.SuspendLayout();
            // 
            // groupBoxMatrix
            // 
            this.groupBoxMatrix.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBoxMatrix.BackColor = System.Drawing.Color.LightGray;
            this.groupBoxMatrix.Controls.Add(this.dataGridViewMSA);
            this.groupBoxMatrix.Font = new System.Drawing.Font("Calibri", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.groupBoxMatrix.Location = new System.Drawing.Point(12, 12);
            this.groupBoxMatrix.Name = "groupBoxMatrix";
            this.groupBoxMatrix.Size = new System.Drawing.Size(1239, 577);
            this.groupBoxMatrix.TabIndex = 2;
            this.groupBoxMatrix.TabStop = false;
            this.groupBoxMatrix.Text = "Матрица";
            // 
            // dataGridViewMSA
            // 
            this.dataGridViewMSA.AllowUserToAddRows = false;
            this.dataGridViewMSA.AllowUserToDeleteRows = false;
            this.dataGridViewMSA.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.dataGridViewMSA.AutoSizeRowsMode = System.Windows.Forms.DataGridViewAutoSizeRowsMode.AllHeaders;
            this.dataGridViewMSA.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridViewMSA.Location = new System.Drawing.Point(7, 30);
            this.dataGridViewMSA.Name = "dataGridViewMSA";
            this.dataGridViewMSA.RowHeadersWidthSizeMode = System.Windows.Forms.DataGridViewRowHeadersWidthSizeMode.AutoSizeToAllHeaders;
            this.dataGridViewMSA.Size = new System.Drawing.Size(1226, 541);
            this.dataGridViewMSA.TabIndex = 0;
            this.dataGridViewMSA.CellEndEdit += new System.Windows.Forms.DataGridViewCellEventHandler(this.dataGridViewMSA_CellEndEdit);
            this.dataGridViewMSA.CellValueChanged += new System.Windows.Forms.DataGridViewCellEventHandler(this.dataGridViewMSA_CellValueChanged);
            this.dataGridViewMSA.EditingControlShowing += new System.Windows.Forms.DataGridViewEditingControlShowingEventHandler(this.dataGridViewMSA_EditingControlShowing);
            this.dataGridViewMSA.KeyUp += new System.Windows.Forms.KeyEventHandler(this.dataGridViewMSA_KeyUp);
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
            this.groupBoxInformation.TabIndex = 3;
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
            // FormMSA
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1264, 681);
            this.Controls.Add(this.groupBoxInformation);
            this.Controls.Add(this.groupBoxMatrix);
            this.Name = "FormMSA";
            this.Text = "Матричная схема алгоритма";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.FormMSA_FormClosing);
            this.VisibleChanged += new System.EventHandler(this.FormMSA_VisibleChanged);
            this.groupBoxMatrix.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewMSA)).EndInit();
            this.groupBoxInformation.ResumeLayout(false);
            this.groupBoxInformation.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox groupBoxMatrix;
        private System.Windows.Forms.DataGridView dataGridViewMSA;
        private System.Windows.Forms.GroupBox groupBoxInformation;
        private System.Windows.Forms.Label labelTime;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label labelNumberErrors;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.ProgressBar progressBar;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label labelCondition;
        private System.Windows.Forms.Label label1;
    }
}