using System.Collections;
using DG.Tweening;
using UnityEngine;
using UnityEngine.InputSystem;
public class SlingShotHandler : MonoBehaviour
{
    [Header("Sounds")]
    [SerializeField] private AudioClip elasticPulledClip;
    [SerializeField] private AudioClip[] elasticReleasedClips;

    [Header("LineRenderers")]
    [SerializeField] private LineRenderer leftLineRenderer;
    [SerializeField] private LineRenderer rightLineRenderer;
    [Header("Transforms")]
    [SerializeField] private Transform leftTransform;
    [SerializeField] private Transform rightTransform;
    [SerializeField] private Transform centerTransform;
    [SerializeField] private Transform idleTransform;
    [SerializeField] private Transform elasticTransform;
    [Header("SlingShotConfig")]
    [SerializeField] private float maxLength = 5f;
    [SerializeField] private float shotForce = 5f;
    [SerializeField] private float respawnTimer = 2f;
    [SerializeField] private float slingSpeedModifier = 1.2f;
    [SerializeField] private AnimationCurve animationCurve;
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
    private AudioSource audioSource;
    private void Awake()
    {
        audioSource = GetComponent<AudioSource>();

        leftLineRenderer.enabled = false;
        rightLineRenderer.enabled = false;
        SpawnBirb();
    }

    private void Update()
    {
        if (InputManager.WasLeftMouseButtonPressed && slingShotArea.IsWithinSlingShotArea())
        {
            clickedWithinArea = true;
            if (birbOnSlingShot)
            {
                Debug.Log(SoundManager.Instance);
                Debug.Log(elasticPulledClip);
                Debug.Log(audioSource);
                SoundManager.Instance.PlayClip(elasticPulledClip, audioSource);
            }
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
                birbOnSlingShot = false;
                birb.LaunchBirb(direction, shotForce);
                SoundManager.Instance.PlayRandomClip(elasticReleasedClips, audioSource);
                GameManager.Instance.UseShot();
                AnimateSlingShot();
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

    #region tween
    private void AnimateSlingShot()
    {
        elasticTransform.position = leftLineRenderer.GetPosition(0);
        float distance = Vector2.Distance(elasticTransform.position, centerTransform.position);
        float time = distance / slingSpeedModifier;

        elasticTransform.DOMove(centerTransform.position, time).SetEase(animationCurve);
        StartCoroutine(AnimateSlingShotLines(elasticTransform, time));
    }

    private IEnumerator AnimateSlingShotLines(Transform trans, float time)
    {
        float elapsedTime = 0f;

        while (elapsedTime < time)
        {
            elapsedTime += Time.deltaTime;
            SetLines(trans.position);
            yield return null;
        }
    }
    #endregion
}

