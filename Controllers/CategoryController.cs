using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Shop.Models;

namespace Shop.Controllers
{
  [Route("categories")]
  public class CategoryController : ControllerBase
  {
    [HttpGet]
    [Route("")]
    public async Task<ActionResult<List<Category>>> Get()
    {
      return new List<Category>();
    }

    [HttpGet]
    [Route("{id:int}")]
    public async Task<ActionResult<Category>> GetById(int id)
    {
      var category = new Category{
        Id = id
      };

      return category;
    }

    [HttpPost]
    [Route("")]
    public async Task<ActionResult<Category>> Post([FromBody] Category model)
    {
      if (!ModelState.IsValid)
        return BadRequest(ModelState);
      
      return Ok(model);
    }

    [HttpPut]
    [Route("")]
    public async Task<ActionResult<Category>> Put([FromBody] Category model)
    {
      if (!ModelState.IsValid)
        return BadRequest(ModelState);

      return Ok(model);
      
    }

    [HttpDelete]
    [Route("{id:int}")]
    public async Task<ActionResult<string>> Delete(int id)
    {
      return Ok();
    }
  }
}