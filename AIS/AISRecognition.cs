using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AIS
{
    /// <summary>
    /// Класс, для описания ИИС, решающей задачу распознавания.
    /// </summary>
    class AISRecognition
    {
        /// <summary>
        /// Популяция антител (без учета клеток памяти).
        /// </summary>
        private Antibody[] abPopulation;

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
        private Antibody[] clonePopulation;

        /// <summary>
        /// Популяция клеток памяти.
        /// </summary>
        private MemoryCell[] memCellsPopulation;

        /// <summary>
        /// Текущий номер поколения ИИС.
        /// </summary>
        private int genNumber = 0;

        /// <summary>
        /// Текущий антиген, с которым работает ИИС.
        /// </summary>
        private Antigen currentAg;

        /// <summary>
        /// Номер текущего антигена из обучающей выборки, с которым работает ИИС.
        /// </summary>
        private int currentAgNumber;

        /// <summary>
        /// Создание ИИС с заданным размером популяции антител и числом клеток памяти.
        /// </summary>
        /// <param name="populationAbSize">Размер популяции антител.</param>
        /// <param name="memCellsPopulationSize">Размер популяции клеток памяти.</param>
        public AISRecognition(int populationAbSize, int memCellsPopulationSize)
        {
            this.PopulationAbSize = populationAbSize;
            this.abPopulation = new Antibody[populationAbSize];
            for (int i = 0; i < populationAbSize; i++)
            {
                //this.Population[i] = new Antibody();
                this.abPopulation[i] = new Antibody(i);
            }

            this.PopulationMCellSize = memCellsPopulationSize;
            memCellsPopulation = new MemoryCell[memCellsPopulationSize];
        }

        /// <summary>
        /// Обучение ИИС.
        /// </summary>
        /// <param name="trainingSet">Обучающая выборка.</param>
        /// <param name="accuracy">Требуемая точность, от 0 до 1.</param>
        /// <param name="selectionForCloningAmount">Число лучших антител, отбираемых для клонирования.</param>
        /// <param name="mutationProbability">Максимальная вероятность мутации.</param>
        public void Training(Antigen[] trainingSet, double accuracy, int selectionForCloningAmount, int mutationProbability)
        {
            this.currentAgNumber = 0;
            while (this.currentAgNumber < trainingSet.Length)
            {
                this.currentAg = trainingSet[this.currentAgNumber];
                while (true)
                {
                    SetPopulationAffinity();
                    abPopulation = BubbleSort(abPopulation);

                    CloneBest(selectionForCloningAmount);
                    Mutation(mutationProbability);
                    CloneReplace();
                    Edit();

                    Console.WriteLine("TreningAgNumber: {0}  Generation: {1} MemCellAff: {2}", this.currentAgNumber, genNumber, memCellsPopulation[this.currentAgNumber].Affinnity.ToString("F2"));
                    if (memCellsPopulation[this.currentAgNumber].Affinnity >= accuracy)
                    {
                        break;
                    }

                    genNumber++;
                }
                currentAgNumber++;
            }

            // показать популяцию клеток памяти, сформированную в результате обучения
            foreach (MemoryCell mc in memCellsPopulation)
            {
                mc.ShowCell();
            }
        }

        /// <summary>
        /// Редактирование популяции.
        /// </summary>
        /// <remarks>Для разнообразия пытаемся добавить случайное антитело, если его аффинность лучше чем у худшего антитела в популяции - заменяем.</remarks>
        private void Edit()
        {
            Antibody someAb = new Antibody();
            someAb.SetAffinnity(this.currentAg);
            if(someAb.Affinnity>abPopulation[PopulationAbSize-1].Affinnity)
            {
                abPopulation[PopulationAbSize - 1] = someAb;
            }
        }

        /// <summary>
        /// Оператор клонирования и замены.
        /// </summary>
        private void CloneReplace()
        {
            for (int i = 0; i < clonePopulation.Count(); i++)
            {
                clonePopulation[i].SetAffinnity(this.currentAg);
            }
            clonePopulation = BubbleSort(clonePopulation);
            Antibody bestAb = clonePopulation[0].Affinnity > abPopulation[0].Affinnity ? clonePopulation[0] : abPopulation[0];

            if (memCellsPopulation[this.currentAgNumber] != null)
            {
                if (bestAb.Affinnity > memCellsPopulation[this.currentAgNumber].Affinnity)
                {
                    memCellsPopulation[this.currentAgNumber] = new MemoryCell(bestAb, this.currentAg.RecognizedNumber);
                }
            }
            else
            {
                memCellsPopulation[this.currentAgNumber] = new MemoryCell(bestAb, this.currentAg.RecognizedNumber);
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
                int cloneAmount = abPopulation.Length / (i+1);
                for (int j = 0; j < cloneAmount; j++)
                {
                    Antibody someClone = abPopulation[i].Clone();
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
        private void SetPopulationAffinity()
        {
            for (int i = 0; i < abPopulation.Length; i++)
            {
                abPopulation[i].SetAffinnity(this.currentAg);
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
            foreach (MemoryCell memcell in memCellsPopulation)
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
