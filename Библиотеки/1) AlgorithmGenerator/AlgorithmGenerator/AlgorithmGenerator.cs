using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OperatorAlgorithm;

namespace AlgorithmGenerator {

    public class Generator
    {
        /////////////////////////////////////Поля класса/////////////////////////////////////
        private Random          random;                     //Переменная для генерации случайных чисел (НЕ УБИРАТЬ, ТАК КАК РАНДОМ ПЕРЕСТАНЕТ ВЫДАВАТЬ СЛУЧАЙНЫЕ ЧИСЛА)
        private int             nOperator;                  //Количество операторных вершин в алгоритме.
        private int             nCondition;                 //Количество условных вершин в алгоритме.
        private List<int>       notAddedVertex;             //Список еще не присоединненых вершин к алгоритму.
        private List<int>       addedVertex;                //Список присоединенных вершин к алгоритму.
        private List<int>       vertexHasFreeOutput;        //Список присединеных вершин к алгоритму, которые имеют свободные выходы.
        private const int       FREE_TRANSITION = -1;       //Маркер еще не проведенного перехода.
        private int[]           transitionFromOperator;     //Сгенерированные переходы из операторных вершин.
        private int[,]          transitionFromCondition;    //Сгенерированные переходы из условных вершин ([0,i] - Истинный путь, [1,i] - Ложный путь).
        private string          captionOperator;            //Идентификатор операторной вершины.
        private string          captionCondition;           //Идентификатор условной вершины..
        private string          indexStartOperator;         //Индекс начальной вершины.
        private string          indexFinalOperator;         //Индекс конечной вершины.

        /////////////////////////////////////Public методы/////////////////////////////////////
        /// <summary>
        /// Генерация операторного алгоритма.
        /// </summary>
        /// <param name="nOperator">Количество операторных вершин (минимум 2)</param>
        /// <param name="nCondition">Количество условных вершин</param>
        /// <param name="captionOperator">Идентификатор оператора</param>
        /// <param name="captionCondition">Идентификатор условия</param>
        /// <param name="indexStartOperator">Индекс начальной вершины</param>
        /// <param name="indexFinalOperator">Индекс конечной вершины</param>
        /// <returns>Сгенерированный алгоритм</returns>
        public Algorithm generate(int nOperator, int nCondition, string captionOperator, string captionCondition, string indexStartOperator, string indexFinalOperator)
        {
            //Инициализация полей.
            random                  = new Random();
            this.nOperator          = nOperator;
            this.nCondition         = nCondition;
            notAddedVertex          = new List<int>();         
            addedVertex             = new List<int>() { 0 };    //Изначально только начальная вершина присоединена.
            vertexHasFreeOutput     = new List<int>() { 0 };    //Изначально тначальная вершина только имеет выход.
            this.captionOperator    = captionOperator;
            this.captionCondition   = captionCondition;
            this.indexStartOperator = indexStartOperator;
            this.indexFinalOperator = indexFinalOperator;

            //Инициализация массивов переходов.
            transitionFromOperator  = new int[nOperator - 1]; //Из конечного оператора нет выхода, поэтому n-1.
            transitionFromCondition = new int[2, nCondition];  
            for (int i = 0; i < nOperator - 1; i++) {
                transitionFromOperator[i] = FREE_TRANSITION;
            }
            for (int i = 0; i < nCondition; i++) {
                transitionFromCondition[0, i] = FREE_TRANSITION;
                transitionFromCondition[1, i] = FREE_TRANSITION;
            }

            //Генерация алгоритма
            generateAlgorithm();

            //Поиск и удаление условных циклов.
            removeCircleCondition();

            //Формирование результата.
            OperatorVertex[]    operatorVertex  = new OperatorVertex[nOperator];
            ConditionVertex[]   conditionVertex = new ConditionVertex[nCondition];

            //Начальный оператор
            string  vertexName  = captionOperator + indexStartOperator;
            string  vertexTo    = decodeVertex(transitionFromOperator[0]);
            operatorVertex[0]   = new OperatorVertex(vertexName, vertexTo);
            operatorVertex[0].IsStartOperator = true;

            //Конечный оператор.
            vertexName                      = captionOperator + indexFinalOperator;
            vertexTo                        = null;
            operatorVertex[nOperator - 1]   = new OperatorVertex(vertexName, vertexTo);

            //Остальные операторы
            for (int i = 1; i < nOperator - 1; i++) {
                vertexName          = captionOperator + i.ToString();
                vertexTo            = decodeVertex(transitionFromOperator[i]);
                operatorVertex[i]   = new OperatorVertex(vertexName, vertexTo);
            }

            //Условные вершины.
            string vertexTo_IfTrue;
            string vertexTo_IfFalse;
            for (int i = 0; i < nCondition; i++) {
                vertexName          = captionCondition + i.ToString();
                vertexTo_IfTrue     = decodeVertex(transitionFromCondition[0, i]);
                vertexTo_IfFalse    = decodeVertex(transitionFromCondition[1, i]);
                conditionVertex[i]  = new ConditionVertex(vertexName, vertexTo_IfTrue, vertexTo_IfFalse);
            }
            return new Algorithm(operatorVertex, conditionVertex);
        }



        /////////////////////////////////////Private методы/////////////////////////////////////

        //************************************************************************************************
        // Определение типа вершины.
        //************************************************************************************************

        private bool isOperator(int index)
        {
            return (index < nOperator);
        }

        private bool isCondition(int index)
        {
            return (index >= nOperator);
        }

        private bool isStartOperator(int index)
        {
            return (index == 0);
        }

        private bool isFinalOperator(int index)
        {
            return (index == nOperator - 1);
        }


        //************************************************************************************************
        // Генерация алгоритма.
        //************************************************************************************************

        //Присоединение вершины к алгоритму.
        private void addVertex(int vertexTo)
        {
            //Перемещение вершины из одного списка в другой.
            addedVertex.Add(vertexTo);                      //Добавление в список присоединенных вершин.
            notAddedVertex.Remove(vertexTo);                //Удаление из списка еще не присоединенных врешин.

            //Если это не конечная вершина, то добавляем выходы из вершины.
            if (!isFinalOperator(vertexTo)) {
                vertexHasFreeOutput.Add(vertexTo);
                //Если условная вершина, то добавляем еще один выход, так как у условия 2 выхода.
                if (isCondition(vertexTo)) {
                    vertexHasFreeOutput.Add(vertexTo);
                }
            }
        }

        //Получение случайной вершины назначения, не нарушающую корректность генерируемого алгоритма.
        private int getVertexDestination(int vertexFrom)
        {
            //Объявление переменных.
            int vertexTo;           //Выбранная вершина назначения.
            int indexToList;        //Индекс вершины в списке.

            //Если выход последний из свободных, то следует выбрать вершину с учетом еще не добавленных вершин.
            if (vertexHasFreeOutput.Count == 0) {
                //Если есть еще не добавленные вершины, то выбираем из еще не добавленных вершин.
                if (notAddedVertex.Count != 0) {
                    indexToList = random.Next(0, notAddedVertex.Count - 1);
                    vertexTo    = notAddedVertex[indexToList];
                    //Если выбрали конечную вершину, но при этом есть еще не добавленные вершины, то выбираем из еще не добавленных вершин.
                    if (isFinalOperator(vertexTo) && (notAddedVertex.Count != 1)) {
                        indexToList = getAnotherIndex(indexToList, notAddedVertex.Count);
                        vertexTo    = notAddedVertex[indexToList];
                    }
                    addVertex(vertexTo);
                }
                //Если все вершины добавлены, то выбираем случайную врешину.
                else {
                    indexToList = random.Next(1, addedVertex.Count - 1);
                    vertexTo    = addedVertex[indexToList];
                    //Если вершина указвыает на саму себя, то выбираем другую.
                    if (vertexFrom == vertexTo) {
                        indexToList = getAnotherIndex(indexToList, addedVertex.Count);
                        vertexTo = addedVertex[indexToList];
                    }
                }
            }
            //Если выход не последний из свободных, то выбираем случайную вершину из добавленных или еще не добавленных.
            else {
                //Для соединения с уже присоединенной вершиной нужно:
                //Вероятность 40% и минимум 3 добавленных вершины (начальная, текущая вершина и вершина для соединения)
                //Либо, если все вершины уже добавлены.
                if (((random.Next(0, 100) < 40) && (addedVertex.Count > 3)) || (notAddedVertex.Count == 0)) {
                    indexToList = random.Next(1, addedVertex.Count - 1);
                    vertexTo = addedVertex[indexToList];
                    //Если вершина указвыает на саму себя, то выбираем другую вершину.
                    if (vertexFrom == vertexTo) {
                        indexToList = getAnotherIndex(indexToList, addedVertex.Count);
                        vertexTo = addedVertex[indexToList];
                    }
                }
                //Выбор случайной вершины из еще не добавленных.
                else {
                    vertexTo = notAddedVertex[random.Next(0, notAddedVertex.Count - 1)];
                    addVertex(vertexTo);
                }
            }

            //Возвращение результата.
            return vertexTo;


            /////////////////////////////////////Локальные функции/////////////////////////////////////
            //Получение другого индекса, отличного от исключенного индекса и в интервале от 0 до указанной границы.
            int getAnotherIndex(int excludedIndex, int lengthInterval)
            {
                //Если левый интервал от указанного индекса больше, то выбираем из левой части.
                if (excludedIndex - 1 > lengthInterval - 1 - excludedIndex) {
                    return random.Next(1, excludedIndex - 1);                                 //На 0 находится начальный оператор, поэтому рандом с 1.
                }   
                //Если правый интервал от указанного индекса больше, то выбираем из правой части.
                else {
                    return random.Next(excludedIndex + 1, lengthInterval - 1);
                }
            }
            ///////////////////////////////////////////////////////////////////////////////////////////
        }

        private void generateAlgorithm()
        {
            //Запись всех вершин в список еще не добавленных вершин.
            for (int i = 1; i < nOperator + nCondition; i++) {
                notAddedVertex.Add(i);
            }

            //Объявление переменных, используемых в основном цикле.
            int vertexFrom;                 //Вершина, из которой генерируется переход.
            int vertexTo;                   //Вершина, в которую сгенерирован переход.
            int indexOutput;                //Индекс выхода из списка свободных выходов.

            //Основной цикл.
            while (vertexHasFreeOutput.Count > 0) {
                //Выбор случайной вершины из присоединенных, которая имеет свободный выход.
                indexOutput = random.Next(0, vertexHasFreeOutput.Count - 1);  //Выбор случайного индекса из списка вершин, имеющих свободных выход.
                vertexFrom  = vertexHasFreeOutput[indexOutput];                 //Получение вершины по индексу.
                vertexHasFreeOutput.RemoveAt(indexOutput);                      //Удаление выхода из свободных.          
                //Выбор вершины назначени.
                vertexTo = getVertexDestination(vertexFrom);
                //Если операторная вершина, то просто добавляем переход в вершину назначения.
                if (isOperator(vertexFrom)) {
                    transitionFromOperator[vertexFrom] = vertexTo;
                }
                //Если условная вершина, то либо выбираем случайный выход, либо методом исключения.
                else {
                    vertexFrom -= nOperator;
                    if ((transitionFromCondition[0, vertexFrom] == FREE_TRANSITION) && (transitionFromCondition[1, vertexFrom] == FREE_TRANSITION)) {
                        transitionFromCondition[random.Next(0, 1), vertexFrom] = vertexTo;
                    }
                    else {
                        if (transitionFromCondition[0, vertexFrom] == FREE_TRANSITION) {
                            transitionFromCondition[0, vertexFrom] = vertexTo;
                        }
                        else {
                            transitionFromCondition[1, vertexFrom] = vertexTo;
                        }
                    }
                }
            }
        }


        //************************************************************************************************
        // Корректировка алгоритма.
        //************************************************************************************************

        //Проверка вершины на безопасность.
        private void checkCondition(ref bool[] safeVertex, ref List<int> path, int vertex)
        {
            //Из каждой условной вершины проверяем два выхода
            //Если оба выхода ведут к безопасным вершинам, значит условная вершина является безопасной
            //Иначе необходимо проверить не безопасную вершину, к которой осуществляется переход
            //В конечном счете, идя по небезопасным вершинам, мы найдем безопасную, либо цикл.

            int iCondition = vertex - nOperator;  //Индекс перехода в списке переходов из условных вершин.

            //Если переход по истине пока не является безопасным, то проверяем его безопасность.
            if (!safeVertex[transitionFromCondition[0, iCondition]]) {
                bool findCircle = false;
                //Проверка наличия вершины назначения в пройденном пути.
                for (int i = 0; i < path.Count(); i++) {
                    //Если цикл найден - устраняем, иначе переходим к выбранной вершине.
                    if (path[i] == transitionFromCondition[0, iCondition]) {
                        transitionFromCondition[0, iCondition] = random.Next(1, nOperator); //Перевод перехода на операторную вершину.
                        findCircle = true;
                        break;
                    }
                }
                //Если цикл не найден, то проверяем не безопасную вершину.
                if (!findCircle) {
                    path.Add(transitionFromCondition[0, iCondition]);
                    checkCondition(ref safeVertex, ref path, transitionFromCondition[0, iCondition]);
                }
            }

            //Если переход по лжи пока не является безопасным, то проверяем его безопасность.
            if (!safeVertex[transitionFromCondition[1, iCondition]]) {
                bool findCircle = false;
                //Проверка наличия вершины назначения в пройденном пути.
                for (int i = 0; i < path.Count(); i++) {
                    //Если цикл найден - устраняем, иначе переходим к выбранной вершине.
                    if (path[i] == transitionFromCondition[1, iCondition]) {
                        transitionFromCondition[1, iCondition] = random.Next(1, nOperator); //Перевод перехода на операторную вершину.
                        findCircle = true;
                        break;
                    }
                }
                //Если цикл не найден, то проверяем не безопасную вершину.
                if (!findCircle) {
                    path.Add(transitionFromCondition[1, iCondition]);
                    checkCondition(ref safeVertex, ref path, transitionFromCondition[1, iCondition]);
                }
            }

            //Оба перехода являются безопасными, отмечаем вершину как безопасной и возвращаемся.
            safeVertex[iCondition + nOperator] = true;
            path.RemoveAt(path.Count - 1);
        }

        private void removeCircleCondition()
        {
            //Безопасная вершина - вершина, которая не ведет к циклу.
            bool[] safeVertex = new bool[nOperator + nCondition];

            //Изначально операторная вершина является безопасной.
            for (int i = 0; i < nOperator; i++) {
                safeVertex[i] = true;
            }

            //Изначально условная вершина явялется не безопасной.
            for (int i = nOperator; i < nOperator + nCondition; i++) {
                safeVertex[i] = false;
            }

            //Проверка путей из каждой условной вершины.
            //Чтобы правильно определить конец цикла, просматриваются переходы из операторов,
            //и условная вершина, в которую происходит переход из операторной вершины, берется как начало пути.
            for (int i = 0; i < nOperator - 1; i++) {
                if (isCondition(transitionFromOperator[i])) {
                    List<int> path = new List<int> {
                    transitionFromOperator[i]
                };
                    checkCondition(ref safeVertex, ref path, transitionFromOperator[i]);
                }
            }
        }


        //************************************************************************************************
        // Преобразование кода вершины в символьное представление.
        //************************************************************************************************

        private string decodeVertex(int index)
        {
            if (isOperator(index)) {
                if (isStartOperator(index)) {
                    return captionOperator + indexStartOperator;
                }
                else if (isFinalOperator(index)) {
                    return captionOperator + indexFinalOperator;
                }
                else {
                    return captionOperator + index.ToString();
                }
            }
            else {
                return captionCondition + (index - nOperator).ToString();
            }
        }

    }
}
