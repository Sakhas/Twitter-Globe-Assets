using UnityEngine;
using System.Collections;
using UnityEngine.EventSystems;
using WPM;

public class OnHoverEffectScript : MonoBehaviour
{
    public GUIStyle labelStyle, labelStyleShadow;
    public Tweet tweet;
    public bool isMouseOver = false;

    void Awake()
    {

        GetComponent<Renderer>().material.color = new Color(29, 202, 255);
        labelStyle = new GUIStyle();
        labelStyle.alignment = TextAnchor.MiddleCenter;
        labelStyle.normal.textColor = Color.white;
        labelStyleShadow = new GUIStyle(labelStyle);
        labelStyleShadow.normal.textColor = Color.white;
    }

    private void Start()
    {
        GetComponent<Renderer>().material.color = new Color(29, 202, 255);
    }

    void OnMouseOver()
    {
        isMouseOver = true;
    }

    void OnGUI()
    {
        if(isMouseOver) {
            GetComponent<Renderer>().material.color = new Color(0, 255, 0);
            float x, y;
            string text = "User: " + tweet.userName + "\n" + tweet.text;
            Vector3 mousePos = Input.mousePosition;
            x = GUIResizer.authoredScreenWidth * (mousePos.x / Screen.width);
            y = GUIResizer.authoredScreenHeight - GUIResizer.authoredScreenHeight * (mousePos.y / Screen.height) - 60 * (Input.touchSupported ? 3 : 1); // slightly up for touch devices
            GUI.Label(new Rect(x - 1, y - 1, 0, 10), text, labelStyleShadow);
        }
    }

    void OnMouseExit()
    {
        isMouseOver = false;
        GetComponent<Renderer>().material.color = new Color(29, 202, 255);
    }

}
