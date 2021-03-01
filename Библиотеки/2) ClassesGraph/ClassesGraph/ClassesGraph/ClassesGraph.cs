using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using System.Windows.Forms;

namespace ClassesGraph
{
    public struct TYPE_OBJECT
    {
        public const int DEFAULT            = 0;
        public const int TEXT               = 1;
        public const int PLACE_FOR_LINE     = 2;
        public const int LINE               = 3;
        public const int VERTEX             = 4;
    }

    public class ObjectGraph
    {
        /////////////////////////////////////Параметры объекта/////////////////////////////////////
        /*Идентификаторы объекта*/
        protected int       m_ID;               //Идентификатор объекта.
        protected int       m_type;             //Тип объекта.        

        /*Параметры области объекта*/
        protected Point     m_pointArea;        //Точка левого верхнего угла области, в которую входит объект.  
        protected Size      m_sizeArea;         //Размер области, в которую входит объект.  

        /*Контур объекта*/
        protected bool      m_visibleContour;   //Видимость контура.
        protected Point[]   m_aPointContour;    //Точки контура.
        protected int       m_widthPenContour;  //Толщина пера рисования контура.
        protected int       m_typeLineContour;  //Тип линии контура.
        protected Color     m_colorContour;     //Цвет контура.

        /*Заливка объекта*/
        protected bool      m_visibleBrush;     //Видимость заливки.   
        protected Color     m_colorBrush;       //Цвет заливки области внутри контура. 



        /////////////////////////////////////Наследуемые методы объекта/////////////////////////////////////
        /*Конструктор*/
        public ObjectGraph(int id, int type, Point[] aPointContour)
        {
            m_ID                = id;
            m_type              = type;

            m_pointArea         = new Point(0, 0);
            m_sizeArea          = new Size(0, 0);

            m_visibleContour    = true;
            m_aPointContour     = aPointContour;
            m_widthPenContour   = 2;
            m_typeLineContour   = 0;
            m_colorContour      = Color.Black;

            m_visibleBrush      = true;
            m_colorBrush        = Color.White;

            //Определение области, в которую входит объект.
            if (aPointContour.Length > 0) {
                Point maxPoint = new Point(0, 0);
                m_pointArea = aPointContour[0];
                for (int i = 0; i < aPointContour.Length; i++) {
                    if (maxPoint.X < m_aPointContour[i].X) {
                        maxPoint.X = m_aPointContour[i].X;
                    }
                    if (maxPoint.Y < m_aPointContour[i].Y) {
                        maxPoint.Y = m_aPointContour[i].Y;
                    }
                    if (m_aPointContour[i].X < m_pointArea.X) {
                        m_pointArea.X = m_aPointContour[i].X;
                    }
                    if (m_aPointContour[i].Y < m_pointArea.Y) {
                        m_pointArea.Y = m_aPointContour[i].Y;
                    }
                }
                m_sizeArea.Width = maxPoint.X - m_pointArea.X;
                m_sizeArea.Height = maxPoint.Y - m_pointArea.Y;
            }
        }

        /*Получение идентификаторов объекта*/
        public int getID()
        {
            return m_ID;
        }
        public int getType()
        {
            return m_type;
        }

        /*Получение характеристик области, в которой входит объект*/
        public Point getPointArea()
        {
            return m_pointArea;
        }
        public Size getSizeArea()
        {
            return m_sizeArea;
        }

        /*Получение точек контура*/
        public int getCountPointContour()
        {
            return m_aPointContour.Length;
        }
        public Point getPointContour(int index)
        {
            if ((index < m_aPointContour.Length) && (index >= 0)) {
                return m_aPointContour[index];
            }
            return new Point(-1, -1);
        }

        /*Сеттеры оформления контура*/
        public void setVisibleContour(bool visible)
        {
            m_visibleContour = visible;
        }
        public void setWidthPenContour(int width)
        {
            if (width > 0) {
                m_widthPenContour = width;
            }
        }
        public void setTypeLineContour(int typeLine)
        {
            m_typeLineContour = typeLine;
        }
        public void setColorContour(Color color)
        {
            m_colorContour = color;
        }

        /*Геттеры оформления контура*/
        public bool getVisibleContour()
        {
            return m_visibleContour;
        }
        public int getWidthPenContour()
        {
            return m_widthPenContour;
        }
        public int getTypeLineContour()
        {
            return m_typeLineContour;
        }
        public Color getColorContour()
        {
            return m_colorContour;
        }

        /*Сеттеры оформления заливки*/
        public void setVisibleBrush(bool visible)
        {
            m_visibleBrush = visible;
        }
        public void setColorBrush(Color color)
        {
            m_colorBrush = color;
        }

        /*Геттеры оформления заливки*/
        public bool getVisibleBrush()
        {
            return m_visibleBrush;
        }
        public Color getColorBrush()
        {
            return m_colorBrush;
        }



        /////////////////////////////////////Переопределяемые методы объекта//////////////////////////////////
        /*Отображение объекта*/
        public virtual void drawObject(Bitmap bmp)
        {
            Graphics graph = Graphics.FromImage(bmp);

            //Отображение заливки.
            if (m_visibleBrush == true) {
                graph.FillPolygon(new SolidBrush(m_colorBrush), m_aPointContour);
            }

            //Отображение контура.
            if (m_visibleContour == true) {
                Pen pen = new Pen(m_colorContour, m_widthPenContour);
                switch (m_typeLineContour) {
                    case 0: pen.DashStyle = System.Drawing.Drawing2D.DashStyle.Solid; break;
                    case 1: pen.DashStyle = System.Drawing.Drawing2D.DashStyle.Dash; break;
                    case 2: pen.DashStyle = System.Drawing.Drawing2D.DashStyle.Dot; break;
                    case 3: pen.DashStyle = System.Drawing.Drawing2D.DashStyle.DashDot; break;
                    case 4: pen.DashStyle = System.Drawing.Drawing2D.DashStyle.DashDotDot; break;
                    case 5: pen.DashStyle = System.Drawing.Drawing2D.DashStyle.Custom; break;
                    default: pen.DashStyle = System.Drawing.Drawing2D.DashStyle.Solid; break;
                }
                graph.DrawLines(pen, m_aPointContour);
            }
        }

        /*Перемещение объекта*/
        public virtual void moveObject(Point vectorMove)
        {
            //Сдвиг точек объекта.
            for (int i = 0; i < m_aPointContour.Length; i++) {
                m_aPointContour[i].X = m_aPointContour[i].X + vectorMove.X;
                m_aPointContour[i].Y = m_aPointContour[i].Y + vectorMove.Y;
            }

            //Сдвиг области.
            m_pointArea.X = m_pointArea.X + vectorMove.X;
            m_pointArea.Y = m_pointArea.Y + vectorMove.Y;
        }

        /*Определение вхождения курсора в область, в которую входит объект*/
        public virtual bool cursorOnArea(Point cursor)
        {
            if ((m_pointArea.X <= cursor.X) && (m_pointArea.X + m_sizeArea.Width >= cursor.X)) {
                if ((m_pointArea.Y <= cursor.Y) && (m_pointArea.Y + m_sizeArea.Height >= cursor.Y)) {
                    return true;
                }
            }
            return false;
        }
    }

    public class TextGraph : ObjectGraph
    {
        /////////////////////////////////////Параметры объекта/////////////////////////////////////
        /*Параметры текста*/
        protected string        m_text;         //Текст.
        protected string        m_fontText;     //Шрифт текста. 
        protected int           m_sizeText;     //Размер шрифта текста.
        protected Color         m_colorText;    //Цвет текста.

        /*Ссылки на объекты*/
        protected ObjectGraph   m_object;       //Текст может быть привязан к другому объекту.



        /////////////////////////////////////Наследуемые методы объекта/////////////////////////////////////
        /*Конструктор*/
        public TextGraph(int id, int type, Point pointText, string text)
            : base(id, type, getPointContourText(text, pointText))
        {
            m_text      = text;
            m_fontText  = "Calibri";
            m_sizeText  = 14;
            m_colorText = Color.Black;
        }

        /////////////////////////////////////Переопределяемые методы объекта//////////////////////////////////
        /*Отображение объекта*/
        public override void drawObject(Bitmap bmp)
        {
            Graphics graph = Graphics.FromImage(bmp);

            //Отрисовка фона с контуром.
            base.drawObject(bmp);

            //Отрисовка текста.
            graph.DrawString(m_text, new Font(m_fontText, m_sizeText), new SolidBrush(m_colorText), m_pointArea);
        }



        /////////////////////////////////////Собственные методы объекта//////////////////////////////////
        /*Геттеры*/
        public string getText()
        {
            return m_text;
        }
        public string getFontText()
        {
            return m_fontText;
        }
        public int getSizeText()
        {
            return m_sizeText;
        }
        public Color getColorText()
        {
            return m_colorText;
        }
        public ObjectGraph getObject()
        {
            return m_object;
        }

        /*Сеттеры*/
        public void setText(string text)
        {
            m_text = text;
            setArea();
        }
        public void setFontText(string fontText)
        {
            m_fontText = fontText;
            setArea();
        }
        public void setSizeText(int sizeText)
        {
            m_sizeText = sizeText;
            setArea();
        }
        public void setColorText(Color colorText)
        {
            m_colorText = colorText;
        }
        public void setObject(ObjectGraph objectGraph)
        {
            m_object = objectGraph;
        }

        /*Определение области текста*/
        private void setArea()
        {
            //Определение размера текста в пикселях.
            m_sizeArea = TextRenderer.MeasureText(m_text, new Font(m_fontText, m_sizeText));

            //Переопределение контура текста.
            m_aPointContour[1].X = m_pointArea.X + m_sizeArea.Width;   //Верхняя правая точка.
            m_aPointContour[1].Y = m_pointArea.Y;
            m_aPointContour[2].X = m_pointArea.X + m_sizeArea.Width;   //Нижняя правая точка.
            m_aPointContour[2].Y = m_pointArea.Y + m_sizeArea.Height;
            m_aPointContour[3].X = m_pointArea.X;                      //Нижняя левая точка.
            m_aPointContour[3].Y = m_pointArea.Y + m_sizeArea.Height;
        }



        /////////////////////////////////////Статические методы объекта//////////////////////////////////
        /*Формирование точек контура текста*/
        private static Point[] getPointContourText(string text, Point pointText)
        {
            Point[] aPointContour = new Point[5];

            //Определение размера текста в пикселях.
            Size sizeArea = TextRenderer.MeasureText(text, new Font("Calibri", 14));

            //Переопределение контура текста.
            aPointContour[0] = pointText;                        //Верхняя левая точка.
            aPointContour[1].X = pointText.X + sizeArea.Width;   //Верхняя правая точка.
            aPointContour[1].Y = pointText.Y;
            aPointContour[2].X = pointText.X + sizeArea.Width;   //Нижняя правая точка.
            aPointContour[2].Y = pointText.Y + sizeArea.Height;
            aPointContour[3].X = pointText.X;                    //Нижняя левая точка.
            aPointContour[3].Y = pointText.Y + sizeArea.Height;
            aPointContour[4] = pointText;

            return aPointContour;
        }
    }

    public class PlaceForLine : ObjectGraph
    {
        /////////////////////////////////////Параметры объекта/////////////////////////////////////
        /*Параметры объекта*/
        protected Point         m_pointForLine; //Точка, откуда начинается или куда присоединяется линия.
        protected bool          m_isOutputLine; //Линия, связанная с данным местом, выходит из него (true) или входит (false)
        protected bool          m_locked;       //Блокировка места.

        /*Ссылки на объекты*/
        protected ObjectGraph   m_objectGraph;  //Ссылка на объект, к которому принадлежит данное место.
        protected LineGraph     m_lineGraph;    //Ссылка линию, связанную с данным местом. 



        /////////////////////////////////////Наследуемые методы объекта/////////////////////////////////////
        /*Конструктор*/
        public PlaceForLine(int id, int type, ObjectGraph objectGraph, Point pointForLine, Size sizePlace)
            : base(id, type, getPointContourPlace(pointForLine, sizePlace))
        {
            m_pointForLine  = pointForLine;
            m_isOutputLine  = true;
            m_locked        = false;

            m_objectGraph   = objectGraph;
            m_lineGraph     = null;
        }



        /////////////////////////////////////Переопределенные методы объекта//////////////////////////////////
        /*Перемещение*/
        public override void moveObject(Point vectorMove)
        {
            //Перемещение объекта.
            base.moveObject(vectorMove);

            //Перемещение точки для начала линии.
            m_pointForLine.X = m_pointForLine.X + vectorMove.X;
            m_pointForLine.Y = m_pointForLine.Y + vectorMove.Y;

            //Перемещение точки линии.
            if (m_lineGraph != null) {
                if (m_isOutputLine == true) {
                    m_lineGraph.moveFirstPointLine(vectorMove);
                }
                else {
                    m_lineGraph.moveLastPointLine(vectorMove);
                }
            }
        }

        /*Отрисовка объекта*/
        public override void drawObject(Bitmap bmp)
        {
            if (m_locked == false) {
                base.drawObject(bmp);
            }
        }

        /*Определение вхождения курсора в область, в которую входит объект*/
        public override bool cursorOnArea(Point cursor)
        {
            if (m_locked == false) {
                return base.cursorOnArea(cursor);
            }
            else {
                return false;
            }
        }



        /////////////////////////////////////Собственные методы объекта//////////////////////////////////
        /*Геттеры*/
        public Point getPointForLine()
        {
            return m_pointForLine;
        }
        public ObjectGraph getObject()
        {
            return m_objectGraph;
        }
        public LineGraph getLine()
        {
            return m_lineGraph;
        }
        public bool getLocked()
        {
            return m_locked;
        }

        /*Сеттеры*/
        public void setLine(LineGraph line, bool isOutputLine)
        {
            m_lineGraph = line;
            m_isOutputLine = isOutputLine;
        }
        public void setLocked(bool locked)
        {
            m_locked = locked;
        }



        /////////////////////////////////////Статические методы объекта//////////////////////////////////
        /*Формирование точек контура места*/
        private static Point[] getPointContourPlace(Point pointForLine, Size sizePlace)
        {
            //Определение верхней левой точки места.
            Point pointArea = new Point(pointForLine.X - sizePlace.Width / 2, pointForLine.Y - sizePlace.Height / 2);

            //Контур места.
            Point[] aPointContour = new Point[5];
            aPointContour[0] = pointArea;                         //Левая верхняя точка.
            aPointContour[1].X = pointArea.X + sizePlace.Width;   //Верхняя правая точка.
            aPointContour[1].Y = pointArea.Y;
            aPointContour[2].X = pointArea.X + sizePlace.Width;   //Нижняя правая точка.
            aPointContour[2].Y = pointArea.Y + sizePlace.Height;
            aPointContour[3].X = pointArea.X;                     //Нижняя леваяточка.
            aPointContour[3].Y = pointArea.Y + sizePlace.Height;
            aPointContour[4] = pointArea;

            return aPointContour;
        }
    }

    public class VertexGraph : ObjectGraph
    {
        /////////////////////////////////////Параметры объекта/////////////////////////////////////
        /*Объекты*/
        protected TextGraph             m_textGraph;                //Текст вершины.
        protected List<PlaceForLine>    m_aPlaceInputLine;          //Места для соединения линии.
        protected List<PlaceForLine>    m_aPlaceOutputLine;         //Места для вывода линии.

        /*Реагирование мест на курсор*/
        protected bool                  m_reactInputPlaceOnCursor;  //При вызове метода cursorOnArea учитывается
        protected bool                  m_reactOutputPlaceOnCursor; //еще и места для линий.



        /////////////////////////////////////Наследуемые методы объекта/////////////////////////////////////
        /*Конструктор*/
        public VertexGraph(int id, int type, Point[] aPointContour, TextGraph textGraph) : base(id, type, aPointContour)
        {
            m_textGraph                 = textGraph;
            m_aPlaceInputLine           = new List<PlaceForLine>();
            m_aPlaceOutputLine          = new List<PlaceForLine>();

            m_reactInputPlaceOnCursor   = false;
            m_reactOutputPlaceOnCursor  = false;
        }



        /////////////////////////////////////Переопределенные методы объекта//////////////////////////////////
        /*Отрисовка объекта*/
        public override void drawObject(Bitmap bmp)
        {
            base.drawObject(bmp);

            if (m_textGraph != null) {
                m_textGraph.drawObject(bmp);
            }
        }

        /*Перемещение*/
        public override void moveObject(Point vectorMove)
        {
            //Сдвиг объекта.
            base.moveObject(vectorMove);

            //Сдвиг текста.
            if (m_textGraph != null) {
                m_textGraph.moveObject(vectorMove);
            }

            //Сдвиг мест для линий.
            for (int i = 0; i < m_aPlaceInputLine.Count; i++) {
                m_aPlaceInputLine[i].moveObject(vectorMove);
            }
            for (int i = 0; i < m_aPlaceOutputLine.Count; i++) {
                m_aPlaceOutputLine[i].moveObject(vectorMove);
            }
        }

        /*Определение вхождения курсора в область, в которую входит объект*/
        public override bool cursorOnArea(Point cursor)
        {
            if (base.cursorOnArea(cursor) == true) {
                return true;
            }
            else {
                if (m_reactInputPlaceOnCursor == true) {
                    for (int i = 0; i < m_aPlaceInputLine.Count; i++) {
                        if (m_aPlaceInputLine[i].cursorOnArea(cursor) == true) {
                            return true;
                        }
                    }
                }
                if (m_reactOutputPlaceOnCursor == true) {
                    for (int i = 0; i < m_aPlaceOutputLine.Count; i++) {
                        if (m_aPlaceOutputLine[i].cursorOnArea(cursor) == true) {
                            return true;
                        }
                    }
                }
            }
            return false;
        }



        /////////////////////////////////////Собственные методы объекта//////////////////////////////////
        /*Геттеры мест для линий*/
        public int getCountPlaceInputLine()
        {
            return m_aPlaceInputLine.Count;
        }
        public int getCountPlaceOutputLine()
        {
            return m_aPlaceOutputLine.Count;
        }
        public PlaceForLine getPlaceInputLine(int index)
        {
            if ((index < m_aPlaceInputLine.Count) && (index >= 0)) {
                return m_aPlaceInputLine[index];
            }
            else {
                return null;
            }
        }
        public PlaceForLine getPlaceOutputLine(int index)
        {
            if ((index < m_aPlaceOutputLine.Count) && (index >= 0)) {
                return m_aPlaceOutputLine[index];
            }
            else {
                return null;
            }
        }

        /*Добавление мест для линий*/
        public void addPlaceInputLine(PlaceForLine placeForLine)
        {
            m_aPlaceInputLine.Add(placeForLine);
        }
        public void addPlaceOutputLine(PlaceForLine placeForLine)
        {
            m_aPlaceOutputLine.Add(placeForLine);
        }

        /*Удаление мест для линий*/
        public void deletePlaceInputLine(int index)
        {
            if ((index < m_aPlaceInputLine.Count) && (index >= 0)) {
                m_aPlaceInputLine.RemoveAt(index);
            }
        }
        public void deletePlaceOutputLine(int index)
        {
            if ((index < m_aPlaceOutputLine.Count) && (index >= 0)) {
                m_aPlaceOutputLine.RemoveAt(index);
            }
        }

        /*Отображение мест для линий*/
        public void drawPlaceInputLine(Bitmap bmp)
        {
            for (int i = 0; i < m_aPlaceInputLine.Count; i++) {
                m_aPlaceInputLine[i].drawObject(bmp);
            }
        }
        public void drawPlaceOutputLine(Bitmap bmp)
        {
            for (int i = 0; i < m_aPlaceOutputLine.Count; i++) {
                m_aPlaceOutputLine[i].drawObject(bmp);
            }
        }

        /*Геттеры реакций мест для линий*/
        public bool getReactInputPlaceOnCursor()
        {
            return m_reactInputPlaceOnCursor;
        }
        public bool getReactOutputPlaceOnCursor()
        {
            return m_reactOutputPlaceOnCursor;
        }

        /*Сеттеры реакций мест для линий*/
        public void setReactInputPlaceOnCursor(bool react)
        {
            m_reactInputPlaceOnCursor = react;
        }
        public void setReactOutputPlaceOnCursor(bool react)
        {
            m_reactOutputPlaceOnCursor = react;
        }

        /*Получение текста*/
        public TextGraph getText()
        {
            return m_textGraph;
        }
    }

    public class LineGraph : ObjectGraph
    {
        /////////////////////////////////////Параметры объекта/////////////////////////////////////
        /*Точки линии*/
        protected List<Point>   m_aPointLine;                   //Точки линии.

        /*Объекты, связанные линией*/
        protected ObjectGraph   m_objectFrom;                   //Объект, из которого выводится линия.
        protected ObjectGraph   m_objectTo;                     //Объект, на который указывает линия.

        /*Дополнительные параметры линии*/
        protected int           m_widthArrow;                   //Ширина стрелки.
        protected int           m_lengthArrow;                  //Длина стрелки.
        protected int           m_indentArrow;                  //Расстояние стрелки от конца линии.
        protected PlaceForLine  m_placeContinueLine;            //Место для продолжения линии.
        protected TextGraph     m_textGraph;                    //Текст линии.
        protected int           m_distanceLine;                 //Расстояние от линии, при которой cursorOnArea еще отвечает true.

        /*Реагирование мест на курсор*/
        protected bool          m_reactContinuePlaceOnCursor;   //При вызове метода cursorOnArea учитывается
        protected bool          m_reactTextOnCursor;            //еще место для линий и текст.



        /////////////////////////////////////Наследуемые методы объекта/////////////////////////////////////
        /*Конструктор*/
        public LineGraph(int id, int type, ObjectGraph objectFrom, Point startPoint, TextGraph textGraph, Size sizePlaceContinueLine) : base(id, type, new Point[0])
        {
            m_aPointLine    = new List<Point>();
            m_objectFrom    = objectFrom;
            m_objectTo      = null;
            m_aPointLine.Add(startPoint);

            m_widthArrow        = 14;
            m_lengthArrow       = 16;
            m_indentArrow       = 0;
            m_placeContinueLine = new PlaceForLine(0, TYPE_OBJECT.PLACE_FOR_LINE, this, startPoint, sizePlaceContinueLine);
            m_textGraph         = textGraph;
            m_distanceLine      = 6;

            m_reactContinuePlaceOnCursor    = false;
            m_reactTextOnCursor             = false;
        }



        /////////////////////////////////////Переопределенные методы объекта//////////////////////////////////
        /*Отрисовка объекта*/
        public override void drawObject(Bitmap bmp)
        {
            Graphics graph = Graphics.FromImage(bmp);
            Pen pen = new Pen(m_colorContour, m_widthPenContour);

            switch (m_typeLineContour) {
                case 0: pen.DashStyle = System.Drawing.Drawing2D.DashStyle.Solid; break;
                case 1: pen.DashStyle = System.Drawing.Drawing2D.DashStyle.Dash; break;
                case 2: pen.DashStyle = System.Drawing.Drawing2D.DashStyle.Dot; break;
                case 3: pen.DashStyle = System.Drawing.Drawing2D.DashStyle.DashDot; break;
                case 4: pen.DashStyle = System.Drawing.Drawing2D.DashStyle.DashDotDot; break;
                case 5: pen.DashStyle = System.Drawing.Drawing2D.DashStyle.Custom; break;
                default: pen.DashStyle = System.Drawing.Drawing2D.DashStyle.Solid; break;
            }

            //Отрисовка линий.
            if (m_aPointLine.Count >= 2) {
                for (int i = 0; i < m_aPointLine.Count - 1; i++) {
                    graph.DrawLine(pen, m_aPointLine[i], m_aPointLine[i + 1]);
                }
            }

            //Отрисовка стрелки.
            if (m_objectTo != null) {
                drawArrow(bmp);
            }

            //Отрисовка текста.
            if (m_textGraph != null) {
                m_textGraph.drawObject(bmp);
            }
        }

        /*Определение вхождения курсора в область, в которую входит объект*/
        public override bool cursorOnArea(Point cursor)
        {
            if (m_aPointLine.Count >= 2) {
                for (int i = 0; i < m_aPointLine.Count - 1; i++) {
                    if (cursorOnSegmentLine(i, cursor) == true) {
                        return true;
                    }
                }
            }
            if (m_reactContinuePlaceOnCursor == true) {
                if (m_placeContinueLine.cursorOnArea(cursor) == true) {
                    return true;
                }
            }
            if ((m_reactTextOnCursor == true) && (m_textGraph != null)) {
                if (m_textGraph.cursorOnArea(cursor) == true) {
                    return true;
                }
            }
            return false;
        }



        /////////////////////////////////////Переопределяемые методы объекта//////////////////////////////////
        /*Перемещение точек линии*/
        public virtual void moveFirstPointLine(Point moveVector)
        {
            m_aPointLine[0] = new Point(m_aPointLine[0].X + moveVector.X, m_aPointLine[0].Y + moveVector.Y);

            if (m_aPointLine.Count == 1) {
                m_placeContinueLine.moveObject(moveVector);
            }
        }
        public virtual void moveLastPointLine(Point moveVector)
        {
            m_aPointLine[m_aPointLine.Count - 1] = new Point(m_aPointLine[m_aPointLine.Count - 1].X + moveVector.X, m_aPointLine[m_aPointLine.Count - 1].Y + moveVector.Y);
            m_placeContinueLine.moveObject(moveVector);
        }
        public virtual void movePointLine(int index, Point moveVector)
        {
            if (index < m_aPointLine.Count) {
                if (index == 0) {
                    moveFirstPointLine(moveVector);
                }
                else if (index == m_aPointLine.Count - 1) {
                    moveLastPointLine(moveVector);
                }
                else {
                    m_aPointLine[index] = new Point(m_aPointLine[index].X + moveVector.X, m_aPointLine[index].Y + moveVector.Y);
                }
            }
        }

        /*Добавление и удаление точек линии*/
        public virtual void deleteLastPoint()
        {
            if (m_aPointLine.Count > 1) {
                int last = m_aPointLine.Count - 1;
                int dx = m_aPointLine[last - 1].X - m_aPointLine[last].X;
                int dy = m_aPointLine[last - 1].Y - m_aPointLine[last].Y;
                m_placeContinueLine.moveObject(new Point(dx, dy));
                m_aPointLine.RemoveAt(m_aPointLine.Count - 1);
            }
        }
        public virtual void deletePoint(int index)
        {
            if ((index < m_aPointLine.Count - 1) && (index > 0)) {
                m_aPointLine.RemoveAt(index);
            }
            else if ((index == m_aPointLine.Count - 1) && (index != 0)) {
                deleteLastPoint();
            }
        }
        public virtual void addToEndPoint(Point point)
        {
            Point moveVector = new Point(point.X - m_aPointLine[m_aPointLine.Count - 1].X, point.Y - m_aPointLine[m_aPointLine.Count - 1].Y);
            m_placeContinueLine.moveObject(moveVector);
            m_aPointLine.Add(point);
        }
        public virtual void insertPoint(int index, Point point)
        {
            if ((index < m_aPointLine.Count - 1) && (index > 0)) {
                m_aPointLine.Insert(index, point);
            }
            else if (index >= m_aPointLine.Count - 1) {
                addToEndPoint(point);
            }
        }



        /////////////////////////////////////Собственные методы объекта//////////////////////////////////
        /*Получение объектов связанных линией*/
        public ObjectGraph getObjectFrom()
        {
            return m_objectFrom;
        }
        public ObjectGraph getObjectTo()
        {
            return m_objectTo;
        }
        public TextGraph getText()
        {
            return m_textGraph;
        }
        public PlaceForLine getPlaceContinueLine()
        {
            return m_placeContinueLine;
        }

        /*Получение точек линии*/
        public int getCountPointLine()
        {
            return m_aPointLine.Count;
        }
        public Point getPointLine(int index)
        {
            if ((index < m_aPointLine.Count) && (index >= 0)) {
                return m_aPointLine[index];
            }
            else {
                return new Point(-1, -1);
            }
        }

        /*Получение и установка реакций*/
        public bool getReactContinuePlaceOnCursor()
        {
            return m_reactContinuePlaceOnCursor;
        }
        public bool getReactTextOnCursor()
        {
            return m_reactTextOnCursor;
        }
        public void setReactContiunePlaceOnCursor(bool react)
        {
            m_reactContinuePlaceOnCursor = react;
        }
        public void setReactTextOnCursor(bool react)
        {
            m_reactTextOnCursor = react;
        }

        /*Получение и устанвока дистанции линии*/
        public int getDistanceLine()
        {
            return m_distanceLine;
        }
        public void setDistanceLine(int distance)
        {
            m_distanceLine = distance;
        }

        /*Получение и установка параметров стрелки*/
        public int getWidthArrow()
        {
            return m_widthArrow;
        }
        public int getLengthArrow()
        {
            return m_lengthArrow;
        }
        public int getIndentArrow()
        {
            return m_indentArrow;
        }
        public void setWidthArrow(int width)
        {
            if (width > 0) {
                m_widthArrow = width;
            }
        }
        public void setLengthArrow(int length)
        {
            if (length > 0) {
                m_lengthArrow = length;
            }
        }
        public void setIndentArrow(int indent)
        {
            if (indent >= 0) {
                m_indentArrow = indent;
            }
        }

        /*Привязка конца линии к объекту*/
        public void setObjectTo(ObjectGraph objectTo)
        {
            m_objectTo = objectTo;
        }

        /*Привязка текста к линии*/
        public void setText(TextGraph text)
        {
            m_textGraph = text;
        }

        /*Рисование стрелки*/
        private void drawArrow(Bitmap bmp)
        {
            int XB = m_aPointLine[m_aPointLine.Count - 2].X;  //Координаты начала.
            int YB = m_aPointLine[m_aPointLine.Count - 2].Y;
            int XE = m_aPointLine[m_aPointLine.Count - 1].X;  //Координаты конца.
            int YE = m_aPointLine[m_aPointLine.Count - 1].Y;

            int AX = XE - XB;
            int AY = YE - YB;
            int AD = (int)Math.Sqrt(AX * AX + AY * AY);       // Длина Всей Стрелки.
            int XI = XE - AX * m_indentArrow / AD;
            int YI = YE - AY * m_indentArrow / AD;
            int NX = XI - AX * m_lengthArrow / AD;
            int NY = YI - AY * m_lengthArrow / AD;

            Point[] pointArrow = new Point[4];
            pointArrow[0].X = XI;
            pointArrow[0].Y = YI;
            pointArrow[1].X = NX - AY * m_widthArrow / (AD * 2);
            pointArrow[1].Y = NY + AX * m_widthArrow / (AD * 2);
            pointArrow[2].X = XI - AX * m_lengthArrow / AD;
            pointArrow[2].Y = YI - AY * m_lengthArrow / AD;
            pointArrow[3].X = NX + AY * m_widthArrow / (AD * 2);
            pointArrow[3].Y = NY - AX * m_widthArrow / (AD * 2);

            Graphics graph = Graphics.FromImage(bmp);
            graph.FillPolygon(new SolidBrush(m_colorContour), pointArrow);     //Заливка полигона.

        }

        /*Работа с сегментами линий*/
        public bool cursorOnSegmentLine(int indexSegment, Point cursor)
        {
            if (indexSegment < m_aPointLine.Count - 1) {
                Point point1 = m_aPointLine[indexSegment];
                Point point2 = m_aPointLine[indexSegment + 1];
                //Если линия не вертикальная.
                if (point1.X != point2.X) {
                    //Если линия не горизонтальная.
                    if (point1.Y != point2.Y) {
                        //Если курсор лежит в интервале по X.
                        if (Math.Sign(cursor.X - point1.X) != Math.Sign(cursor.X - point2.X)) {
                            //Если курсор лежит в интервале по Y.
                            if (Math.Sign(cursor.Y - point1.Y) != Math.Sign(cursor.Y - point2.Y)) {
                                int A = point2.Y - point1.Y;                                                        //Ax + By + C = 0
                                int B = point1.X - point2.X;                                                        //Уравнение прямой.
                                int C = point1.Y * (point2.X - point1.X) - point1.X * (point2.Y - point1.Y);
                                double dist = Math.Abs(A * cursor.X + B * cursor.Y + C) / Math.Sqrt(A * A + B * B); //Расстоярие от точки до линии.
                                if (dist < m_distanceLine) {
                                    return true;
                                }
                            }
                        }
                    }
                    //Если линия горизонтальная.
                    else {
                        //Если курсор в пределах линии по Y.
                        if (Math.Abs(point1.Y - cursor.Y) < m_distanceLine) {
                            //Если курсор лежит в интервале по X.
                            if (Math.Sign(cursor.X - point1.X) != Math.Sign(cursor.X - point2.X)) {
                                return true;
                            }
                        }
                    }
                }
                //Если линия вертикальная.
                else {
                    //Если курсор в пределах линии по X.
                    if (Math.Abs(point1.X - cursor.X) < m_distanceLine) {
                        //Если курсор лежит в интервале по Y.
                        if (Math.Sign(cursor.Y - point1.Y) != Math.Sign(cursor.Y - point2.Y)) {
                            return true;
                        }
                    }
                }
            }
            return false;
        }
        public void drawSegmentLine(Bitmap bmp, int indexSegment)
        {
            if (indexSegment < m_aPointLine.Count - 1) {
                Graphics    graph   = Graphics.FromImage(bmp);
                Pen         pen     = new Pen(m_colorContour, m_widthPenContour);
                graph.DrawLine(pen, m_aPointLine[indexSegment], m_aPointLine[indexSegment + 1]);

                //Отрисовка стрелки.
                if ((m_objectTo != null) && (indexSegment == m_aPointLine.Count - 2)) {
                    drawArrow(bmp);
                }
            }
        }

    }

}
