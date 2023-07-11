using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCharacter : MonoBehaviour
{
    [SerializeField] private float attack = 10f;
    [SerializeField] private int health = 100;
    [SerializeField] private int level = 1;
    [SerializeField] private int maxLevel = 5;
    [SerializeField] private int xpThreshold = 100;
    [SerializeField] private int experiencePoints = 0;
    [SerializeField] private string heroName = "Peasant";

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            Attack();
        }

        if (Input.GetKeyDown(KeyCode.L))
        {
            LevelUp();
        }
    }

    private void Attack()
    {
        GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");

        if (enemies.Length > 0)
        {
            foreach (GameObject enemy in enemies)
            {
                Enemy enemyComponent = enemy.GetComponent<Enemy>();

                if (enemyComponent != null)
                {
                    float damage = CalculateDamage();
                    float roundedDamage = Mathf.Round(damage * 10f) / 10f; // Round the damage to 1 decimal place
                    enemyComponent.TakeDamage(roundedDamage);
                }
            }
        }
        else
        {
            Debug.Log("No enemies to attack!");
        }
    }

    private float CalculateDamage()
    {
        float modifiedAttack = attack * Mathf.Pow(1f, level - 1);
        return Mathf.Round(modifiedAttack * 10f) / 10f;
    }

    public void GainExperience(int xpAmount)
    {
        experiencePoints += xpAmount;
        Debug.Log("Gained " + xpAmount + " XP. Total XP: " + experiencePoints);

        if (experiencePoints >= xpThreshold)
        {
            Debug.Log("Press 'L' to level up!");
        }
    }

    private void LevelUp()
    {
        if (experiencePoints >= xpThreshold)
        {
            level++;
            experiencePoints = 0; // Reset XP to 0
            xpThreshold *= 2;
            attack *= 1.2525f; // Increase attack value by 125.25%

            Debug.Log("Level Up! Current Level: " + level);
            Debug.Log("Current Attack Value: " + CalculateDamage().ToString("F1"));

            GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");

            if (enemies.Length > 0)
            {
                foreach (GameObject enemy in enemies)
                {
                    Enemy enemyComponent = enemy.GetComponent<Enemy>();

                    if (enemyComponent != null)
                    {
                        enemyComponent.GenerateRandomLevel();
                        enemyComponent.GenerateRandomHealth();
                    }
                }
            }

            if (level >= maxLevel)
            {
                Debug.Log("Congratulations! You have reached the maximum level and won the game! :)");
                EndGame();
                return;
            }
        }
        else
        {
            Debug.Log("You need to reach " + xpThreshold + " XP to level up!");
        }
    }

    private void EndGame()
    {
        // Reset player and enemy
        attack = 10f;
        health = 100;
        level = 1;
        xpThreshold = 100;
        experiencePoints = 0;

        Debug.Log("Game ended. Press 'R' to restart.");

        GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");
        foreach (GameObject enemy in enemies)
        {
            Destroy(enemy);
        }

        // 'R' key to restart the game
        StartCoroutine(WaitForRestartInput());
    }

    private IEnumerator WaitForRestartInput()
    {
        while (true)
        {
            if (Input.GetKeyDown(KeyCode.R))
            {
                RestartGame();
                break;
            }
            yield return null;
        }
    }
     
    private void InstantiateEnemy()
{
    GameObject instantiatedEnemy = new GameObject("Enemy");
    instantiatedEnemy.tag = "Enemy";

    Enemy enemy = instantiatedEnemy.AddComponent<Enemy>();

    if (enemy != null)
    {
        enemy.GenerateRandomLevel();
        enemy.GenerateRandomHealth();
    }
    else
    {
        Debug.LogError("Enemy component not found on the instantiated game object.");
    }
}

    private void RestartGame()
    {
        // Reset player properties
        attack = 10f;
        health = 100;
        level = 1;
        xpThreshold = 100;
        experiencePoints = 0;

        Debug.Log("Game restarted to level 1!");

        InstantiateEnemy();
    }
}
