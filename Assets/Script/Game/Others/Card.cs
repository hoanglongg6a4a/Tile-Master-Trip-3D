using DG.Tweening;
using System;
using UnityEngine;

public class Card : MonoBehaviour
{
    [SerializeField] private ConfigSO config;
    [SerializeField] private MeshRenderer mRenderer;
    [SerializeField] private BoxCollider collider;
    [SerializeField] private CardType cardType;
    [SerializeField] private Rigidbody rb;
    public CardType CardType { get => cardType; }
    public Rigidbody Rb { get => rb; set => rb = value; }

    // Start is called before the first frame update
    public void Init(CardType cardType, Material material)
    {
        this.cardType = cardType;
        this.mRenderer.material = material;
    }
    private void FixedUpdate()
    {
        Vector3 objectDirection = transform.up;
        if (objectDirection.y < 0.5f)
        {
            objectDirection.y = config.LimitToLipCard;
            transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(objectDirection), config.ForceToLipCard * Time.fixedDeltaTime);
        }

    }
    public void FlyToEmptySlot(Vector3 slotPos, float flyTime, Action CheckMatch = null)
    {
        slotPos.y += 1f;
        CheckInOrOut(false);
        transform.DORotate(Vector3.zero, flyTime);
        transform.DOScale(Vector3.one * config.ScaleDownRatio, flyTime);
        transform.DOMove(slotPos, flyTime).OnComplete(() =>
        {
            CheckMatch?.Invoke();
        });
    }
    public void ReturnToFloor(Vector3 pos, float flyTime)
    {
        transform.DOScale(Vector3.one, flyTime);
        CheckInOrOut(true);
        transform.DOMove(pos, flyTime);
    }
    public void CheckInOrOut(bool status)
    {
        rb.isKinematic = !status;
        collider.enabled = status;
    }
    // Update is called once per frame
}
