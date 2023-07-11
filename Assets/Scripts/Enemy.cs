using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    // Enemy levels
    [SerializeField] public int level;
    [SerializeField] public int health;

    public void Start()
    {
        GenerateRandomLevel();
        GenerateRandomHealth();
        Debug.Log("New enemy appeared! Level: " + level + ", Health: " + health);
    }

    public void GenerateRandomLevel()
    {
        level = Random.Range(1, 5);
    }

    public void GenerateRandomHealth()
    {
        health = Mathf.FloorToInt(level * 10f);
    }

    public void TakeDamage(float damage)
    {
        health -= (int)damage; // Explicitly convert damage to int

        string damageText = damage.ToString("F1"); // Convert damage to string
        Debug.Log("Dealt " + damageText + " damage to the enemy!");

        if (health <= 0)
        {
            Die();
        }
    }

    public void Die()
    {
        Debug.Log("Enemy defeated!");

        PlayerCharacter playerCharacter = FindObjectOfType<PlayerCharacter>();
        int xpReward = GetRandomXP();
        playerCharacter.GainExperience(xpReward);

        // Reset enemy properties
        GenerateRandomLevel();
        GenerateRandomHealth();

        // Move enemy to a new position (modify as per your game requirements)
        transform.position = GetRandomRespawnPosition();

        Debug.Log("New enemy appeared! Level: " + level + ", Health: " + health);
    }

    Vector3 GetRandomRespawnPosition()
    {
        // Implement your logic to determine a random respawn position for the enemy
        // Example: Generate a random position within a predefined area
        Vector3 respawnPosition = new Vector3(Random.Range(-10f, 10f), 0f, Random.Range(-10f, 10f));
        return respawnPosition;
    }

    int GetRandomXP()
    {
        return Random.Range(10, 51);
    }
}
