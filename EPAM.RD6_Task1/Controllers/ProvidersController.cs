using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using BLL.Services;
using BLL.DTO;
using Microsoft.AspNetCore.Mvc;

namespace EPAM.RD6_Task1.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProvidersController : ControllerBase
    {
        private ProvidersService _service;
        private ProvidersService Service
        {
            get
            {
                if (_service == null)
                {
                    _service = new ProvidersService();
                }
                return _service;
            }
        }

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]     // Created
        [ProducesResponseType(StatusCodes.Status400BadRequest)]  // BadRequest
        //Post api/providers
        public ActionResult<CategoryDTO> PostProvider([FromBody]ProviderDTO provider)
        {
            try
            {
                if (provider == null) return BadRequest();
                var result = Service.Create(provider);
                if (result == null) return BadRequest(result);
                return CreatedAtAction(nameof(GetProvider), new { id = result.Id }, result);
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
        //Get api/provider/id
        public ActionResult<ProviderDTO> GetProvider(int? id)
        {
            try
            {
                var provider = Service.GetById(id);
                if (provider == null)
                {
                    return NotFound();
                }
                else
                {
                    return Ok(provider);
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
        //Delete api/providers
        public ActionResult<ProviderDTO> DeleteProvider([FromBody]ProviderDTO provider)
        {
            try
            {
                if (provider == null) return BadRequest();
                Service.Delete(provider);
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
        //Put api/providers
        public ActionResult<ProviderDTO> UpdateProvider([FromBody]ProviderDTO provider)
        {
            try
            {
                if (provider == null) return BadRequest();
                Service.Update(provider);
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
        //Get api/provider
        public ActionResult<IEnumerable<ProviderDTO>> GetProviders()
        {
            try
            {
                var provider = Service.GetAll();
                if (provider == null)
                {
                    return NotFound();
                }
                else
                {
                    return Ok(provider);
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
        //Get api/providers/{id}/products
        public ActionResult<IEnumerable<ProductDTO>> GetAllProductsOfProvider(int? id)
        {
            try
            {
                var products = Service.GetProductsByProvider(id);
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
        [Route("filter")]
        [ProducesResponseType(StatusCodes.Status200OK)]     // Ok
        [ProducesResponseType(StatusCodes.Status404NotFound)]  // NotFound
        [ProducesResponseType(StatusCodes.Status400BadRequest)]  // BadRequest
        //Get api/products/filter
        public ActionResult<IEnumerable<ProviderDTO>> GetAllProvidersWithFilter([FromBody] IEnumerable<PropertyFilterDTO> propertyFilters)
        {
            try
            {
                var providers = Service.GetProvidersWithFilter(propertyFilters);
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
    }
}