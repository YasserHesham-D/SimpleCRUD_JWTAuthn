using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SimpleCRUD_JWTAuthn.Data;
using SimpleCRUD_JWTAuthn.Model;
using SimpleCRUD_JWTAuthn.Model.Dto_s;

namespace SimpleCRUD_JWTAuthn.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class ItemsController(AppDbContext context) : ControllerBase
    {
        [HttpPost]
        [Route("[Action]")]
        public IActionResult AddItem([FromBody] ItemDto Dto)
        {
            var newitem = new Item
            {
                Name = Dto.Name,
                Price = Dto.Price,
                InStock = Dto.InStock,
                Description = Dto.Description
            };

            context.Items.Add(newitem);
            context.SaveChanges();

            return Ok(newitem);
        }

        [HttpPut]
        [Route("[Action]{Id:Guid}")]
        public IActionResult EditItem([FromHeader]Guid Id,[FromBody] Item item)
        {
            var SelectedItem = context.Items.Find(Id);

            if(SelectedItem is null) return NotFound();

            SelectedItem.Name = item.Name;
            SelectedItem.Price = item.Price;
            SelectedItem.InStock = item.InStock;
            SelectedItem.Description = item.Description;

            context.Items.Add(SelectedItem);
            context.SaveChanges();

            return Ok(SelectedItem);
        }

        [HttpDelete]
        [Route("[Action]{Id:Guid}")]
        public IActionResult DeleteItem([FromHeader] Guid Id) 
        {
            var item = context.Items.Find(Id);

            if(item is null) return NotFound();

            context.Items.Remove(item);
            context.SaveChanges();

            return Ok();
        }

        [HttpGet]
        [Route("[Action]")]
        [AllowAnonymous]
        public IActionResult ShowItems() 
        {
            var Items = context.Items.ToList();
        
            return Ok(Items);
        }

        [HttpGet]
        [Route("[Action]{Id:Guid}")]
        public IActionResult ShowItems(Guid Id)
        {
            var Items = context.Items.Find(Id);

            if(Items is null) return NotFound();

            return Ok(Items);
        }
    }
}
