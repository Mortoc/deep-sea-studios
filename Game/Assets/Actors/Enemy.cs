using System;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : ActorBase
{

    public class SmoothedVector
    {
        private struct Sample
        {
            public Vector3 Position { get; set; }
            public float Time { get; set; }
        }

        private float _interval;
        private List<Sample> _samples;

        public SmoothedVector(float interval)
        {
            // Initialize the list to our best guess for the number of samples
            _samples = new List<Sample>();
            _interval = interval;
        }

        public void AddSample(Vector3 position)
        {
            _samples.Add(new Sample() { Position = position, Time = Time.time });

            float sampleTimeout = Time.time - _interval;
            for (; _samples[0].Time < sampleTimeout; )
            {
                _samples.RemoveAt(0);
            }
        }

        public bool HasSamples
        {
            get { return _samples.Count > 0; }
        }

        public Vector3 GetSmoothedVector()
        {
            float recpCount = 1.0f / (float)_samples.Count;
            Vector3 avg = Vector3.zero;

            foreach (Sample sample in _samples)
                avg += sample.Position;

            return avg * recpCount;
        }
    }

    private static int mEnemyIdCounter = 1;
    private int mId = 0;

    private float mMovementSpeed = 0.05f;
    private float mTurnSpeed = 1.5f;
    private SmoothedVector mMoveDir = new SmoothedVector(5.0f);

    public Enemy(Vector3 initialPosition)
        : base(initialPosition)
    {
    }

    protected override void LoadModel(Vector3 initialPosition)
    {
        //mGameObject = GameObject.CreatePrimitive(PrimitiveType.Sphere);
        mGameObject = (GameObject)(GameObject.Instantiate(Resources.Load("Monster")));
        mGameObject.transform.position = initialPosition;
        mGameObject.transform.localScale = Vector3.one;

        var anim = mGameObject.GetComponentInChildren<Animation>();
        if (anim)
        {
            anim.Play();
        }

        mId = mEnemyIdCounter++;
        mGameObject.name = "enemy " + mId;

        Rigidbody rigidbody = mGameObject.AddComponent<Rigidbody>();
        rigidbody.useGravity = false;
        rigidbody.constraints = RigidbodyConstraints.FreezeRotationZ;
        rigidbody.detectCollisions = false;

        mGameObject.rigidbody.velocity = mGameObject.transform.forward;

        mGameObject.AddComponent<CharacterController>();
        
        mMoveVector = mGameObject.transform.forward.normalized * mMovementSpeed;
    }

    private bool mUpdateRandomDirection = true;
    private Vector3 mRandomRotation = Vector3.zero;
    private float mWanderInDirectionSeconds = 0.0f;
    private DateTime mPickedNewWanderDirectionTime = DateTime.Now;

    private enum CollisionState
    {
        NoCollision,
        CurrentlyColliding,
        NewCollision
    }

    private CollisionState mPreviousCollisionState = CollisionState.NoCollision;
    private CollisionState mCurrentCollisionState = CollisionState.NoCollision;
    Vector3 mInverseDirection = Vector3.zero;
    Vector3 mInitialDirection = Vector3.zero;
    Vector3 mLateralForceDirection = Vector3.zero;
    Vector3 mMoveVector = Vector3.zero;

    protected override void ActorUpdate()
    {
        //if the enemy is colliding with something, update the forward vector away from the collision until it's no longer colliding

        //Debug.DrawRay(  mGameObject.transform.position,
        //                mGameObject.transform.forward * 200.0f,
        //                Color.blue);
        //Debug.DrawRay(mGameObject.transform.position,
        //                mMoveVector * 200.0f,
        //                Color.yellow);

        RaycastHit[] hitInfoArray = Physics.RaycastAll(mGameObject.transform.position,
                                                       mMoveVector.normalized,
                                                       200.0f);
        
        RaycastHit hitInfo = new RaycastHit();
        Collider hitCollider = null;
        if (hitInfoArray.Length > 0)
        {           
            for (int i = 0; i < hitInfoArray.Length; ++i)
            {
                //Debug.Log("collider name: " + hitInfoArray[i].collider.gameObject.name);
                if ((hitInfoArray[i].collider.gameObject.GetInstanceID() != mGameObject.collider.gameObject.GetInstanceID()))
                {
                    hitInfo = hitInfoArray[i];
                    hitCollider = hitInfoArray[i].collider;

                    if (mPreviousCollisionState == CollisionState.NoCollision)
                    {
                        mCurrentCollisionState = CollisionState.NewCollision;
                    }
                    else if (mPreviousCollisionState == CollisionState.NewCollision || mPreviousCollisionState == CollisionState.CurrentlyColliding)
                    {
                        mCurrentCollisionState = CollisionState.CurrentlyColliding;
                    }
                    break;
                }
            }
        }
        if (hitCollider == null)
        {
            mCurrentCollisionState = CollisionState.NoCollision;
        }

        //process collisions
        if (mCurrentCollisionState == CollisionState.NewCollision)
        {
            //Debug.Log("new collision: " + hitCollider.gameObject.name);

            mInitialDirection = mGameObject.transform.forward;
            mInverseDirection = Vector3.Cross(mGameObject.transform.forward, Vector3.up);

            mLateralForceDirection = hitInfo.normal;
            //Debug.LogError("current velocity: " + mGameObject.rigidbody.velocity);
            //Debug.Break();
        }
        else if (mCurrentCollisionState == CollisionState.CurrentlyColliding)
        {
            mMoveVector = mLateralForceDirection.normalized * mMovementSpeed;

            //Debug.Log("still colliding with: " + hitCollider.gameObject.name +
            //            " lerping - from: " + mInitialDirection +
            //            " to: " + mLateralForceDirection +
            //            " curr: " + mMoveVector);

            //Debug.DrawRay(mGameObject.transform.position,
            //              mMoveVector * 100.0f,
            //              Color.green);
            //Debug.DrawRay(hitInfo.point,
            //                hitInfo.normal* 100.0f,
            //                Color.red);
        }
        else if(mCurrentCollisionState == CollisionState.NoCollision)
        {
            //Debug.Log("no collisions");
            
            double elapsedSeconds = (DateTime.Now - mPickedNewWanderDirectionTime).TotalSeconds;
            if (elapsedSeconds >= mWanderInDirectionSeconds)
            {
                mUpdateRandomDirection = true;
                mWanderInDirectionSeconds = UnityEngine.Random.Range(2.0f, 4.0f);
                mPickedNewWanderDirectionTime = DateTime.Now;
            }

            if (mUpdateRandomDirection || mRandomRotation == Vector3.zero)
            {
                mRandomRotation = new Vector3(  UnityEngine.Random.Range(-0.25f, 0.25f),
                                                UnityEngine.Random.Range(-0.25f, 0.25f),
                                                UnityEngine.Random.Range(-0.25f, 0.25f));
                mUpdateRandomDirection = false;

                //Debug.Log("new random rotation: " + mRandomRotation);
                mMoveVector = mRandomRotation.normalized * mMovementSpeed;
            }
        }

        mMoveDir.AddSample(mMoveVector);
        Vector3 moveDir = mMoveDir.GetSmoothedVector();

        mGameObject.GetComponent<CharacterController>().Move(moveDir);
        mGameObject.rigidbody.rotation = Quaternion.Lerp(Quaternion.LookRotation(mGameObject.transform.forward.normalized),
                                                    Quaternion.LookRotation(moveDir.normalized),
                                                    Time.deltaTime * mTurnSpeed);

        mPreviousCollisionState = mCurrentCollisionState;
    }
}

