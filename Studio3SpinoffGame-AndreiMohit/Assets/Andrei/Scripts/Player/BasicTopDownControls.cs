using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEditor.Timeline.TimelinePlaybackControls;

public class BasicTopDownControls : MonoBehaviour
{
    public float speed = 5.0f;
    public float mouseSensitivity = 0.5f;
    public float depth;

    [SerializeField] Animator animator;
    [SerializeField] GameObject gun;
    [SerializeField] GameObject bullet;

    Rigidbody rb;
    Rigidbody bulletRb;
    bool startMoving = false;
    bool shooting = false;

    private void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        rb = GetComponent<Rigidbody>();
        gun.SetActive(false);
        //animator = GetComponent<Animator>();
    }

    void Update()
    {
        MoveAndRotate();

        if(Input.GetMouseButtonDown(0) & !shooting)
        {
            shooting = true;
            StartCoroutine(Shoot());
        }
        
    }

    void MoveAndRotate()
    {
        float moveHorizontal = Input.GetAxis("Horizontal");
        float moveVertical = Input.GetAxis("Vertical");

        Vector3 movement = transform.right * moveHorizontal + transform.forward * moveVertical;
        Vector3 speedVector = new Vector3(movement.x, rb.velocity.y, movement.z);

        rb.velocity = speedVector.normalized * speed;

        float horizontalInput = Input.GetAxis("Mouse X");
        transform.Rotate(0, horizontalInput * mouseSensitivity * Time.deltaTime, 0);


        if (rb.velocity.magnitude < 0.5f & startMoving)
        {
            startMoving = false;
            animator.SetBool("Run", false);
        }
        else if (rb.velocity.magnitude > 0.6f & !startMoving)
        {
            startMoving = true;
            animator.SetBool("Run", true);
        }
    }

    void ShootProjectile()
    {
        GameObject go = Instantiate(bullet,gun.transform.position, Quaternion.identity);
        bulletRb = go.GetComponent<Rigidbody>();
        bulletRb.useGravity = false;
        bulletRb.velocity = (transform.forward  - transform.right) * 30f;
    }

    IEnumerator Shoot()
    {
        speed = 0f;
        animator.SetBool("Shoot", true);
        gun.SetActive(true);


        yield return new WaitForSeconds(1.5f);
        ShootProjectile();
        yield return new WaitForSeconds(1.5f);

        speed = 5f;
        animator.SetBool("Shoot", false);
        shooting = false;
        gun.SetActive(false);
    }
}
