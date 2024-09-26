using AutoMapper;
using DynamicObjectAPI.Core.DTOs;
using DynamicObjectAPI.Core.Models;
using DynamicObjectAPI.Service.Services.Abstract;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DynamicObjectAPI.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DynamicObjectsController : ControllerBase
    {
        private readonly IDynamicObjectService _dynamicObjectService;
        private readonly IMapper _mapper;

        public DynamicObjectsController(IDynamicObjectService dynamicObjectService, IMapper mapper)
        {
            _dynamicObjectService = dynamicObjectService;
            _mapper = mapper;
        }

        
        [HttpGet]
        public async Task<ActionResult<IEnumerable<DynamicObjectDto>>> GetAll()
        {
            var dynamicObjects = await _dynamicObjectService.GetAllObjectsAsync();
            var dynamicObjectDtos = _mapper.Map<IEnumerable<DynamicObjectDto>>(dynamicObjects);
            return Ok(dynamicObjectDtos);
        }

        
        [HttpGet("{id}")]
        public async Task<ActionResult<DynamicObjectDto>> GetById(int id)
        {
            var dynamicObject = await _dynamicObjectService.GetObjectByIdAsync(id);
            if (dynamicObject == null)
                return NotFound();

            var dynamicObjectDto = _mapper.Map<DynamicObjectDto>(dynamicObject);
            return Ok(dynamicObjectDto);
        }

     
        [HttpPost]
        public async Task<ActionResult> Create([FromBody] DynamicObjectDto dynamicObjectDto)
        {
            if (string.IsNullOrEmpty(dynamicObjectDto.ObjectType))
            {
                ModelState.AddModelError("ObjectType", "ObjectType is required.");
                return BadRequest(ModelState);
            }

            if (dynamicObjectDto.Fields == null || dynamicObjectDto.Fields.Count == 0)
            {
                ModelState.AddModelError("Fields", "Fields cannot be empty.");
                return BadRequest(ModelState);
            }

            var dynamicObject = _mapper.Map<DynamicObject>(dynamicObjectDto);

            await _dynamicObjectService.CreateObjectAsync(dynamicObject);
            return CreatedAtAction(nameof(GetById), new { id = dynamicObject.Id }, dynamicObjectDto);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] DynamicObjectDto dynamicObjectDto)
        {
            if (string.IsNullOrEmpty(dynamicObjectDto.ObjectType))
            {
                ModelState.AddModelError("ObjectType", "ObjectType is required.");
                return BadRequest(ModelState);
            }

            if (dynamicObjectDto.Fields == null || dynamicObjectDto.Fields.Count == 0)
            {
                ModelState.AddModelError("Fields", "Fields cannot be empty.");
                return BadRequest(ModelState);
            }

            var existingObject = await _dynamicObjectService.GetObjectByIdAsync(id);
            if (existingObject == null)
                return NotFound();

            var dynamicObject = _mapper.Map<DynamicObject>(dynamicObjectDto);

            await _dynamicObjectService.UpdateObjectAsync(dynamicObject);
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                var existingObject = await _dynamicObjectService.GetObjectByIdAsync(id);
                if (existingObject == null)
                {
                    return NotFound();
                }

                await _dynamicObjectService.DeleteObjectAsync(id);
                return NoContent();
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error occurred: {ex.Message}"); 
            }
        }


    }
}
