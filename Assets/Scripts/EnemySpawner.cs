using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    public GameObject enemyPrefab;
    public float spawnInterval = 2f;
    public float spawnRadius = 15f;
    public float waveIncreaseRate = 1.1f; // multiplicador por oleada
    public int enemiesPerWave = 5;
    public UpgradeUI upgradeUI;

    private float timer = 0f;
    private int currentWave = 1;
    private int enemiesSpawnedInWave = 0;

    void Update()
    {
        timer += Time.deltaTime;
        if (timer >= spawnInterval && enemiesSpawnedInWave < enemiesPerWave)
        {
            timer = 0f;
            SpawnEnemy();
            enemiesSpawnedInWave++;
        }
        else if (enemiesSpawnedInWave >= enemiesPerWave)
        {
            // Nueva oleada
            currentWave++;
            enemiesSpawnedInWave = 0;
            spawnInterval /= waveIncreaseRate; // más rápido
            enemiesPerWave = Mathf.RoundToInt(enemiesPerWave * waveIncreaseRate);
            Debug.Log("Oleada " + currentWave + " comenzada. Intervalo: " + spawnInterval + ", Enemigos: " + enemiesPerWave);
            if (upgradeUI != null) upgradeUI.ShowUpgrades();
        }
    }

    void SpawnEnemy()
    {
        if (enemyPrefab == null) return;
        Vector2 circle = Random.insideUnitCircle.normalized * spawnRadius;
        Vector3 pos = new Vector3(circle.x, 0, circle.y);
        Instantiate(enemyPrefab, pos, Quaternion.identity);
    }
}