using Mirror;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class MyNetworkPlayer : NetworkBehaviour
{
    //Ref to TextMesh pro element
    [SerializeField]
    private TMP_Text displayNameText = null;
    //Ref to a Renderer so we can change the renderer color
    [SerializeField]
    private Renderer displayColorRenderer = null;



    //If you put this before a field will sync var from the server to the clients
    [SyncVar(hook = nameof(HandleDisplayNameUpdated))]
    //Because we want to see it in the inspector
    [SerializeField]
    //Gave the default valie of "Missing Name" in case something goes wrong when setting a name, at least we will have a value that's telling us that the name is missing
    private string displayName = "Missing Name";

    #region Server

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

    //why the hook, whenever the display color gets updated on a client, the function gets called with the old and new color passed in and with the new color we can update the renderer
    [SyncVar(hook = nameof(HandleDisplayColorUpdated))]
    [SerializeField]
    //The default color is black
    private Color displayColor = Color.black;

    [Server]
    public void SetPlayerColor(Color newColor)
    {
        //newColor = new Color(Random.Range(0f, 1f), Random.Range(0f, 1f), Random.Range(0f, 1f));
        displayColor = newColor;
    }

    //When its called the server will allow it because it has commad, so it will trigger the syncVar and then it will update it on all the clients
    [Command]
    private void CmdSetDisplayName(string newDisplayName)
    {


        SetDisplayName(newDisplayName);
    }

    [ClientRpc]
    private void RpcBlessTheClientsNames(string blessedDisplayName)
    {
        SetDisplayName(blessedDisplayName);
    }

    [ContextMenu("Bless the clients")]
    private void BlessAllTheClients()
    {
        CmdBlessPlayers();
    }

    [Command]
    private void CmdBlessPlayers()
    {
        RpcBlessTheClientsNames("Blessed Names");
    }

    #endregion


    #region Client
    //Logic to update the color
    //We need to use this as a callback for when the color changes thus we need to hook it, Mirror needs 2 parameters for some reason
    private void HandleDisplayColorUpdated(Color oldColor, Color newColor)
    {
        //Change the base color of the renderer
        displayColorRenderer.material.SetColor("_BaseColor", newColor);
    }

    //Logic to update the Display Name of the player
    private void HandleDisplayNameUpdated(string oldDisplayName, string newDisplayName)
    {
        displayNameText.text = newDisplayName;
    }

    //ContectMenu - If I right click on the script component in the editor, it will run what's below it
    [ContextMenu("Set My Name")]
    //If from a client the below method is called, it will run the CmdSetDisplayName and because its a Command, the server will allow it
    private void SetMyname()
    {
        CmdSetDisplayName("My New Name");
    }

    #endregion



}
