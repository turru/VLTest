using UnityEngine;

public class EventManagerCustom : MonoBehaviour {

    public delegate void UpdateCoinsAction();
    public static event UpdateCoinsAction UpdateUI;

    public delegate void InitGameAction();
    public static event InitGameAction InitGame;

    public static EventManagerCustom instancia;

    void Awake()
    {
        if (instancia != null)
        {
            return;
        }

        instancia = this;
    }

    public static void UpdateUIMehod()
    {      
        if (UpdateUI != null)
        {
            UpdateUI();
        }
    }

    public static void InitGameMehod()
    {
        if (InitGame != null)
        {
            InitGame();
        }
    }
}

