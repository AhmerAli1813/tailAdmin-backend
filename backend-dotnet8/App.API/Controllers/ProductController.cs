using App.Services.Dto.General;
using App.Services.Dto.Log;
using App.Services.Dto.Product;
using App.Services.Helper;
using App.Services.Interface;
using Microsoft.AspNetCore.Http.Extensions;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Diagnostics;

namespace App.API.Controllers;

[Route("api/[controller]")]
[ApiController]
public class ProductController : ControllerBase
{
    private readonly IProductService _Service;
    private readonly ILogService _LogService;
    private readonly IHttpContextAccessor _HttpContextAccessor;

    public ProductController(IProductService service, ILogService logService, IHttpContextAccessor httpContextAccessor)
    {
        _Service = service;
        _LogService = logService;
        _HttpContextAccessor = httpContextAccessor;
    }

    [HttpPost("add")]
    public async Task<IActionResult> Add(ProductDto req)
    {
        ResponseDto resp = new ResponseDto();
        var reqTime = DateTime.UtcNow;
        var stopwatch = Stopwatch.StartNew(); // Start timing
        long? LogId = null;
        try
        {
            if (!ModelState.IsValid)
            {
                resp.Error = ModelState.Values
                    .SelectMany(v => v.Errors)
                    .Select(e => e.ErrorMessage)
                    .ToList();
                return BadRequest(resp);
            }

            resp = await _Service.CreateAsync(req);
            stopwatch.Stop(); // Stop timing
            if (resp.IsSucceed)
            {
                LogId = await _LogService.LogInformation(resp.Message, User.Identity.Name);
                return Ok(resp);
            }
            LogId = await _LogService.LogError(resp.Message, User.Identity.Name);

            return StatusCode(resp.StatusCode, resp.Message);
        }
        catch (Exception ex)
        {
            resp.IsSucceed = false;
            resp.Message = "An error occurred while processing the request.";
            resp.Error = new List<string> { ex.Message };
            resp.StatusCode = StatusCodes.Status500InternalServerError;

            // Log Exception Details
            LogId = await _LogService.LogException(ex.Message, User.Identity.Name, ex);

            return StatusCode(500, resp);
        }
        finally
        {
            // Log the request and response details
            var logs = new OpenApiReqLogDto
            {
                CallType = _HttpContextAccessor.HttpContext.Request.GetDisplayUrl(),
                ReqData = JsonConvert.SerializeObject(req),
                ResData = JsonConvert.SerializeObject(resp),
                ReqTime = reqTime,
                StatusCode = resp.StatusCode,
                ResTime = DateTime.Now, // Capture execution time
                ExecutionTime = stopwatch.ElapsedMilliseconds, // Capture execution time
                ServiceUtilizer = "POS",
                UserName = User?.Identity?.Name ?? "Anonymous",
                TransTraceId = LogId,


            };
            await _LogService.SaveRequestLog(_HttpContextAccessor.HttpContext, logs);
        }
    }
    [HttpPost("Update")]
    public async Task<IActionResult> Update(ProductDto req)
    {
        ResponseDto resp = new ResponseDto();
        var reqTime = DateTime.UtcNow;
        var stopwatch = Stopwatch.StartNew(); // Start timing
        long? LogId = null;
        try
        {
            if (!ModelState.IsValid)
            {
                resp.Error = ModelState.Values
                    .SelectMany(v => v.Errors)
                    .Select(e => e.ErrorMessage)
                    .ToList();
                return BadRequest(resp);
            }
            int Id = Convert.ToInt32(CryptoHelper.Decrypt(req.Id));
            resp = await _Service.UpdateAsync(Id, req);
            stopwatch.Stop(); // Stop timing
            if (resp.IsSucceed)
            {
                LogId = await _LogService.LogInformation(resp.Message, User.Identity.Name);
                return Ok(resp);
            }
            LogId = await _LogService.LogError(resp.Message, User.Identity.Name);

            return StatusCode(resp.StatusCode, resp.Message);
        }
        catch (Exception ex)
        {
            resp.IsSucceed = false;
            resp.Message = "An error occurred while processing the request.";
            resp.Error = new List<string> { ex.Message };
            resp.StatusCode = StatusCodes.Status500InternalServerError;

            // Log Exception Details
            LogId = await _LogService.LogException(ex.Message, User.Identity.Name, ex);

            return StatusCode(500, resp);
        }
        finally
        {
            // Log the request and response details
            var logs = new OpenApiReqLogDto
            {
                CallType = _HttpContextAccessor.HttpContext.Request.GetDisplayUrl(),
                ReqData = JsonConvert.SerializeObject(req),
                ResData = JsonConvert.SerializeObject(resp),
                ReqTime = reqTime,
                StatusCode = resp.StatusCode,
                ResTime = DateTime.Now, // Capture execution time
                ExecutionTime = stopwatch.ElapsedMilliseconds, // Capture execution time
                ServiceUtilizer = "POS",
                UserName = User?.Identity?.Name ?? "Anonymous",
                TransTraceId = LogId,


            };
            await _LogService.SaveRequestLog(_HttpContextAccessor.HttpContext, logs);
        }
    }
    [HttpPost("delete")]
    public async Task<IActionResult> Delete(string EncryptId)
    {
        ResponseDto resp = new ResponseDto();
        var reqTime = DateTime.UtcNow;
        var stopwatch = Stopwatch.StartNew(); // Start timing
        long? LogId = null;
        try
        {
            if (!ModelState.IsValid)
            {
                resp.Error = ModelState.Values
                    .SelectMany(v => v.Errors)
                    .Select(e => e.ErrorMessage)
                    .ToList();
                return BadRequest(resp);
            }
            int Id = Convert.ToInt32(CryptoHelper.Decrypt(EncryptId));
            resp = await _Service.DeleteAsync(Id);
            stopwatch.Stop(); // Stop timing
            if (resp.IsSucceed)
            {
                LogId = await _LogService.LogInformation(resp.Message, User.Identity.Name);
                return Ok(resp);
            }
            LogId = await _LogService.LogError(resp.Message, User.Identity.Name);

            return StatusCode(resp.StatusCode, resp.Message);
        }
        catch (Exception ex)
        {
            resp.IsSucceed = false;
            resp.Message = "An error occurred while processing the request.";
            resp.Error = new List<string> { ex.Message };
            resp.StatusCode = StatusCodes.Status500InternalServerError;

            // Log Exception Details
            LogId = await _LogService.LogException(ex.Message, User.Identity.Name, ex);

            return StatusCode(500, resp);
        }
        finally
        {
            // Log the request and response details
            var logs = new OpenApiReqLogDto
            {
                CallType = _HttpContextAccessor.HttpContext.Request.GetDisplayUrl(),
                ReqData = JsonConvert.SerializeObject(_HttpContextAccessor.HttpContext.Request.BodyReader),
                ResData = JsonConvert.SerializeObject(resp),
                ReqTime = reqTime,
                StatusCode = resp.StatusCode,
                ResTime = DateTime.Now, // Capture execution time
                ExecutionTime = stopwatch.ElapsedMilliseconds, // Capture execution time
                ServiceUtilizer = "POS",
                UserName = User?.Identity?.Name ?? "Anonymous",
                TransTraceId = LogId,


            };
            await _LogService.SaveRequestLog(_HttpContextAccessor.HttpContext, logs);
        }
    }
    [HttpPost("recycle")]
    public async Task<IActionResult> Recycle(string Name)
    {
        ResponseDto resp = new ResponseDto();
        var reqTime = DateTime.UtcNow;
        var stopwatch = Stopwatch.StartNew(); // Start timing
        long? LogId = null;
        try
        {
            if (string.IsNullOrEmpty(Name))
            {
                resp.Message = "Product Name is Required";
                resp.IsSucceed = false;
                return BadRequest(resp);
            }
            resp = await _Service.RecycleItemAsync(Name);
            stopwatch.Stop(); // Stop timing
            if (resp.IsSucceed)
            {
                LogId = await _LogService.LogInformation(resp.Message, User.Identity.Name);
                return Ok(resp);
            }
            LogId = await _LogService.LogError(resp.Message, User.Identity.Name);

            return StatusCode(resp.StatusCode, resp.Message);
        }
        catch (Exception ex)
        {
            resp.IsSucceed = false;
            resp.Message = "An error occurred while processing the request.";
            resp.Error = new List<string> { ex.Message };
            resp.StatusCode = StatusCodes.Status500InternalServerError;

            // Log Exception Details
            LogId = await _LogService.LogException(ex.Message, User.Identity.Name, ex);

            return StatusCode(500, resp);
        }
        finally
        {
            // Log the request and response details
            var logs = new OpenApiReqLogDto
            {
                CallType = _HttpContextAccessor.HttpContext.Request.GetDisplayUrl(),
                ReqData = JsonConvert.SerializeObject(_HttpContextAccessor.HttpContext.Request.BodyReader),
                ResData = JsonConvert.SerializeObject(resp),
                ReqTime = reqTime,
                StatusCode = resp.StatusCode,
                ResTime = DateTime.Now, // Capture execution time
                ExecutionTime = stopwatch.ElapsedMilliseconds, // Capture execution time
                ServiceUtilizer = "POS",
                UserName = User?.Identity?.Name ?? "Anonymous",
                TransTraceId = LogId,


            };
            await _LogService.SaveRequestLog(_HttpContextAccessor.HttpContext, logs);
        }
    }
    [HttpPost("active")]
    public async Task<IActionResult> Active(ProductActiveDto req)
    {
        ResponseDto resp = new ResponseDto();
        var reqTime = DateTime.UtcNow;
        var stopwatch = Stopwatch.StartNew(); // Start timing
        long? LogId = null;
        try
        {
            if (!ModelState.IsValid)
            {
                resp.Error = ModelState.Values
                    .SelectMany(v => v.Errors)
                    .Select(e => e.ErrorMessage)
                    .ToList();
                return BadRequest(resp);
            }
            int Id = Convert.ToInt32(CryptoHelper.Decrypt(req.EncryptId));
            resp = await _Service.ActiveAsync(Id,req.IsActive);
            stopwatch.Stop(); // Stop timing
            if (resp.IsSucceed)
            {
                LogId = await _LogService.LogInformation(resp.Message, User.Identity.Name);
                return Ok(resp);
            }
            LogId = await _LogService.LogError(resp.Message, User.Identity.Name);

            return StatusCode(resp.StatusCode, resp.Message);
        }
        catch (Exception ex)
        {
            resp.IsSucceed = false;
            resp.Message = "An error occurred while processing the request.";
            resp.Error = new List<string> { ex.Message };
            resp.StatusCode = StatusCodes.Status500InternalServerError;

            // Log Exception Details
            LogId = await _LogService.LogException(ex.Message, User.Identity.Name, ex);

            return StatusCode(500, resp);
        }
        finally
        {
            // Log the request and response details
            var logs = new OpenApiReqLogDto
            {
                CallType = _HttpContextAccessor.HttpContext.Request.GetDisplayUrl(),
                ReqData = JsonConvert.SerializeObject(_HttpContextAccessor.HttpContext.Request.BodyReader),
                ResData = JsonConvert.SerializeObject(resp),
                ReqTime = reqTime,
                StatusCode = resp.StatusCode,
                ResTime = DateTime.Now, // Capture execution time
                ExecutionTime = stopwatch.ElapsedMilliseconds, // Capture execution time
                ServiceUtilizer = "POS",
                UserName = User?.Identity?.Name ?? "Anonymous",
                TransTraceId = LogId,


            };
            await _LogService.SaveRequestLog(_HttpContextAccessor.HttpContext, logs);
        }
    }

    [HttpGet("get-all")]
    public async Task<IActionResult> GetAll()
    {
        ResponseDto resp = new ResponseDto();
       
        try
        {

            resp = await _Service.GetAllAsync();
            return resp.IsSucceed?Ok(resp): StatusCode(resp.StatusCode, resp.Message);
        }
        catch (Exception ex)
        {
            resp.IsSucceed = false;
            resp.Message = "An error occurred while processing the request.";
            resp.Error = new List<string> { ex.Message };
            resp.StatusCode = StatusCodes.Status500InternalServerError;

            // Log Exception Details
             await _LogService.LogException(ex.Message, User.Identity.Name, ex);

            return StatusCode(500, resp);
        }


    }
    [HttpGet("get/{EncryptId}")]
    public async Task<IActionResult> Get(string EncryptId)
    {
        ResponseDto resp = new ResponseDto();
       
        try
        {
            int Id = Convert.ToInt32(CryptoHelper.Decrypt(EncryptId));

            resp = await _Service.GetByIdAsync(Id);
            return resp.IsSucceed?Ok(resp): StatusCode(resp.StatusCode, resp.Message);
        }
        catch (Exception ex)
        {
            resp.IsSucceed = false;
            resp.Message = "An error occurred while processing the request.";
            resp.Error = new List<string> { ex.Message };
            resp.StatusCode = StatusCodes.Status500InternalServerError;

            // Log Exception Details
             await _LogService.LogException(ex.Message, User.Identity.Name, ex);

            return StatusCode(500, resp);
        }


    }
}
