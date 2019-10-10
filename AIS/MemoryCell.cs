using System;

namespace AIS
{
    /// <summary>
    /// Класс для описания антител-клеток памяти.
    /// </summary>
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

        /// <summary>
        /// Конструктор.
        /// </summary>
        /// <param name="ab">Антитело, взятое за основу при создании клетки памяти.</param>
        /// <param name="RecognizedNumber">Число, распознавать которое приспособлено антитело.</param>
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

        /// <summary>
        /// Получение значения аффинности заданному антигену.
        /// </summary>
        /// <param name="ag">Антиген, с которым сопоставляется клетка памяти.</param>
        /// <returns>Значение аффинности.</returns>
        public double GetAffinnity(Antigen ag)
        {
            double aff = 0;
            int h = HemMeasure(ag);

            // определение аффинности.
            aff = (double)(PixelCount - h) / PixelCount; // получили значение аффинности от 0 (если совпало ноль позиций) до 1 (если совпали все позиции)
            return aff;
        }

        /// <inheritdoc />
        public override void ShowCell()
        {
            Console.WriteLine("MemoryCell " + this.RecognizedNumber + ":");
            base.ShowCell();
        }
    }
}
