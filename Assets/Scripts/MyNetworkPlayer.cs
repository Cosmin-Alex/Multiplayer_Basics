using Mirror;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MyNetworkPlayer : NetworkBehaviour
{
    //If you put this before a field will sync var from the server to the clients
    [SyncVar]
    //Because we want to see it in the inspector
    [SerializeField]
    //Gave the default valie of "Missing Name" in case something goes wrong when setting a name, at least we will have a value that's telling us that the name is missing
    private string displayName = "Missing Name";

    //The displayName string is private and we need a public way to access it

    //This method is only ever going to be run only on the server
    //The Server is the only one who can do this - thus the [server] attribute
    //This ism't necessary, we don't actually need this, but is to protect us from ourselfs, it will stop clients from running this method
    //If we accidentally, somewhere else in our code, call this on the client, it won't even try to change the variable, it will stop it, because he is not the server
    //And will put a warning in the console - kind of a protection
    [Server]
    public void SetDisplayName(string newDisplayName)
    {
        displayName = newDisplayName;
    }

    [SyncVar]
    [SerializeField]
    //The default color is black
    private Color displayColor = Color.black;

    [Server]
    public void SetPlayerColor(Color newColor)
    {
        //newColor = new Color(Random.Range(0f, 1f), Random.Range(0f, 1f), Random.Range(0f, 1f));
        displayColor = newColor;
    }

}
