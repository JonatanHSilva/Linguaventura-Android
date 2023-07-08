using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;


[System.Serializable]
public class Fase
{
    public int fase = 1;

    public Fase(int fase)
    {
        this.fase = fase;
    }
}

public class SetFaseScript : MonoBehaviour
{
    [SerializeField]
    List<Fase> fase = new List<Fase>();
    [SerializeField]
    Fase f;

    int stage;
    public string path;

    // Start is called before the first frame update
    void Start()
    {
        path = Application.dataPath + Path.AltDirectorySeparatorChar + "fase" + Path.AltDirectorySeparatorChar;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public bool SetFase()
    {
        if (Application.isMobilePlatform)
        {
            path = Application.persistentDataPath + Path.AltDirectorySeparatorChar + "StreamingAssets" + Path.AltDirectorySeparatorChar + "fase" + Path.AltDirectorySeparatorChar;
        }
        else
        {
            path = Application.dataPath + Path.AltDirectorySeparatorChar + "fase" + Path.AltDirectorySeparatorChar;
        }

        try
        {
            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }
            SerializableList<Fase> auxiliar = new SerializableList<Fase>();
            auxiliar.Lista = fase;
            string jsonData = JsonUtility.ToJson(auxiliar);
            System.IO.File.WriteAllText(path + "fase.json", jsonData);
            Debug.Log(jsonData);
            Debug.Log("Salvo em: " + path + "fase.json");
            return true;
        }
        catch (System.Exception ex)
        {
            Debug.Log("Erro ao salvar: " + ex.ToString());
            return false;
        }
    }

    public bool LoadFase()
    {
        if (Application.isMobilePlatform)
        {
            path = Application.persistentDataPath + Path.AltDirectorySeparatorChar + "StreamingAssets" + Path.AltDirectorySeparatorChar + "fase" + Path.AltDirectorySeparatorChar;
        }
        else
        {
            path = Application.dataPath + Path.AltDirectorySeparatorChar + "fase" + Path.AltDirectorySeparatorChar;
        }
        
        if (!File.Exists(Path.Combine(path, "fase.json")))
        {
            CreateFase();
            Debug.Log("Criado");
            SetFase();
        }
        
        try
        {
            string data;
            if (Application.isMobilePlatform)
            {
                //path = Application.persistentDataPath + Path.AltDirectorySeparatorChar + "fase" + Path.AltDirectorySeparatorChar;
                if (!Directory.Exists(path))
                {
                    Directory.CreateDirectory(path);
                }
                string filePath = Path.Combine(path, "fase.json");
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
                string filePath = Path.Combine(path, "fase.json");
                data = File.ReadAllText(filePath);
            }
            SerializableList<Fase> aux = JsonUtility.FromJson<SerializableList<Fase>>(data);
            fase = aux.Lista;

            Debug.Log("Arquivo lido de: " + path + "fase.json");
            return true;
        }
        catch (System.Exception ex)
        {
            Debug.Log("Erro ao ler: " + ex.ToString() + "arquivo não existe.");
            return false;
        }
    }

    public void MudarFase()
    {
        LoadFase();
        f = fase[0];
        stage = f.fase;
        stage++;
        f.fase = stage;
        SetFase();
    }

    public int GetFase()
    {
        LoadFase();
        f = fase[0];
        stage = f.fase;
        return stage;
    }

    public void Reinicio()
    {
        LoadFase();
        f = fase[0];
        stage = f.fase;
        stage = 1;
        f.fase = stage;
        SetFase();
    }

    void CreateFase()
    {
        f = new Fase(1);
        fase.Add(f);
    }
}
