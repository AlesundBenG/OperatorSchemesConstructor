namespace SchemesConstructor
{
    partial class FormStatistics
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
            this.groupBoxSettings = new System.Windows.Forms.GroupBox();
            this.labelNumberCondition = new System.Windows.Forms.Label();
            this.labelNumberOperator = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.groupBoxResult = new System.Windows.Forms.GroupBox();
            this.buttonClose = new System.Windows.Forms.Button();
            this.dataGridViewResult = new System.Windows.Forms.DataGridView();
            this.condition = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.errors = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.time = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.groupBoxSettings.SuspendLayout();
            this.groupBoxResult.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewResult)).BeginInit();
            this.SuspendLayout();
            // 
            // groupBoxSettings
            // 
            this.groupBoxSettings.BackColor = System.Drawing.Color.LightGray;
            this.groupBoxSettings.Controls.Add(this.labelNumberCondition);
            this.groupBoxSettings.Controls.Add(this.labelNumberOperator);
            this.groupBoxSettings.Controls.Add(this.label2);
            this.groupBoxSettings.Controls.Add(this.label1);
            this.groupBoxSettings.Font = new System.Drawing.Font("Calibri", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.groupBoxSettings.Location = new System.Drawing.Point(13, 13);
            this.groupBoxSettings.Name = "groupBoxSettings";
            this.groupBoxSettings.Size = new System.Drawing.Size(728, 117);
            this.groupBoxSettings.TabIndex = 0;
            this.groupBoxSettings.TabStop = false;
            this.groupBoxSettings.Text = "Параметры задания";
            // 
            // labelNumberCondition
            // 
            this.labelNumberCondition.AutoSize = true;
            this.labelNumberCondition.Font = new System.Drawing.Font("Calibri", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.labelNumberCondition.Location = new System.Drawing.Point(286, 73);
            this.labelNumberCondition.Name = "labelNumberCondition";
            this.labelNumberCondition.Size = new System.Drawing.Size(16, 23);
            this.labelNumberCondition.TabIndex = 3;
            this.labelNumberCondition.Text = "-";
            // 
            // labelNumberOperator
            // 
            this.labelNumberOperator.AutoSize = true;
            this.labelNumberOperator.Font = new System.Drawing.Font("Calibri", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.labelNumberOperator.Location = new System.Drawing.Point(286, 31);
            this.labelNumberOperator.Name = "labelNumberOperator";
            this.labelNumberOperator.Size = new System.Drawing.Size(16, 23);
            this.labelNumberOperator.TabIndex = 2;
            this.labelNumberOperator.Text = "-";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Calibri", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.label2.Location = new System.Drawing.Point(7, 73);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(271, 23);
            this.label2.TabIndex = 1;
            this.label2.Text = "Количество логических условий:";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Calibri", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.label1.Location = new System.Drawing.Point(7, 31);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(205, 23);
            this.label1.TabIndex = 0;
            this.label1.Text = "Количество операторов:";
            // 
            // groupBoxResult
            // 
            this.groupBoxResult.BackColor = System.Drawing.Color.LightGray;
            this.groupBoxResult.Controls.Add(this.buttonClose);
            this.groupBoxResult.Controls.Add(this.dataGridViewResult);
            this.groupBoxResult.Font = new System.Drawing.Font("Calibri", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.groupBoxResult.Location = new System.Drawing.Point(13, 136);
            this.groupBoxResult.Name = "groupBoxResult";
            this.groupBoxResult.Size = new System.Drawing.Size(728, 248);
            this.groupBoxResult.TabIndex = 1;
            this.groupBoxResult.TabStop = false;
            this.groupBoxResult.Text = "Результат";
            // 
            // buttonClose
            // 
            this.buttonClose.BackColor = System.Drawing.Color.LimeGreen;
            this.buttonClose.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.buttonClose.Location = new System.Drawing.Point(635, 206);
            this.buttonClose.Name = "buttonClose";
            this.buttonClose.Size = new System.Drawing.Size(87, 36);
            this.buttonClose.TabIndex = 1;
            this.buttonClose.Text = "Закрыть";
            this.buttonClose.UseVisualStyleBackColor = false;
            this.buttonClose.Click += new System.EventHandler(this.buttonClose_Click);
            // 
            // dataGridViewResult
            // 
            this.dataGridViewResult.AutoSizeRowsMode = System.Windows.Forms.DataGridViewAutoSizeRowsMode.AllHeaders;
            this.dataGridViewResult.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridViewResult.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.condition,
            this.errors,
            this.time});
            this.dataGridViewResult.Location = new System.Drawing.Point(7, 31);
            this.dataGridViewResult.Name = "dataGridViewResult";
            this.dataGridViewResult.ReadOnly = true;
            this.dataGridViewResult.RowHeadersWidthSizeMode = System.Windows.Forms.DataGridViewRowHeadersWidthSizeMode.AutoSizeToAllHeaders;
            this.dataGridViewResult.Size = new System.Drawing.Size(715, 150);
            this.dataGridViewResult.TabIndex = 0;
            // 
            // condition
            // 
            this.condition.HeaderText = "Состояние";
            this.condition.Name = "condition";
            this.condition.ReadOnly = true;
            this.condition.Width = 200;
            // 
            // errors
            // 
            this.errors.HeaderText = "Количество ошибок";
            this.errors.Name = "errors";
            this.errors.ReadOnly = true;
            this.errors.Width = 200;
            // 
            // time
            // 
            this.time.HeaderText = "Время выполнения";
            this.time.Name = "time";
            this.time.ReadOnly = true;
            this.time.Width = 200;
            // 
            // FormStatistics
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(754, 396);
            this.Controls.Add(this.groupBoxResult);
            this.Controls.Add(this.groupBoxSettings);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Name = "FormStatistics";
            this.Text = "Результат выполнения задания";
            this.groupBoxSettings.ResumeLayout(false);
            this.groupBoxSettings.PerformLayout();
            this.groupBoxResult.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewResult)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox groupBoxSettings;
        private System.Windows.Forms.Label labelNumberCondition;
        private System.Windows.Forms.Label labelNumberOperator;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.GroupBox groupBoxResult;
        private System.Windows.Forms.DataGridView dataGridViewResult;
        private System.Windows.Forms.Button buttonClose;
        private System.Windows.Forms.DataGridViewTextBoxColumn condition;
        private System.Windows.Forms.DataGridViewTextBoxColumn errors;
        private System.Windows.Forms.DataGridViewTextBoxColumn time;
    }
}