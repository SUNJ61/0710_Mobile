using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    /* �ʿ��� ���
    1. ������ ������Ʈ ������
    2. ������ �ð� ����
    3. ������ ũ�� ���� ->������ ��ĳ��
    4. ������ y�� ���� ��ġ ���� ->������ ��ġ����
    */
    public static GameManager instance; //�̱��� ���, ���� �����ϱ� ����
    public GameObject AsteroidPrefab;

    private float timePrev;
    private float Yposmin = -3.0f;
    private float Yposmax = 3.5f;
    private float ScaleMin =1.0f;
    private float ScaleMax =2.5f;

    private Vector3 PosCamera; //�ʱ� ī�޶��� ��ġ�� ������ ����
    private float HitBeginTime; //������ ��̶� �ε�ģ �ð��� �����ϴ� ����
    private bool IsHit = false; //������ ��̶� �ε��ƴ��� �ƴ��� �Ǵ��ϴ� ����

    public bool IsGameover = false; //���� ������ �����ϴ� ����
    void Start()
    {
        timePrev = Time.time;
        instance = this;
    }
    void Update()
    {
        if(Time.time - timePrev > 1.5f)
        {
            SpawnAsteroid();
        }
        if(IsHit)
        {
            HitCamera();
        }
    }

    private void HitCamera()
    {
        float x = Random.Range(-0.05f, 0.05f);
        float y = Random.Range(-0.05f, 0.05f);
        Camera.main.transform.position += new Vector3(x, y, 0f); //�ε�ģ ���� �������� �̾� ������ ��� ���Ͽ� ī�޶� ���.

        if (Time.time - HitBeginTime > 0.3f) //��� �浹 �� 0.3�ʰ� ������ IsHit false, ī�޶� ����ġ
        {
            IsHit = false;
            Camera.main.transform.position = PosCamera;
        }
    }

    void SpawnAsteroid()
    {
        timePrev = Time.time;
        float RandomYpos = Random.Range(Yposmin, Yposmax);
        float AsScale = Random.Range(ScaleMin,ScaleMax);
        AsteroidPrefab.transform.localScale = Vector3.one * AsScale;
        Instantiate(AsteroidPrefab,new Vector3(15.0f, RandomYpos, AsteroidPrefab.transform.position.z),Quaternion.identity);
    }

    public void TurnOn()
    {
        IsHit = true;
        PosCamera = Camera.main.transform.position; //��鸮���� ī�޶� ��ġ�� ����
        HitBeginTime = Time.time;
    }
}
