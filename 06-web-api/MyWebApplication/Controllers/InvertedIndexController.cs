using Microsoft.AspNetCore.Mvc;
using Mohaymen.FullTextSearch.App;
using Mohaymen.FullTextSearch.App.Interfaces;
using Mohaymen.FullTextSearch.DocumentManagement.Models;
using MyWebApplication.Helpers;

namespace MyWebApplication.Controllers;

[ApiController]
[Route("[controller]")]
public class InvertedIndexController : ControllerBase
{
    private readonly IApplicationService _applicationService;
    public InvertedIndexController(IApplicationService applicationService)
    {
        _applicationService = applicationService;
    }
    [HttpGet]
    public IActionResult GetAll([FromQuery]QueryObject queryObject)
    {
        return Ok(
            _applicationService.Search(
            queryObject.MandatoryWords,
            queryObject.OptionalWords,
            queryObject.ExcludedWords
            )
        );
    }
}