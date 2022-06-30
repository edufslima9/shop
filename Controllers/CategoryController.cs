using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Shop.Data;
using Shop.Models;

namespace Shop.Controllers
{
  [Route("categories")]
  public class CategoryController : ControllerBase
  {
    [HttpGet]
    [Route("")]
    public async Task<ActionResult<List<Category>>> Get([FromServices] DataContext context)
    {
      var categories = await context.Categories.AsNoTracking().ToListAsync();
      return Ok(categories);
    }

    [HttpGet]
    [Route("{id:int}")]
    public async Task<ActionResult<Category>> GetById(int id, [FromServices] DataContext context)
    {
      var category = await context.Categories.AsNoTracking().FirstOrDefaultAsync(x => x.Id == id);
      if (category == null)
        return BadRequest( new { message = "Categoria não encontrada" });

      return Ok(category);
    }

    [HttpPost]
    [Route("")]
    public async Task<ActionResult<Category>> Post([FromBody] Category model, [FromServices] DataContext context)
    {
      if (!ModelState.IsValid)
        return BadRequest(ModelState);
      
      try
      {
        context.Categories.Add(model);
        await context.SaveChangesAsync();
        
        return Ok(model);
      }
      catch
      {
        return BadRequest(new { message = "Não foi possível criar a categoria" });
      }
    }

    [HttpPut]
    [Route("")]
    public async Task<ActionResult<Category>> Put([FromBody] Category model, [FromServices] DataContext context)
    {
      if (!ModelState.IsValid)
        return BadRequest(ModelState);

      try
      {
        context.Entry<Category>(model).State = EntityState.Modified;
        await context.SaveChangesAsync();
        return Ok(model);
      }
      catch (DbUpdateConcurrencyException)
      {
        return BadRequest(new { message = "Esta categoria já foi atualizada" });
      }
      catch (Exception)
      {
        return BadRequest(new { message = "Não foi possível atualizar a categoria" });
      }
    }

    [HttpDelete]
    [Route("{id:int}")]
    public async Task<ActionResult<string>> Delete(int id, [FromServices] DataContext context)
    {
      var category = await context.Categories.FirstOrDefaultAsync(x => x.Id == id);
      if (category == null)
        return BadRequest( new { message = "Categoria não encontrada" });

      try
      {
        context.Categories.Remove(category);
        await context.SaveChangesAsync();
        return Ok(new { message = "Categoria removida com sucesso" });
      } 
      catch (Exception)
      {
        return BadRequest(new { message = "Não foi possível remover a categoria" });
      }
    }
  }
}