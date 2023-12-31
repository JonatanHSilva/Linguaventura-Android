using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class MovementPlayerJogo : MonoBehaviour
{
    public float speed;
    bool pause = false;
    public GameObject menu, mensagem, painel, proxPainel, roteiro1, roteiro2, roteiro3, roteiro4, msgVitoria;
    public Rigidbody2D rb;
    public Animator animator, vitoria;
    Vector2 direction;
    SetNomeJogador s;
    SetFaseScript f;
    int fase, prox = 0, acabou = 0;
    float time = 0;

    public TextMeshProUGUI nome;
    

    private void Start()
    {
        s = FindObjectOfType<SetNomeJogador>();
        f = FindObjectOfType<SetFaseScript>();
        fase = f.GetFase();
        if(fase == 1)
        {
            mensagem.SetActive(true);
        }
        if(fase == 5)
        {
            
            msgVitoria.SetActive(true);
        }
        Time.timeScale = 1;
        nome.text = s.GetNome();
    }


    private void Update()
    {
        /*direction.x = Input.GetAxisRaw("Horizontal");
        direction.y = Input.GetAxisRaw("Vertical");*/
        direction.Normalize();

        animator.SetFloat("Horizontal", direction.x);
        animator.SetFloat("Vertical", direction.y);
        animator.SetFloat("Speed", direction.sqrMagnitude);

        if(fase == 1)
        {
            if(prox == 0)
            {
                if (Input.GetKeyDown(KeyCode.Space) || Input.GetKeyDown(KeyCode.Return))
                {
                    roteiro1.SetActive(false);
                    roteiro2.SetActive(true);
                    prox = 1;
                }
            }
            else if(prox == 1)
            {
                if (Input.GetKeyDown(KeyCode.Space) || Input.GetKeyDown(KeyCode.Return))
                {
                    roteiro2.SetActive(false);
                    roteiro3.SetActive(true);
                    prox = 2;
                }
            }
            else if (prox == 2)
            {
                if (Input.GetKeyDown(KeyCode.Space) || Input.GetKeyDown(KeyCode.Return))
                {
                    roteiro3.SetActive(false);
                    roteiro4.SetActive(true);
                    prox = 3;
                }
            }
            else if (prox == 3)
            {
                if (Input.GetKeyDown(KeyCode.Space) || Input.GetKeyDown(KeyCode.Return))
                {
                    roteiro4.SetActive(false);
                    painel.SetActive(true);
                    prox = 4;
                }
            }
            else if (prox == 4)
            {
                if (Input.GetKeyDown(KeyCode.Space) || Input.GetKeyDown(KeyCode.Return))
                {
                    painel.SetActive(false);
                    proxPainel.SetActive(true);
                    prox = 5;
                }
            }
            else
            {
                if (Input.GetKeyDown(KeyCode.Space) || Input.GetKeyDown(KeyCode.Return))
                {
                    proxPainel.SetActive(false);
                }

            }
        }
        else if(fase == 5)
        {
            if (Input.GetKeyDown(KeyCode.Space) || Input.GetKeyDown(KeyCode.Return))
            {
                vitoria.SetTrigger("close");
                acabou = 1;
            }
            if(acabou == 1)
            {
                time += Time.deltaTime;
                if (time > 1) VoltarMenu();
            }
        }
        

        if (Input.GetButtonDown("Cancel"))
        {
            Pausa();
        }
    }

    private void FixedUpdate()
    {
        rb.MovePosition(rb.position + direction * speed * Time.fixedDeltaTime);
    }

    void VoltarMenu()
    {
        f.Reinicio();
        StartCoroutine(Menu());
    }

    IEnumerator Menu()
    {
        yield return new WaitForSeconds(.5f);
        SceneManager.LoadScene("Menu");
    }

    public void Pausa()
    {
        pause = !pause;
        if (pause)
        {
            menu.SetActive(true);
            Time.timeScale = 0;
        }
        else if (pause == false)
        {
            menu.SetActive(false);
            Time.timeScale = 1;
        }
    }

    void OnApplicationFocus(bool saiu){ 
         if(saiu) Pausa(); 
    }

    public void Up()
    {
        direction.y = 1;
    }
    public void Down()
    {
        direction.y = -1;
    }

    public void Left()
    {
        direction.x = -1;
    }

    public void Right()
    {
        direction.x = 1;
    }

    public void Stop()
    {
        direction = Vector2.zero;
    }
}

