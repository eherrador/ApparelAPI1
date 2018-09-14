﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ApparelAPI.Models;

/*
We are using Entity Framework Core InMemory database provider.
This database provider allows Entity Framework Core to be used with an in-memory database.
https://docs.microsoft.com/es-es/ef/core/providers/in-memory/
https://www.nuget.org/packages/Microsoft.EntityFrameworkCore.InMemory/2.2.0-preview1-35029
*/
namespace ApparelAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ApparelsController : ControllerBase
    {
        private readonly CatalogContext _context;

        public ApparelsController(CatalogContext context)
        {
            _context = context;

            if (_context.Products.Count() == 0)
            {
                // Create a new Product if collection is empty,
                _context.Products.Add(new Product { SKU = "1A", Quantity = 10, Name = "Khaki" });
                _context.Products.Add(new Product { SKU = "2B", Quantity = 20, Name = "Puffer" });
                _context.Products.Add(new Product { SKU = "3C", Quantity = 30, Name = "Chino" });
                _context.SaveChanges();
            }
        }

        // GET api/Apparels
        [HttpGet]
        public IActionResult Get()
        {
            return Ok(_context.Products.ToList());
        }

        // GET api/Apparels/5
        [HttpGet("{sku}")]
        public IActionResult Get(string sku)
        {
            Product item = _context.Products.Where(b => b.SKU.Contains(sku)).First();
            if (item == null)
            {
                return NotFound();
            }
            return Ok(item);
        }

        // POST api/Apparels
        [HttpPost("{sku}")]
        public IActionResult Post(string sku, [FromBody] int quantity)
        {
            Product item = _context.Products.Where(b => b.SKU.Contains(sku)).First();
            if (item == null)
            {
                return NotFound();
            }

            item.Quantity = quantity;

            _context.Products.Update(item);
            _context.SaveChanges();
            return Ok(item);
        }

        // PUT api/Apparels/5
        [HttpPut("{sku}")]
        public IActionResult Put(string sku, [FromBody] Product product)
        {
            var item = _context.Products.Where(b => b.SKU.Contains(sku)).First();
            if (item == null)
            {
                return NotFound();
            }

            item.SKU = product.SKU;
            item.Quantity = product.Quantity;
            item.Name = product.Name;

            _context.Products.Update(item);
            _context.SaveChanges();
            return NoContent();
        }

        // DELETE api/Apparels/5
        [HttpDelete("{sku}")]
        public IActionResult Delete(string sku)
        {
            var item = _context.Products.Where(b => b.SKU.Contains(sku)).First();
            if (item == null)
            {
                return NotFound();
            }

            _context.Products.Remove(item);
            _context.SaveChanges();
            return NoContent();
        }
    }
}
