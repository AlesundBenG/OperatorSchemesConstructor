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
using InterfaceGSA;
using CheckingTheScheme;

namespace SchemesConstructor
{
    public partial class FormGSA :Form
    {
        /////////////////////////////////////Поля класса/////////////////////////////////////
        private FormMain            formMain;             //Родительское окно.
        private Algorithm           algorithm;            //Алгоритм задания.
        private List<ObjectGraph>   aObjectGraph;         //Список объектов.
        private Bitmap              originalBitmap;       //Слой, на котором изображены не выделенные вершины и линии.
        private DetectorObject      detectorObject;       //Обнаружитель объектов.        
        private MoveObject          moveVertex;           //Перемещатель вершин.
        private ConstructorLine     constructorLine;      //Конструктор линий.
        private int                 countID;              //Счетчик идентификаторов объектов.
        private bool                mousePress;           //Состояние левой кнопки мыши (Зажата или нет).
        private VertexGraph         bufferCreatedVertex;  //Буфер созданной вершины.
        private CheckingTheGraph    checkingTheGraph;     //Проверка ГСА.
        private Color               colorEmpty;           //Цвет вершины, из которой не все выходы проведены.
        private Color               colorCorrect;         //Цвет вершины, из которой все переходы верны.
        private Color               colorNotCorrect;      //Цвет вершины, из которой есть не правильно построенный переход.
        private Timer               timer;                //Время выполнения.
        private DateTime            dateOpenForm;         //Время последнего времени открытия формы.
        private long                totalTick;            //Все время выполнения задания.



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

        /// <summary>
        /// Конструктор.
        /// </summary>
        /// <param name="algorithm"Операторный алгоритм, по которому строится граф></param>
        /// <param name="formMain">Ссылка на главную форму</param>
        public FormGSA(Algorithm algorithm, FormMain formMain)
        {
            //Инициализация компонентов.
            InitializeComponent();

            //Получение информации из главной формы.
            this.algorithm  = algorithm;
            this.formMain   = formMain;

            //Инициализация списков объектов.
            aObjectGraph = new List<ObjectGraph>();

            //Добавление объектов в список объектов.
            for (int i = 0; i < algorithm.NumberOperator; i++) {
                comboBoxOperator.Items.Add(algorithm.getOperatorVertex(i).Name);
            }
            for (int i = 0; i < algorithm.NumberCondition; i++) {
                comboBoxCondition.Items.Add(algorithm.getConditionVertex(i).Name);
            }
            comboBoxOperator.SelectedIndex = 0;                                                 //Установка указателя на первый элемент.
            if (algorithm.NumberCondition == 0) {
                buttonCreateCondition.Enabled = false;
            }
            else {
                comboBoxCondition.SelectedIndex = 0;
            }

            //Инициализация буфера.
            countID = 0;
            mousePress = false;
            bufferCreatedVertex = null;

            checkingTheGraph = new CheckingTheGraph(algorithm, "+", "-");

            //Создание средств для рисования.
            originalBitmap = new Bitmap(pictureBoxGraph.Width, pictureBoxGraph.Height);   //Инициализация 
            Graphics graph = Graphics.FromImage(originalBitmap);
            graph.Clear(pictureBoxGraph.BackColor);
            pictureBoxGraph.Image = originalBitmap;

            //Создание инструментов для построения.
            detectorObject = new DetectorObject(aObjectGraph, originalBitmap);
            moveVertex = new MoveObject(aObjectGraph, pictureBoxGraph.BackColor);
            constructorLine = new ConstructorLine(aObjectGraph, pictureBoxGraph.BackColor);

            //Подстройка прогресс бара под задание.
            progressBar.Maximum = algorithm.NumberOperator + algorithm.NumberCondition - 1;  //Из конечной вершины нет перехода.

            //Инициализация цветов.
            colorEmpty = pictureBoxGraph.BackColor;
            colorCorrect = Color.FromArgb(138, 255, 138);
            colorNotCorrect = Color.LightPink;

            //Время.
            totalTick = 0;                            //Общее время.
            dateOpenForm = DateTime.Now;                 //Время начала.
            timer = new Timer {                   //Инициализация таймера.
                Interval = 1000                                 //Интервал в миллисекундах.
            };
            timer.Tick += new EventHandler(tickTimer); //Добавление события подсчета времени.
            timer.Start();                                    //Запуск таймера.    
        }




        /////////////////////////////////////Private методы/////////////////////////////////////

        //************************************************************************************************
        // Определение типа вершины.
        //************************************************************************************************

        //Проверка, является ли объект оператором.
        public bool isOperator(ObjectGraph objectGraph)
        {
            if (objectGraph.getType() == TYPE_OBJECT.VERTEX) {
                VertexGraph vertex = (VertexGraph)objectGraph;
                if (vertex.getText() != null) {
                    string nameVertex = vertex.getText().getText();
                    return algorithm.isOperator(nameVertex);
                }
            }
            return false;
        }

        //Проверка, является ли объект условием.
        public bool isCondition(ObjectGraph objectGraph)
        {
            if (objectGraph.getType() == TYPE_OBJECT.VERTEX) {
                VertexGraph vertex = (VertexGraph)objectGraph;
                if (vertex.getText() != null) {
                    string nameVertex = vertex.getText().getText();
                    return algorithm.isCondition(nameVertex);
                }
            }
            return false;
        }



        //************************************************************************************************
        // Обработка особенности условной вершины - 3 места для вывода линии, но из вершины может выходить только 2 линии.
        //************************************************************************************************

        //Проверка блокировки третьего места условной вершины.
        private void checkLockPlaceFromCondition(VertexGraph condition)
        {
            //Подсчет количества занятых линиями мест.
            int numberBusyPlace = 0;
            for (int i = 0; i < 3; i++) {
                if (condition.getPlaceOutputLine(i).getLine() != null) {
                    numberBusyPlace++;
                }
            }
            //Если 2 из 3 места заняты, то третье место блокируется.
            if (numberBusyPlace == 2) {
                //Поиск не занятого места, чтобы его заблокировать.
                for (int i = 0; i < 3; i++) {
                    if (condition.getPlaceOutputLine(i).getLine() == null) {
                        condition.getPlaceOutputLine(i).setLocked(true);
                    }
                }
            }
        }

        /*Проверка разблокировки третьего места условной вершины*/
        private void checkUnLockPlaceFromCondition(VertexGraph condition)
        {
            //Поиск не заблокированного места.
            bool thereUnlock = false;
            for (int i = 0; i < 3; i++) {
                if (condition.getPlaceOutputLine(i).getLocked() == false) {
                    thereUnlock = true;
                    break;
                }
            }

            //Если есть не заблокированное место, то ищем место, которое могло быть заблокировано из-за того,
            //что из улсовной вершины может выходить только две линии.
            if (thereUnlock == true) {
                for (int i = 0; i < 3; i++) {
                    if ((condition.getPlaceOutputLine(i).getLocked() == true) && (condition.getPlaceOutputLine(i).getLine() == null)) {
                        condition.getPlaceOutputLine(i).setLocked(false);
                        break;
                    }
                }
            }
        }



        //************************************************************************************************
        // Проверка коррекстного выполнения задания.
        //************************************************************************************************

        //Проверка проведенного перехода.
        public void checkingTransiton(LineGraph transition)
        {
            CheckResult result = checkingTheGraph.checkTransition(transition);
            ObjectGraph objectTo = checkingTheGraph.getObjectTo(transition);

            if (objectTo != null) {
                //Если переход построен верно.
                if (!result.hasError) {
                    VertexGraph vertexFrom = (VertexGraph)transition.getObjectFrom();       //Преобразованеи безопасное, иначе бы result.hasError был бы false.
                                                                                            //Если переход построен из операторной вершины.
                    if (isOperator(vertexFrom) == true) {
                        vertexFrom.setColorBrush(colorCorrect);                           //Выделение вершины как верной.
                        vertexFrom.drawObject(originalBitmap);                            //Отрисовка вершины.
                        incProgress();                                                      //Увеличение прогресса.
                    }
                    //Если переохд построен из условной вершины.
                    else if (isCondition(vertexFrom) == true) {
                        //Определение, все ли переходы построены верно.
                        for (int i = 0; i < 3; i++) {
                            LineGraph line = vertexFrom.getPlaceOutputLine(i).getLine();
                            if ((line != null) && (line != transition)) {
                                result = checkingTheGraph.checkTransition(line);
                                if (!result.hasError) {
                                    vertexFrom.setColorBrush(colorCorrect);               //Выделение вершины как верной.
                                    vertexFrom.drawObject(originalBitmap);                //Отрисовка вершины.
                                    incProgress();                                          //Увеличение прогресса.
                                }
                                break;
                            }
                        }
                    }
                }
                //Если соединение построено не верно.
                else {
                    MessageBox.Show(result.explanationError);
                    ObjectGraph vertexFrom = transition.getObjectFrom();
                    vertexFrom.setColorBrush(colorNotCorrect);                            //Выделение вершины как не верной.
                    vertexFrom.drawObject(originalBitmap);                                //Отрисовка вершины.
                    incError();                                                             //Увеличение количества ошибок.
                }

                //Проверка путей, которые указывали на данный путь.
                for (int i = 0; i < aObjectGraph.Count; i++) {
                    if (aObjectGraph[i].getType() == TYPE_OBJECT.LINE) {
                        LineGraph tempLine = (LineGraph)aObjectGraph[i];
                        if (tempLine.getID() != transition.getID()) {
                            if (tempLine.getObjectTo() != null) {
                                if (tempLine.getObjectTo().getID() == transition.getID()) {
                                    checkingTransiton(tempLine);
                                }
                            }
                        }
                    }
                }
            }
        }

        //Проверка удаленного перехода.
        public void checkingDeletedTransition(LineGraph transition)
        {
            ObjectGraph vertexFrom = transition.getObjectFrom();

            if (vertexFrom.getColorBrush() != colorEmpty) {
                if (vertexFrom.getColorBrush() == colorCorrect) {
                    decProgress();
                }
                vertexFrom.setColorBrush(colorEmpty);
                vertexFrom.drawObject(originalBitmap);
                pictureBoxGraph.Image = originalBitmap;
            }

            /*Проверка путей, которые указывали на данный путь*/
            for (int i = 0; i < aObjectGraph.Count; i++) {
                if (aObjectGraph[i].getType() == TYPE_OBJECT.LINE) {
                    LineGraph tempLine = (LineGraph)aObjectGraph[i];
                    if (tempLine.getID() != transition.getID()) {
                        if (tempLine.getObjectTo() != null) {
                            if (tempLine.getObjectTo().getID() == transition.getID()) {
                                checkingDeletedTransition(tempLine);
                            }
                        }
                    }
                }
            }
        }



        //************************************************************************************************
        // Управление информацие о выполнении задания.
        //************************************************************************************************

        //Установка завершения выполнения задания.
        private void setDoneJob()
        {
            //Завершение счета времени.
            totalTick += DateTime.Now.Ticks - dateOpenForm.Ticks;   //Прибавление к общему времени.
            timer.Stop();                                             //Остановка таймера.

            //Отображение завершения выполнения задания.
            labelCondition.Text = "Выполнено";
            labelCondition.BackColor = Color.LimeGreen;
            formMain.setDoneGSA(true);
            MessageBox.Show("Задание по построению ГСА выполнено.", "Сообщение", MessageBoxButtons.OK, MessageBoxIcon.Information);
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
            formMain.setDoneGSA(false);
        }

        //Увеличение прогресса.
        private void incProgress()
        {
            progressBar.Value += 1;     //Увеличение прогресса.
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
            long tick = (DateTime.Now.Ticks - dateOpenForm.Ticks) + totalTick;    //Вычисление тиков.
            DateTime stopWatch = new DateTime();                                               //Инициализация промежутка времени.

            stopWatch = stopWatch.AddTicks(tick);                                             //Добавление времени.
            labelTime.Text = String.Format("{0:HH:mm:ss}", stopWatch);                             //Демонстрация времени.
        }


        //************************************************************************************************
        // Обработка событий мыши на поле для построения ГСА.
        //************************************************************************************************

        //Движение мыши.
        private void pictureBoxGraph_MouseMove(object sender, MouseEventArgs e)
        {
            Point cursor = new Point(e.X, e.Y);

            //Движение с зажатой клавишей.
            if (mousePress == true) {
                //Перемещение объекта.
                if (moveVertex.getEventFlag() == true) {
                    pictureBoxGraph.Image = moveVertex.move(cursor);
                }
                //Построение линии.
                else if (constructorLine.getEventFlag() == true) {
                    pictureBoxGraph.Image = constructorLine.construction(cursor);
                }
            }
            //Движение без зажатой клавиши.
            else {
                //Передвижение мыши для выбора места для создаваемого объекта.
                if (bufferCreatedVertex != null) {
                    moveVertex.preparation(bufferCreatedVertex, originalBitmap, new Point(0, 0));
                    mousePress = true;
                }
                //Передвижение мыши для выбора объекта.
                else {
                    detectorObject.detect(cursor);
                    //Если была реакция на вхождение или уход курсора с объекта.
                    if (detectorObject.bitmapWasChange() == true) {
                        pictureBoxGraph.Image = detectorObject.getDrawBitmap();   //Отображение изменения.
                        //Определение курсора.
                        if (detectorObject.getBufferObject() == null) {
                            this.Cursor = Cursors.Default;                          //Стандартный курсор.
                        }
                        else {
                            switch (detectorObject.getBufferObject().getType()) {
                                case TYPE_OBJECT.PLACE_FOR_LINE: {
                                    this.Cursor = Cursors.Hand;                 //Курсор для построения линии.
                                    break;
                                }
                                case TYPE_OBJECT.LINE: {
                                    this.Cursor = Cursors.No;                   //Курсор для удаления сегмента линии.
                                    break;
                                }
                                default: {
                                    this.Cursor = Cursors.SizeAll;              //Курсор для перемещения объекта.
                                    break;
                                }
                            }
                        }
                    }
                }
            }
        }

        //Нажатие мыши.
        private void pictureBoxGraph_MouseDown(object sender, MouseEventArgs e)
        {
            Point cursor = new Point(e.X, e.Y);
            ObjectGraph bufferObject = detectorObject.getBufferObject();

            //Нажатие клавиши для выбора начального места для вершины.
            if (bufferCreatedVertex != null) {
                addVertex();
            }

            //Если кнопка нажата на объекте.
            if (bufferObject != null) {
                switch (bufferObject.getType()) {
                    /////////////////////////////////////////////////////////////////////////////////////////////////
                    //Удаление сегмента линии.
                    case TYPE_OBJECT.LINE: {
                        LineGraph line = (LineGraph)bufferObject;                  //Приведение типов.
                        if (line.getObjectFrom() != null) {
                            if (line.getObjectFrom().getType() == TYPE_OBJECT.VERTEX) {
                                VertexGraph vertexFrom = (VertexGraph)(line.getObjectFrom());       //Получение вершины, из которой выходила линия.
                                checkingDeletedTransition(line);                                    //Проверка удаленного разорванного пути.
                                originalBitmap = constructorLine.deleteSegmentLine(line, originalBitmap, cursor); //Удаление сегментов линии с отображенеим на холсте.
                                pictureBoxGraph.Image = originalBitmap;                           //Отображение изменения на холсте.
                                detectorObject.setOriginalBitmap(originalBitmap);               //В распознаватель объектов посылаем новый холст.
                                this.Cursor = Cursors.Default;                                      //Изменение курсора мыши на обычный.
                                                                                                    //Проверяем необходимость разблокировки заблокированного 3 места.
                                if (isCondition(vertexFrom) == true) {
                                    checkUnLockPlaceFromCondition(vertexFrom);
                                }
                            }
                        }
                        break;
                    }
                    /////////////////////////////////////////////////////////////////////////////////////////////////
                    //Построение линии.
                    case TYPE_OBJECT.PLACE_FOR_LINE: {
                        PlaceForLine place = (PlaceForLine)detectorObject.getBufferObject();
                        //Если место для линии принадлежит линии, то продолжаем линию.
                        if (place.getObject().getType() == TYPE_OBJECT.LINE) {
                            pictureBoxGraph.Image = constructorLine.preparation((LineGraph)place.getObject(), originalBitmap);
                        }
                        //Если место для линии не принадлежит линии, то создаем новую линию.
                        else {
                            detectorObject.getBufferObject().setColorContour(Color.Black);    //Снимаем выделение у места для вывода линии.
                            pictureBoxGraph.Image = constructorLine.preparation(createLine(place), originalBitmap);
                            //Проверяем необходимость блокировки 3 места, если это условие.
                            if (isCondition(place.getObject()) == true) {
                                checkLockPlaceFromCondition((VertexGraph)place.getObject());
                            }
                            countID++;    //Увеличиваем индекс первого свободного идентификатора.
                        }

                        break;
                    }
                    /////////////////////////////////////////////////////////////////////////////////////////////////
                    //Перемещение объекта.
                    default: {
                        pictureBoxGraph.Image = moveVertex.preparation(detectorObject.getBufferObject(), detectorObject.getDrawBitmap(), cursor);
                        break;
                    }
                    /////////////////////////////////////////////////////////////////////////////////////////////////
                }
            }

            //Установка флага зажатой клавиши мыши.
            mousePress = true;
        }

        //Отжатие мыши.
        private void pictureBoxGraph_MouseUp(object sender, MouseEventArgs e)
        {
            //Если производилось перемещение объекта, то завершаем перемещение.
            if (moveVertex.getEventFlag() == true) {
                originalBitmap = (Bitmap)moveVertex.finish().Clone();        //Завершение перемещения с отображением на холсте.
                pictureBoxGraph.Image = originalBitmap;                             //Отображение изменения на экране.
                detectorObject.setOriginalBitmap(originalBitmap);                   //Загрузка нового холста в распознаватель объектов.
            }
            //Если производилось построение линии, то завершаем и проверяем построенный переход.
            else if (constructorLine.getEventFlag() == true) {
                LineGraph transition = constructorLine.getBufferLine();             //Получение построенной линии.
                ObjectGraph objectFrom = transition.getObjectFrom();                    //Получение объекта, из которого выходит линия.
                //Если линия выходит из логического условия, то добавляем текст.
                if (isCondition(objectFrom) == true) {
                    if (transition.getText() == null) {
                        TextGraph textGraph = new TextGraph(0, TYPE_OBJECT.TEXT, getPointTextLine((VertexGraph)objectFrom, (PlaceForLine)detectorObject.getBufferObject()), getTextLine((VertexGraph)objectFrom));
                        textGraph.setVisibleBrush(false);
                        textGraph.setVisibleContour(false);
                        textGraph.setObject(transition);                                    //Привязка линии к тексту.
                        transition.setText(textGraph);                                      //Привязка текста к линии.
                    }
                }
                originalBitmap = (Bitmap)constructorLine.finish().Clone();          //Завершение построения линии с отображением.
                checkingTransiton(transition);                                          //Проверка построенного перехода на корректность.
                detectorObject.setOriginalBitmap(originalBitmap);                   //Загрузка нового холста в распознаватель объектов.
                pictureBoxGraph.Image = originalBitmap;                             //Отображение изменения на экране.
            }

            //Сброс флага зажатой клавиши мыши.
            mousePress = false;
        }



        //************************************************************************************************
        // Создание и добавление вершин алгоритма.
        //************************************************************************************************

        //Создание операторной вершины.
        private void buttonCreateOperator_Click(object sender, EventArgs e)
        {
            //Свойства добавляемого объекта.
            int id = countID;
            int type = TYPE_OBJECT.VERTEX;
            Point[] aPointObject = new Point[5] { new Point(0, 0), new Point(70, 0), new Point(70, 30), new Point(0, 30), new Point(0, 0) };
            Point pointText = new Point(20, 3);
            string text = comboBoxOperator.Text;

            //Текст вершины.
            TextGraph textVertex = new TextGraph(0, TYPE_OBJECT.TEXT, pointText, text);
            textVertex.setVisibleContour(false);
            textVertex.setVisibleBrush(false);

            //Создание вершины.
            VertexGraph vertex = new VertexGraph(id, type, aPointObject, textVertex);
            vertex.setColorBrush(colorEmpty);
            vertex.setReactOutputPlaceOnCursor(false);

            //Добавление мест для линий.
            if (text[1] != '0') {
                vertex.addPlaceInputLine(new PlaceForLine(0, TYPE_OBJECT.PLACE_FOR_LINE, vertex, new Point(35, 0), new Size(8, 8)));    //Место для соединения линии сверху.
            }
            if (text[1] != 'k') {
                vertex.addPlaceOutputLine(new PlaceForLine(0, TYPE_OBJECT.PLACE_FOR_LINE, vertex, new Point(35, 30), new Size(8, 8)));  //Место для вывода линии снизу.
            }

            //Добавление объекта в буффер созданного объекта, но еще не добавленного.
            bufferCreatedVertex = vertex;
        }

        //Создание условной вершины.
        private void buttonCreateCondition_Click(object sender, EventArgs e)
        {
            //Свойства добавляемого объекта.
            int id = countID;
            int type = TYPE_OBJECT.VERTEX;
            Point[] aPointObject = new Point[5] { new Point(35, 0), new Point(70, 15), new Point(35, 30), new Point(0, 15), new Point(35, 0) };
            Point pointText = new Point(20, 3);
            string text = comboBoxCondition.Text;

            //Текст вершины.
            TextGraph textVertex = new TextGraph(0, TYPE_OBJECT.TEXT, pointText, text);
            textVertex.setVisibleContour(false);
            textVertex.setVisibleBrush(false);

            //Создание вершины.
            VertexGraph vertex = new VertexGraph(id, type, aPointObject, textVertex);
            vertex.setColorBrush(colorEmpty);
            vertex.setReactOutputPlaceOnCursor(false);

            //Добавление мест для линий.
            vertex.addPlaceInputLine(new PlaceForLine(0, TYPE_OBJECT.PLACE_FOR_LINE, vertex, new Point(35, 0), new Size(8, 8)));    //Место для соединения линии сверху.
            vertex.addPlaceOutputLine(new PlaceForLine(0, TYPE_OBJECT.PLACE_FOR_LINE, vertex, new Point(70, 15), new Size(8, 8)));  //Место для вывода линии справа.
            vertex.addPlaceOutputLine(new PlaceForLine(1, TYPE_OBJECT.PLACE_FOR_LINE, vertex, new Point(35, 30), new Size(8, 8)));  //Место для вывода линии снизу.
            vertex.addPlaceOutputLine(new PlaceForLine(2, TYPE_OBJECT.PLACE_FOR_LINE, vertex, new Point(0, 15), new Size(8, 8)));   //Место для вывода линии слева.

            //Добавление объекта в буфер созданного объекта, но еще не добавленного.
            bufferCreatedVertex = vertex;
        }

        //Добавление вершины.
        private void addVertex()
        {
            //Добавление объекта в список объектов.
            aObjectGraph.Add(bufferCreatedVertex);
            countID++;

            //Завершение перемещения объекта для выбора начального места.
            originalBitmap = moveVertex.finish();

            //Отображение измененного слоя.
            pictureBoxGraph.Image = originalBitmap;                   //Отобразить на экране.
            detectorObject.setOriginalBitmap(originalBitmap);       //Изменение слоя в распознавателе объектов.

            //Удаление из списка еще не добавленных объектов.
            if (isOperator(bufferCreatedVertex) == true) {
                comboBoxOperator.Items.Remove(bufferCreatedVertex.getText().getText());
                //Если список пуст, то блокируем кнопку добавления.
                if (comboBoxOperator.Items.Count == 0) {
                    buttonCreateOperator.Enabled = false;
                }
                //Если список не пуст, то устанавливаем указатель на первый элемент в списке.
                else {
                    comboBoxOperator.SelectedIndex = 0;
                }
            }
            else if (isCondition(bufferCreatedVertex) == true) {
                comboBoxCondition.Items.Remove(bufferCreatedVertex.getText().getText());
                //Если список пуст, то блокируем кнопку добавления.
                if (comboBoxCondition.Items.Count == 0) {
                    buttonCreateCondition.Enabled = false;
                }
                //Если список не пуст, то устанавливаем указатель на первый элемент в списке.
                else {
                    comboBoxCondition.SelectedIndex = 0;
                }
            }

            //Очистка буфера.
            bufferCreatedVertex = null;
        }


        //************************************************************************************************
        // Создание и добавление линии.
        //************************************************************************************************

        //Создание линии.
        public LineGraph createLine(PlaceForLine placeFrom)
        {
            //Параметры добавляемой линии.
            int id = countID;
            int type = TYPE_OBJECT.LINE; ;
            ObjectGraph objectFrom = placeFrom.getObject();
            Point startPoint = new Point(placeFrom.getPointForLine().X, placeFrom.getPointForLine().Y);

            //Создание линии.
            LineGraph lineGraph = new LineGraph(id, type, objectFrom, startPoint, null, new Size(8, 8));

            //Добавление линии в список объектов.
            aObjectGraph.Add(lineGraph);

            //Привязка линии к объекту.
            placeFrom.setLine(lineGraph, true);                     //Присоединение линии к месту.
            placeFrom.setLocked(true);                              //Место занято.
            placeFrom.setColorContour(Color.Black);                 //Снятие выделения места.

            return lineGraph;
        }

        //Определение текста линии.
        private string getTextLine(VertexGraph vertexFrom)
        {
            //Если условная вершина, то выбор истинного или ложного перехода.
            if (isCondition(vertexFrom) == true) {
                bool isFirstTransition = true;

                for (int i = 0; i < 3; i++) {
                    if (vertexFrom.getPlaceOutputLine(i).getLine() != null) {
                        if (vertexFrom.getPlaceOutputLine(i).getLine().getText() != null) {
                            isFirstTransition = false; break;
                        }
                    }
                }

                //Если еще ни одна линия не проведена.
                if (isFirstTransition == true) {
                    var result = MessageBox.Show("Первый переход будет истинным?", "Выбор логического условия", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                    if (result == DialogResult.Yes) {
                        return "+";
                    }
                    else {
                        return "-";
                    }
                }
                //Если одна линия уже проведена.
                else {
                    //Определение условия перехода по уже созданному переходу.
                    for (int i = 0; i < 3; i++) {
                        if (vertexFrom.getPlaceOutputLine(i).getLine() != null) {
                            if (vertexFrom.getPlaceOutputLine(i).getLine().getText() != null) {
                                if (vertexFrom.getPlaceOutputLine(i).getLine().getText().getText() == "+") {
                                    return "-";
                                }
                                else {
                                    return "+";
                                }
                            }
                        }
                    }
                }
            }

            //Если операторная вершина, то текста у линии нет.
            return "";
        }

        //Определение положения текста линии.
        private Point getPointTextLine(VertexGraph vertexFrom, PlaceForLine placeFrom)
        {
            //Если условная вершина то выбор положения текста относительно места для вывода линии.
            if (isCondition(vertexFrom) == true) {
                Point pointPlace = placeFrom.getPointArea();
                switch (placeFrom.getID()) {
                    case 0: return new Point(pointPlace.X + placeFrom.getSizeArea().Width, pointPlace.Y - 20);          //Справа.
                    case 1: return new Point(pointPlace.X + 5, pointPlace.Y);                                           //Снизу.
                    case 2: return new Point(pointPlace.X - 3, pointPlace.Y - 20);                                      //Слева.
                    default: return new Point(0, 0);
                }
            }
            else {
                return new Point(0, 0);
            }
        }



        //************************************************************************************************
        // Управление формой задания.
        //************************************************************************************************

        //Закрытие формы построения ГСА.
        private void FormGSA_FormClosing(object sender, FormClosingEventArgs e)
        {
            e.Cancel = true;     //Отменяем закрытие формы.
            this.Visible = false;    //Скрытие формы, чтобы заново потом при открытии ее не создавать.
        }

        //Скрытие и открытие окна.
        private void FormGSA_VisibleChanged(object sender, EventArgs e)
        {
            //Если счет должен вестись.
            if (progressBar.Value != progressBar.Maximum) {
                //Если окно открыли.
                if (this.Visible == true) {
                    dateOpenForm = DateTime.Now;                              //Запись текущего времени.
                    timer.Start();                                            //Запуск таймера.
                }
                //Если окно закрыли.
                else {
                    totalTick += DateTime.Now.Ticks - dateOpenForm.Ticks;   //Прибавление к общему времени.
                    timer.Stop();                                             //Остановка таймера.
                }
            }
        }


    }
}
