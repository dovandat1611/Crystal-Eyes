using Crystal_Eyes_Controller.Dtos.Cart;
using Crystal_Eyes_Controller.Models;
using static System.Net.WebRequestMethods;

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

		public static string OTP(string otp)
		{
			return $@"
                <!DOCTYPE html>
                <html>
                <head>
                    <meta name=""viewport"" content=""width=device-width, initial-scale=1"">
                    <title>Xác thực tài khoản</title>
                </head>
                <body style=""font-family: Arial, sans-serif;"">
                    <div style=""margin: 20px auto; max-width: 600px; width: 100%; background-color: #ffffff; box-shadow: 0 5px 15px rgba(0, 0, 0, 0.3); overflow: hidden; border-radius: 20px;"">
                        <div style=""background-color: #000000; padding: 20px; text-align: center; color: #ffffff; font-size: 1.5rem; font-weight: bold;"">
                            Mã OTP - Quên Mật Khẩu | <span style=""color: #ffffff;"">Crystal Eyes</span>
                        </div>
                        <div style=""padding: 20px; text-align: center; color: #666666;"">
                            <div style=""background-color: #f4f4f4; padding: 20px; border-radius: 5px; box-shadow: 0 2px 8px rgba(8, 120, 211, 0.1); text-align: left;"">
                                <br><br>
                                <div style=""text-align: center;"">
                                    <span style=""display: inline-block; font-size: 24px;background-color: rgb(25, 23, 23); color: #ffffff; padding: 10px 20px; border-radius: 5px; text-decoration: none; font-weight: bold;"">
                                        {otp}
                                    </span>
                                </div>
                                <br><br>
                                <p style=""text-align: center;"">Lưu ý: không được chia sẻ mã này cho bất cứ ai!</p>
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

		public static string Feedback()
		{
			return $@"
            <!DOCTYPE html>
            <html>
            <head>
                <meta name=""viewport"" content=""width=device-width, initial-scale=1"">
                <title>Cảm ơn vì đã đánh giá sản phẩm</title>
            </head>
            <body style=""font-family: Arial, sans-serif;"">
                <div style=""margin: 20px auto; max-width: 600px; width: 100%; background-color: #ffffff; box-shadow: 0 5px 15px rgba(0, 0, 0, 0.3); overflow: hidden; border-radius: 20px;"">
                    <div style=""background-color: #000000; padding: 20px; text-align: center; color: #ffffff; font-size: 1.5rem; font-weight: bold;"">
                        Cảm ơn bạn đã đánh giá sản phẩm 🌟
                    </div>
                    <div style=""padding: 20px; text-align: center; color: #666666;"">
                        <div style=""background-color: #f4f4f4; padding: 20px; border-radius: 5px; box-shadow: 0 2px 8px rgba(8, 120, 211, 0.1); text-align: left;"">
                            <p style=""text-align: center;"">Cảm ơn bạn rất nhiều vì đã dành thời gian đánh giá sản phẩm của chúng tôi! 🙏. Chúng tôi rất trân trọng sự đóng góp của bạn và hy vọng sẽ tiếp tục phục vụ bạn trong tương lai. 💖</p>
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

		public static string ADD_ORDER(Order order, List<CartViewDto> orderDetails)
		{
			// Nếu không có yêu cầu, gán giá trị "Không có"
			if (string.IsNullOrEmpty(order.ContentReservation))
			{
				order.ContentReservation = "Không có";
			}

			string orderDetailsHtml = "";

			// Tạo HTML cho chi tiết đơn hàng
			foreach (var detail in orderDetails)
			{
				string formattedPrice = string.Format("{0:N0} VND", detail.Price);
				string formattedTotal = string.Format("{0:N0} VND", detail.TotalPrice);

				orderDetailsHtml += $@"
            <tr>
                <td style=""border: 1px solid #ddd; padding: 12px; text-align: center;"">
                    <img src=""{detail.MainImage}"" alt=""{detail.Name}"" style=""width: 50px; height: 50px; border-radius: 10px;"">
                </td>
                <td style=""border: 1px solid #ddd; padding: 12px; text-align: center;"">{detail.Name}</td>
                <td style=""border: 1px solid #ddd; padding: 12px; text-align: center;"">{detail.ColorName}</td>
                <td style=""border: 1px solid #ddd; padding: 12px; text-align: center;"">{detail.Quantity}</td>
                <td style=""border: 1px solid #ddd; padding: 12px; text-align: center;"">{formattedPrice}</td>
                <td style=""border: 1px solid #ddd; padding: 12px; text-align: center;"">{formattedTotal}</td>
            </tr>";
			}

			return $@"
<!DOCTYPE html>
<html>
<head>
    <meta name=""viewport"" content=""width=device-width, initial-scale=1"">
    <title>Xác thực tài khoản</title>
</head>
<body style=""font-family: Arial, sans-serif; background-color: #f4f4f4; margin: 0; padding: 0;"">
    <div style=""margin: 20px auto; max-width: 600px; width: 100%; background-color: #ffffff; box-shadow: 0 5px 15px rgba(0, 0, 0, 0.3); overflow: hidden; border-radius: 20px;"">
        <!-- Header -->
        <div style=""background-color: #000000; padding: 20px; text-align: center; color: #ffffff; font-size: 1.5rem; font-weight: bold;"">
            Thông tin đơn hàng
        </div>
        
        <!-- Content -->
        <div style=""padding: 20px; text-align: center; color: #000000;"">
            <div style=""background-color: #ffffff; padding: 20px; border-radius: 5px; box-shadow: 0 2px 8px rgba(8, 120, 211, 0.1); text-align: left;"">
                
                <!-- Order Info -->
                <div class=""order-info"">
                    <p style=""margin: 10px 0; font-size: 16px; font-weight: bold;"">Họ tên người nhận: <span style=""font-weight: normal;"">{order.NameReceiver}</span></p>
                    <p style=""margin: 10px 0; font-size: 16px; font-weight: bold;"">Số điện thoại: <span style=""font-weight: normal;"">{order.PhoneReceiver}</span></p>
                    <p style=""margin: 10px 0; font-size: 16px; font-weight: bold;"">Địa chỉ nhận hàng: <span style=""font-weight: normal;"">{order.AddressReceiver}</span></p>
                    <p style=""margin: 10px 0; font-size: 16px; font-weight: bold;"">Yêu cầu: <span style=""font-weight: normal;"">{order.ContentReservation}</span></p>
                    <p style=""margin: 10px 0; font-size: 16px; font-weight: bold;"">Tổng số tiền: <span style=""font-weight: normal;"">{string.Format("{0:N0} VND", order.TotalAmount)}</span></p>
                </div>

                <!-- Order Detail Table -->
                <table style=""width: 100%; border-collapse: collapse; margin-top: 20px;"">
                    <thead>
                        <tr>
                            <th style=""border: 1px solid #ddd; padding: 12px; text-align: center; background-color: #f1f1f1;"">Ảnh</th>
                            <th style=""border: 1px solid #ddd; padding: 12px; text-align: center; background-color: #f1f1f1;"">Tên sản phẩm</th>
                            <th style=""border: 1px solid #ddd; padding: 12px; text-align: center; background-color: #f1f1f1;"">Màu sắc</th>
                            <th style=""border: 1px solid #ddd; padding: 12px; text-align: center; background-color: #f1f1f1;"">Số lượng</th>
                            <th style=""border: 1px solid #ddd; padding: 12px; text-align: center; background-color: #f1f1f1;"">Giá</th>
                            <th style=""border: 1px solid #ddd; padding: 12px; text-align: center; background-color: #f1f1f1;"">Tổng tiền</th>
                        </tr>
                    </thead>
                    <tbody>
                        {orderDetailsHtml}
                    </tbody>
                </table>
            </div>
        </div>

        <!-- Footer -->
        <div style=""background-color: #000000; padding: 10px; text-align: center; color: #ffffff; font-size: 0.875rem; font-weight: bold;"">
            © <script>document.write(new Date().getFullYear());</script> | Bản quyền thuộc về Crystal Eyes.
        </div>
    </div>
</body>
</html>";
		}



	}
}
