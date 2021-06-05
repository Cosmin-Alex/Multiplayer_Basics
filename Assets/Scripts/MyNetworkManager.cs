using System.Collections;
using System.Collections.Generic;
using Mirror;
using UnityEngine;

public class MyNetworkManager : NetworkManager
{
    //Reference to the virtual function in NetworkManager and that we are going to override it
    public override void OnClientConnect(NetworkConnection conn)
    {
        //Do your stuff - do your logic that you were ment to do
        base.OnClientConnect(conn);

        //So technically now first do what you normaly do (base.OnClientConnect(conn);) and then call the debug message
        Debug.Log("I connected to a server!");
    }
}
