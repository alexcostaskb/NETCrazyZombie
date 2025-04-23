using TMPro;
using Unity.Collections;
using UnityEngine;
using UnityEngine.UI;
using Unity.Netcode;


public class PlayerManager : NetworkBehaviour
{
    public const int MAX_LIFE = 100;
    public const int BULLET_DAMAGE = 10;
     
    public NetworkVariable<int> health;

    public NetworkVariable<int> spawns;
    public NetworkVariable<FixedString128Bytes> username;

    [SerializeField] Image m_HealthBarImage;
    [SerializeField] TMP_Text m_UsernameLabel;

    private GameObject playerSpawner;
    public TextMeshProUGUI txtHealth;

    public TextMeshProUGUI txtSpawns;

    private void Awake()
    {
        health = new NetworkVariable<int>(MAX_LIFE);
        username = new NetworkVariable<FixedString128Bytes>(Utilities.GetRandomUsername());
        playerSpawner = GameObject.Find("PlayerSpawner");
    }

    public override void OnNetworkSpawn()
    {
        base.OnNetworkSpawn();
        health.OnValueChanged += OnClientHealthChanged;
        spawns.OnValueChanged += OnSpawnsChanged;
        username.OnValueChanged += OnClientUsernameChanged;
        ChangeNameRpc(Utilities.GetRandomUsername());
        gameObject.transform.position = playerSpawner.GetComponent<SpawnPointManager>().GetRandomSpawnPoint();
        OnClientHealthChanged(MAX_LIFE, MAX_LIFE);
    }

    public override void OnNetworkDespawn()
    {
        base.OnNetworkDespawn();
        health.OnValueChanged -= OnClientHealthChanged;
        username.OnValueChanged -= OnClientUsernameChanged;
        ApplyDamage(0);
    }

    private void OnClientUsernameChanged(FixedString128Bytes previousValue, FixedString128Bytes newValue)
    {
        m_UsernameLabel.text = newValue.ToString();
    }

    void ApplyDamage(int damage)
    {
        if(IsOwner && health.Value > 0)
        {
            ApplyDamageRpc(damage);   
        }        
    }

    [Rpc(SendTo.Server)]
    public void ChangeNameRpc(FixedString128Bytes newValue)
    {
        if(!IsServer) return;
        username.Value = newValue;        
    }

    [Rpc(SendTo.Server)]
    public void ApplyDamageRpc(int damage)
    {
        if(!IsServer) return;
        
        if (health.Value > 0)
        {
            health.Value -= damage;
        }
        if (health.Value <= 0)
        {
            Die();
        }
    }

    [Rpc(SendTo.Server)]
    public void ApplySpawnRpc()
    {
        if(!IsServer) return;
        
        spawns.Value++;
    }

    void OnClientHealthChanged(int previousHealth, int newHealth)
    {
        m_HealthBarImage.rectTransform.localScale = new Vector3((float)newHealth / 100.0f, 1);//(float)newHealth / 100.0f;
        const int k_MaxHealth = 100;
        float healthPercent = (float)newHealth / k_MaxHealth;
        Color healthBarColor = new Color(1 - healthPercent, healthPercent, 0);
        m_HealthBarImage.color = healthBarColor;
        txtHealth.text = newHealth.ToString();
    }

    void OnSpawnsChanged(int previousValue, int newValue)
    {
       txtSpawns.text = newValue.ToString();
    }


    void OnCollisionEnter(Collision collision)
    {
        if(IsServer){
            if (collision.gameObject.CompareTag("Bullet"))
            {
                ApplyDamage(BULLET_DAMAGE);
            }
        }
    }

    private void Die()
    {
        if (!IsServer) return;

        /* NetworkObject networkObject = GetComponent<NetworkObject>();

        if (networkObject != null)
        {
            networkObject.Despawn(true); // Despawn and destroy on all clients
        }

        Invoke("Respawn",3); */
        Respawn();
    }

    private void Respawn()
    {
        if(!IsServer) return;

        //NetworkObject networkObject = GetComponent<NetworkObject>();
        gameObject.transform.position = playerSpawner.GetComponent<SpawnPointManager>().GetRandomSpawnPoint();
        health.Value = MAX_LIFE;
        spawns.Value++;
        //networkObject.Spawn(); // Reaparece el jugador
    }
}
