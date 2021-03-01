using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace OperatorAlgorithm
{
    public class OperatorVertex
    {
        /////////////////////////////////////Поля класса/////////////////////////////////////
        private string  name;           //Имя вершины.
        private string  vertexTo;       //Вершина назначения.
        private bool    isStart;        //Флаг о том, что вершина является начальной.


        /////////////////////////////////////Свойства класса/////////////////////////////////////
        public string Name
        {
            set
            {
                name = value ?? throw new ArgumentException("Недопустимое имя вершины (null).");
            }
            get
            {
                return name;
            }
        }
        public string VertexTo {
            set
            {
                vertexTo = value;
            }
            get 
            {
                return vertexTo;
            }
        }
        public bool IsStartOperator
        {
            set
            {
                isStart = value;
            }
            get
            {
                return isStart;
            }
        }
        public bool IsFinalOperator
        {
            get
            {
                return (vertexTo == null);
            }
        }


        /////////////////////////////////////Public методы/////////////////////////////////////
        public OperatorVertex(string name, string vertexTo)
        {
            this.name       = name ?? throw new ArgumentException("Недопустимое имя вершины (null).");
            this.vertexTo   = vertexTo;
            this.isStart    = false;
        }
    }

    public class ConditionVertex
    {
        /////////////////////////////////////Поля класса/////////////////////////////////////
        private string name;                //Имя вершины.
        private string vertexTo_IfTrue;     //Вершина назначения при выполнении условия.
        private string vertexTo_IfFalse;    //Вершина назначения при не выполнении условия.


        /////////////////////////////////////Свойства класса/////////////////////////////////////
        public string Name
        {
            set
            {
                name = value ?? throw new ArgumentException("Недопустимое имя вершины (null).");
            }
            get
            {
                return name;
            }
        }
        public string VertexTo_IfTrue
        {
            set
            {
                vertexTo_IfTrue = value ?? throw new Exception("Недопустимое значение имени вершины назначения при истине (null).");
            }
            get
            {
                return vertexTo_IfTrue;
            }
        }
        public string VertexTo_IfFalse
        {
            set
            {
                vertexTo_IfFalse = value ?? throw new Exception("Недопустимое значение имени вершины назначения при лжи (null).");
            }
            get
            {
                return vertexTo_IfFalse;
            }
        }

        /////////////////////////////////////Public методы/////////////////////////////////////
        public ConditionVertex(string name, string vertexTo_IfTrue, string vertexTo_IfFalse)
        {
            this.name               = name              ?? throw new ArgumentException("Недопустимое имя вершины (null).");
            this.vertexTo_IfTrue    = vertexTo_IfTrue   ?? throw new Exception("Недопустимое значение имени вершины назначения при истине (null).");
            this.vertexTo_IfFalse   = vertexTo_IfFalse  ?? throw new Exception("Недопустимое значение имени вершины назначения при лжи (null).");
        }
    }

    public class Algorithm
    {
        /////////////////////////////////////Поля класса/////////////////////////////////////
        private OperatorVertex      startOperator;    //Начальный оператор.
        private OperatorVertex      finalOperator;    //Конечный оператор.
        private OperatorVertex[]    aOperator;        //Массив операторных вершин алгоритма.
        private ConditionVertex[]   aCondition;       //Массив условных вершин алгоритма.



        /////////////////////////////////////Свойства класса/////////////////////////////////////
        public OperatorVertex StartOperator 
        { 
            get
            {
                return startOperator;
            }
        }
        public OperatorVertex FinalOperator
        {
            get
            {
                return finalOperator;
            }
        }
        public int NumberOperator
        {
            get
            {
                return aOperator.Length;
            }
        }
        public int NumberCondition
        {
            get
            {
                return aCondition.Length;
            }
        }



        /////////////////////////////////////Public методы/////////////////////////////////////

        /// <summary>
        /// Конструктор алгоритма.
        /// </summary>
        /// <param name="startOperator">Обозначение начального оператора</param>
        /// <param name="finalOperator">Обозначение конечного оператора</param>
        /// <param name="fromOperator">Набор переходов из операторов</param>
        /// <param name="fromCondition">Набор переходов из логических условий</param>
        public Algorithm(OperatorVertex[] aOperatorVertex, ConditionVertex[] aConditionVertex)
        {
            //Запись сведений о алгоритме.
            this.aOperator      = aOperatorVertex   ?? throw new ArgumentException("Недопустимое значение массива операторных вершин (null).");
            this.aCondition     = aConditionVertex  ?? throw new ArgumentException("Недопустимое значение массива условных вершин (null).");

            //Проверка корректности операторного алгоритма.
            if (hasEqualVertexName()) {
                throw new ArgumentException("Все имена вершин должны быть уникальными.");
            }
            if (!algorithmHasUnicueStartOperator()) {
                throw new ArgumentException("Алгоритм должен иметь единственный начальный оператор.");
            }
            if (!algorithmHasUnicueFinalOperator()) {
                throw new ArgumentException("Алгоритм должен иметь единственный конечный оператор.");
            }
            if (startOperatorHasInput()) {
                throw new ArgumentException("К начальному оператору не должно быть перехода.");
            }
            if (!eachVertexAchievable()) {
                throw new ArgumentException("Каждая вершина должна быть достижимой от начального оператора.");
            }
            if (thereCircleCondition()) {
                throw new ArgumentException("В алгоритме не должно быть циклов, состоящих только из условных вершин.");
            }
        }


        //************************************************************************************************
        // Определение типа вершины.
        //************************************************************************************************

        /// <summary>
        /// Определение, является ли вершина стартовой.
        /// </summary>
        /// <param name="nameVertex">Имя вершины</param>
        /// <returns></returns>
        public bool isStartOperator(string nameVertex)
        {
            return (nameVertex == startOperator.Name);
        }

        /// <summary>
        /// Определение, является ли вершина конечной.
        /// </summary>
        /// <param name="nameVertex">Имя вершины</param>
        /// <returns></returns>
        public bool isFinalOperator(string nameVertex)
        {
            return (nameVertex == finalOperator.Name);
        }

        /// <summary>
        /// Определение, явялется ли вершина оператором.
        /// </summary>
        /// <param name="nameVertex">Имя вершины</param>
        /// <returns></returns>
        public bool isOperator(string nameVertex)
        {
            for (int i = 0; i < aOperator.Length; i++) {
                if (aOperator[i].Name == nameVertex) {
                    return true;
                }
            }
            return false;
        }

        /// <summary>
        /// Определение, является ли вершина условной вершиной.
        /// </summary>
        /// <param name="nameVertex">Имя вершины</param>
        /// <returns></returns>
        public bool isCondition(string nameVertex)
        {
            for (int i = 0; i < aCondition.Length; i++) {
                if (aCondition[i].Name == nameVertex) {
                    return true;
                }
            }
            return false;
        }


        //************************************************************************************************
        // Получение вершины.
        //************************************************************************************************
        
        /// <summary>
        /// Получение операторной вершины по индексу.
        /// </summary>
        /// <param name="indexVertex">Индекс вершины</param>
        /// <returns></returns>
        public OperatorVertex getOperatorVertex(int indexVertex)
        {
            return aOperator[indexVertex];
        }

        /// <summary>
        /// Получение условной вершины по индексу.
        /// </summary>
        /// <param name="indexVertex">Индекс вершины</param>
        /// <returns></returns>
        public ConditionVertex getConditionVertex(int indexVertex)
        {
            return aCondition[indexVertex];
        }

        /// <summary>
        /// Получение операторной вершины по имени.
        /// </summary>
        /// <param name="vertex">Имя вершины</param>
        /// <returns>Вершина или исключение</returns>
        public OperatorVertex getOperatorVertex(string vertex)
        {
            for (int i = 0; i < aOperator.Length; i++) {
                if (aOperator[i].Name == vertex) {
                    return aOperator[i];
                }
            }
            throw new ArgumentException("\"" + vertex + "\"" + " не является вершиной алгоритма.");
        }

        /// <summary>
        /// Получение условной вершины по имени.
        /// </summary>
        /// <param name="vertex">Имя вершины</param>
        /// <returns>Вершина или исключение</returns>
        public ConditionVertex getConditionVertex(string vertex)
        {
            for (int i = 0; i < aCondition.Length; i++) {
                if (aCondition[i].Name == vertex) {
                    return aCondition[i];
                }
            }
            throw new ArgumentException("\"" + vertex + "\"" + " не является условной вершиной алгоритма.");
        }



        /////////////////////////////////////Private методы/////////////////////////////////////

        //************************************************************************************************
        // Проверка корректности алгоритма.
        //************************************************************************************************
        private bool hasEqualVertexName()
        {
            for (int i = 0; i < aOperator.Length; i++) {
                for (int j = i + 1; j < aOperator.Length; j++) {
                    if (aOperator[i].Name == aOperator[j].Name) {
                        return true;
                    }
                }
                for (int j = 0; j < aCondition.Length; j++) {
                    if (aOperator[i].Name == aCondition[j].Name) {
                        return true;
                    }
                }
            }
            for (int i = 0; i < aCondition.Length; i++) {
                for (int j = i + 1; j < aCondition.Length; j++) {
                    if (aCondition[i].Name == aCondition[j].Name) {
                        return true;
                    }
                }
            }
            return false;
        }

        private bool algorithmHasUnicueStartOperator()
        {
            bool hasStartOperator = false;

            for (int i = 0; i < aOperator.Length; i++) {
                if (aOperator[i].IsStartOperator) {
                    if (hasStartOperator) {
                        return false;
                    }
                    hasStartOperator = true;
                    startOperator = aOperator[i];
                }
            }
            return hasStartOperator;
        }

        private bool algorithmHasUnicueFinalOperator()
        {
            bool hasFinalOperator = false;

            for (int i = 0; i < aOperator.Length; i++) {
                if (aOperator[i].VertexTo == null) {
                    if (hasFinalOperator) {
                        return false;
                    }
                    hasFinalOperator = true;
                    finalOperator = aOperator[i];
                }
            }
            return hasFinalOperator;
        }

        private bool startOperatorHasInput()
        {
            string nameStartOperator = startOperator.Name;

            for (int i = 0; i < aOperator.Length; i++) {
                if (aOperator[i].VertexTo == nameStartOperator) {
                    return true;
                }
            }
            for (int i = 0; i < aCondition.Length; i++) {
                if ((aCondition[i].VertexTo_IfTrue == nameStartOperator) || (aCondition[i].VertexTo_IfFalse == nameStartOperator)) {
                    return true;
                }
            }
            return false;
        }

        private bool eachVertexAchievable()
        {
            return true;
        }

        private bool thereCircleCondition()
        {
            //Безопасная вершина - вершина, которая не ведет к циклу.
            List<string> safeVertex = new List<string>();

            //Изначально все операторные вершины являются безопасными, а условные не безопасными.
            for (int i = 0; i < aOperator.Length; i++) {
                safeVertex.Add(aOperator[i].Name);
            }

            //Проверка путей из каждой условной вершины, чтобы сделать вывод, является ли вершина безопасной.
            for (int i = 0; i < aCondition.Length; i++) {
                //При проверке прошлых условных вершин, данная вершина может уже находиться в списке безопасных.
                if (!safeVertex.Contains(aCondition[i].Name)) {
                    List<string> path = new List<string> { aCondition[i].Name };
                    if (circleFound(ref path, aCondition[i].Name)) {
                        return true;
                    }
                }  
            }
            return false;


            /////////////////////////////////////Локальные функции/////////////////////////////////////
            //Поиск условных циклов.
            bool circleFound(ref List<string> path, string vertexFrom)
            {
                //Из каждой условной вершины проверяем два выхода.
                //Если оба выхода ведут к безопасным вершинам, значит условная вершина является безопасной
                //Иначе необходимо проверить не безопасную вершину, к которой осуществляется переход.
                //В конечном счете, идя по небезопасным вершинам, мы найдем безопасную, либо цикл.

                string vertexToIfTrue = getConditionVertex(vertexFrom).VertexTo_IfTrue;

                //Если переход по истине пока не является безопасным, то проверяем его безопасность.
                if (!safeVertex.Contains(vertexToIfTrue)) {
                    //Проверка наличия вершины назначения в пройденном пути.
                    for (int i = 0; i < path.Count(); i++) {
                        if (path[i] == vertexToIfTrue) {
                            return true;
                        }
                    }
                    //Если цикл не найден, то проверяем не безопасную вершину.
                    path.Add(vertexToIfTrue);
                    if (circleFound(ref path, vertexToIfTrue)) {
                        return true;
                    }
                }

                string vertexToIfFalse = getConditionVertex(vertexFrom).VertexTo_IfFalse;

                //Если переход по лжи пока не является безопасным, то проверяем его безопасность.
                if (!safeVertex.Contains(vertexToIfFalse)) {
                    //Проверка наличия вершины назначения в пройденном пути.
                    for (int i = 0; i < path.Count(); i++) {
                        if (path[i] == vertexToIfFalse) {
                            return true;
                        }
                    }
                    //Если цикл не найден, то проверяем не безопасную вершину.
                    path.Add(vertexToIfFalse);
                    if (circleFound(ref path, vertexToIfFalse)) {
                        return true;
                    }
                }

                //Оба перехода являются безопасными, отмечаем вершину как безопасной и возвращаемся.
                safeVertex.Add(vertexFrom);
                path.RemoveAt(path.Count - 1);
                return false;
            }
            ///////////////////////////////////////////////////////////////////////////////////////////

        }

    }
}