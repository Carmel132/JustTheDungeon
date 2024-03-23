using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class ReticleManager : MonoBehaviour
{

    void Start()
    {
        Cursor.visible = false;
    }

    void OnGUI()
    {
        #if UNITY_EDITOR
                Cursor.visible = Input.mousePosition.x == 0 || Input.mousePosition.y == 0 || Input.mousePosition.x >= Handles.GetMainGameViewSize().x - 1 || Input.mousePosition.y >= Handles.GetMainGameViewSize().y - 1;
        #else
            Cursor.visible = Input.mousePosition.x == 0 || Input.mousePosition.y == 0 || Input.mousePosition.x >= Screen.width - 1 || Input.mousePosition.y >= Screen.height - 1;
        #endif
        transform.position = (Vector3)(Vector2)Camera.main.ScreenToWorldPoint(Input.mousePosition);
    }

}
