using BLL.DTO;
using BLL.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;

namespace EPAM.RD6_Task1.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductsController : ControllerBase
    {
        private ProductsService _service;
        private ProductsService Service
        {
            get
            {
                if (_service == null)
                {
                    _service = new ProductsService();
                }
                return _service;
            }
        }

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]     // Created
        [ProducesResponseType(StatusCodes.Status400BadRequest)]  // BadRequest
        //Post api/products
        public ActionResult<ProductDTO> PostProduct([FromBody]ProductDTO product)
        {
            try
            {
                if (product == null) return BadRequest();
                var result = Service.Create(product);
                if (result == null) return BadRequest(result);
                return CreatedAtAction(nameof(GetProduct), new { id = result.Id }, result);
            }
            catch (Exception)
            {
                return BadRequest();
            }
        }

        [HttpGet]
        [Route("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]     // Ok
        [ProducesResponseType(StatusCodes.Status404NotFound)]  // NotFound
        [ProducesResponseType(StatusCodes.Status400BadRequest)]  // BadRequest
        //Get api/product/id
        public ActionResult<ProductDTO> GetProduct(int? id)
        {
            try
            {
                var product = Service.GetById(id);
                if (product == null)
                {
                    return NotFound();
                }
                else
                {
                    return Ok(product);
                }
            }
            catch
            {
                return BadRequest();
            }
        }

        [HttpDelete]
        [ProducesResponseType(StatusCodes.Status204NoContent)]     // NoContent
        [ProducesResponseType(StatusCodes.Status404NotFound)]  // NotFound
        [ProducesResponseType(StatusCodes.Status400BadRequest)]  // BadRequest
        //Delete api/products
        public ActionResult<ProductDTO> DeleteProduct([FromBody]ProductDTO product)
        {
            try
            {
                if (product == null) return BadRequest();
                Service.Delete(product);
                return NoContent();
            }
            catch (ArgumentException)
            {
                return NotFound();
            }
            catch
            {
                return BadRequest();
            }
        }

        [HttpPut]
        [ProducesResponseType(StatusCodes.Status204NoContent)]     // NoContent
        [ProducesResponseType(StatusCodes.Status404NotFound)]  // NotFound
        [ProducesResponseType(StatusCodes.Status400BadRequest)]  // BadRequest
        //Put api/products
        public ActionResult<ProductDTO> UpdateProduct([FromBody]ProductDTO product)
        {
            try
            {
                if (product == null) return BadRequest();
                Service.Update(product);
                return NoContent();
            }
            catch (ArgumentException)
            {
                return NotFound();
            }
            catch
            {
                return BadRequest();
            }
        }

        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]     // Ok
        [ProducesResponseType(StatusCodes.Status404NotFound)]  // NotFound
        [ProducesResponseType(StatusCodes.Status400BadRequest)]  // BadRequest
        //Get api/products
        public ActionResult<IEnumerable<ProductDTO>> GetProducts()
        {
            try
            {
                var products = Service.GetAll();
                if (products == null)
                {
                    return NotFound();
                }
                else
                {
                    return Ok(products);
                }
            }
            catch
            {
                return BadRequest();
            }
        }

        [HttpGet]
        [Route("filter")]
        [ProducesResponseType(StatusCodes.Status200OK)]     // Ok
        [ProducesResponseType(StatusCodes.Status404NotFound)]  // NotFound
        [ProducesResponseType(StatusCodes.Status400BadRequest)]  // BadRequest
        //Get api/products/filter
        public ActionResult<IEnumerable<ProductDTO>> GetAllProductsWithFilter([FromBody] IEnumerable<PropertyFilterDTO> propertyFilters)
        {
            try
            {
                var products = Service.GetProductsWithFilter(propertyFilters);
                if (products == null)
                {
                    return NotFound();
                }
                else
                {
                    return Ok(products);
                }
            }
            catch (ArgumentException)
            {
                return NotFound();
            }
            catch (Exception)
            {
                return BadRequest();
            }
        }
    }
}