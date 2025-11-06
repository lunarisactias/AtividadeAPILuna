using Microsoft.AspNetCore.Mvc;
using API_Aula.Model;

namespace API_Aula.Controllers;
public class PlayerController : ControllerBase
{
    public static List<Player> players = new()
    {
        new Player() { id = "1", vida = 100, quantidadeDeItens = 5, posicaoX = 10, posicaoY = 20, posicaoZ = 30 },
        new Player() { id = "2", vida = 80, quantidadeDeItens = 3, posicaoX = 15, posicaoY = 25, posicaoZ = 35 },
        new Player() { id = "3", vida = 60, quantidadeDeItens = 7, posicaoX = 20, posicaoY = 30, posicaoZ = 40 }
    };

    [HttpGet]
    [Route("api/player")]
    public IActionResult GetPlayers()
    {
        return Ok(players);
    }

    [HttpGet]
    [Route("api/player/{id}")]
    public IActionResult GetPlayer(string id)
    {
        var player = players.FirstOrDefault(p => p.id == id);

        if (player == null)
        {
            return NotFound();
        }

        return Ok(player);
    }

    [HttpPost]
    [Route("api/player")]
    public IActionResult CreatePlayer([FromBody] Player newPlayer)
    {
        players.Add(newPlayer);
        return Ok(newPlayer);
    }

    [HttpPut]
    [Route("api/player/{id}")]
    public IActionResult UpdatePlayer(string id, [FromBody] Player updatedPlayer)
    {
        var player = players.FirstOrDefault(p => p.id == id);

        if (player == null)
        {
            return NotFound();
        }

        player.vida = updatedPlayer.vida;
        player.quantidadeDeItens = updatedPlayer.quantidadeDeItens;
        player.posicaoX = updatedPlayer.posicaoX;
        player.posicaoY = updatedPlayer.posicaoY;
        player.posicaoZ = updatedPlayer.posicaoZ;

        return Ok(player);
    }

    [HttpDelete]
    [Route("api/player/{id}")]
    public IActionResult DeletePlayer(string id)
    {
        var player = players.FirstOrDefault(p => p.id == id);

        if (player == null)
        {
            return NotFound();
        }

        players.Remove(player);

        return Ok();
    }
}
