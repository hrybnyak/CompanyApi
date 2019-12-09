using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using BLL.Services;
using BLL.DTO;
using Microsoft.AspNetCore.Http;
using System.Linq.Expressions;

namespace EPAM.RD6_Task1.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CategoriesController : Controller
    {
        private CategoriesService _service;
        private CategoriesService Service
        {
            get
            {
                if (_service == null)
                {
                    _service = new CategoriesService();
                }
                return _service;
            }
        }

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]     // Created
        [ProducesResponseType(StatusCodes.Status400BadRequest)]  // BadRequest
        //Post api/categories
        public ActionResult<CategoryDTO> PostCategory([FromBody]CategoryDTO category)
        {
            try
            {
                if (category == null) return BadRequest();
                var result = Service.Create(category);
                if (result == null) return BadRequest(result);
                return CreatedAtAction(nameof(GetCategory), new { id = result.Id }, result);
            }
            catch (Exception)
            {
                return BadRequest();
            }
        }

        [HttpDelete]
        [ProducesResponseType(StatusCodes.Status204NoContent)]     // NoContent
        [ProducesResponseType(StatusCodes.Status404NotFound)]  // NotFound
        [ProducesResponseType(StatusCodes.Status400BadRequest)]  // BadRequest
        //Delete api/categories
        public ActionResult<CategoryDTO> DeleteCategory([FromBody]CategoryDTO category)
        {
            try
            {
                if (category == null) return BadRequest();
                Service.Delete(category);
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
        //Put api/categories
        public ActionResult<CategoryDTO> UpdateCategory([FromBody]CategoryDTO category)
        {
            try
            {
                if (category == null) return BadRequest();
                Service.Update(category);
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
        [Route("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]     // Ok
        [ProducesResponseType(StatusCodes.Status404NotFound)]  // NotFound
        [ProducesResponseType(StatusCodes.Status400BadRequest)]  // BadRequest
        //Get api/categories/id
        public ActionResult<CategoryDTO> GetCategory(int? id)
        {
            try
            {
                var category = Service.GetById(id);
                if (category == null)
                {
                    return NotFound();
                }
                else
                {
                    return Ok(category);
                }
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
        //Get api/categories
        public ActionResult<IEnumerable<CategoryDTO>> GetCategories()
        {
            try
            {
                var categories = Service.GetAll();
                if (categories == null)
                {
                    return NotFound();
                }
                else
                {
                    return Ok(categories);
                }
            }
            catch
            {
                return BadRequest();
            }
        }

        [HttpGet]
        [Route("{id}/products")]
        [ProducesResponseType(StatusCodes.Status200OK)]     // Ok
        [ProducesResponseType(StatusCodes.Status404NotFound)]  // NotFound
        [ProducesResponseType(StatusCodes.Status400BadRequest)]  // BadRequest
        //Get api/categories/{id}/products
        public ActionResult<IEnumerable<ProductDTO>> GetAllProductsOfCategory(int? id)
        {
            try
            {
                var products = Service.GetProductsByCategory(id);
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

        [HttpGet]
        [Route("{id}/providers")]
        [ProducesResponseType(StatusCodes.Status200OK)]     // Ok
        [ProducesResponseType(StatusCodes.Status404NotFound)]  // NotFound
        [ProducesResponseType(StatusCodes.Status400BadRequest)]  // BadRequest
        //Get api/categories/{id}/providers
        public ActionResult<IEnumerable<ProductDTO>> GetAllProvidersOfCategory(int? id)
        {
            try
            {
                var providers = Service.GetProvidersByCategory(id);
                if (providers == null)
                {
                    return NotFound();
                }
                else
                {
                    return Ok(providers);
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

        [HttpGet]
        [Route("filter")]
        [ProducesResponseType(StatusCodes.Status200OK)]     // Ok
        [ProducesResponseType(StatusCodes.Status404NotFound)]  // NotFound
        [ProducesResponseType(StatusCodes.Status400BadRequest)]  // BadRequest
        //Get api/categories/filter
        public ActionResult<IEnumerable<CategoryDTO>> GetAllCategoriesWithFilter([FromBody] IEnumerable<PropertyFilterDTO> propertyFilters)
        {
            try
            {
                var categories = Service.GetCategoriesWithFilter(propertyFilters);
                if (categories == null)
                {
                    return NotFound();
                }
                else
                {
                    return Ok(categories);
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