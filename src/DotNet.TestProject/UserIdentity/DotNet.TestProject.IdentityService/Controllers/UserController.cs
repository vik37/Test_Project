namespace DotNet.TestProject.IdentityService.Controllers
{
    /// <summary>
    /// User API Controller
    /// </summary>
    [Route("api/{version:apiVersion}/[controller]")]
    [ApiVersion("1.0")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly ILogger<UserController> _logger;
        private readonly IMediator _mediator;
        private readonly IUserQuery _userQuery;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="logger"></param>
        /// <param name="mediator"></param>
        /// <param name="userQuery"></param>
        public UserController(ILogger<UserController> logger, IMediator mediator,
                                IUserQuery userQuery)
        {
            _logger = logger;
            _mediator = mediator;
            _userQuery = userQuery;
        }

        /// <summary>
        ///   This API Calls HTTP Get Method 
        ///   Its Authorized - User must be Logged.  
        /// </summary>
        /// <returns>Single User Model by ID</returns>
        [Authorize]
        [HttpGet("{userId}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IActionResult> GetUser([FromRoute] string userId)
        {
            if (string.IsNullOrEmpty(userId))
                return BadRequest();

            try
            {
                var user = await _userQuery.GetUser(userId);
                if (user is null)
                    return NotFound();

                _logger.LogInformation("Successfully fetcing the User");
                return Ok(user);
            }
            catch (Exception ex)
            {
                _logger.LogError("Sommething bad happend: Exception - {Exception}, {Message}", ex, ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }

        /// <summary>
        /// Post Method Intended For Registration New User
        /// </summary>
        /// <param name="command"></param>
        /// <returns>Ok, BadRequest, InternalServerError (200,400,500)</returns>
        /// <exception cref="IdentityUserException"></exception>
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [Route("register")]
        public async Task<IActionResult> Register([FromBody] RegisterCommand command)
        {
            try
            {
                var result =  await _mediator.Send(command,default);

                return CreatedAtAction(nameof(Login),result);
            }
            catch(IdentityUserException ex)
            {
                _logger.LogWarning("Wrong Registration - {Controller}, {Message}, ({Exception})",typeof(UserController),ex.Message,ex);

                if(ex.InnerException is ValidationException validationException)
                {
                    return BadRequest(validationException.Errors);
                }
                return BadRequest(ex.Message);
            }
            catch (Exception ex)
            {
                _logger.LogError("Sommething bad happend: Exception - {Exception}, {Message}",ex,ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }

        /// <summary>
        /// Post Method Intended For User Login
        /// </summary>
        /// <param name="command"></param>
        /// <returns>Ok, NotFound, InternalServerError (200,404,500)</returns>
        [HttpPost]
        [Route("login")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> Login([FromBody] LoginCommand command)
        {
            try
            {
                var response = await _mediator.Send(command,default);
                return Ok(response);
            }
            catch(IdentityUserException ex)
            {
                _logger.LogWarning("Wrong Try For Login - {Controller}, {Message}, ({Exception})", typeof(UserController), ex.Message, ex);
                return NotFound(ex.Message);
            }
            catch (Exception ex)
            {
                _logger.LogError("Sommething bad happend: Exception - {Exception}, {Message}", ex, ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }

        /// <summary>
        /// Update User
        /// Only Admins can use
        /// </summary>
        /// <param name="command"></param>
        /// <returns>Ok, BadRequest, InternalServerError (200,404,500)</returns>
        [Authorize(Roles = "admin")]
        [HttpPut]
        [Route("update")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> UpdateUser([FromBody] UpdateUserCommand command)
        {
            try
            {
                var response = await _mediator.Send(command, default);
                return Ok(response);
            }
            catch (IdentityUserException ex)
            {
                _logger.LogWarning("Wrong Try For Login - {Controller}, {Message}, ({Exception})", typeof(UserController), ex.Message, ex);
                return NotFound(ex.Message);
            }
            catch (Exception ex)
            {
                _logger.LogError("Sommething bad happend: Exception - {Exception}, {Message}", ex, ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }

        /// <summary>
        /// Remove User by ID
        /// Only Admins can use
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [Authorize(Roles = "admin")]
        [HttpDelete]
        [Route("delete/{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> RemoveUser(string id)
        {
            try
            {
                var removeUserCommand = new RemoveUserCommand();
                removeUserCommand.Id = id;  
                var result = await _mediator.Send(removeUserCommand);

                if(!result)
                    return NotFound();

                return NoContent();

            }
            catch (Exception)
            {

                throw;
            }
        }
    }
}

