using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

[System.Serializable]
public class Dano
{
    public int dano;

    public Dano(int dano)
    {
        this.dano = dano;
    }
}

public class SetDanoScript : MonoBehaviour
{
    [SerializeField]
    List<Dano> dano = new List<Dano>();
    [SerializeField]
    Dano d;
    public string path;

    int damage;
    private void Start()
    {
        path = Application.streamingAssetsPath + Path.AltDirectorySeparatorChar + "dano" + Path.AltDirectorySeparatorChar;
    }
    void SalvarDano()
    {
        if (Application.isMobilePlatform)
        {
            path = Application.persistentDataPath + Path.AltDirectorySeparatorChar + "StreamingAssets" + Path.AltDirectorySeparatorChar + "dano" + Path.AltDirectorySeparatorChar;
        }
        else
        {
            path = Application.dataPath + Path.AltDirectorySeparatorChar + "dano" + Path.AltDirectorySeparatorChar;
        }

        try
        {
            if (!System.IO.Directory.Exists(path))
            {
                System.IO.Directory.CreateDirectory(path);
            }
            SerializableList<Dano> auxiliar = new SerializableList<Dano>();
            auxiliar.Lista = dano;
            string jsonData = JsonUtility.ToJson(auxiliar);
            System.IO.File.WriteAllText(path + "dano.json", jsonData);
            Debug.Log(jsonData);
            Debug.Log("Salvo em: " + path + "dano.json");
        }
        catch (System.Exception ex)
        {
            Debug.Log("Erro ao salvar: " + ex.ToString());
        }
    }

    void LoadDano()
    {
        if (Application.isMobilePlatform)
        {
            path = Application.persistentDataPath + Path.AltDirectorySeparatorChar + "StreamingAssets" + Path.AltDirectorySeparatorChar + "dano" + Path.AltDirectorySeparatorChar;
        }
        else
        {
            path = Application.dataPath + Path.AltDirectorySeparatorChar + "dano" + Path.AltDirectorySeparatorChar;
        }
        
        if (!File.Exists(Path.Combine(path, "dano.json")))
        {
            CreateDano();
        }

        try
        {
            string data;
            if (Application.isMobilePlatform)
            {
                //path = Application.persistentDataPath + Path.AltDirectorySeparatorChar + "dano" + Path.AltDirectorySeparatorChar;
                if (!Directory.Exists(path))
                {
                    Directory.CreateDirectory(path);
                }
                string filePath = Path.Combine(path, "dano.json");
                data = File.ReadAllText(filePath);
                /*UnityEngine.Networking.UnityWebRequest www = UnityEngine.Networking.UnityWebRequest.Get(Path.Combine(path + "fase.json"));
                www.SendWebRequest();
                while (!www.isDone) { }

                data = www.downloadHandler.text;*/
            }
            else
            {
                if (!System.IO.Directory.Exists(path))
                {
                    System.IO.Directory.CreateDirectory(path);
                }
                string filePath = System.IO.Path.Combine(path, "dano.json");
                data = System.IO.File.ReadAllText(filePath);
            }
            SerializableList<Dano> aux = JsonUtility.FromJson<SerializableList<Dano>>(data);
            dano = aux.Lista;

            Debug.Log("Arquivo lido de: " + path + "dano.json");
        }
        catch (System.Exception ex)
        {
            Debug.Log("Erro ao ler: " + ex.ToString());
        }
    }

    public void SetDano(int damage)
    {
        LoadDano();
        d = dano[0];
        this.damage = damage;
        d.dano = this.damage;
        SalvarDano();
    }

    public int GetDano()
    {
        LoadDano();
        d = dano[0];
        damage = d.dano;
        return damage;
    }

    void CreateDano()
    {
        d = new(10);
        dano.Add(d);
    }
}
