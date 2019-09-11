using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class JumpPlayer : MonoBehaviour
{
    [SerializeField]
    private bool isGrounded;

    private Rigidbody rb;

    private void Awake()
    {
        FindObjectOfType<InputPortSerie>().onButtonJumpEvent += Jump_onButtonJumpEvent;
    }

    private void Jump_onButtonJumpEvent(object sender, InputPortSerie.OnButtonJumpEventArgs e)
    {
        if (e.Jump == true && isGrounded == true)
        {

            rb.AddForce(new Vector3(0, 5, 0), ForceMode.Impulse);
            isGrounded = false;

        }
    }

    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    void OnCollisionStay()
    {
        isGrounded = true;
    }


}