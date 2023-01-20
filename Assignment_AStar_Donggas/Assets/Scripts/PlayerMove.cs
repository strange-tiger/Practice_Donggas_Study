using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Define;

public class PlayerMove : MonoBehaviour
{
    [SerializeField] DestinationSetter destSetter;
    [SerializeField] Rigidbody rigidboby;

    public (int x, int z) CurrentTilePosition { get; private set; }

    private Coroutine currentCoroutine;

    private void OnEnable()
    {
        destSetter.DestinationChanged -= Move;
        destSetter.DestinationChanged += Move;
    }

    private void OnDisable()
    {
        destSetter.DestinationChanged -= Move;
    }

    private void Move(Vector3 destination)
    {
        SetCurTilePosition();

        if (currentCoroutine != null)
        {
            StopCoroutine(currentCoroutine);
        }
        currentCoroutine = StartCoroutine(moveToDestination(destination));
    }

    private void SetCurTilePosition()
    {
        int tileX = Mathf.RoundToInt(transform.position.x);
        int tileZ = Mathf.RoundToInt(transform.position.z);

        CurrentTilePosition = (tileX, tileZ);
        rigidboby.position = new Vector3(tileX, 0f, tileZ);
    }

    private IEnumerator moveToDestination(Vector3 destination)
    {
        Vector3 direction = Player.PLAYER_SPEED * (destination - rigidboby.position);

        float elapsedTime = 0f;
        while (elapsedTime < 1f)
        {
            yield return null;
            
            elapsedTime += Player.PLAYER_SPEED * Time.deltaTime;

            rigidboby.MovePosition(rigidboby.position + Time.deltaTime * direction);
        }
    }
}
