using UnityEngine;
using UnityEngine.UI;

using System;
using System.Collections;
using System.Collections.Generic;

using ExitGames.Client.Photon.LoadBalancing;


public class GameController : MonoBehaviour
{
	public HexMap _map;

	public Canvas _joinGameCanvas;

	public Text _gameInfoText;

	public InputField _usernameInput;
	public TurnManager TurnManager { get; private set; }

	public GameObject _localPlayerPrefab;
	private LocalPlayer _localPlayer;
	public LocalPlayer LocalPlayer
	{
		get { return _localPlayer; }
	}

	public Image _existingGamesListRoot;

	public bool PlayerHasCompletedTurn { get; private set; }

	public GameObject _foreignPlayerPrefab;
	private Dictionary<int, ForeignPlayer> _foreignPlayers = new Dictionary<int, ForeignPlayer>();

	void Awake()
	{	
		_gameInfoText.text = "";
		Application.runInBackground = true;
		CustomTypes.Register();

		TurnManager = new TurnManager(this, _map);
		
		TurnManager.PlayerName = "Player Loading";

		GameListUpdate();

		if( PlayerPrefs.HasKey("USERNAME") ) 
		{
			_usernameInput.text = PlayerPrefs.GetString("USERNAME");
		}

		StartCoroutine (EnableJoinGUIOnConnected());
	}

	void OnEnable()
	{
		TurnManager.OnStateChangeAction += StateChanged;
		TurnManager.OnTurnComplete += OnTurnComplete;
		TurnManager.OnGamePopulatedAndReady += SpawnLocalPlayer;
	}

	void OnDisable()
	{
		TurnManager.OnStateChangeAction -= StateChanged;
		TurnManager.OnTurnComplete -= OnTurnComplete;
		TurnManager.OnGamePopulatedAndReady -= SpawnLocalPlayer;
	}

	IEnumerator EnableJoinGUIOnConnected()
	{
		yield return 0;

		Growl.ShowMessage 
		(
			"Connecting to Server...", 
			() => TurnManager.IsConnectedAndReady
		);
		_joinGameCanvas.gameObject.SetActive(false);

		while( !TurnManager.IsConnectedAndReady )
		{
			yield return 0;
		}

		_joinGameCanvas.gameObject.SetActive(true);
	}

	private void StateChanged(ClientState newState)
	{
	}

	public void Update()
	{
		TurnManager.Service();
	}

	public void GameListUpdate()
	{
		Debug.Log(string.Format("GameListUpdate() Saved Games: {0}", TurnManager.SavedGames.Count));

		if (TurnManager.SavedGames.Count == 0)
		{
			_existingGamesListRoot.gameObject.SetActive(false);
			return;
		}
//		
//		this.MainMenuInfo.Front.Text = string.Format("{0}\n{1} saves", TurnManager.PlayerName, TurnManager.SavedGames.Count);
//		this.MainMenuInfo.ToFront();
//		
//		int saveGameCount = TurnManager.SavedGames.Count;
//		
//		int buttonNr = 0;// apply to button 0 and up
//		int lastSaveGameBtn = TurnManager.Length - 1;
//		bool moreSavesThanButtons = (saveGameCount > TurnManager.Length);
//		
//		for (int saveGameIndex = savegameListStartIndex; saveGameIndex < saveGameCount; saveGameIndex++)
//		{
//			if (buttonNr == lastSaveGameBtn && moreSavesThanButtons)
//			{
//				break;
//			}
//			
//			SaveGameInfo saveGame = TurnManager.SavedGames[saveGameIndex];  // save to access this by index, as the for-loop only goes up to < saveGameCount
//			this.ReapplyButton(this.LoadGameButtons[buttonNr++], saveGame.DisplayName + ((saveGame.MyTurn)?"\nyour turn":"\nwaiting"), "LoadGameMsg", new object[] { saveGame.RoomName, saveGame.MyPlayerId });
//		}
//		
//		if (moreSavesThanButtons)
//		{
//			this.ReapplyButton(this.LoadGameButtons[lastSaveGameBtn], ">>", "PageSaveGameListMsg", null);
//		}
	}

	public void JoinGame()
	{
		if( _usernameInput.text.Length == 0 )
		{
			Growl.ShowError("Invalid User Name");
			return;
		}

		PlayerPrefs.SetString("USERNAME", _usernameInput.text);

		TurnManager.PlayerName = _usernameInput.text;

		if( !TurnManager.OpJoinRandomRoom(null, 0) ) 
		{
			TurnManager.CreateTurnbasedRoom();
		}

		Destroy(_joinGameCanvas.gameObject);
	}

	private void OnTurnComplete()
	{
		if( TurnManager.CurrentRoom.Players.Count != 2 )
		{
			_gameInfoText.text = "";
		}
		else if( TurnManager.IsMyTurn )
		{
			_gameInfoText.text = "Your Turn";
			_localPlayer.TakeTurn(TurnManager);
		}
		else
		{
			_gameInfoText.text = string.Format ("{0}'s Turn", TurnManager.Opponent.Name);
		}
	}

	private void SpawnLocalPlayer()
	{
		var localPlayerObj = (GameObject)Instantiate(_localPlayerPrefab);
		_localPlayer = localPlayerObj.GetComponent<LocalPlayer>();
		_localPlayer.Map = _map;

		var startHex = _map.GetRandomValidHex();

		_localPlayer.SetInitialPosition(startHex.i, startHex.j);

		Camera.main.transform.parent = _localPlayer.transform;
		Camera.main.transform.localPosition = new Vector3(0.0f, -4.0f, -5.0f);

		if( TurnManager.IsMyTurn )
		{
			_localPlayer.TakeTurn(TurnManager);
		}
	}

	public void UpdateForeignPlayers(List<ForeignPlayerInfo> playerInfos)
	{
		foreach(var playerInfo in playerInfos)
		{
			ForeignPlayer foreignPlayer;
			if(_foreignPlayers.TryGetValue(playerInfo.PlayerId, out foreignPlayer))
			{
				foreignPlayer.UpdatePosition(playerInfo.i, playerInfo.j);
			}
			else
			{
				var newPlayer = (GameObject)Instantiate (_foreignPlayerPrefab);
				foreignPlayer = newPlayer.GetComponent<ForeignPlayer>();
				foreignPlayer.Map = _map;
				newPlayer.transform.position = _map.HexIdxToPos(playerInfo.i, playerInfo.j);
				_foreignPlayers[playerInfo.PlayerId] = foreignPlayer;
			}
		}
	}

	public void OnApplicationQuit()
	{
		if( TurnManager.loadBalancingPeer != null ) 
		{
			TurnManager.loadBalancingPeer.StopThread();
		}
		TurnManager.Disconnect();
	}
}
