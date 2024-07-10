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
    IEnumerator Start() //이것도 void 스타트 처럼 초기에 자동 호출 된다. 
    {
        Farcollider2D = GetComponent<BoxCollider2D>();
        Width = Farcollider2D.size.x + 20.0f; //배경 이미지의 x축 사이즈를 알 수 있다.
        tr = GetComponent<Transform>();
        Speed = 10.0f;
        yield return null; //한프레임을 쉰다. (자동 호출후 한프레임 쉬고 다음 동작 구현가능)
        StartCoroutine(BackgroundLoop());
    }
    private IEnumerator BackgroundLoop()
    {
        while (GameManager.instance.IsGameover == false) //게임 오버 전까지 계속 반복
        {
            tr.Translate(Vector3.left * Speed * Time.deltaTime);
            if(tr.position.x <= -Width) //해당 좌표값에 가면 이미지 뒤로 돌리기
            {
                RePosition();
            }
            yield return new WaitForSeconds(0.002f);
        }
    }

    void RePosition()
    {
        Vector2 offset = new Vector3(Width * 2.0f, 0f, tr.position.z); //Vector2에 Vector3정보 담기 가능
        //이미지의 2배 길이, 이미지 높이, 이미지 z좌표를 저장
        tr.position = (Vector2)tr.position + offset;
    }
}
