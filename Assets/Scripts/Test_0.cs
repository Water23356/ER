using UnityEngine;

public class Test_0 : MonoBehaviour
{
    [SerializeField]
    private float speed = 30;

    private void Start()
    {
        Debug.Log("你好呀");
    }

    private Vector3 moveDir;

    private void Update()
    {
        float x = Input.GetAxis("Horizontal");
        float y = Input.GetAxis("Vertical");

        transform.position += new Vector3(x, y, 0) * speed * Time.deltaTime;
    }
}