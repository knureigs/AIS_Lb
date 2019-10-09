using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IVS_Lb_1
{
    class Program
    {
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
                ShowAntigen(ag);
            }

            AISRecognition ais = new AISRecognition(6, 3);
            ais.Training(trainingSet, (Cell.PixelCount - 1) / Cell.PixelCount);

            Console.WriteLine();

            // тестовый антиген, потом вместо него будет функция для запроса от пользователя
            Antigen[] testAg = {
                new Antigen(new bool[,] { { false, true, false }, { true, true, false }, { false, true, false }, { false, true, false } }, 1),
                new Antigen(new bool[,] { { true, false, true }, { true, true, true }, { false, false, true }, { false, false, true }  }, 4),
                new Antigen(new bool[,] { { true, false, true }, { true, true, true }, { false, true, false }, { false, true, false }  }, 4),
                new Antigen(new bool[,] { { true, true, true }, { true, false, true }, { false, false, true }, { false, false, true } }, 7),
                new Antigen(new bool[,] { { true, true, true }, { false, false, true }, { false, false, true }, { false, false, true } }, 7)
            };
            foreach (Antigen ag in testAg)
            {
                ShowAntigen(ag);
                ais.GetResult(ag);
                Console.WriteLine();
                Console.WriteLine();
            }
        }

        private static void ShowAntigen(Antigen antigen)
        {
            Console.WriteLine("Antigen " + antigen.RecognizedNumber + ":");

            int xDemension = antigen.Pixels.GetLength(0);
            int yDemension = antigen.Pixels.GetLength(1);
            for (int i = 0; i < xDemension; i++)
            {
                for (int j = 0; j < yDemension; j++)
                    Console.Write(Convert.ToInt32(antigen.Pixels[i, j]));
                Console.WriteLine();
            }
            Console.WriteLine();
        }
    }    
}
