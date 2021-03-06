using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Shop.Data;
using Shop.Models;

namespace Shop.Controllers
{
  [Route("products")]
  public class ProductController : ControllerBase
  {
    [HttpGet]
    [Route("")]
    [AllowAnonymous]
    public async Task<ActionResult<List<Product>>> Get([FromServices] DataContext context)
    {
      var products = await context.Products
      .Include(x => x.Category)
      .AsNoTracking()
      .ToListAsync();

      return Ok(products);
    }

    [HttpGet]
    [Route("{id:int}")]
    [AllowAnonymous]
    public async Task<ActionResult<Product>> GetById(int id, [FromServices] DataContext context)
    {
      var product = await context.Products
        .Include(x => x.Category)
        .AsNoTracking()
        .FirstOrDefaultAsync(x => x.Id == id);

      if (product == null)
        return BadRequest( new { message = "Produto não encontrado" });

      return Ok(product);
    }

    [HttpGet]
    [Route("categories/{id:int}")]
    [AllowAnonymous]
    public async Task<ActionResult<List<Product>>> GetByCategoryId(int categoryId, [FromServices] DataContext context)
    {
      var products = await context.Products
        .Include(x => x.Category)
        .AsNoTracking()
        .Where(x => x.CategoryId == categoryId)
        .ToListAsync();
        
      return Ok(products);
    }

    [HttpPost]
    [Route("")]
    [Authorize(Roles = "employee")]
    public async Task<ActionResult<Product>> Post([FromBody] Product model, [FromServices] DataContext context)
    {
      if (!ModelState.IsValid)
        return BadRequest(ModelState);
      
      try
      {
        context.Products.Add(model);
        await context.SaveChangesAsync();
        
        return Ok(model);
      }
      catch
      {
        return BadRequest(new { message = "Não foi possível criar o produto" });
      }
    }

    [HttpPut]
    [Route("")]
    [Authorize(Roles = "manager")]
    public async Task<ActionResult<Product>> Put([FromBody] Product model, [FromServices] DataContext context)
    {
      if (!ModelState.IsValid)
        return BadRequest(ModelState);

      try
      {
        context.Entry<Product>(model).State = EntityState.Modified;
        await context.SaveChangesAsync();
        return Ok(model);
      }
      catch (DbUpdateConcurrencyException)
      {
        return BadRequest(new { message = "Este produto já foi atualizada" });
      }
      catch (Exception)
      {
        return BadRequest(new { message = "Não foi possível atualizar o produto" });
      }
    }

    [HttpDelete]
    [Route("{id:int}")]
    [Authorize(Roles = "manager")]
    public async Task<ActionResult<string>> Delete(int id, [FromServices] DataContext context)
    {
      var product = await context.Products.FirstOrDefaultAsync(x => x.Id == id);
      if (product == null)
        return BadRequest( new { message = "Produto não encontrado" });

      try
      {
        context.Products.Remove(product);
        await context.SaveChangesAsync();
        return Ok(new { message = "Produto removido com sucesso" });
      } 
      catch (Exception)
      {
        return BadRequest(new { message = "Não foi possível remover o produto" });
      }
    }
  }
}