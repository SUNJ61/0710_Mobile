using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AsteroidCtrl : MonoBehaviour
{
    private Transform tr;
    private float Speed = 0f;

    public GameObject Hit_effect;

    void Start()
    {
        Speed = Random.Range(10.0f, 15f);
        tr = GetComponent<Transform>();
    }
    void Update()
    {
        tr.Translate(Vector3.left * Speed * Time.deltaTime);
        if (tr.position.x <= -15.0f)
        {
            Destroy(gameObject);
        }
    }

    private void OnTriggerEnter2D(Collider2D col)
    {
        if(col.gameObject.tag == "COIN")
        {
            Destroy(this.gameObject);
            Destroy(col.gameObject);
            GameObject eff = Instantiate(Hit_effect, new Vector3(tr.position.x, tr.position.y, -3), Quaternion.identity);
            SoundManager.Instance.HitAsteroid();
            Destroy(eff, 0.5f);
            
        }
    }
}
