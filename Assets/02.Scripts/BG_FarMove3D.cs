using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class NewBehaviourScript : MonoBehaviour
{
    /* �ʿ��� ���
    1. ����� ������ �ӵ�
    2. ����� ������ ����
    3. �޽������� �ȿ� ���͸��� �̹���
    */
    private MeshRenderer mesh; //�̹���

    private float Speed = 0.2f; //�ӵ�
    private float x; //����

    void Start()
    {
        mesh = GetComponent<MeshRenderer>();
    }
    void Update()
    {
        x += Speed * Time.deltaTime;
        mesh.material.mainTextureOffset =  new Vector2 (x, 0f); //���͸��� �̹����� ������ �������� ��ġ, ������ ��ȭ ��Ų��.
        //�̹����� x�� ���� �������� �����̹Ƿ� ī�޶� ���忡���� �̹����� �������� ���� ��ó�� ���δ�.
    }
}
