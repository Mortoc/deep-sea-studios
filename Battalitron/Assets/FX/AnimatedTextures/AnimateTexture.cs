using UnityEngine;
using System.Collections;

public class AnimateTexture : MonoBehaviour 
{
    [SerializeField]
    private Vector2 _animateSpeed = Vector2.one;
    private Vector2 _currentOffset = Vector2.zero;

    [SerializeField]
    private Material _material;

	void Update ()
    {
        if (_material)
        {
            _currentOffset += _animateSpeed * Time.deltaTime;
            _material.SetTextureOffset("_MainTex", _currentOffset);
        }
	
	}
}
