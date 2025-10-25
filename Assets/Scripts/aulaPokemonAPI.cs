using UnityEngine;
using System.Net.Http;
using System.Threading.Tasks;
using UnityEngine.UI;
using UnityEngine.Networking;
using System.Collections;

public class aulaPokemonAPI : MonoBehaviour
{
    private readonly HttpClient httpClient = new HttpClient();
    private const string baseUrl = "https://pokeapi.co/api/v2/pokemon/";
    public Image pikachuImg;


    async void Start()
    {
        Pokemon pikachu = await GetDadosPokemon("ditto");

        if (pikachu != null)
        {
            Debug.Log($"Nome: {pikachu.name}");
            Debug.Log($"ID: {pikachu.id}");
            Debug.Log($"Altura: {pikachu.height}");
            Debug.Log($"Peso: {pikachu.weight}");
            Debug.Log($"Experiência Base: {pikachu.base_experience}");

            if (pikachu.types != null && pikachu.types.Length > 0)
            {
                Debug.Log($"Tipo Principal: {pikachu.types[0].type.name}");
            }

            // Carrega a imagem do Pikachu na variável pikachuImg
            StartCoroutine(LoadPokemonSprite(pikachu.sprites.front_default));


        }
    }
    public async Task<Pokemon> GetDadosPokemon(string nome)
    {

        string url = baseUrl + nome.ToLower();

        HttpResponseMessage response = await httpClient.GetAsync(url);
        string jsonResponse = await response.Content.ReadAsStringAsync();
        Pokemon pokemon = JsonUtility.FromJson<Pokemon>(jsonResponse);
        return pokemon;
    }
    IEnumerator LoadPokemonSprite(string spriteUrl)
    {
        using (UnityWebRequest request = UnityWebRequestTexture.GetTexture(spriteUrl))
        {
            yield return request.SendWebRequest();

            if (request.result == UnityWebRequest.Result.Success)
            {
                Texture2D texture = ((DownloadHandlerTexture)request.downloadHandler).texture;
                Sprite sprite = Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), Vector2.one * 0.5f);
                pikachuImg.sprite = sprite;
                Debug.Log("Imagem do Pikachu carregada com sucesso!");
            }
            else
            {
                Debug.LogError($"Erro ao carregar imagem: {request.error}");
            }
        }
    }

    // Update is called once per frame
    void Update()
    {

    }

    void OnDestroy()
    {
        httpClient?.Dispose();
    }
}
