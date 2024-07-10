using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class BG_NearMove2D : MonoBehaviour
{
    private Transform tr;
    private BoxCollider2D box;

    private float Speed;
    private float Width;
    IEnumerator Start()
    {
        tr = GetComponent<Transform>();
        box = GetComponent<BoxCollider2D>();
        Width = box.size.x + 16f;
        Speed = 8.0f;
        yield return null;
        StartCoroutine(BackgroundLoop());
    }
    IEnumerator BackgroundLoop()
    {
        while(!GameManager.instance.IsGameover)
        {
            tr.Translate(Vector3.left * Speed * Time.deltaTime);
            if(tr.position.x <= -Width)
            {
                RePosition();
            }
            yield return new WaitForSeconds(0.002f);
        }
    }

    void RePosition()
    {
        Vector2 offset = new Vector3(Width * 2.0f, 0f, tr.position.z);
        tr.position = (Vector2)tr.position + offset;
    }
}
