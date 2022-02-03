using UnityEngine;

public class Player : MonoBehaviour
{
    // Start is called before the first frame update
    public float MovementSpeed = 1;
    public float JumpForce = 1;

    public Rigidbody2D _rigidbody;

    void Start()
    {
        _rigidbody = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.X))
        {
            float distance = Vector2.Distance(GameObject.Find("Monster").transform.position,
                transform.position);

            if (distance <= 0.5f)
            {
                GameObject.Find("Monster").SendMessage("Attacked");
            }
        }
        var movement = Input.GetAxis("Horizontal");
        transform.position += new Vector3(movement, 0, 0) * Time.deltaTime * MovementSpeed;

        if (Input.GetButtonDown("Jump") && Mathf.Abs(_rigidbody.velocity.y) < 0.001f)
        {
            _rigidbody.AddForce(new Vector2(0, JumpForce), ForceMode2D.Impulse);
        }
    }
}
