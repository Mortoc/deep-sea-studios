using System;
using System.Collections;
using System.Collections.Generic;

using ExitGames.Client.Photon;
using ExitGames.Client.Photon.Lite;
using ExitGames.Client.Photon.LoadBalancing;

using UnityEngine;
using Hashtable = ExitGames.Client.Photon.Hashtable;
using Random = UnityEngine.Random;
using TBPlayer = ExitGames.Client.Photon.LoadBalancing.Player;


public class SaveGameInfo
{
	public int MyPlayerId;
	public string RoomName;
	public string DisplayName;
	public bool MyTurn;
	public Dictionary<string, object> AvailableProperties;
	
	public string ToStringFull()
	{
		return string.Format("\"{0}\"[{1}] {2} ({3})", RoomName, MyPlayerId, MyTurn, SupportClass.DictionaryToString(AvailableProperties));
	}
}

public struct ForeignPlayerInfo
{
	public int PlayerId;
	public int i;
	public int j;
}

/// <summary>The network/connection handling class of the HexaTest.</summary>
/// <remarks>
/// This game uses a fair amount of room properties to save the state plus two room properties
/// that are made available in the save-game list (and the lobby):
/// "turn"     is the id of the player who's turn is next. not necessarily "who's turn was done last".
/// "players"  is a colon-separated list of the 2 player names.

/// </remarks>
public class TurnManager : LoadBalancingClient
{	
	private const string APP_ID = "5ba08b91-1d03-49fa-88fa-e61879c70cbe";
	public TurnManager(GameController controller, HexMap map) 
		: base(null, APP_ID, "0.1") 
	{	
		ConnectToRegionMaster ("EU");
		Map = map;
		Controller = controller;
	}

	public GameController Controller { get; private set; }
	public HexMap Map { get; private set; }

	public event Action OnGamePopulatedAndReady; // Called when the game has an opponent and is ready to go
	public event Action OnTurnComplete; // Called every time a player completes their turn

	public const string PropTurn = "turn";
	public const string PropNames = "names";
	
	private const byte MaxPlayers = 2;
	
	public int TurnNumber = 1;
	
	public int PlayerIdToMakeThisTurn;  // who's turn this is. when "done", set the other player's actorNumber and save
	
	public bool IsMyTurn
	{
		get
		{
			//Debug.Log(PlayerIdToMakeThisTurn + "'s turn. You are: " + this.LocalPlayer.ID); 
			return this.PlayerIdToMakeThisTurn == this.LocalPlayer.ID;
		}
	}

	public List<SaveGameInfo> SavedGames = new List<SaveGameInfo>();

	public bool GameCanStart 
	{
		get { return this.CurrentRoom != null && this.CurrentRoom.Players.Count == 2; }
	}
			
	/// <summary>Returns the Player instance for the remote player (not LocalPlayer.ID) in a two-player game.</summary>
	/// <returns>Might be null if there is no other player yet or anymore.</returns>
	public TBPlayer Opponent
	{
		get
		{
			
			TBPlayer opp = this.LocalPlayer.GetNext();
			//Debug.Log("you: " + this.LocalPlayer.ToString() + " other: " + opp.ToString());
			return opp;
		}
	}
	
	public override void OnOperationResponse(OperationResponse operationResponse)
	{
		base.OnOperationResponse(operationResponse);
		
		switch (operationResponse.OperationCode)
		{
		case (byte)OperationCode.WebRpc:
			if (operationResponse.ReturnCode == 0)
			{
				this.OnWebRpcResponse(new WebRpcResponse(operationResponse));
			}
			break;
		case (byte)OperationCode.JoinGame:
		case (byte)OperationCode.CreateGame:
			if (operationResponse.ReturnCode != 0)
			{
				Debug.Log(string.Format("Join or Create failed for: '{2}' Code: {0} Msg: {1}", operationResponse.ReturnCode, operationResponse.DebugMessage, this.CurrentRoom));
			}
			if (this.Server == ServerConnection.GameServer)
			{
				if (operationResponse.ReturnCode == 0)
				{
					this.LoadBoardFromProperties(false);
				}
			}
			break;
		case (byte)OperationCode.JoinRandomGame:
			if (operationResponse.ReturnCode == ErrorCode.NoRandomMatchFound)
			{
				// no room found: we create one!
				this.CreateTurnbasedRoom();
			}
			break;
		}
	}
	
	
	public override void OnEvent(EventData photonEvent)
	{
		base.OnEvent(photonEvent);
		
		switch (photonEvent.Code)
		{
		case EventCode.PropertiesChanged:

			this.LoadBoardFromProperties(true);
			break;
		case EventCode.Join:
			if (this.CurrentRoom.Players.Count == 2 && this.CurrentRoom.IsOpen)
			{
				this.CurrentRoom.IsOpen = false;
				this.CurrentRoom.IsVisible = false;
				this.SavePlayersInProps();

				if( OnGamePopulatedAndReady != null )
					OnGamePopulatedAndReady();
			}
			break;
		case EventCode.Leave:
			if (this.CurrentRoom.Players.Count == 1)
			{
				this.CurrentRoom.IsOpen = true;
				this.CurrentRoom.IsVisible = true;

				Growl.ShowMessage("Opponent Left", () => this.CurrentRoom.Players.Count != 2);
			}
			break;
		}
	}
	
	public override void DebugReturn(DebugLevel level, string message)
	{
		base.DebugReturn(level, message);
		Debug.Log(message);
	}
	
	public override void OnStatusChanged(StatusCode statusCode)
	{
		base.OnStatusChanged(statusCode);
		
		switch (statusCode)
		{
		case StatusCode.Exception:
		case StatusCode.ExceptionOnReceive:
		case StatusCode.TimeoutDisconnect:
		case StatusCode.DisconnectByServer:
		case StatusCode.DisconnectByServerLogic:
			Growl.ShowError("Connection Error");
			Debug.LogError(string.Format("Error on connection level. StatusCode: {0}", statusCode));
			break;
		case StatusCode.ExceptionOnConnect:
			Growl.ShowError("Connection Error");
			Debug.LogWarning(string.Format("Exception on connection level. Is the server running? Is the address ({0}) reachable?", this.CurrentServerAddress));
			break;
		case StatusCode.Disconnect:
			this.SavedGames.Clear();
			break;
		}
	}
	
	private void OnWebRpcResponse(WebRpcResponse response)
	{
		Debug.Log(string.Format("OnWebRpcResponse. Code: {0} Content: {1}", response.ReturnCode, SupportClass.DictionaryToString(response.Parameters)));
		if (response.ReturnCode == 0)
		{
			if (response.Parameters == null)
			{
				Debug.Log("WebRpc executed ok but didn't get content back. This happens for empty save-game lists.");
				Controller.GameListUpdate();
				return;
			}
			
			if (response.Name.Equals("GetGameList"))
			{
				this.SavedGames.Clear();
				
				// the response for GetGameList contains a Room's name as Key and another Dictionary<string,object> with the values the web service sends
				foreach (KeyValuePair<string, object> pair in response.Parameters)
				{
					// per key (room name), we send 
					// "ActorNr" which is the PlayerId/ActorNumber this user had in the room
					// "Properties" which is another Dictionary<string,object> with the properties that the lobby sees
					Dictionary<string, object> roomValues = pair.Value as Dictionary<string, object>;
					
					SaveGameInfo si = new SaveGameInfo();
					si.RoomName = pair.Key;
					si.DisplayName = pair.Key;  // we might have a better display name for this room. see below.
					si.MyPlayerId = (int)roomValues["ActorNr"];
					si.AvailableProperties = roomValues["Properties"] as Dictionary<string, object>;
					
					// let's find out of it's our turn to play and if we know the opponent's name (which we will display as game name). 
					if (si.AvailableProperties != null)
					{
						// PropTurn is a value per room that gets set to the player who's turn is next.
						if (si.AvailableProperties.ContainsKey(PropTurn))
						{
							int nextPlayer = (int) si.AvailableProperties[PropTurn];
							si.MyTurn = nextPlayer == si.MyPlayerId;
						}
						
						// PropNames is set to a list of the player names. this can easily be turned into a name for the game to display
						if (si.AvailableProperties.ContainsKey(PropNames))
						{
							string display = (string)si.AvailableProperties[PropNames];
							display = display.ToLower();
							display = display.Replace(this.PlayerName.ToLower(), "");
							display = display.Replace(";", "");
							si.DisplayName = "vs. " + display;
						}
					}
					
					//Debug.Log(si.ToStringFull());
					this.SavedGames.Add(si);
				}
				Controller.GameListUpdate();
			}
		}
		
	}

	private int _localPlayerI;
	private int _localPlayerJ;
	public void UpdateLocalPlayerPos(int i, int j)
	{
		_localPlayerI = i;
		_localPlayerJ = j;
	}
	
	public void SaveBoardToProperties()
	{
		var boardProps = Map.GetBoardAsCustomProperties();
		boardProps.Add("pt", this.PlayerIdToMakeThisTurn);  // "pt" is for "player turn" and contains the ID/actorNumber of the player who's turn it is
		boardProps.Add("t#", this.TurnNumber);
		boardProps.Add("pos-" + this.LocalPlayer.ID, new int[]{_localPlayerI, _localPlayerJ});

		// our turn will be over if 2 tiles are clicked/flipped but not the same. in that case, we update the other player if inactive
		bool webForwardToPush = false;
		if (Controller.PlayerHasCompletedTurn)
		{
			TBPlayer otherPlayer = this.Opponent;
			if (otherPlayer != null)
			{
				boardProps.Add(PropTurn, otherPlayer.ID); // used to identify which player's turn the NEXT is. the WebHooks might send a PushMessage to that user.
				if (otherPlayer.IsInactive) 
				{
					webForwardToPush = true;            // this will send the props to the WebHooks, which in turn will push a message to the other player.
				}
			}
		}
		
		
		//Debug.Log(string.Format("saved board to room-props {0}", SupportClass.DictionaryToString(boardProps)));
		this.OpSetCustomPropertiesOfRoom(boardProps, webForwardToPush);
	}
	
	public void SavePlayersInProps()
	{
		if (this.CurrentRoom == null || this.CurrentRoom.CustomProperties == null || this.CurrentRoom.CustomProperties.ContainsKey(PropNames))
		{
			Debug.Log("Skipped saving names. They are already saved.");
			return;
		}
		
		Debug.Log("Saving names.");
		Hashtable boardProps = new Hashtable();
		boardProps[PropNames] = string.Format("{0};{1}", this.LocalPlayer.Name, this.Opponent.Name);
		this.OpSetCustomPropertiesOfRoom(boardProps);
	}
	
	public void LoadBoardFromProperties(bool calledByEvent)
	{
		//board.InitializeBoard();
		
		Hashtable roomProps = this.CurrentRoom.CustomProperties;
		Debug.Log(string.Format("Board Properties: {0}", SupportClass.DictionaryToString(roomProps)));
		
		if (roomProps.Count == 0)
		{
			// we are in a fresh room with no saved board.
			Map.InitializeBoard();
			this.SaveBoardToProperties();
		}

		// we are in a game that has props (a board). read those (as update or as init, depending on calledByEvent)
		Map.SetBoardByCustomProperties(roomProps);

		// we set properties "pt" (player turn) and "t#" (turn number). those props might have changed
		// it's easier to use a variable in gui, so read the latter property now
		if (this.CurrentRoom.CustomProperties.ContainsKey("t#"))
		{
			this.TurnNumber = (int) this.CurrentRoom.CustomProperties["t#"];
		}
		else
		{
			this.TurnNumber = 1;
		}

		var initialTurn = this.PlayerIdToMakeThisTurn;
		if (this.CurrentRoom.CustomProperties.ContainsKey("pt"))
		{
			this.PlayerIdToMakeThisTurn = (int) this.CurrentRoom.CustomProperties["pt"];
			Debug.Log("This turn was played by player.ID: " + this.PlayerIdToMakeThisTurn);
		}
		else
		{
			this.PlayerIdToMakeThisTurn = 0;
		}
		
		// if the game didn't save a player's turn yet (it is 0): use master
		if (this.PlayerIdToMakeThisTurn == 0)
		{
			this.PlayerIdToMakeThisTurn = this.CurrentRoom.MasterClientId;
		}

		if( initialTurn != this.PlayerIdToMakeThisTurn && OnTurnComplete != null ) 
		{
			OnTurnComplete();
		}
		
		Debug.Log ("loading foreign info");
		var foreignPlayerPositions = new List<ForeignPlayerInfo>();
		foreach(var key in this.CurrentRoom.CustomProperties.Keys)
		{
			Debug.Log ("checking key " + key);
			if( key.ToString().StartsWith("pos-") )
			{
				var playerId = int.Parse(key.ToString().Split ('-')[1]);
				if( playerId != this.LocalPlayer.ID )
				{
					var coords = (int[])this.CurrentRoom.CustomProperties[key];
					var info = new ForeignPlayerInfo() 
					{
						PlayerId = playerId,
						i = coords[0],
						j = coords[1]
					};

					foreignPlayerPositions.Add (info);
					Debug.Log ("Not player ID, storing coords: " + info.i + " " + info.j);
				}
			}
		}

		Controller.UpdateForeignPlayers(foreignPlayerPositions);
	}
		
	
	public void CreateTurnbasedRoom()
	{
		string newRoomName = string.Format("{0}-{1}", this.PlayerName, Random.Range(0,1000).ToString("D4"));    // for int, Random.Range is max-exclusive!
		//Debug.Log(string.Format("CreateTurnbasedRoom(): {0}", newRoomName));
		
		RoomOptions roomOptions = new RoomOptions()
		{
			MaxPlayers = 2,
			CustomRoomPropertiesForLobby = new string[] { PropTurn, PropNames },
			PlayerTtl = int.MaxValue,
			EmptyRoomTtl = 5000
		};
		this.OpCreateRoom(newRoomName, roomOptions, TypedLobby.Default);

		Growl.ShowMessage("Waiting for Opponent", () => CurrentRoom.Players.Count == 2);
	}

	public void HandoverTurnToNextPlayer()
	{
		var nextPlayerId = 0;
		if (this.LocalPlayer != null)
		{
			TBPlayer nextPlayer = this.LocalPlayer.GetNextFor(this.PlayerIdToMakeThisTurn);
			if (nextPlayer != null)
			{
				nextPlayerId = nextPlayer.ID;
			}
		}
		
		this.PlayerIdToMakeThisTurn = nextPlayerId;
		SaveBoardToProperties();

		if( OnTurnComplete != null ) 
		{
			OnTurnComplete();
		}
	}
}
