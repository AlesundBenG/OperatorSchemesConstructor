namespace SchemesConstructor
{
    partial class SettingGenerate
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
            this.groupBoxSetting = new System.Windows.Forms.GroupBox();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.trackBarNumberOperator = new System.Windows.Forms.TrackBar();
            this.trackBarNumberCondition = new System.Windows.Forms.TrackBar();
            this.labelNumberOperator = new System.Windows.Forms.Label();
            this.labelNumberCondition = new System.Windows.Forms.Label();
            this.buttonCancel = new System.Windows.Forms.Button();
            this.buttonGenerate = new System.Windows.Forms.Button();
            this.groupBoxSetting.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.trackBarNumberOperator)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.trackBarNumberCondition)).BeginInit();
            this.SuspendLayout();
            // 
            // groupBoxSetting
            // 
            this.groupBoxSetting.BackColor = System.Drawing.Color.LightGray;
            this.groupBoxSetting.Controls.Add(this.buttonGenerate);
            this.groupBoxSetting.Controls.Add(this.buttonCancel);
            this.groupBoxSetting.Controls.Add(this.labelNumberCondition);
            this.groupBoxSetting.Controls.Add(this.labelNumberOperator);
            this.groupBoxSetting.Controls.Add(this.trackBarNumberCondition);
            this.groupBoxSetting.Controls.Add(this.trackBarNumberOperator);
            this.groupBoxSetting.Controls.Add(this.label2);
            this.groupBoxSetting.Controls.Add(this.label1);
            this.groupBoxSetting.Font = new System.Drawing.Font("Calibri", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.groupBoxSetting.Location = new System.Drawing.Point(13, 13);
            this.groupBoxSetting.Name = "groupBoxSetting";
            this.groupBoxSetting.Size = new System.Drawing.Size(359, 236);
            this.groupBoxSetting.TabIndex = 0;
            this.groupBoxSetting.TabStop = false;
            this.groupBoxSetting.Text = "Параметры генерации задания";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Calibri", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.label1.Location = new System.Drawing.Point(6, 36);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(205, 23);
            this.label1.TabIndex = 0;
            this.label1.Text = "Количество операторов:";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Calibri", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.label2.Location = new System.Drawing.Point(6, 110);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(271, 23);
            this.label2.TabIndex = 1;
            this.label2.Text = "Количество логических условий:";
            // 
            // trackBarNumberOperator
            // 
            this.trackBarNumberOperator.Location = new System.Drawing.Point(10, 62);
            this.trackBarNumberOperator.Maximum = 15;
            this.trackBarNumberOperator.Minimum = 2;
            this.trackBarNumberOperator.Name = "trackBarNumberOperator";
            this.trackBarNumberOperator.Size = new System.Drawing.Size(280, 45);
            this.trackBarNumberOperator.TabIndex = 2;
            this.trackBarNumberOperator.Value = 10;
            this.trackBarNumberOperator.Scroll += new System.EventHandler(this.trackBarNumberOperator_Scroll);
            // 
            // trackBarNumberCondition
            // 
            this.trackBarNumberCondition.Location = new System.Drawing.Point(10, 136);
            this.trackBarNumberCondition.Maximum = 15;
            this.trackBarNumberCondition.Name = "trackBarNumberCondition";
            this.trackBarNumberCondition.Size = new System.Drawing.Size(280, 45);
            this.trackBarNumberCondition.TabIndex = 3;
            this.trackBarNumberCondition.Value = 8;
            this.trackBarNumberCondition.Scroll += new System.EventHandler(this.trackBarNumberCondition_Scroll);
            // 
            // labelNumberOperator
            // 
            this.labelNumberOperator.AutoSize = true;
            this.labelNumberOperator.Font = new System.Drawing.Font("Calibri", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.labelNumberOperator.Location = new System.Drawing.Point(293, 36);
            this.labelNumberOperator.Name = "labelNumberOperator";
            this.labelNumberOperator.Size = new System.Drawing.Size(30, 23);
            this.labelNumberOperator.TabIndex = 4;
            this.labelNumberOperator.Text = "10";
            // 
            // labelNumberCondition
            // 
            this.labelNumberCondition.AutoSize = true;
            this.labelNumberCondition.Font = new System.Drawing.Font("Calibri", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.labelNumberCondition.Location = new System.Drawing.Point(293, 110);
            this.labelNumberCondition.Name = "labelNumberCondition";
            this.labelNumberCondition.Size = new System.Drawing.Size(20, 23);
            this.labelNumberCondition.TabIndex = 5;
            this.labelNumberCondition.Text = "8";
            // 
            // buttonCancel
            // 
            this.buttonCancel.BackColor = System.Drawing.Color.LimeGreen;
            this.buttonCancel.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.buttonCancel.Location = new System.Drawing.Point(265, 195);
            this.buttonCancel.Name = "buttonCancel";
            this.buttonCancel.Size = new System.Drawing.Size(88, 35);
            this.buttonCancel.TabIndex = 6;
            this.buttonCancel.Text = "Отмена";
            this.buttonCancel.UseVisualStyleBackColor = false;
            this.buttonCancel.Click += new System.EventHandler(this.buttonCancel_Click);
            // 
            // buttonGenerate
            // 
            this.buttonGenerate.BackColor = System.Drawing.Color.LimeGreen;
            this.buttonGenerate.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.buttonGenerate.Location = new System.Drawing.Point(104, 195);
            this.buttonGenerate.Name = "buttonGenerate";
            this.buttonGenerate.Size = new System.Drawing.Size(143, 35);
            this.buttonGenerate.TabIndex = 7;
            this.buttonGenerate.Text = "Сгенерировать";
            this.buttonGenerate.UseVisualStyleBackColor = false;
            this.buttonGenerate.Click += new System.EventHandler(this.buttonGenerate_Click);
            // 
            // SettingGenerate
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(384, 261);
            this.Controls.Add(this.groupBoxSetting);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Name = "SettingGenerate";
            this.Text = "Генерация задания";
            this.groupBoxSetting.ResumeLayout(false);
            this.groupBoxSetting.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.trackBarNumberOperator)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.trackBarNumberCondition)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox groupBoxSetting;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TrackBar trackBarNumberCondition;
        private System.Windows.Forms.TrackBar trackBarNumberOperator;
        private System.Windows.Forms.Label labelNumberCondition;
        private System.Windows.Forms.Label labelNumberOperator;
        private System.Windows.Forms.Button buttonGenerate;
        private System.Windows.Forms.Button buttonCancel;
    }
}