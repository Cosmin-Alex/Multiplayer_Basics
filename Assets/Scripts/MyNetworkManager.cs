using System.Collections;
using System.Collections.Generic;
using Mirror;
using UnityEngine;

public class MyNetworkManager : NetworkManager
{
    //private int to hold the count of connected players
    //private int numberOfPlayers;

    //Reference to the virtual function in NetworkManager and that we are going to override it
    public override void OnClientConnect(NetworkConnection conn)
    {
        //Do your stuff - do your logic that you were ment to do
        base.OnClientConnect(conn);

        //So technically now first do what you normaly do (base.OnClientConnect(conn);) and then call the debug message
        Debug.Log("Client Log: I connected to a server!");
    }

    //Reference to the virtual function in NetworkManager and that we are going to override it
    public override void OnServerAddPlayer(NetworkConnection conn)
    {
        //Do your stuff - do your logic that you were ment to do
        base.OnServerAddPlayer(conn);

        //So technically now first do what you normaly do (base.OnClientConnect(conn);) and then call the debug message
        Debug.Log("Server Log: A player has been added!");

        //Get the connected players and store them
        //numberOfPlayers = numPlayers;

        //Tell me how the number of players connected
        //Debug.Log("Server Log: Number of players = " + numberOfPlayers);

        //The "$" at the begining the string changes the static string into a dinamic one - this is feature in C# - this lets us add a variable into the string
        //Used the {} in order to recognize the variable
        Debug.Log($"Server Log: Number of players = {numPlayers}");
    }

    
}
