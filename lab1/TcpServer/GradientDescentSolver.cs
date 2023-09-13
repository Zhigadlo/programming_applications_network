using Mathos.Parser;

namespace TcpServer
{
    public class GradientDescentSolver
    {
        private MathParser _parser;
        private string _expression;
        private double _xStart;
        private double _learningRate;
        private int _numIterations;
        private const double EPSILON = 0.0001;
        public GradientDescentSolver(string expression)
        {
            _parser = new MathParser();
            _expression = expression;
        }
        public GradientDescentSolver(string expression, double xStart, double learningRate, int numIterations)
        {
            _parser = new MathParser();
            _expression = expression;
            _xStart = xStart;
            _learningRate = learningRate;
            _numIterations = numIterations;
        }

        public void SetxStart(double xStart) => _xStart = xStart;
        public void SetLearningRate(double learningRate) => _learningRate = learningRate;
        public void SetNumberOfIteration(int numOfIteration) => _numIterations = numOfIteration;


        public double Solve()
        {
            double x = _xStart;
            for (int i = 0; i < _numIterations; i++)
            {
                double gradient = GetGradient(_expression, x);
                x = x - _learningRate * gradient;
            }
            return x;
        }

        /// <summary>
        /// метод для нахождения градиента методом центральной разности
        /// </summary>
        /// <param name="expression"></param>
        /// <param name="x"></param>
        /// <returns></returns>
        private double GetGradient(string expression, double x)
        {
            double f1 = GetValueOfFunction(expression, x + EPSILON);
            double f2 = GetValueOfFunction(expression, x - EPSILON);
            return (f1 - f2) / (2 * EPSILON);
        }

        private double GetValueOfFunction(string expression, double x)
        {
            _parser.LocalVariables.Clear();
            _parser.LocalVariables.Add("x", x + EPSILON);
            return _parser.Parse(expression);
        }
    }
}
