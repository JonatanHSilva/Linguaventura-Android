using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

[System.Serializable]
public class Bau
{
    public int indice, aberto;

    public Bau(int indice, int aberto)
    {
        this.indice = indice;
        this.aberto = aberto;
    }
}

public class SetBauScript : MonoBehaviour
{
    [SerializeField]
    List<Bau> baus;
    [SerializeField]
    Bau bau;
    public string path;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    void SaveBau()
    {
        if (Application.isMobilePlatform)
        {
            path = Application.persistentDataPath + Path.AltDirectorySeparatorChar + "StreamingAssets" + Path.AltDirectorySeparatorChar + "bau" + Path.AltDirectorySeparatorChar;
        }
        else
        {
            path = Application.dataPath + Path.AltDirectorySeparatorChar + "bau" + Path.AltDirectorySeparatorChar;
        }

        try
        {
            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }
            SerializableList<Bau> auxiliar = new SerializableList<Bau>();
            auxiliar.Lista = baus;
            string jsonData = JsonUtility.ToJson(auxiliar);
            System.IO.File.WriteAllText(path + "bau.json", jsonData);
            Debug.Log(jsonData);
            Debug.Log("Salvo em: " + path + "bau.json");
        }
        catch (System.Exception ex)
        {
            Debug.Log("Erro ao salvar: " + ex.ToString());
        }
    }

    void LoadBau()
    {
        if (Application.isMobilePlatform)
        {
            path = Application.persistentDataPath + Path.AltDirectorySeparatorChar + "StreamingAssets" + Path.AltDirectorySeparatorChar + "bau" + Path.AltDirectorySeparatorChar;
        }
        else
        {
            path = Application.dataPath + Path.AltDirectorySeparatorChar + "bau" + Path.AltDirectorySeparatorChar;
        }
        
        if (!File.Exists(Path.Combine(path, "bau.json")))
        {
            ResetBau();
        }
        
        try
        {
            string data;
            if (Application.isMobilePlatform)
            {
                //path = Application.persistentDataPath + Path.AltDirectorySeparatorChar + "bau" + Path.AltDirectorySeparatorChar;
                if (!Directory.Exists(path))
                {
                    Directory.CreateDirectory(path);
                }
                string filePath = Path.Combine(path, "bau.json");
                data = File.ReadAllText(filePath);
                /*UnityEngine.Networking.UnityWebRequest www = UnityEngine.Networking.UnityWebRequest.Get(Path.Combine(path + "fase.json"));
                www.SendWebRequest();
                while (!www.isDone) { }

                data = www.downloadHandler.text;*/

            }
            else
            {
                if (!Directory.Exists(path))
                {
                    Directory.CreateDirectory(path);
                }
                string filePath = Path.Combine(path, "bau.json");
                data = File.ReadAllText(filePath);
            }
            SerializableList<Bau> aux = JsonUtility.FromJson<SerializableList<Bau>>(data);
            baus = aux.Lista;

            Debug.Log("Arquivo lido de: " + path + "bau.json");
        }
        catch (System.Exception ex)
        {
            Debug.Log("Erro ao ler: " + ex.ToString());
            
        }
    }
    public void SetBau(int indice)
    {
        LoadBau();
        bau = baus[indice];
        bau.aberto = 1;
        SaveBau();
    }

    public int GetBau(int indice)
    {
        LoadBau();
        bau = baus[indice];
        return bau.aberto;
    }

    public void ResetBau()
    {
        baus = new List<Bau>();
        for (int i = 0; i < 12; i++)
        {
            bau = new(i, 0);
            baus.Add(bau);
        }
        SaveBau();
    }

    private void OnApplicationQuit()
    {
        ResetBau();
    }
}
