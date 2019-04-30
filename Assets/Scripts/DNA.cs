using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DNA
{
    List<int> Genes = new List<int>();
    int DNA_Length = 0;
    int Max_Values = 0;

    public DNA(int l,int v)
    {
        DNA_Length = l;
        Max_Values = v;
        SetRandom();
    }

    private void SetRandom()
    {
        Genes.Clear();
        for (int i = 0; i < DNA_Length; i++)
        {
            Genes.Add(Random.Range(-Max_Values, Max_Values));
        }
    }

    public void SetGene(int pos,int value)
    {
        Genes[pos] = value;
    }

    public int GetGene(int pos)
    {
        return Genes[pos];
    }

    public void Combine(DNA Parent1,DNA Parent2)
    {
        for (int i = 0; i < DNA_Length; i++)
        {
            Genes[i] = Random.Range(0, 10) < 5 ? Parent1.Genes[i] : Parent2.Genes[i];
        }
    }

    public void Mutate()
    {
       Genes[Random.Range(0, DNA_Length)] = Random.Range(-Max_Values, Max_Values);
    }
}
