import UnityEngine

class Spin (MonoBehaviour): 
	
	spinRate = 30.0f

	def Update ():
		transform.Rotate(Vector3.up * spinRate * Time.deltaTime)
