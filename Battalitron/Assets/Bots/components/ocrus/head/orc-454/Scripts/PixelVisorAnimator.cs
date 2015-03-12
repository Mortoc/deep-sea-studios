using UnityEngine;
using System.Collections;


namespace DSS.Bots.Components
{
	public class PixelVisorAnimator : MonoBehaviour 
	{
		[SerializeField]
		private Material _pixels;

		[SerializeField]
		private Texture _happy;
		
		[SerializeField]
		private Texture _sad;
		
		[SerializeField]
		private Texture _angry;
		
		[SerializeField]
		private Texture _blink;

		[SerializeField]
		private float _blinkFrequency = 8.0f;
		
		[SerializeField]
		private float _blinkTimingJitter = 3.0f;
		
		[SerializeField]
		private float _blinkLength = 0.15f;

		private Coroutine _blinking;
		private Texture _current;

		void Start()
		{
			Sad();
		}

		private void SetEmotionTexture(Texture texture)
		{
			_current = texture;
			_pixels.SetTexture ("_EmissionMap", texture);
				
			if( _blinking != null ) StopCoroutine(_blinking);

			_blinking = StartCoroutine(AnimateBlinks());
		}

		public void Happy()
		{
			SetEmotionTexture(_happy);
		}
		
		public void Angry()
		{
			SetEmotionTexture(_angry);
		}
		
		public void Sad()
		{
			SetEmotionTexture(_sad);
		}

		private IEnumerator AnimateBlinks()
		{
			yield return new WaitForSeconds( _blinkFrequency );

			while(gameObject) 
			{
				_pixels.SetTexture ("_EmissionMap", _blink);
				yield return new WaitForSeconds( _blinkLength );
				_pixels.SetTexture ("_EmissionMap", _current);

				
				yield return new WaitForSeconds
				( 
	                Mathf.Lerp
					(
						_blinkFrequency - _blinkTimingJitter, 
						_blinkFrequency + _blinkTimingJitter, 
						Random.value 
					)
				);
			}
		}
	}
}