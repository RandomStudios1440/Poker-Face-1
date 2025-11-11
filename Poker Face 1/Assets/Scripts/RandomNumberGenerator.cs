using UnityEngine;

public class RandomNumberGenerator : MonoBehaviour
{
    public float RadNum = 0f;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        RadNum = Random.Range(1, 64);
        Debug.Log(RadNum);
    }
}
