using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BG_NearMove : MonoBehaviour
{
    /* �ʿ��� ���
    1. ����� ������ �ӵ�
    2. ����� ������ ����
    3. �̹����� ��ġ����
    4. �̹����� �̵��� ��ġ
    */
    private Transform tr = null; //�ʿ� ���۳�Ʈ (����� ���ÿ� �ʱ�ȭ -> �����ϰ� �ʱ�ȭ�� ���ߴٰ� ���� ����� �װ� ����)

    private float Speed = 10f; //�ӵ�
    private float x; //����
    private Vector2 offset = Vector2.zero; //����� ���ÿ� �ʱ�ȭ
    void Start()
    {
        tr = GetComponent<Transform>();
    }
    void Update()
    {
        tr.Translate(Vector2.left * Speed * Time.deltaTime); //������ �������� �ʴ� �͵��� translate�� �����δ�. (�� -> ������ ������)

        if (tr.position.x <= -61f)
        {
            tr.position = new Vector3(-19.4f, tr.position.y, tr.position.z); //�̹����� x��ǥ�� -63�� �Ǹ� �ٽ� 17.5�� �ǵ�����.
        }
    }
}
