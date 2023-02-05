using UnityEngine;
using System.Collections;

public static class Probability
{
    public static bool IsOccurred (float percentage)
    {
        int prob = System.Convert.ToInt32(percentage * 100f);
        int i = Random.Range(0, 10000);
        if (i < prob)
            return true;
        return false;
    }

    public static bool IsOccurred(int percentage)
    {
        int i = Random.Range(0, 100);
        if (i < percentage)
            return true;
        return false;
    }

    //Use this method with caution due to float - decimal conversion. Can cause unexpected results
    public static int WhichOccurred(params float[] probabilities)
    {
        decimal[] convertedArray = new decimal[probabilities.Length];

        for(int i = 0; i < probabilities.Length; i++)
        {
            convertedArray[i] = (decimal)probabilities[i];
        }

        return WhichOccurred(convertedArray);
    }

    public static int WhichOccurred(decimal[] probabilities)
    {
        float n = 0;
        int[] intProbabilities = new int[probabilities.Length];

        for (int i = 0; i < probabilities.Length; i++)
        {
            //Debug.Log(probabilities[i]);
            intProbabilities[i] = System.Convert.ToInt32(probabilities[i] * 100);
            n += intProbabilities[i];
        }
        //Debug.Log(n);
        //if (n != 10000f)
        //{
        //    Debug.LogError("Valori probabilità errati");
        //}

        int randomNumber = Random.Range(0, (int)n);
        int minRange = 0;
        bool find = false;
        int choose = 0;
        for(int i = 0; i < probabilities.Length && !find; i++)
        {
            if (intProbabilities[i] > 0)
            {
                int maxRange = minRange + intProbabilities[i];
                if (randomNumber >= minRange && randomNumber < maxRange)
                {
                    choose = i;
                    find = true;
                }
                minRange = maxRange;
            }
        }

        if(!find)
            Debug.LogError("Error Probability!");

        return choose;
    }


}
