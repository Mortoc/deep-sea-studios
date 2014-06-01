using System;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : ActorBase
{
    private static int mEnemyIdCounter = 1;
    private int mId = 0;

    private float mMovementSpeed = 2.0f;
    private CharacterController mCharacterController = null;
    private EnemyComponent mEnemyComponent = null;

    private GameObject mSteeringGameObject = null;

    public Enemy(Vector3 initialPosition)
        : base(initialPosition)
    {
    }

    protected override void LoadModel(Vector3 initialPosition)
    {
        mGameObject = GameObject.CreatePrimitive(PrimitiveType.Sphere);
        mGameObject.transform.position = initialPosition;
        mGameObject.transform.localScale = new Vector3(50.0f, 50.0f, 100.0f);
        mId = mEnemyIdCounter++;
        mGameObject.name = "enemy " + mId;

        Rigidbody rigidbody = mGameObject.AddComponent<Rigidbody>();
        rigidbody.useGravity = false;

        mCharacterController = mGameObject.AddComponent<CharacterController>();


        mSteeringGameObject = GameObject.CreatePrimitive(PrimitiveType.Cube);
        Rigidbody steeringRigidbody = mSteeringGameObject.AddComponent<Rigidbody>();
        steeringRigidbody.useGravity = false;
        steeringRigidbody.isKinematic = true;
        mSteeringGameObject.GetComponent<BoxCollider>().isTrigger = true;
        mEnemyComponent = mSteeringGameObject.AddComponent<EnemyComponent>();
        //GameObject.DestroyImmediate(mSteeringGameObject.GetComponent<MeshRenderer>());
        mSteeringGameObject.name = "steering object";
        mSteeringGameObject.transform.position = new Vector3(mGameObject.transform.position.x,
                                                                mGameObject.transform.position.y,
                                                                mGameObject.transform.position.z + mGameObject.transform.localScale.z * 0.5f);

        mSteeringGameObject.transform.parent = mGameObject.transform;

        mSteeringGameObject.transform.localScale = new Vector3(1.0f, 1.0f, 2.0f);
        mSteeringGameObject.transform.localPosition = new Vector3(mSteeringGameObject.transform.localPosition.x,
                                                                    mSteeringGameObject.transform.localPosition.y,
                                                                    mSteeringGameObject.transform.localPosition.z + (mSteeringGameObject.transform.localScale.z * 0.5f));

    }

    private bool mUpdateRandomDirection = true;
    private Vector3 mRandomRotation = Vector3.zero;
    private float mWanderInDirectionSeconds = 0.0f;
    private DateTime mPickedNewWanderDirectionTime = DateTime.Now;

    protected override void ActorUpdate()
    {


        Vector3 moveVector = Vector3.zero;

        //if the enemy is colliding with something, update the forward vector away from the collision until it's no longer colliding
        //if (mEnemyComponent.Triggers.Count > 0)
        //{
        RaycastHit hitInfo = new RaycastHit();
        bool didHit = Physics.CapsuleCast(  mSteeringGameObject.transform.position - new Vector3(0.0f, 0.0f, mSteeringGameObject.collider.bounds.size.z * 0.5f),
                                            mSteeringGameObject.transform.position + new Vector3(0.0f, 0.0f, mSteeringGameObject.collider.bounds.size.z * 0.5f),
                                            1.0f, mGameObject.transform.forward, out hitInfo, mSteeringGameObject.collider.bounds.size.z);



        Debug.DrawLine(mGameObject.transform.localPosition,
                       mGameObject.transform.localPosition + (mGameObject.transform.forward * 3.0f)); //(mSteeringGameObject.collider.bounds.size.z)));
            
                                    //new Vector3( mGameObject.transform.position.x,
                                    //mGameObject.transform.position.y,
                                    //mGameObject.transform.position.z + mGameObject.transform.localScale.z * 0.5f) + new Vector3(0.0f, 0.0f, 2.0f));
                        
        if (didHit)
        {
            Debug.Log("hit: " + hitInfo.collider.gameObject.name);
            Vector3 hitVector = hitInfo.point - mGameObject.transform.position;
            float angleBetweenHitPointAndForwardVector = Vector3.Angle(hitVector, mGameObject.transform.forward);

            if (Vector3.Dot(hitVector, mGameObject.transform.right) > 0.0f)
            {
                mGameObject.transform.Rotate(Vector3.Cross(mGameObject.transform.forward, mGameObject.transform.right), -1.0f);
            }
            else
            {
                mGameObject.transform.Rotate(Vector3.Cross(mGameObject.transform.forward, mGameObject.transform.right), 1.0f);
            }
        }

        //float closestDistance = float.MaxValue;
        //Collider closestCollider = null;
        //foreach (KeyValuePair<int, Collider> collider in mEnemyComponent.Triggers)
        //{
        //    Debug.Log("collision inst id: " + collider.Value.gameObject.GetInstanceID());
        //    float distanceToCollision = Vector3.Distance(collider.Value.ClosestPointOnBounds(), mGameObject.transform.position);
        //    if (closestCollider == null || closestDistance > )
        //    {

        //        closestCollider = collider.Value;
        //    }
        //}
        //}
        //otherwise meander around the scene
        else
        {
            double elapsedSeconds = (DateTime.Now - mPickedNewWanderDirectionTime).TotalSeconds;
            if (elapsedSeconds >= mWanderInDirectionSeconds)
            {
                mUpdateRandomDirection = true;
                mWanderInDirectionSeconds = UnityEngine.Random.Range(1.0f, 4.0f);
                mPickedNewWanderDirectionTime = DateTime.Now;
            }

            if (mUpdateRandomDirection || mRandomRotation == Vector3.zero)
            {
                mRandomRotation = new Vector3(UnityEngine.Random.Range(-1.0f, 1.0f),
                                                UnityEngine.Random.Range(-1.0f, 1.0f),
                                                UnityEngine.Random.Range(-1.0f, 1.0f));
                mUpdateRandomDirection = false;

                Debug.Log("new random rotation: " + mRandomRotation);
            }

            mGameObject.transform.Rotate(mRandomRotation.x, mRandomRotation.y, mRandomRotation.z);
        }

        moveVector = mGameObject.transform.forward.normalized * mMovementSpeed;
        mCharacterController.transform.forward = mGameObject.transform.forward;
        mCharacterController.Move(moveVector);
    }
}

