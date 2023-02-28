using System;
using System.ComponentModel.DataAnnotations;
using System.Security.Claims;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using dotnetserver.Models;
using dotnetserver.Services.JWT;
using Google.Apis.Auth;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;

namespace dotnetserver.Controllers
{
    [ApiController]
    [Authorize]
    [Route("api/account")]
    public class AccountController : ControllerBase
    {
        private readonly ILogger<AccountController> _logger;
        private readonly IUserService _userService;
        private readonly IJwtAuthManager _jwtAuthManager;
        private readonly IAWSService _awsService;
        private readonly string _baseAvatarUrl;
        private readonly GoogleJsonWebSignature.ValidationSettings _settings;


        public AccountController(ILogger<AccountController> logger, IUserService userService, IJwtAuthManager jwtAuthManager, IAWSService awsService)
        {
            _logger = logger;
            _userService = userService;
            _jwtAuthManager = jwtAuthManager;
            _awsService = awsService;
            _baseAvatarUrl = "https://storage.yandexcloud.net/tm-bucket/";
            _settings = new GoogleJsonWebSignature.ValidationSettings();

        }

        /// <summary>
        /// Authorize user by given credentials
        /// </summary>
        /// <remarks>
        /// That method generates new pair access/refresh tokens,
        /// Adds user to server claims,
        /// Returns all user data.
        /// </remarks>
        /// <returns>Returns all user data</returns>
        /// <response code="200">Success</response>
        /// <response code="400">Bad Request if model state is invalid</response>
        /// <response code="401">Unauthorized if got incorrect credentials</response>
        [AllowAnonymous]
        [ProducesResponseType(typeof(LoginResult), 200)]
        [HttpPost("login")]
        public async Task<ActionResult> Login([FromBody] LoginRequest request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            if (!await _userService.IsValidUserCredentials(request.username, request.password))
            {
                return Unauthorized();
            }

            var user = await _userService.GetUserData(request.username);
            var claims = new[]
            {
                new Claim(ClaimTypes.Name, user.username),
                new Claim(ClaimTypes.NameIdentifier, user.userId.ToString())
            };

            var jwtResult = _jwtAuthManager.GenerateTokens(request.username, claims, DateTime.Now);
            _logger.LogInformation($"User [{request.username}] logged in the system.");
            return Ok(new LoginResult
            {
                username = user.username,
                userId = user.userId,
                firstName = user.firstName,
                lastName = user.lastName,
                lastBoardId = user.lastBoardId,
                AccessToken = jwtResult.AccessToken,
                RefreshToken = jwtResult.RefreshToken.TokenString
            });
        }

        /// <summary>
        /// Authorize user by given credentials
        /// </summary>
        /// <remarks>
        /// That method generates new pair access/refresh tokens,
        /// Adds user to server claims,
        /// Returns JWT token
        /// </remarks>
        /// <returns>Returns all user data</returns>
        /// <response code="200">Success</response>
        /// <response code="400">Bad Request if model state is invalid</response>
        /// <response code="401">Unauthorized if got incorrect credentials</response>
        [AllowAnonymous]
        [ProducesResponseType(typeof(LoginResult), 200)]
        [HttpPost(nameof(OAuthLogin))]
        public async Task<ActionResult> OAuthLogin([FromBody] OAuthRequest request)
        {
            GoogleJsonWebSignature.Payload payload;
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }
            try
            {
                payload = GoogleJsonWebSignature.ValidateAsync(request.IdToken, _settings).Result;
            }
            catch
            {
                return Unauthorized();
            }
            if (!await _userService.IsAnExistingUser(payload.Email))
            {
                await _userService.RegisterUser(new SignUpUser
                {
                    username = payload.Email,
                    firstName = payload.Name,
                    lastName = payload.FamilyName,
                    avatar = null,
                    password = RandomAlphaNumericString.Generate(10)
                });
            }

            var user = await _userService.GetUserData(payload.Email);
            var claims = new[]
            {
                new Claim(ClaimTypes.Name, user.username),
                new Claim(ClaimTypes.NameIdentifier, user.userId.ToString())
            };

            var jwtResult = _jwtAuthManager.GenerateTokens(payload.Email, claims, DateTime.Now);
            _logger.LogInformation($"User [{payload.Email}] logged in the system.");
            return Ok(new LoginResult
            {
                username = user.username,
                userId = user.userId,
                firstName = user.firstName,
                lastName = user.lastName,
                lastBoardId = user.lastBoardId,
                AccessToken = jwtResult.AccessToken,
                RefreshToken = jwtResult.RefreshToken.TokenString
            });
        }

        /// <summary>
        /// Register user by given credentials
        /// </summary>
        /// <remarks>
        /// That method add new user data to database,
        /// Generates new pair access/refresh tokens,
        /// Adds user to server claims,
        /// Returns all user data.
        /// </remarks>
        /// <returns>Returns all user data</returns>
        /// <response code="200">Success</response>
        /// <response code="400">Bad Request if model state is invalid</response>
        /// <response code="401">Unauthorized if got already registered email</response>
        [AllowAnonymous]
        [ProducesResponseType(typeof(LoginResult), 200)]
        [HttpPost("register")]
        public async Task<ActionResult> Register([FromBody] SignUpUser request)
        {
            if (!ModelState.IsValid)
                return BadRequest();
            if (await _userService.IsAnExistingUser(request.username))
                return Unauthorized();


            var userId = await _userService.RegisterUser(request);

            if (userId == 0)
                return Unauthorized();

            var claims = new[]
            {
                new Claim(ClaimTypes.Name,request.username),
                new Claim(ClaimTypes.NameIdentifier, userId.ToString())
            };

            var jwtResult = _jwtAuthManager.GenerateTokens(request.username, claims, DateTime.Now);
            _logger.LogInformation($"User [{request.username}] logged in the system.");
            return Ok(new LoginResult
            {
                username = request.username,
                userId = userId,
                firstName = request.firstName,
                lastName = request.lastName,
                lastBoardId = 0,
                avatar = request.avatar,
                AccessToken = jwtResult.AccessToken,
                RefreshToken = jwtResult.RefreshToken.TokenString
            });
        }

        /// <summary>
        /// Get user ID from server claims and returns it
        /// </summary>
        /// <remarks>
        /// Use that method to get current session user ID if its needed
        /// </remarks>
        /// <returns>User ID</returns>
        /// <response code="200">Success</response>
        /// <response code="401">Unauthorized if get request from unauthorized client</response>
        [ProducesResponseType(typeof(UserData), 200)]
        [HttpGet("user")]
        [Authorize]
        public async Task<ActionResult> GetCurrentUser()
        {
            var userName = User.Identity?.Name;
            var user = await _userService.GetUserData(userName);

            return Ok(new UserData
            {
                username = user.username,
                userId = user.userId,
                firstName = user.firstName,
                lastName = user.lastName,
                lastBoardId = user.lastBoardId,
                avatar = user.avatar
            });
        }

        /// <summary>
        /// Remove user from server claims and delete his access and refresh tokens.
        /// </summary>
        /// <returns>Nothing</returns>
        /// <response code="200">Success</response>
        /// <response code="401">Unauthorized if get request from unauthorized client</response>
        [HttpPost("logout")]
        [Authorize]
        public ActionResult Logout()
        {
            // optionally "revoke" JWT token on the server side --> add the current token to a block-list
            // https://github.com/auth0/node-jsonwebtoken/issues/375

            var userName = User.Identity?.Name;
            _jwtAuthManager.RemoveRefreshTokenByUserName(userName);
            _logger.LogInformation($"User [{userName}] logged out the system.");
            return Ok();
        }

        /// <summary>
        /// reAuthorize user by given refresh token
        /// </summary>
        /// <remarks>
        /// That method generates new pair access/refresh tokens,
        /// Returns all user data.
        /// If got invalid token, method returns Unauthorized status code
        /// </remarks>
        /// <returns>Returns all user data</returns>
        /// <response code="200">Success</response>
        /// <response code="401">Unauthorized if got broken refresh token</response>
        [ProducesResponseType(typeof(RefreshTokenResponse), 200)]
        [HttpPost("refresh-token")]
        [Authorize]
        public async Task<ActionResult> RefreshToken([FromBody] RefreshTokenRequest request)
        {
            try
            {
                var userName = User.Identity?.Name;
                _logger.LogInformation($"User [{userName}] is trying to refresh JWT token.");

                if (string.IsNullOrWhiteSpace(request.RefreshToken))
                {
                    return Unauthorized();
                }

                var user = await _userService.GetUserData(userName);

                var accessToken = await HttpContext.GetTokenAsync("Bearer", "access_token");
                var jwtResult = _jwtAuthManager.Refresh(request.RefreshToken, accessToken, DateTime.Now);
                _logger.LogInformation($"User [{userName}] has refreshed JWT token.");
                return Ok(new RefreshTokenResponse
                {
                    AccessToken = jwtResult.AccessToken,
                    RefreshToken = jwtResult.RefreshToken.TokenString
                });
            }
            catch (SecurityTokenException e)
            {
                return Unauthorized(e.Message); // return 401 so that the client side can redirect the user to login page
            }
        }

        /// <summary>
        /// Returns Id of last board that was used by user.
        /// </summary>
        /// <returns>user.lastBoardId</returns>
        /// <response code="401">If user unauthorized</response>
        /// <response code="200">Success</response>
        [HttpGet("last-board")]
        public async Task<IActionResult> GetLastBoard()
        {
            var userName = User.Identity?.Name;
            var user = await _userService.GetUserData(userName);
            return Ok(user.lastBoardId);
        }

        /// <summary>
        /// Sets Id of last board
        /// </summary>
        /// <returns>nothings</returns>
        /// <param name="boardId" type="string" description="The ID of the board that need to be deleted"> </param>
        /// <response code="401">If user unauthorized</response>
        /// <response code="200">Success</response>
        [HttpPatch("last-board/{boardId}")]
        public async Task<IActionResult> SetLastBoard(string boardId)
        {
            await _userService.SetLastBoardId(boardId);
            return Ok();
        }

        [HttpPost("upload-avatar/")]
        public async Task<IActionResult> UploadAvatart(IFormFile avatart)
        {
            var userName = User.Identity?.Name;
            await _awsService.Upload(avatart, userName);
            await _userService.SetAvatart(userName, _baseAvatarUrl + userName + "-avatar");
            return Ok();
        }
    }
}
