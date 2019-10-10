using System;

namespace AIS
{
    /// <summary>
    /// Абстрактный класс для всех клеток - антигенов и антител, описываемых двумерным двоичным массивом.
    /// </summary>
    public abstract class Cell
    {
        /// <summary>
        /// Двумерный массив, описывающий антитело или антиген. По сути - этакий "генотип".
        /// </summary>
        public bool[,] Pixels;

        /// <summary>
        /// Размерность по горизонтали массива, описывающего антитело или антиген.
        /// </summary>
        public const int DemensionX = 4;

        /// <summary>
        /// Размерность по вертикали массива, описывающего антитело или антиген.
        /// </summary>
        public const int DemensionY = 3;

        /// <summary>
        /// Число пикселей изображения, описываемого клеткой.
        /// </summary>
        public const int PixelCount = 12;

        /// <summary>
        /// Вывод в консоль данных о клетке.
        /// </summary>
        public virtual void ShowCell()
        {
            int xDemension = this.Pixels.GetLength(0);
            int yDemension = this.Pixels.GetLength(1);
            for (int i = 0; i < xDemension; i++)
            {
                for (int j = 0; j < yDemension; j++)
                    Console.Write(Convert.ToInt32(this.Pixels[i, j]));
                Console.WriteLine();
            }
            Console.WriteLine();
        }
    }
}
