using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;
using System.IO;

[System.Serializable]
public class Nome
{
    public string nome;

    public Nome(string nome)
    {
        this.nome = nome;
    }
}

public class SetNomeJogador : MonoBehaviour
{
    [SerializeField]
    List<Nome> nome = new List<Nome>();
    [SerializeField]
    Nome n;
    MenuScript m;

    bool click = false;

    public TMP_InputField nomePlayer;

    string nomeJogador;
    public string path;
    // Start is called before the first frame update
    void Start()
    {
        m = FindObjectOfType<MenuScript>();
        path = Application.dataPath + Path.AltDirectorySeparatorChar + "nome" + Path.AltDirectorySeparatorChar;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SetNome()
    {
        try
        {
            if (!System.IO.Directory.Exists(path))
            {
                System.IO.Directory.CreateDirectory(path);
            }
            SerializableList<Nome> auxiliar = new SerializableList<Nome>();
            auxiliar.Lista = nome;
            string jsonData = JsonUtility.ToJson(auxiliar);
            System.IO.File.WriteAllText(path + "nomejogador.json", jsonData);
            Debug.Log(jsonData);
            Debug.Log("Salvo em: " + path + "nomejogador.json");
        }
        catch (System.Exception ex)
        {
            Debug.Log("Erro ao salvar: " + ex.ToString());
        }
    }

    public void LoadNome()
    {
        path = Application.dataPath + Path.AltDirectorySeparatorChar + "nome" + Path.AltDirectorySeparatorChar;
        if (!File.Exists(path + "nomejogador.json"))
        {
            CreateNome();
        }
        
        try
        {
            string data;
            if (Application.isMobilePlatform)
            {
                path = Application.persistentDataPath + Path.AltDirectorySeparatorChar + "nome" + Path.AltDirectorySeparatorChar;
                if (!Directory.Exists(path))
                {
                    Directory.CreateDirectory(path);
                }
                string filePath = Path.Combine(path, "nomejogador.json");
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
                string filePath = System.IO.Path.Combine(path + "nomejogador.json");
                data = System.IO.File.ReadAllText(filePath);
                Debug.Log("Er");
            }
            SerializableList<Nome> aux = JsonUtility.FromJson<SerializableList<Nome>>(data);
            nome = aux.Lista;

            Debug.Log("Arquivo lido de: " + path + "nomejogador.json");
        }
        catch (System.Exception ex)
        {
            Debug.Log("Erro ao ler: " + ex.ToString());
            Debug.Log("Er");
        }
    }

    public void MudarNome()
    {
        if(click)
        {
            if (nomePlayer.text != "")
            {
                n.nome = new(nomePlayer.text);
                nome.Add(n);
                SetNome();
            }
            else
            {
                CreateNome();
            }
            m.Jogar();
        }
    }

    public string GetNome()
    {
        LoadNome();
        n = nome[0];
        nomeJogador = n.nome;
        return nomeJogador;
    }

    public void clicked()
    {
        click = true;
        MudarNome();
    }

    void CreateNome()
    {
        n = new("Jogador");
        nome.Add(n);
        SetNome();
    }
}
