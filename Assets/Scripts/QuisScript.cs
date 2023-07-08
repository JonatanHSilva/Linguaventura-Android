
using System.Collections;
using System.Collections.Generic;
using System.IO;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Android;

[System.Serializable]
public class SerializableList<T>
{
    public List<T> Lista;
}
[System.Serializable]
public class Pergunta
{
    public string pergunta = "";
    public string respostaCerta = "";
    public string respostaErradaA = "";
    public string respostaErradaB = "";
    public string respostaErradaC = "";
    public int qtdRespostas = 0;
    public Pergunta() { qtdRespostas = 2; }
    public Pergunta(string pergunta, string respostaCerta, string respostaErrada)
    {
        this.pergunta = pergunta;
        this.respostaCerta = respostaCerta;
        this.respostaErradaA = respostaErrada;
        qtdRespostas = 2;
    }
    public Pergunta(string pergunta, string respostaCerta, string respostaErradaA, string respostaErradaB)
    {
        this.pergunta = pergunta;
        this.respostaCerta = respostaCerta;
        this.respostaErradaA = respostaErradaA;
        this.respostaErradaB = respostaErradaB;
        qtdRespostas = 3;
    }
    public Pergunta(string pergunta, string respostaCerta, string respostaErradaA, string respostaErradaB, string respostaErradaC)
    {
        this.pergunta = pergunta;
        this.respostaCerta = respostaCerta;
        this.respostaErradaA = respostaErradaA;
        this.respostaErradaB = respostaErradaB;
        this.respostaErradaC = respostaErradaC;
        qtdRespostas = 4;
    }
}

[System.Serializable]
public class Resposta
{
    public string pergunta = "";
    public string respostaCerta = "";
    public int quantidadeErros = 0;
    public Resposta(string pergunta, string respostaCerta,int quantidadeErros)
    {
        this.pergunta = pergunta;
        this.respostaCerta = respostaCerta;
        this.quantidadeErros = quantidadeErros;

    }
}

public class QuisScript : MonoBehaviour
{
    public GameObject PainelRespostas, quiz;
    public TextMeshProUGUI Pergunta;

    public int quantidadePerguntas = 10;
    public GameObject[] buttons;
    [SerializeField]
    public GameObject buttonCerto;
    [SerializeField]
    public GameObject buttonErrado;
    [SerializeField]
    public List<Pergunta> perguntas;
    public List<Pergunta> perguntasSelecionadas;
    [SerializeField]
    public List<Resposta> respostas = new List<Resposta>();
    Pergunta perguntaAtual;
    [SerializeField]
    public string ProximoLevel;
    [SerializeField]
    public string fileName;

    public string path;
    void Start()
    {
        path = Application.dataPath + Path.AltDirectorySeparatorChar + "data" + Path.AltDirectorySeparatorChar;
        LoadPerguntas();
        Random.InitState((int)(System.DateTime.Now.Second));
        SelecionarPergunta();
        IniciarButtons();
        SetResposta();
    }

    void Update()
    {

    }
    public bool SalvarPerguntas()
    {
        if (Application.isMobilePlatform)
        {
            path = Application.persistentDataPath + Path.AltDirectorySeparatorChar + "StreamingAssets" + Path.AltDirectorySeparatorChar + "data" + Path.AltDirectorySeparatorChar;
        }
        else
        {
            path = Application.dataPath + Path.AltDirectorySeparatorChar + "data" + Path.AltDirectorySeparatorChar;
        }

        try
        {
            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }
            SerializableList<Pergunta> aux = new SerializableList<Pergunta>();
            aux.Lista = perguntas;
            string jsonData = JsonUtility.ToJson(aux);
            System.IO.File.WriteAllText(path + fileName + ".json", jsonData);
            Debug.Log(jsonData);
            Debug.Log("Salvo em: " + path + fileName + ".json");
            return true;
        }
        catch (System.Exception ex)
        {
            Debug.Log("Erro ao salvar: " + ex.ToString());
            return false;
        }
    }
    public bool LoadPerguntas()
    {
        if (Application.isMobilePlatform)
        {
            path = Application.persistentDataPath + Path.AltDirectorySeparatorChar + "StreamingAssets" + Path.AltDirectorySeparatorChar + "data" + Path.AltDirectorySeparatorChar;
        }
        else
        {
            path = Application.dataPath + Path.AltDirectorySeparatorChar + "data" + Path.AltDirectorySeparatorChar;
        }
        
        if (!File.Exists(Path.Combine(path, fileName + ".json")))
        {
            SalvarPerguntas();
        }
        
        try
        {
            string data;
            if (Application.isMobilePlatform)
            {
                //path = Application.persistentDataPath + Path.AltDirectorySeparatorChar + "data" + Path.AltDirectorySeparatorChar;
                if (!Directory.Exists(path))
                {
                    Directory.CreateDirectory(path);
                }
                string filePath = Path.Combine(path, fileName + ".json");
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
                string filePath = System.IO.Path.Combine(path, fileName + ".json");
                data = System.IO.File.ReadAllText(filePath);
            }
            SerializableList<Pergunta> aux = JsonUtility.FromJson<SerializableList<Pergunta>>(data);
            perguntas = aux.Lista;

            Debug.Log("Arquivo lido de: " + path + fileName + ".json");
            return true;
        }
        catch (System.Exception ex)
        {
            Debug.Log("Erro no load: " + ex.ToString());
            return false;
        }
    }

    public bool SalvarRespostas()
    {
        try
        {
            if (!System.IO.Directory.Exists(path+ "Respostas"+ Path.AltDirectorySeparatorChar))
            {
                System.IO.Directory.CreateDirectory(path + "Respostas" + Path.AltDirectorySeparatorChar);
            }
            SerializableList<Resposta> aux = new SerializableList<Resposta>();
            aux.Lista = respostas;
            string jsonData = JsonUtility.ToJson(aux);
            System.IO.File.WriteAllText(path + "Respostas" + Path.AltDirectorySeparatorChar + fileName + ".json", jsonData);
            Debug.Log(jsonData);
            Debug.Log("Salvo em: " + path + "Respostas" + Path.AltDirectorySeparatorChar + fileName + ".json");
            return true;
        }
        catch (System.Exception ex)
        {
            Debug.Log("Erro ao salvar: " + ex.ToString());
            return false;
        }
    }

    private void SetResposta()
    {

        for (int i = 0; i < buttons.Length; i++) buttons[i].transform.SetParent(null);
        for (int i = 0; i < buttons.Length; i++)
        {

            GameObject temp = buttons[i];
            int r = Random.Range(i, buttons.Length);
            buttons[i] = buttons[r];
            buttons[r] = temp;
            buttons[i].transform.SetParent(PainelRespostas.transform);
        }

    }
    void SelecionarPergunta()
    {
        if (perguntas.Count == 0 || perguntas == null || perguntasSelecionadas.Count >= quantidadePerguntas)
        {
            SalvarRespostas();
            //SceneManager.LoadScene(ProximoLevel);
            return;
        }
        
        perguntaAtual = perguntas[Random.Range(0, perguntas.Count)];
        perguntasSelecionadas.Add(perguntaAtual);
        perguntas.Remove(perguntaAtual);
        respostas.Add(new Resposta(perguntaAtual.pergunta,perguntaAtual.respostaCerta,0));
    }
    void IniciarButtons()
    {
        foreach (var button in buttons)
        {
            Destroy(button.gameObject);
        }
        buttons = new GameObject[perguntaAtual.qtdRespostas];
        buttons[0] = Instantiate(buttonCerto, PainelRespostas.transform);
        for (int i = 1; i < perguntaAtual.qtdRespostas; i++)
        {
            buttons[i] = Instantiate(buttonErrado, PainelRespostas.transform);
        }
        Pergunta.text = perguntaAtual.pergunta;
        buttons[0].GetComponentInChildren<TextMeshProUGUI>().text = perguntaAtual.respostaCerta;
        buttons[1].GetComponentInChildren<TextMeshProUGUI>().text = perguntaAtual.respostaErradaA;
        if (perguntaAtual.qtdRespostas > 2)
        {
            buttons[2].GetComponentInChildren<TextMeshProUGUI>().text = perguntaAtual.respostaErradaB;
        }
        else
        {
            //buttons[2].SetActive(false); 
        }
        if (perguntaAtual.qtdRespostas > 3)
        {
            buttons[3].GetComponentInChildren<TextMeshProUGUI>().text = perguntaAtual.respostaErradaC;
        }
        else
        {
            //buttons[3].SetActive(false); 
        }

    }
    public void Certo()
    {
        quiz.SetActive(false);
        Time.timeScale = 1;
        Debug.Log("Certo: " + perguntaAtual.respostaCerta);
        SelecionarPergunta();
        IniciarButtons();
        SetResposta();
    }
    public void Errado()
    {
        respostas[respostas.Count - 1].quantidadeErros++;
        SelecionarPergunta();
        IniciarButtons();
        SetResposta();
        Time.timeScale = 1;
        quiz.SetActive(false);
    }
}