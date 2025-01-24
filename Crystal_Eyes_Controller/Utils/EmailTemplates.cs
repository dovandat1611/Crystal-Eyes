namespace Crystal_Eyes_Controller.Utils
{
	public class EmailTemplates
	{
		public static string Verify(string name, string encryptionId)
		{

			return $@"
            <!DOCTYPE html>
            <html>
            <head>
                <meta name=""viewport"" content=""width=device-width, initial-scale=1"">
                <title>Xác thực tài khoản</title>
            </head>
            <body style=""font-family: Arial, sans-serif; background-color: #f4f4f4; margin: 0; padding: 0;"">
                <div style=""margin: 20px auto; max-width: 600px; width: 100%; background-color: #ffffff; box-shadow: 0 5px 15px rgba(0, 0, 0, 0.3); overflow: hidden; border-radius: 20px;"">
                    <div style=""background-color: #000000; padding: 20px; text-align: center; color: #ffffff; font-size: 1.5rem; font-weight: bold;"">
                        Xác thực tài khoản | <span style=""color: #ffffff;"">Crystal Eyes</span>
                    </div>
                    <div style=""padding: 20px; text-align: center; color: #000000;"">
                        <img src=""https://res.cloudinary.com/do9iyczi3/image/upload/v1737732350/CrystalEyes_mny0hl.png"" alt=""Logo Crystal Eyes"" width=""200"" style=""display: block; margin: 0 auto; margin-bottom: 10px; border-radius: 10px; box-shadow: 0 5px 15px rgba(0, 0, 0, 0.1);"" />
                        <div style=""background-color: #f4f4f4; padding: 20px; border-radius: 5px; box-shadow: 0 2px 8px rgba(8, 120, 211, 0.1); text-align: left;"">
                            <p>Xin chào, <span style=""font-weight: bold; color: #000000;"">{name}</span></p>
                            <p>Bạn cần xác thực địa chỉ email này để đăng nhập trên <span style=""font-weight: bold; color: #000000;"">Crystal Eyes</span>.</p>
                            <p>Vui lòng click vào nút ""Xác thực"" bên dưới.</p>
                            <br><br>
                            <div style=""text-align: center;"">
                                <a href=""https://localhost:7032/verify-account?code={encryptionId}"" style=""display: inline-block; background-color: #000000; color: #ffffff; padding: 10px 20px; border-radius: 5px; text-decoration: none; font-weight: bold;"">
                                    Xác Thực
                                </a>
                            </div>
                            <br><br>
                        </div>
                    </div>
                    <div style=""background-color: #000000; padding: 10px; text-align: center; color: #ffffff; font-size: 0.875rem; font-weight: bold;"">
                        © <script>document.write(new Date().getFullYear());</script> | Bản quyền thuộc về Crystal Eyes.
                    </div>
                </div>
            </body>
            </html>
			";

		}
	}
}
