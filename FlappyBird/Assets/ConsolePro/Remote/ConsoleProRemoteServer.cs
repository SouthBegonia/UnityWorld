// Uncomment to use in Editor
#define USECONSOLEPROREMOTESERVERINEDITOR

#if (!UNITY_EDITOR && DEBUG) || (UNITY_EDITOR && USECONSOLEPROREMOTESERVERINEDITOR)
	#define USECONSOLEPROREMOTESERVER
#endif

#if (UNITY_WP_8_1 || UNITY_WSA)
	#define UNSUPPORTEDCONSOLEPROREMOTESERVER
#endif

#if UNITY_EDITOR && !USECONSOLEPROREMOTESERVER
#elif UNSUPPORTEDCONSOLEPROREMOTESERVER
#elif !USECONSOLEPROREMOTESERVER
#else
using System;
using System.Collections.Generic;
#endif

using UnityEngine;

#if USECONSOLEPROREMOTESERVER
using FlyingWormConsole3.LiteNetLib;
using FlyingWormConsole3.LiteNetLib.Utils;
#endif

namespace FlyingWormConsole3
{
	#if USECONSOLEPROREMOTESERVER
public class ConsoleProRemoteServer : MonoBehaviour, INetEventListener
	#else
public class ConsoleProRemoteServer : MonoBehaviour
	#endif
{
	public bool useNATPunch = false;
	public int port = 51000;

	#if UNITY_EDITOR && !USECONSOLEPROREMOTESERVER

	#elif UNSUPPORTEDCONSOLEPROREMOTESERVER

	public void Awake()
	{
		Debug.Log("Console Pro Remote Server is not supported on this platform");
	}

	#elif !USECONSOLEPROREMOTESERVER
	
	public void Awake()
	{
		Debug.Log("Console Pro Remote Server is disabled in release mode, please use a Development build or define DEBUG to use it");
	}

	#else

	private NetManager _netServer;
	private NetPeer _ourPeer;
	private NetDataWriter _dataWriter;

	[System.SerializableAttribute]
	public class QueuedLog
	{
		public string timestamp;
		public string message;
		public string logType;
	}

	[NonSerializedAttribute]
	public List<QueuedLog> logs = new List<QueuedLog>();

	private static ConsoleProRemoteServer instance = null;

	void Awake()
	{
		if(instance != null)
		{
			Destroy(gameObject);
		}
		
		instance = this;
		
		DontDestroyOnLoad(gameObject);

		Debug.Log("Starting Console Pro Server on port : " + port);

		_dataWriter = new NetDataWriter();
		_netServer = new NetManager(this, 100, "ConsolePro");
		_netServer.Start(port);
		_netServer.DiscoveryEnabled = true;
		_netServer.UpdateTime = 15;
		_netServer.MergeEnabled = true;
		_netServer.NatPunchEnabled = useNATPunch;
	}

	void OnDestroy()
	{
		if(_netServer != null)
		{
			_netServer.Stop();
		}
	}

	public void OnPeerConnected(NetPeer peer)
	{
		Debug.Log("Connected to " + peer.EndPoint);
		_ourPeer = peer;
	}

	public void OnPeerDisconnected(NetPeer peer, DisconnectInfo disconnectInfo)
	{
		Debug.Log("Disconnected from " + peer.EndPoint + ", info: " + disconnectInfo.Reason);
		if (peer == _ourPeer)
		{
			_ourPeer = null;
		}
	}

	public void OnNetworkReceive(NetPeer peer, NetDataReader reader)
	{
	
	}

	public void OnPeerDisconnected(NetPeer peer, DisconnectReason reason, int socketErrorCode)
	{

	}

	public void OnNetworkError(NetEndPoint endPoint, int socketErrorCode)
	{
	}

	public void OnNetworkReceiveUnconnected(NetEndPoint remoteEndPoint, NetDataReader reader, UnconnectedMessageType messageType)
	{
		if (messageType == UnconnectedMessageType.DiscoveryRequest)
		{
			// Debug.Log("[SERVER] Received discovery request. Send discovery response");
			_netServer.SendDiscoveryResponse(new byte[] {1}, remoteEndPoint);
		}
	}

	public void OnNetworkLatencyUpdate(NetPeer peer, int latency)
	{
		
	}


	#if UNITY_4_0 || UNITY_4_0_1 || UNITY_4_1 || UNITY_4_2 || UNITY_4_3 || UNITY_4_5 || UNITY_4_6 || UNITY_4_7 || UNITY_4_8 || UNITY_4_9

	void OnEnable()
	{
		Application.RegisterLogCallback(LogCallback);
	}

	void Update()
	{
		Application.RegisterLogCallback(LogCallback);
	}

	void OnDisable()
	{
		Application.RegisterLogCallback(null);
	}

	#else

	void OnEnable()
	{
		Application.logMessageReceivedThreaded += LogCallback;
	}

	void OnDisable()
	{
		Application.logMessageReceivedThreaded -= LogCallback;
	}

	#endif

	public void LogCallback(string logString, string stackTrace, LogType type)
	{
		if(!logString.StartsWith("CPIGNORE"))
		{
			QueueLog(logString, stackTrace, type);
		}
	}

	void QueueLog(string logString, string stackTrace, LogType type)
	{
		if(logs.Count > 1000)
		{
			while(logs.Count > 1000)
			{
				logs.RemoveAt(0);
			}
		}

		#if CSHARP_7_3_OR_NEWER
			logString = $"{logString}\n{stackTrace}\n";
			logs.Add(new QueuedLog() { message = logString, logType = type.ToString(), timestamp = $"[{DateTime.Now.ToString("HH:mm:ss")}]" } );
		#else
			logString = logString + "\n" + stackTrace + "\n";
			logs.Add(new QueuedLog() { message = logString, logType = type.ToString(), timestamp = "[" + DateTime.Now.ToString("HH:mm:ss") + "]" } );
		#endif
	}
	
	void LateUpdate()
	{
		if(_netServer == null)
		{
			return;
		}

		_netServer.PollEvents();

		if(_ourPeer == null)
		{
			return;
		}

		if(logs.Count <= 0)
		{
			return;
		}

		string cMessage = "";
		
		foreach(var cLog in logs)
		{
			cMessage = JsonUtility.ToJson(cLog);
			_dataWriter.Reset();
			_dataWriter.Put(cMessage);
			_ourPeer.Send(_dataWriter, SendOptions.ReliableOrdered);
		}

		logs.Clear();
	}

	#endif
}
}