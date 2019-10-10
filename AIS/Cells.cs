﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

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

    public class MemoryCell : Antibody
    {
        /// <summary>
        /// Значение аффинности антитела антигену. Изначально 0, поскольку неизвестно.
        /// </summary>
        public int? RecognizedNumber
        {
            get;
            private set;
        }

        public MemoryCell(Antibody ab, int? RecognizedNumber)
        {
            this.RecognizedNumber = RecognizedNumber;
            this.Ag = ab.Ag;
            this.Affinnity = ab.Affinnity;
            this.Pixels = new bool[DemensionX, DemensionY];
            for (int i = 0; i < DemensionX; i++)
            {
                for (int j = 0; j < DemensionY; j++)
                {
                    Pixels[i, j] = ab.Pixels[i, j];
                }
            }
        }

        public double GetAffinnity(Antigen ag)
        {
            double aff = 0;
            int h = HemMeasure(ag);

            // определение аффинности.
            aff = (double)(PixelCount - h) / PixelCount; // получили значение аффинности от 0 (если совпало ноль позиций) до 1 (если совпали все позиции)
            return aff;
        }

        public override void ShowCell()
        {
            Console.WriteLine("MemoryCell " + this.RecognizedNumber + ":");
            base.ShowCell();
        }
    }

    /// <summary>
    /// Класс, описывающий антитело.
    /// </summary>
    public class Antibody : Cell
    {
        /// <summary>
        /// Антиген, при сопоставлении с которым определялось значение аффинности.
        /// </summary>
        public Antigen Ag;

        /// <summary>
        /// Значение аффинности антитела антигену. Изначально 0, поскольку неизвестно.
        /// </summary>
        public double Affinnity
        {
            get;
            protected set;
        }

        /// <summary>
        /// Конструктор, описывающий создание случайного антитела.
        /// </summary>
        public Antibody()
        {
            this.Pixels = new bool[DemensionX, DemensionY];

            Random rand = new Random();
            for (int i = 0; i < DemensionX; i++)
            {
                for (int j = 0; j < DemensionY; j++)
                {
                    this.Pixels[i, j] = rand.Next(0, 2) == 0 ? false : true;
                }
            }
        }

        /// <summary>
        /// Конструктор, описывающий создание случайного антитела.
        /// </summary>
        public Antibody(int seed)
        {
            this.Pixels = new bool[DemensionX, DemensionY];

            Random rand = new Random(seed);
            for (int i = 0; i < DemensionX; i++)
            {
                for (int j = 0; j < DemensionY; j++)
                {
                    this.Pixels[i, j] = rand.Next(0, 2) == 0 ? false : true;
                }
            }
        }

        /// <summary>
        /// Копирующий конструктор для антитела.
        /// </summary>
        /// <param name="ab">Антитело, с которого снимается копия.</param>
        public Antibody(Antibody ab)
        {
            this.Pixels = (bool[,])ab.Pixels.Clone();
            this.Ag = ab.Ag;
            this.Affinnity = ab.Affinnity;
        }

        /// <summary>
        /// Определение значения аффинности для антитела.
        /// </summary>
        /// <param name="ag">Антиген, с которым сравнивается антитело.</param>
        public void SetAffinnity(Antigen ag)
        {
            this.Ag = ag; // связали антитело с антигеном, с которым выполняется сравнение.
            int h = HemMeasure(ag);

            // определение аффинности.
            this.Affinnity = (double)(PixelCount - h) / PixelCount; // получили значение аффинности от 0 (если совпало ноль позиций) до 1 (если совпали все позиции)
        }

        /// <summary>
        /// Вычисление расстояния Хэмминга для булевых массивов, описывающих изображения. 
        /// </summary>
        /// <param name="ag">Антиген, описываемый булевым массивом.</param>
        /// <returns>Число расхождений между данным антителом и заданным антигеном.</returns>
        protected int HemMeasure(Antigen ag)
        {
            int h = 0; // расстояние Хемминга, можно использовать как меру близости антитела антигену.

            // антитела и антигены должны описываться массивами одинакового размера.
            if (DemensionX != ag.Pixels.GetLength(0) && DemensionY != ag.Pixels.GetLength(1))
                throw new Exception();

            // подсчет расстояния Хемминга.
            for (int i = 0; i < DemensionX; i++)
            {
                for (int j = 0; j < DemensionY; j++)
                {
                    if (this.Pixels[i, j] != ag.Pixels[i, j])
                        h++;
                }
            }

            return h;
        }

        /// <summary>
        /// Оператор клонирования антитела.
        /// </summary>
        /// <returns>Клон данного антитела, с установленной аффинностью.</returns>
        public Antibody Clone()
        {
            return new Antibody(this);
        }

        /// <summary>
        /// Мутация, выполняемая с определенной вероятностью.
        /// </summary>
        /// <param name="probability">Вероятность мутации, 0-100.</param>
        public void Mutate(int probability)
        {
            Random rand = new Random();//Random(0)

            for (int i = 0; i < DemensionX; i++)
            {
                for (int j = 0; j < DemensionY; j++)
                {
                    bool isMut = false;
                    isMut = rand.Next(0, 100) < probability; //выходит вероятность мутации для каждого элемента "генотипа", описывающего антитело.
                    if (!isMut)
                        this.Pixels[i, j] = (this.Pixels[i, j] == false) ? true : false;
                }
            }
        }
    }

    /// <summary>
    /// Класс, описывающий антиген.
    /// </summary>
    public class Antigen : Cell
    {
        /// <summary>
        /// Истинное значение распознаваемого символа. В рабочей ситуации не определено.
        /// </summary>
        public int? RecognizedNumber = null;

        public Antigen(bool[,] array, int number)
        {
            this.Pixels = (bool[,])array.Clone();
            this.RecognizedNumber = number;
        }

        public Antigen(int[,] array)
        {
            this.Pixels = (bool[,])array.Clone();
            //this.DemensionX = mas.GetLength(0);
            //this.DemensionY = mas.GetLength(1);
        }

        public override void ShowCell()
        {
            Console.WriteLine("Antigen " + this.RecognizedNumber + ":");
            base.ShowCell();
        }
    }
}