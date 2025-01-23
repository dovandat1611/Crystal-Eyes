using Crystal_Eyes_Controller.Dtos.Email;

namespace Crystal_Eyes_Controller.IServices
{
	public interface IMailSystemService
	{
		Task SendMail(MailContent mailContent);

		Task SendEmailAsync(string email, string subject, string htmlMessage);
	}
}
