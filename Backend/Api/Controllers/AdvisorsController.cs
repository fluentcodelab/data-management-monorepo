using CSharpFunctionalExtensions;
using DataManagement.Application.Contracts;
using DataManagement.Application.DTOs;
using DataManagement.Domain;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers;

[ApiController]
[Route(BaseApiPath + "[controller]")]
[Consumes("application/json")]
[Produces("application/json")]
[ProducesResponseType(StatusCodes.Status500InternalServerError)]
public class AdvisorsController(IAdvisorService service) : ControllerBase
{
    private const string BaseApiPath = "api/v1/";

    [HttpGet]
    [ProducesResponseType(typeof(IEnumerable<AdvisorDto>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetAll()
    {
        var list = await service.GetAsync();
        return Ok(list);
    }

    [HttpGet("{id:int}")]
    [ProducesResponseType(typeof(AdvisorDto), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(Error), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetById(int id)
    {
        var (_, isFailure, advisorDto, error) = await service.GetAsync(id);
        return isFailure ? NotFound(error) : Ok(advisorDto);
    }

    [HttpPost]
    [ProducesResponseType(typeof(AdvisorDto), StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(IEnumerable<Error>), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Add([FromBody] AdvisorCreationDto payload)
    {
        var (_, isFailure, advisorDto, errors) = await service.AddAsync(payload);
        return isFailure
            ? BadRequest(errors)
            : CreatedAtAction(nameof(GetById), new { id = advisorDto.Id }, advisorDto);
    }

    [HttpPut("{id:int}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(typeof(IEnumerable<Error>), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Update(int id, [FromBody] AdvisorUpdateDto payload)
    {
        var result = await service.UpdateAsync(id, payload);
        return result.IsFailure ? BadRequest(result.Error) : NoContent();
    }

    [HttpDelete("{id:int}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(typeof(Error), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Delete(int id)
    {
        var result = await service.DeleteAsync(id);
        return result.IsFailure ? BadRequest(result.Error) : NoContent();
    }
}