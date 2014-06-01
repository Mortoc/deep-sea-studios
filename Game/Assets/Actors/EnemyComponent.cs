using System;
using System.Collections.Generic;
using UnityEngine;

public class EnemyComponent : MonoBehaviour
{
    private Dictionary<int, Collider> mInstanceIdToCollider = new Dictionary<int, Collider>();
    public Dictionary<int, Collider> Triggers
    {
        get { return mInstanceIdToCollider; }
    }

    private void Awake()
    {
        Debug.Log("added component");
    }

    //private void OnCollisionEnter(Collision collision)
    //{
    //    Debug.Log("collision: " + collision.gameObject.name);
    //    GameObject hitGameObject = collision.collider.gameObject;

    //    if(!mInstanceIdToCollider.ContainsKey(hitGameObject.GetInstanceID()))
    //    {
    //        mInstanceIdToCollider.Add(hitGameObject.GetInstanceID(), collision.collider);
    //    }
    //}

    //private void OnCollisionExit(Collision collision)
    //{
    //    mInstanceIdToCollider.Remove(collision.collider.gameObject.GetInstanceID());
    //}

    private void OnTriggerEnter(Collider collidingObject)
    {

        //Debug.Log("this game id: " + this.gameObject.GetInstanceID());
        //Debug.Log("this parent id: " + this.gameObject.transform.parent.GetInstanceID());
        //Debug.Log("this parent GO id: " + this.gameObject.transform.parent.gameObject.GetInstanceID());

        GameObject hitGameObject = collidingObject.gameObject;

        if (!mInstanceIdToCollider.ContainsKey(hitGameObject.GetInstanceID()) &&
            hitGameObject.GetInstanceID() != this.gameObject.transform.parent.gameObject.GetInstanceID())
        {
            Debug.Log("new trigger: " + collidingObject.gameObject.name);
            mInstanceIdToCollider.Add(hitGameObject.GetInstanceID(), collidingObject);
        }
    }

    private void OnTriggerExit(Collider collidingObject)
    {
        mInstanceIdToCollider.Remove(collidingObject.gameObject.GetInstanceID());
    }
}

