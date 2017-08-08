using UnityEngine;
using System.Collections;

public class HoverObject : MonoBehaviour
{
    public GameObject obj;

    // Use this for initialization
    void Start()
    {
        Instantiate(obj);
    }

    void OnMouseOver()
    {
        print("Mouse over");
    }

    void OnMouseExit()
    {
        
    }

    public void setObject(GameObject obj)
    {
        this.obj = obj;
    }

}
