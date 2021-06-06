using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;
using UnityEngine.AI;

public class PlayerMovement : NetworkBehaviour
{
    [SerializeField]
    private NavMeshAgent agent = null;

    //Reff to the Camera. Because the client will be using the main camera to actually figure out where we clicked in the world
    private Camera mainCamera;

    #region Server

    //This is excecuted on the server
    [Command]
    private void CmdMove(Vector3 position)
    {
        //Checks if the place they clicked is valid, basically it tests if that place is valid, and if its not, it will return and will not run whats after it.
        //This will true or false based on whether its a valid position
        if(!NavMesh.SamplePosition(position, out NavMeshHit hit, 1f, NavMesh.AllAreas))
        {
            return;
        }

        //Insted of using position, we use hit.position, because its taking the hit from where the sample was becuase his has a position, which is where the hit was
        //And this will be somewhere that's definetely valid on the NavMesh
        agent.SetDestination(hit.position);
    }

    #endregion

    #region Client

    //Reff to the main camera
    //We don't want to get it in start because that means all the players will get the cameras for all instances of the player and its not necessary
    //We only need to know about our own player

    //OnStartAuthrity its a start method for the person/player that owns this object which is exactly what we want - so only call it for us
    public override void OnStartAuthority()
    {
        base.OnStartAuthority();

        //Grabs the mains camera anc cashes it
        mainCamera = Camera.main;
    }

    //We us the update method the find out when we click and where we clicked

    //[ClientCallback] is being used because the update method is called on the client and on the server so the server will also be doing raycasting and everything
    //Even if we run it on clients, its still gonna be run on all clients but we only want it to run on our own client
    //Used [ClientCallback] it is now a client only update
    [ClientCallback]
    private void Update()
    {
        //Because its run on all the clients, we check for authority - thus if a client does not have authority, it does not belong it him, it won't run it
        if(!hasAuthority) { return; }

        //Checkes if we clicked the right mouse, if we didn't it will return
        if(!Input.GetMouseButtonDown(1)) { return; }

        //Finds out where the cursor is
        //We pass a screen point ray aka where our mouse cursor is and will return a ray which will be able to tell us where in the world we actually clicked and stores it in ray
        Ray ray = mainCamera.ScreenPointToRay(Input.mousePosition);

        //A ray cast which actually takes the ray and casts it into the scene
        //It will physically try and draw that ray and see what it hits, it returns some data on that hit and it will store it in hit, and the max range is to infinity.
        //If the Raycast doen't hut anything, then it was clicked into the void and will do nothing.
        if(!Physics.Raycast(ray, out RaycastHit hit, Mathf.Infinity)) { return; }

        //Finally 1. Make sure it belongs to us 2. Make sure pressed corrent mouse button 3. Get world position where the mouse is 4. if its not in the void, get the position using the raycast
        //Move!
        //The hit var that comes back a point var on it
        CmdMove(hit.point);
    }

    #endregion

}
