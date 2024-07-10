using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    /* 필요한 요소
    1. 생성할 오브젝트 프리팹
    2. 생성할 시간 간격
    3. 생성시 크기 랜덤 ->프리팹 스캐일
    4. 생성시 y축 상하 위치 랜덤 ->프리팹 위치정보
    */
    public static GameManager instance; //싱글턴 기법, 쉽게 접근하기 위해
    public GameObject AsteroidPrefab;

    private float timePrev;
    private float Yposmin = -3.0f;
    private float Yposmax = 3.5f;
    private float ScaleMin =1.0f;
    private float ScaleMax =2.5f;

    private Vector3 PosCamera; //초기 카메라의 위치를 가지는 변수
    private float HitBeginTime; //로켓이 운석이랑 부딪친 시간을 저장하는 변수
    private bool IsHit = false; //로켓이 운석이랑 부딪쳤는지 아닌지 판단하는 변수

    public bool IsGameover = false; //게임 오버를 관리하는 변수
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
        Camera.main.transform.position += new Vector3(x, y, 0f); //부딪친 동안 랜덤으로 뽑아 낸값을 계속 더하여 카메라 흔듦.

        if (Time.time - HitBeginTime > 0.3f) //운석과 충돌 후 0.3초가 지나면 IsHit false, 카메라 원위치
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
        PosCamera = Camera.main.transform.position; //흔들리기전 카메라 위치값 저장
        HitBeginTime = Time.time;
    }
}
