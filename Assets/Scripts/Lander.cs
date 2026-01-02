using UnityEngine;
using UnityEngine.InputSystem;
public class Lander : MonoBehaviour
{

   private Rigidbody2D landerRigidBody2D;

    private void Awake()
    {
      landerRigidBody2D =  GetComponent<Rigidbody2D>();
        Debug.Log(Vector2.Dot(new Vector2(0, 1), new Vector2(0, 1)));
        Debug.Log(Vector2.Dot(new Vector2(0, 1), new Vector2(.5f, .5f)));
        Debug.Log(Vector2.Dot(new Vector2(0, 1), new Vector2(1,0)));
        Debug.Log(Vector2.Dot(new Vector2(1, 0), new Vector2(-1, 0)));
    }

    private void FixedUpdate()
    {   //Time.deltaTime - num of seconds since the last frame.
        if (Keyboard.current.upArrowKey.isPressed)
        {
            float force = 700f;
            landerRigidBody2D.AddForce(force * transform.up * Time.deltaTime);
        }

        if (Keyboard.current.leftArrowKey.isPressed)
        { //torque force - forta de cuplu , o forta de rotatie
            float turnSpeed = +100f;
            landerRigidBody2D.AddTorque(turnSpeed * Time.deltaTime);
        }

        if (Keyboard.current.rightArrowKey.isPressed)
        {
            float turnSpeed = -100f;
            landerRigidBody2D.AddTorque(turnSpeed * Time.deltaTime);
        }
    }

    private void OnCollisionEnter2D(Collision2D collision2D)
    {
        Debug.Log(collision2D.relativeVelocity.magnitude);
        float softLandingVelocityMagnitude = 4f;
        if(collision2D.relativeVelocity.magnitude > softLandingVelocityMagnitude)
        {
            Debug.Log("Landed too hard!");
            return;
        }
        Debug.Log("Successful landing!");
    }




}
