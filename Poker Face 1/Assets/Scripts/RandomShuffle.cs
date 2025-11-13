using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class RandomGenerator : MonoBehaviour
{
    public List<int> shuffleList = new List<int>();
    public int listRange;
    public int shuffletimes;
    private void Start()
    {
        for (int i = 0; i < listRange; i++)
        {
            shuffleList.Add(i + 1);
        }
    }
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            ShuffleListRandomly();
        }
    }

    void ShuffleListRandomly()
    {
        for(int i = 0;i < shuffleList.Count;i++)
        {
            for (int s= 0; i < shuffleList.Count; s++)
            {
                int r = (int)(Random.value * (shuffleList[i] - s));
                int tempValue = shuffleList[r];
                shuffleList[r] = shuffleList[s];
                shuffleList[s] = tempValue;
            }
        }
    }
}
 