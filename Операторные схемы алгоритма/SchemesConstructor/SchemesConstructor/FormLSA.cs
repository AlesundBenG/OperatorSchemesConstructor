using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using OperatorAlgorithm;
using ClassesGraph;
using CheckingTheScheme;

namespace SchemesConstructor
{
    public partial class FormLSA :Form
    {
        /////////////////////////////////////Поля класса/////////////////////////////////////
        private Algorithm               algorithm;              //Алгоритм задания.
        private FormMain                formMain;               //Родительское окно.
        private Timer                   timer;                  //Время выполнения.
        private DateTime                dateOpenForm;           //Время последнего времени открытия формы.
        private long                    totalTick;              //Все время выполнения задания.
        private CheckingTheLogicScheme  checkingTheLogicScheme; //Проверка логических схем.
        private Color                   colorEmpty;             //Цвет пустой ячейки.
        private Color                   colorCorrect;           //Цвет правильно заполненной ячейки.
        private Color                   colorNotCorrect;        //Цвет не правильно заполненной ячейки.
        private int                     row;                     //Строка отредактированной ячейки.
        private int                     column;                  //Столбец отредактированной ячейки.



        /////////////////////////////////////Свойства класса/////////////////////////////////////

        /// <summary>
        /// Флаг выполнения задания.
        /// </summary>
        public bool JobDone
        {
            get
            {
                return (progressBar.Value == progressBar.Maximum);
            }
        }

        /// <summary>
        /// Количество ошибок при выполнении задания.
        /// </summary>
        public int NumberError
        {
            get
            {
                return Convert.ToInt32(labelNumberErrors.Text);
            }
        }

        /// <summary>
        /// Время выполнения задания в формате HH:mm:ss
        /// </summary>
        public string ExecutionTime
        {
            get
            {
                DateTime stopWatch = new DateTime();                //Инициализация промежутка времени.
                stopWatch = stopWatch.AddTicks(totalTick);          //Добавление времени.
                return String.Format("{0:HH:mm:ss}", stopWatch);    //Возврат времени.
            }
        }

        /// <summary>
        /// Время выполнения задания в тиках.
        /// </summary>
        public long TotalTick
        {
            get
            {
                return totalTick;
            }
        }



        /////////////////////////////////////Public методы/////////////////////////////////////
        public FormLSA(Algorithm algorithm, FormMain formMain)
        {
            InitializeComponent();

            //Получение информации из главной формы.
            this.formMain = formMain;
            this.algorithm = algorithm;
            int nLine = algorithm.NumberOperator - 1;

            //Добавление столбцов матрицы.
            dataGridViewS1.Columns.Add("transition", "Формулы");
            dataGridViewS2.Columns.Add("transition", "Формулы");
            dataGridViewS3.Columns.Add("transition", "Формулы");
            dataGridViewS3_Minimized.Columns.Add("transition", "Формулы");

            dataGridViewS1.Columns[0].Width = 2000;
            dataGridViewS2.Columns[0].Width = 2000;
            dataGridViewS3.Columns[0].Width = 2000;
            dataGridViewS3_Minimized.Columns[0].Width = 2000;

            //Добавление строк матрицы.
            dataGridViewS1.Rows.Add(nLine);
            dataGridViewS2.Rows.Add(nLine);
            dataGridViewS3.Rows.Add(nLine);
            dataGridViewS3_Minimized.Rows.Add(nLine);
            for (int i = 0; i < nLine; i++) {
                dataGridViewS1.Rows[i].HeaderCell.Value = algorithm.getOperatorVertex(i).Name + " →";
                dataGridViewS2.Rows[i].HeaderCell.Value = algorithm.getOperatorVertex(i).Name + " →";
                dataGridViewS3.Rows[i].HeaderCell.Value = algorithm.getOperatorVertex(i).Name + " →";
                dataGridViewS3_Minimized.Rows[i].HeaderCell.Value = algorithm.getOperatorVertex(i).Name + " →";
            }

            dataGridViewS1.DefaultCellStyle.Font = new Font("calibri", 14, FontStyle.Regular);  
            dataGridViewS2.DefaultCellStyle.Font = new Font("calibri", 14, FontStyle.Regular);  
            dataGridViewS3.DefaultCellStyle.Font = new Font("calibri", 14, FontStyle.Regular);
            dataGridViewS3_Minimized.DefaultCellStyle.Font = new Font("calibri", 14, FontStyle.Regular);
            textBoxLSA.Font = new Font("calibri", 14, FontStyle.Regular);                       


            dataGridViewS1.ReadOnly             = false;
            dataGridViewS2.ReadOnly             = true;
            dataGridViewS3.ReadOnly             = true;
            dataGridViewS3_Minimized.ReadOnly   = true;
            textBoxLSA.ReadOnly = true;

            //Подстройка прогресс бара под задание.
            progressBar.Maximum = 4 * (algorithm.NumberOperator - 1);  //Из конечной вершины нет перехода.


            //Инициализация цветов.
            colorEmpty = Color.White;
            colorCorrect    = Color.LightGreen;
            colorNotCorrect = Color.LightPink;

            checkingTheLogicScheme = new CheckingTheLogicScheme(algorithm);


            foreach (DataGridViewColumn column in dataGridViewS1.Columns) {
                column.SortMode = DataGridViewColumnSortMode.NotSortable;
            }
            foreach (DataGridViewColumn column in dataGridViewS2.Columns) {
                column.SortMode = DataGridViewColumnSortMode.NotSortable;
            }
            foreach (DataGridViewColumn column in dataGridViewS3.Columns) {
                column.SortMode = DataGridViewColumnSortMode.NotSortable;
            }
            foreach (DataGridViewColumn column in dataGridViewS3_Minimized.Columns) {
                column.SortMode = DataGridViewColumnSortMode.NotSortable;
            }

            //Время.
            totalTick = 0;                            //Общее время.
            dateOpenForm = DateTime.Now;                 //Время начала.
            timer = new Timer();                  //Инициализация таймера.
            timer.Interval = 1000;                         //Интервал в миллисекундах.
            timer.Tick += new EventHandler(tickTimer); //Добавление события подсчета времени.
            timer.Start();                                    //Запуск таймера.    

        }



        /////////////////////////////////////Private методы/////////////////////////////////////

        //************************************************************************************************
        // Информация о выполнении задания.
        //************************************************************************************************

        //Установка завершения выполнения задания.
        private void setDoneJob()
        {
            //Завершение счета времени.
            totalTick += DateTime.Now.Ticks - dateOpenForm.Ticks;       //Прибавление к общему времени.
            timer.Stop();                                               //Остановка таймера.

            //Отображение завершения выполнения задания.
            labelCondition.Text = "Выполнено";
            labelCondition.BackColor = Color.LimeGreen;
            formMain.setDoneLSA(true);
            MessageBox.Show("Задание по построению ЛСА выполнено.", "Сообщение", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        //Сброс завершения выполнения задания.
        private void resetDoneJob()
        {
            //Продолжение счета времени. 
            dateOpenForm = DateTime.Now;      //Запись текущего времени.
            timer.Start();                    //Запуск таймера.

            //Отображение не завершенности выполнения задания.
            labelCondition.Text = "Не выполнено";
            labelCondition.BackColor = Color.Gold;
            formMain.setDoneMSA(false);
        }

        //Увеличение прогресса.
        private void incProgress()
        {
            progressBar.Value += 1; //Увеличение прогресса.

            if (progressBar.Value == algorithm.NumberOperator - 1) {
                dataGridViewS1.ReadOnly = true;
                dataGridViewS2.ReadOnly = false;

                for (int i = 0; i < algorithm.NumberOperator - 1; i++) {
                    dataGridViewS2.Rows[i].Cells[0].Value = dataGridViewS1.Rows[i].Cells[0].Value;
                }

                MessageBox.Show("S1 построена верно, можно переходить к S2.", "Информация", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            else if (progressBar.Value == 2 * (algorithm.NumberOperator - 1)) {
                dataGridViewS2.ReadOnly = true;
                dataGridViewS3.ReadOnly = false;

                for (int i = 0; i < algorithm.NumberOperator - 1; i++) {
                    dataGridViewS3.Rows[i].Cells[0].Value = dataGridViewS2.Rows[i].Cells[0].Value;
                }

                MessageBox.Show("S2 построена верно, можно переходить к S3.", "Информация", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            else if (progressBar.Value == 3 * (algorithm.NumberOperator - 1)) {
                dataGridViewS3.ReadOnly = true;
                textBoxLSA.ReadOnly = false;

                MessageBox.Show("S3 построена верно, можно переходить к ЛСА.", "Информация", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }


            //Если задание полностью выполнено, то фиксируем событие выполненного задания.
            if (progressBar.Value == progressBar.Maximum) {
                setDoneJob();
                textBoxLSA.ReadOnly = true;
            }
        }

        //Уменьшение прогресса.
        private void decProgress()
        {
            //Если задание было полностью выполнено, то сброс события выполненного задания.
            if (progressBar.Value == progressBar.Maximum) {
                resetDoneJob();
            }
            progressBar.Value = progressBar.Value - 1;                                            //Уменьшение прогресса.
        }

        //Добавление ошибки.
        private void incError()
        {
            labelNumberErrors.Text = (Convert.ToInt32(labelNumberErrors.Text) + 1).ToString();
        }

        //Счет таймера.
        private void tickTimer(object sender, EventArgs e)
        {
            long tick = (DateTime.Now.Ticks - dateOpenForm.Ticks) + totalTick;  //Вычисление тиков.
            DateTime stopWatch = new DateTime();                                //Инициализация промежутка времени.

            stopWatch = stopWatch.AddTicks(tick);                               //Добавление времени.
            labelTime.Text = String.Format("{0:HH:mm:ss}", stopWatch);          //Демонстрация времени.
        }


        //************************************************************************************************
        // События над формой.
        //************************************************************************************************

        //Изменение видимости формы.
        private void FormLSA_VisibleChanged(object sender, EventArgs e)
        {
            //Если счет должен вестись.
            if (progressBar.Value != progressBar.Maximum) {
                //Если окно открыли.
                if (this.Visible == true) {
                    dateOpenForm = DateTime.Now;                                //Запись текущего времени.
                    timer.Start();                                              //Запуск таймера.
                }
                //Если окно закрыли.
                else {
                    totalTick += DateTime.Now.Ticks - dateOpenForm.Ticks;       //Прибавление к общему времени.
                    timer.Stop();                                               //Остановка таймера.
                }
            }
        }

        //Закрытие формы построения ЛСА.
        private void FormLSA_FormClosing(object sender, FormClosingEventArgs e)
        {
            e.Cancel        = true;     //Отменяем закрытие формы.
            this.Visible    = false;    //Скрытие формы, чтобы заново потом при открытии ее не создавать.
        }


        //************************************************************************************************
        // Фиксация строки и столбца отредактированной ячейки.
        //************************************************************************************************

        private void dataGridViewS1_CellEndEdit(object sender, DataGridViewCellEventArgs e)
        {
            //Проблема в том, что переход на следующую ячейку матрицы происходит до вызова события KeyUp,
            //поэтому метод получения текущей ячейки показывает не отредактированную ячейку, а следующую.
            row     = dataGridViewS1.CurrentRow.Index;
            column  = dataGridViewS1.CurrentCell.ColumnIndex;
        }

        private void dataGridViewS2_CellEndEdit(object sender, DataGridViewCellEventArgs e)
        {
            //Проблема в том, что переход на следующую ячейку матрицы происходит до вызова события KeyUp,
            //поэтому метод получения текущей ячейки показывает не отредактированную ячейку, а следующую.
            row     = dataGridViewS2.CurrentRow.Index;
            column  = dataGridViewS2.CurrentCell.ColumnIndex;
        }

        private void dataGridViewS3_CellEndEdit(object sender, DataGridViewCellEventArgs e)
        {
            //Проблема в том, что переход на следующую ячейку матрицы происходит до вызова события KeyUp,
            //поэтому метод получения текущей ячейки показывает не отредактированную ячейку, а следующую.
            row     = dataGridViewS3.CurrentRow.Index;
            column  = dataGridViewS3.CurrentCell.ColumnIndex;
        }

        private void dataGridViewS3_Minimized_CellEndEdit(object sender, DataGridViewCellEventArgs e)
        {
            //Проблема в том, что переход на следующую ячейку матрицы происходит до вызова события KeyUp,
            //поэтому метод получения текущей ячейки показывает не отредактированную ячейку, а следующую.
            row     = dataGridViewS3_Minimized.CurrentRow.Index;
            column  = dataGridViewS3_Minimized.CurrentCell.ColumnIndex;
        }


        //************************************************************************************************
        // Получение элемента управления для редактирования ячейки.
        //************************************************************************************************

        private void dataGridViewS1_EditingControlShowing(object sender, DataGridViewEditingControlShowingEventArgs e)
        {
            TextBox cell = (TextBox)e.Control;                          //Ячейка таблицы как textbox.
            cell.KeyPress += new KeyPressEventHandler(cell_KeyPress);   //Добавление обработчика события нажатия клавиши при вводе в ячейку.
        }

        private void dataGridViewS2_EditingControlShowing(object sender, DataGridViewEditingControlShowingEventArgs e)
        {
            TextBox cell = (TextBox)e.Control;                          //Ячейка таблицы как textbox.
            cell.KeyPress += new KeyPressEventHandler(cell_KeyPress);   //Добавление обработчика события нажатия клавиши при вводе в ячейку.
        }

        private void dataGridViewS3_EditingControlShowing(object sender, DataGridViewEditingControlShowingEventArgs e)
        {
            TextBox cell    = (TextBox)e.Control;                           //Ячейка таблицы как textbox.
            cell.KeyPress   += new KeyPressEventHandler(cell_KeyPress);     //Добавление обработчика события ввода символа.
            cell.KeyDown    -= new KeyEventHandler(cell_KeyDown);           //Добавление обработчика события нажатия клавиши при вводе в ячейку.
            cell.KeyDown    += new KeyEventHandler(cell_KeyDown);           //Добавление обработчика события нажатия клавиши при вводе в ячейку.
        }

        private void dataGridViewS3_Minimized_EditingControlShowing(object sender, DataGridViewEditingControlShowingEventArgs e)
        {
            TextBox cell    = (TextBox)e.Control;                           //Ячейка таблицы как textbox.
            cell.KeyPress   += new KeyPressEventHandler(cell_KeyPress);     //Добавление обработчика события ввода символа.
            cell.KeyDown    -= new KeyEventHandler(cell_KeyDown);             //Добавление обработчика события нажатия клавиши при вводе в ячейку.
            cell.KeyDown    += new KeyEventHandler(cell_KeyDown);             //Добавление обработчика события нажатия клавиши при вводе в ячейку.
        }


        //************************************************************************************************
        // Обработка событий при редактировании ячеек схем.
        //************************************************************************************************

        //Замена вводимых символов.
        void cell_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == '~') {
                e.KeyChar = '¬';
            }
            else if (e.KeyChar == 'v') {
                e.KeyChar = '∨';
            }
        }

        //Обработка сочетаний клавиш.
        void cell_KeyDown(object sender, KeyEventArgs e)
        {
            string replacedCharacter = null;

            //Замена вводимого символа на специализированный символ, при зажатом Ctrl.
            if (e.Alt) {
                if ((e.KeyCode >= Keys.D0) && (e.KeyCode <= Keys.D9)) {
                    switch (e.KeyCode) {
                        case Keys.D0: replacedCharacter = "⁰"; break;
                        case Keys.D1: replacedCharacter = "¹"; break;
                        case Keys.D2: replacedCharacter = "²"; break;
                        case Keys.D3: replacedCharacter = "³"; break;
                        case Keys.D4: replacedCharacter = "⁴"; break;
                        case Keys.D5: replacedCharacter = "⁵"; break;
                        case Keys.D6: replacedCharacter = "⁶"; break;
                        case Keys.D7: replacedCharacter = "⁷"; break;
                        case Keys.D8: replacedCharacter = "⁸"; break;
                        case Keys.D9: replacedCharacter = "⁹"; break;
                    }
                }
                else if (e.KeyCode == Keys.U) {
                    replacedCharacter = "↑";
                }
                else if (e.KeyCode == Keys.D) {
                    replacedCharacter = "↓";
                }
                else if (e.KeyCode == Keys.W) {
                    replacedCharacter = "ω";
                }
            }

            //Если необходимо вводимый символ заменить на специализированный.
            if (replacedCharacter != null) {
                TextBox cell        = (sender as TextBox);                              //Приведение типов.
                int     position    = cell.SelectionStart;                              //Позиция курсора в строке.
                cell.Text           = cell.Text.Insert(position, replacedCharacter);    //Вставка символа.
                cell.SelectionStart = position + 1;                                     //Установка курсора в позицию за вставленным символом.   
                e.Handled           = true;                                             //Событие нажатия клавшиы обработано.
            }
        }


        //************************************************************************************************
        // Проверка схем.
        //************************************************************************************************

        private void dataGridViewS1_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter) {
                checkCell(dataGridViewS1, "s1");
            }
        }

        private void dataGridViewS2_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter) {
                checkCell(dataGridViewS2, "s2");
            }
        }

        private void dataGridViewS3_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter) {
                checkCell(dataGridViewS3, "s3");
            }
        }

        private void dataGridViewS3_Minimized_KeyUp(object sender, KeyEventArgs e)
        {

        }




        private void checkCell(DataGridView dataGridView, string scheme)
        {
            //Если ячейка содержит информацию.
            if (dataGridView.Rows[row].Cells[column].Value != null) {
                string value        = dataGridView.Rows[row].Cells[column].Value.ToString();
                CheckResult result  = checkingTheLogicScheme.checkLineLogicScheme(algorithm.getOperatorVertex(row).Name, value, scheme);
                //Если информация введна корректно и до этого ячейка не содержала правильную информацию, то увеличиваем прогресс и выделяем ячейку.
                if (!result.hasError) {
                    if (dataGridView.Rows[row].Cells[column].Style.BackColor != colorCorrect) {
                        dataGridView.Rows[row].Cells[column].Style.BackColor = colorCorrect;
                        incProgress();
                    }
                }
                //Если информация введена не корректно, то добавляем ошибку.
                else {
                    MessageBox.Show(result.explanationError, "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    //Если до этого ячейка содержала верную информацию, то снимаем выделение ячейки и уменьшаем прогресс.
                    if (dataGridView.Rows[row].Cells[column].Style.BackColor == colorCorrect) {
                        decProgress();
                    }
                    dataGridView.Rows[row].Cells[column].Style.BackColor = colorNotCorrect;
                    incError();
                }
            }
            //Если ячейка не содержит информацию.
            else {
                //Если до этого ячейка содержала информацию.
                if (dataGridView.Rows[row].Cells[column].Style.BackColor != colorEmpty) {
                    //Если до этого ячейка содержала верную информацию.
                    if (dataGridView.Rows[row].Cells[column].Style.BackColor == colorCorrect) {
                        decProgress();
                    }
                    dataGridView.Rows[row].Cells[column].Style.BackColor = colorEmpty;
                    decProgress();
                }
            }
        }

        private void textBoxLSA_KeyDown(object sender, KeyEventArgs e)
        {
            string replacedCharacter = null;

            //Замена вводимого символа на специализированный символ, при зажатом Ctrl.
            if (e.Alt) {
                if ((e.KeyCode >= Keys.D0) && (e.KeyCode <= Keys.D9)) {
                    switch (e.KeyCode) {
                        case Keys.D0: replacedCharacter = "⁰"; break;
                        case Keys.D1: replacedCharacter = "¹"; break;
                        case Keys.D2: replacedCharacter = "²"; break;
                        case Keys.D3: replacedCharacter = "³"; break;
                        case Keys.D4: replacedCharacter = "⁴"; break;
                        case Keys.D5: replacedCharacter = "⁵"; break;
                        case Keys.D6: replacedCharacter = "⁶"; break;
                        case Keys.D7: replacedCharacter = "⁷"; break;
                        case Keys.D8: replacedCharacter = "⁸"; break;
                        case Keys.D9: replacedCharacter = "⁹"; break;
                    }
                }
                else if (e.KeyCode == Keys.U) {
                    replacedCharacter = "↑";
                }
                else if (e.KeyCode == Keys.D) {
                    replacedCharacter = "↓";
                }
                else if (e.KeyCode == Keys.W) {
                    replacedCharacter = "ω";
                }
            }

            //Если необходимо вводимый символ заменить на специализированный.
            if (replacedCharacter != null) {
                int position = textBoxLSA.SelectionStart;                              //Позиция курсора в строке.
                textBoxLSA.Text = textBoxLSA.Text.Insert(position, replacedCharacter);    //Вставка символа.
                textBoxLSA.SelectionStart = position + 1;                                     //Установка курсора в позицию за вставленным символом.   
                e.Handled = true;                                             //Событие нажатия клавшиы обработано.
            }
        }

        private void textBoxLSA_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == '~') {
                e.KeyChar = '¬';
            }
            else if (e.KeyChar == 'v') {
                e.KeyChar = '∨';
            }
        }

        private void buttonCheckLSA_Click(object sender, EventArgs e)
        {
            //Если ячейка содержит информацию.
            if (textBoxLSA != null) {
                string value = "";
                for (int i = 0; i < textBoxLSA.Lines.Length; i++) {
                    value += textBoxLSA.Lines[i];
                }

                CheckResult result = checkingTheLogicScheme.checkLSA(value);
                //Если информация введна корректно и до этого ячейка не содержала правильную информацию, то увеличиваем прогресс и выделяем ячейку.
                if (!result.hasError) {
                    textBoxLSA.BackColor = colorCorrect;
                    for (int i = 0; i < algorithm.NumberOperator - 1; i++) {
                        incProgress();
                    }
                }
                //Если информация введена не корректно, то добавляем ошибку.
                else {
                    MessageBox.Show(result.explanationError, "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    textBoxLSA.BackColor = colorNotCorrect;
                }
            }
        }
    }
}
