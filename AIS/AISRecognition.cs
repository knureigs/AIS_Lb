using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AIS
{
    class AISRecognition
    {
        /// <summary>
        /// Популяция антител (без учета клеток памяти).
        /// </summary>
        Antibody[] Population;

        /// <summary>
        /// Размер популяции антител (без учета клеток памяти).
        /// </summary>
        public int PopulationAbSize { get; private set; }

        /// <summary>
        /// Размер популяции клеток памяти.
        /// </summary>
        public int PopulationMCellSize { get; private set; }

        /// <summary>
        /// Популяция клонов.
        /// </summary>
        Antibody[] clonePopulation;

        /// <summary>
        /// Популяция клеток памяти.
        /// </summary>
        MemoryCell[] memCells;

        /// <summary>
        /// Текущий номер поколения ИИС.
        /// </summary>
        int genNumber = 0;

        public AISRecognition(int populationAbSize, int memCellsPopulationSize)
        {
            this.PopulationAbSize = populationAbSize;
            this.Population = new Antibody[populationAbSize];
            for (int i = 0; i < populationAbSize; i++)
            {
                //this.Population[i] = new Antibody();
                this.Population[i] = new Antibody(i);
            }

            this.PopulationMCellSize = memCellsPopulationSize;
            memCells = new MemoryCell[memCellsPopulationSize];
        }

        /// <summary>
        /// Обучение ИИС.
        /// </summary>
        /// <param name="trainingSet">Обучающая выборка.</param>
        /// <param name="accuracy">Требуемая точность, от 0 до 1.</param>
        public void Training(Antigen[] trainingSet, double accuracy)
        {
            int currentAgNumber = 0;
            while (currentAgNumber < trainingSet.Length)
            {
                while (true)
                {
                    SetPopulationAffinity(trainingSet[currentAgNumber]);
                    Population = BubbleSort(Population);

                    CloneBest(4);
                    Mutation(30);
                    CloneReplace(trainingSet[currentAgNumber], currentAgNumber);
                    // Edit

                    Console.WriteLine("Ag: " + currentAgNumber + " Generation: " + genNumber + " MemCellAff: " + memCells[currentAgNumber].Affinnity.ToString("F2"));
                    if (memCells[currentAgNumber].Affinnity >= accuracy)
                    {
                        break;
                    }

                    genNumber++;
                }
                currentAgNumber++;
            }

            // показать популяцию клеток памяти, сформированную в результате обучения
            foreach (MemoryCell mc in memCells)
            {
                mc.ShowCell();
            }
        }

        /// <summary>
        /// Оператор клонирования и замены.
        /// </summary>
        /// <param name="ag">Текущий антиген из обучающей выборки.</param>
        /// <param name="currentAgNumber">Номер антигена из обучающей выборки.</param>
        private void CloneReplace(Antigen ag, int currentAgNumber)
        {
            for (int i = 0; i < clonePopulation.Count(); i++)
            {
                clonePopulation[i].SetAffinnity(ag);
            }
            clonePopulation = BubbleSort(clonePopulation);
            Antibody bestAb = clonePopulation[0].Affinnity > Population[0].Affinnity ? clonePopulation[0] : Population[0];

            if (memCells[currentAgNumber] != null)
            {
                if (bestAb.Affinnity > memCells[currentAgNumber].Affinnity)
                {
                    memCells[currentAgNumber] = new MemoryCell(bestAb, ag.RecognizedNumber);
                }
            }
            else
            {
                memCells[currentAgNumber] = new MemoryCell(bestAb, ag.RecognizedNumber);
            }
        }

        /// <summary>
        /// Оператор мутации.
        /// </summary>
        /// <param name="probability">Вероятность мутации, от 1 до 100.</param>
        private void Mutation(int probability)
        {
            for (int i = 0; i < clonePopulation.Count(); i++)
            {
                clonePopulation[i].Mutate(probability);// по клоналгу вообще чуть сложнее.
            }
        }

        /// <summary>
        /// Клонирование антител с лучшей аффинностью.
        /// </summary>
        /// <param name="amount"></param>
        private void CloneBest(int amount)
        {
            List<Antibody> clones = new List<Antibody>();
            for (int i = 0; i < amount; i++)
            {
                int cloneAmount = Population.Length / (i+1);
                for (int j = 0; j < cloneAmount; j++)
                {
                    Antibody someClone = Population[i].Clone();
                    clones.Add(someClone);
                }
            }
            clonePopulation = clones.ToArray();
        }
        
        /// <summary>
        /// Сортировка популяции антител методом пузырька.
        /// </summary>
        /// <param name="mas">Сортируемый набор антител.</param>
        /// <returns>Отсортированный набор антител.</returns>
        private Antibody[] BubbleSort(Antibody[] mas)
        {
            Antibody temp;
            for (int i = 0; i < mas.Length - 1; i++)
            {
                bool f = false;// already sorted
                for (int j = 0; j < mas.Length - i - 1; j++)
                {
                    if (mas[j + 1].Affinnity > mas[j].Affinnity)
                    {
                        f = true;
                        temp = mas[j + 1];
                        mas[j + 1] = mas[j];
                        mas[j] = temp;
                    }
                }
                if (!f)
                    break;
            }
            return mas;
        }

        /// <summary>
        /// Определение аффинностей всех антител в популяции.
        /// </summary>
        /// <param name="antigen">Антиген, аффинность которого вычисляется.</param>
        private void SetPopulationAffinity(Antigen antigen)
        {
            for (int i = 0; i < Population.Length; i++)
            {
                Population[i].SetAffinnity(antigen);
            }
        }

        /// <summary>
        /// Получение результата распознавания.
        /// </summary>
        /// <param name="testAg">Проверяемый антиген.</param>
        public void GetResult(Antigen testAg)
        {
            int? res = 0;
            double currentmaxAffinity = 0;
            foreach (MemoryCell memcell in memCells)
            {
                double aff = memcell.GetAffinnity(testAg);
                if (aff > currentmaxAffinity)
                {
                    res = memcell.RecognizedNumber;
                    currentmaxAffinity = aff;
                }
            }
            Console.WriteLine("Result: " + res);
            Console.Read();
        }
    }
}
