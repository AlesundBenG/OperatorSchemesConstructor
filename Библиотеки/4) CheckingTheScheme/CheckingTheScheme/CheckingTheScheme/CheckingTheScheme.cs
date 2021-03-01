using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using ClassesGraph;
using OperatorAlgorithm;

namespace CheckingTheScheme
{
    /// <summary>
    /// Результат проверки с пояснением.
    /// </summary>
    public struct CheckResult
    {
        public bool     hasError;           //Флаг наличие ошибки.
        public string   explanationError;   //Обоснование ошибки.
    }

    /// <summary>
    /// Класс для разбиения строки на лексемы.
    /// </summary>
    public class Lexer
    {
        /////////////////////////////////////Поля класса/////////////////////////////////////
        private readonly Algorithm  algorithm;          //Алгоритм.
        private readonly string[]   aVertexAlgorithm;   //Вершины алгоритма.



        /////////////////////////////////////Структуры/////////////////////////////////////
        /// <summary>
        /// Типы лексемы.
        /// </summary>
        public enum TYPE_LEXEM
        {
            notDefined,         //Не распознанная лексема.
            specialized,        //Специализированная лексема.
            nameOperator,       //Имя операторной вершины.
            nameCondition       //Имя условной вершины.
        }

        /// <summary>
        /// Распознанный элемент строки.
        /// </summary>
        public struct Lexem
        {
            public string       name;       //Сама лексема.
            public TYPE_LEXEM   type;       //Тип лексемы.
            public int          position;   //Позиция в строке.
        }


        /////////////////////////////////////Public методы/////////////////////////////////////
        /// <summary>
        /// Конструктор.
        /// </summary>
        /// <param name="algorithm">Операторный алгоритм, из которого будут взяты наименования вершин.</param>
        public Lexer(Algorithm algorithm)
        {
            this.algorithm = algorithm;

            //Инициализация массива вершин алгоритма.
            aVertexAlgorithm = new string[algorithm.NumberOperator + algorithm.NumberCondition];

            //Заполненеи массива вершин алгоритма.
            for (int i = 0; i < algorithm.NumberOperator; i++) {
                aVertexAlgorithm[i] = algorithm.getOperatorVertex(i).Name;
            }
            for (int i = 0; i < algorithm.NumberCondition; i++) {
                aVertexAlgorithm[algorithm.NumberOperator + i] = algorithm.getConditionVertex(i).Name;
            }

            //Отсортировать массив вершин по длинне, так как название одной вершины может быть подстрокой другой.
            //Поэтому при распознавании сначала будет проверка вершин с большим именем, а далее по убыванию.
            //Да-да-да, использую пузырек. :)
            string temp;
            for (int i = 0; i < aVertexAlgorithm.Length; i++) {
                for (int j = i + 1; j < aVertexAlgorithm.Length; j++) {
                    if (aVertexAlgorithm[i].Length < aVertexAlgorithm[j].Length) {
                        temp                = aVertexAlgorithm[i];
                        aVertexAlgorithm[i] = aVertexAlgorithm[j];
                        aVertexAlgorithm[j] = temp;
                    }
                }
            }
        }

        /// <summary>
        /// Разбиение строки на лексемы.
        /// </summary>
        /// <param name="logicString">Разбиваемая строка</param>
        /// <param name="typeScheme">Тип схемы (MSA, s1, s2, s3, LSA)</param>
        /// <returns>Список лексем</returns>
        public List<Lexem> getListLexem(string logicString, string typeScheme)
        {
            //Объявление переменных, участвующих в основном цикле.
            List<Lexem> listLexem           = new List<Lexem>();                    //Список лексем.      
            int         iPosition           = 0;                                    //Позиция в строке.
            char[]      specializedSymbol   = getSpecializedSymbol(typeScheme);     //Массив специализированных символов в зависимости от схемы.

            //Основной цикл.
            while (iPosition < logicString.Length) {
                //Если пробел, то продвигаемся дальше.
                if (logicString[iPosition] == ' ') {
                    iPosition++;
                }
                //Если не пробел, то распознаем лексему.
                else {
                    string  nameLexem           = "";
                    bool    isSpecializedSymbol = false;
                    bool    isNameVertex        = false;
                    //Проверка, является ли символ специализированным.
                    for (int i = 0; i < specializedSymbol.Length; i++) {
                        if (logicString[iPosition] == specializedSymbol[i]) {
                            nameLexem           = specializedSymbol[i].ToString();
                            isSpecializedSymbol = true;
                            break;
                        }
                    }
                    //Если не является специализированным, то проверка, является ли набор символов вершиной.
                    if (!isSpecializedSymbol) {
                        for (int i = 0; i < aVertexAlgorithm.Length; i++) {
                            if (iPosition + aVertexAlgorithm[i].Length <= logicString.Length) {
                                if (logicString.Substring(iPosition, aVertexAlgorithm[i].Length) == aVertexAlgorithm[i]) {
                                    nameLexem       = aVertexAlgorithm[i];
                                    isNameVertex    = true;
                                    break;
                                }
                            }
                        }
                    }
                    //Если не является ни специализированным, ни именем вершины.
                    if ((!isSpecializedSymbol) && (!isNameVertex)) {
                        nameLexem = getNotRecognizedLexem(iPosition, logicString, specializedSymbol);
                    }
                    //Добавление лексемы и сдвиг указателя.
                    addToList(nameLexem, isSpecializedSymbol, isNameVertex, iPosition);
                    iPosition += nameLexem.Length;
                }
            }

            //Возврат результата.
            return listLexem;


            /////////////////////////////////////Локальные функции/////////////////////////////////////
            //Добавление лексемы в список.
            void addToList(string nameLexem, bool isSpecialized, bool isNameVertex, int startPosition)
            {
                Lexem lexem = new Lexem() { name = nameLexem, position = startPosition };

                if (isSpecialized) {
                    lexem.type = TYPE_LEXEM.specialized;
                }
                else if (isNameVertex) {
                    if (algorithm.isOperator(nameLexem)) {
                        lexem.type = TYPE_LEXEM.nameOperator;
                    }
                    else {
                        lexem.type = TYPE_LEXEM.nameCondition;
                    }
                }
                else {
                    lexem.type = TYPE_LEXEM.notDefined;
                }

                listLexem.Add(lexem);
            }
            ///////////////////////////////////////////////////////////////////////////////////////////
        }

        /// <summary>
        /// Получение массива специализированных символов.
        /// </summary>
        /// <param name="typeScheme">Тип схемы (MSA, s1, s2, s3, LSA)</param>
        /// <returns>Массив специализированных символов, либо исключение</returns>
        public char[] getSpecializedSymbol(string typeScheme)
        {
            switch (typeScheme) {
                case "MSA":     return new char[4]  { '¬', '∨', '1', '-' };
                case "s1":      return new char[2]  { '¬', '∨' };
                case "s2":      return new char[4]  { '¬', '∨', '(', ')' };
                case "s3":      return new char[15] { '¬', '∨', '↑', '↓', '⁰', '¹', '²', '³', '⁴', '⁵', '⁶', '⁷', '⁸', '⁹', '*' };
                case "LSA":     return new char[16] { '¬', '∨', '↑', '↓', '⁰', '¹', '²', '³', '⁴', '⁵', '⁶', '⁷', '⁸', '⁹', '*', 'ω' };
                default: throw new ArgumentException("Специализированные символы для схемы " + typeScheme + " не определены.");
            }
        }



        /////////////////////////////////////Private методы/////////////////////////////////////
        //Получение лексемы, которая не распознана.
        private string getNotRecognizedLexem(int iPosition, string logicString, char[] specializedSymbol)
        {
            string notRecognizedLexem = "";

            while (iPosition < logicString.Length) {
                //Если является специализированным, то заканчиваем формирование лексемы.
                for (int i = 0; i < specializedSymbol.Length; i++) {
                    if (logicString[iPosition] == specializedSymbol[i]) {
                        return notRecognizedLexem;
                    }
                }
                //Если является вершиной, то заканчиваем формирование лексемы.
                for (int i = 0; i < aVertexAlgorithm.Length; i++) {
                    if (iPosition + aVertexAlgorithm[i].Length <= logicString.Length) {
                        if (logicString.Substring(iPosition, aVertexAlgorithm[i].Length) == aVertexAlgorithm[i]) {
                            return notRecognizedLexem;
                        }
                    }
                }
                //Если не нашли соответствия, то продвигаемся дальше.
                notRecognizedLexem += logicString[iPosition];
                iPosition++;
            }

            //Возвращение результата.
            return notRecognizedLexem;
        }

    }

    /// <summary>
    /// Проверка графической схемы алгоритма.
    /// </summary>
    public class CheckingTheGraph
    {
        /////////////////////////////////////Поля класса/////////////////////////////////////
        private readonly Algorithm  algorithm;      //Алгоритм.
        private readonly string     captionTrue;    //Обозначение перехода при истине.
        private readonly string     captionFalse;   //Обозначение перехода при лжи.



        /////////////////////////////////////Public методы/////////////////////////////////////
        /// <summary>
        /// Конструктор.
        /// </summary>
        /// <param name="algorithm">Операторный алгоритм</param>
        /// <param name="captionTrue">Обозначение перехода при истине из условной вершины</param>
        /// <param name="captionFalse">Обозначение перехода при лжи из условной вершины</param>
        public CheckingTheGraph(Algorithm algorithm, string captionTrue, string captionFalse)
        {
            this.algorithm      = algorithm;
            this.captionTrue    = captionTrue;
            this.captionFalse   = captionFalse;
        }

        /// <summary>
        /// Проверка перехода.
        /// </summary>
        /// <param name="transition">Линия, соединяющая вершины</param>
        /// <returns>Результат проерки с обоснованием ошибки</returns>
        public CheckResult checkTransition(LineGraph transition)
        {
            //Получить вершины, связанные линией.
            ObjectGraph vertexFrom  = getObjectFrom(transition);
            ObjectGraph vertexTo    = getObjectTo(transition);

            //Если один из объектов отсутствует.
            if ((vertexFrom == null) || (vertexTo == null)) {
                return new CheckResult() {
                    hasError            = true,
                    explanationError    =
                    ((vertexFrom == null)   ? "Отсутствует вершина из которой происходит переход" : "")     +
                    ((vertexTo == null)     ? "Отсутствует вершина, в которую происходит переход." : "."),
                };
            }

            //Если один из объектов не является вершиной.
            if ((vertexFrom.getType() != TYPE_OBJECT.VERTEX) || (vertexFrom.getType() != TYPE_OBJECT.VERTEX)) {
                return new CheckResult() {
                    hasError            = true,
                    explanationError    = "Метод проверки перехода работает только с вершинами и с линиями.",
                };
            }

            //Получение текста у вершины.
            TextGraph textFrom  = ((VertexGraph)vertexFrom).getText();
            TextGraph textTo    = ((VertexGraph)vertexTo).getText();

            //Если у одной из вершин отсутствует текст.
            if ((textFrom == null) || (textTo == null)) {
                return new CheckResult() {
                    hasError            = true,
                    explanationError    =
                    ((textFrom == null)     ? "Не определено имя вершины из которой происходит переход" : "") +
                    ((textTo == null)       ? "Не определено имя вершины, в которую происходит переход." : "."),
                };
            }

            //Получение имени вершины.
            string nameVertexFrom   = textFrom.getText();
            string nameVertexTo     = textTo.getText();

            //Проверка перехода из операторной вершины.
            if (algorithm.isOperator(nameVertexFrom)) {
                return checkTransitionFromOperator();
            }
            else if (algorithm.isCondition(nameVertexFrom)) {
                return checkTransitionFromCondition();
            }
            else {
                return new CheckResult() {
                    hasError            = true,
                    explanationError    = "Не определен тип вершины \"" + nameVertexFrom + "\"."
                };
            }


            /////////////////////////////////////Локальные функции/////////////////////////////////////
            //Проверка перехода из операторной вершины.
            CheckResult checkTransitionFromOperator()
            {
                //Получение условия перехода.
                TextGraph   textCondition = transition.getText();
                string      nameCondition = (textCondition == null) ? null : textCondition.getText();

                //У линии не должно быть текста.
                if (nameCondition != null) {
                    return new CheckResult() {
                        hasError            = true,
                        explanationError    = "Присутствует не коррекстное условие перехода из операторной вершины \"" + nameVertexFrom + "\" в вершину \"" + nameVertexTo + "\"."
                    };
                }
                if (nameVertexTo == algorithm.getOperatorVertex(nameVertexFrom).VertexTo) {
                    return new CheckResult() { hasError = false };
                }
                else {
                    return new CheckResult() {
                        hasError            = true,
                        explanationError    = "Не правильно определен переход из вершины \"" + nameVertexFrom + "\" (\"" + nameVertexTo + "\")."
                    };
                }
            }

            //Проверка перехода из условной вершины.
            CheckResult checkTransitionFromCondition()
            {
                //Получение условия перехода.
                TextGraph   textCondition = transition.getText();
                string      nameCondition = (textCondition == null) ? null : textCondition.getText();

                //Проверка наличия текста у линии.
                if (nameCondition == null) {
                    return new CheckResult() {
                        hasError            = true,
                        explanationError    = "Отсутсвтует условие перехода из условной вершины \"" + nameVertexFrom + "\" в вершину \"" + nameVertexTo + "\"."
                    };
                }

                //Если условие задано не верно.
                if ((nameCondition != captionTrue) && (nameCondition != captionFalse)) {
                    return new CheckResult() {
                        hasError = true,
                        explanationError = "Не коррекстное обозначение условия перехода из вершины \"" + nameVertexFrom + "\" в вершину \"" + nameVertexTo + "\" (\"" + nameCondition + "\")."
                    };
                }

                //Если все коррекстно, то проверяем правильность перехода.
                if (nameCondition == captionTrue) {
                    if (nameVertexTo == algorithm.getConditionVertex(nameVertexFrom).VertexTo_IfTrue) {
                        return new CheckResult() { hasError = false };
                    }
                    else {
                        return new CheckResult() {
                            hasError            = true,
                            explanationError    = "Не правильно определен переход по истине из вершины \"" + nameVertexFrom + "\" (\"" + nameVertexTo + "\")."
                        };
                    }
                }
                else {
                    if (nameVertexTo == algorithm.getConditionVertex(nameVertexFrom).VertexTo_IfFalse) {
                        return new CheckResult() { hasError = false };
                    }
                    else {
                        return new CheckResult() {
                            hasError            = true,
                            explanationError    = "Не правильно определен переход по лжи из вершины \"" + nameVertexFrom + "\" (\"" + nameVertexTo + "\").",
                        };
                    }
                }
            }
            ///////////////////////////////////////////////////////////////////////////////////////////
        }

        /// <summary>
        /// Получение объекта, из которого выходит переход.
        /// </summary>
        /// <param name="transition">Линия перехода</param>
        /// <returns>Либо объект, либо null</returns>
        public ObjectGraph getObjectFrom(LineGraph transition)
        {
            return transition.getObjectFrom();
        }

        /// <summary>
        /// Получение объекта, из которого входит переход
        /// </summary>
        /// <param name="transition">Линия перехода</param>
        /// <returns>Либо объект, либо null</returns>
        public ObjectGraph getObjectTo(LineGraph transition)
        {
            List<int> traversedPath = new List<int> { transition.getID() };     //Пройденный путь прямых до объекта.
            return travel(transition, traversedPath);
        }



        /////////////////////////////////////Private методы/////////////////////////////////////
        //Продвижение по линиям графа для нахождения конца пути.
        private ObjectGraph travel(LineGraph transition, List<int> traversedPath)
        {
            //Если линия не указывает на объект.
            if (transition.getObjectTo() == null) {
                return null;
            }

            //Если дошли до конечного объекта.
            if (transition.getObjectTo().getType() != TYPE_OBJECT.LINE) {
                return transition.getObjectTo();
            }

            //Если линия укаызвает на другую линию, то проверяем наличие замкнутого цикла линий.
            for (int i = 0; i < traversedPath.Count; i++) {
                if (traversedPath[i] == transition.getObjectTo().getID()) {
                    return null;
                }
            }

            //Если замкнутого цикла нет, то переходим к линии, на которую указывает другая линия.
            traversedPath.Add(transition.getObjectTo().getID());
            return travel((LineGraph)transition.getObjectTo(), traversedPath);
        }

    }

    /// <summary>
    /// Проверка матричной схемы алгоритма.
    /// </summary>
    public class CheckingTheMatrix
    {
        /////////////////////////////////////Поля класса/////////////////////////////////////
        private readonly Algorithm  algorithm;  //Алгоритм.
        private readonly Lexer      lexer;      //Лексер для распознания лексем в строке.



        /////////////////////////////////////Структуры/////////////////////////////////////
        /// <summary>
        /// Результат конвертации набора лексем в набор переходов.
        /// </summary>
        private struct ResultConvert
        {
            public bool                     hasErrors;          //Флаг наличие ошибки.
            public string                   explanationError;   //Обоснование ошибки.
            public OperatorVertex           fromOperator;       //Переход из операторной вершины    (Оператор, к которой принадлежит строка матрциы)
            public List<ConditionVertex>    fromCondition;      //Набор переохд из условых вершин   (Набор условий, через которые происходит переход в оператор, принадлежащему столбцу матрицы).
        }



        /////////////////////////////////////Public методы/////////////////////////////////////
        /// <summary>
        /// Конструктор.
        /// </summary>
        /// <param name="algorithm">Операторный алгоритм.</param>
        public CheckingTheMatrix(Algorithm algorithm)
        {
            this.algorithm  = algorithm;
            lexer           = new Lexer(algorithm);
        }

        /// <summary>
        /// Проверка правильности заполнения ячейки матрицы.
        /// </summary>
        /// <param name="operatorFrom">Операторная вершина, из которой происходит переход (строка)</param>
        /// <param name="operatorTo">Операторная вершина, в которую происходит переход (столбец)</param>
        /// <param name="valueCell">Значение ячейки матрицы</param>
        /// <returns>Результат проверки</returns>
        public CheckResult checkCorrectMatrixCell(string operatorFrom, string operatorTo, string valueCell)
        {
            //Разбиение строки на лексемы.
            List<Lexer.Lexem> resultSplit = lexer.getListLexem(valueCell, "MSA");

            //Проверка наличия не распознанной лексемы.
            for (int i = 0; i < resultSplit.Count(); i++) {
                if (resultSplit[i].type == Lexer.TYPE_LEXEM.notDefined) {
                    return new CheckResult() {
                        hasError            = true,
                        explanationError    = "Не распознана лексема \"" + resultSplit[i].name + "\"."
                    };
                }
            }

            //Преобразование лексем в набор переходов.
            ResultConvert resultConvert = convertCellToTransition(operatorFrom, operatorTo, resultSplit);
            if (resultConvert.hasErrors) {
                return new CheckResult() {
                    hasError            = true,
                    explanationError    = resultConvert.explanationError
                };
            }

            //Проверка переходов.
            return checkCorrectPartAlgorithm(operatorFrom, operatorTo, resultConvert.fromOperator, resultConvert.fromCondition);
        }



        /////////////////////////////////////Private методы/////////////////////////////////////

        //************************************************************************************************
        // Преобразованеие списка лексем в набор переходов.
        //************************************************************************************************

        //Попытка добавления перехода из операторной вершины.
        private CheckResult tryAddTransitionFromOperator(OperatorVertex operatorVertex, string from, string to)
        {
            //Если вершина назначения еще не определена или определена и совпадает с добавления.
            if ((operatorVertex.VertexTo == "") || (operatorVertex.VertexTo == to)) {
                operatorVertex.VertexTo = to;
                return new CheckResult() { hasError = false };
            }
            else {
                return new CheckResult() {
                    hasError            = true,
                    explanationError    = "Неоднозначно определен переход из вершины \"" + from + "\" (\"" + operatorVertex.VertexTo + " и \"" + to + "\")."
                };
            }
        }

        //Попытка добавления переохда из условной вершины.
        private CheckResult tryAddTransitionFromCondition(List<ConditionVertex> conditionVertex, string from, string to, bool isTrueWay)
        {
            //Поиск вершины from в списке уже добавленных вершин.
            for (int i = 0; i < conditionVertex.Count(); i++) {
                if (conditionVertex[i].Name == from) {
                    if (isTrueWay) {
                        //Если вершина назначения еще не определена или определена и совпадает с добавления.
                        if ((conditionVertex[i].VertexTo_IfTrue == "") || (conditionVertex[i].VertexTo_IfTrue == to)) {
                            conditionVertex[i].VertexTo_IfTrue = to;
                            return new CheckResult() { hasError = false };
                        }
                        //Если вершина назначения уже определена, то она должна совпадать с добавляемой.
                        else {
                            return new CheckResult() {
                                hasError = true,
                                explanationError = "Неоднозначно определен переход по истине из вершины \"" + from + "\" (\"" + conditionVertex[i].VertexTo_IfTrue + " и \"" + to + "\")."
                            };
                        }

                    }
                    else {
                        //Если вершина назначения еще не определена или определена и совпадает с добавления.
                        if ((conditionVertex[i].VertexTo_IfFalse == "") || (conditionVertex[i].VertexTo_IfFalse == to)) {
                            conditionVertex[i].VertexTo_IfFalse = to;
                            return new CheckResult() { hasError = false };
                        }
                        //Если вершина назначения уже определена, то она должна совпадать с добавляемой.
                        else {
                            return new CheckResult() {
                                hasError = true,
                                explanationError = "Неоднозначно определен переход по лжи из вершины \"" + from + "\" (\"" + conditionVertex[i].VertexTo_IfFalse + " и \"" + to + "\")."
                            };
                        }
                    }
                }
            }

            //Если вершина еще не добавлена, то добавляем.
            string vertexToIfTrue   = (isTrueWay) ? to : "";
            string vertexToIfFalse  = (isTrueWay) ? "" : to;
                
            conditionVertex.Add(new ConditionVertex(from, vertexToIfTrue, vertexToIfFalse));
            return new CheckResult() { hasError = false };
        }

        //Преобразование ячейки матрицы в набор переходов.
        private ResultConvert convertCellToTransition(string operatorFrom, string operatorTo, List<Lexer.Lexem> listLexem)
        {
            //Инициализация результата конвертирования.
            ResultConvert result = new ResultConvert() {
                hasErrors           = false,
                explanationError    = "",
                fromOperator        = new OperatorVertex(operatorFrom, ""),
                fromCondition       = new List<ConditionVertex>()
            };

            //Переменные.
            Lexer.Lexem     currentLexem        = new Lexer.Lexem();    //Текущая лексема.
            int             indexLexem          = 0;                    //Индекс лексемы.
            bool            currentIsTrueWay    = true;                 //Логическое условие текущей вершины.
            bool            futureIsTrueWay     = true;                 //Логическое условие следующей вершины.  
            string          currentVertex       = "";                   //Текущая вершина.

            //Флаги
            bool            endLine             = false;                //Флаг конца строки.
            CheckResult     successAdd          = new CheckResult();    //Флаг успешного добавления перехода/переходов.

            //Автомат Мура для преобразования ячейки МСА в набор переходов.
            int condition = 0;                                          //Переход в начальное состояние автомата.
            while (true) {
                switch (condition) {
                    //////////////////////////////////////////////////////////////
                    case 0: {
                        //Операция состояния.
                        lambda0();

                        //Не ожидаемый конец строки.
                        if (endLine) {
                            result.hasErrors        = true;
                            result.explanationError = getExplanationError();
                            return result;
                        }
                        //Лексемы, под которыми есть переходы.
                        else if (currentLexem.name == "-") {
                            condition = 1;
                        }
                        else if (currentLexem.name == "1") {
                            condition = 2;
                        }
                        else if ((currentLexem.type == Lexer.TYPE_LEXEM.nameCondition) || (currentLexem.name == "¬")) {
                            condition = 3;
                        }
                        //Лексемы, под которыми нет переходов.
                        else {
                            result.hasErrors        = true;
                            result.explanationError = getExplanationError();
                            return result;
                        }
                        break;
                    }
                    //////////////////////////////////////////////////////////////
                    case 1: {
                        //Операция состояния.
                        lambda1();

                        //Ожидаемый конец строки.
                        if (endLine) {
                            condition = 10;
                        }
                        //Лексемы, под которыми нет переходов.
                        else {
                            result.hasErrors        = true;
                            result.explanationError = getExplanationError();
                            return result;
                        }
                        break;
                    }
                    //////////////////////////////////////////////////////////////
                    case 2: {
                        //Операция состояния.
                        lambda2();

                        //Ожидаемый конец строки.
                        if (endLine) {
                            condition = 10;
                        }
                        //Лексемы, под которыми нет переходов.
                        else {
                            result.hasErrors        = true;
                            result.explanationError = getExplanationError();
                            return result;
                        }
                        break;
                    }
                    //////////////////////////////////////////////////////////////
                    case 3: {
                        //Операция состояния.
                        lambda3();

                        //Не ожидаемый конец строки.
                        if (endLine) {
                            result.hasErrors        = true;
                            result.explanationError = getExplanationError();
                            return result;
                        }
                        //Лексемы, под которыми есть переходы.
                        else if (currentLexem.name == "¬") {
                            condition = 4;
                        }
                        else if (currentLexem.type == Lexer.TYPE_LEXEM.nameCondition) {
                            condition = 5;
                        }
                        //Лексемы, под которыми нет переходов.
                        else {
                            result.hasErrors        = true;
                            result.explanationError = getExplanationError();
                            return result;
                        }
                        break;
                    }
                    //////////////////////////////////////////////////////////////
                    case 4: {
                        //Операция состояния.
                        lambda4();

                        //Не ожидаемый конец строки.
                        if (endLine) {
                            result.hasErrors        = true;
                            result.explanationError = getExplanationError();
                            return result;
                        }
                        //Лексемы, под которыми есть переходы.
                        else if (currentLexem.type == Lexer.TYPE_LEXEM.nameCondition) {
                            condition = 5;
                        }
                        //Лексемы, под которыми нет переходов.
                        else {
                            result.hasErrors        = true;
                            result.explanationError = getExplanationError();
                            return result;
                        }
                        break;
                    }
                    //////////////////////////////////////////////////////////////
                    case 5: {
                        //Операция состояния.
                        lambda5();

                        //Анализ результата операции.
                        if (!successAdd.hasError) {
                            condition = 6;
                        }
                        else {
                            result.hasErrors        = true;
                            result.explanationError = successAdd.explanationError;
                            return result;
                        }
                        break;
                    }
                    //////////////////////////////////////////////////////////////
                    case 6: {
                        //Операция состояния.
                        lambda6();

                        //Ожидаемый конец строки.
                        if (endLine) {
                            condition = 9;
                        }
                        //Лексемы, под которыми есть переходы.
                        else if (currentLexem.name == "¬") {
                            condition = 4;
                        }
                        else if (currentLexem.type == Lexer.TYPE_LEXEM.nameCondition) {
                            condition = 5;
                        }
                        else if (currentLexem.name == "∨") {
                            condition = 7;
                        }
                        //Лексемы, под которыми нет переходов.
                        else {
                            result.hasErrors        = true;
                            result.explanationError = getExplanationError();
                            return result;
                        }
                        break;
                    }
                    //////////////////////////////////////////////////////////////
                    case 7: {
                        //Операция состояния.
                        lambda7();

                        //Анализ результата операции.
                        if (!successAdd.hasError) {
                            condition = 8;
                        }
                        else {
                            result.hasErrors        = true;
                            result.explanationError = successAdd.explanationError;
                            return result;
                        }
                        break;
                    }
                    //////////////////////////////////////////////////////////////
                    case 8: {
                        //Операция состояния.
                        lambda1();

                        //Безусловный переход.
                        condition = 3;
                        break;
                    }
                    //////////////////////////////////////////////////////////////
                    case 9: {
                        //Операция состояния.
                        lambda7();

                        //Анализ результата операции.
                        if (!successAdd.hasError) {
                            condition = 10;
                        }
                        else {
                            result.hasErrors        = true;
                            result.explanationError = successAdd.explanationError;
                            return result;
                        }
                        break;
                    }
                    //////////////////////////////////////////////////////////////
                    case 10: {
                        return result;
                    }
                    //////////////////////////////////////////////////////////////
                    default: {
                        throw new Exception("Переход в неизвестное состояние при конвертации ячейки матрицы в набор переходов.");
                    }
                    //////////////////////////////////////////////////////////////
                }
            }


            /////////////////////////////////////Локальные функции/////////////////////////////////////
            //Формирование сообщения об ошибке.
            string getExplanationError()
            {
                switch (condition) {
                    //////////////////////////////////////////////////////////////
                    case 0: {
                        if (endLine) {
                            return "Ячейка матрицы пуста.";
                        }
                        else if (currentLexem.name == "∨") {
                            return "Отсутствует левое выражение операции \"ИЛИ\".";
                        }
                        else if (currentLexem.type == Lexer.TYPE_LEXEM.nameOperator) {
                            return "Ячейка матрицы не может содержать операторную вершину \"" + currentLexem.name + "\".";
                        }
                        break;
                    }
                    //////////////////////////////////////////////////////////////
                    case 1: {
                        return "После первого символа \"-\" ожидался конец строки.";
                    }
                    //////////////////////////////////////////////////////////////
                    case 2: {
                        return "После первого символа \"1\" ожидался конец строки.";
                    }
                    //////////////////////////////////////////////////////////////
                    case 3: {
                        if (endLine) {
                            return "Отсутствует правое выражение операции \"ИЛИ\".";
                        }
                        break;
                    }
                    //////////////////////////////////////////////////////////////
                    case 4: {
                        if (endLine) {
                            return "Отсутствует условная вершина после операции \"НЕ\".";
                        }
                        else if (currentLexem.name == "¬") {
                            return "Выражение  \"¬¬\" является избыточным, необходимо его убрать.";
                        }
                        else if (currentLexem.type == Lexer.TYPE_LEXEM.nameOperator) {
                            return "Ячейка матрицы не может содержать операторную вершину \"" + currentLexem.name + "\".";
                        }
                        else {
                            return "Отсутствует условная вершина после операции \"НЕ\".";
                        }
                    }
                    //////////////////////////////////////////////////////////////
                    case 5: {
                        //Пояснение этой ошикби выдается при попытке добавления перехода.
                        break;
                    }
                    //////////////////////////////////////////////////////////////
                    case 6: {
                        if (currentLexem.name == "-") {
                            return "Символ \"-\" не может находиться в строке с другими символами.";
                        }
                        else if (currentLexem.name == "1") {
                            return "Присутствует символ \"1\", который не является частью имени вершины и который не может находиться в строке с другими символами.";
                        }
                        else if (currentLexem.type == Lexer.TYPE_LEXEM.nameOperator) {
                            return "Ячейка матрицы не может содержать операторную вершину \"" + currentLexem.name + "\".";
                        }
                        break;
                    }
                    //////////////////////////////////////////////////////////////
                    case 7: {
                        //Пояснение этой ошикби выдается при попытке добавления перехода.
                        break;
                    }
                    //////////////////////////////////////////////////////////////
                    case 8: {
                        //В данном состоянии не может возникнуть ошибка, так как безусловный переход.
                        break;
                    }
                    //////////////////////////////////////////////////////////////
                    case 9: {
                        //Пояснение этой ошикби выдается при попытке добавления перехода.
                        break;
                    }
                    //////////////////////////////////////////////////////////////
                    case 10: {
                        //В данном сосоянии не может возникнуть ошибка, так как оно является конечным.
                        break;
                    }
                    //////////////////////////////////////////////////////////////
                }
                return "Здесь должно было быть пояснение ошибки, возникшей в состоянии " + condition + " под входной лексемой \"" + currentLexem.name + "\".";
            }

            //Выходные сигналы.
            void lambda0()
            {
                indexLexem = 0;
                if (indexLexem < listLexem.Count()) {
                    currentLexem = listLexem[indexLexem];
                }
                else {
                    endLine = true;
                }
            }
            void lambda1()
            {
                indexLexem++;
                if (indexLexem < listLexem.Count()) {
                    currentLexem = listLexem[indexLexem];
                }
                else {
                    endLine = true;
                }
            }
            void lambda2()
            {
                result.fromOperator = new OperatorVertex(operatorFrom, operatorTo);
                indexLexem++;
                if (indexLexem < listLexem.Count()) {
                    currentLexem = listLexem[indexLexem];
                }
                else {
                    endLine = true;
                }
            }
            void lambda3()
            {
                currentIsTrueWay = true;
                futureIsTrueWay = true;
                currentVertex = operatorFrom;
            }
            void lambda4()
            {
                futureIsTrueWay = false;
                indexLexem++;
                if (indexLexem < listLexem.Count()) {
                    currentLexem = listLexem[indexLexem];
                }
                else {
                    endLine = true;
                }
            }
            void lambda5()
            {
                if (algorithm.isOperator(currentVertex)) {
                    successAdd = tryAddTransitionFromOperator(result.fromOperator, currentVertex, currentLexem.name);
                }
                else {
                    successAdd = tryAddTransitionFromCondition(result.fromCondition, currentVertex, currentLexem.name, currentIsTrueWay);
                }
            }
            void lambda6()
            {
                currentIsTrueWay = futureIsTrueWay;
                futureIsTrueWay = true;
                currentVertex = currentLexem.name;
                indexLexem++;
                if (indexLexem < listLexem.Count()) {
                    currentLexem = listLexem[indexLexem];
                }
                else {
                    endLine = true;
                }
            }
            void lambda7()
            {
                //Здесь обязательно будет currentVertex условной вершиной, так как операторная не пройдет.
                successAdd = tryAddTransitionFromCondition(result.fromCondition, currentVertex, operatorTo, currentIsTrueWay);
            }
            ///////////////////////////////////////////////////////////////////////////////////////////
        }


        //************************************************************************************************
        // Проход по графу.
        //************************************************************************************************

        //Проверка достижимости операторной вершины operatorTo из операторной вершины operatorFrom.
        private bool tryToAchieveToOperator_FromOperator(OperatorVertex operatorVertex, string operatorTo)
        {
            if (algorithm.isOperator(operatorVertex.VertexTo)) {
                return (operatorVertex.VertexTo == operatorTo);
            }
            else {
                return tryToAchieveToOperator_FromCondition(algorithm.getConditionVertex(operatorVertex.VertexTo), operatorTo);
            }
        }

        //Попытка дойти из условной вершины в операторную вершину vertexTo.
        private bool tryToAchieveToOperator_FromCondition(ConditionVertex conditionVertex, string operatorTo)
        {
            //Вершины назначения из текущей условной вершины.
            string vertexToIfTrue   = conditionVertex.VertexTo_IfTrue;
            string vertexToIfFalse  = conditionVertex.VertexTo_IfFalse;

            //Если вершина назначения является вершиной, которую проверяем на достижимость.
            if ((vertexToIfTrue == operatorTo) || (vertexToIfFalse == operatorTo)) {
                return true;
            }
            //Если вершина назначения не является вершиной, которую проверяем на достижимость.
            else {
                //Достижимость вершины operatorTo в ветви по истине.
                if (algorithm.isCondition(vertexToIfTrue)) {
                    if (tryToAchieveToOperator_FromCondition(algorithm.getConditionVertex(vertexToIfTrue), operatorTo)) {
                        return true;
                    }
                }
                //Если по истинной ветве вершина operatorTO не достижима, то проверка ветви по лжи.
                if (algorithm.isOperator(vertexToIfFalse)) {
                    return false;
                }
                else {
                    return tryToAchieveToOperator_FromCondition(algorithm.getConditionVertex(vertexToIfFalse), operatorTo);
                }
            }
        }

        //Проход по вершинам с проверкой переохдов.
        private CheckResult travel(ConditionVertex checkedVertex, string operatorTo, List<ConditionVertex> conditionVertex)
        {
            ConditionVertex originalCondition = algorithm.getConditionVertex(checkedVertex.Name);

            //Если в данной вершине на истинном путе прерывается путь.
            if (checkedVertex.VertexTo_IfTrue == "") {
                //Если путь прерывается на операторной вершине.
                if (algorithm.isOperator(originalCondition.VertexTo_IfTrue)) {
                    //Если путь прерывается на вершине достижения.
                    if (originalCondition.VertexTo_IfTrue == operatorTo) {
                        return new CheckResult() {
                            hasError            = true,
                            explanationError    = "Не все пути перечислины."
                        };
                    }
                }
                //Если путь прерывается на условной вершине.
                else {
                    //Смотрим, можно ли достичь operatorTo из условной вершины, в которую происходит переход в оригинальном алгоритме.
                    if (tryToAchieveToOperator_FromCondition(algorithm.getConditionVertex(originalCondition.VertexTo_IfTrue), operatorTo)) {
                        return new CheckResult() {
                            hasError            = true,
                            explanationError    = "Не все пути перечислины."
                        };
                    }
                }
            }
            //Если на истинном переходе не прерывается путь.
            else {
                if (algorithm.getConditionVertex(checkedVertex.Name).VertexTo_IfTrue == checkedVertex.VertexTo_IfTrue) {
                    if (algorithm.isOperator(checkedVertex.VertexTo_IfTrue)) {
                        if (operatorTo != checkedVertex.VertexTo_IfTrue) {
                            return new CheckResult() {
                                hasError = true,
                                explanationError = "Не правильно определен переход при истине из вершины \"" + checkedVertex.Name + "\" (\"" + checkedVertex.VertexTo_IfTrue + "\")."
                            };
                        }
                    }
                    else {
                        CheckResult result = travel(getCheckedConditionVertex(checkedVertex.VertexTo_IfTrue), operatorTo, conditionVertex);
                        if (result.hasError) {
                            return result;
                        }
                    }
                }
                else {
                    return new CheckResult() {
                        hasError            = true,
                        explanationError    = "Не правильно определен переход при истине из вершины \"" + checkedVertex.Name + "\" (\"" + checkedVertex.VertexTo_IfTrue + "\")."
                    };
                }
            }

            //Если в данной вершине на ложном путе прерывается путь.
            if (checkedVertex.VertexTo_IfFalse == "") {
                //Если путь прерывается на операторной вершине.
                if (algorithm.isOperator(originalCondition.VertexTo_IfFalse)) {
                    if (originalCondition.VertexTo_IfFalse == operatorTo) {
                        return new CheckResult() {
                            hasError            = true,
                            explanationError    = "Не все пути перечислины."
                        };
                    }
                    else {
                        return new CheckResult() { hasError = false };
                    }
                }
                //Если путь прерывается на условной вершине.
                else {
                    //Смотрим, можно ли достичь operatorTo из условной вершины, в которую происходит переход в оригинальном алгоритме.
                    if (tryToAchieveToOperator_FromCondition(algorithm.getConditionVertex(originalCondition.VertexTo_IfFalse), operatorTo)) {
                        return new CheckResult() {
                            hasError = true,
                            explanationError = "Не все пути перечислины."
                        };
                    }
                    else {
                        return new CheckResult() { hasError = false };
                    }
                }
            }
            else {
                if (algorithm.getConditionVertex(checkedVertex.Name).VertexTo_IfFalse == checkedVertex.VertexTo_IfFalse) {
                    if (algorithm.isOperator(checkedVertex.VertexTo_IfFalse)) {
                        if (operatorTo == checkedVertex.VertexTo_IfFalse) {
                            return new CheckResult() { hasError = false };
                        }
                        else {
                            return new CheckResult() {
                                hasError = true,
                                explanationError = "Не правильно определен переход при лжи из вершины \"" + checkedVertex.Name + "\" (\"" + checkedVertex.VertexTo_IfFalse + "\")."
                            };
                        }
                    }
                    else {
                        return travel(getCheckedConditionVertex(checkedVertex.VertexTo_IfFalse), operatorTo, conditionVertex);
                    }
                }
                else {
                    return new CheckResult() {
                        hasError = true,
                        explanationError = "Не правильно определен переход при лжи из вершины \"" + checkedVertex.Name + "\" (\"" + checkedVertex.VertexTo_IfFalse + "\")."
                    };
                }
            }



            /////////////////////////////////////Локальные функции/////////////////////////////////////
            ConditionVertex getCheckedConditionVertex(string nameVertex)
            {
                for (int i = 0; i < conditionVertex.Count; i++) {
                    if (conditionVertex[i].Name == nameVertex) {
                        return conditionVertex[i];
                    }
                }
                throw new Exception("press F");
            } 
            ///////////////////////////////////////////////////////////////////////////////////////////
        }


        //************************************************************************************************
        // Полвеока переходов.
        //************************************************************************************************

        //Проверка коррекстности построеннонной части алгоритма.
        private CheckResult checkCorrectPartAlgorithm(string operatorFrom, string operatorTo, OperatorVertex checkedVertexFrom, List<ConditionVertex> conditionVertex)
        {
            //После конвертации набора лексем в набор переходов может быть три ситуации:
            //1. Если нет перехода из operatorFrom в operatorTo. Тогда в списке operatorVertex и conditionnVertex не будет ни одной вершины.
            //2. Если переход напрямую из operatorFrom в operatorTo. Тогда в списке operatorVertex будет один переход, а список conditionVertex будет пуст.
            //3. Если переход через другие условные вершины, тогда в списке operatorVertex будет один переход из operatorFrom в условную вершину, а в списке conditionVertex будут переходы из условных вершин.
            // Если переход из условной вершины имеет "", значит, идя по тому пути далее, не будет перехода в operatorTo.

            OperatorVertex originalVertexFrom = algorithm.getOperatorVertex(operatorFrom);

            //Ситуация, когда переход напрямую из operatorFrom в operatorTo.
            if (originalVertexFrom.VertexTo == operatorTo) {
                if (checkedVertexFrom.VertexTo == operatorTo) {
                    return new CheckResult() { hasError = false };
                }
                else {
                    if (checkedVertexFrom.VertexTo == "") {
                        return new CheckResult {
                            hasError            = true,
                            explanationError    = "Вершина \"" + operatorTo + "\" достижима из вершины \"" + operatorFrom + "\"."
                        };
                    }
                    else {
                        return new CheckResult {
                            hasError            = true,
                            explanationError    = "Не правильно определен переход из вершины \"" + checkedVertexFrom.Name + "\" (\"" + checkedVertexFrom.VertexTo + "\")."
                        };
                    }
                }
            }

            //Ситуация, когда вершина operatorTo не достижима из операторной вершины operatorFrom.
            if (!tryToAchieveToOperator_FromOperator(originalVertexFrom, operatorTo)) {
                if (checkedVertexFrom.VertexTo == "") {
                    return new CheckResult() { hasError = false };
                }
                else {
                    return new CheckResult {
                        hasError            = true,
                        explanationError    = "Не правильно определен переход из вершины \"" + checkedVertexFrom.Name + "\" (\"" + checkedVertexFrom.VertexTo + "\")."
                    };
                }
            }

            //Ситуация, когда вершина operatorTo достижима через условные вершины.
            if (checkedVertexFrom.VertexTo == originalVertexFrom.VertexTo) {
                for (int i = 0; i < conditionVertex.Count; i++) {
                    if (originalVertexFrom.VertexTo == conditionVertex[i].Name) {
                        return travel(conditionVertex[i], operatorTo, conditionVertex);
                    }
                }
            }
            else {
                if (checkedVertexFrom.VertexTo == "") {
                    return new CheckResult {
                        hasError            = true,
                        explanationError    = "Вершина \"" + operatorTo + "\" достижима из вершины \"" + operatorFrom + "\"."
                    };
                }
                else {
                    return new CheckResult {
                        hasError            = true,
                        explanationError    = "Не правильно определен переход из вершины \"" + checkedVertexFrom.Name + "\" (\"" + checkedVertexFrom.VertexTo + "\")."
                    };
                }
            }

            throw new Exception("В функции checkCorrectPartAlgorithm есть не рассмотренный вариант. ");
        }


    }

    public class CheckingTheLogicScheme
    {
        /////////////////////////////////////Поля класса/////////////////////////////////////
        private readonly Algorithm              algorithm;          //Алгоритм задания.
        private readonly Algorithm              minimizedAlgorithm; //Минимизированный алгоритм для ЛСА.
        private readonly List<ReplacedVertex>   replacedVertex;     //Замененные вершины при минимизации алгоритма.
        private readonly string[]               aVertexAlgorithm;   //Массив вершин в алгоритме.
        private readonly string[]               aNumberUp;          //Цифры верхнего регистра.
        private readonly Lexer                  lexer;              //Лексер для распознания лексем в строке.



        /////////////////////////////////////Структуры/////////////////////////////////////
        /// <summary>
        /// Результат преобразования набора лексем в набор переходов.
        /// </summary>
        private struct ResultConvert
        {
            public bool                     hasErrors;          //Флаг наличие ошибки.
            public string                   explanationError;   //Обоснование ошибки.
            public List<OperatorVertex>     fromOperator;       //Переходы из операторных врешин.
            public List<ConditionVertex>    fromCondition;      //Переходы из условных вершин.

        }

        /// <summary>
        /// Условная вершина, ждущая стрелку вниз.
        /// </summary>
        private struct ConditionWaitDownArrow
        {
            public string   indexArrow;     //Индекс стрелки.
            public string   nameCondition;  //Имя условной вершины.
            public bool     isFalseWay;     //Стрелка вниз принадлежит переходу при лжи.
        }

        /// <summary>
        /// Встреченные стрелки вниз.
        /// </summary>
        private struct MetDownArrow
        {
            public string indexArrow;   //Индекс стрелки.
            public string vertexTo;     //Вершина, на которую указывает стрелка.
        }

        /// <summary>
        /// Замененные вершины при минимизации алгоритма.
        /// </summary>
        private struct ReplacedVertex
        {
            public string original; //Изначальная вершина.
            public string replaced; //Замененная вершина.
        }

        /////////////////////////////////////Public методы/////////////////////////////////////
        /// <summary>
        /// Конструктор.
        /// </summary>
        /// <param name="algorithm">Операторный алгоритм</param>
        public CheckingTheLogicScheme(Algorithm algorithm)
        {
            //Инициализация полей экземпляра.
            this.algorithm          = algorithm;
            this.aNumberUp          = new string[10] { "⁰", "¹", "²", "³", "⁴", "⁵", "⁶", "⁷", "⁸", "⁹" };
            this.aVertexAlgorithm   = new string[algorithm.NumberOperator + algorithm.NumberCondition];
            this.lexer              = new Lexer(algorithm);

            //Заполненеи массива вершин алгоритма.
            for (int i = 0; i < algorithm.NumberOperator; i++) {
                aVertexAlgorithm[i] = algorithm.getOperatorVertex(i).Name;
            }
            for (int i = 0; i < algorithm.NumberCondition; i++) {
                aVertexAlgorithm[algorithm.NumberOperator + i] = algorithm.getConditionVertex(i).Name;
            }

            //Отсортировать массив вершин по длинне, так как название одной вершины может быть подстрокой другой.
            //Поэтому при распознавании сначала будет проверка вершин с большим именем, а далее по убыванию.
            //Да-да-да, использую пузырек. :)
            string temp;
            for (int i = 0; i < aVertexAlgorithm.Length; i++) {
                for (int j = i + 1; j < aVertexAlgorithm.Length; j++) {
                    if (aVertexAlgorithm[i].Length < aVertexAlgorithm[j].Length) {
                        temp = aVertexAlgorithm[i];
                        aVertexAlgorithm[i] = aVertexAlgorithm[j];
                        aVertexAlgorithm[j] = temp;
                    }
                }
            }

            //Минимизация алгоритма для ЛСА (Удаление условных вершин ,которые ведут к одной вершине при истине и лжи)
            List<OperatorVertex>    operatorVertex      = new List<OperatorVertex>();
            List<ConditionVertex>   conditionVertex     = new List<ConditionVertex>();
            replacedVertex                              = new List<ReplacedVertex>();
            for (int i = 0; i < algorithm.NumberOperator; i++) {
                operatorVertex.Add(algorithm.getOperatorVertex(i));
            }
            for (int i = 0; i < algorithm.NumberCondition; i++) {
                conditionVertex.Add(algorithm.getConditionVertex(i));
            }
            int indexVertex = 0;
            while (indexVertex < conditionVertex.Count) {
                ConditionVertex vertexFrom = conditionVertex[indexVertex];
                if (vertexFrom.VertexTo_IfTrue == vertexFrom.VertexTo_IfFalse) {
                    deleteConditionVertex(indexVertex, vertexFrom);
                    indexVertex = 0;
                }
                else {
                    indexVertex++;
                }
            }
            minimizedAlgorithm = new Algorithm(operatorVertex.ToArray(), conditionVertex.ToArray());



            /////////////////////////////////////Локальные функции/////////////////////////////////////
            //Удаление условной вершины из списка вершин.
            void deleteConditionVertex(int index, ConditionVertex vertex)
            {
                replacedVertex.Add(new ReplacedVertex() { original = vertex.Name, replaced = vertex.VertexTo_IfTrue });
                conditionVertex.RemoveAt(index);

                for (int i = 0; i < operatorVertex.Count; i++) {
                    if (operatorVertex[i].VertexTo == vertex.Name) {
                        operatorVertex[i].VertexTo = vertex.VertexTo_IfTrue;
                    }
                }
                for (int i = 0; i < conditionVertex.Count; i++) {
                    if (conditionVertex[i].VertexTo_IfTrue == vertex.Name) {
                        conditionVertex[i].VertexTo_IfTrue = vertex.VertexTo_IfTrue;
                    }
                    if (conditionVertex[i].VertexTo_IfFalse == vertex.VertexTo_IfFalse) {
                        conditionVertex[i].VertexTo_IfFalse = vertex.VertexTo_IfTrue;
                    }
                }
            }
            ///////////////////////////////////////////////////////////////////////////////////////////
        }

        /// <summary>
        /// Проверка линии логического перехода схем S1-S3.
        /// </summary>
        /// <param name="vertexFrom">Операторная вершина, к которой принадлежит переход</param>
        /// <param name="logicString">Переход</param>
        /// <param name="typeScheme">Тип схемы (s1-s3)</param>
        /// <returns>Результат проверки</returns>
        public CheckResult checkLineLogicScheme(string vertexFrom, string logicString, string typeScheme)
        {
            //Разбиение строки на лексемы.
            List<Lexer.Lexem> resultSplit = lexer.getListLexem(logicString, typeScheme);

            //Проверка наличия не распознанной лексемы.
            for (int i = 0; i < resultSplit.Count(); i++) {
                if (resultSplit[i].type == Lexer.TYPE_LEXEM.notDefined) {
                    return new CheckResult() {
                        hasError            = true,
                        explanationError    = "Не распознана лексема \"" + resultSplit[i].name + "\"."
                    };
                }
            }

            //Преобразование лексем в набор переходов.
            ResultConvert resultConvert = convertLogicStringToTransition(vertexFrom, resultSplit, typeScheme);
            if (resultConvert.hasErrors) {
                return new CheckResult() {
                    hasError            = true,
                    explanationError    = resultConvert.explanationError
                };
            }

            //Проверка набора переходов.
            return checkCorrectPartAlgorithm(resultConvert.fromOperator, resultConvert.fromCondition);
        }

        /// <summary>
        /// Проверка ЛСА.
        /// </summary>
        /// <param name="LSA">Строка логической схемы алгоритма</param>
        /// <returns>Результат првоерки</returns>
        public CheckResult checkLSA(string LSA)
        {
            //Разбиение строки на лексемы.
            List<Lexer.Lexem> resultSplit = lexer.getListLexem(LSA, "LSA");

            //Проверка наличия не распознанной лексемы.
            for (int i = 0; i < resultSplit.Count(); i++) {
                if (resultSplit[i].type == Lexer.TYPE_LEXEM.notDefined) {
                    return new CheckResult() {
                        hasError            = true,
                        explanationError    = "Не распознана лексема \"" + resultSplit[i].name + "\"."
                    };
                }
            }

            //Проверка уникальности вершин в строке.
            for (int i = 0; i < resultSplit.Count(); i++) {
                if ((resultSplit[i].type == Lexer.TYPE_LEXEM.nameOperator) || (resultSplit[i].type == Lexer.TYPE_LEXEM.nameCondition)) {
                    for (int j = i + 1; j < resultSplit.Count(); j++) {
                        if (resultSplit[i].name == resultSplit[j].name) {
                            return new CheckResult() {
                                hasError            = true,
                                explanationError    = "ЛСА не минимизирована, так как вершина \"" + resultSplit[i].name + "\" встречается более одного раза."
                            };
                        }
                    }
                }
            }

            //Проверка минимизации алгоритма путем устранения не нужных условных вершин.
            for (int i = 0; i < replacedVertex.Count; i++) {
                for (int j = 0; j < resultSplit.Count; j++) {
                    if (replacedVertex[i].original == resultSplit[j].name) {
                        return new CheckResult() {
                            hasError            = true,
                            explanationError    = "ЛСА не минимизирована, так как присутствуют не нужные условные вершины, которые при истине и лжи ведут к одной и той же вершине."
                        };
                    }
                }
            }

            //Преобразование лексем в набор переходов.
            ResultConvert resultConvert = convertLSAToTransition(resultSplit);
            if (resultConvert.hasErrors) {
                return new CheckResult() {
                    hasError            = true,
                    explanationError    = resultConvert.explanationError
                };
            }

            //Проверка набора переходов.
            return checkCorrectAlgorithm(resultConvert.fromOperator, resultConvert.fromCondition);
        }



        /////////////////////////////////////Private методы/////////////////////////////////////

        //************************************************************************************************
        // Конвертирование списка лексем в набор переходов.
        //************************************************************************************************

        //Попытка добавления перехода из опреаторной вершины.
        private CheckResult tryAddTransitionFromOperator(List<OperatorVertex> transition, string from, string to)
        {
            //Поиск уже добавленного перехода из вершины.
            for (int i = 0; i < transition.Count(); i++) {
                if (transition[i].Name == from) {
                    //Если вершина назначения еще не определена или определена и совпадает с добавления.
                    if ((transition[i].VertexTo == "") || (transition[i].VertexTo == to)) {
                        transition[i].VertexTo = to;
                        return new CheckResult() { hasError = false };
                    }
                    else {
                        return new CheckResult() {
                            hasError            = true,
                            explanationError    = "Неоднозначно определен переход из вершины \"" + from + "\" (\"" + transition[i].VertexTo + " и \"" + to + "\")."
                        };
                    }
                }
            }

            //Если переход еще не добавлен, то добавляем.
            transition.Add(new OperatorVertex(from, to));
            return new CheckResult() { hasError = false };
        }

        //Попытка добавления перехода из условной вершины.
        private CheckResult tryAddTransitionFromCondition(List<ConditionVertex> transition, string from, string to, bool isTrueWay)
        {
            //Поиск уже добавленного перехода из вершины.
            for (int i = 0; i < transition.Count(); i++) {
                if (transition[i].Name == from) {
                    if (isTrueWay) {
                        //Если вершина назначения еще не определена или определена и совпадает с добавления.
                        if ((transition[i].VertexTo_IfTrue == "") || (transition[i].VertexTo_IfTrue == to)) {
                            transition[i].VertexTo_IfTrue = to;
                            return new CheckResult() { hasError = false };
                        }
                        else {
                            return new CheckResult() {
                                hasError            = true,
                                explanationError    = "Неоднозначно определен переход при истине из вершины \"" + from + "\" (\"" + transition[i].VertexTo_IfTrue + " и \"" + to + "\")."
                            };
                        }
                    }
                    else {
                        //Если вершина назначения еще не определена или определена и совпадает с добавления.
                        if ((transition[i].VertexTo_IfFalse == "") || (transition[i].VertexTo_IfFalse == to)) {
                            transition[i].VertexTo_IfFalse = to;
                            return new CheckResult() { hasError = false };
                        }
                        else {
                            return new CheckResult() {
                                hasError = true,
                                explanationError = "Неоднозначно определен переход при лжи из вершины \"" + from + "\" (\"" + transition[i].VertexTo_IfFalse + " и \"" + to + "\")."
                            };
                        }
                    }
                }
            }

            //Если вершина еще не добавлена, то добавляем.
            string vertexToIfTrue   = (isTrueWay) ? to : "";
            string vertexToIfFalse  = (isTrueWay) ? "" : to;

            transition.Add(new ConditionVertex(from, vertexToIfTrue, vertexToIfFalse));
            return new CheckResult() { hasError = false };
        }

        //Преобразование набора лексем в набор переходов.
        private ResultConvert convertLogicStringToTransition(string vertexFrom, List<Lexer.Lexem> listLexem, string typeScheme)
        {
            switch (typeScheme) {
                case "s1":  return convertS1ToTransition(vertexFrom, listLexem);
                case "s2":  return convertS2ToTransition(vertexFrom, listLexem);
                case "s3":  return convertS3ToTransition(vertexFrom, listLexem);
                default: throw new ArgumentException("Преобразование схемы " + typeScheme + " в набор переходов не определено (только s1-s3).");
            }
        }

        //Автомат преобразования S1 в набор переходов.
        private ResultConvert convertS1ToTransition(string vertexFrom, List<Lexer.Lexem> listLexem)
        {
            //Инициализация результата конвертирования.
            ResultConvert result =  new ResultConvert() {
                hasErrors           = false,
                explanationError    = "",
                fromOperator        = new List<OperatorVertex>(),
                fromCondition       = new List<ConditionVertex>()
            };

            //Переменные.
            Lexer.Lexem     currentLexem        = new Lexer.Lexem();    //Текущая лексема.
            int             indexLexem          = 0;                    //Индекс лексемы.
            bool            currentIsTrueWay    = true;                 //Логическое условие текущей вершины.
            bool            futureIsTrueWay     = true;                 //Логическое условие следующей вершины.  
            string          currentVertex       = "";                   //Текущая вершина.

            //Флаги
            bool            endLine             = false;                //Флаг конца строки.
            CheckResult     successAdd          = new CheckResult();    //Флаг успешного добавления.
            bool            equalStart          = false;                //Флаг совпадения стартовой вершины с текущей вершиной.

            //Автомат Мура для преобразования S1 в набор переходов.
            int condition = 0;                                          //Переход в начальное состояние автомата.
            while (true) {
                switch (condition) {
                    //////////////////////////////////////////////////////////////
                    case 0: {
                        //Операция состояния.
                        lambda0();

                        //Не ожидаемый конец строки.
                        if (endLine) {
                            result.hasErrors        = true;
                            result.explanationError = getExplanationError();
                            return result;
                        }
                        //Лексемы, под которыми есть переходы.
                        else if (currentLexem.name == "¬") {
                            condition = 2;
                        }
                        else if (currentLexem.type == Lexer.TYPE_LEXEM.nameCondition) {
                            condition = 3;
                        }
                        else if (currentLexem.type == Lexer.TYPE_LEXEM.nameOperator) {
                            condition = 1;
                        }
                        //Лексемы, под которыми нет переходов.
                        else {
                            result.hasErrors        = true;
                            result.explanationError = getExplanationError();
                            return result;
                        }
                        break;
                    }
                    //////////////////////////////////////////////////////////////
                    case 1: {
                        //Операция состояния.
                        lambda1();

                        //Ожидаемый конец строки.
                        if (endLine) {
                            condition = 13;
                        }
                        //Лексемы, под которыми нет переходов.
                        else {
                            result.hasErrors        = true;
                            result.explanationError = getExplanationError();
                            return result;
                        }
                        break;
                    }
                    //////////////////////////////////////////////////////////////
                    case 2: {
                        //Операция состояния.
                        lambda2();

                        //Не ожидаемый конец строки.
                        if (endLine) {
                            result.hasErrors        = true;
                            result.explanationError = getExplanationError();
                            return result;
                        }
                        //Лексемы, под которыми есть переходы.
                        else if (currentLexem.name == "¬") {
                            condition = 2;
                        }
                        else if (currentLexem.type == Lexer.TYPE_LEXEM.nameCondition) {
                            condition = 3;
                        }
                        //Лексемы, под которыми нет переходов.
                        else {
                            result.hasErrors        = true;
                            result.explanationError = getExplanationError();
                            return result;
                        }
                        break;
                    }
                    //////////////////////////////////////////////////////////////
                    case 3: {
                        //Операция состояния.
                        lambda3();

                        //Безуслонвый переход.
                        condition = 4;
                        break;
                    }
                    //////////////////////////////////////////////////////////////
                    case 4: {
                        //Операция состояния.
                        lambda4();

                        //Не ожидаемый конец строки.
                        if (endLine) {
                            result.hasErrors        = true;
                            result.explanationError = getExplanationError();
                            return result;
                        }
                        //Лексемы, под которыми есть переходы.
                        else if (currentLexem.name == "¬") {
                            condition = 6;
                        }
                        else if (currentLexem.type == Lexer.TYPE_LEXEM.nameCondition) {
                            condition = 5;
                        }
                        else if (currentLexem.type == Lexer.TYPE_LEXEM.nameOperator) {
                            condition = 8;
                        }
                        //Лексемы, под которыми нет переходов.
                        else {
                            result.hasErrors        = true;
                            result.explanationError = getExplanationError();
                            return result;
                        }
                        break;
                    }
                    //////////////////////////////////////////////////////////////
                    case 5: {
                        //Операция состояния.
                        lambda5();

                        //Анализ результата операции.
                        if (!successAdd.hasError) {
                            condition = 7;
                        }
                        else {
                            result.hasErrors        = true;
                            result.explanationError = successAdd.explanationError;
                            return result;
                        }
                        break;
                    }
                    //////////////////////////////////////////////////////////////
                    case 6: {
                        //Операция состояния.
                        lambda6();

                        //Не ожидаемый конец строки.
                        if (endLine) {
                            result.hasErrors        = true;
                            result.explanationError = getExplanationError();
                            return result;
                        }
                        //Лексемы, под которыми есть переходы.
                        else if (currentLexem.name == "¬") {
                            condition = 6;
                        }
                        else if (currentLexem.type == Lexer.TYPE_LEXEM.nameCondition) {
                            condition = 5;
                        }
                        //Лексемы, под которыми нет переходов.
                        else {
                            result.hasErrors        = true;
                            result.explanationError = getExplanationError();
                            return result;
                        }
                        break;
                    }
                    //////////////////////////////////////////////////////////////
                    case 7: {
                        //Операция состояния.
                        lambda7();

                        //Безусловный переход.
                        condition = 4;
                        break;
                    }
                    //////////////////////////////////////////////////////////////
                    case 8: {
                        //Операция состояния.
                        lambda8();

                        //Анализ результата операции.
                        if (!successAdd.hasError) {
                            condition = 9;
                        }
                        else {
                            result.hasErrors        = true;
                            result.explanationError = successAdd.explanationError;
                            return result;
                        }
                        break;
                    }
                    //////////////////////////////////////////////////////////////
                    case 9: {
                        //Операция состояния.
                        lambda9();

                        //Не ожидаемый конец строки.
                        if (endLine) {
                            condition = 13;
                        }
                        else if (currentLexem.name == "∨") {
                            condition = 10;
                        }
                        //Лексемы, под которыми нет переходов.
                        else {
                            result.hasErrors        = true;
                            result.explanationError = getExplanationError();
                            return result;
                        }
                        break;
                    }
                    //////////////////////////////////////////////////////////////
                    case 10: {
                        //Операция состояния.
                        lambda10();

                        //Не ожидаемый конец строки.
                        if (endLine) {
                            result.hasErrors        = true;
                            result.explanationError = getExplanationError();
                            return result;
                        }
                        //Лексемы, под которыми есть переходы.
                        else if (currentLexem.name == "¬") {
                            condition = 11;
                        }
                        else if (currentLexem.type == Lexer.TYPE_LEXEM.nameCondition) {
                            condition = 12;
                        }
                        //Лексемы, под которыми нет переходов.
                        else {
                            result.hasErrors        = true;
                            result.explanationError = getExplanationError();
                            return result;
                        }
                        break;
                    }
                    //////////////////////////////////////////////////////////////
                    case 11: {
                        //Операция состояния.
                        lambda2();

                        //Не ожидаемый конец строки.
                        if (endLine) {
                            result.hasErrors        = true;
                            result.explanationError = getExplanationError();
                            return result;
                        }
                        //Лексемы, под которыми есть переходы.
                        else if (currentLexem.name == "¬") {
                            condition = 11;
                        }
                        else if (currentLexem.type == Lexer.TYPE_LEXEM.nameCondition) {
                            condition = 12;
                        }
                        //Лексемы, под которыми нет переходов.
                        else {
                            result.hasErrors        = true;
                            result.explanationError = getExplanationError();
                            return result;
                        }
                        break;
                    }
                    //////////////////////////////////////////////////////////////
                    case 12: {
                        //Операция состояния.
                        lambda11();

                        //Анализ результата операции.
                        if (equalStart) {
                            condition = 4;
                        }
                        else {
                            result.hasErrors        = true;
                            result.explanationError = getExplanationError();
                            return result;
                        }
                        break;
                    }
                    //////////////////////////////////////////////////////////////
                    case 13: {
                        return result;
                    }
                    //////////////////////////////////////////////////////////////
                    default: {
                        throw new Exception("Переход в неизвестное состояние при конвертации S1 в набор переходов.");
                    }
                    //////////////////////////////////////////////////////////////
                }
            }


            /////////////////////////////////////Локальные функции/////////////////////////////////////
            //Формирование сообщения об ошибке.
            string getExplanationError()
            {
                switch (condition) {
                    //////////////////////////////////////////////////////////////
                    case 0: {
                        if (endLine) {
                            return "Не определен переход из вершины \"" + vertexFrom + "\".";
                        }
                        else if (currentLexem.name == "∨") {
                            return "Отсутствует левое выражение операции \"ИЛИ\"";
                        }
                        break;
                    }
                    //////////////////////////////////////////////////////////////
                    case 1: {
                        return "После операторной вершины \"" + result.fromOperator[0].VertexTo + "\" ожидался конец выражения.";
                    }
                    //////////////////////////////////////////////////////////////
                    case 2: {
                        if (endLine) {
                            return "Отсутствует условная вершина после операции \"НЕ\".";
                        }
                        else if (currentLexem.type == Lexer.TYPE_LEXEM.nameOperator) {
                            return "Операция \"НЕ\" не применима к операторной вершине \"" + currentLexem.name + "\".";
                        }
                        else {
                            return "Отсутствует условная вершина после операции \"НЕ\".";
                        }
                    }
                    //////////////////////////////////////////////////////////////
                    case 3: {
                        //В данном состоянии не может возникнуть ошибка, так как безусловный переход.
                        break;
                    }
                    //////////////////////////////////////////////////////////////
                    case 4: {
                        return "Не определен переход из вершины \"" + currentVertex + "\".";
                    }
                    //////////////////////////////////////////////////////////////
                    case 5: {
                        //Пояснение этой ошикби выдается при попытке добавления перехода.
                        break;
                    }
                    //////////////////////////////////////////////////////////////
                    case 6: {
                        if (endLine) {
                            return "Отсутствует условная вершина после операции \"НЕ\".";
                        }
                        else if (currentLexem.type == Lexer.TYPE_LEXEM.nameOperator) {
                            return "Операция \"НЕ\" не применима к операторной вершине \"" + currentLexem.name + "\".";
                        }
                        else {
                            return "Отсутствует условная вершина после операции \"НЕ\".";
                        }
                    }
                    //////////////////////////////////////////////////////////////
                    case 7: {
                        //В данном состоянии не может возникнуть ошибка, так как безусловный переход.
                        break;
                    }
                    //////////////////////////////////////////////////////////////
                    case 8: {
                        //Пояснение этой ошикби выдается при попытке добавления перехода.
                        break;
                    }
                    //////////////////////////////////////////////////////////////
                    case 9: {
                        return "После операторной вершины + \"" + listLexem[indexLexem + -1].name + "\" ожидался конец выражения или символ \"∨\".";
                    }
                    //////////////////////////////////////////////////////////////
                    case 10: {
                        if (endLine) {
                            return "Отсутствует правое выражение операции \"ИЛИ\".";
                        }
                        else if (currentLexem.name == "∨") {
                            return "Не найдено выражение между двумя операцями \"ИЛИ\".";
                        }
                        else if (currentLexem.type == Lexer.TYPE_LEXEM.nameOperator) {
                            return "Неоднозначно определен переход из вершины \"" + vertexFrom + "\" (\"" + result.fromOperator[0].VertexTo + "\" и \"" + currentLexem.name + "\").";
                        }
                        break;
                    }
                    //////////////////////////////////////////////////////////////
                    case 11: {
                        if (endLine) {
                            return "Отсутствует условная вершина после операции \"НЕ\".";
                        }
                        else if (currentLexem.type == Lexer.TYPE_LEXEM.nameOperator) {
                            return "Операция \"НЕ\" не применима к операторной вершине \"" + currentLexem.name + "\".";
                        }
                        else {
                            return "Отсутствует условная вершина после операции \"НЕ\".";
                        }
                    }
                    //////////////////////////////////////////////////////////////
                    case 12: {
                        return "Неоднозначно определен переход из вершины \"" + vertexFrom + "\" (\"" + result.fromOperator[0].VertexTo + "\" и \"" + currentLexem.name + "\").";
                    }
                    //////////////////////////////////////////////////////////////
                    case 13: {
                        //В данном сосоянии не может возникнуть ошибка, так как оно является конечным.
                        break;
                    }
                    //////////////////////////////////////////////////////////////
                }
                return "Здесь должно было быть пояснение ошибки, возникшей в состоянии " + condition + " под входной лексемой \"" + currentLexem.name + "\".";
            }

            //Выходные сигналы автомата.
            void lambda0()
            {
                currentIsTrueWay    = true;
                futureIsTrueWay     = true;
                indexLexem          = 0;
                if (indexLexem < listLexem.Count()) {
                    currentLexem = listLexem[indexLexem];
                }
                else {
                    endLine = true;
                }
            }
            void lambda1()
            {
                tryAddTransitionFromOperator(result.fromOperator, vertexFrom, currentLexem.name);
                indexLexem++;
                if (indexLexem < listLexem.Count()) {
                    currentLexem = listLexem[indexLexem];
                }
                else {
                    endLine = true;
                }
            }
            void lambda2()
            {
                currentIsTrueWay = false;
                indexLexem++;
                if (indexLexem < listLexem.Count()) {
                    currentLexem = listLexem[indexLexem];
                }
                else {
                    endLine = true;
                }
            }
            void lambda3()
            {
                tryAddTransitionFromOperator(result.fromOperator, vertexFrom, currentLexem.name);
            }
            void lambda4()
            {
                currentVertex = currentLexem.name;
                indexLexem++;
                if (indexLexem < listLexem.Count()) {
                    currentLexem = listLexem[indexLexem];
                }
                else {
                    endLine = true;
                }
            }
            void lambda5()
            {
                successAdd = tryAddTransitionFromCondition(result.fromCondition, currentVertex, currentLexem.name, currentIsTrueWay);
            }
            void lambda6()
            {
                futureIsTrueWay = false;
                indexLexem++;
                if (indexLexem < listLexem.Count()) {
                    currentLexem = listLexem[indexLexem];
                }
                else {
                    endLine = true;
                }
            }
            void lambda7()
            {
                currentIsTrueWay    = futureIsTrueWay;
                futureIsTrueWay     = true;
            }
            void lambda8()
            {
                successAdd = tryAddTransitionFromCondition(result.fromCondition, currentVertex, currentLexem.name, currentIsTrueWay);
            }
            void lambda9()
            {
                currentIsTrueWay    = true;
                futureIsTrueWay     = true;
                indexLexem++;
                if (indexLexem < listLexem.Count()) {
                    currentLexem = listLexem[indexLexem];
                }
                else {
                    endLine = true;
                }
            }
            void lambda10()
            {
                indexLexem++;
                if (indexLexem < listLexem.Count()) {
                    currentLexem = listLexem[indexLexem];
                }
                else {
                    endLine = true;
                }
            }
            void lambda11()
            {
                equalStart = (result.fromOperator[0].VertexTo == currentLexem.name);
            }
            ///////////////////////////////////////////////////////////////////////////////////////////
        }

        //Автомат преобразования S2 в набор переходов.
        private ResultConvert convertS2ToTransition(string vertexFrom, List<Lexer.Lexem> listLexem)
        {
            //Инициализация результата конвертирования.
            ResultConvert result = new ResultConvert() {
                hasErrors           = false,
                explanationError    = "",
                fromOperator        = new List<OperatorVertex>(),
                fromCondition       = new List<ConditionVertex>()
            };

            //Магазинная память.
            Stack<string> stack = new Stack<string>();                  //Магазинная память.

            //Переменные.
            Lexer.Lexem     currentLexem        = new Lexer.Lexem();    //Текущая лексема.
            int             indexLexem          = 0;                    //Индекс лексемы.
            bool            currentIsTrueWay    = true;                 //Логическое условие текущей вершины.
            bool            futureIsTrueWay     = true;                 //Логическое условие следующей вершины.  
            string          currentVertex       = "";                   //Текущая вершина.
            string          bufferStack         = "";                   //Данные, прочитанные из стека.

            //Флаги
            bool            endLine             = false;                //Флаг конца строки.
            CheckResult     successAdd          = new CheckResult();    //Флаг успешного добавления перехода/переходов.
            bool            equalVertex         = false;                //Флаг совпадения первых элементов конъюнкций подмножества.
            bool            differentCondition  = false;                //Флаг того, что первые элемнеты конъюнкций в одном подмножестве равны.

            //Автомат Мура для преобразования S2 в набор переходов.
            int condition = 0;                                          //Переход в начальное состояние автомата.
            while (true) {
                switch (condition) {
                    //////////////////////////////////////////////////////////////
                    case 0: {
                        //Операция состояния.
                        lambda0();

                        //Не ожидаемый конец строки.
                        if (endLine) {
                            result.hasErrors        = true;
                            result.explanationError = getExplanationError();
                            return result;
                        }
                        //Лексемы, под которыми есть переходы.
                        else if (currentLexem.name == "¬") {
                            condition = 2;
                        }
                        else if (currentLexem.type == Lexer.TYPE_LEXEM.nameCondition) {
                            condition = 3;
                        }
                        else if (currentLexem.type == Lexer.TYPE_LEXEM.nameOperator) {
                            condition = 1;
                        }
                        //Лексемы, под которыми нет переходов.
                        else {
                            result.hasErrors        = true;
                            result.explanationError = getExplanationError();
                            return result;
                        }
                        break;
                    }
                    //////////////////////////////////////////////////////////////
                    case 1: {
                        //Операция состояния.
                        lambda1();

                        //Ожидаемый конец строки.
                        if (endLine) {
                            condition = 21;
                        }
                        //Лексемы, под которыми нет переходов.
                        else {
                            result.hasErrors        = true;
                            result.explanationError = getExplanationError();
                            return result;
                        }
                        break;
                    }
                    //////////////////////////////////////////////////////////////
                    case 2: {
                        //Операция состояния.
                        lambda2();

                        //Не ожидаемый конец строки.
                        if (endLine) {
                            result.hasErrors        = true;
                            result.explanationError = getExplanationError();
                            return result;
                        }
                        //Лексемы, под которыми есть переходы.
                        else if (currentLexem.type == Lexer.TYPE_LEXEM.nameCondition) {
                            condition = 3;
                        }
                        //Лексемы, под которыми нет переходов.
                        else {
                            result.hasErrors        = true;
                            result.explanationError = getExplanationError();
                            return result;
                        }
                        break;
                    }
                    //////////////////////////////////////////////////////////////
                    case 3: {
                        //Операция состояния.
                        lambda3();

                        //Безуслонвый переход.
                        condition = 4;
                        break;
                    }
                    //////////////////////////////////////////////////////////////
                    case 4: {
                        //Операция состояния.
                        lambda4();

                        //Безуслонвый переход.
                        condition = 5;
                        break;
                    }
                    //////////////////////////////////////////////////////////////
                    case 5: {
                        //Операция состояния.
                        lambda5();

                        //Проверка наличия лексемы для анализа.
                        if (endLine) {
                            result.hasErrors        = true;
                            result.explanationError = getExplanationError();
                            return result;
                        }
                        //Лексемы, под которыми есть переходы.
                        else if (currentLexem.name == "(") {
                            condition = 6;
                        }
                        else if (currentLexem.type == Lexer.TYPE_LEXEM.nameOperator) {
                            condition = 10;
                        }
                        //Лексемы, под которыми нет переходов.
                        else {
                            result.hasErrors        = true;
                            result.explanationError = getExplanationError();
                            return result;
                        }
                        break;
                    }
                    //////////////////////////////////////////////////////////////
                    case 6: {
                        //Операция состояния.
                        lambda6();

                        //Не ожидаемый конец строки.
                        if (endLine) {
                            result.hasErrors        = true;
                            result.explanationError = getExplanationError();
                            return result;
                        }
                        //Лексемы, под которыми есть переходы.
                        else if (currentLexem.name == "¬") {
                            condition = 7;
                        }
                        else if (currentLexem.type == Lexer.TYPE_LEXEM.nameCondition) {
                            condition = 8;
                        }
                        //Лексемы, под которыми нет переходов.
                        else {
                            result.hasErrors        = true;
                            result.explanationError = getExplanationError();
                            return result;
                        }
                        break;
                    }
                    //////////////////////////////////////////////////////////////
                    case 7: {
                        //Операция состояния.
                        lambda7();

                        //Не ожидаемыйконец строки.
                        if (endLine) {
                            result.hasErrors        = true;
                            result.explanationError = getExplanationError();
                            return result;
                        }
                        //Лексемы, под которыми есть переходы.
                        else if (currentLexem.type == Lexer.TYPE_LEXEM.nameCondition) {
                            condition = 8;
                        }
                        //Лексемы, под которыми нет переходов.
                        else {
                            result.hasErrors        = true;
                            result.explanationError = getExplanationError();
                            return result;
                        }
                        break;
                    }
                    //////////////////////////////////////////////////////////////
                    case 8: {
                        //Операция состояния.
                        lambda8();

                        //Анализ результата операции.
                        if (!successAdd.hasError) {
                            condition = 9;
                        }
                        else {
                            result.hasErrors        = true;
                            result.explanationError = successAdd.explanationError;
                            return result;
                        }
                        break;
                    }
                    //////////////////////////////////////////////////////////////
                    case 9: {
                        //Операция состояния.
                        lambda9();

                        //Безусловный переход.
                        condition = 4;
                        break;
                    }
                    //////////////////////////////////////////////////////////////
                    case 10: {
                        //Операция состояния.
                        lambda8();

                        //Анализ результата операции.
                        if (!successAdd.hasError) {
                            condition = 11;
                        }
                        else {
                            result.hasErrors        = true;
                            result.explanationError = successAdd.explanationError;
                            return result;
                        }
                        break;
                    }
                    //////////////////////////////////////////////////////////////
                    case 11: {
                        //Операция состояния.
                        lambda10();

                        //Проверка наличия прочитанного данного из стека.
                        if (bufferStack == null) {
                            result.hasErrors        = true;
                            result.explanationError = getExplanationError();
                            return result;
                        }
                        //Анализ полученного данного из стека.
                        else if (bufferStack == "first") {
                            condition = 12;
                        }
                        else if (bufferStack == "second") {
                            condition = 18;
                        }
                        else {
                            result.hasErrors        = true;
                            result.explanationError = getExplanationError();
                            return result;
                        }
                        break;
                    }
                    //////////////////////////////////////////////////////////////
                    case 12: {
                        //Операция состояния.
                        lambda13();

                        //Не ожидаемый конец строки.
                        if (endLine) {
                            result.hasErrors        = true;
                            result.explanationError = getExplanationError();
                            return result;
                        }
                        //Лексемы, под которыми есть переходы.
                        else if (currentLexem.name == "∨") {
                            condition = 13;
                        }
                        //Лексемы, под которыми нет переходов.
                        else {
                            result.hasErrors        = true;
                            result.explanationError = getExplanationError();
                            return result;
                        }
                        break;
                    }
                    //////////////////////////////////////////////////////////////
                    case 13: {
                        //Операция состояния.
                        lambda11();

                        //Не ожидаемый конец строки.
                        if (endLine) {
                            result.hasErrors        = true;
                            result.explanationError = getExplanationError();
                            return result;
                        }
                        //Лексемы, под которыми есть переходы.
                        else if (currentLexem.name == "¬") {
                            condition = 14;
                        }
                        else if (currentLexem.type == Lexer.TYPE_LEXEM.nameCondition) {
                            condition = 15;
                        }
                        //Лексемы, под которыми нет переходов.
                        else {
                            result.hasErrors        = true;
                            result.explanationError = getExplanationError();
                            return result;
                        }
                        break;
                    }
                    //////////////////////////////////////////////////////////////
                    case 14: {
                        //Операция состояния.
                        lambda2();

                        //Не ожидаемый конец строки.
                        if (endLine) {
                            result.hasErrors        = true;
                            result.explanationError = getExplanationError();
                            return result;
                        }
                        //Лексемы, под которыми есть переходы.
                        else if (currentLexem.type == Lexer.TYPE_LEXEM.nameCondition) {
                            condition = 15;
                        }
                        //Лексемы, под которыми нет переходов.
                        else {
                            result.hasErrors        = true;
                            result.explanationError = getExplanationError();
                            return result;
                        }
                        break;
                    }
                    //////////////////////////////////////////////////////////////
                    case 15: {
                        //Операция состояния.
                        lambda10();

                        //Проверка наличия прочитанного данного из стека.
                        if (bufferStack == null) {
                            result.hasErrors        = true;
                            result.explanationError = getExplanationError();
                            return result;
                        }
                        //Анализ полученного данного из стека.
                        else if ((bufferStack == "1") && (!currentIsTrueWay)) {
                            condition = 16;
                        }
                        else if ((bufferStack == "0") && (currentIsTrueWay)) {
                            condition = 16;
                        }
                        //Лексемы, под которыми нет переходов.
                        else {
                            result.hasErrors        = true;
                            result.explanationError = getExplanationError();
                            return result;
                        }
                        break;
                    }
                    //////////////////////////////////////////////////////////////
                    case 16: {
                        //Операция состояния.
                        lambda10();

                        //Проверка наличия прочитанного данного из стека.
                        if (bufferStack == null) {
                            result.hasErrors        = true;
                            result.explanationError = getExplanationError();
                            return result;
                        }
                        //Анализ полученного данного из стека.
                        else if (bufferStack == currentLexem.name) {
                            condition = 17;
                        }
                        else {
                            result.hasErrors        = true;
                            result.explanationError = getExplanationError();
                            return result;
                        }
                        break;
                    }
                    //////////////////////////////////////////////////////////////
                    case 17: {
                        //Операция состояния.
                        lambda12();

                        //Безуслонвый переход.
                        condition = 5;
                        break;
                    }
                    //////////////////////////////////////////////////////////////
                    case 18: {
                        //Операция состояния.
                        lambda10();

                        //Проверка наличия прочитанного данного из стека.
                        if (bufferStack == null) {
                            condition = 20;
                        }
                        //Анализ полученного данного из стека.
                        else if (bufferStack == "subset") {
                            condition = 19;
                        }
                        else {
                            result.hasErrors        = true;
                            result.explanationError = getExplanationError();
                            return result;
                        }
                        break;
                    }
                    //////////////////////////////////////////////////////////////
                    case 19: {
                        //Операция состояния.
                        lambda13();

                        //Не ожидаемый конец строки.
                        if (endLine) {
                            result.hasErrors        = true;
                            result.explanationError = getExplanationError();
                            return result;
                        }
                        //Лексемы, под которыми есть переходы.
                        else if (currentLexem.name == ")") {
                            condition = 11;
                        }
                        //Лексемы, под которыми нет переходов.
                        else {
                            result.hasErrors        = true;
                            result.explanationError = getExplanationError();
                            return result;
                        }
                        break;
                    }
                    //////////////////////////////////////////////////////////////
                    case 20: {
                        //Операция состояния.
                        lambda13();

                        //Ожидаемый конец строки.
                        if (endLine) {
                            condition = 21;
                        }
                        //Лексемы, под которыми нет переходов.
                        else {
                            result.hasErrors        = true;
                            result.explanationError = getExplanationError();
                            return result;
                        }
                        break;
                    }
                    //////////////////////////////////////////////////////////////
                    case 21: {
                        return result;
                    }
                    //////////////////////////////////////////////////////////////
                    default: {
                        throw new Exception("Переход в неизвестное состояние при конвертации S2 в набор переходов.");
                    }
                    //////////////////////////////////////////////////////////////
                }
            }

            /////////////////////////////////////Локальные функции/////////////////////////////////////
            //Формирование сообщения об ошибке.
            string getExplanationError()
            {
                switch (condition) {
                    //////////////////////////////////////////////////////////////
                    case 0: {
                        if (endLine) {
                            return "Не определен переход из вершины \"" + vertexFrom + "\".";
                        }
                        else if (currentLexem.name == "∨") {
                            return "Отсутствует левое выражение операции \"ИЛИ\".";
                        }
                        else if (currentLexem.name == "(") {
                            return "Отсутствует условная вершина перед символом начала подмножества \"(\".";
                        }
                        else if (currentLexem.name == ")") {
                            return "Отсутствует символ начала подмножества \"(\".";
                        }
                        break;
                    }
                    //////////////////////////////////////////////////////////////
                    case 1: {
                        return "После операторной вершины \"" + result.fromOperator[0].Name + "\" ожидался конец выражения.";
                    }
                    //////////////////////////////////////////////////////////////
                    case 2: {
                        if (endLine) {
                            return "Отсутствует условная вершина после операции \"НЕ\".";
                        }
                        else if (currentLexem.name == "¬") {
                            return "Выражение  \"¬¬\" является избыточным, необходимо его убрать.";
                        }
                        else if (currentLexem.type == Lexer.TYPE_LEXEM.nameOperator) {
                            return "Операция \"НЕ\" не применима к операторной вершине \"" + currentLexem.name + "\".";
                        }
                        else {
                            return "Отсутствует условная вершина после операции \"НЕ\".";
                        }
                    }
                    //////////////////////////////////////////////////////////////
                    case 3: {
                        //В данном состоянии не может возникнуть ошибка, так как безусловный переход.
                        break;
                    }
                    //////////////////////////////////////////////////////////////
                    case 4: {
                        //В данном состоянии не может возникнуть ошибка, так как безусловный переход.
                        break;
                    }
                    //////////////////////////////////////////////////////////////
                    case 5: {
                        return "После вершины \"" + currentVertex + "\" ожидалось объявление подмножества или операторная вершина.";
                    }
                    //////////////////////////////////////////////////////////////
                    case 6: {
                        return "Ожидалась условная вершина после символа \"(\" в качестве начала подмножества.";
                    }
                    //////////////////////////////////////////////////////////////
                    case 7: {
                        if (endLine) {
                            return "Отсутствует условная вершина после операции \"НЕ\".";
                        }
                        else if (currentLexem.name == "¬") {
                            return "Выражение  \"¬¬\" является избыточным, необходимо его убрать.";
                        }
                        else if (currentLexem.type == Lexer.TYPE_LEXEM.nameOperator) {
                            return "Операция \"НЕ\" не применима к операторной вершине \"" + currentLexem.name + "\".";
                        }
                        else {
                            return "Отсутствует условная вершина после операции \"НЕ\".";
                        }
                    }
                    //////////////////////////////////////////////////////////////
                    case 8: {
                        //Пояснение этой ошикби выдается при попытке добавления перехода.
                        break;
                    }
                    //////////////////////////////////////////////////////////////
                    case 9: {
                        //В данном состоянии не может возникнуть ошибка, так как безусловный переход.
                        break;
                    }
                    //////////////////////////////////////////////////////////////
                    case 10: {
                        //Пояснение этой ошикби выдается при попытке добавления перехода.
                        break;
                    }
                    //////////////////////////////////////////////////////////////
                    case 11: {
                        //По идее тут не может возникнуть ошикби, так как при любых раскладах
                        //должны достать флаг либо first либо second.
                        break;
                    }
                    //////////////////////////////////////////////////////////////
                    case 12: {
                        bufferStack = stack.Pop();  //Достаем условие.
                        bufferStack = stack.Pop();  //Достаем вершину.
                        return "Ожидался символ \"∨\" для указания альтернативного пути из условной вершины \"" + bufferStack + "\"";
                    }
                    //////////////////////////////////////////////////////////////
                    case 13: {
                        bufferStack = stack.Pop();  //Достаем условие.
                        bufferStack = stack.Pop();  //Достаем вершину.
                        return "После символа \"∨\" ожидался альтернативный путь из условной вершины \"" + bufferStack + "\"";
                    }
                    //////////////////////////////////////////////////////////////
                    case 14: {
                        if (endLine) {
                            return "Отсутствует условная вершина после операции \"НЕ\".";
                        }
                        else if (currentLexem.name == "¬") {
                            return "Выражение  \"¬¬\" является избыточным, необходимо его убрать.";
                        }
                        else if (currentLexem.type == Lexer.TYPE_LEXEM.nameOperator) {
                            return "Операция \"НЕ\" не применима к операторной вершине \"" + currentLexem.name + "\".";
                        }
                        else {
                            return "Отсутствует условная вершина после операции \"НЕ\".";
                        }
                    }
                    //////////////////////////////////////////////////////////////
                    case 15: {
                        string conditionWay = (bufferStack == "1") ? "при лжи." : "при истине.";
                        bufferStack = stack.Pop();
                        return "Из вершины \"" + bufferStack + "\" ожидался альтернативный переход " + conditionWay;
                    }
                    //////////////////////////////////////////////////////////////
                    case 16: {
                        return "После символа \"∨\" ожидался альтернативный путь из условной вершины \"" + bufferStack + "\"";
                    }
                    //////////////////////////////////////////////////////////////
                    case 17: {
                        //В данном состоянии не может возникнуть ошибка, так как безусловный переход.
                        break;
                    }
                    //////////////////////////////////////////////////////////////
                    case 18: {
                        //По идее тут не может возникнуть ошибки, так как при любом раскладе
                        //После флага second должен идти либо subset, либо null.
                        break;
                    }
                    //////////////////////////////////////////////////////////////
                    case 19: {
                        return "После второго пути одного из подмножетсва ожидался символ \")\".";
                    }
                    //////////////////////////////////////////////////////////////
                    case 20: {
                        if (currentLexem.name == ")") {
                            return "Присутствует лишняя скобка у второго подмножества вершины \"" + result.fromOperator[0].VertexTo + "\".";
                        }
                        else {
                            return "После второго подмножества вершины \"" + result.fromOperator[0].VertexTo + "\" ожидался конец строки.";
                        }
                    }
                    //////////////////////////////////////////////////////////////
                    case 21: {
                        //В данном сосоянии не может возникнуть ошибка, так как оно является конечным.
                        break;
                    }
                    //////////////////////////////////////////////////////////////
                }
                return "Здесь должно было быть пояснение ошибки, возникшей в состоянии " + condition + " под входной лексемой \"" + currentLexem.name + "\".";
            }

            //Выходные сигналы автомата.
            void lambda0()
            {
                currentIsTrueWay    = true;
                futureIsTrueWay     = true;
                indexLexem          = 0;
                if (indexLexem < listLexem.Count()) {
                    currentLexem = listLexem[indexLexem];
                }
                else {
                    endLine = true;
                }
            }
            void lambda1()
            {
                tryAddTransitionFromOperator(result.fromOperator, vertexFrom, currentLexem.name);
                indexLexem++;
                if (indexLexem < listLexem.Count()) {
                    currentLexem = listLexem[indexLexem];
                }
                else {
                    endLine = true;
                }
            }
            void lambda2()
            {
                currentIsTrueWay = false; ;
                indexLexem++;
                if (indexLexem < listLexem.Count()) {
                    currentLexem = listLexem[indexLexem];
                }
                else {
                    endLine = true;
                }
            }
            void lambda3()
            {
                tryAddTransitionFromOperator(result.fromOperator, vertexFrom, currentLexem.name);
            }
            void lambda4()
            {
                stack.Push(currentLexem.name);
                stack.Push((currentIsTrueWay) ? "1" : "0");
                stack.Push("first");
            }
            void lambda5()
            {
                currentVertex = currentLexem.name;
                indexLexem++;
                if (indexLexem < listLexem.Count()) {
                    currentLexem = listLexem[indexLexem];
                }
                else {
                    endLine = true;
                }
            }
            void lambda6()
            {
                stack.Push("subset");
                indexLexem++;
                if (indexLexem < listLexem.Count()) {
                    currentLexem = listLexem[indexLexem];
                }
                else {
                    endLine = true;
                }
            }
            void lambda7()
            {
                futureIsTrueWay = false;
                indexLexem++;
                if (indexLexem < listLexem.Count()) {
                    currentLexem = listLexem[indexLexem];
                }
                else {
                    endLine = true;
                }
            }
            void lambda8()
            {
                successAdd = tryAddTransitionFromCondition(result.fromCondition, currentVertex, currentLexem.name, currentIsTrueWay);
            }
            void lambda9()
            {
                currentIsTrueWay    = futureIsTrueWay;
                futureIsTrueWay     = true;
            }
            void lambda10()
            {
                bufferStack = (stack.Count > 0) ? stack.Pop() : null;
            }
            void lambda11()
            {
                currentIsTrueWay    = true;
                futureIsTrueWay     = true;
                indexLexem++;
                if (indexLexem < listLexem.Count()) {
                    currentLexem = listLexem[indexLexem];
                }
                else {
                    endLine = true;
                }
            }
            void lambda12()
            {
                stack.Push("second");
            }
            void lambda13()
            {
                indexLexem++;
                if (indexLexem < listLexem.Count()) {
                    currentLexem = listLexem[indexLexem];
                }
                else {
                    endLine = true;
                }
            }
            ///////////////////////////////////////////////////////////////////////////////////////////
        }

        //Автомат преобразования S3 в набор переходов.
        private ResultConvert convertS3ToTransition(string vertexFrom, List<Lexer.Lexem> listLexem)
        {
            //Инициализация результата конвертирования.
            ResultConvert result = new ResultConvert() {
                hasErrors           = false,
                explanationError    = "",
                fromOperator        = new List<OperatorVertex>(),
                fromCondition       = new List<ConditionVertex>()
            };

            //Списки.
            List<ConditionWaitDownArrow>    waitingCondition = new List<ConditionWaitDownArrow>();   //Список ждущих условных вершин соответствующей стрелки вниз.
            List<MetDownArrow>              alreadyMetDownArrow = new List<MetDownArrow>();             //Список уже встреченных стрелок вниз.

            //Переменные.
            Lexer.Lexem currentLexem        = new Lexer.Lexem();    //Текущая лексема.
            int         indexLexem          = 0;                    //Индекс лексемы.
            bool        currentIsTrueWay    = true;                 //Логическое условие текущей вершины.
            bool        futureIsTrueWay     = true;                 //Логическое условие следующей вершины.  
            string      currentVertex       = "";                   //Текущая вершина.
            string      bufferNumberUp      = "";                   //Буфер цифр верхнего регистра.
            string      linkDownArrow       = "";                   //Вершина, на которую ссылается соответствующая срелка вниз.

            //Флаги
            bool        endLine         = false;                    //Флаг конца строки.
            CheckResult successAdd      = new CheckResult();        //Флаг успешного добавления перехода/переходов.
            bool        unicueDownArrow = false;                    //Флаг уникальности стрелки вниз с соответствующим индекосм.
            bool        wasMetDownArrow = false;                    //Флаг того, что стрелка вниз с соответствующим индекосм уже была встречена до этого.

            //Автомат Мура для преобразования S3 в набор переходов.
            int condition = 0;                                      //Переход в начальное состояние автомата.
            while (true) {
                switch (condition) {
                    //////////////////////////////////////////////////////////////
                    case 0: {
                        //Операция состояния.
                        lambda0();

                        //Не ожидаемый конец строки.
                        if (endLine) {
                            result.hasErrors        = true;
                            result.explanationError = getExplanationError();
                            return result;
                        }
                        //Лексемы, под которыми есть переходы.
                        else if (currentLexem.name == "¬") {
                            condition = 2;
                        }
                        else if (currentLexem.type == Lexer.TYPE_LEXEM.nameCondition) {
                            condition = 3;
                        }
                        else if (currentLexem.type == Lexer.TYPE_LEXEM.nameOperator) {
                            condition = 1;
                        }
                        //Лексемы, под которыми нет переходов.
                        else {
                            result.hasErrors        = true;
                            result.explanationError = getExplanationError();
                            return result;
                        }
                        break;
                    }
                    //////////////////////////////////////////////////////////////
                    case 1: {
                        //Операция состояния.
                        lambda1();

                        //Ожидаемый конец строки.
                        if (endLine) {
                            condition = 24;
                        }
                        //Лексемы, под которыми нет переходов.
                        else {
                            result.hasErrors        = true;
                            result.explanationError = getExplanationError();
                            return result;
                        }
                        break;
                    }
                    //////////////////////////////////////////////////////////////
                    case 2: {
                        //Операция состояния.
                        lambda2();

                        //Не ожидаемый конец строки.
                        if (endLine) {
                            result.hasErrors        = true;
                            result.explanationError = getExplanationError();
                            return result;
                        }
                        //Лексемы, под которыми есть переходы.
                        else if (currentLexem.type == Lexer.TYPE_LEXEM.nameCondition) {
                            condition = 3;
                        }
                        //Лексемы, под которыми нет переходов.
                        else {
                            result.hasErrors        = true;
                            result.explanationError = getExplanationError();
                            return result;
                        }
                        break;
                    }
                    //////////////////////////////////////////////////////////////
                    case 3: {
                        //Операция состояния.
                        lambda3();

                        //Безусловный переход.
                        condition = 4;
                        break;
                    }
                    //////////////////////////////////////////////////////////////
                    case 4: {
                        //Операция состояния.
                        lambda4();

                        //Не ожидаемый конец строки.
                        if (endLine) {
                            result.hasErrors        = true;
                            result.explanationError = getExplanationError();
                            return result;
                        }
                        //Лексемы, под которыми есть переходы.
                        else if (currentLexem.name == "↑") {
                            condition = 5;
                        }
                        //Лексемы, под которыми нет переходов.
                        else {
                            result.hasErrors        = true;
                            result.explanationError = getExplanationError();
                            return result;
                        }
                        break;
                    }
                    //////////////////////////////////////////////////////////////
                    case 5: {
                        //Операция состояния.
                        lambda5();

                        //Не ожидаемый конец строки.
                        if (endLine) {
                            result.hasErrors        = true;
                            result.explanationError = getExplanationError();
                            return result;
                        }
                        //Лексемы, под которыми есть переходы.
                        else if (aNumberUp.Contains(currentLexem.name)) {
                            condition = 6;
                        }
                        //Лексемы, под которыми нет переходов.
                        else {
                            result.hasErrors        = true;
                            result.explanationError = getExplanationError();
                            return result;
                        }
                        break;
                    }
                    //////////////////////////////////////////////////////////////
                    case 6: {
                        //Операция состояния.
                        lambda6();

                        //Не ожидаемый конец строки.
                        if (endLine) {
                            result.hasErrors        = true;
                            result.explanationError = getExplanationError();
                            return result;
                        }
                        //Лексемы, под которыми есть переходы.
                        else if (aNumberUp.Contains(currentLexem.name)) {
                            condition = 6;
                        }
                        else {
                            condition = 7;
                        }
                        break;
                    }
                    //////////////////////////////////////////////////////////////
                    case 7: {
                        //Операция состояния.
                        lambda7();

                        //Анализ результата операции.
                        if (wasMetDownArrow) {
                            condition = 8;
                        }
                        else {
                            condition = 9;
                        }
                        break;
                    }
                    //////////////////////////////////////////////////////////////
                    case 8: {
                        //Операция состояния.
                        lambda8();

                        //Анализ результата операции.
                        if (!successAdd.hasError) {
                            condition = 10;
                        }
                        else {
                            result.hasErrors        = true;
                            result.explanationError = successAdd.explanationError;
                            return result;
                        }
                        break;
                    }
                    //////////////////////////////////////////////////////////////
                    case 9: {
                        //Операция состояния.
                        lambda9();

                        //Безусловный переход.
                        condition = 10;
                        break;
                    }
                    //////////////////////////////////////////////////////////////
                    case 10: {
                        //Не ожидаемый конец строки.
                        if (endLine) {
                            result.hasErrors        = true;
                            result.explanationError = getExplanationError();
                            return result;
                        }
                        //Лексемы, под которыми есть переходы.
                        else if (currentLexem.name == "¬") {
                            condition = 11;
                        }
                        else if (currentLexem.type == Lexer.TYPE_LEXEM.nameCondition) {
                            condition = 12;
                        }
                        else if (currentLexem.type == Lexer.TYPE_LEXEM.nameOperator) {
                            condition = 14;
                        }
                        //Лексемы, под которыми нет переходов.
                        else {
                            result.hasErrors        = true;
                            result.explanationError = getExplanationError();
                            return result;
                        }
                        break;
                    }
                    //////////////////////////////////////////////////////////////
                    case 11: {
                        //Операция состояния.
                        lambda10();

                        //Не ожидаемый конец строки.
                        if (endLine) {
                            result.hasErrors        = true;
                            result.explanationError = getExplanationError();
                            return result;
                        }
                        //Лексемы, под которыми есть переходы.
                        else if (currentLexem.type == Lexer.TYPE_LEXEM.nameCondition) {
                            condition = 12;
                        }
                        //Лексемы, под которыми нет переходов.
                        else {
                            result.hasErrors        = true;
                            result.explanationError = getExplanationError();
                            return result;
                        }
                        break;
                    }
                    //////////////////////////////////////////////////////////////
                    case 12: {
                        //Операция состояния.
                        lambda11();

                        //Анализ результата опреации.
                        if (!successAdd.hasError) {
                            condition = 13;
                        }
                        else {
                            result.hasErrors        = true;
                            result.explanationError = successAdd.explanationError;
                            return result;
                        }
                        break;
                    }
                    //////////////////////////////////////////////////////////////
                    case 13: {
                        //Операция состояния.
                        lambda12();

                        //Безусловный переход.
                        condition = 4;
                        break;
                    }
                    //////////////////////////////////////////////////////////////
                    case 14: {
                        //Операция состояния.
                        lambda11();

                        //Анализ резульатта операции.
                        if (!successAdd.hasError) {
                            condition = 15;
                        }
                        else {
                            result.hasErrors        = true;
                            result.explanationError = successAdd.explanationError;
                            return result;
                        }
                        break;
                    }
                    //////////////////////////////////////////////////////////////
                    case 15: {
                        //Операция состояния.
                        lambda13();

                        //Ожидаемый конец строки.
                        if (endLine) {
                            condition = 24;
                        }
                        //Лексемы, под которыми есть переходы.
                        else if (currentLexem.name == "*") {
                            condition = 16;
                        }
                        //Лексемы, под которыми нет переходов.
                        else {
                            result.hasErrors        = true;
                            result.explanationError = getExplanationError();
                            return result;
                        }
                        break;
                    }
                    //////////////////////////////////////////////////////////////
                    case 16: {
                        //Операция состояния.
                        lambda13();

                        //Не ожидаемый конец строки.
                        if (endLine) {
                            result.hasErrors        = true;
                            result.explanationError = getExplanationError();
                            return result;
                        }
                        //Лексемы, под которыми есть переходы.
                        else if (currentLexem.name == "↓") {
                            condition = 17;
                        }
                        //Лексемы, под которыми нет переходов.
                        else {
                            result.hasErrors        = true;
                            result.explanationError = getExplanationError();
                            return result;
                        }
                        break;
                    }
                    //////////////////////////////////////////////////////////////
                    case 17: {
                        //Операция состояния.
                        lambda5();

                        //Не ожидаемый конец строки.
                        if (endLine) {
                            result.hasErrors        = true;
                            result.explanationError = getExplanationError();
                            return result;
                        }
                        //Лексемы, под которыми есть переходы.
                        else if (aNumberUp.Contains(currentLexem.name)) {
                            condition = 18;
                        }
                        //Лексемы, под которыми нет переходов.
                        else {
                            result.hasErrors        = true;
                            result.explanationError = getExplanationError();
                            return result;
                        }
                        break;
                    }
                    //////////////////////////////////////////////////////////////
                    case 18: {
                        //Операция состояния.
                        lambda6();

                        //Не ожидаемый конец строки.
                        if (endLine) {
                            result.hasErrors        = true;
                            result.explanationError = getExplanationError();
                            return result;
                        }
                        //Лексемы, под которыми есть переходы.
                        else if (aNumberUp.Contains(currentLexem.name)) {
                            condition = 18;
                        }
                        else {
                            condition = 19;
                        }
                        break;
                    }
                    //////////////////////////////////////////////////////////////
                    case 19: {
                        //Операция состояния.
                        lambda14();

                        //Анализ результата операции.
                        if (unicueDownArrow) {
                            condition = 20;
                        }
                        else {
                            result.hasErrors        = true;
                            result.explanationError = getExplanationError();
                            return result;
                        }
                        break;
                    }
                    //////////////////////////////////////////////////////////////
                    case 20: {
                        //Не ожидаемый конец строки.
                        if (endLine) {
                            result.hasErrors        = true;
                            result.explanationError = getExplanationError();
                            return result;
                        }
                        //Лексемы, под которыми есть переходы.
                        else if (currentLexem.name == "¬") {
                            condition = 21;
                        }
                        else if (currentLexem.type == Lexer.TYPE_LEXEM.nameCondition) {
                            condition = 22;
                        }
                        else if (currentLexem.type == Lexer.TYPE_LEXEM.nameOperator) {
                            condition = 22;
                        }
                        //Лексемы, под которыми нет переходов.
                        else {
                            result.hasErrors        = true;
                            result.explanationError = getExplanationError();
                            return result;
                        }
                        break;
                    }
                    //////////////////////////////////////////////////////////////
                    case 21: {
                        //Операция состояния.
                        lambda10();

                        //Не ожидаемый конец строки.
                        if (endLine) {
                            result.hasErrors        = true;
                            result.explanationError = getExplanationError();
                            return result;
                        }
                        //Лексемы, под которыми есть переходы.
                        else if (currentLexem.type == Lexer.TYPE_LEXEM.nameCondition) {
                            condition = 22;
                        }
                        //Лексемы, под которыми нет переходов.
                        else {
                            result.hasErrors        = true;
                            result.explanationError = getExplanationError();
                            return result;
                        }
                        break;
                    }
                    //////////////////////////////////////////////////////////////
                    case 22: {
                        //Операция состояния.
                        lambda15();

                        condition = 23;
                        break;
                    }
                    //////////////////////////////////////////////////////////////
                    case 23: {
                        //Операция состояния.
                        lambda16();

                        //Анализ резульатат операции.
                        if (!successAdd.hasError) {
                            if (currentLexem.type == Lexer.TYPE_LEXEM.nameCondition) {
                                condition = 13;
                            }
                            else {
                                condition = 15;
                            }
                        }
                        //Лексемы, под которыми нет переходов.
                        else {
                            result.hasErrors        = true;
                            result.explanationError = successAdd.explanationError;
                            return result;
                        }
                        break;
                    }
                    //////////////////////////////////////////////////////////////
                    case 24: {
                        if (waitingCondition.Count != 0) {
                            result.hasErrors        = true;
                            result.explanationError = "Отсутствует \"↓" + waitingCondition[0].indexArrow + "\".";
                        }
                        return result;
                    }
                    //////////////////////////////////////////////////////////////
                    default: {
                        throw new Exception("Переход в неизвестное состояние при конвертации S3 в набор переходов.");
                    }
                    //////////////////////////////////////////////////////////////
                }
            }


            /////////////////////////////////////Локальные функции/////////////////////////////////////
            //Формирование сообщения об ошибке.
            string getExplanationError()
            {
                switch (condition) {
                    //////////////////////////////////////////////////////////////
                    case 0: {
                        if (endLine) {
                            return "Не определен переход из вершины \"" + vertexFrom + "\".";
                        }
                        else if (currentLexem.name == "*") {
                            return "Отсутствует левое выражение у символа \"*\".";
                        }
                        else if (currentLexem.name == "↓") {
                            return "Символ \"↓\" не допустим в начале строки S3 (допустим при минимизации S3), так как он сигнализирует о бесконечном цикле условных вершин.";
                        }
                        else {
                            return "Не определен переход из вершины \"" + vertexFrom + "\".";
                        }
                    }
                    //////////////////////////////////////////////////////////////
                    case 1: {
                        return "После вершины \"" + result.fromOperator[0].VertexTo + "\" ожидался конец выражения.";
                    }
                    //////////////////////////////////////////////////////////////
                    case 2: {
                        if (endLine) {
                            return "Отсутствует условная вершина после операции \"НЕ\".";
                        }
                        else if (currentLexem.name == "¬") {
                            return "Выражение  \"¬¬\" является избыточным, необходимо его убрать.";
                        }
                        else if (currentLexem.type == Lexer.TYPE_LEXEM.nameOperator) {
                            return "Операция \"НЕ\" не применима к операторной вершине \"" + currentLexem.name + "\".";
                        }
                        else {
                            return "Отсутствует условная вершина после операции \"НЕ\".";
                        }
                    }
                    //////////////////////////////////////////////////////////////
                    case 3: {
                        //В данном состоянии не может возникнуть ошибка, так как безусловный переход.
                        break;
                    }
                    //////////////////////////////////////////////////////////////
                    case 4: {
                        if (endLine) {
                            return "Не определен переход из вершины \"" + currentVertex + "\".";
                        }
                        else {
                            return "После условной вершины \"" + currentVertex + "\" отсутствует символ \"↑\".";
                        }
                    }
                    //////////////////////////////////////////////////////////////
                    case 5: {
                        return "Отсутствует идентификатор у \"↑\" после условной вершины \"" + currentVertex + "\".";
                    }
                    //////////////////////////////////////////////////////////////
                    case 6: {
                        //В данном состоянии не может возникнуть ошибка, так как при всех лексемах есть выход.
                        break;
                    }
                    //////////////////////////////////////////////////////////////
                    case 7: {
                        //В данном состоянии не может возникнуть ошибка, та как анализируется результат операции.
                        break;
                    }
                    //////////////////////////////////////////////////////////////
                    case 8: {
                        //Пояснение этой ошикби выдается при попытке добавления перехода.
                        break;
                    }
                    //////////////////////////////////////////////////////////////
                    case 9: {
                        //В данном состоянии не может возникнуть ошибки, так как безусловный переход.
                        break;
                    }
                    //////////////////////////////////////////////////////////////
                    case 10: {
                        if (endLine) {
                            return "Не определен переход из вершины \"" + currentVertex + "\".";
                        }
                        else if (aNumberUp.Contains(currentLexem.name)) {
                            return "Не корректно задан индекс \"↑⁰" + currentLexem.name + "\".";
                        }
                        else if (currentLexem.name == "↓") {
                            return "Сочетание ↑ ↓ можно использовать только при минимизации S3 или построении ЛСА.";
                        }
                        else {
                            return "Не определен переход из вершины \"" + currentVertex + "\".";
                        }
                    }
                    //////////////////////////////////////////////////////////////
                    case 11: {
                        if (endLine) {
                            return "Отсутствует условная вершина после операции \"НЕ\".";
                        }
                        else if (currentLexem.name == "¬") {
                            return "Выражение  \"¬¬\" является избыточным, необходимо его убрать.";
                        }
                        else if (currentLexem.type == Lexer.TYPE_LEXEM.nameOperator) {
                            return "Операция \"НЕ\" не применима к операторной вершине \"" + currentLexem.name + "\".";
                        }
                        else {
                            return "Отсутствует условная вершина после операции \"НЕ\".";
                        }
                    }
                    //////////////////////////////////////////////////////////////
                    case 12: {
                        //Пояснение этой ошикби выдается при попытке добавления перехода.
                        break;
                    }
                    //////////////////////////////////////////////////////////////
                    case 13: {
                        //В данном состоянии не может возникнуть ошибка, так как безусловный переход.
                        break;
                    }
                    //////////////////////////////////////////////////////////////
                    case 14: {
                        //Пояснение этой ошикби выдается при попытке добавления перехода.
                        break;
                    }
                    //////////////////////////////////////////////////////////////
                    case 15: {
                        return "После операторной вершины \"" + listLexem[indexLexem - 1].name + "\" ожидался символ \"*\" или конец строки.";
                    }
                    //////////////////////////////////////////////////////////////
                    case 16: {
                        if (endLine) {
                            return "Отсутствует правое выражение у символа \"*\".";
                        }
                        else {
                            return "После символа \"*\" необходим символ \"↓\", иначе идущая вершина после символа \"*\" будет не достижимой.";
                        }
                    }
                    //////////////////////////////////////////////////////////////
                    case 17: {
                        return "Отсутствует идентификатор у \"↓\" после условной вершины \"" + currentVertex + "\".";
                    }
                    //////////////////////////////////////////////////////////////
                    case 18: {
                        //В данном состоянии не может возникнуть ошибка, так как при всех лексемах есть выход.
                        break;
                    }
                    //////////////////////////////////////////////////////////////
                    case 19: {
                        return "Выход \"↓" + bufferNumberUp + "\" является не уникальным.";
                    }
                    //////////////////////////////////////////////////////////////
                    case 20: {
                        if (endLine) {
                            return "Не определен переход из \"↓" + bufferNumberUp + "\".";
                        }
                        else if (aNumberUp.Contains(currentLexem.name)) {
                            return "Не корректно задан индекс \"↓⁰" + currentLexem.name + "\".";
                        }
                        else if (currentLexem.name == "↓") {
                            return "Сочетание  ↓ ↓ является избыточным, необходимо заменить его на одну ↓.";
                        }
                        else {
                            return "Не определен переход из \"↓" + bufferNumberUp + "\".";
                        }
                    }
                    //////////////////////////////////////////////////////////////
                    case 21: {
                        if (endLine) {
                            return "Отсутствует условная вершина после операции \"НЕ\".";
                        }
                        else if (currentLexem.name == "¬") {
                            return "Выражение  \"¬¬\" является избыточным.";
                        }
                        else if (currentLexem.type == Lexer.TYPE_LEXEM.nameOperator) {
                            return "Операция \"НЕ\" не применима к операторной вершине \"" + currentLexem.name + "\".";
                        }
                        else {
                            return "Отсутствует условная вершина после операции \"НЕ\".";
                        }
                    }
                    //////////////////////////////////////////////////////////////
                    case 22: {
                        //В данном состоянии не может возникнуть ошибка, так как безусловный переход.
                        break;
                    }
                    //////////////////////////////////////////////////////////////
                    case 23: {
                        //Пояснение этой ошикби выдается при попытке добавления перехода.
                        break;
                    }
                    //////////////////////////////////////////////////////////////
                    case 24: {
                        //В данном сосоянии не может возникнуть ошибка, так как оно является конечным.
                        break;
                    }
                    //////////////////////////////////////////////////////////////
                }
                return "Здесь должно было быть пояснение ошибки, возникшей в состоянии " + condition + " под входной лексемой \"" + currentLexem.name + "\".";
            }

            //Выходные сигналы автомата.
            void lambda0()
            {
                currentIsTrueWay    = true;
                futureIsTrueWay     = true;
                indexLexem = 0;
                if (indexLexem < listLexem.Count()) {
                    currentLexem = listLexem[indexLexem];
                }
                else {
                    endLine = true;
                }
            }
            void lambda1()
            {
                tryAddTransitionFromOperator(result.fromOperator, vertexFrom, currentLexem.name);
                indexLexem++;
                if (indexLexem < listLexem.Count()) {
                    currentLexem = listLexem[indexLexem];
                }
                else {
                    endLine = true;
                }
            }
            void lambda2()
            {
                currentIsTrueWay = false;
                indexLexem++;
                if (indexLexem < listLexem.Count()) {
                    currentLexem = listLexem[indexLexem];
                }
                else {
                    endLine = true;
                }
            }
            void lambda3()
            {
                tryAddTransitionFromOperator(result.fromOperator, vertexFrom, currentLexem.name);
            }
            void lambda4()
            {
                currentVertex = currentLexem.name;
                indexLexem++;
                if (indexLexem < listLexem.Count()) {
                    currentLexem = listLexem[indexLexem];
                }
                else {
                    endLine = true;
                }
            }
            void lambda5()
            {
                bufferNumberUp = "";
                indexLexem++;
                if (indexLexem < listLexem.Count()) {
                    currentLexem = listLexem[indexLexem];
                }
                else {
                    endLine = true;
                }
            }
            void lambda6()
            {
                bufferNumberUp += currentLexem.name;
                indexLexem++;
                if (indexLexem < listLexem.Count()) {
                    currentLexem = listLexem[indexLexem];
                }
                else {
                    endLine = true;
                }
            }
            void lambda7()
            {
                wasMetDownArrow = false;
                for (int i = 0; i < alreadyMetDownArrow.Count; i++) {
                    if (alreadyMetDownArrow[i].indexArrow == bufferNumberUp) {
                        linkDownArrow = alreadyMetDownArrow[i].vertexTo;
                        wasMetDownArrow = true;
                        break;
                    }
                }
            }
            void lambda8()
            {
                successAdd = tryAddTransitionFromCondition(result.fromCondition, currentVertex, linkDownArrow, !currentIsTrueWay);
            }
            void lambda9()
            {
                waitingCondition.Add(new ConditionWaitDownArrow() { indexArrow = bufferNumberUp, nameCondition = currentVertex, isFalseWay = currentIsTrueWay });
            }
            void lambda10()
            {
                futureIsTrueWay = false;
                indexLexem++;
                if (indexLexem < listLexem.Count()) {
                    currentLexem = listLexem[indexLexem];
                }
                else {
                    endLine = true;
                }
            }
            void lambda11()
            {
                successAdd = tryAddTransitionFromCondition(result.fromCondition, currentVertex, currentLexem.name, currentIsTrueWay);
            }
            void lambda12()
            {
                currentIsTrueWay    = futureIsTrueWay;
                futureIsTrueWay     = true;
            }
            void lambda13()
            {
                indexLexem++;
                if (indexLexem < listLexem.Count()) {
                    currentLexem = listLexem[indexLexem];
                }
                else {
                    endLine = true;
                }
            }
            void lambda14()
            {
                unicueDownArrow = true;
                for (int i = 0; i < alreadyMetDownArrow.Count; i++) {
                    if (bufferNumberUp == alreadyMetDownArrow[i].indexArrow) {
                        unicueDownArrow = false;
                        break;
                    }
                }
            }
            void lambda15()
            {
                alreadyMetDownArrow.Add(new MetDownArrow() { vertexTo = currentLexem.name, indexArrow = bufferNumberUp });
            }
            void lambda16()
            {
                for (int j = 0; j < waitingCondition.Count; j++) {
                    if (waitingCondition[j].indexArrow == bufferNumberUp) {
                        successAdd = tryAddTransitionFromCondition(result.fromCondition, waitingCondition[j].nameCondition, currentLexem.name, !waitingCondition[j].isFalseWay);
                        if (successAdd.hasError) {
                            break;
                        }
                        waitingCondition.RemoveAt(j);
                        j--;
                    }
                }
            }
            ///////////////////////////////////////////////////////////////////////////////////////////
        }

        //Автомат преобразования ЛСА в набор переходов.
        private ResultConvert convertLSAToTransition(List<Lexer.Lexem> listLexem)
        {
            //Инициализация результата конвертирования.
            ResultConvert result = new ResultConvert() {
                hasErrors           = false,
                explanationError    = "",
                fromOperator        = new List<OperatorVertex>(),
                fromCondition       = new List<ConditionVertex>()
            };

            //Списки.
            List<ConditionWaitDownArrow>    waitingCondition    = new List<ConditionWaitDownArrow>();   //Список ждущих условных вершин соответствующей стрелки вниз.
            List<MetDownArrow>              alreadyMetDownArrow = new List<MetDownArrow>();             //Список уже встреченных стрелок вниз.

            //Переменные.
            Lexer.Lexem     currentLexem = new Lexer.Lexem();           //Текущая лексема.
            int             indexLexem          = 0;                    //Индекс лексемы.
            bool            currentIsTrueWay    = true;                 //Логическое условие текущей вершины.
            bool            futureIsTrueWay     = true;                 //Логическое условие следующей вершины.  
            string          currentVertex       = "";                   //Текущая вершина.
            string          bufferNumberUp      = "";                   //Буфер цифр верхнего регистра.
            string          linkDownArrow       = "";                   //Вершина, на которую ссылается соответствующая срелка вниз.

            //Флаги
            bool            endLine             = false;                //Флаг конца строки.
            CheckResult     successAdd          = new CheckResult();    //Флаг успешного добавления перехода/переходов.
            bool            unicueDownArrow     = false;                //Флаг уникальности стрелки вниз с соответствующим индекосм.
            bool            wasMetDownArrow     = false;                //Флаг того, что стрелка вниз с соответствующим индекосм уже была встречена до этого.
            bool            wasW                = false;                //Флаг присутствия безусловного прехода.
            bool            isFinalOperator     = false;                //Флаг того, что текущая лексема является конечной вершиной.

            //Автомат Мура для преобразования S3 в набор переходов.
            int condition = 0;                                      //Переход в начальное состояние автомата.
            while (true) {
                switch (condition) {
                    //////////////////////////////////////////////////////////////
                    case 0: {
                        //Операция состояния.
                        lambda0();

                        //Не ожидаемый конец строки.
                        if (endLine) {
                            result.hasErrors        = true;
                            result.explanationError = getExplanationError();
                            return result;
                        }
                        //Лексемы, под которыми есть переходы.
                        else if (algorithm.isStartOperator(currentLexem.name)) {
                            condition = 1;
                        }
                        //Лексемы, под которыми нет переходов.
                        else {
                            result.hasErrors        = true;
                            result.explanationError = getExplanationError();
                            return result;
                        }
                        break;
                    }
                    //////////////////////////////////////////////////////////////
                    case 1: {
                        //Операция состояния.
                        lambda1();

                        //Не ожидаемый конец строки.
                        if (!endLine) {
                            condition = 2;
                        }
                        //Лексемы, под которыми нет переходов.
                        else {
                            result.hasErrors        = true;
                            result.explanationError = getExplanationError();
                            return result;
                        }
                        break;
                    }
                    //////////////////////////////////////////////////////////////
                    case 2: {
                        //Не ожидаемый конец строки.
                        if (endLine) {
                            result.hasErrors        = true;
                            result.explanationError = getExplanationError();
                            return result;
                        }
                        //Лексемы, под которыми есть переходы.
                        else if (currentLexem.name == "¬") {
                            condition = 5;
                        }
                        else if (currentLexem.name == "↓") {
                            condition = 14;
                        }
                        else if (currentLexem.name == "ω") {
                            condition = 20;
                        }
                        else if (currentLexem.type == Lexer.TYPE_LEXEM.nameCondition) {
                            condition = 6;
                        }
                        else if (currentLexem.type == Lexer.TYPE_LEXEM.nameOperator) {
                            condition = 3;
                        }
                        //Лексемы, под которыми нет переходов.
                        else {
                            result.hasErrors        = true;
                            result.explanationError = getExplanationError();
                            return result;
                        }
                        break;
                    }
                    //////////////////////////////////////////////////////////////
                    case 3: {
                        //Операция состояния.
                        lambda2();

                        //Анализ результата операции.
                        if (!successAdd.hasError) {
                            condition = 4;
                        }
                        else {
                            result.hasErrors        = true;
                            result.explanationError = successAdd.explanationError;
                            return result;
                        }
                        break;
                    }
                    //////////////////////////////////////////////////////////////
                    case 4: {
                        //Операция состояния.
                        lambda3();

                        //Анализ результата операции.
                        if (isFinalOperator) {
                            condition = 21;
                        }
                        else {
                            condition = 1;
                        }
                        break;
                    }
                    //////////////////////////////////////////////////////////////
                    case 5: {
                        //Операция состояния.
                        lambda4();

                        //Не ожидаемый конец строки.
                        if (endLine) {
                            result.hasErrors        = true;
                            result.explanationError = getExplanationError();
                            return result;
                        }
                        //Лексемы, под которыми есть переходы.
                        else if (currentLexem.type == Lexer.TYPE_LEXEM.nameCondition) {
                            condition = 6;
                        }
                        //Лексемы, под которыми нет переходов.
                        else {
                            result.hasErrors        = true;
                            result.explanationError = getExplanationError();
                            return result;
                        }
                        break;
                    }
                    //////////////////////////////////////////////////////////////
                    case 6: {
                        //Операция состояния.
                        lambda2();

                        //Анализ результата операции.
                        if (!successAdd.hasError) {
                            condition = 7;
                        }
                        else {
                            result.hasErrors        = true;
                            result.explanationError = successAdd.explanationError;
                            return result;
                        }
                        break;
                    }
                    //////////////////////////////////////////////////////////////
                    case 7: {
                        //Операция состояния.
                        lambda1();

                        //Не ожидаемый конец строки.
                        if (endLine) {
                            result.hasErrors        = true;
                            result.explanationError = getExplanationError();
                            return result;
                        }
                        //Лексемы, под которыми есть переходы.
                        else if (currentLexem.name == "↑") {
                            condition = 8;
                        }
                        //Лексемы, под которыми нет переходов.
                        else {
                            result.hasErrors        = true;
                            result.explanationError = getExplanationError();
                            return result;
                        }
                        break;
                    }
                    //////////////////////////////////////////////////////////////
                    case 8: {
                        //Операция состояния.
                        lambda5();

                        //Не ожидаемый конец строки.
                        if (endLine) {
                            result.hasErrors        = true;
                            result.explanationError = getExplanationError();
                            return result;
                        }
                        //Лексемы, под которыми есть переходы.
                        else if (aNumberUp.Contains(currentLexem.name)) {
                            condition = 9;
                        }
                        //Лексемы, под которыми нет переходов.
                        else {
                            result.hasErrors        = true;
                            result.explanationError = getExplanationError();
                            return result;
                        }
                        break;
                    }
                    //////////////////////////////////////////////////////////////
                    case 9: {
                        //Операция состояния.
                        lambda6();

                        if (endLine) {
                            condition = 10;
                        }
                        //Лексемы, под которыми есть переходы.
                        else if (aNumberUp.Contains(currentLexem.name)) {
                            condition = 9;
                        }
                        else {
                            condition = 10;
                        }
                        break;
                    }
                    //////////////////////////////////////////////////////////////
                    case 10: {
                        //Операция состояния.
                        lambda7();

                        //Анализ результата операции.
                        if (wasMetDownArrow) {
                            condition = 11;
                        }
                        else {
                            condition = 12;
                        }
                        break;
                    }
                    //////////////////////////////////////////////////////////////
                    case 11: {
                        //Операция состояния.
                        lambda8();

                        //Анализ результата операции.
                        if (!successAdd.hasError) {
                            condition = 13;
                        }
                        else {
                            result.hasErrors        = true;
                            result.explanationError = successAdd.explanationError;
                            return result;
                        }
                        break;
                    }
                    //////////////////////////////////////////////////////////////
                    case 12: {
                        //Операция состояния.
                        lambda9();

                        //Безусловный переход.
                        condition = 13;
                        break;
                    }
                    //////////////////////////////////////////////////////////////
                    case 13: {
                        //Анализ флага.
                        if (wasW) {
                            condition = 22;
                        }
                        else {
                            condition = 2;
                        }
                        break;
                    }
                    //////////////////////////////////////////////////////////////
                    case 14: {
                        //Операция состояния.
                        lambda5();

                        //Не ожидаемый конец строки.
                        if (endLine) {
                            result.hasErrors        = true;
                            result.explanationError = getExplanationError();
                            return result;
                        }
                        //Лексемы, под которыми есть переходы.
                        else if (aNumberUp.Contains(currentLexem.name)) {
                            condition = 15;
                        }
                        //Лексемы, под которыми нет переходов.
                        else {
                            result.hasErrors        = true;
                            result.explanationError = getExplanationError();
                            return result;
                        }
                        break;
                    }
                    //////////////////////////////////////////////////////////////
                    case 15: {
                        //Операция состояния.
                        lambda6();

                        //Не ожидаемый конец строки.
                        if (endLine) {
                            result.hasErrors        = true;
                            result.explanationError = getExplanationError();
                            return result;
                        }
                        //Лексемы, под которыми есть переходы.
                        else if (aNumberUp.Contains(currentLexem.name)) {
                            condition = 15;
                        }
                        else {
                            condition = 16;
                        }
                        break;
                    }
                    //////////////////////////////////////////////////////////////
                    case 16: {
                        //Операция состояния.
                        lambda10();

                        //Анализ результата операции.
                        if (unicueDownArrow) {
                            if (currentLexem.name == "¬") {
                                condition = 17;
                            }
                            else if ((currentLexem.type == Lexer.TYPE_LEXEM.nameOperator) || (currentLexem.type == Lexer.TYPE_LEXEM.nameCondition)) {
                                condition = 18;
                            }
                            else {
                                result.hasErrors        = true;
                                result.explanationError = getExplanationError();
                                return result;
                            }
                        }
                        else {
                            result.hasErrors        = true;
                            result.explanationError = getExplanationError();
                            return result;
                        }
                        break;
                    }
                    //////////////////////////////////////////////////////////////
                    case 17: {
                        //Операция состояния.
                        lambda4();

                        //Не ожидаемый конец строки.
                        if (endLine) {
                            result.hasErrors        = true;
                            result.explanationError = getExplanationError();
                            return result;
                        }
                        //Лексемы, под которыми есть переходы.
                        else if (currentLexem.type == Lexer.TYPE_LEXEM.nameCondition) {
                            condition = 18;
                        }
                        //Лексемы, под которыми нет переходов.
                        else {
                            result.hasErrors        = true;
                            result.explanationError = getExplanationError();
                            return result;
                        }
                        break;
                    }
                    //////////////////////////////////////////////////////////////
                    case 18: {
                        //Операция состояния.
                        lambda11();

                        //Анализ результата операции.
                        if (!successAdd.hasError) {
                            condition = 19;
                        }
                        else {
                            result.hasErrors        = true;
                            result.explanationError = getExplanationError();
                            return result;
                        }

                        break;
                    }
                    //////////////////////////////////////////////////////////////
                    case 19: {
                        //Операция состояния.
                        lambda12();

                        //Анализ флага.
                        if (wasW) {
                            condition = 4;
                        }
                        else {
                            condition = 2;
                        }
                        break;
                    }
                    //////////////////////////////////////////////////////////////
                    case 20: {
                        //Операция состояния.
                        lambda14();

                        //Не ожидаемый конец строки.
                        if (endLine) {
                            result.hasErrors        = true;
                            result.explanationError = getExplanationError();
                            return result;
                        }
                        //Лексемы, под которыми есть переходы.
                        else if (currentLexem.name == "↑") {
                            condition = 8;
                        }
                        //Лексемы, под которыми нет переходов.
                        else {
                            result.hasErrors        = true;
                            result.explanationError = getExplanationError();
                            return result;
                        }
                        break;
                    }
                    //////////////////////////////////////////////////////////////
                    case 21: {
                        //Операция состояния.
                        lambda13();

                        //Ожидаемый конец.
                        if (endLine) {
                            condition = 24;
                        }
                        //Лексемы, под которыми есть переходы.
                        else if (currentLexem.name == "*") {
                            condition = 23;
                        }
                        //Лексемы, под которыми нет переходов.
                        else {
                            result.hasErrors        = true;
                            result.explanationError = getExplanationError();
                            return result;
                        }
                        break;
                    }
                    //////////////////////////////////////////////////////////////
                    case 22: {
                        //Операция состояния.
                        lambda15();
                         
                        //Безусловный переход.
                        condition = 23;
                        break;
                    }
                    //////////////////////////////////////////////////////////////
                    case 23: {
                        //Операция состояния.
                        lambda13();

                        //Ожидаемый конец.
                        if (endLine) {
                            condition = 24;
                        }
                        //Лексемы, под которыми есть переходы.
                        else if (currentLexem.name == "↓") {
                            condition = 14;
                        }
                        else if (currentLexem.name == "*") {
                            condition = 23;
                        }
                        //Лексемы, под которыми нет переходов.
                        else {
                            result.hasErrors = true;
                            result.explanationError = getExplanationError();
                            return result;
                        }

                        break;
                    }
                    //////////////////////////////////////////////////////////////
                    case 24: {
                        if (waitingCondition.Count != 0) {
                            result.hasErrors        = true;
                            result.explanationError = "Отсутствует \"↓" + waitingCondition[0].indexArrow + "\".";
                        }
                        return result;
                    }
                    //////////////////////////////////////////////////////////////
                    default: {
                        throw new Exception("Переход в неизвестное состояние при конвертации ЛСА в набор переходов.");
                    }
                    //////////////////////////////////////////////////////////////
                }
            }


            /////////////////////////////////////Локальные функции/////////////////////////////////////
            //Формирование сообщения об ошибке.
            string getExplanationError()
            {
                switch (condition) {
                    //////////////////////////////////////////////////////////////
                    case 0: {
                        if (endLine) {
                            return "Строка пуста.";
                        }
                        else {
                            return "ЛСА должна начинаться с начального оператора.";
                        }
                    }
                    //////////////////////////////////////////////////////////////
                    case 1: {
                        return "Не определен переход из вершины \"" + currentVertex + "\".";
                    }
                    //////////////////////////////////////////////////////////////
                    case 2: {
                        if (endLine) {
                            return "Не определен переход из вершины \"" + currentVertex + "\".";
                        }
                        else if (currentLexem.name == "↑") {
                            return "Символ \"↑\" может идти только после условной вершины.";
                        }
                        else {
                            return "Не определен переход из вершины \"" + currentVertex + "\".";
                        }
                    }
                    //////////////////////////////////////////////////////////////
                    case 3: {
                        //Пояснение этой ошикби выдается при попытке добавления перехода.
                        break;
                    }
                    //////////////////////////////////////////////////////////////
                    case 4: {
                        //В данном состоянии не может возникнуть ошибки, так как проверяется флаг, при любом значении которого есть переход.
                        break;
                    }
                    //////////////////////////////////////////////////////////////
                    case 5: {
                        if (endLine) {
                            return "Отсутствует условная вершина после операции \"НЕ\".";
                        }
                        else if (currentLexem.name == "¬") {
                            return "Выражение  \"¬¬\" является избыточным, необходимо его убрать.";
                        }
                        else if (currentLexem.type == Lexer.TYPE_LEXEM.nameOperator) {
                            return "Операция \"НЕ\" не применима к операторной вершине \"" + currentLexem.name + "\".";
                        }
                        else {
                            return "Отсутствует условная вершина после операции \"НЕ\".";
                        }
                    }
                    //////////////////////////////////////////////////////////////
                    case 6: {
                        //Пояснение этой ошикби выдается при попытке добавления перехода.
                        break;
                    }
                    //////////////////////////////////////////////////////////////
                    case 7: {
                        if (endLine) {
                            return "Не определен переход из вершины \"" + currentVertex + "\".";
                        }
                        else {
                            return "После условной вершины \"" + currentVertex + "\" отсутствует символ \"↑\".";
                        }
                    }
                    //////////////////////////////////////////////////////////////
                    case 8: {
                        if (wasW) {
                            return "Отсутствует идентификатор у \"↑\" безусловного перехода.";
                        }
                        else {
                            return "Отсутствует идентификатор у \"↑\" после условной вершины \"" + currentVertex + "\".";
                        }
                    }
                    //////////////////////////////////////////////////////////////
                    case 9: {
                        //В данном состоянии не может возникнуть ошибка, так как при всех лексемах есть выход.
                        break;
                    }
                    //////////////////////////////////////////////////////////////
                    case 10: {
                        //В данном состоянии не может возникнуть ошибки, так как проверяется флаг, при любом значении которого есть переход.
                        break;
                    }
                    //////////////////////////////////////////////////////////////
                    case 11: {
                        //Пояснение этой ошикби выдается при попытке добавления перехода.
                        break;
                    }
                    //////////////////////////////////////////////////////////////
                    case 12: {
                        //В данном состоянии не может возникнуть ошибка, так как безусловный переход.
                        break;
                    }
                    //////////////////////////////////////////////////////////////
                    case 13: {
                        //В данном состоянии не может возникнуть ошибки, так как проверяется флаг, при любом значении которого есть переход.
                        break;
                    }
                    //////////////////////////////////////////////////////////////
                    case 14: {
                        return "Отсутствует идентификатор у \"↓\"";
                    }
                    //////////////////////////////////////////////////////////////
                    case 15: {
                        //В данном состоянии не может возникнуть ошибка, так как при всех лексемах есть выход.
                        break;
                    }
                    //////////////////////////////////////////////////////////////
                    case 16: {
                        return "Выход \"↓" + bufferNumberUp + "\" является не уникальным.";
                    }
                    //////////////////////////////////////////////////////////////
                    case 17: {
                        if (endLine) {
                            return "Не определен переход из \"↓" + bufferNumberUp + "\".";
                        }
                        else if (aNumberUp.Contains(currentLexem.name)) {
                            return "Не корректно задан индекс \"↓⁰" + currentLexem.name + "\".";
                        }
                        else if (currentLexem.name == "↓") {
                            return "Сочетание  ↓ ↓ является избыточным, необходимо заменить его на одну ↓.";
                        }
                        else {
                            return "Не определен переход из \"↓" + bufferNumberUp + "\".";
                        }
                    }
                    //////////////////////////////////////////////////////////////
                    case 18: {
                        if (endLine) {
                            return "Отсутствует условная вершина после операции \"НЕ\".";
                        }
                        else if (currentLexem.name == "¬") {
                            return "Выражение  \"¬¬\" является избыточным, необходимо его убрать.";
                        }
                        else if (currentLexem.type == Lexer.TYPE_LEXEM.nameOperator) {
                            return "Операция \"НЕ\" не применима к операторной вершине \"" + currentLexem.name + "\".";
                        }
                        else {
                            return "Отсутствует условная вершина после операции \"НЕ\".";
                        }
                    }
                    //////////////////////////////////////////////////////////////
                    case 19: {
                        //Пояснение этой ошикби выдается при попытке добавления перехода.
                        break;
                    }
                    //////////////////////////////////////////////////////////////
                    case 20: {
                        //В данном состоянии не может возникнуть ошибки, так как проверяется флаг, при любом значении которого есть переход.
                        break;
                    }
                    //////////////////////////////////////////////////////////////
                    case 21: {
                        return "После символа \"ω\" отсутствует символ \"↑\".";
                    }
                    //////////////////////////////////////////////////////////////
                    case 22: {
                        if (algorithm.isFinalOperator(listLexem[indexLexem - 1].name)) {
                            return "После конечной вершины должен быть конец строки, либо символ \"*\".";
                        }
                        else {
                            return "После символа безусловного перехода \"ω\" должен быть конец строки, либо либо символ \"↓\", иначе дальше идущая вершина будет не достижимой.";
                        }
                    }
                    //////////////////////////////////////////////////////////////
                    case 23: {
                        //В данном сосоянии не может возникнуть ошибка, так как оно является конечным.
                        break;
                    }
                    //////////////////////////////////////////////////////////////
                }
                return "Здесь должно было быть пояснение ошибки, возникшей в состоянии " + condition + " под входной лексемой \"" + currentLexem.name + "\".";
            }

            //Выходные сигналы автомата.
            void lambda0()
            {
                currentIsTrueWay    = true;
                futureIsTrueWay     = true;
                indexLexem          = 0;
                if (indexLexem < listLexem.Count()) {
                    currentLexem = listLexem[indexLexem];
                }
                else {
                    endLine = true;
                }
            }
            void lambda1()
            {
                currentVertex       = currentLexem.name;
                currentIsTrueWay    = futureIsTrueWay;
                futureIsTrueWay     = true;
                wasW                = false;
                indexLexem++;
                if (indexLexem < listLexem.Count()) {
                    currentLexem = listLexem[indexLexem];
                }
                else {
                    endLine = true;
                }
            }
            void lambda2()
            {
                if (algorithm.isOperator(currentVertex)) {
                    successAdd = tryAddTransitionFromOperator(result.fromOperator, currentVertex, currentLexem.name);
                }
                else {
                    successAdd = tryAddTransitionFromCondition(result.fromCondition, currentVertex, currentLexem.name, currentIsTrueWay);
                }
            }
            void lambda3()
            {
                isFinalOperator = algorithm.isFinalOperator(currentLexem.name);
                wasW            = true;
            }
            void lambda4()
            {
                futureIsTrueWay = false;
                indexLexem++;
                if (indexLexem < listLexem.Count()) {
                    currentLexem = listLexem[indexLexem];
                }
                else {
                    endLine = true;
                }
            }
            void lambda5()
            {
                bufferNumberUp = "";
                indexLexem++;
                if (indexLexem < listLexem.Count()) {
                    currentLexem = listLexem[indexLexem];
                }
                else {
                    endLine = true;
                }
            }
            void lambda6()
            {
                bufferNumberUp += currentLexem.name;
                indexLexem++;
                if (indexLexem < listLexem.Count()) {
                    currentLexem = listLexem[indexLexem];
                }
                else {
                    endLine = true;
                }
            }
            void lambda7()
            {
                wasMetDownArrow = false;
                for (int i = 0; i < alreadyMetDownArrow.Count; i++) {
                    if (alreadyMetDownArrow[i].indexArrow == bufferNumberUp) {
                        linkDownArrow = alreadyMetDownArrow[i].vertexTo;
                        wasMetDownArrow = true;
                        break;
                    }
                }
            }
            void lambda8()
            {
                if (algorithm.isOperator(currentVertex)) {
                    successAdd = tryAddTransitionFromOperator(result.fromOperator, currentVertex, linkDownArrow);
                }
                else {
                    successAdd = tryAddTransitionFromCondition(result.fromCondition, currentVertex, linkDownArrow, !currentIsTrueWay);
                }
            }
            void lambda9()
            {
                waitingCondition.Add(new ConditionWaitDownArrow() { indexArrow = bufferNumberUp, nameCondition = currentVertex, isFalseWay = currentIsTrueWay });
            }
            void lambda10()
            {
                unicueDownArrow = true;
                for (int i = 0; i < alreadyMetDownArrow.Count; i++) {
                    if (bufferNumberUp == alreadyMetDownArrow[i].indexArrow) {
                        unicueDownArrow = false;
                        break;
                    }
                }
            }
            void lambda11()
            {
                for (int j = 0; j < waitingCondition.Count; j++) {
                    if (waitingCondition[j].indexArrow == bufferNumberUp) {
                        if (algorithm.isOperator(waitingCondition[j].nameCondition)) {
                            successAdd = tryAddTransitionFromOperator(result.fromOperator, waitingCondition[j].nameCondition, currentLexem.name);
                        }
                        else {
                            successAdd = tryAddTransitionFromCondition(result.fromCondition, waitingCondition[j].nameCondition, currentLexem.name, !waitingCondition[j].isFalseWay);
                        }
                        if (successAdd.hasError) {
                            break;
                        }
                        waitingCondition.RemoveAt(j);
                        j--;
                    }
                }
            }
            void lambda12()
            {
                alreadyMetDownArrow.Add(new MetDownArrow() { vertexTo = currentLexem.name, indexArrow = bufferNumberUp });
            }
            void lambda13()
            {
                indexLexem++;
                if (indexLexem < listLexem.Count()) {
                    currentLexem = listLexem[indexLexem];
                }
                else {
                    endLine = true;
                }
            }
            void lambda14()
            {
                wasW = true;
                currentIsTrueWay = !currentIsTrueWay;
                indexLexem++;
                if (indexLexem < listLexem.Count()) {
                    currentLexem = listLexem[indexLexem];
                }
                else {
                    endLine = true;
                }
            }
            void lambda15()
            {
                indexLexem--;
                currentLexem = listLexem[indexLexem];
            }
            ///////////////////////////////////////////////////////////////////////////////////////////
        }



        //************************************************************************************************
        // Првоерка набора переходов.
        //************************************************************************************************

        //Проход по графу до операторных вершин с проверкой правильности построенных путей.
        private CheckResult travel(ConditionVertex checkedVertex,  List<ConditionVertex> conditionVertex)
        {
            ConditionVertex originalCondition = algorithm.getConditionVertex(checkedVertex.Name);

            //Если в данной вершине на истинном путе прерывается путь.
            if (checkedVertex.VertexTo_IfTrue == "") {
                return new CheckResult() {
                    hasError            = true,
                    explanationError    = "Не определен переход при истине из вершины \"" + checkedVertex.Name + "\"."
                };
            }
            //Если на истинном переходе не прерывается путь.
            else {
                if (checkedVertex.VertexTo_IfTrue == originalCondition.VertexTo_IfTrue) {
                    if (algorithm.isCondition(originalCondition.VertexTo_IfTrue)) {
                        CheckResult result = travel(getCheckedVertex(originalCondition.VertexTo_IfTrue), conditionVertex);
                        if (result.hasError) {
                            return result;
                        }
                    }
                }
                else {
                    return new CheckResult() {
                        hasError            = true,
                        explanationError    = "Не правильно определен переход при истине из вершины \"" + checkedVertex.Name + "\" (\"" + checkedVertex.VertexTo_IfTrue + "\")."
                    };
                }
            }

            //Если в данной вершине на ложном путе прерывается путь.
            if (checkedVertex.VertexTo_IfFalse == "") {
                return new CheckResult() {
                    hasError            = true,
                    explanationError    = "Не определен переход при лжи из вершины \"" + checkedVertex.Name + "\"."
                };
            }
            //Если на ложном переходе не прерывается путь.
            else {
                if (checkedVertex.VertexTo_IfFalse == originalCondition.VertexTo_IfFalse) {
                    if (algorithm.isCondition(originalCondition.VertexTo_IfFalse)) {
                        return travel(getCheckedVertex(originalCondition.VertexTo_IfFalse), conditionVertex);
                    }
                    else {
                        return new CheckResult() { hasError = false };
                    }
                }
                else {
                    return new CheckResult() {
                        hasError            = true,
                        explanationError    = "Не правильно определен переход при лжи из вершины \"" + checkedVertex.Name + "\" (\"" + checkedVertex.VertexTo_IfFalse + "\")."
                    };
                }
            }

            /////////////////////////////////////Локальные функции/////////////////////////////////////
            //Полученеи условной вершины из списка построенных переходов.
            ConditionVertex getCheckedVertex(string name)
            {
                for (int i = 0; i < conditionVertex.Count; i++) {
                    if (conditionVertex[i].Name == name) {
                        return conditionVertex[i];
                    }
                }
                throw new Exception("Запрос на выдачу вершины с именем \"" + name + "\" из списка построенных вершин, которой нет в списке.");
            }
            ///////////////////////////////////////////////////////////////////////////////////////////
        }

        //Проверка части алгоритма.
        private CheckResult checkCorrectPartAlgorithm(List<OperatorVertex> fromOperator, List<ConditionVertex> fromCondition)
        {
            //После преобразования набора лексем в набор переходов в списке fromOperator должен быть один переход
            //из вершины, к которой принадлежит логическая строка. В списке fromCondition должно быть один и более переходов, 
            //если fromOperator ведет в условную вершину, либо ни одного, если в операторную вершину.

            OperatorVertex originalVertexFrom = algorithm.getOperatorVertex(fromOperator[0].Name);

            //Не правильно построен переход из операторной вершины вершины.
            if (originalVertexFrom.VertexTo != fromOperator[0].VertexTo) {
                return new CheckResult {
                    hasError = true,
                    explanationError = "Не правильно определен переход из вершины \"" + fromOperator[0].Name + "\" (\"" + fromOperator[0].VertexTo + "\")."
                };
            }

            //Ситуация, когда переход напрямую из оператора в оператор.
            if (algorithm.isOperator(originalVertexFrom.VertexTo)) {
                return new CheckResult() { hasError = false };
            }
            //Ситуакция, когда переход в условную вершину.
            else {
                return travel(getCheckedVertex(originalVertexFrom.VertexTo), fromCondition);
            }

            /////////////////////////////////////Локальные функции/////////////////////////////////////
            //Полученеи условной вершины из списка построенных переходов.
            ConditionVertex getCheckedVertex(string name) {
                for (int i = 0; i < fromCondition.Count; i++) {
                    if (fromCondition[i].Name == name) {
                        return fromCondition[i];
                    }
                }
                throw new Exception("Запрос на выдачу вершины с именем \"" + name + "\" из списка построенных вершин, которой нет в списке.");
            }
            ///////////////////////////////////////////////////////////////////////////////////////////
        }

        private CheckResult checkCorrectAlgorithm(List<OperatorVertex> fromOperator, List<ConditionVertex> fromCondition)
        {
            //Проверка на наличие всех переходов и их коррекстность.
            //Больше вершин, чем в минимизированном алгоритме не может быть, так ранее проверяется на принадлежность имени вершины алгоритму
            //Плюс проверяется, чтобы были устранены все не нужные условные вершины, поэтому может только недоставать переходов.

            //Проверка переходов из операторных вершин.
            for (int i = 0; i < minimizedAlgorithm.NumberOperator; i++) {
                OperatorVertex vertex = minimizedAlgorithm.getOperatorVertex(i);
                if (!vertex.IsFinalOperator) {
                    bool wasFound = false;
                    for (int j = 0; j < fromOperator.Count; j++) {
                        if (vertex.Name == fromOperator[j].Name) {
                            wasFound = true;
                            if (vertex.VertexTo == fromOperator[j].VertexTo) {
                                break;
                            }
                            else {
                                return new CheckResult() {
                                    hasError = true,
                                    explanationError = "Не правильно определен переход из вершины \"" + vertex.Name + "\" (\"" + fromOperator[j].VertexTo + "\")."
                                };
                            }
                        }
                    }
                    if (!wasFound) {
                        return new CheckResult() {
                            hasError = true,
                            explanationError = "Отсутствует переход из вершины \"" + vertex.Name + "\"."
                        };
                    }
                }
            }

            //Проверка переходов из условных вершин.
            for (int i = 0; i < minimizedAlgorithm.NumberCondition; i++) {
                ConditionVertex vertex      = minimizedAlgorithm.getConditionVertex(i);
                bool            wasFound    = false;
                for (int j = 0; j < fromCondition.Count; j++) {
                    if (vertex.Name == fromCondition[j].Name) {
                        wasFound = true;
                        if (vertex.VertexTo_IfTrue != fromCondition[j].VertexTo_IfTrue) {
                            return new CheckResult() {
                                hasError            = true,
                                explanationError    = "Не правильно определен переход при истине из вершины \"" + vertex.Name + "\" (\"" + fromCondition[j].VertexTo_IfTrue + "\")."
                            };
                        }
                        if (vertex.VertexTo_IfFalse == fromCondition[j].VertexTo_IfFalse) {
                            break;
                        }
                        else { 
                            return new CheckResult() {
                                hasError            = true,
                                explanationError    = "Не правильно определен переход при лжи из вершины \"" + vertex.Name + "\" (\"" + fromCondition[j].VertexTo_IfFalse + "\")."
                            };
                        }
                    }
                }
                if (!wasFound) {
                    return new CheckResult() {
                        hasError            = true,
                        explanationError    = "Отсутствует переход из вершины \"" + vertex.Name + "\"."
                    };
                }
            }

            return new CheckResult() { hasError = false };
        }
    }
}

