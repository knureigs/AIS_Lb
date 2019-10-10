using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AIS;

namespace IVS_Lb_1
{
    class Program
    {
        /// <summary>
        /// Размер популяции антител (без учета клеток памяти).
        /// </summary>
        private const int PopulationAbSize = 6;// 600

        /// <summary>
        /// Размер популяции клеток памяти.
        /// </summary>
        private const int PopulationMCellSize = 3;

        /// <summary>
        /// Число лучших антител, отбираемых для клонирования.
        /// </summary>
        private const int SelectionForCloningAmount = 4;

        /// <summary>
        /// Максимальная вероятность мутации.
        /// </summary>
        private const int MutationProbability = 30;

        static void Main(string[] args)
        {
            // описали антигены, составляющие обучающую выборку.
            Antigen[] trainingSet = new Antigen[] {
                //new Antigen(new int[,] { { 0, 1, 0 }, { 0, 1, 0 }, { 0, 1, 0 }, { 0, 1, 0 } }, 1),
                //new Antigen(new int[,] { { 1, 0, 1 }, { 1, 0, 1 }, { 1, 1, 1 }, { 0, 0, 1 } }, 4),
                //new Antigen(new int[,] { { 1, 1, 1 }, { 0, 0, 1 }, { 0, 0, 1 }, { 0, 0, 1 } }, 7)
                new Antigen(new bool[,] { { false, true, false }, { false, true, false }, { false, true, false }, { false, true, false } }, 1),
                new Antigen(new bool[,] { { true, false, true }, { true, false, true }, { true, true, true }, { false, false, true } }, 4),
                new Antigen(new bool[,] { { true, true, true }, { false, false, true }, { false, false, true }, { false, false, true } }, 7)
            };
            foreach (Antigen ag in trainingSet)
            {
                ag.ShowCell();
            }

            AISRecognition ais = new AISRecognition(PopulationAbSize, PopulationMCellSize);
            ais.Training(trainingSet, (double)(Cell.PixelCount - 1) / Cell.PixelCount, SelectionForCloningAmount, MutationProbability);

            Console.WriteLine();
            Console.WriteLine("Test set:");
            Console.WriteLine();

            // набор тестовых антигенов, потом вместо него будет функция для запроса ввода антигена от пользователя
            Antigen[] testAg = {
                new Antigen(new bool[,] { { false, true, false }, { true, true, false }, { false, true, false }, { false, true, false } }, 1),
                new Antigen(new bool[,] { { true, false, true }, { true, true, true }, { false, false, true }, { false, false, true }  }, 4),
                new Antigen(new bool[,] { { true, false, true }, { true, true, true }, { false, true, false }, { false, true, false }  }, 4),
                new Antigen(new bool[,] { { true, true, true }, { true, false, true }, { false, false, true }, { false, false, true } }, 7),
                new Antigen(new bool[,] { { true, true, true }, { false, false, true }, { false, false, true }, { false, false, true } }, 7)
            };
            foreach (Antigen ag in testAg)
            {
                ag.ShowCell();
                ais.GetResult(ag);
                Console.WriteLine();
                Console.WriteLine();
            }
        }
    }    
}
