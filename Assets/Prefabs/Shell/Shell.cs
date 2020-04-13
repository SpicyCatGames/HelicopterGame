using UnityEngine;
using System.Collections;

public class Shell : MonoBehaviour
{
    [SerializeField] private float speed = 2;
    [SerializeField] private float lifeTime = 10;
    void FixedUpdate()
    {
        transform.position += transform.TransformDirection(Vector2.up) * speed * Time.deltaTime;
    }
    private void Update()
    {
        lifeTime -= Time.deltaTime;
        if (lifeTime < 0)
        {
            Destroy(this.gameObject);
        }
    }
}
