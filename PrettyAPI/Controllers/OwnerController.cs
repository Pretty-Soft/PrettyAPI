using AutoMapper;
using Contracts;
using Entities.DataTransferObjects;
using Entities.Helper;
using Entities.Models;
using Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using static System.Runtime.InteropServices.JavaScript.JSType;
using System.Linq.Expressions;
using Microsoft.AspNetCore.Authorization;

namespace PrettyAPI.Controllers
{
    [Route("api/owner")]
    [ApiController]
    public class OwnerController : ControllerBase
    {
        private ILoggerManager _logger;
        private IRepositoryWrapper _repository;
        private IMapper _mapper;
        public OwnerController(ILoggerManager logger, IRepositoryWrapper repository, IMapper mapper)
        {
            _logger = logger;
            _repository = repository;
            _mapper = mapper;
        }

        [HttpGet, Authorize]
        [Route("get-owners")]
        public async Task<IActionResult> Get([FromQuery] Pagination pagination)    
        {
            try
            {
                var returnData = new ApiResponseSuccess<IEnumerable<OwnerDto>>();
                var owners =  _repository.Owner.FindAll(pagination);
                
                _logger.LogInfo($"Returned all owners from database.");

                returnData.data =_mapper.Map<IEnumerable<OwnerDto>>(owners);
                returnData.statusCode = StatusCodes.Status200OK;
                returnData.status = "success";
                returnData.message = "";
                HttpContext.Response.Headers.Append("X-Total-Count","0");
               
                return StatusCode(StatusCodes.Status200OK, returnData);
                
            }
            catch (Exception ex)
            {
                _logger.LogError($"Something went wrong inside GetAllOwners action: {ex.Message}");
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpGet]
        [Route("get-by-id/{id}")]
        public async Task<IActionResult> Get(Guid id)
        {
            try
            {
                
                var returnData = new ApiResponseSuccess<OwnerDto>();
                var owner =await _repository.Owner.GetOwnerByIdAsync(id);

                if (owner is null)
                {
                    _logger.LogError($"Owner with id: {id}, hasn't been found in db.");
                    return StatusCode(StatusCodes.Status404NotFound, null);
                }
                else
                {
                    _logger.LogInfo($"Returned owner with id: {id}");

                    returnData.data = _mapper.Map<OwnerDto>(owner);
                    returnData.statusCode = StatusCodes.Status200OK;
                    returnData.status = "success";
                    returnData.message = "";

                    return StatusCode(StatusCodes.Status200OK, returnData);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError($"Something went wrong inside GetOwnerById action: {ex.Message}");
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpGet("owner-with-account/{id}")]
        public async Task<IActionResult> GetDetails(Guid id)    
        {
              
                var returnData = new ApiResponseSuccess<OwnerDto>();
                var owner =await _repository.Owner.GetOwnerWithDetailsAsync(id);
                if (owner == null)
                {
                    _logger.LogError($"Owner with id: {id}, hasn't been found in db.");
                    return StatusCode(StatusCodes.Status404NotFound, returnData);
                }
                else
                {
                    _logger.LogInfo($"Returned owner with details for id: {id}");

                    returnData.data = _mapper.Map<OwnerDto>(owner);
                    returnData.statusCode = StatusCodes.Status200OK;
                    returnData.status = "success";
                    returnData.message = "";

                    return StatusCode(StatusCodes.Status200OK, returnData);
                }
           
        }

        [HttpPost("create-owner")]
        public async Task<IActionResult> Post([FromForm] OwnerForCreationDto owner)
        {
            
                var returnData = new ApiResponseSuccess<OwnerDto>();
                if (owner == null)
                {
                    _logger.LogError("Owner object sent from client is null.");
                    return BadRequest("Owner object is null");
                }

                if (!ModelState.IsValid)
                {
                    _logger.LogError("Invalid owner object sent from client.");
                    return BadRequest("Invalid model object");
                }

                var ownerEntity = _mapper.Map<Owner>(owner);

                _repository.Owner.CreateOwner(ownerEntity);
                await _repository.SaveAsync();


                returnData.data = _mapper.Map<OwnerDto>(ownerEntity);
                returnData.statusCode = StatusCodes.Status200OK;
                returnData.status = "success";
                returnData.message = "";
           

            return StatusCode(StatusCodes.Status201Created, returnData);
            
        }

        [HttpPut("update-owner/{id}")]
        public async Task<IActionResult> Put(Guid id, [FromForm] OwnerForUpdateDto owner)
        {
            try
            {
                var returnData = new ApiResponseSuccess<OwnerForUpdateDto>();
                if (owner == null)
                {
                    _logger.LogError("Owner object sent from client is null.");
                    return BadRequest("Owner object is null");
                }

                if (!ModelState.IsValid)
                {
                    _logger.LogError("Invalid owner object sent from client.");
                    return BadRequest("Invalid model object");
                }

                var ownerEntity = await _repository.Owner.GetOwnerByIdAsync(id);
                if (ownerEntity == null)
                {
                    _logger.LogError($"Owner with id: {id}, hasn't been found in db.");
                    return NotFound();
                }

                _mapper.Map(owner, ownerEntity);

                _repository.Owner.UpdateOwner(ownerEntity);
                await _repository.SaveAsync();

                returnData.data = null;
                returnData.statusCode = StatusCodes.Status201Created;
                returnData.status = "success";
                returnData.message = "";

                return StatusCode(StatusCodes.Status201Created, returnData);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Something went wrong inside UpdateOwner action: {ex.Message}");
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpDelete("delete-owner/{id}")]
        public async Task<IActionResult> Delete(Guid id)    
        {
            try
            {
                var returnData = new ApiResponseSuccess<OwnerDto>();
                var owner = await _repository.Owner.GetOwnerByIdAsync(id);
                if (owner == null)
                {
                    _logger.LogError($"Owner with id: {id}, hasn't been found in db.");
                    return NotFound();
                }

                if (_repository.Account.AccountsByOwnerAsync(id).Result.Any())
                {
                    _logger.LogError($"Cannot delete owner with id: {id}. It has related accounts. Delete those accounts first");
                    return BadRequest("Cannot delete owner. It has related accounts. Delete those accounts first");
                }

                _repository.Owner.DeleteOwner(owner);
                await _repository.SaveAsync();

                returnData.data = null;
                returnData.statusCode = StatusCodes.Status204NoContent;
                returnData.status = "success";
                returnData.message = "deleted";

                return StatusCode(StatusCodes.Status200OK, returnData);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Something went wrong inside DeleteOwner action: {ex.Message}");
                return StatusCode(500, "Internal server error");
            }
        }
    }
}
