using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using OperatorAlgorithm;        //Класс представления операторного алгоритма.
using AlgorithmGenerator;       //Класс генерации операторного алгоритма.

namespace SchemesConstructor
{
    public partial class FormMain :Form
    {
        /////////////////////////////////////Поля класса/////////////////////////////////////
        private Algorithm       algorithm;      //Алгоритм задания.
        private SettingGenerate formSetting;    //Форма для настройки генерации задания.
        private FormStatistics  formStatistics; //Форма результата.
        private FormGSA         formGSA;        //Форма для построения ГСА.
        private FormMSA         formMSA;        //Форма для построения МСА.
        private FormLSA         formLSA;        //Форма для построения ЛСА.



        /////////////////////////////////////Public методы/////////////////////////////////////

        /// <summary>
        /// Конструктор..
        /// </summary>
        public FormMain()
        {
            //Инициализация компонентов.
            InitializeComponent();

            //Перечисление всех установленных свойств.
            this.Size                   = new Size(400, 720);
            this.MinimumSize            = new Size(400, 720);

            groupBoxAlgorthm.BackColor  = Color.LightGray;
            groupBoxAlgorthm.Anchor     = (AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right);
            groupBoxAlgorthm.Text       = "Словесное описание алгоритма";
            groupBoxAlgorthm.Font       = new Font("calibri", 14, FontStyle.Bold);

            groupBoxScheme.BackColor    = Color.LightGray;
            groupBoxScheme.Anchor       = (AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right);
            groupBoxScheme.Text         = "Схемы";
            groupBoxScheme.Font         = new Font("calibri", 14, FontStyle.Bold);

            textBoxAlgorithm.Multiline  = true;
            textBoxAlgorithm.ReadOnly   = true;
            textBoxAlgorithm.Anchor     = (AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right);
            textBoxAlgorithm.ScrollBars = ScrollBars.Both;
            textBoxAlgorithm.Font       = new Font("calibri", 14, FontStyle.Regular);
            textBoxAlgorithm.WordWrap   = false;    //Отключение переноса на другую строку, если текст не влазит в textbox.

            buttonGSA.BackColor         = Color.LightGray;
            buttonGSA.FlatStyle         = FlatStyle.Flat;
            buttonGSA.Location          = new Point(5, 40);
            buttonGSA.Size              = new Size(90,36);
            buttonGSA.Text              = "ГСА";
            buttonGSA.Font              = new Font("calibri", 14, FontStyle.Bold);
            buttonGSA.Enabled           = false;    //Активной становится тогда, когда задание сгенерировано.

            buttonMSA.BackColor         = Color.LightGray;
            buttonMSA.FlatStyle         = FlatStyle.Flat;
            buttonMSA.Location          = new Point(135, 40);
            buttonMSA.Size              = new Size(90, 36);
            buttonMSA.Text              = "МСА";
            buttonMSA.Font              = new Font("calibri", 14, FontStyle.Bold);
            buttonMSA.Enabled           = false;    //Активной становится тогда, когда задание сгенерировано.

            buttonLSA.BackColor         = Color.LightGray;
            buttonLSA.Anchor            = (AnchorStyles.Top |  AnchorStyles.Right);
            buttonLSA.FlatStyle         = FlatStyle.Flat;
            buttonLSA.Location          = new Point(265, 40);
            buttonLSA.Size              = new Size(90, 36);
            buttonLSA.Text              = "ЛСА";
            buttonLSA.Font              = new Font("calibri", 14, FontStyle.Bold);
            buttonLSA.Enabled           = false;    //Активной становится тогда, когда задание сгенерировано.

            //Поля класса.
            algorithm       = null;
            formSetting     = new SettingGenerate();
            formStatistics  = new FormStatistics();
            formGSA         = null;
            formMSA         = null;
            formLSA         = null;
        }

        /// <summary>
        /// Установка выполнения задания по ГСА.
        /// </summary>
        /// <param name="done">Выполнено или нет</param>
        public void setDoneGSA(bool done)
        {
            if (done) {
                buttonGSA.BackColor             = Color.LimeGreen;
                formStatistics.ConditionGSA     = "Выполнено";
                formStatistics.NumberErrorsGSA  = formGSA.NumberError.ToString();
                formStatistics.TimeGSA          = formGSA.ExecutionTime;
            }
            else {
                buttonGSA.BackColor             = Color.Gold;
                formStatistics.ConditionGSA     = "Не выполнено";
                formStatistics.NumberErrorsGSA  = "";
                formStatistics.TimeGSA          = "";
            }
        }

        /// <summary>
        /// Установка выполнения задания по МСА.
        /// </summary>
        /// <param name="done">Выполнено или нет</param>
        public void setDoneMSA(bool done)
        {
            if (done) {
                buttonMSA.BackColor             = Color.LimeGreen;
                formStatistics.ConditionMSA     = "Выполнено";
                formStatistics.NumberErrorsMSA  = formMSA.NumberError.ToString();
                formStatistics.TimeMSA          = formMSA.ExecutionTime;
            }
            else {
                buttonMSA.BackColor             = Color.Gold;
                formStatistics.ConditionMSA     = "Не выполнено";
                formStatistics.NumberErrorsMSA  = "";
                formStatistics.TimeMSA          = "";
            }
        }

        /// <summary>
        /// Устанвока выполнения задания по ЛСА.
        /// </summary>
        /// <param name="done">Выполнено или нет</param>
        public void setDoneLSA(bool done)
        {
            if (done) {
                buttonLSA.BackColor             = Color.LimeGreen;
                formStatistics.ConditionLSA     = "Выполнено";
                formStatistics.NumberErrorsLSA  = formLSA.NumberError.ToString();
                formStatistics.TimeLSA          = formLSA.ExecutionTime;
            }
            else {
                buttonLSA.BackColor             = Color.Gold;
                formStatistics.ConditionLSA     = "Не выполнено";
                formStatistics.NumberErrorsLSA  = "";
                formStatistics.TimeLSA          = "";
            }
        }



        /////////////////////////////////////Private методы/////////////////////////////////////

        //************************************************************************************************
        // Меню.
        //************************************************************************************************

        private void генерацияToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //Соглашение с новой генерацией.
            bool generate;
            if (algorithm != null) {
                DialogResult result = MessageBox.Show("При генерации задания данные будут потеряны, продолжить?", "Предупреждение", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                generate = (result == DialogResult.Yes);
            }
            else {
                generate = true;
            }

            //Генерация.
            if (generate) {
                if (formSetting.ShowDialog() == DialogResult.OK) {
                    //Генерация алгоритма.
                    Generator   generator   = new Generator();
                    int         nOperator   = formSetting.NumberOperator;
                    int         nCondition  = formSetting.NumberCondition;
                    algorithm = generator.generate(nOperator, nCondition, "A", "p", "0", "k");
                    //Отображение алгоритма.
                    showAlgorithm();
                    //Инициализация компонентов для открытия форм.
                    initializationForm();
                    //Настройка статистики.
                    resetStatistics(nOperator, nCondition);
                }
            }


            /////////////////////////////////////Локальные функции/////////////////////////////////////      
            //Отображение алгоритма в textBox.
            void showAlgorithm()
            {
                textBoxAlgorithm.Clear();                                                           //Очистка поля для отображения алгоритма.
                for (int i = 0; i < algorithm.NumberOperator - 1; i++) {
                    textBoxAlgorithm.Text +=
                        "from " + algorithm.getOperatorVertex(i).Name       +                       //Вершина, из которой идет переход.
                        " to "  + algorithm.getOperatorVertex(i).VertexTo   +                       //Вершина, в которую происходит переход.
                        Environment.NewLine;                                                        //Переход на новую строку.
                }
                for (int i = 0; i < algorithm.NumberCondition; i++) {
                    textBoxAlgorithm.Text +=
                        "from "         + algorithm.getConditionVertex(i).Name +                    //Вершина, из которой идет переход.
                        " to "          + algorithm.getConditionVertex(i).VertexTo_IfTrue   +       //Вершина, в которую происходит переход при истине.
                        " (if true) or" +
                        " to "          + algorithm.getConditionVertex(i).VertexTo_IfFalse  +       //Вершина, в которую происходит переход при лжи.
                        " (if false)"   +
                        Environment.NewLine;                                                        //Переход на новую строку.
                }
                textBoxAlgorithm.Select(0, 0);                                                      //Снятие выделения после вставки текста.
            }

            //Инициализация компонентов для открытия форм.
            void initializationForm()
            {
                //Активация кнопок схем.
                buttonGSA.Enabled   = true;
                buttonMSA.Enabled   = true;
                buttonLSA.Enabled   = true;

                //Выделение кнопок схем.
                buttonGSA.BackColor = Color.Gold;
                buttonMSA.BackColor = Color.Gold;
                buttonLSA.BackColor = Color.Gold;

                //Закрытие форм, которые остались от прошлого задания.
                if (formGSA != null) {
                    formGSA.Close();
                    formGSA = null;
                }
                if (formMSA != null) {
                    formMSA.Close();
                    formMSA = null;
                }
                if (formLSA != null) {
                    formLSA = null;
                    formLSA.Close();
                }
            }

            //Перезагрузка статистики.
            void resetStatistics(int nOperator, int nCondition)
            {
                formStatistics.resetStatistics();
                formStatistics.NumberOperator   = nOperator.ToString();
                formStatistics.NumberCondition  = nCondition.ToString();
                formStatistics.ConditionGSA     = "Не выполнено";
                formStatistics.ConditionMSA     = "Не выполнено";
                formStatistics.ConditionLSA     = "Не выполнено";
            }
            ///////////////////////////////////////////////////////////////////////////////////////////
        }

        private void результатВыполненяToolStripMenuItem_Click(object sender, EventArgs e)
        {
            formStatistics.ShowDialog();
        }

        private void инструкцияToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //Тут должен был быть вызвано окно с инструкцией.
        }

        private void оПрограммеToolStripMenuItem_Click(object sender, EventArgs e)
        {
            MessageBox.Show("Программа для построения и проверки операторных схем алгоритмов.\nВятГУ; 2020 год; ИВТ-41; Баташев П.А.", "О программе", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void выходToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.Close();
        }


        //************************************************************************************************
        // Панель кнопок открытия схем.
        //************************************************************************************************

        private void buttonGSA_Click(object sender, EventArgs e)
        {
            //Если форма еще не была открыта.
            if (formGSA == null) {
                formGSA = new FormGSA(algorithm, this);     //Создание главной формы задания.
                formGSA.Show();                             //Демонстрация формы.
            }
            //Если форма была открыта однажды.
            else {
                formGSA.Visible = true;
            }
        }

        private void buttonMSA_Click(object sender, EventArgs e)
        {
            //Если форма еще не была открыта.
            if (formMSA == null) {
                formMSA = new FormMSA(algorithm, this);     //Создание главной формы задания.
                formMSA.Show();                             //Демонстрация формы.
            }
            //Если форма была открыта однажды.
            else {
                formMSA.Visible = true;
            }
        }

        private void buttonLSA_Click(object sender, EventArgs e)
        {
            //Если форма еще не была открыта.
            if (formLSA == null) {
                formLSA = new FormLSA(algorithm, this);     //Создание главной формы задания.
                formLSA.Show();                             //Демонстрация формы.
            }
            //Если форма была открыта однажды.
            else {
                formLSA.Visible = true;
            }
        }


        //************************************************************************************************
        // Дополнительные события
        //************************************************************************************************

        //Изменение размеров панели с кнопками для открытия схем.
        private void groupBoxScheme_SizeChanged(object sender, EventArgs e)
        {
            //Перемещение кнопки открытия МСА в центр панели.
            buttonMSA.Location = new Point(groupBoxScheme.Width / 2 - buttonMSA.Width / 2, buttonMSA.Location.Y);
        }

        //Закрытие формы.
        private void FormMain_FormClosing(object sender, FormClosingEventArgs e)
        {
            DialogResult result = MessageBox.Show("При закрытии программы данные будут потеряны, продолжить?", "Предупреждение" , MessageBoxButtons.YesNo, MessageBoxIcon.Question);

            //Если было соглашение с закрытием формы.
            if (result == DialogResult.Yes) {
                //Закрытие форм, которые остались открытыми.
                if (formGSA != null) {
                    formGSA.Close();
                }
                if (formMSA != null) {
                    formMSA.Close();
                }
                if (formLSA != null) {
                    formLSA.Close();
                }
            }
            //Если не было соглашения с закрытием формы.
            else {
                e.Cancel = true;   //Отмена закрытия формы.
            }
        }
    }
}
