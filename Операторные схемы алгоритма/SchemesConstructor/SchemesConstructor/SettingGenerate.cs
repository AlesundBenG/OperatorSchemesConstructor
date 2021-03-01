using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SchemesConstructor
{
    public partial class SettingGenerate :Form
    {
        /////////////////////////////////////Свойства класса/////////////////////////////////////
        
        ///
        /// <summary>
        /// Количество опреаторных вершин, выбранных пользователем.
        /// </summary>
        public int NumberOperator
        {
            get
            {
                return trackBarNumberOperator.Value;
            }
        }

        /// <summary>
        /// Количество логических условий, выбранных пользователем.
        /// </summary>
        public int NumberCondition
        {
            get
            {
                return trackBarNumberCondition.Value;
            }
        }



        /////////////////////////////////////Public методы/////////////////////////////////////
        
        /// <summary>
        /// Конструктор.
        /// </summary>
        public SettingGenerate()
        {
            //Инициализация компонентов.
            InitializeComponent();

            //Перечисление всех установленных свойств.
            this.Size                       = new Size(400, 300);
            this.FormBorderStyle            = FormBorderStyle.FixedDialog;  //Фиксированный размер формы.

            groupBoxSetting.BackColor       = Color.LightGray;
            groupBoxSetting.Anchor          = (AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right);
            groupBoxSetting.Text            = "Параметры генерации задания";
            groupBoxSetting.Font            = new Font("calibri", 14, FontStyle.Bold);

            label1.Text                     = "Количество операторов:";
            label1.Font                     = new Font("calibri", 14, FontStyle.Regular);

            label2.Text                     = "Количество логических условий:";
            label2.Font                     = new Font("calibri", 14, FontStyle.Regular);

            labelNumberOperator.Text        = "10";
            labelNumberOperator.Font        = new Font("calibri", 14, FontStyle.Regular);

            labelNumberCondition.Text       = "8";
            labelNumberCondition.Font       = new Font("calibri", 14, FontStyle.Regular);

            trackBarNumberOperator.Minimum  = 2;    //Как минимум алгоритм должен иметь начальный и конечный оператор.
            trackBarNumberOperator.Maximum  = 15;
            trackBarNumberOperator.Value    = 10;
            trackBarNumberOperator.Size     = new Size(280, 45);

            trackBarNumberCondition.Minimum = 0;
            trackBarNumberCondition.Maximum = 15;
            trackBarNumberCondition.Value   = 8;
            trackBarNumberCondition.Size    = new Size(280, 45);

            buttonGenerate.BackColor        = Color.LimeGreen;
            buttonGenerate.FlatStyle        = FlatStyle.Flat;
            buttonGenerate.Size             = new Size(143, 35);
            buttonGenerate.Text             = "Сгенерировать";
            buttonGenerate.Font             = new Font("calibri", 14, FontStyle.Bold);

            buttonCancel.BackColor          = Color.LimeGreen;
            buttonCancel.FlatStyle          = FlatStyle.Flat;
            buttonCancel.Size               = new Size(88, 35);
            buttonCancel.Text               = "Отмена";
            buttonCancel.Font               = new Font("calibri", 14, FontStyle.Bold);
        }



        /////////////////////////////////////Private методы/////////////////////////////////////

        //************************************************************************************************
        // Кнопки.
        //************************************************************************************************

        private void buttonGenerate_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.OK;
            this.Close();
        }

        private void buttonCancel_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
            this.Close();
        }


        //************************************************************************************************
        // Изменение параметров генерации.
        //************************************************************************************************

        private void trackBarNumberOperator_Scroll(object sender, EventArgs e)
        {
            labelNumberOperator.Text = trackBarNumberOperator.Value.ToString();
        }

        private void trackBarNumberCondition_Scroll(object sender, EventArgs e)
        {
            labelNumberCondition.Text = trackBarNumberCondition.Value.ToString();
        }
    }
}
