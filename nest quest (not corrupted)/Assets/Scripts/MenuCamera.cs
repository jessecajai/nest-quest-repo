using UnityEngine;

public class MenuCamera : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    void Update() 
    {
    transform.position += new Vector3(0, 0, Mathf.Sin(Time.time * 0.2f) * 0.001f);
    }

}
