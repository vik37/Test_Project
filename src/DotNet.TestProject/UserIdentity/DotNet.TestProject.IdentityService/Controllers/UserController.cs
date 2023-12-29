using DotNet.TestProject.IdentityService.Infrastructure.Filters;

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
        private readonly IMediator _mediator;
        private readonly IUserQuery _userQuery;

        #pragma warning disable
        public UserController(IMediator mediator,
                                IUserQuery userQuery)
        {
            _mediator = mediator ?? throw new ArgumentNullException();
            _userQuery = userQuery ?? throw new ArgumentNullException();
        }

#pragma warning restore
        /// <summary>
        ///   This API Calls HTTP Get Method 
        ///   Its Authorized - User must be Logged.  
        /// </summary>
        /// <returns>Single User Model by ID</returns>
        [Authorize]
        [HttpGet("{userId}")]
        [CheckUserIdIArgumentAsyncFilter]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IActionResult> GetUser([FromRoute] string userId)
        {
            var user = await _userQuery.GetUser(userId);
                
            return user is not null ? Ok(user): throw new IdentityUserException("User does not exist");
        }

        /// <summary>
        /// Post Method Intended For Registration New User
        /// </summary>
        /// <param name="command"></param>
        /// <returns>Ok, BadRequest, InternalServerError (200,400,500)</returns>
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [Route("register")]
        public async Task<IActionResult> Register([FromBody] RegisterCommand command)
        {
            var result = await _mediator.Send(command, default);

            if (!result)
                return BadRequest();

            return Ok();
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
                var response = await _mediator.Send(command,default);

                return Ok(response);
        }

        /// <summary>
        /// Update User
        /// Only Admins can use
        /// </summary>
        /// <param name="command"></param>
        /// <returns>Ok, BadRequest, InternalServerError (200,404,500)</returns>
        [Authorize]
        [HttpPut]
        [Route("update")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> UpdateUser([FromBody] UpdateUserCommand command)
        {
           var response = await _mediator.Send(command, default);
           return Ok(response);
        }

        /// <summary>
        /// Remove User by ID
        /// Only Admins can use
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        [Authorize(Roles = "admin")]
        [HttpDelete]
        [Route("delete/{userId}")]
        [CheckUserIdIArgumentAsyncFilter]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> RemoveUser([FromRoute] string userId)
        {
            var removeUserCommand = new RemoveUserCommand() { Id = userId};
           
            var result = await _mediator.Send(removeUserCommand);

            if (!result)
                throw new ArgumentException("User was not Found");

            return NoContent();
        }
    }
}
