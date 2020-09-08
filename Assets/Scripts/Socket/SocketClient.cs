using System;
using System.Collections;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Runtime.Serialization;
using System.Text;
using System.Threading;
using UnityEngine;
using LitJson;

public class StateObject
{
	// Client socket.     
	public Socket workSocket = null;
	// Size of receive buffer.     
	public const int BufferSize = 256;
	// Receive buffer.     
	public byte[] buffer = new byte[BufferSize];
	// Received data string.     
	public StringBuilder sb = new StringBuilder();

}

struct SocketData
{
	[DataMember]
	public string DataHead;//数据头
	[DataMember]
	public string Data; //存储名字
}

//playerData
struct PlayerListData
{
	[DataMember] 
	public int PlayerId;
	[DataMember]
	public string PlayerName;
}
public class SocketClient : MonoBehaviour {

	// Client socket.     
	private Socket workSocket = null;
	// Size of receive buffer.     
	private const int BufferSize = 256;
	// Receive buffer.     
	private byte[] buffer = new byte[BufferSize];
	// Received data string.     
	private StringBuilder sb = new StringBuilder();
	
	private const int port = 6420;
	// ManualResetEvent instances signal completion.     
	private static ManualResetEvent connectDone = new ManualResetEvent(false);
	private static ManualResetEvent sendDone = new ManualResetEvent(false);
	private static ManualResetEvent receiveDone = new ManualResetEvent(false);
	
	private static String response = String.Empty;
	// player list data
	private List<PlayerListData> playerListDatas;
	
	void Start () {
		playerListDatas = new List<PlayerListData>();
	}
	void Update () {
		
	}
	private void Connected()
	{
		try
		{
			// Establish the remote endpoint for the socket.     
			// The name of the     
			// remote device is "host.contoso.com".     
			//IPHostEntry ipHostInfo = Dns.Resolve("user");
			//IPAddress ipAddress = ipHostInfo.AddressList[0];
			IPAddress ipAddress = IPAddress.Parse("169.254.231.40");
			IPEndPoint remoteEP = new IPEndPoint(ipAddress, port);
			// Create a TCP/IP socket.     
			Socket client = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
			// Connect to the remote endpoint.     
			client.BeginConnect(remoteEP, new AsyncCallback(ConnectCallback), client);
			connectDone.WaitOne();
			Debug.Log("Connected succeed ");
		}
		catch (Exception e)
		{
			Debug.Log(e.ToString());
		}
	}

	private static void ConnectCallback(IAsyncResult ar)
	{
		try
		{
			// Retrieve the socket from the state object.     
			Socket client = (Socket)ar.AsyncState;
			// Complete the connection.     
			client.EndConnect(ar);
			
			// Signal that the connection has been made.     
			connectDone.Set();
		}
		catch (Exception e)
		{
			Debug.Log(e.ToString());
		}
	}
	
	private static void Send(Socket client, String data)
	{
		// Convert the string data to byte data using ASCII encoding.     
		byte[] byteData = Encoding.ASCII.GetBytes(data);
		// Begin sending the data to the remote device.     
		client.BeginSend(byteData, 0, byteData.Length, 0, new AsyncCallback(SendCallback), client);
		sendDone.WaitOne();
		Debug.Log("Send succeed ");

	}
	
	private static void SendCallback(IAsyncResult ar)
	{
		try
		{
			// Retrieve the socket from the state object.     
			Socket client = (Socket)ar.AsyncState;
			// Complete sending the data to the remote device.     
			int bytesSent = client.EndSend(ar);
			Debug.Log("Sent {0} bytes to server. "+ bytesSent);
			// Signal that all bytes have been sent.     
			sendDone.Set();
		}
		catch (Exception e)
		{
			Debug.Log(e.ToString());
		}
	}
	
	private static void Receive(Socket client)
	{
		try
		{
			// Create the state object.     
			StateObject state = new StateObject();
			state.workSocket = client;
			// Begin receiving the data from the remote device.     
			client.BeginReceive(state.buffer, 0, StateObject.BufferSize, 0, new AsyncCallback(ReceiveCallback), state);
			receiveDone.WaitOne();
			Debug.Log("Receive succeed ");

		}
		catch (Exception e)
		{
			Debug.Log(e.ToString());
		}
	}
	private static void ReceiveCallback(IAsyncResult ar)
	{
		try
		{
			// Retrieve the state object and the client socket     
			// from the asynchronous state object.     
			StateObject state = (StateObject)ar.AsyncState;
			Socket client = state.workSocket;
			// Read data from the remote device.     
			int bytesRead = client.EndReceive(ar);
			if (bytesRead > 0)
			{
				// There might be more data, so store the data received so far.     

				state.sb.Append(Encoding.ASCII.GetString(state.buffer, 0, bytesRead));
				// Get the rest of the data.     
				client.BeginReceive(state.buffer, 0, StateObject.BufferSize, 0, new AsyncCallback(ReceiveCallback), state);
			}
			else
			{
				// All the data has arrived; put it in response.     
				if (state.sb.Length > 1)
				{
					response = state.sb.ToString();
				}
				// Signal that all bytes have been received.     
				receiveDone.Set();
			}
		}
		catch (Exception e)
		{
			Debug.Log(e.ToString());
		}
	}
	//DataProcessing
	private string DataProcessingTOJsonString(string head,string data)
	{
		SocketData SendData;
		SendData.DataHead = head;
		SendData.Data = data;
		return JsonMapper.ToJson(SendData);
	}
	//StringProcessingToJson
	private SocketData JsonStringProcessingTOData(string socketData)
	{
		SocketData ReceiveData;
		JsonData jsonData;
		jsonData = JsonMapper.ToObject(socketData);
		ReceiveData.DataHead = (string)jsonData["DataHead"];
		ReceiveData.Data = (string)jsonData["Data"];
		return ReceiveData;
	}
	//PlayerDataProcessingToJson
	private string PlayerDataProcessingToJson(List<PlayerListData> playerListdatas)
	{
		return JsonMapper.ToJson(playerListdatas);
	}
	//Add data to PlayerData
	private List<PlayerListData> AddPlayerDatasToList(int playerId ,string playerName,List<PlayerListData> playerDataList)
	{
		PlayerListData tempDatas;
		tempDatas.PlayerId = playerId;
		tempDatas.PlayerName = playerName;
		playerDataList.Add(tempDatas);
		return playerDataList;
	}
	//JsonProcessingToPlayerData
	private List<PlayerListData> JsonProcessingToPlayerData(string socketData)
	{
		List<PlayerListData> playerDataList = new List<PlayerListData>();
		JsonData playerJsonData;
		playerJsonData = JsonMapper.ToObject(socketData);
		for (int i = 0; i < playerJsonData.Count; i++)
		{
			PlayerListData playerListData;
			playerListData.PlayerId = (int) playerJsonData[i]["PlayerId"];
			playerListData.PlayerName = (string) playerJsonData[i]["PlayerName"];
			playerDataList.Add(playerListData);
		}

		return playerDataList;
	}
	

}
