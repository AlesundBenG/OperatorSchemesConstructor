using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using ClassesGraph;

namespace InterfaceGSA
{
    public struct DetectorObject
    {
        /////////////////////////////////////Переменные/////////////////////////////////////
        /*Объекты*/
        private List<ObjectGraph>   m_aObjectGraph;         //Ссылка на список объектов.

        /*Буфер*/
        private ObjectGraph         m_bufferObject;         //Ссылка на обнаруженный объект.
        private int                 m_indexLineSegment;     //Сегмент линии, на котором был курсор.

        /*Слои*/
        private Bitmap              m_drawBitmap;           //Слой для рисования.
        private Bitmap              m_originalBitmap;       //Оригинальный слой.

        /*Инструменты рисования*/
        private Graphics            m_graph;                //Инструмент для отрисовки области объекта.
        private Pen                 m_pen;                  //Кисть для отрисовки области объекта.

        /*Флаги*/
        private bool                m_bitmapWasDisplayed;   //Измененный слой был отображен.



        /////////////////////////////////////Public методы/////////////////////////////////////
        /*Конструктор*/
        public DetectorObject(List<ObjectGraph> aObjectGraph, Bitmap originalBitmap)
        {
            m_aObjectGraph          = aObjectGraph;

            m_bufferObject          = null;
            m_indexLineSegment      = 0;

            m_originalBitmap        = (Bitmap)originalBitmap.Clone();
            m_drawBitmap            = (Bitmap)originalBitmap.Clone();

            m_graph                 = Graphics.FromImage(m_drawBitmap);
            m_pen                   = new Pen(Color.Gray, 1);
            m_pen.DashStyle         = System.Drawing.Drawing2D.DashStyle.Dash;

            m_bitmapWasDisplayed    = true;
        }

        /*Определение наличия объекта под курсором*/
        public bool detect(Point cursor)
        {
            //Если курсор не был на объекте, то проверяем вхождение курсора на объект.
            if (m_bufferObject == null) {
                //Проход с заду наперед, так как шанс того, что курсор будет наведен на объект, 
                //который недавно добавлен, весьма велик.
                for (int i = m_aObjectGraph.Count - 1; i >= 0; i--) {
                    switch (m_aObjectGraph[i].getType()) {
                        ////////////////////////////////////////////////////////////////////////////////////////
                        case TYPE_OBJECT.VERTEX: {
                                if (checkEnterCursorInVertex((VertexGraph)m_aObjectGraph[i], cursor) == true) {
                                    return true;    //Ответ - под курсором есть объект.
                                }
                                break;
                            }
                        ////////////////////////////////////////////////////////////////////////////////////////
                        case TYPE_OBJECT.LINE: {
                                if (checkEnterCursorInLine((LineGraph)m_aObjectGraph[i], cursor) == true) {
                                    return true;    //Ответ - под курсором есть объект.
                                }
                                break;
                            }
                        ////////////////////////////////////////////////////////////////////////////////////////
                        case TYPE_OBJECT.TEXT: {
                                if (checkEnterCursorInText((TextGraph)m_aObjectGraph[i], cursor) == true) {
                                    return true;    //Ответ - под курсором есть объект.
                                }
                                break;
                            }
                        ////////////////////////////////////////////////////////////////////////////////////////
                        default: {
                                if (checkEnterCursorInObject(m_aObjectGraph[i], cursor) == true) {
                                    return true;    //Ответ - под курсором есть объект.
                                }
                                break;
                            }
                        ////////////////////////////////////////////////////////////////////////////////////////
                    }
                }
                return false;   //Ответ - под курсором нет объекта.
            }
            //Если курсор был на объекте, то проверяем уход курсора с объекта.
            else {
                switch (m_bufferObject.getType()) {
                    ////////////////////////////////////////////////////////////////////////////////////////
                    case TYPE_OBJECT.VERTEX: {
                            if (checkLeaveCursorFromVertex((VertexGraph)m_bufferObject, cursor) == true) {
                                return detect(cursor);      //Проверка перехода на другой объект.
                            }
                            break;
                        }
                    ////////////////////////////////////////////////////////////////////////////////////////
                    case TYPE_OBJECT.LINE: {
                            if (checkLeaveCursorFromLine((LineGraph)m_bufferObject, cursor) == true) {
                                return detect(cursor);      //Проверка перехода на другой объект.
                            }
                            break;
                        }
                    ////////////////////////////////////////////////////////////////////////////////////////
                    case TYPE_OBJECT.PLACE_FOR_LINE: {
                            if (checkLeaveCursorFromPlaceForLine((PlaceForLine)m_bufferObject, cursor) == true) {
                                return detect(cursor);      //Проверка перехода на другой объект.
                            }
                            break;
                        }
                    ////////////////////////////////////////////////////////////////////////////////////////
                    case TYPE_OBJECT.TEXT: {
                            if (checkLeaveCursorFromText((TextGraph)m_bufferObject, cursor) == true) {
                                return detect(cursor);      //Проверка перехода на другой объект.
                            }
                            break;
                        }
                    ////////////////////////////////////////////////////////////////////////////////////////
                    default: {
                            if (checkLeaveCursorFromObject(m_bufferObject, cursor) == true) {
                                return detect(cursor);      //Проверка перехода на другой объект.
                            }   
                            break;
                        }
                    ////////////////////////////////////////////////////////////////////////////////////////
                }
                return true;    //Ответ - объект остается под курсором.
            }
        }

        /*Проверка изменения слоя*/
        public bool bitmapWasChange()
        {
            return !m_bitmapWasDisplayed;
        }

        /*Установка и получение слоев*/
        public void setOriginalBitmap(Bitmap originalBitmap)
        {
            m_originalBitmap    = (Bitmap)originalBitmap.Clone();   //Копирование оригинального слоя.
            m_drawBitmap        = (Bitmap)originalBitmap.Clone();   //Копирование оригинального слоя.
            m_graph             = Graphics.FromImage(m_drawBitmap); //Обновление инструмента.
            m_bufferObject      = null;                             //Освобождение буфера.
        }
        public Bitmap getDrawBitmap()
        {
            m_bitmapWasDisplayed = true;    //Измененный слой был отображен.
            return m_drawBitmap;
        }

        /*Получение объектов из буферов*/
        public ObjectGraph getBufferObject()
        {
            return m_bufferObject;
        }



        /////////////////////////////////////Private методы/////////////////////////////////////
        /*Скрытие места для линий*/
        private void hidePlaceForLine(PlaceForLine placeForLine)
        {
            Graphics graph = Graphics.FromImage(m_drawBitmap);

            //Получение характеристик места.
            Point   pointArea       = placeForLine.getPointArea();
            Size    sizeArea        = placeForLine.getSizeArea();
            int     widthContour    = placeForLine.getWidthPenContour();

            //Свойства области замены с учетом толщины пера контура.
            pointArea.X     = pointArea.X - widthContour;
            pointArea.Y     = pointArea.Y - widthContour;
            sizeArea.Width  = sizeArea.Width + 2 * widthContour;
            sizeArea.Height = sizeArea.Height + 2 * widthContour;

            //Замена области места оригинальным слоем.
            copyAreaBitmap(pointArea.X, pointArea.Y, sizeArea.Width, sizeArea.Height);
        }

        /*Выделение сегментов линии до курсора*/
        private void selectSegmentLine(LineGraph line, Point cursor)
        {
            line.setColorContour(Color.Red);                            //Цвет линии до курсора.

            for (int i = line.getCountPointLine() - 2; i >= 0; i--) {
                line.drawSegmentLine(m_drawBitmap, i);                  //Отрисовка сегмента.
                if (line.cursorOnSegmentLine(i, cursor) == true) {
                    m_indexLineSegment = i;                             //Запись индекса сегмента в буфер.
                    line.setColorContour(Color.Black);                  //Цвет линии после курсора.
                }
            }
        }

        /*Проверка входа или выхода курсора с объекта*/
        private bool checkEnterCursorInObject(ObjectGraph objectGraph, Point cursor)
        {
            if (objectGraph.cursorOnArea(cursor) == true) {
                objectGraph.setColorContour(Color.Red);                                                             //Установка выделение объекта.
                objectGraph.drawObject(m_drawBitmap);                                                               //Отрисовка выделенного объекта.
                m_graph.DrawRectangle(m_pen, new Rectangle(objectGraph.getPointArea(), objectGraph.getSizeArea())); //Отрисовка области объекта.
                m_bufferObject = objectGraph;                                                                       //Загрузка объекта в буфер.
                m_bitmapWasDisplayed = false;                                                                       //Слой был изменен, но не отображен.
                return true;                                                                                        //Ответ - курсор попал на объект
            }
            else {
                return false;                                                                                       //Ответ - курсор не попал на объект.
            }
        }
        private bool checkLeaveCursorFromObject(ObjectGraph objectGraph, Point cursor)
        {
            if (objectGraph.cursorOnArea(cursor) == true) {
                return false;                                                               //Ответ - курсор не ушел с объекта.
            }
            else {
                deleteAreaContour(objectGraph.getPointArea(), objectGraph.getSizeArea());   //Удаление контура объекта.
                objectGraph.setColorContour(Color.Black);                                   //Снятие выделения объекта.
                objectGraph.drawObject(m_drawBitmap);                                       //Отрисовка не выделенного объекта.
                m_bitmapWasDisplayed = false;                                               //Слой был изменен, но не отображен.
                m_bufferObject = null;                                                      //Освобождение буфера.
                return true;                                                                //Ответ - курсор ушел с объекта.
            }
        }

        /*Проверка входа или выхода курсора с текста*/
        private bool checkEnterCursorInText(TextGraph text, Point cursor)
        {
            if (checkEnterCursorInObject(text, cursor) == true) {
                //Если с текстом связан объект, то выделяем контур данного объекта.
                if (text.getObject() != null) {
                    text.getObject().setColorContour(Color.Red);
                    text.getObject().drawObject(m_drawBitmap);
                }
                return true;
            }
            else {
                return false;
            }
        }
        private bool checkLeaveCursorFromText(TextGraph text, Point cursor)
        {
            if (checkLeaveCursorFromObject(text, cursor) == true) {
                //Если с текстом связан объект, то снимаем выделение контура данного объекта.
                if (text.getObject() != null) {
                    text.getObject().setColorContour(Color.Black);
                    text.getObject().drawObject(m_drawBitmap);
                }
                return true;
            }
            else {
                return false;
            }
        }

        /*Проверка входа или выхода курсора с вершины*/
        private bool checkEnterCursorInVertex(VertexGraph vertex, Point cursor)
        {
            //Проверка курсора на вершие.
            if (vertex.cursorOnArea(cursor) == true) {
                m_bufferObject = vertex;
            }

            //Проврка курсора на месте для вывода линии.
            for (int i = 0; i < vertex.getCountPlaceOutputLine(); i++) {
                if (vertex.getPlaceOutputLine(i).cursorOnArea(cursor) == true) {
                    m_bufferObject = vertex.getPlaceOutputLine(i);
                    break;
                }
            }

            //Если курсор указывает на объект.
            if ((m_bufferObject != null)) {
                if (m_bufferObject.getType() == TYPE_OBJECT.PLACE_FOR_LINE) {
                    m_bufferObject.setColorContour(Color.Red);                  //Выделение места для вывода линии.
                }
                else {
                    vertex.setColorContour(Color.Red);                          //Установка выделения вершины.
                    vertex.drawObject(m_drawBitmap);                            //Отрисовка выделенной вершины.
                }
                vertex.drawPlaceOutputLine(m_drawBitmap);                       //Отрисовка мест для вывода линии.
                m_bitmapWasDisplayed = false;                                   //Слой был изменен, но не отображен.
                return true;                                                    //Ответ - курсор попал на объект
            }
            //Если курсор не указывает на объект.
            else {
                return false;                                                   //Ответ - курсор не попал на объект.
            }
        }
        private bool checkLeaveCursorFromVertex(VertexGraph vertex, Point cursor)
        {
            //Проверка перехода курсора на место для линии.
            for (int i = 0; i < vertex.getCountPlaceOutputLine(); i++) {
                if (vertex.getPlaceOutputLine(i).cursorOnArea(cursor) == true) {
                    vertex.setColorContour(Color.Black);                          //Установка выделения вершины.
                    vertex.drawObject(m_drawBitmap);                            //Отрисовка выделенной вершины.
                    m_bufferObject = vertex.getPlaceOutputLine(i);                  //Помещение места в буфер.
                    m_bufferObject.setColorContour(Color.Red);                      //Выделение места для вывода линии.
                    vertex.drawPlaceOutputLine(m_drawBitmap);                       //Отрисовка мест для вывода линии.
                    m_bitmapWasDisplayed = false;                                   //Слой был изменен, но не отображен.
                    return false;                                                   //Ответ - курсор не ушел с объекта.
                }
            }

            //Если курсор ушел с вершины окончательно.
            if (vertex.cursorOnArea(cursor) == false) {
                for (int i = 0; i < vertex.getCountPlaceOutputLine(); i++) {
                    hidePlaceForLine(vertex.getPlaceOutputLine(i));                 //Скрытие места для вывода линий.
                }
                vertex.setColorContour(Color.Black);                                //Снятие выделения контура.
                vertex.drawObject(m_drawBitmap);                                    //Отрисовка изменения.
                m_bitmapWasDisplayed = false;                                       //Слой был изменен, но не отображен.
                m_bufferObject = null;                                              //Курсор не указвыает на объект.
                return true;                                                        //Ответ - курсор ушел с объекта.  
            }
            //Если курсор остается на объекте.
            else {
                return false;                                                       //Ответ - курсор не ушел с объекта. 
            }
        }

        /*Проверка входа или выхода курсора с линии*/
        private bool checkEnterCursorInLine(LineGraph line, Point cursor)
        {
            //Проверка курсора на линии.
            if (line.cursorOnArea(cursor) == true) {
                m_bufferObject = line; 
            }

            //Првоерка курсора на месте для выведения линии.
            if (line.getPlaceContinueLine().cursorOnArea(cursor) == true) {
                m_bufferObject = line.getPlaceContinueLine();
            }

            //Проверка курсора на тексте.
            if (line.getText() != null) {
                if (line.getText().cursorOnArea(cursor) == true) {
                    m_bufferObject = line.getText();
                }
            }

            //Если курсор указывает на объект.
            if (m_bufferObject != null) {
                switch (m_bufferObject.getType()) {
                    ////////////////////////////////////////////////////////////////////////////////////////
                    //Если курсор на месте для вывода линии.
                    case TYPE_OBJECT.PLACE_FOR_LINE: {
                            m_bufferObject.setColorContour(Color.Red);              //Выделение места.
                            m_bufferObject.drawObject(m_drawBitmap);                //Отображение выделения.
                            break;
                        }
                    ////////////////////////////////////////////////////////////////////////////////////////
                    //Если курсор на тексте линии.
                    case TYPE_OBJECT.TEXT: {
                            line.setColorContour(Color.Red);                        //Выделение линии для обозначения принадлежности текста.
                            line.drawObject(m_drawBitmap);                          //Отображение выделения линии.
                            line.setColorContour(Color.Black);                      //Снятие выделения линии, так как курсор не на линии.
                            checkEnterCursorInObject(m_bufferObject, cursor);       //Вход на текст, как на обычный объект.
                            break;
                        }
                    ////////////////////////////////////////////////////////////////////////////////////////
                    //Если курсор на самой линии.
                    default: {
                            selectSegmentLine(line, cursor);                        //Выделение сегментов линии для удаления.
                            line.getPlaceContinueLine().drawObject(m_drawBitmap);   //Отрисовка места для продолжения линии.
                            break;
                        }
                    ////////////////////////////////////////////////////////////////////////////////////////
                }
                m_bitmapWasDisplayed = false;                                       //Слой был изменен, но не отображен.
                return true;                                                        //Ответ - курсор попал на объект
            }
            //Если курсор не указывает на объект.
            else {
                return false;                                                       //Ответ - курсор не попал на объект.
            }
        }
        private bool checkLeaveCursorFromLine(LineGraph line, Point cursor)
        {
            //Проверка перехода курсора на место для линии.
            if (line.getPlaceContinueLine().cursorOnArea(cursor) == true) {
                line.drawObject(m_drawBitmap);                                          //Снятие выделения объекта.
                m_bufferObject = line.getPlaceContinueLine();                           //Запись в буфер места для линии.
                m_bufferObject.setColorContour(Color.Red);                              //Выделение места.
                m_bufferObject.drawObject(m_drawBitmap);                                //Отрисовка выделения места.
                m_bitmapWasDisplayed = false;                                           //Слой был изменен, но не отображен.
                return false;                                                           //Ответ - курсор не ушел с объекта.
            }

            //Если курсор ушел с сегмента линии.
            if (line.cursorOnSegmentLine(m_indexLineSegment, cursor) == false) {
                //Если курсор ушел с линии.
                if (line.cursorOnArea(cursor) == false) {
                    line.setColorContour(Color.Black);                                  //Снятие выделения контура.
                    line.drawObject(m_drawBitmap);                                      //Отрисовка изменения.
                    hidePlaceForLine(line.getPlaceContinueLine());                      //Скрытие места для продолжение линий.
                    m_bufferObject = null;                                              //Курсор не указвыает на объект.
                    m_bitmapWasDisplayed = false;                                       //Слой был изменен, но не отображен.
                    return true;                                                        //Ответ - курсор ушел с объекта. 
                }
                //Если курсор остался на линии.
                else {
                    selectSegmentLine(line, cursor);                                    //Выделение сегментов линии для удаления.
                    line.getPlaceContinueLine().drawObject(m_drawBitmap);               //Отрисовка места для линии.
                    m_bitmapWasDisplayed = false;                                       //Слой был изменен, но не отображен.
                    return false;                                                       //Ответ - курсор осталься на объекте.
                }
            }
            //Если курсор не ушел с сегмента для линии.
            else {
                return false;                                                           //Ответ - курсор осталься на объекте.
            }
        }

        /*Проверка ухода курсора с места для линии*/
        private bool checkLeaveCursorFromPlaceForLine(PlaceForLine place, Point cursor)
        {
            //Если кусрор ушел с места для линии.
            if (place.cursorOnArea(cursor) == false) {
                place.setColorContour(Color.Black);         //Снятие выделения места.
                place.drawObject(m_drawBitmap);             //Отрисовка изменения.
                m_bitmapWasDisplayed = false;               //Слой был изменен, но не отображен.
                m_bufferObject = null;                      //Освобождение буфера.
                //Реакция объекта, связанного с местом.
                if (place.getObject() != null) {
                    switch (place.getObject().getType()) {
                        ////////////////////////////////////////////////////////////////////////////////////////
                        case TYPE_OBJECT.VERTEX: {
                                //Проверка выхода с объекта, связанноо с местом.
                                if (checkLeaveCursorFromVertex((VertexGraph)place.getObject(), cursor) == true) {
                                    return true;
                                }
                                else {
                                    checkEnterCursorInVertex((VertexGraph)place.getObject(), cursor);
                                    return true;
                                }
                            }
                        ////////////////////////////////////////////////////////////////////////////////////////
                        case TYPE_OBJECT.LINE: {
                                //Проверка возвращения курсора на объект.
                                if (checkLeaveCursorFromLine((LineGraph)place.getObject(), cursor) == true) {
                                    return true;
                                }
                                else {
                                    checkEnterCursorInLine((LineGraph)place.getObject(), cursor);
                                    return true;
                                }
                            }
                        ////////////////////////////////////////////////////////////////////////////////////////
                    }
                }
                return true;        //Ответ - ушел с места для вывода линии.
            }
            //Если курсор не ушел с места для линии.
            else {
                return false;       //Ответ - курсор остается на месте для вывода линии.
            }
        }

        /*Получение видимой области прямоугольнкиа*/
        private Rectangle getVisibleRectangle(int x, int y, int width, int height, Size area)
        {
            //Левая граница контура выходит за левую границу области.
            if (x < 0) {
                //Если объект не полностью выходит, то вычисляем видимую часть,
                //Иначе ширина видимой части равняется 0.
                width   = (x + width > 0) ? x + width : 0;
                x       = 0;
            }
            //Правая граница контура выходит за правую границу области.
            if (x + width > area.Width) {
                //Если объект не полностью выходит, то вычисляем видимую часть,
                //Иначе ширина видимой части равняется 0.
                width = (x < area.Width) ? area.Width - x : 0;
            }

            //Верхняя граница контура выходит за верхнюю границу области.
            if (y < 0) {
                //Если объект не полностью выходит, то вычисляем видимую часть,
                //Иначе высота видимой части равняется 0.
                height = (y + height > 0) ? y + height : 0;
                y = 0;
            }
            //Нижняя граница контура выходит за нижнюю границу слоя.
            if (y + height > area.Height) {
                //Если объект не полностью выходит, то вычисляем видимую часть,
                //Иначе ширина видимой части равняется 0.
                height = (y < area.Height) ? area.Height - y : 0;
            }
            return new Rectangle(x, y, width, height);
        }

        /*Удаление контура области объекта*/
        private void deleteAreaContour(Point point, Size size)
        {
            //Вычисление видимой части контура.
            Rectangle rectangle = getVisibleRectangle(point.X, point.Y, size.Width, size.Height, m_drawBitmap.Size);

            //Если частично или полностью входит в область видимоси, то удаляем конутр. 
            if ((rectangle.Width != 0) && (rectangle.Height != 0)) {
                bool drawUp     = true;
                bool drawDown   = true;
                bool drawLeft   = true;
                bool drawRigth  = true;
                //Левая граница выходит за область.
                if (point.X < 0) {
                    drawLeft = false;
                }
                //Верхняя граница выходит за область.
                if (point.Y < 0) {
                    drawUp = false;
                }
                //Правая граница выходит за область.
                if (point.X + size.Width > m_drawBitmap.Width) {
                    drawRigth = false;
                }
                //Левая граница выходит за область.
                if (point.Y + size.Height > m_drawBitmap.Height) {
                    drawDown = false;
                }
                //Отрисовка линий, входящих в область.
                Rectangle lineDraw;
                if (drawUp == true) {
                    lineDraw = new Rectangle(rectangle.X, rectangle.Y, rectangle.Width, 1);
                    m_graph.DrawImage(m_originalBitmap.Clone(lineDraw, System.Drawing.Imaging.PixelFormat.Format64bppArgb), rectangle.Location);
                }
                if (drawDown == true) {
                    lineDraw = new Rectangle(rectangle.X, rectangle.Y + rectangle.Height, rectangle.Width, 1);
                    m_graph.DrawImage(m_originalBitmap.Clone(lineDraw, System.Drawing.Imaging.PixelFormat.Format64bppArgb), rectangle.X, rectangle.Y + rectangle.Height);
                }
                if (drawLeft == true) {
                    lineDraw = new Rectangle(rectangle.X, rectangle.Y, 1, rectangle.Height);
                    m_graph.DrawImage(m_originalBitmap.Clone(lineDraw, System.Drawing.Imaging.PixelFormat.Format64bppArgb), rectangle.Location);
                }
                if (drawRigth == true) {
                    lineDraw = new Rectangle(rectangle.X + rectangle.Width, rectangle.Y, 1, rectangle.Height + 1);
                    m_graph.DrawImage(m_originalBitmap.Clone(lineDraw, System.Drawing.Imaging.PixelFormat.Format64bppArgb), rectangle.X + rectangle.Width, rectangle.Y);
                }
            }
        }

        /*Замена участка слоя другим слоем*/
        private void copyAreaBitmap(int x, int y, int width, int height)
        {
            //Вычисление видимой заменяемого слоя.
            Rectangle rectangle = getVisibleRectangle(x, y, width, height, m_drawBitmap.Size);

            //Если слой входит в видимую часть, то заменяем ее.
            if ((rectangle.Width > 0) && (rectangle.Height > 0)) {
                Bitmap copyArea = m_originalBitmap.Clone(rectangle, System.Drawing.Imaging.PixelFormat.Format64bppArgb);
                m_graph.DrawImage(copyArea, rectangle.Location);
            }
        }

    }

    public struct MoveObject
    {
        /////////////////////////////////////Переменные для перемещения/////////////////////////////////////
        /*Объекты*/
        private List<ObjectGraph>   m_aObjectGraph;   //Ссылка на список объектов.

        /*Перемещаемый объект*/
        private ObjectGraph         m_bufferObject;     //Ссылка на перемещаемый объект.
        private Rectangle           m_areaObject;       //Область перемещаемого объекта.

        /*Инструменты рисования*/
        private Graphics            m_graph;            //Инструмент для отрисовки линий для выравнивания и области объекта.
        private Pen                 m_pen;              //Кисть для рисования линий выравнвиания и области объекта.

        /*Слои*/
        private Bitmap              m_drawBitmap;       //Слой для рисования.
        private Bitmap              m_originalBitmap;   //Оригинальный слой.
        private Color               m_backColor;        //Цвет фона.

        /*Фраги*/
        private bool                m_eventFlag;        //Событие передвижения объекта.

        /*Координаты мыши*/
        private Point               m_lastPointCursor;  //Последние координаты мыши.

        /*Выравнивание с другими объектами*/
        private int                 m_vertical;         //Индекс объекта, с которым было вертикальное выравнивание.
        private int                 m_horizontal;       //Индекс объекта, с которым было горизонтальное выравнивание.
        /*При выравнивании происходит смещение объекта.
          При нежелании выравнить объекты, смещение нужно вернуть,
          Иначе объект будет "уплывать" из-под курсора при каждом "отлипании" при нежелании выравнивания*/
        private int                 m_offestX;          //Смещение для выравнивания.
        private int                 m_offestY;          //Смещение для выравнивания.

        /////////////////////////////////////Public методы/////////////////////////////////////
        /*Конструктор*/
        public MoveObject(List<ObjectGraph> aObjectGraph, Color backColor)
        {
            m_aObjectGraph      = aObjectGraph;

            m_bufferObject      = null;
            m_areaObject        = new Rectangle(0, 0, 0, 0);

            m_graph             = null;
            m_pen               = new Pen(Color.Gray, 1);
            m_pen.DashStyle     = System.Drawing.Drawing2D.DashStyle.Dash;

            m_eventFlag         = false;


            m_drawBitmap        = null;
            m_originalBitmap    = null;
            m_backColor         = backColor;

            m_lastPointCursor   = new Point(0, 0);

            m_vertical          = -1;
            m_horizontal        = -1;
            m_offestX           = 0;
            m_offestY           = 0;
        }

        /*Получение флага события перемещения вершины*/
        public bool getEventFlag()
        {
            return m_eventFlag;
        }

        /*Установка цвета фона*/
        public void setBackColor(Color backColor)
        {
            m_backColor = backColor;
        }

        /*Этапы перемещения вершины*/
        public Bitmap preparation(ObjectGraph objectGraph, Bitmap originalBitmap, Point cursor)
        {
            //Фиксация события перемещения.
            m_eventFlag             = true;
            m_bufferObject          = objectGraph;

            //Копирование слоев.
            m_originalBitmap        = (Bitmap)originalBitmap.Clone();
            m_drawBitmap            = (Bitmap)originalBitmap.Clone();
            m_graph                 = Graphics.FromImage(m_drawBitmap);

            //Параметры контура.
            m_areaObject.Location   = m_bufferObject.getPointArea();
            m_areaObject.Size       = m_bufferObject.getSizeArea();

            //Объекты для выравнивания.
            m_vertical              = -1;
            m_horizontal            = -1;
            m_offestX               = 0;
            m_offestY               = 0;

            //Текущее положение курсора.
            m_lastPointCursor       = cursor;

            //Отрисовываем контур объекта.
            m_graph.DrawRectangle(m_pen, m_areaObject);

            //Возвращение подготовленного слоя.
            return m_drawBitmap;
        }
        public Bitmap move(Point cursor)
        {
            Point dPoint = new Point(cursor.X - m_lastPointCursor.X, cursor.Y - m_lastPointCursor.Y);   //Смещение курсора отсносительно зафиксированной точки мыши.
            deleteAreaContour(m_areaObject.Location, m_areaObject.Size);                                //Скрытие контура прошлого положения.

            //Вертикальное выравнивание.
            if (m_vertical == -1) {
                searchVertexForSmoothVertiacl(cursor.X, dPoint.X);
            }
            else {
                checkCancelSmoothVertical(cursor.X, dPoint.X);
            }
            //Горизонтальное выравнивание.
            if (m_horizontal == -1) {
                searchVertexForSmoothHorizontal(cursor.Y, dPoint.Y);
            }
            else {
                checkCancelSmoothHorizontal(cursor.Y, dPoint.Y);
            }

            //Отрисовка пунктира контура.
            m_graph.DrawRectangle(m_pen, m_areaObject);                                                 

            //Возвращение измененного слоя.
            return m_drawBitmap;
        }
        public Bitmap finish()
        {
            //Перемещение объекта.
            Point vectorMove = new Point(m_areaObject.X - m_bufferObject.getPointArea().X, m_areaObject.Y - m_bufferObject.getPointArea().Y);
            m_bufferObject.moveObject(vectorMove);
            m_bufferObject.setColorContour(Color.Black);

            //Очистка холста.
            m_graph.Clear(m_backColor);

            //Отрисовка объектов.
            for (int i = m_aObjectGraph.Count - 1; i >= 0; i--) {
                m_aObjectGraph[i].drawObject(m_drawBitmap);
            }

            //Сброс флага события.
            m_eventFlag = false;

            //Возвращение холста после перемещения.
            return m_drawBitmap;
        }

        /////////////////////////////////////Private методы/////////////////////////////////////
        /*Выравнвиание объектов с другими объектами*/
        private void searchVertexForSmoothVertiacl(int cursorX, int dX)
        {
            Point pointBuffer = m_areaObject.Location;
            
            //Проход по вершинам с поиском объекда для выравнвиания.
            for (int i = m_aObjectGraph.Count - 1; i >= 0; i--) {
                if (m_aObjectGraph[i].getID() != m_bufferObject.getID()) {
                    if ((m_aObjectGraph[i].getPointArea().X - 5 < pointBuffer.X) && (m_aObjectGraph[i].getPointArea().X + 5 > pointBuffer.X)) {
                        m_vertical = i;
                        break;
                    }
                }
            }

            //Если не нашли объект для выравнивания, то просто сдвигаем объект.
            if (m_vertical == -1) {
                m_areaObject.X      = m_areaObject.X + dX;                                          //Передвижение объекта по горизонтали.
                m_lastPointCursor.X = cursorX;                                                      //Изменение предыдущей координаты мыши.     
            }
            //Если нашли объект для выравнивания, то подстраиваем положение под объект.
            else {
                m_offestX       = m_areaObject.X - m_aObjectGraph[m_vertical].getPointArea().X;     //Высчитывание смещения для выравнивания.
                m_areaObject.X  = m_areaObject.X - m_offestX;                                       //Выравнвиание по X.
                m_graph.DrawLine(m_pen, m_areaObject.X, 0, m_areaObject.X, m_drawBitmap.Height);    //Отрисовка полосы выравнвиания.
            }
        }
        private void searchVertexForSmoothHorizontal(int cursorY, int dY)
        {
            Point pointBuffer = m_areaObject.Location;

            //Проход по вершинам с поиском объекда для выравнвиания.
            for (int i = m_aObjectGraph.Count - 1; i >= 0; i--) {
                if (m_aObjectGraph[i].getID() != m_bufferObject.getID()) {
                    if ((m_aObjectGraph[i].getPointArea().Y - 5 < pointBuffer.Y) && (m_aObjectGraph[i].getPointArea().Y + 5 > pointBuffer.Y)) {
                        m_horizontal = i;
                        break;
                    }
                }
            }
            //Если не нашли объект для выравнивания, то просто сдвигаем объект.
            if (m_horizontal == -1) {
                m_areaObject.Y      = m_areaObject.Y + dY;                                          //Передвижение объекта по горизонтали.
                m_lastPointCursor.Y = cursorY;                                                      //Изменение предыдущей координаты мыши.        
            }
            //Если нашли объект для выравнивания, то подстраиваем положение под объект.
            else {
                m_offestY       = m_areaObject.Y - m_aObjectGraph[m_horizontal].getPointArea().Y;   //Высчитывание смещения для выравнивания.
                m_areaObject.Y  = m_areaObject.Y - m_offestY;                                       //Выравнвиание по Y.
                m_graph.DrawLine(m_pen, 0, m_areaObject.Y, m_drawBitmap.Width, m_areaObject.Y);     //Отрисовка полосы выравнвиания.
            }
        }
        private void checkCancelSmoothVertical(int cursorX, int dX)
        {
            //Если вышли за предел вертикального выравнвиания, то отвязываем привязку.
            if (Math.Abs(dX) > 10) {
                copyAreaBitmap(m_areaObject.X, 0, 1, m_drawBitmap.Height);                          //Удаление полосы выравнвиаия.
                m_vertical              = -1;                                                       //Сброс флага вертикального выравнивания.
                m_areaObject.X          = m_areaObject.X + dX + m_offestX;                          //Передвижение объекта по горизонтали с восстановлением смещения.
                m_lastPointCursor.X     = cursorX;                                                  //Отвязываем зафиксированную точку мыши.
            }
            else {
                m_graph.DrawLine(m_pen, m_areaObject.X, 0, m_areaObject.X, m_drawBitmap.Height);    //Отрисовка полосы выравнивания.
            }
        }
        private void checkCancelSmoothHorizontal(int cursorY, int dY)
        {
            //Если вышли за предел вертикального выравнвиания, то отвязываем привязку.
            if (Math.Abs(dY) > 10) {
                copyAreaBitmap(0, m_areaObject.Y, m_drawBitmap.Width, 1);                       //Удаление полосы выравнвиаия.
                m_horizontal            = -1;                                                   //Сброс флага горизонтального выравнивания.
                m_areaObject.Y          = m_areaObject.Y + dY + m_offestY;                      //Передвижение объекта по вертикали с восстановлением смещения.
                m_lastPointCursor.Y     = cursorY;                                              //Отвязываем зафиксированную точку мыши.
            }
            else {
                m_graph.DrawLine(m_pen, 0, m_areaObject.Y, m_drawBitmap.Width, m_areaObject.Y); //Отрисовка полосы выравнивания.
            }
        }

        /*Получение видимой области квадрата*/
        private Rectangle getVisibleRectangle(int x, int y, int width, int height, Size area)
        {
            //Левая граница контура выходит за левую границу области.
            if (x < 0) {
                //Если объект не полностью выходит, то вычисляем видимую часть,
                //Иначе ширина видимой части равняется 0.
                width = (x + width > 0) ? x + width : 0;
                x = 0;
            }
            //Правая граница контура выходит за правую границу области.
            if (x + width > area.Width) {
                //Если объект не полностью выходит, то вычисляем видимую часть,
                //Иначе ширина видимой части равняется 0.
                width = (x < area.Width) ? area.Width - x : 0;
            }

            //Верхняя граница контура выходит за верхнюю границу области.
            if (y < 0) {
                //Если объект не полностью выходит, то вычисляем видимую часть,
                //Иначе высота видимой части равняется 0.
                height = (y + height > 0) ? y + height : 0;
                y = 0;
            }
            //Нижняя граница контура выходит за нижнюю границу слоя.
            if (y + height > area.Height) {
                //Если объект не полностью выходит, то вычисляем видимую часть,
                //Иначе ширина видимой части равняется 0.
                height = (y < area.Height) ? area.Height - y : 0;
            }
            return new Rectangle(x, y, width, height);
        }

        /*Удаление контура области объекта*/
        private void deleteAreaContour(Point point, Size size)
        {
            //Вычисление видимой части контура.
            Rectangle rectangle = getVisibleRectangle(point.X, point.Y, size.Width, size.Height, m_drawBitmap.Size);

            //Если частично или полностью входит в область видимоси, то удаляем конутр. 
            if ((rectangle.Width != 0) && (rectangle.Height != 0)) {
                bool drawUp = true;
                bool drawDown = true;
                bool drawLeft = true;
                bool drawRigth = true;
                //Левая граница выходит за область.
                if (point.X < 0) {
                    drawLeft = false;
                }
                //Верхняя граница выходит за область.
                if (point.Y < 0) {
                    drawUp = false;
                }
                //Правая граница выходит за область.
                if (point.X + size.Width > m_drawBitmap.Width) {
                    drawRigth = false;
                }
                //Левая граница выходит за область.
                if (point.Y + size.Height > m_drawBitmap.Height) {
                    drawDown = false;
                }
                //Отрисовка линий, входящих в область.
                Rectangle lineDraw;
                if (drawUp == true) {
                    lineDraw = new Rectangle(rectangle.X, rectangle.Y, rectangle.Width, 1);
                    m_graph.DrawImage(m_originalBitmap.Clone(lineDraw, System.Drawing.Imaging.PixelFormat.Format64bppArgb), rectangle.Location);
                }
                if (drawDown == true) {
                    lineDraw = new Rectangle(rectangle.X, rectangle.Y + rectangle.Height, rectangle.Width, 1);
                    m_graph.DrawImage(m_originalBitmap.Clone(lineDraw, System.Drawing.Imaging.PixelFormat.Format64bppArgb), rectangle.X, rectangle.Y + rectangle.Height);
                }
                if (drawLeft == true) {
                    lineDraw = new Rectangle(rectangle.X, rectangle.Y, 1, rectangle.Height);
                    m_graph.DrawImage(m_originalBitmap.Clone(lineDraw, System.Drawing.Imaging.PixelFormat.Format64bppArgb), rectangle.Location);
                }
                if (drawRigth == true) {
                    lineDraw = new Rectangle(rectangle.X + rectangle.Width, rectangle.Y, 1, rectangle.Height + 1);
                    m_graph.DrawImage(m_originalBitmap.Clone(lineDraw, System.Drawing.Imaging.PixelFormat.Format64bppArgb), rectangle.X + rectangle.Width, rectangle.Y);
                }
            }
        }

        /*Замена участка слоя другим слоем*/
        private void copyAreaBitmap(int x, int y, int width, int height)
        {
            //Вычисление видимой заменяемого слоя.
            Rectangle rectangle = getVisibleRectangle(x, y, width, height, m_drawBitmap.Size);

            //Если слой входит в видимую часть, то заменяем ее.
            if ((rectangle.Width > 0) && (rectangle.Height > 0)) {
                Bitmap copyArea = m_originalBitmap.Clone(rectangle, System.Drawing.Imaging.PixelFormat.Format64bppArgb);
                m_graph.DrawImage(copyArea, rectangle.Location);
            }
        }

    }

    public struct ConstructorLine
    {
        /////////////////////////////////////Переменные для перемещения/////////////////////////////////////
        /*Объекты*/
        private List<ObjectGraph>   m_aObjectGraph;     //Ссылка на список объектов.

        /*Редактируемая линия*/
        private LineGraph           m_line;             //Ссылка на редактируемую линию.

        /*Инструменты рисования*/
        private Graphics            m_graph;            //Инструмент для отрисовки линий для выравнивания.
        private Pen                 m_pen;              //Кисть для рисования строящегося отрезка.

        /*Слои*/
        private Bitmap              m_drawBitmap;       //Слой для рисования.
        private Bitmap              m_originalBitmap;   //Оригинальный слой.
        private Color               m_backColor;        //Цвет фона.

        /*Фраги*/
        private bool                m_eventFlag;        //Событие построения линии.

        /*Объекты привязки*/
        private PlaceForLine        m_placeTo;          //Ссылка на место для соединения линии.
        private LineGraph           m_lineTo;           //Ссылка на линию для соединения.

        /*Точки строящейся линии*/
        private Point               m_oldPoint;         //Последняя точка построенной линии
        private Point               m_nowPoint;         //Новая точка, которая будет посленей после завершения

        /////////////////////////////////////Public методы/////////////////////////////////////
        /*Конструктор*/
        public ConstructorLine(List<ObjectGraph> aObjectGraph, Color backColor)
        {
            m_aObjectGraph      = aObjectGraph;

            m_line              = null;

            m_graph             = null;
            m_pen               = new Pen(Color.Gray, 1);
            m_pen.DashStyle     = System.Drawing.Drawing2D.DashStyle.Dash;

            m_drawBitmap        = null;
            m_originalBitmap    = null;
            m_backColor         = backColor;

            m_eventFlag         = false;

            m_placeTo           = null;
            m_lineTo            = null;

            m_oldPoint          = new Point(0, 0);
            m_nowPoint          = new Point(0, 0);
        }

        /*Получение флага события построения линии*/
        public bool getEventFlag()
        {
            return m_eventFlag;
        }

        /*Получение последней редактируемой линии*/
        public LineGraph getBufferLine()
        {
            return m_line;
        }

        /*Установка цвета фона*/
        public void setBackColor(Color backColor)
        {
            m_backColor = backColor;
        }

        /*Этапы построения линии*/
        public Bitmap preparation(LineGraph line, Bitmap originalBitmap)
        {
            //Фиксация события построения линии.
            m_eventFlag         = true;
            m_placeTo           = null;
            m_line              = line;
            line.getPlaceContinueLine().setColorContour(Color.Black);

            //Подготовка слоя для рисования.
            m_drawBitmap        = (Bitmap)originalBitmap.Clone();
            m_originalBitmap    = (Bitmap)originalBitmap.Clone();
            m_graph             = Graphics.FromImage(m_drawBitmap);

            //Отрисовка мест для присоединения линии на объектах.
            for (int i = 0; i < m_aObjectGraph.Count; i++) {
                if (m_aObjectGraph[i].getType() == TYPE_OBJECT.VERTEX) {
                    ((VertexGraph)m_aObjectGraph[i]).drawPlaceInputLine(m_drawBitmap);
                }
            }

            //Фиксирование точек строящегося отрезка.
            m_oldPoint = m_line.getPlaceContinueLine().getPointForLine();
            m_nowPoint = m_oldPoint;

            //Возвращение подготовленного слоя.
            return m_drawBitmap;
        }
        public Bitmap construction(Point cursor)
        {
            // старой версии строящегося отрезка.
            deleteLine(m_oldPoint, m_nowPoint);

            //Если линия горизонтальная.
            if (Math.Abs(m_oldPoint.X - cursor.X) > Math.Abs(m_oldPoint.Y - cursor.Y)) {
                m_nowPoint.X = cursor.X;
                m_nowPoint.Y = m_oldPoint.Y;
            }
            //Если линия вертикальная.
            else {
                m_nowPoint.X = m_oldPoint.X;
                m_nowPoint.Y = cursor.Y;
            }

            //Проверка указания линии на объект.
            if ((m_placeTo == null) && (m_lineTo == null)) {
                for (int i = 0; i < m_aObjectGraph.Count; i++) {
                    //Если объект - линия.
                    if (m_aObjectGraph[i].getType() == TYPE_OBJECT.LINE) {
                        //Если линия не указвыает на саму себя.
                        if (m_aObjectGraph[i].getID() != m_line.getID()) {
                            //Если линия указывает на линию.
                            if (m_aObjectGraph[i].cursorOnArea(m_nowPoint) == true) {
                                m_lineTo = (LineGraph)m_aObjectGraph[i];        //Добавление в буфер.
                                m_lineTo.setColorContour(Color.Red);            //Выделение линии.
                                m_lineTo.drawObject(m_drawBitmap);              //Отрисовка выделения.
                                break;
                            }
                        }
                    }
                    //Если объект - вершина.
                    else if (m_aObjectGraph[i].getType() == TYPE_OBJECT.VERTEX) {
                        //Если линия указвыает на место для присоединения линии.
                        m_placeTo = getPlaceForLineByCursor((VertexGraph)m_aObjectGraph[i], m_nowPoint);
                        if (m_placeTo != null) {
                            m_placeTo.setColorContour(Color.Red);           //Выделение места.
                            m_placeTo.drawObject(m_drawBitmap);             //Отрисовка выделения.
                            break;
                        }
                    }
                }
            }
            //Проверка ухода линии с объекта.
            else {
                //Если линия указвыала на другую линию.
                if (m_lineTo != null) {
                    if (m_lineTo.cursorOnArea(m_nowPoint) == false) {
                        m_lineTo.setColorContour(Color.Black);              //Снятие выделение линии.
                        m_lineTo.drawObject(m_drawBitmap);                  //Отрисовка изменения.
                        m_lineTo = null;                                    //Освобождение буфера.
                    }
                }
                //Если линия указывала на место для линии.
                else {
                    if (m_placeTo.cursorOnArea(m_nowPoint) == false) {
                        m_placeTo.setColorContour(Color.Black);             //Снятие выделение места.
                        m_placeTo.drawObject(m_drawBitmap);                 //Отрисовка изменения.  
                        m_placeTo = null;                                   //Освобождение буера.
                    }
                }
            }

            //Отрисовка измененной линии.
            m_graph.DrawLine(m_pen, m_oldPoint, m_nowPoint);
            return m_drawBitmap;
        }
        public Bitmap finish()
        {
            //Линия указывает на другой объект.
            if ((m_placeTo != null) || (m_lineTo != null)) {
                //Линия соединяет с вершиной.
                if (m_placeTo != null) {
                    Point moveVector = new Point(m_placeTo.getPointForLine().X - m_line.getPointLine(m_line.getCountPointLine() - 1).X, 0);
                    m_line.moveLastPointLine(moveVector);                                               //Выравнивание последней точки.
                    m_line.addToEndPoint(m_placeTo.getPointForLine());                                  //Добавление конечной точки линии.
                    m_line.setObjectTo(m_placeTo.getObject());                                          //Привязка линии к вершине.
                    m_placeTo.setColorContour(Color.Black);                                             //Сброс выделения места для присоединения линии.
                    m_placeTo.setLine(m_line, false);                                                   //Присоединение линии к месту для соединения линии.
                    m_placeTo.setLocked(true);                                                          //Место у объекта для соединения линии занято.
                }
                //Линия соединяет с другой линией.
                else {
                    m_line.addToEndPoint(m_nowPoint);                                                   //Добавление конечной точки линии.
                    m_line.setObjectTo(m_lineTo);                                                       //Привязка линии к линии.    
                    m_lineTo.setColorContour(Color.Black);                                              //Снятие выделения линии.    
                }
                m_line.getPlaceContinueLine().setLocked(true);                                          //Блокировка продолжения линии.                                                                                                                  
            }
            //Линия не указывает на другой объект.
            else {
                int nPointLine = m_line.getCountPointLine();

                //Определение добавления точки или сдвига уже существующей.
                if (nPointLine > 1) {
                    //Последний отрезок линии вертикальный.
                    if (m_line.getPointLine(nPointLine - 1).X == m_line.getPointLine(nPointLine - 2).X) {
                        //Если новый отрезок тоже будет вертикальным.
                        if (m_line.getPointLine(nPointLine - 1).X == m_nowPoint.X) {
                            Point vectorMove = new Point(0, m_nowPoint.Y - m_line.getPointLine(nPointLine - 1).Y);
                            m_line.moveLastPointLine(vectorMove);                                       //Сдвигаем последнюю точку.
                        }
                        //Если новый отрезок будет горизонтальным.
                        else {
                            m_line.addToEndPoint(m_nowPoint);                                           //Добавляем новую точку.
                        }
                    }
                    //Последний отрезок линии горизонтальный.
                    else {
                        //Если новый отрезок тоже будет горизонтальным.
                        if (m_line.getPointLine(nPointLine - 1).Y == m_nowPoint.Y) {
                            Point vectorMove = new Point(m_nowPoint.X - m_line.getPointLine(nPointLine - 1).X, 0);
                            m_line.moveLastPointLine(vectorMove);                                       //Сдвигаем последнюю точку.
                        }
                        //Если новый отрезок будет ввертикальным.
                        else {
                            m_line.addToEndPoint(m_nowPoint);                                           //Добавляем новую точку.
                        }
                    }
                }
                else {
                    m_line.addToEndPoint(m_nowPoint);                                                //Добавляем новую точку.
                }
            }

            //Очистка холста.
            m_graph.Clear(m_backColor);

            //Отрисовыаем все объекты заново, так как он мог их перекрывать.
            for (int i = 0; i < m_aObjectGraph.Count; i++) {
                m_aObjectGraph[i].drawObject(m_drawBitmap);
            }

            m_eventFlag = false;

            return m_drawBitmap;
        }

        /*Удаление сегмента линии*/
        public Bitmap deleteSegmentLine(LineGraph line, Bitmap originalBitmap, Point cursor)
        {
            //Удаление частей линий до курсора.
            for (int i = line.getCountPointLine() - 2; i >= 0; i--) {
                if (line.cursorOnSegmentLine(i, cursor) == true) {
                    line.deleteLastPoint();
                    break;
                }
                else {
                    line.deleteLastPoint();
                }
            }

            //Освобождение места для соединения линии объекта, на который казывала линия.
            if (line.getObjectTo() != null) {
                if (line.getObjectTo().getType() == TYPE_OBJECT.VERTEX) {
                    PlaceForLine place = getPlace((VertexGraph)line.getObjectTo(), line);           //Получение места.
                    place.setLine(null, false);                                                     //Отвязка линии от места для присоединения линии.
                    place.setLocked(false);                                                         //Снятие блокировки места присоединения линии.
                }
                line.setObjectTo(null);                                                             //Отвязка линии от объекта.
                line.getPlaceContinueLine().setLocked(false);                                       //Снятие блокировки места для продолжения линии.
            }

            //Линии, указывающие на удаляемую линию.
            LineGraph tempLine;
            for (int i = 0; i < m_aObjectGraph.Count; i++) {
                if (m_aObjectGraph[i].getType() == TYPE_OBJECT.LINE) {
                    tempLine = (LineGraph)m_aObjectGraph[i];
                    if (tempLine.getObjectTo() != null) {
                        if (tempLine.getObjectTo().getID() == line.getID()) {
                            /*Если последняя точка другой линии не указывает на измененную линию*/
                            if (line.cursorOnArea(tempLine.getPointLine(tempLine.getCountPointLine() - 1)) == false) {
                                tempLine.setObjectTo(null);                                         //Отвязка линии от линии.
                                tempLine.getPlaceContinueLine().setLocked(false);                   //Снятие блокировки места для продолжения линии.
                            }
                        }
                    }
                }
            }

            //Если линия была удалена целиком.
            if (line.getCountPointLine() == 1) {
                deleteFullLine(line);
            }

            //Отрисовка изменения.
            Graphics graph = Graphics.FromImage(originalBitmap);
            graph.Clear(m_backColor);
            for (int i = 0; i < m_aObjectGraph.Count; i++) {
                m_aObjectGraph[i].drawObject(originalBitmap);
            }

            return originalBitmap;
        }

        /////////////////////////////////////Private методы/////////////////////////////////////
        /*Получение места для присоединения линии, если курсор указвыает на его*/
        private PlaceForLine getPlaceForLineByCursor(VertexGraph vertex, Point cursor)
        {
            for (int i = 0; i < vertex.getCountPlaceInputLine(); i++) {
                if (vertex.getPlaceInputLine(i).cursorOnArea(cursor) == true) {
                    return vertex.getPlaceInputLine(i);
                }
            }
            return null;
        }

        /*Замена участка слоя другим слоем*/
        private void deleteLine(Point point1, Point point2)
        {
            if (point1 != point2) {
                //Если вертикальная линия.
                if (point1.X == point2.X) {
                    //Если линия сверху вниз.
                    if (point1.Y < point2.Y) {
                        if (point2.Y > m_originalBitmap.Height) {
                            point2.Y = m_originalBitmap.Height;
                        }
                        m_graph.DrawImage(m_originalBitmap.Clone(new Rectangle(point1.X, point1.Y, 1, point2.Y - point1.Y), System.Drawing.Imaging.PixelFormat.Format64bppArgb), point1.X, point1.Y);
                    }
                    //Если линия снизу вверх.
                    else {
                        if (point2.Y < 0) {
                            point2.Y = 0;
                        }
                        m_graph.DrawImage(m_originalBitmap.Clone(new Rectangle(point2.X, point2.Y, 1, point1.Y - point2.Y), System.Drawing.Imaging.PixelFormat.Format64bppArgb), point2.X, point2.Y);
                    }
                }
                //Если горизонтальная линия.
                else {
                    //Если линия слева направо.
                    if (point1.X < point2.X) {
                        if (point2.X > m_originalBitmap.Width) {
                            point2.X = m_originalBitmap.Width;
                        }
                        m_graph.DrawImage(m_originalBitmap.Clone(new Rectangle(point1.X, point1.Y, point2.X - point1.X, 1), System.Drawing.Imaging.PixelFormat.Format64bppArgb), point1.X, point1.Y);
                    }
                    //Если линия справа налево.
                    else {
                        if (point2.X < 0) {
                            point2.X = 0;
                        }
                        m_graph.DrawImage(m_originalBitmap.Clone(new Rectangle(point2.X, point2.Y, point1.X - point2.X, 1), System.Drawing.Imaging.PixelFormat.Format64bppArgb), point2.X, point2.Y);
                    }
                }
            }
        }

        /*Получение места, через которое вершина связана с линией*/
        private PlaceForLine getPlace(VertexGraph vertex, LineGraph line)
        {
            for (int i = 0; i < vertex.getCountPlaceInputLine(); i++) {
                if (vertex.getPlaceInputLine(i).getLine() != null) {
                    if (vertex.getPlaceInputLine(i).getLine().getID() == line.getID()) {
                        return vertex.getPlaceInputLine(i);
                    }
                }
            }
            return null;
        }

        /*Удаление линии целиком*/
        private void deleteFullLine(LineGraph line)
        {
            //Получение вершины, из которой выведена линия.
            VertexGraph objectGraph = (VertexGraph)line.getObjectFrom();

            //Отвязка линии от места.
            for (int i = 0; i < objectGraph.getCountPlaceOutputLine(); i++) {
                PlaceForLine outputPlace = objectGraph.getPlaceOutputLine(i);
                if (outputPlace.getLine() != null) {
                    if (outputPlace.getLine().getID() == line.getID()) {
                        outputPlace.setLine(null, false);                   //Отвязка линии от места для вывода линии.
                        outputPlace.setLocked(false);                       //Снятие блокировки с места вывода линии.
                        break;
                    }
                }
            }

            //Удаление линии из списка объектов.
            m_aObjectGraph.Remove(line);
        }

    }

}
