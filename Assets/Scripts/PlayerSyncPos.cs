using UnityEngine;
using System.Collections;
using UnityEngine.Networking;

public class PlayerSyncPos : NetworkBehaviour
{
    [SyncVar]
    private Vector2 syncPosition;

    [SyncVar]
    private Vector3 syncScale;

    public float lerpSpeed;

    void FixedUpdate()
    {
        LerpPosition();
        TransmitPosition();
    }

    void LerpPosition()
    {
        if (!isLocalPlayer)
        {
            transform.position = Vector2.Lerp(transform.position, syncPosition, Time.deltaTime * lerpSpeed);
            transform.localScale = syncScale;
        }
    }

    [Client]
    void TransmitPosition()
    {
        if (isLocalPlayer)
        {
            CmdSendPosition(transform.position, transform.localScale);
        }
    }

    [Command]
    void CmdSendPosition(Vector2 pos, Vector3 client_scale)
    {
        syncPosition = pos;
        syncScale = client_scale;
    }

}