using UnityEngine;
using System.Collections;
using System.Collections.Generic;
public class Shop : MonoBehaviour
{
    private int Money = 7887;

    private int numOfNormal = 0;

    private int costOfNormal = 100;
   
    public void buyNormal()
    {
        if(Money >= costOfNormal)
        {
            numOfNormal++;
            Money -= costOfNormal;
            print ("Normal deck Purchased. You now have " + numOfNormal.ToString()
                + "Normal and" + Money.ToString() + " Money ");
        }
        else
        {
            print("Not enough cash");
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
