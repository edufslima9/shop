using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Shop.Data;
using Shop.Models;
using System;
using Microsoft.AspNetCore.Authorization;
using System.Linq;
using Shop.Services;

namespace Shop.Controllers
{
  [Route("users")]
  public class UserController : Controller
  {

    [HttpGet]
    [Route("")]
    [Authorize(Roles = "manager")]
    public async Task<ActionResult<List<User>>> Get([FromServices] DataContext context)
    {
      var users = await context.Users.AsNoTracking()
        .ToListAsync();
      return Ok(users);
    }

    [HttpPost]
    [Route("")]
    [AllowAnonymous]
    public async Task<ActionResult<User>> Create([FromServices] DataContext context, [FromBody] User model)
    {
      if (!ModelState.IsValid)
        return BadRequest(ModelState);

      try
      {
        // Força o usuário a ser sempre "funcionário"
        model.Role = "employee";

        context.Users.Add(model);
        await context.SaveChangesAsync();

        // Esconde a senha na response
        model.Password = "";

        return Ok(model);
      }
      catch (Exception)
      {
        return BadRequest(new { message = "Não foi possível criar o usuário" });
      }
    }

    [HttpPut]
    [Route("{id:int}")]
    [Authorize(Roles = "manager")]
    public async Task<ActionResult<User>> Put([FromServices] DataContext context, int id, [FromBody] User model)
    {
      if (!ModelState.IsValid)
        return BadRequest(ModelState);
      
      if (id != model.Id)
        return NotFound(new { message = "Usuário não encontrado" });

      try
      {
        context.Entry(model).State = EntityState.Modified;
        await context.SaveChangesAsync();

        model.Password = "";
        return Ok(model);
      }
      catch (Exception)
      {
        return BadRequest(new { message = "Não foi possível alterar o usuário" });
      }
    }

    [HttpPost]
    [Route("login")]
    public async Task<ActionResult<dynamic>> Authenticate([FromServices] DataContext context, [FromBody] User model)
    {
      var user = await context.Users.AsNoTracking()
        .Where(x => x.UserName == model.UserName && x.Password == model.Password)
        .FirstOrDefaultAsync();

      if (user == null)
        return NotFound(new { message = "Usuário ou senha inválidos" });

      var token = TokenService.GenerateToken(user);

      user.Password = "";
      return Ok(new
      {
        user = user,
        token = token
      });
    }
  }
}