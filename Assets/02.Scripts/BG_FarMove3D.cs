using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class NewBehaviourScript : MonoBehaviour
{
    /* 필요한 요소
    1. 배경이 움직일 속도
    2. 배경이 움직일 방향
    3. 메쉬렌더러 안에 머터리얼 이미지
    */
    private MeshRenderer mesh; //이미지

    private float Speed = 0.2f; //속도
    private float x; //방향

    void Start()
    {
        mesh = GetComponent<MeshRenderer>();
    }
    void Update()
    {
        x += Speed * Time.deltaTime;
        mesh.material.mainTextureOffset =  new Vector2 (x, 0f); //머터리얼에 이미지의 정보를 가져오고 위치, 방향을 변화 시킨다.
        //이미지가 x축 양의 방향으로 움직이므로 카메라 입장에서는 이미지가 왼쪽으로 가는 것처럼 보인다.
    }
}
