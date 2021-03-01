using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SchemesConstructor
{
    public partial class FormStatistics :Form
    {
        ////////////////////////////////////Свойства класса/////////////////////////////////////
        public string NumberOperator
        {
            set
            {
                labelNumberOperator.Text = value;
            }
            get
            {
                return labelNumberOperator.Text;
            }
        }
        public string NumberCondition
        {
            set
            {
                labelNumberCondition.Text = value;
            }
            get
            {
                return labelNumberCondition.Text;
            }
        }

        public string ConditionGSA
        {
            set
            {
                dataGridViewResult.Rows[0].Cells[0].Value = value;
            }
            get
            {
                return dataGridViewResult.Rows[0].Cells[0].Value.ToString();
            }
        }
        public string ConditionMSA
        {
            set
            {
                dataGridViewResult.Rows[1].Cells[0].Value = value;
            }
            get
            {
                return dataGridViewResult.Rows[1].Cells[0].Value.ToString();
            }
        }
        public string ConditionLSA
        {
            set
            {
                dataGridViewResult.Rows[2].Cells[0].Value = value;
            }
            get
            {
                return dataGridViewResult.Rows[2].Cells[0].Value.ToString();
            }
        }

        public string NumberErrorsGSA
        {
            set
            {
                dataGridViewResult.Rows[0].Cells[1].Value = value;
            }
            get
            {
                return dataGridViewResult.Rows[0].Cells[1].Value.ToString();
            }
        }
        public string NumberErrorsMSA
        {
            set
            {
                dataGridViewResult.Rows[1].Cells[1].Value = value;
            }
            get
            {
                return dataGridViewResult.Rows[1].Cells[1].Value.ToString();
            }
        }
        public string NumberErrorsLSA
        {
            set
            {
                dataGridViewResult.Rows[2].Cells[1].Value = value;
            }
            get
            {
                return dataGridViewResult.Rows[2].Cells[1].Value.ToString();
            }
        }

        public string TimeGSA
        {
            set
            {
                dataGridViewResult.Rows[0].Cells[2].Value = value;
            }
            get
            {
                return dataGridViewResult.Rows[0].Cells[2].Value.ToString();
            }
        }
        public string TimeMSA
        {
            set
            {
                dataGridViewResult.Rows[1].Cells[2].Value = value;
            }
            get
            {
                return dataGridViewResult.Rows[1].Cells[2].Value.ToString();
            }
        }
        public string TimeLSA
        {
            set
            {
                dataGridViewResult.Rows[2].Cells[2].Value = value;
            }
            get
            {
                return dataGridViewResult.Rows[2].Cells[2].Value.ToString();
            }
        }

        

        /////////////////////////////////////Public методы/////////////////////////////////////
        
        /// <summary>
        /// Конструктор.
        /// </summary>
        public FormStatistics()
        {
            //Инициализация компонентов.
            InitializeComponent();

            //Перечисление всех установленных свойств.
            this.Size                   = new Size(770, 435);
            this.FormBorderStyle        = FormBorderStyle.FixedDialog;  //Фиксированный размер формы.

            groupBoxSettings.BackColor  = Color.LightGray;
            groupBoxSettings.Text       = "Параметры задания";
            groupBoxSettings.Font       = new Font("calibri", 14, FontStyle.Bold);

            groupBoxResult.BackColor    = Color.LightGray;
            groupBoxResult.Text         = "Результат";
            groupBoxResult.Font         = new Font("calibri", 14, FontStyle.Bold);

            label1.Text                 = "Количество операторов:";
            label1.Font                 = new Font("calibri", 14, FontStyle.Regular);

            label2.Text                 = "Количество логических условий:";
            label2.Font                 = new Font("calibri", 14, FontStyle.Regular);

            labelNumberOperator.Text    = "-";
            labelNumberOperator.Font    = new Font("calibri", 14, FontStyle.Regular);

            labelNumberCondition.Text    = "-";
            labelNumberCondition.Font    = new Font("calibri", 14, FontStyle.Regular);


            dataGridViewResult.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.AllHeaders;  //Размер строки по содержимому.
            dataGridViewResult.Rows.Add(2);
            dataGridViewResult.Rows[0].HeaderCell.Value = "ГСА";
            dataGridViewResult.Rows[1].HeaderCell.Value = "МСА";
            dataGridViewResult.Rows[2].HeaderCell.Value = "ЛСА";
            dataGridViewResult.ReadOnly                 = true;
            dataGridViewResult.DefaultCellStyle.Font    = new Font("calibri", 14, FontStyle.Regular);
            dataGridViewResult.RowHeadersWidthSizeMode  = DataGridViewRowHeadersWidthSizeMode.AutoSizeToAllHeaders; //Размер столбца по содержимому.

            foreach (DataGridViewColumn column in dataGridViewResult.Columns) {
                column.SortMode = DataGridViewColumnSortMode.NotSortable;               //Отключение сортировки строк по содержимому.
            }
        }

        /// <summary>
        /// Сброс параметров статистики.
        /// </summary>
        public void resetStatistics()
        {
            NumberOperator  = "-";
            NumberCondition = "-";

            for (int i = 0; i < 3; i++) {
                for (int j = 0; j < 3; j++) {
                    dataGridViewResult.Rows[i].Cells[j].Value = "";
                }
            }
        }

        /////////////////////////////////////Private методы/////////////////////////////////////
        
        //Закрытие формы.
        private void buttonClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
