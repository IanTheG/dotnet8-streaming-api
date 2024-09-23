// using MediatR;

// using Microsoft.AspNetCore.Mvc;

// namespace api.Controllers;

// [ApiController]
// public abstract class ApiControllerBase : ControllerBase
// {
//     private ISender _mediator = null!;

//     protected ISender Mediator => _mediator ??= HttpContext.RequestServices.GetRequiredService<ISender>();

//     /// <summary>
//     /// This method parses the response from commands and queries into the 200 or 400 API response
//     /// </summary>
//     /// <typeparam name="T"></typeparam>
//     /// <param name="response"></param>
//     /// <returns></returns>
//     public ActionResult HandleResult<T>(ResponseVm<T> response)
//     {
//         return StatusCode(Convert.ToInt32(response.StatusCode), response);
//     }
// }
