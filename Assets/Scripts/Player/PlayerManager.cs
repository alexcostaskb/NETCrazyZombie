using TMPro;
using Unity.Collections;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.UI;

public class PlayerManager : NetworkBehaviour
{
    public const int MAX_LIFE = 100;
    public const int BULLET_DAMAGE = 10;

    public NetworkVariable<int> health;

    public NetworkVariable<int> spawns;
    public NetworkVariable<FixedString128Bytes> username;

    [SerializeField] private Image m_HealthBarImage;
    [SerializeField] private TMP_Text m_UsernameLabel;

    private GameObject playerSpawner;
    public TextMeshProUGUI txtHealth;

    public TextMeshProUGUI txtSpawns;

    /// <summary>
    /// Se ejecuta al inicio del juego. Se utiliza para inicializar las variables de salud y nombre de usuario.
    /// </summary>
    private void Awake()
    {
        health = new NetworkVariable<int>(MAX_LIFE);
        username = new NetworkVariable<FixedString128Bytes>(Utilities.GetRandomUsername());
        playerSpawner = GameObject.Find("PlayerSpawner");
    }

    /// <summary>
    /// Cuando el objeto se genera en la red, se inicializan los valores de salud y nombre de usuario.
    /// </summary>
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

    /// <summary>
    /// Cuando el objeto se destruye en la red, se eliminan los eventos de cambio de valor.
    /// </summary>
    public override void OnNetworkDespawn()
    {
        base.OnNetworkDespawn();
        health.OnValueChanged -= OnClientHealthChanged;
        username.OnValueChanged -= OnClientUsernameChanged;
        ApplyDamage(0);
    }

    /// <summary>
    ///  
    /// </summary>
    /// <param name="previousValue"></param>
    /// <param name="newValue"></param>
    private void OnClientUsernameChanged(FixedString128Bytes previousValue, FixedString128Bytes newValue)
    {
        m_UsernameLabel.text = newValue.ToString();
    }

    /// <summary>
    /// Se encarga de aplicar daño al jugador. Se llama desde el cliente y se ejecuta en el servidor.
    /// </summary>
    /// <param name="damage"></param>
    private void ApplyDamage(int damage)
    {
        if (IsOwner && health.Value > 0)
        {
            ApplyDamageRpc(damage);
        }
    }

    /// <summary>
    /// Se encarga de cambiar el nombre del jugador. Se llama desde el cliente y se ejecuta en el servidor.
    /// </summary>
    /// <param name="newValue"></param>
    [Rpc(SendTo.Server)]
    public void ChangeNameRpc(FixedString128Bytes newValue)
    {
        if (!IsServer)
        {
            return;
        }

        username.Value = newValue;
    }

    /// <summary>
    /// Se encarga de aplicar daño al jugador. Se llama desde el cliente y se ejecuta en el servidor.
    /// </summary>
    /// <param name="damage"></param>
    [Rpc(SendTo.Server)]
    public void ApplyDamageRpc(int damage)
    {
        if (!IsServer)
        {
            return;
        }

        if (health.Value > 0)
        {
            health.Value -= damage;
        }
        if (health.Value <= 0)
        {
            Die();
        }
    }

    /// <summary>
    /// se ejecuta cuando el jugador genera un spawn. Se llama desde el cliente y se ejecuta en el servidor.
    /// </summary>
    [Rpc(SendTo.Server)]
    public void ApplySpawnRpc()
    {
        if (!IsServer)
        {
            return;
        }

        spawns.Value++;
    }

    /// <summary>
    /// Se encarga de cambiar el color de la barra de salud y el texto de salud. Se llama desde el cliente y se ejecuta en el servidor.
    /// </summary>
    /// <param name="previousHealth"></param>
    /// <param name="newHealth"></param>
    private void OnClientHealthChanged(int previousHealth, int newHealth)
    {
        m_HealthBarImage.rectTransform.localScale = new Vector3(newHealth / 100.0f, 1);//(float)newHealth / 100.0f;
        const int k_MaxHealth = 100;
        float healthPercent = (float)newHealth / k_MaxHealth;
        Color healthBarColor = new Color(1 - healthPercent, healthPercent, 0);
        m_HealthBarImage.color = healthBarColor;
        txtHealth.text = newHealth.ToString();
    }

    /// <summary>
    /// Se encarga de cambiar el texto de los spawns. Se llama desde el cliente y se ejecuta en el servidor.
    /// </summary>
    /// <param name="previousValue"></param>
    /// <param name="newValue"></param>
    private void OnSpawnsChanged(int previousValue, int newValue)
    {
        txtSpawns.text = newValue.ToString();
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (IsServer)
        {
            if (collision.gameObject.CompareTag("Bullet"))
            {
                ApplyDamage(BULLET_DAMAGE);
            }
        }
    }

    /// <summary>
    /// Se encarga de eliminar al jugador. Se llama desde el cliente y se ejecuta en el servidor.
    /// </summary>
    private void Die()
    {
        if (!IsServer)
        {
            return;
        }

        /* NetworkObject networkObject = GetComponent<NetworkObject>();

        if (networkObject != null)
        {
            networkObject.Despawn(true); // Despawn and destroy on all clients
        }

        Invoke("Respawn",3); */
        Respawn();
    }

    /// <summary>
    /// Se encarga de reaparecer al jugador. Se llama desde el cliente y se ejecuta en el servidor.
    /// </summary>
    private void Respawn()
    {
        if (!IsServer)
        {
            return;
        }

        //NetworkObject networkObject = GetComponent<NetworkObject>();
        gameObject.transform.position = playerSpawner.GetComponent<SpawnPointManager>().GetRandomSpawnPoint();
        health.Value = MAX_LIFE;
        spawns.Value++;
        //networkObject.Spawn(); // Reaparece el jugador
    }
}