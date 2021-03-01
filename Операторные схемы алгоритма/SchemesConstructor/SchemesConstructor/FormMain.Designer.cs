namespace SchemesConstructor
{
    partial class FormMain
    {
        /// <summary>
        /// Обязательная переменная конструктора.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Освободить все используемые ресурсы.
        /// </summary>
        /// <param name="disposing">истинно, если управляемый ресурс должен быть удален; иначе ложно.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null)) {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Код, автоматически созданный конструктором форм Windows

        /// <summary>
        /// Требуемый метод для поддержки конструктора — не изменяйте 
        /// содержимое этого метода с помощью редактора кода.
        /// </summary>
        private void InitializeComponent()
        {
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.генерацияЗаданияToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.генерацияToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.результатВыполненяToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.инструкцияToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.оПрограммеToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.выходToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.groupBoxAlgorthm = new System.Windows.Forms.GroupBox();
            this.textBoxAlgorithm = new System.Windows.Forms.TextBox();
            this.groupBoxScheme = new System.Windows.Forms.GroupBox();
            this.buttonLSA = new System.Windows.Forms.Button();
            this.buttonMSA = new System.Windows.Forms.Button();
            this.buttonGSA = new System.Windows.Forms.Button();
            this.menuStrip1.SuspendLayout();
            this.groupBoxAlgorthm.SuspendLayout();
            this.groupBoxScheme.SuspendLayout();
            this.SuspendLayout();
            // 
            // menuStrip1
            // 
            this.menuStrip1.BackColor = System.Drawing.Color.LightGray;
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.генерацияЗаданияToolStripMenuItem,
            this.инструкцияToolStripMenuItem,
            this.оПрограммеToolStripMenuItem,
            this.выходToolStripMenuItem});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Size = new System.Drawing.Size(384, 24);
            this.menuStrip1.TabIndex = 0;
            this.menuStrip1.Text = "menuStrip1";
            // 
            // генерацияЗаданияToolStripMenuItem
            // 
            this.генерацияЗаданияToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.генерацияToolStripMenuItem,
            this.результатВыполненяToolStripMenuItem});
            this.генерацияЗаданияToolStripMenuItem.Name = "генерацияЗаданияToolStripMenuItem";
            this.генерацияЗаданияToolStripMenuItem.Size = new System.Drawing.Size(64, 20);
            this.генерацияЗаданияToolStripMenuItem.Text = "Задание";
            // 
            // генерацияToolStripMenuItem
            // 
            this.генерацияToolStripMenuItem.Name = "генерацияToolStripMenuItem";
            this.генерацияToolStripMenuItem.Size = new System.Drawing.Size(192, 22);
            this.генерацияToolStripMenuItem.Text = "Генерация";
            this.генерацияToolStripMenuItem.Click += new System.EventHandler(this.генерацияToolStripMenuItem_Click);
            // 
            // результатВыполненяToolStripMenuItem
            // 
            this.результатВыполненяToolStripMenuItem.Name = "результатВыполненяToolStripMenuItem";
            this.результатВыполненяToolStripMenuItem.Size = new System.Drawing.Size(192, 22);
            this.результатВыполненяToolStripMenuItem.Text = "Результат выполненя";
            this.результатВыполненяToolStripMenuItem.Click += new System.EventHandler(this.результатВыполненяToolStripMenuItem_Click);
            // 
            // инструкцияToolStripMenuItem
            // 
            this.инструкцияToolStripMenuItem.Name = "инструкцияToolStripMenuItem";
            this.инструкцияToolStripMenuItem.Size = new System.Drawing.Size(85, 20);
            this.инструкцияToolStripMenuItem.Text = "Инструкция";
            this.инструкцияToolStripMenuItem.Click += new System.EventHandler(this.инструкцияToolStripMenuItem_Click);
            // 
            // оПрограммеToolStripMenuItem
            // 
            this.оПрограммеToolStripMenuItem.Name = "оПрограммеToolStripMenuItem";
            this.оПрограммеToolStripMenuItem.Size = new System.Drawing.Size(94, 20);
            this.оПрограммеToolStripMenuItem.Text = "О программе";
            this.оПрограммеToolStripMenuItem.Click += new System.EventHandler(this.оПрограммеToolStripMenuItem_Click);
            // 
            // выходToolStripMenuItem
            // 
            this.выходToolStripMenuItem.Name = "выходToolStripMenuItem";
            this.выходToolStripMenuItem.Size = new System.Drawing.Size(53, 20);
            this.выходToolStripMenuItem.Text = "Выход";
            this.выходToolStripMenuItem.Click += new System.EventHandler(this.выходToolStripMenuItem_Click);
            // 
            // groupBoxAlgorthm
            // 
            this.groupBoxAlgorthm.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBoxAlgorthm.BackColor = System.Drawing.Color.LightGray;
            this.groupBoxAlgorthm.Controls.Add(this.textBoxAlgorithm);
            this.groupBoxAlgorthm.Font = new System.Drawing.Font("Calibri", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.groupBoxAlgorthm.Location = new System.Drawing.Point(12, 36);
            this.groupBoxAlgorthm.Name = "groupBoxAlgorthm";
            this.groupBoxAlgorthm.Size = new System.Drawing.Size(360, 538);
            this.groupBoxAlgorthm.TabIndex = 1;
            this.groupBoxAlgorthm.TabStop = false;
            this.groupBoxAlgorthm.Text = "Словесное описание алгоритма";
            // 
            // textBoxAlgorithm
            // 
            this.textBoxAlgorithm.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.textBoxAlgorithm.Font = new System.Drawing.Font("Calibri", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.textBoxAlgorithm.Location = new System.Drawing.Point(7, 31);
            this.textBoxAlgorithm.Multiline = true;
            this.textBoxAlgorithm.Name = "textBoxAlgorithm";
            this.textBoxAlgorithm.ReadOnly = true;
            this.textBoxAlgorithm.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this.textBoxAlgorithm.Size = new System.Drawing.Size(347, 501);
            this.textBoxAlgorithm.TabIndex = 0;
            this.textBoxAlgorithm.WordWrap = false;
            // 
            // groupBoxScheme
            // 
            this.groupBoxScheme.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBoxScheme.BackColor = System.Drawing.Color.LightGray;
            this.groupBoxScheme.Controls.Add(this.buttonLSA);
            this.groupBoxScheme.Controls.Add(this.buttonMSA);
            this.groupBoxScheme.Controls.Add(this.buttonGSA);
            this.groupBoxScheme.Font = new System.Drawing.Font("Calibri", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.groupBoxScheme.Location = new System.Drawing.Point(12, 580);
            this.groupBoxScheme.Name = "groupBoxScheme";
            this.groupBoxScheme.Size = new System.Drawing.Size(360, 89);
            this.groupBoxScheme.TabIndex = 2;
            this.groupBoxScheme.TabStop = false;
            this.groupBoxScheme.Text = "Схемы";
            this.groupBoxScheme.SizeChanged += new System.EventHandler(this.groupBoxScheme_SizeChanged);
            // 
            // buttonLSA
            // 
            this.buttonLSA.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonLSA.Enabled = false;
            this.buttonLSA.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.buttonLSA.Location = new System.Drawing.Point(265, 40);
            this.buttonLSA.Name = "buttonLSA";
            this.buttonLSA.Size = new System.Drawing.Size(90, 36);
            this.buttonLSA.TabIndex = 2;
            this.buttonLSA.Text = "ЛСА";
            this.buttonLSA.UseVisualStyleBackColor = true;
            this.buttonLSA.Click += new System.EventHandler(this.buttonLSA_Click);
            // 
            // buttonMSA
            // 
            this.buttonMSA.Enabled = false;
            this.buttonMSA.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.buttonMSA.Location = new System.Drawing.Point(135, 40);
            this.buttonMSA.Name = "buttonMSA";
            this.buttonMSA.Size = new System.Drawing.Size(90, 36);
            this.buttonMSA.TabIndex = 1;
            this.buttonMSA.Text = "МСА";
            this.buttonMSA.UseVisualStyleBackColor = true;
            this.buttonMSA.Click += new System.EventHandler(this.buttonMSA_Click);
            // 
            // buttonGSA
            // 
            this.buttonGSA.Enabled = false;
            this.buttonGSA.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.buttonGSA.Location = new System.Drawing.Point(5, 40);
            this.buttonGSA.Name = "buttonGSA";
            this.buttonGSA.Size = new System.Drawing.Size(90, 36);
            this.buttonGSA.TabIndex = 0;
            this.buttonGSA.Text = "ГСА";
            this.buttonGSA.UseVisualStyleBackColor = true;
            this.buttonGSA.Click += new System.EventHandler(this.buttonGSA_Click);
            // 
            // FormMain
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(384, 681);
            this.Controls.Add(this.groupBoxScheme);
            this.Controls.Add(this.groupBoxAlgorthm);
            this.Controls.Add(this.menuStrip1);
            this.MainMenuStrip = this.menuStrip1;
            this.MinimumSize = new System.Drawing.Size(400, 720);
            this.Name = "FormMain";
            this.Text = "Операторные схемы алгоритма";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.FormMain_FormClosing);
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            this.groupBoxAlgorthm.ResumeLayout(false);
            this.groupBoxAlgorthm.PerformLayout();
            this.groupBoxScheme.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.ToolStripMenuItem генерацияЗаданияToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem инструкцияToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem оПрограммеToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem выходToolStripMenuItem;
        private System.Windows.Forms.GroupBox groupBoxAlgorthm;
        private System.Windows.Forms.ToolStripMenuItem генерацияToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem результатВыполненяToolStripMenuItem;
        private System.Windows.Forms.TextBox textBoxAlgorithm;
        private System.Windows.Forms.GroupBox groupBoxScheme;
        private System.Windows.Forms.Button buttonLSA;
        private System.Windows.Forms.Button buttonMSA;
        private System.Windows.Forms.Button buttonGSA;
    }
}

