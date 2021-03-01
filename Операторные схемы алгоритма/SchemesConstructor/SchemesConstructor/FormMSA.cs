using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using OperatorAlgorithm;    //Класс представления операторного алгоритма.
using CheckingTheScheme;    //Проверка схемы алгоритма.

namespace SchemesConstructor
{
    public partial class FormMSA :Form
    {
        /////////////////////////////////////Поля класса/////////////////////////////////////
        private FormMain            formMain;           //Родительское окно.
        private Algorithm           algorithm;          //Алгоритм задания.
        private CheckingTheMatrix   checkingMatrinx;    //Проверка МСА.
        private Color               colorEmpty;         //Цвет пустой ячейки.
        private Color               colorCorrect;       //Цвет правильно заполненной ячейки.
        private Color               colorNotCorrect;    //Цвет не правильно заполненной ячейки.
        private Timer               timer;              //Время выполнения.
        private DateTime            dateOpenForm;       //Время последнего открытия формы.
        private long                totalTick;          //Все время выполнения задания.
        private int                 row;                //Строка отредактированной ячейки.
        private int                 column;             //Столбец отредактированной ячейки.



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
                DateTime    stopWatch = new DateTime();             //Инициализация промежутка времени.
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
        
        /// <summary>
        /// Конструктор.
        /// </summary>
        /// <param name="algorithm">Операторный алгоритм, по которому строится матрица</param>
        /// <param name="formMain">Ссылка на главную форму</param>
        public FormMSA(Algorithm algorithm, FormMain formMain)
        {
            InitializeComponent();
            dataGridViewMSA.DefaultCellStyle.Font = new Font("calibri", 9, FontStyle.Regular);  //Постоянно сбрасывается из-за того, что groupBox имеет другой шрифт.

            //Получение информации из главной формы.
            this.formMain = formMain;
            this.algorithm = algorithm;
            int nColumns = algorithm.NumberOperator - 1;

            //Инициализация проверяющего МСА.
            checkingMatrinx = new CheckingTheMatrix(algorithm);

            //Инициализация цветов.
            colorEmpty = Color.White;
            colorCorrect = Color.LightGreen;
            colorNotCorrect = Color.LightPink;

            //Добавление столбцов матрицы.
            for (int i = 1; i < nColumns + 1; i++) {
                dataGridViewMSA.Columns.Add(algorithm.getOperatorVertex(i).Name, algorithm.getOperatorVertex(i).Name);
            }

            //Добавление строк матрицы.
            dataGridViewMSA.Rows.Add(nColumns);
            for (int i = 0; i < nColumns; i++) {
                dataGridViewMSA.Rows[i].HeaderCell.Value = algorithm.getOperatorVertex(i).Name;
            }
            //dataGridViewMSA.RowHeadersWidth = 80;                                                   //Размер столба наименований. Почему-то на него не хочет работать Autosize.

            //Подстройка прогресс бара под задание.
            progressBar.Maximum = nColumns * nColumns;

            foreach (DataGridViewColumn column in dataGridViewMSA.Columns) {
                column.SortMode = DataGridViewColumnSortMode.NotSortable;
            }

            //Время.
            totalTick = 0;
            dateOpenForm = DateTime.Now;                 //Время начала.
            timer = new Timer {                   //Инициализация таймера.
                Interval = 1000                                 //Интервал в миллисекундах.
            };
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
            labelCondition.Text         = "Выполнено";
            labelCondition.BackColor    = Color.LimeGreen;
            formMain.setDoneMSA(true);
            MessageBox.Show("Задание по построению МСА выполнено.", "Сообщение", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        //Сброс завершения выполнения задания.
        private void resetDoneJob()
        {
            //Продолжение счета времени. 
            dateOpenForm = DateTime.Now;      //Запись текущего времени.
            timer.Start();                    //Запуск таймера.

            //Отображение не завершенности выполнения задания.
            labelCondition.Text         = "Не выполнено";
            labelCondition.BackColor    = Color.Gold;
            formMain.setDoneMSA(false);
        }

        //Увеличение прогресса.
        private void incProgress()
        {
            progressBar.Value += 1; //Увеличение прогресса.
            //Если задание полностью выполнено, то фиксируем событие выполненного задания.
            if (progressBar.Value == progressBar.Maximum) {
                setDoneJob();
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

        //Закрытие формы построения МСА.
        private void FormMSA_FormClosing(object sender, FormClosingEventArgs e)
        {
            e.Cancel        = true;     //Отменяем закрытие формы.
            this.Visible    = false;    //Скрытие формы, чтобы заново потом при открытии ее не создавать.
        }

        //Изменение видимости формы.
        private void FormMSA_VisibleChanged(object sender, EventArgs e)
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


        //************************************************************************************************
        // Ввод и редактироваие ячеек матрицы.
        //************************************************************************************************

        //Проверка ячейки при окончании ввода и нажатии Enter.
        private void dataGridViewMSA_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter) {
                //Если ячейка содержит информацию.
                if (dataGridViewMSA.Rows[row].Cells[column].Value != null) {
                    string operatorFrom = dataGridViewMSA.Rows[row].HeaderCell.Value.ToString();
                    string operatorTo   = dataGridViewMSA.Columns[column].HeaderCell.Value.ToString();
                    string value        = dataGridViewMSA.Rows[row].Cells[column].Value.ToString();
                    CheckResult result  = checkingMatrinx.checkCorrectMatrixCell(operatorFrom, operatorTo, value);

                    //Если информация введна корректно и до этого ячейка не содержала правильную информацию, то увеличиваем прогресс и выделяем ячейку.
                    if (!result.hasError) {
                        if (dataGridViewMSA.Rows[row].Cells[column].Style.BackColor != colorCorrect) {
                            dataGridViewMSA.Rows[row].Cells[column].Style.BackColor = colorCorrect;
                            incProgress();
                        }
                    }
                    //Если информация введена не корректно, то добавляем ошибку.
                    else {
                        MessageBox.Show(result.explanationError, "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);

                        //Если до этого ячейка содержала верную информацию, то снимаем выделение ячейки и уменьшаем прогресс.
                        if (dataGridViewMSA.Rows[row].Cells[column].Style.BackColor == colorCorrect) {
                            decProgress();
                        }
                        dataGridViewMSA.Rows[row].Cells[column].Style.BackColor = colorNotCorrect;
                        incError();
                    }
                }
                //Если ячейка не содержит информацию.
                else {
                    //Если до этого ячейка содержала верную информацию, то снимаем выделение ячейки и уменьшаем прогресс.
                    if (dataGridViewMSA.Rows[row].Cells[column].Style.BackColor == colorCorrect) {
                        dataGridViewMSA.Rows[row].Cells[column].Style.BackColor = colorEmpty;
                        decProgress();
                    }
                }
            }
        }

        //Фиксация строки и столбца отредактированной ячейки.
        private void dataGridViewMSA_CellEndEdit(object sender, DataGridViewCellEventArgs e)
        {
            //Проблема в том, что переход на следующую ячейку матрицы происходит до вызова события KeyUp,
            //поэтому метод получения текущей ячейки показывает не отредактированную ячейку, а следующую.
            row     = dataGridViewMSA.CurrentRow.Index;
            column  = dataGridViewMSA.CurrentCell.ColumnIndex;
        }

        //Обработка события при нажатии клавиши.
        private void cell_KeyPress(object sender, KeyPressEventArgs e)
        {
            //Замена символов.
            if (e.KeyChar == '~') {
                e.KeyChar = '¬';
            }
            else if (e.KeyChar == 'v') {
                e.KeyChar = '∨';
            }
        }

        //Получение элемента управления для редактирования ячейки.
        private void dataGridViewMSA_EditingControlShowing(object sender, DataGridViewEditingControlShowingEventArgs e)
        {
            TextBox cell    = (TextBox)e.Control;                       //Ячейка таблицы как textbox.
            cell.KeyPress   += new KeyPressEventHandler(cell_KeyPress); //Добавление обработчика события нажатия клавиши при вводе в ячейку.
        }

        //Отслеживание изменения ячейки.
        private void dataGridViewMSA_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {
            if ((dataGridViewMSA.CurrentRow != null) && (dataGridViewMSA.CurrentCell != null)) {
                row     = dataGridViewMSA.CurrentRow.Index;
                column  = dataGridViewMSA.CurrentCell.ColumnIndex;
                //Если до этого ячейка содержала верную информацию, то снимаем выделение ячейки и уменьшаем прогресс.
                if (dataGridViewMSA.Rows[row].Cells[column].Style.BackColor == colorCorrect) {
                    dataGridViewMSA.Rows[row].Cells[column].Style.BackColor = colorEmpty;
                    decProgress();
                }
            }
        }
    }
}
