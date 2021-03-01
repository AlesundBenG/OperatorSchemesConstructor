namespace SchemesConstructor
{
    partial class FormGSA
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
            this.groupBoxInformation = new System.Windows.Forms.GroupBox();
            this.labelTime = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.labelNumberErrors = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.progressBar = new System.Windows.Forms.ProgressBar();
            this.label3 = new System.Windows.Forms.Label();
            this.labelCondition = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.panel1 = new System.Windows.Forms.Panel();
            this.pictureBoxGraph = new System.Windows.Forms.PictureBox();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.buttonCreateOperator = new System.Windows.Forms.Button();
            this.buttonCreateCondition = new System.Windows.Forms.Button();
            this.comboBoxCondition = new System.Windows.Forms.ComboBox();
            this.comboBoxOperator = new System.Windows.Forms.ComboBox();
            this.groupBoxInformation.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxGraph)).BeginInit();
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
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
            this.groupBoxInformation.Location = new System.Drawing.Point(11, 594);
            this.groupBoxInformation.Name = "groupBoxInformation";
            this.groupBoxInformation.Size = new System.Drawing.Size(1239, 74);
            this.groupBoxInformation.TabIndex = 6;
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
            // groupBox2
            // 
            this.groupBox2.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox2.BackColor = System.Drawing.Color.LightGray;
            this.groupBox2.Controls.Add(this.panel1);
            this.groupBox2.Font = new System.Drawing.Font("Calibri", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.groupBox2.Location = new System.Drawing.Point(172, 12);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(1079, 576);
            this.groupBox2.TabIndex = 5;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Графическая схема алгоритма";
            // 
            // panel1
            // 
            this.panel1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.panel1.AutoScroll = true;
            this.panel1.Controls.Add(this.pictureBoxGraph);
            this.panel1.Location = new System.Drawing.Point(6, 31);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(1067, 539);
            this.panel1.TabIndex = 0;
            // 
            // pictureBoxGraph
            // 
            this.pictureBoxGraph.BackColor = System.Drawing.Color.WhiteSmoke;
            this.pictureBoxGraph.Location = new System.Drawing.Point(0, 0);
            this.pictureBoxGraph.Name = "pictureBoxGraph";
            this.pictureBoxGraph.Size = new System.Drawing.Size(1260, 1000);
            this.pictureBoxGraph.TabIndex = 0;
            this.pictureBoxGraph.TabStop = false;
            this.pictureBoxGraph.MouseDown += new System.Windows.Forms.MouseEventHandler(this.pictureBoxGraph_MouseDown);
            this.pictureBoxGraph.MouseMove += new System.Windows.Forms.MouseEventHandler(this.pictureBoxGraph_MouseMove);
            this.pictureBoxGraph.MouseUp += new System.Windows.Forms.MouseEventHandler(this.pictureBoxGraph_MouseUp);
            // 
            // groupBox1
            // 
            this.groupBox1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
            this.groupBox1.BackColor = System.Drawing.Color.LightGray;
            this.groupBox1.Controls.Add(this.buttonCreateOperator);
            this.groupBox1.Controls.Add(this.buttonCreateCondition);
            this.groupBox1.Controls.Add(this.comboBoxCondition);
            this.groupBox1.Controls.Add(this.comboBoxOperator);
            this.groupBox1.Font = new System.Drawing.Font("Calibri", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.groupBox1.Location = new System.Drawing.Point(12, 12);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(154, 576);
            this.groupBox1.TabIndex = 4;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Панедль инструментов";
            // 
            // buttonCreateOperator
            // 
            this.buttonCreateOperator.AutoSize = true;
            this.buttonCreateOperator.BackColor = System.Drawing.Color.LightGreen;
            this.buttonCreateOperator.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.buttonCreateOperator.Font = new System.Drawing.Font("Calibri", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.buttonCreateOperator.Location = new System.Drawing.Point(7, 101);
            this.buttonCreateOperator.Name = "buttonCreateOperator";
            this.buttonCreateOperator.Size = new System.Drawing.Size(100, 35);
            this.buttonCreateOperator.TabIndex = 3;
            this.buttonCreateOperator.Text = "Создать";
            this.buttonCreateOperator.UseVisualStyleBackColor = false;
            this.buttonCreateOperator.Click += new System.EventHandler(this.buttonCreateOperator_Click);
            // 
            // buttonCreateCondition
            // 
            this.buttonCreateCondition.AutoSize = true;
            this.buttonCreateCondition.BackColor = System.Drawing.Color.PaleGreen;
            this.buttonCreateCondition.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.buttonCreateCondition.Font = new System.Drawing.Font("Calibri", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.buttonCreateCondition.Location = new System.Drawing.Point(6, 194);
            this.buttonCreateCondition.Name = "buttonCreateCondition";
            this.buttonCreateCondition.Size = new System.Drawing.Size(100, 35);
            this.buttonCreateCondition.TabIndex = 2;
            this.buttonCreateCondition.Text = "Создать";
            this.buttonCreateCondition.UseVisualStyleBackColor = false;
            this.buttonCreateCondition.Click += new System.EventHandler(this.buttonCreateCondition_Click);
            // 
            // comboBoxCondition
            // 
            this.comboBoxCondition.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBoxCondition.Font = new System.Drawing.Font("Calibri", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.comboBoxCondition.FormattingEnabled = true;
            this.comboBoxCondition.Location = new System.Drawing.Point(7, 157);
            this.comboBoxCondition.Name = "comboBoxCondition";
            this.comboBoxCondition.Size = new System.Drawing.Size(141, 31);
            this.comboBoxCondition.TabIndex = 1;
            // 
            // comboBoxOperator
            // 
            this.comboBoxOperator.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBoxOperator.Font = new System.Drawing.Font("Calibri", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.comboBoxOperator.FormattingEnabled = true;
            this.comboBoxOperator.Location = new System.Drawing.Point(7, 64);
            this.comboBoxOperator.Name = "comboBoxOperator";
            this.comboBoxOperator.Size = new System.Drawing.Size(141, 31);
            this.comboBoxOperator.TabIndex = 0;
            // 
            // FormGSA
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1264, 681);
            this.Controls.Add(this.groupBoxInformation);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.groupBox1);
            this.Name = "FormGSA";
            this.Text = "Графическая схема алгоритма";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.FormGSA_FormClosing);
            this.VisibleChanged += new System.EventHandler(this.FormGSA_VisibleChanged);
            this.groupBoxInformation.ResumeLayout(false);
            this.groupBoxInformation.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.panel1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxGraph)).EndInit();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox groupBoxInformation;
        private System.Windows.Forms.Label labelTime;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label labelNumberErrors;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.ProgressBar progressBar;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label labelCondition;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.PictureBox pictureBoxGraph;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Button buttonCreateOperator;
        private System.Windows.Forms.Button buttonCreateCondition;
        private System.Windows.Forms.ComboBox comboBoxCondition;
        private System.Windows.Forms.ComboBox comboBoxOperator;
    }
}