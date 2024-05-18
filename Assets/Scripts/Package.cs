using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Package : MonoBehaviour, IThrowable
{
    private int health;

    public int Health { get => health; }

    [SerializeField]
    private int maxHealth = 150;

    [SerializeField]
    private int collisionDamage = 20;

    [SerializeField]
    private Slider healthBar;

    [SerializeField]
    private TextMeshProUGUI healthText;

    private BoxCollider boxCollider;

    private bool hasBeenDamagedRecently = false;

    private float recentlyDamagedTimer;

    private const float timeForRecentDamage = 0.1f;

    private void Start()
    {
        health = maxHealth;

        boxCollider = transform.GetComponent<BoxCollider>();
    }

    private void Update()
    {
        healthBar.value = (float)health / maxHealth;
        healthText.text = health + "hp";

        if(hasBeenDamagedRecently)
        {
            recentlyDamagedTimer -= Time.deltaTime;

            if(recentlyDamagedTimer <= 0)
            {
                hasBeenDamagedRecently = false;
            }
        }
    }

    private void OnCollisionEnter(Collision collision)
    {       
        if(collision.gameObject.TryGetComponent<Recipient>(out var recipient))
        {
            if(recipient == Deliveries.GetInstance().CurrentRecipient())
            {
                return;
            }
        }

        health -= collisionDamage;

        hasBeenDamagedRecently = true;
        recentlyDamagedTimer = timeForRecentDamage;

        if(health <= 0)
        {
            Destroy(gameObject);
        }
    }

    public void PerformGrabLogic()
    {
        if(boxCollider == null)
        {
            boxCollider = transform.GetComponent<BoxCollider>();
        }

        boxCollider.enabled = false;
        transform.forward = transform.parent.forward;
    }

    public void PerformThrowLogic()
    {
        boxCollider.enabled = true;
    }
}
