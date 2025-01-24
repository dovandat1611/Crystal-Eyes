using Crystal_Eyes_Controller.Dtos.Authentication;

namespace Crystal_Eyes_Controller.IServices
{
	public interface IAuthenticationService
	{
		Task<(bool IsSuccess, string Message)> RegisterAsync(RegisterViewModel model);
		Task<(bool IsSuccess, string Message)> VerifyAsync(string user_id);
	}
}
