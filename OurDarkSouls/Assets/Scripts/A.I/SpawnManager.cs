using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SG
{
    public class SpawnManager : MonoBehaviour
    {
        public Transform[] spawnPoints;
        public GameObject enemyPrefab;

        private void Start() 
        {
            SpwanNewEnemy();
        }

        private void OnEnable() 
        {
            // EnemyStatsManager.OnEnemyKilled += SpwanNewEnemy;
            // PlayerCombatManager.OnEnemyKilledText += SpwanNewEnemy;
        }

        private void SpwanNewEnemy()
        {
            int randomNumber = Mathf.RoundToInt(Random.Range(0f, spawnPoints.Length - 1));

            Instantiate(enemyPrefab, spawnPoints[randomNumber].transform.position, Quaternion.identity);
        }
    }
}
