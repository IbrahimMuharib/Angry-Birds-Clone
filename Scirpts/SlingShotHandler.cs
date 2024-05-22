using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;
public class SlingShotHandler : MonoBehaviour
{
    [Header("LineRenderers")]
    [SerializeField] private LineRenderer leftLineRenderer;
    [SerializeField] private LineRenderer rightLineRenderer;
    [Header("Transforms")]
    [SerializeField] private Transform leftTransform;
    [SerializeField] private Transform rightTransform;
    [SerializeField] private Transform centerTransform;
    [SerializeField] private Transform idleTransform;
    [Header("SlingShotConfig")]
    [SerializeField] private float maxLength = 5f;
    [SerializeField] private float shotForce = 5f;
    [SerializeField] private float respawnTimer = 2f;
    [SerializeField] private SlingShotArea slingShotArea;
    [Header("Birb")]
    [SerializeField] private Birb birbPrefab;
    [SerializeField] private float birbPositionOffset = 2f;
    private Vector2 linesPosition;
    private Vector2 direction;
    private Vector2 directionNormalized;
    private Birb birb;
    private bool clickedWithinArea = false;
    private bool birbOnSlingShot = false;
    private void Awake()
    {
        leftLineRenderer.enabled = false;
        rightLineRenderer.enabled = false;
        SpawnBirb();
    }

    private void Update()
    {
        if (InputManager.WasLeftMouseButtonPressed && slingShotArea.IsWithinSlingShotArea())
        {
            clickedWithinArea = true;
        }
        if (InputManager.IsLeftMouseButtonPressed && clickedWithinArea && birbOnSlingShot)
        {
            DrawSlingShot();
            positionAndRotateAngie();
        }

        if (InputManager.WasLeftMouseButtonReleased && birbOnSlingShot && clickedWithinArea)
        {
            if (GameManager.Instance.canShoot())
            {
                clickedWithinArea = false;
                birb.LaunchBirb(direction, shotForce);
                GameManager.Instance.UseShot();
                birbOnSlingShot = false;
                ResetSlingShot();
                if (GameManager.Instance.canShoot())
                {
                    StartCoroutine(spawnAngieAfterTime());
                }
            }
        }

    }

    #region slingshot
    private void DrawSlingShot()
    {
        Vector2 touchPosition = Camera.main.ScreenToWorldPoint(InputManager.MousePosition);
        linesPosition = Vector2.ClampMagnitude(touchPosition - (Vector2)centerTransform.position, maxLength) + (Vector2)centerTransform.position;
        SetLines(linesPosition);

        direction = (Vector2)centerTransform.position - linesPosition;
        directionNormalized = direction.normalized;
    }

    private void SetLines(Vector2 position)
    {
        if (!leftLineRenderer.enabled)
        {
            leftLineRenderer.enabled = true;
        }
        if (!rightLineRenderer.enabled)
        {
            rightLineRenderer.enabled = true;
        }

        rightLineRenderer.SetPosition(0, position);
        rightLineRenderer.SetPosition(1, rightTransform.position);
        leftLineRenderer.SetPosition(0, position);
        leftLineRenderer.SetPosition(1, leftTransform.position);

    }

    private void ResetSlingShot()
    {
        SetLines(centerTransform.position);
    }
    #endregion

    #region birb
    private void SpawnBirb()
    {
        if (birb != null)
        {
            Destroy(birb.gameObject);
        }
        SetLines(idleTransform.position);

        Vector2 dir = (centerTransform.position - idleTransform.position).normalized;
        Vector2 spawnPosition = (Vector2)idleTransform.position + dir * birbPositionOffset;
        birb = Instantiate(birbPrefab, spawnPosition, Quaternion.identity);
        birb.transform.right = dir;
        birbOnSlingShot = true;

    }

    private void positionAndRotateAngie()
    {
        birb.transform.position = linesPosition + directionNormalized * birbPositionOffset;
        birb.transform.right = directionNormalized;
    }

    private IEnumerator spawnAngieAfterTime()
    {
        yield return new WaitForSeconds(respawnTimer);
        SpawnBirb();
    }
    #endregion
}

