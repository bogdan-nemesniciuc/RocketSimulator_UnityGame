using System;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SocialPlatforms.Impl;
public class Lander : MonoBehaviour
{


    public static Lander Instace { get; private set; }



    public event EventHandler OnUpForce;
    public event EventHandler OnRightForce;
    public event EventHandler OnLeftForce;


    public event EventHandler OnBeforeForce;

    public event EventHandler OnCoinPickup;

    public event EventHandler<OnLandedEventArgs> OnLanded;

    public class OnLandedEventArgs : EventArgs
    {
        public LandingType landingType;
        public int score;
        public float dotVector;
        public float landingSpeed;
        public float scoreMultiplier;
    }
    public enum LandingType
    {
        Succes,
        WrongLandingArea,
        TooSteepAngle,
        TooFastLanding,
    }

    private Rigidbody2D landerRigidBody2D;
    private float fuelAmount ;
    private float fuelAmountMax = 10f;


    private void Awake()
    {
        fuelAmount = fuelAmountMax;
        Instace = this;
      landerRigidBody2D =  GetComponent<Rigidbody2D>();
        //Debug.Log(Vector2.Dot(new Vector2(0, 1), new Vector2(0, 1)));
        //Debug.Log(Vector2.Dot(new Vector2(0, 1), new Vector2(.5f, .5f)));
        //Debug.Log(Vector2.Dot(new Vector2(0, 1), new Vector2(1,0)));
        //Debug.Log(Vector2.Dot(new Vector2(1, 0), new Vector2(-1, 0)));
    }

    private void FixedUpdate()
    {   
        OnBeforeForce?.Invoke(this , EventArgs.Empty);

       // Debug.Log("Fuel Amount: " + fuelAmount);
        if(fuelAmount<=0f)
        {
            //no fuel
            return;
        }


        if(Keyboard.current.upArrowKey.isPressed ||
            Keyboard.current.leftArrowKey.isPressed ||
            Keyboard.current.rightArrowKey.isPressed)
        {
            //Pressing any input will consume fuel
            ConsumeFuel();
        }
        
        //Time.deltaTime - num of seconds since the last frame.
        if (Keyboard.current.upArrowKey.isPressed)
        {
            float force = 700f;
            landerRigidBody2D.AddForce(force * transform.up * Time.deltaTime);


             OnUpForce?.Invoke(this, EventArgs.Empty);
        }

        if (Keyboard.current.leftArrowKey.isPressed)
        { //torque force - forta de cuplu , o forta de rotatie
            float turnSpeed = +100f;
            landerRigidBody2D.AddTorque(turnSpeed * Time.deltaTime);
            OnLeftForce?.Invoke(this, EventArgs.Empty);
        }

        if (Keyboard.current.rightArrowKey.isPressed)
        {
            float turnSpeed = -100f;
            landerRigidBody2D.AddTorque(turnSpeed * Time.deltaTime);
            OnRightForce?.Invoke(this, EventArgs.Empty);
        }
    }

    private void OnCollisionEnter2D(Collision2D collision2D)
    {

        if(!collision2D.gameObject.TryGetComponent(out LandingPad landingPad))
        {
            Debug.Log("Crashed on the Terrain!");
            OnLanded?.Invoke(this, new OnLandedEventArgs
            {
                landingType = LandingType.WrongLandingArea,
                dotVector = 0f,
                landingSpeed = 0f,
                scoreMultiplier = 0,
                score = 0
            });
            return;
        }

        //if(collision2D.gameObject.TryGetComponent(out Terrain terrain))
        //{
        //    Debug.Log("Terrain");
        //}

        Debug.Log(collision2D.relativeVelocity.magnitude);
        //velocity - vector magnitude - the scalar length of the vector
        //velocityMagnitude - the speed of the gameObject
        float softLandingVelocityMagnitude = 4f;
        float relativeVelocityMagnitude = collision2D.relativeVelocity.magnitude;
        
        if (relativeVelocityMagnitude > softLandingVelocityMagnitude)
        {
            Debug.Log("Landed too hard!");
            OnLanded?.Invoke(this, new OnLandedEventArgs
            {
                landingType = LandingType.TooFastLanding,
                dotVector = 0f,
                landingSpeed = relativeVelocityMagnitude,
                scoreMultiplier = 0,
                score = 0
            });
            return;
        }


        float dotVector = Vector2.Dot(Vector2.up, transform.up);
        Debug.Log("Vectors are pointing in the direction " + dotVector);
        //vector2.up - (0,1) - the world
        //tranform.up - the object , the green line 
        //the angle between them
        float minDotVector = .90f;
        if(dotVector < minDotVector)
        {
            //Aterizare cu unghi abrupt
            Debug.Log("Landed on a too steep angle");
            OnLanded?.Invoke(this, new OnLandedEventArgs
            {
                landingType = LandingType.TooSteepAngle,
                dotVector = dotVector,
                landingSpeed = relativeVelocityMagnitude,
                scoreMultiplier = 0,
                score = 0
            });
            return;
        }


        Debug.Log("Successful landing!");

        float maxScoreAmountLandingAngle = 100;
        float scoreDotVectorMultiplier = 10f;
        float landingAngleScore =maxScoreAmountLandingAngle -  Mathf.Abs(dotVector - 1f) * scoreDotVectorMultiplier * maxScoreAmountLandingAngle ;

        float maxScoreAmountLandingSpeed = 100;
        float landingSpeedScore = (softLandingVelocityMagnitude - relativeVelocityMagnitude) * maxScoreAmountLandingSpeed;


        Debug.Log("landingAngleScore: " + landingAngleScore);
        Debug.Log("landingSpeedScore: " + landingSpeedScore);

        int score = Mathf.RoundToInt ( (landingAngleScore + landingSpeedScore) * landingPad.GetScoreMultiplier() );

        Debug.Log("Landing score: " + score);
        OnLanded?.Invoke(this, new OnLandedEventArgs
        {
            landingType = LandingType.Succes,
            dotVector = dotVector,
            landingSpeed = relativeVelocityMagnitude,
            scoreMultiplier = landingPad.GetScoreMultiplier(),
            score = score
        });

    }
    private void OnTriggerEnter2D(Collider2D collider2D)
    {
        if(collider2D.gameObject.TryGetComponent(out FuelPickup fuelPickup))
        {
            float addFuelAmount = 10f;
            fuelAmount += addFuelAmount;

            if(fuelAmount> fuelAmountMax)
            {
                fuelAmount = fuelAmountMax;
            }

            fuelPickup.DestroySelf();
        }
        if (collider2D.gameObject.TryGetComponent(out CoinPickup coinPickup))
        {
            OnCoinPickup?.Invoke(this,EventArgs.Empty);
            coinPickup.DestroySelf();
        }
    }

    private void ConsumeFuel()
    {
        float fuelConsumptionAmount = 1f;
        fuelAmount -= fuelConsumptionAmount * Time.deltaTime;
    }


    public float GetSpeedX()
    {
        return landerRigidBody2D.linearVelocityX;
    }

    public float GetSpeedY()
    {
        return landerRigidBody2D.linearVelocityY;
    }

    public float GetFuel()
    {
        return fuelAmount;
    }

    public float GetFuelAmountNormalized()
    {
        return fuelAmount / fuelAmountMax;
    }
}
