using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class BG_FarMove2D : MonoBehaviour
{
    private Transform tr;
    private BoxCollider2D Farcollider2D;

    private float Speed;
    private float Width;
    IEnumerator Start() //�̰͵� void ��ŸƮ ó�� �ʱ⿡ �ڵ� ȣ�� �ȴ�. 
    {
        Farcollider2D = GetComponent<BoxCollider2D>();
        Width = Farcollider2D.size.x + 20.0f; //��� �̹����� x�� ����� �� �� �ִ�.
        tr = GetComponent<Transform>();
        Speed = 10.0f;
        yield return null; //���������� ����. (�ڵ� ȣ���� �������� ���� ���� ���� ��������)
        StartCoroutine(BackgroundLoop());
    }
    private IEnumerator BackgroundLoop()
    {
        while (GameManager.instance.IsGameover == false) //���� ���� ������ ��� �ݺ�
        {
            tr.Translate(Vector3.left * Speed * Time.deltaTime);
            if(tr.position.x <= -Width) //�ش� ��ǥ���� ���� �̹��� �ڷ� ������
            {
                RePosition();
            }
            yield return new WaitForSeconds(0.002f);
        }
    }

    void RePosition()
    {
        Vector2 offset = new Vector3(Width * 2.0f, 0f, tr.position.z); //Vector2�� Vector3���� ��� ����
        //�̹����� 2�� ����, �̹��� ����, �̹��� z��ǥ�� ����
        tr.position = (Vector2)tr.position + offset;
    }
}
