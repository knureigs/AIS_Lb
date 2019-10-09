using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace IVS_Lb_1
{
    class AISRecognition
    {
        /// <summary>
        /// Популяция антител.
        /// </summary>
        Antibody[] Population;
        Antibody[] clonePopulation;
        MemoryCell[] memCells;


        int genNumber = 0;

        public AISRecognition(int populationSize, int memCellsPopulationSize)
        {
            // создали популяцию антител, случайных
            this.Population = new Antibody[populationSize];
            for (int i = 0; i < populationSize; i++)
            {
                this.Population[i] = new Antibody();
            }
            memCells = new MemoryCell[memCellsPopulationSize];
        }

        public void Training(Antigen[] trainingSet, double accuracy)
        {
            int currentAgNumber = 0;
            while (currentAgNumber < trainingSet.Length)
            {
                while (true)
                {
                    SetPopulationAffinity(trainingSet[currentAgNumber]);
                    SortPopulation();

                    CloneBest(4);
                    Mutation(30);
                    CloneReplace(trainingSet[currentAgNumber], currentAgNumber);
                    // Edit

                    
                    Console.WriteLine("Ag: " + currentAgNumber + " Generation: " + genNumber + " MaxAff: " + Population[0].Affinnity);
                    if (memCells[currentAgNumber].Affinnity >= accuracy)
                    {
                        break;
                    }

                    genNumber++;
                }
                currentAgNumber++;
            }
        }

        private void CloneReplace(Antigen ag, int currentAgNumber)
        {
            for (int i = 0; i < clonePopulation.Count(); i++)
            {
                clonePopulation[i].SetAffinnity(ag);
            }
            clonePopulation = BubbleSort(clonePopulation);
            Antibody bestAb = clonePopulation[0].Affinnity > Population[0].Affinnity ? clonePopulation[0] : Population[0];
            memCells[currentAgNumber] = new MemoryCell(bestAb, ag.RecognizedNumber);
        }

        private void Mutation(int probability)
        {
            for (int i = 0; i < clonePopulation.Count(); i++)
            {
                clonePopulation[i].Mutate(probability);// по клоналгу вообще чуть сложнее.
            }
        }

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

        private void SortPopulation()
        {
            Population = BubbleSort(Population);
        }
        private Antibody[] BubbleSort(Antibody[] mas)
        {
            Antibody temp;
            for (int i = 0; i < mas.Length - 1; i++)
            {
                bool f = false;
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

        private void SetPopulationAffinity(Antigen antigen)
        {
            for (int i = 0; i < Population.Length; i++)
            {
                Population[i].SetAffinnity(antigen);
            }
        }
        internal void GetResult(Antigen testAg)
        {
            int? res = 0;
            foreach (MemoryCell memcell in memCells)
            {
                double currentmaxAffinity = 0;
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

        // клонируем все антитела, имеющую аффинность выше средней, и с ними же сравниваем клонов - более успешные клоны заменят своего "оригинала".
        //private void CloneMutateEdit(int probability, Antigen antigen)
        //{
        //    //List<Antibody> masClones = new List<Antibody>();
        //    for (int i = 0; i < Population.Length; i++)
        //    {
        //        if (Population[i].Affinnity >= avgAff)
        //        {
        //            Antibody someClone = Population[i].Clone();
        //            someClone.Mutate(probability);
        //            someClone.SetAffinnity(antigen);
        //            if (someClone.Affinnity > Population[i].Affinnity)
        //                Population[i] = someClone;
        //        }
        //    }
        //}

        //double avgAff = 0;

        // определяем аффинность всех антител одному антмигену, находим среднюю аффинность
        //private void SetPopulationAffinity(Antigen antigen)
        //{
        //    double maxAffinnity = 0;
        //    double minAffinnity = double.MaxValue;
        //    for (int i = 0; i < Population.Length; i++)
        //    {
        //        Population[i].SetAffinnity(antigen);
        //        if (maxAffinnity <= Population[i].Affinnity)
        //            maxAffinnity = Population[i].Affinnity;
        //        if (minAffinnity > Population[i].Affinnity)
        //            minAffinnity = Population[i].Affinnity;
        //    }
        //    avgAff = (maxAffinnity + minAffinnity) / 2;
        //}


    }
}
