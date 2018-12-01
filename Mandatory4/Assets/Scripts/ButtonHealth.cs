using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;

public class ButtonHealth : NetworkBehaviour
{


    public const int maxHealth = 500;
    [SyncVar(hook = "OnChangeHealth")] // a hook is called everytime a variable changes to one client.
    public int currentHealth = maxHealth;

    public RectTransform healthBar;

    // Use this for initialization
    public void TakeDamage(int amount)
    {
        //if (!isServer)
        //    return;

        currentHealth -= amount;
        if (currentHealth <= 0)
        {
            currentHealth = maxHealth;
            //RpcRespawn();
        }
        //else if (currentHealth > maxHealth)
        //{
        //    currentHealth = maxHealth;
        //}

        //healthBar.sizeDelta = new Vector2(currentHealth, healthBar.sizeDelta.y);
    }

    void OnChangeHealth(int currentHealth)
    {
        healthBar.sizeDelta = new Vector2(currentHealth, healthBar.sizeDelta.y);
    }
}

