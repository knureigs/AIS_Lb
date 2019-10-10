using System;

namespace AIS
{
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
