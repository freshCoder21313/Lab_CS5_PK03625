using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lab.Utility.NonRelated
{
    public class PopularSort //for int
    {
        private static readonly Lazy<PopularSort> instance = new Lazy<PopularSort>(() => new PopularSort());
        public static PopularSort Instance = instance.Value;
        public List<int> Bubble(List<int> dynamics)
        {
            var length = dynamics.Count();
            foreach (int i in Enumerable.Range(0, length))
            {
                foreach (int j in Enumerable.Range(0 , length - i -1))
                {
                    if (dynamics[j] > dynamics[j + 1])
                    {
                        int temp = dynamics[j];
                        dynamics[j] = dynamics[j + 1];
                        dynamics[j + 1] = temp;
                    }
                }
            }
            return dynamics;
        }
        public List<int> Insertion(List<int> ints)
        {
            foreach (int i in Enumerable.Range(1, ints.Count()))
            {
                int key = ints[i];
                int j = i - 1;
                while (j >= 0 && key < ints[j])
                {
                    ints[j + 1] = ints[j];
                    j--;
                }
                ints[j + 1] = key;
            }
            return ints;
        }
        public List<int> Selection(List<int> ints)
        {
            foreach (int i in Enumerable.Range(0, ints.Count()))
            {
                int min_idx = i;
                foreach (int j in Enumerable.Range(i + 1, ints.Count()))
                {
                    if (ints[min_idx] > ints[j])
                    {
                        min_idx = j;
                    }
                }
                int temp = ints[min_idx];
                ints[min_idx] = ints[i];
                ints[i] = temp;
            }
            return ints;
        }
        public List<int> Merge(List<int> ints)
        {
            if (ints.Count > 1)
            {
                int mid = ints.Count / 2;
                List<int> L = ints.Take(mid).ToList();
                List<int> R = ints.Skip(mid).ToList();

                Merge(L);
                Merge(R);

                int i = 0, j = 0, k = 0;
                while (i < L.Count && j < R.Count)
                {
                    if (L[i] < R[j])
                    {
                        ints[k] = L[i];
                        i++;
                    }
                    else
                    {
                        ints[k] = R[j];
                        j++;
                    }
                    k++;
                }
                // If any element was left in L
                while (i < L.Count)
                {
                    ints[k] = L[i];
                    i++;
                    k++;
                }
                // If any element was left in R
                while (j < R.Count)
                {
                    ints[k] = R[j];
                    j++;
                    k++;
                }
            }
            return ints;
        }

        public List<int> Quick(List<int> ints, int low, int high)
        {
            if (low < high)
            {
                int pi = Partition(ints, low, high - 1);
                Quick(ints, low, pi - 1);
                Quick(ints, pi + 1, high);
            }
            return ints;
        }
        public int Partition(List<int> ints, int low, int high) //Quick_sub
        {
            int pivot = ints[high];
            int i = low - 1;
            foreach (int j in Enumerable.Range(low, high))
            {
                if (ints[j] <= pivot)
                {
                    i++;
                    int temp = ints[i];
                    ints[i] = ints[j];
                    ints[j] = temp;
                }
            }
            int temp2 = ints[high];
            ints[high] = ints[i+1];
            ints[i+1] = temp2;

            return i + 1;
        }
    }
}
