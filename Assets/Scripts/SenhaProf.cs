using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;
using System.IO;
using UnityEngine.UI;

[Serializable]
public class Senha
{
    public string senha = "";

    public Senha(string senha)
    {
        this.senha = senha;
    }
}

public class SenhaProf : MonoBehaviour
{
    [SerializeField]
    List<Senha> senha = new List<Senha>();
    [SerializeField]
    Senha s;
    public TMP_InputField password;
    public TMP_InputField changePassword;
    public TMP_InputField matchedPassword;
    public GameObject areaProf, senhaCorreta;
    public Text mensagem;
    public Button botao;
    bool click = false;
    int campo = 1;
    public string path;

    private void Start()
    {
        path = Application.streamingAssetsPath + Path.AltDirectorySeparatorChar + "senha" + Path.AltDirectorySeparatorChar;
        LoadSenha();
    }

    private void Update()
    {
        botao.onClick.AddListener(clicked);
        if (Input.GetKeyDown(KeyCode.Tab))
        {
            campo++;
            if(campo == 4)
            {
                campo = 1;
            }
        }

        switch (campo)
        {
            case 1:
                password.Select();
                break;
            case 2:
                changePassword.Select();
                break;
            case 3:
                matchedPassword.Select();
                break;
        }
    }


    public bool SetSenha()
    {
        if (Application.isMobilePlatform)
        {
            path = Application.persistentDataPath + Path.AltDirectorySeparatorChar + "StreamingAssets" + Path.AltDirectorySeparatorChar + "senha" + Path.AltDirectorySeparatorChar;
        }
        else
        {
            path = Application.dataPath + Path.AltDirectorySeparatorChar + "senha" + Path.AltDirectorySeparatorChar;
        }

        try
        {
            if (!System.IO.Directory.Exists(path))
            {
                System.IO.Directory.CreateDirectory(path);
            }
            SerializableList<Senha> auxiliar = new SerializableList<Senha>();
            auxiliar.Lista = senha;
            string jsonData = JsonUtility.ToJson(auxiliar);
            System.IO.File.WriteAllText(path + "senha.json", jsonData);
            Debug.Log(jsonData);
            Debug.Log("Salvo em: " + path + "senha.json");
            return true;
        }
        catch (System.Exception ex)
        {
            Debug.Log("Erro ao salvar: " + ex.ToString());
            return false;
        }
    }

    public bool LoadSenha()
    {
        if (Application.isMobilePlatform)
        {
            path = Application.persistentDataPath + Path.AltDirectorySeparatorChar + "StreamingAssets" + Path.AltDirectorySeparatorChar + "senha" + Path.AltDirectorySeparatorChar;
        }
        else
        {
            path = Application.dataPath + Path.AltDirectorySeparatorChar + "senha" + Path.AltDirectorySeparatorChar;
        }
        
        if (!File.Exists(Path.Combine(path, "senha.json")))
        {
            SenhaPadrao();
        }
        
        try
        {
            string data;
            if (Application.isMobilePlatform)
            {
                //path = Application.persistentDataPath + Path.AltDirectorySeparatorChar + "senha" + Path.AltDirectorySeparatorChar;
                if (!Directory.Exists(path))
                {
                    Directory.CreateDirectory(path);
                }
                string filePath = Path.Combine(path + "senha.json");
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
                string filePath = System.IO.Path.Combine(path + "senha.json");
                data = System.IO.File.ReadAllText(filePath);
            }
            
            SerializableList<Senha> aux = JsonUtility.FromJson<SerializableList<Senha>>(data);
            senha = aux.Lista;

            Debug.Log("Arquivo lido de: " + path + "senha.json");
            return true;
        }
        catch (Exception ex)
        {
            Debug.Log("Erro ao ler: " + ex.ToString());
            return false;
        }
    }

    public void VerificarSenha()
    {
        LoadSenha();
        s = senha[0];
        if (Input.GetButtonDown("Submit") || click == true)
        {
            Senha();
            click = false;
        }
    }

    public void AlterarSenha()
    {
        s = senha[0];
        if(Input.GetButtonDown("Submit") || click == true)
        {
            if (password.text != s.senha)
            {
                mensagem.text = "Senha Atual Incorreta. Tente Novamente.";
                Invoke("Erase", 5);
            }
            else if(password.text == changePassword.text)
            {
                mensagem.text = "Digite uma senha diferente da atual.";
                Invoke("Erase", 5);
            }
            else
            {
                if (changePassword.text != matchedPassword.text)
                {
                    mensagem.text = "Senhas nao conferem. Digite novamente.";
                    Invoke("Erase", 5);
                }
                else if (changePassword.text != "")
                {
                    s.senha = changePassword.text;
                    SetSenha();
                    areaProf.SetActive(false);
                    senhaCorreta.SetActive(true);
                    changePassword.text = "";
                    password.text = "";
                    matchedPassword.text = "";
                    Erase();
                }
                else
                {
                    mensagem.text = "Digite uma nova senha.";
                    Invoke("Erase", 5);
                }
            }

            
        }
    }
    
    void SenhaPadrao()
    {
        s = new("1234");
        senha.Add(s);
        SetSenha();
    }
    void clicked()
    {
        click = true;
    }

    void Erase()
    {
        mensagem.text = "";
    }

    void Senha()
    {
        if (password.text == s.senha)
        {
            areaProf.SetActive(false);
            senhaCorreta.SetActive(true);
            password.text = "";
            Erase();
        }
        else
        {
            password.text = "";
            mensagem.text = "Senha Incorreta. Tente Novamente.";
            Invoke("Erase", 5);
        }
    }
}
