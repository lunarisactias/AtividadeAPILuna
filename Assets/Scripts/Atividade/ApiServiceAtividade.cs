using System;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

public class ApiServiceAtividade
{
    private readonly HttpClient httpClient;
    private const string BASE_URL = "https://68f95aabdeff18f212b9511d.mockapi.io";

    public ApiServiceAtividade()
    {
        httpClient = new HttpClient();
    }

    public async Task<PlayerClass> GetJogador(string id)
    {
        try
        {
            string url = $"{BASE_URL}/Player/{id}";
            Debug.Log($"GET: {url}");

            HttpResponseMessage response = await httpClient.GetAsync(url);
            response.EnsureSuccessStatusCode();

            string json = await response.Content.ReadAsStringAsync();
            Debug.Log($"Jogador recebido: {json}");

            PlayerClass jogador = JsonUtility.FromJson<PlayerClass>(json);
            return jogador;
        }
        catch (Exception ex)
        {
            Debug.LogError($"Erro ao buscar jogador {id}: {ex.Message}");
            return null;
        }
    }

    /// <summary>
    /// Atualiza dados do jogador
    /// </summary>
    public async Task<PlayerClass> AtualizarJogador(string id, PlayerClass jogador)
    {
        try
        {
            string url = $"{BASE_URL}/Player/{id}";
            Debug.Log($"PUT: {url}");

            string json = JsonUtility.ToJson(jogador);
            Debug.Log($"JSON sendo enviado: {json}");

            StringContent content = new StringContent(json, Encoding.UTF8, "application/json");

            HttpResponseMessage response = await httpClient.PutAsync(url, content);
            response.EnsureSuccessStatusCode();

            string responseJson = await response.Content.ReadAsStringAsync();
            Debug.Log($"Jogador atualizado: {responseJson}");

            return JsonUtility.FromJson<PlayerClass>(responseJson);
        }
        catch (Exception ex)
        {
            Debug.LogError($"Erro ao atualizar jogador {id}: {ex.Message}");
            return null;
        }
    }
    public async Task<PlayerClass> CriarJogador(PlayerClass jogador)
    {
        try
        {
            string url = $"{BASE_URL}/Player";
            Debug.Log($"POST: {url}");

            string json = JsonUtility.ToJson(jogador);
            Debug.Log($"JSON sendo enviado: {json}");

            StringContent content = new StringContent(json, Encoding.UTF8, "application/json");

            HttpResponseMessage response = await httpClient.PostAsync(url, content);
            response.EnsureSuccessStatusCode();

            string responseJson = await response.Content.ReadAsStringAsync();
            Debug.Log($"Jogador criado: {responseJson}");

            return JsonUtility.FromJson<PlayerClass>(responseJson);
        }
        catch (Exception ex)
        {
            Debug.LogError($"Erro ao criar jogador: {ex.Message}");
            return null;
        }
    }
}
