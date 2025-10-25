using UnityEngine;
using TMPro;
using System.Collections;
using System.Threading.Tasks;

public class Player : MonoBehaviour
{
    [Header("Player Stats")]
    public int health = 100;
    public int totalItems = 0;
    public int xPos;
    public int yPos;
    public int zPos;
    public int speed = 5;

    [Header("UI Elements")]
    public GameObject menuPanel;
    public TextMeshProUGUI healthText;
    public TextMeshProUGUI itemsText;
    public TextMeshProUGUI posXText;
    public TextMeshProUGUI posYText;
    public TextMeshProUGUI posZText;
    public TextMeshProUGUI savingText;

    private ApiServiceAtividade apiService;
    PlayerClass criadoJogador1;

    async void Start()
    {
        apiService = new ApiServiceAtividade();

        criadoJogador1 = await apiService.GetJogador("1");
        health = criadoJogador1.vida;
        totalItems = criadoJogador1.quantidadeDeItens;

        StartCoroutine(UpdatePosition());
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            menuPanel.SetActive(!menuPanel.activeSelf);
        }

        healthText.text = "Vida: " + criadoJogador1.vida.ToString();
        itemsText.text = "Quantidade de itens: " + criadoJogador1.quantidadeDeItens.ToString();
        posXText.text = "Posição X: " + criadoJogador1.posicaoX.ToString();
        posYText.text = "Posição Y: " + criadoJogador1.posicaoY.ToString();
        posZText.text = "Posição Z: " + criadoJogador1.posicaoZ.ToString();

        Move();
    }

    public void Move()
    {
        float moveX = Input.GetAxis("Horizontal");
        float moveY = Input.GetAxis("Vertical");
        
        Vector2 movement = new Vector2(moveX, moveY) * speed * Time.deltaTime;

        transform.Translate(new Vector3(movement.x, movement.y, 0));

        
    }

    private async void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision != null)
        {
            if (collision.gameObject.CompareTag("Item"))
            {
                totalItems += 1;
                criadoJogador1.quantidadeDeItens = totalItems;
                PlayerClass atualizadoJogador1 = await apiService.AtualizarJogador(criadoJogador1.id, criadoJogador1);
                StartCoroutine(ShowSavingText());
                Destroy(collision.gameObject);
            }
            if (collision.gameObject.CompareTag("Spike"))
            {
                health -= 10;
                criadoJogador1.vida = health;
                PlayerClass atualizadoJogador1 = await apiService.AtualizarJogador(criadoJogador1.id, criadoJogador1);
                Destroy(collision.gameObject);

            }
        }
    }

    public void ResetData()
    {
        totalItems = 0;
        health = 100;
        criadoJogador1.vida = 100;
        criadoJogador1.quantidadeDeItens = 0;
        criadoJogador1.posicaoX = 0;
        criadoJogador1.posicaoY = 0;
        criadoJogador1.posicaoZ = 0;

        UpdatePlayerData();
    }

    IEnumerator ShowSavingText()
    {
        savingText.gameObject.SetActive(true);
        yield return new WaitForSeconds(2);
        savingText.gameObject.SetActive(false);
    }

    IEnumerator UpdatePosition()
    {
        while (true)
        {

            criadoJogador1.posicaoX = Mathf.RoundToInt(transform.position.x);
            criadoJogador1.posicaoY = Mathf.RoundToInt(transform.position.y);
            criadoJogador1.posicaoZ = Mathf.RoundToInt(transform.position.z);

            UpdatePlayerData();

            yield return new WaitForSeconds(1f);
        }
    }

    public async void UpdatePlayerData()
    {
        PlayerClass atualizadoJogador1 = await apiService.AtualizarJogador(criadoJogador1.id, criadoJogador1);
    }
}
