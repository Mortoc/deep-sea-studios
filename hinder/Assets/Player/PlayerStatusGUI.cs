using UnityEngine;
using UnityEngine.UI;

using System.Collections;

public class PlayerStatusGUI : MonoBehaviour 
{
	[SerializeField]
	public ControllerManager.PlayerNumber playerNum;

	
	[SerializeField]
	private Image _background;
	
	[SerializeField]
	private Image _healthImage;
	
	[SerializeField]
	private Text _playerStatus;


	void Start()
	{
		ResetGui();
	}

	private void ResetGui()
	{
		SetTextToPlayerName();
		SetHealthPercent(0.0f);
	}

	private void SetTextToPlayerName()
	{
		_playerStatus.text = "Player " + ((int)playerNum + 1);
	}

	private void SetHealthPercent(float percent)
	{
		var healthRect = _healthImage.GetComponent<RectTransform>();

		_healthImage.rectTransform.SetSizeWithCurrentAnchors
		(
			RectTransform.Axis.Horizontal, 
		    (1.0f - percent) * 200.0f
		);
	}


	public void UpdateHealth(float hitpoints, float damage)
	{
		SetHealthPercent(damage / hitpoints);
	}

	public void SetRespawnTimer(float seconds)
	{
		StartCoroutine(CountDownRespawn(seconds));
	}

	private IEnumerator CountDownRespawn(float seconds)
	{
		for(var time = 0.0f; time < seconds; time += Time.deltaTime)
		{
			_playerStatus.text = "Respawning in " + (seconds - time).ToString ("f1");
			yield return 0;
		}
		ResetGui();
	}
}
