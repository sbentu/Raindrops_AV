using UnityEngine;
using System.Collections.Generic;

public class SpawnManager : MonoBehaviour
{
    public GameObject gocciaConTestoPrefab;
    public GameObject gocciaSpecialePrefab;
    public float intervalloSpawn;
    public float intervalloSpawnSpeciale;
    private float rangeSpawnX = 8f;
    public float speed = 1f;

    private void Start()
    {
        InvokeRepeating("SpawnGocciaConTesto", 0f, intervalloSpawn);
        InvokeRepeating("SpawnGocciaSpeciale", intervalloSpawnSpeciale, intervalloSpawnSpeciale);

    }

    private void SpawnGocciaConTesto()
    {
        float posX = Random.Range(-rangeSpawnX, rangeSpawnX);
        GameObject nuovaGoccia = Instantiate(gocciaConTestoPrefab, new Vector2(posX, 6f), Quaternion.identity);

        // Passa il riferimento del SpawnManager alla goccia appena creata
        GocciaConTesto gocciaScript = nuovaGoccia.GetComponent<GocciaConTesto>();
        if (gocciaScript != null)
        {
            gocciaScript.SetSpawnManager(this);
        }
    }


    private void SpawnGocciaSpeciale()
    {
        float posX = Random.Range(-rangeSpawnX, rangeSpawnX);
        GameObject gocciaSpeciale = Instantiate(gocciaSpecialePrefab, new Vector2(posX, 6f), Quaternion.identity);
        // Passa il riferimento del SpawnManager alla goccia appena creata
        GocciaConTesto gocciaScript = gocciaSpeciale.GetComponent<GocciaConTesto>();
        if (gocciaScript != null)
        {
            gocciaScript.SetSpawnManager(this);
        }
    }
    public void MoveGoccia(GameObject goccia)
    {
        Vector2 movimento = new Vector2(0f, -1f);
        goccia.transform.Translate(movimento * speed * Time.deltaTime);
    }
}
