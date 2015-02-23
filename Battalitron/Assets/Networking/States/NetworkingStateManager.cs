using UnityEngine;

using System;
using System.Collections;
using System.Collections.Generic;

using BackstreetBots.States;

namespace BackstreetBots.Networking.States
{
    public class NetworkingStateManager : GameStateManager
    {
        private PhotonNetworkingHandler _networkingHandler;
        public override void Init(string name, GameStateManager parent)
        {
 	        base.Init(name, parent);

            //PhotonNetwork.logLevel = PhotonLogLevel.Full;

            SetupPhotonCallbacks();
            this.TransitionToState<ConnectingState>();
        }

        private void SetupPhotonCallbacks()
        {
            _networkingHandler = gameObject.GetComponent<PhotonNetworkingHandler>()
                                 ?? gameObject.AddComponent<PhotonNetworkingHandler>();

            _networkingHandler.JoinedRoom += JoinedRoom;
            _networkingHandler.JoinedLobby += JoinedLobby;
            _networkingHandler.FailedToConnectToPhoton += FailedToConnectToPhoton;
        }

        public void OnDisable()
        {
            _networkingHandler.JoinedRoom -= JoinedRoom;
            _networkingHandler.JoinedLobby -= JoinedLobby;
            _networkingHandler.FailedToConnectToPhoton -= FailedToConnectToPhoton;
        }

        private void JoinedRoom()
        {
            // This will happen when we enter a multiplayer match

            //Debug.Log("Joined Room");
        }

        private void JoinedLobby()
        {
            //Debug.Log("Joined Lobby");
            this.TransitionToState<InLobbyState>();
        }

        private void FailedToConnectToPhoton(DisconnectCause cause)
        {
            //Debug.Log("Failed to Connect: " + cause);
            this.TransitionToState<CannotConnectState>();
        }
    }


    // all callbacks are listed in enum: PhotonNetworkingMessage
    public class PhotonNetworkingHandler : Photon.MonoBehaviour
    {
        // Connection Events
        public event Action ConnectedToMaster;
        public event Action JoinedRoom;
        public event Action JoinedLobby;

        // Error Events
        public event Action<DisconnectCause> FailedToConnectToPhoton;

        public void OnConnectedToMaster()
        {
            if (ConnectedToMaster != null) ConnectedToMaster();
        }

        public void OnFailedToConnectToPhoton(DisconnectCause cause)
        {
            if (FailedToConnectToPhoton != null) FailedToConnectToPhoton(cause);
        }

        public void OnJoinedRoom()
        {
            if (JoinedRoom != null) JoinedRoom();
        }

        public void OnJoinedLobby()
        {
            if (JoinedLobby != null) JoinedLobby();
            // existing rooms available in PhotonNetwork.GetRoomList()
        }
    }
}