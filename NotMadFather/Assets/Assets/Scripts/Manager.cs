using UnityEngine;

public class Manager : MonoBehaviour
{
    public Player player;

    void Awake()
    {
        try
        {
            player = GameObject.Find("Player").GetComponent<Player>();
        }
        catch
        {
            Debug.LogWarning("Player could not be found");
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (!player.IsAlive)
        {
            Invoke("Respawn", 2);
        }
    }

    private void Respawn()
    {
        player.Respawn();
        // reset enemies, items, the world etc etc
    }
}
