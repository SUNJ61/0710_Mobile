using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BG_NearMove : MonoBehaviour
{
    /* 필요한 요소
    1. 배경이 움직일 속도
    2. 배경이 움직일 방향
    3. 이미지의 위치정보
    4. 이미지가 이동한 위치
    */
    private Transform tr = null; //필요 컴퍼넌트 (선언과 동시에 초기화 -> 선언만하고 초기화를 안했다고 가끔 경고날림 그거 예방)

    private float Speed = 10f; //속도
    private float x; //방향
    private Vector2 offset = Vector2.zero; //선언과 동시에 초기화
    void Start()
    {
        tr = GetComponent<Transform>();
    }
    void Update()
    {
        tr.Translate(Vector2.left * Speed * Time.deltaTime); //빠르게 움직이지 않는 것들은 translate로 움직인다. (우 -> 왼으로 움직임)

        if (tr.position.x <= -61f)
        {
            tr.position = new Vector3(-19.4f, tr.position.y, tr.position.z); //이미지의 x좌표가 -63이 되면 다시 17.5로 되돌린다.
        }
    }
}
