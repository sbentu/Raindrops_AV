using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using TMPro;
using System;

public class GocciaConTesto : MonoBehaviour
{
    
    public UnityEvent OnRaggiuntoLimiteY;
    public UnityEvent OnRaggiuntoLimiteY0;
    public GameController gameController;
    public TextMeshPro displayText;
    private SpawnManager spawnManager;

    public int Risultato { get; private set; } // Proprietà pubblica per accedere al risultato

    public void SetSpawnManager(SpawnManager manager)
    {
        spawnManager = manager;
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        gameObject.SetActive(false);
        GameController.Instance.HandleCollision();
    }

    void Start()
    {
        float numero1, numero2, risultato;
        string operazioneRandomica;

        do
        {
            operazioneRandomica = GeneraOperazioneRandomica();

            numero1 = UnityEngine.Random.Range(1, 11);
            numero2 = UnityEngine.Random.Range(1, 11);

            if (operazioneRandomica == "divisione")
            {
                (numero1, numero2) = CheckNumeri(numero1, numero2);
            }

            risultato = EseguiOperazione(operazioneRandomica, numero1, numero2);
        } while (float.IsNaN(risultato));

        MostraTesto(operazioneRandomica, numero1, numero2, risultato);

        // Imposta il risultato come proprietà pubblica
        Risultato = (int)risultato;
    }

    void Update()
    {
        if (spawnManager != null)
        {
            spawnManager.MoveGoccia(gameObject);
        }
    }

    string GeneraOperazioneRandomica()
    {
        string[] operazioni = { "addizione", "sottrazione", "moltiplicazione", "divisione" };
        int indiceOperazione = UnityEngine.Random.Range(0, operazioni.Length);
        return operazioni[indiceOperazione];
    }

    float EseguiOperazione(string operazione, float numero1, float numero2)
    {
        switch (operazione)
        {
            case "addizione":
                return numero1 + numero2;
            case "sottrazione":
                return numero1 - numero2;
            case "moltiplicazione":
                return numero1 * numero2;
            case "divisione":
                return numero1 / numero2;

            default:
                return float.NaN;
        }
    }


    void MostraTesto(string operazione, float numero1, float numero2, float risultato)
    {
        string simboloOperazione = GetSimboloOperazione(operazione);
        string testoDaMostrare = $"{numero1}{simboloOperazione}{numero2}";
        displayText.text = testoDaMostrare;
    }

    string GetSimboloOperazione(string operazione)
    {
        switch (operazione)
        {
            case "addizione":
                return "+";
            case "sottrazione":
                return "-";
            case "moltiplicazione":
                return "*";
            case "divisione":
                return "/";
            default:
                Debug.LogError("Simbolo per l'operazione non riconosciuto: " + operazione);
                return "?";
        }
    }
    (float nuovoNumero1, float nuovoNumero2) RicalcolaDivisione()
    {
        float n1, n2;
        do
        {
            n1 = UnityEngine.Random.Range(1, 11);
            n2 = UnityEngine.Random.Range(1, 11);
        } while (n1 % n2 != 0 || n1 < n2);
        
        return (n1, n2);
    }

    (float nuovoNumero1, float nuovoNumero2) CheckNumeri(float num1, float num2)
    {
        if (num2 == 0 || num1 % num2 != 0)
        {
            // Ricalcola numeri e risultato
            (num1, num2) = RicalcolaDivisione();;
            return (num1, num2);
        }
        else
        {
            if (num2 != 0)
            {;
                return (num1, num2);
            }
            else
            {
                return (float.NaN, float.NaN); // O qualsiasi altro valore di gestione dell'errore
            }
        }
    }

}