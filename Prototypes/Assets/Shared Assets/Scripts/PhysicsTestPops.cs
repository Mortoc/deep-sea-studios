using UnityEngine;

using System;
using System.Collections;
using System.Collections.Generic;

using Rand = UnityEngine.Random;

namespace DSS
{
	public class PhysicsTestPops : MonoBehaviour
	{
		[SerializeField]
		private float _minTimeBetweenPops = 1.0f;

		[SerializeField]
		private float _maxTimeBetweenPops = 5.0f;
		
		[SerializeField]
		private float _popPower = 15.0f;
		
		IEnumerator Start()
		{
			var rb = GetComponent<Rigidbody>();
			
			while(gameObject)
			{
				var randTime = Mathf.Lerp(_minTimeBetweenPops, _maxTimeBetweenPops, Rand.value);
				yield return new WaitForSeconds(randTime);
				yield return new WaitForFixedUpdate();
				
				rb.AddForce(Rand.onUnitSphere * _popPower, ForceMode.Impulse);
                rb.AddTorque(Rand.onUnitSphere * _popPower, ForceMode.Impulse);
			}
		}
	}
}
