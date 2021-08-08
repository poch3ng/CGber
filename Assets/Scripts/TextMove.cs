using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using MLAPI;

public class TextMove : MonoBehaviour
{
    RectTransform M_RectTransform;

    private void Start()
    {
        M_RectTransform = GetComponent<RectTransform>();
    }
    void Update()
    {
        Vector2 NewPos = M_RectTransform.position;
        NewPos.x += -200 * Time.deltaTime;
        M_RectTransform.position = NewPos;

        float distance = -500 - gameObject.GetComponent<Text>().preferredWidth;
        if (NewPos.x < distance)
        {
            NetworkManager.Destroy(gameObject);
        }
    }
}
