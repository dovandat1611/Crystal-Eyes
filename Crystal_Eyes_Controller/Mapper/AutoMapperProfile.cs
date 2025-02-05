using AutoMapper;
using Crystal_Eyes_Controller.Common;
using Crystal_Eyes_Controller.Dtos;
using Crystal_Eyes_Controller.Dtos.Cart;
using Crystal_Eyes_Controller.Dtos.Feedback;
using Crystal_Eyes_Controller.Dtos.OrderDetail;
using Crystal_Eyes_Controller.Dtos.Product;
using Crystal_Eyes_Controller.Models;

namespace Crystal_Eyes_Controller.Mapper
{
	public class AutoMapperProfile : Profile
	{
		public AutoMapperProfile()
		{
			CreateMap<User, UserLoginDto>()
				.ForMember(dest => dest.UserId, opt => opt.MapFrom(src => src.UserId))
				.ForMember(dest => dest.RoleName, opt => opt.MapFrom(src => src.RoleName))
				.ForMember(dest => dest.Email, opt => opt.MapFrom(src => src.Email))
				.ForMember(dest => dest.Name, opt => opt.MapFrom(src =>
					src.Customer != null ? src.Customer.Name :
					src.Admin != null ? src.Admin.Name : null))
				.ForMember(dest => dest.Phone, opt => opt.MapFrom(src =>
					src.Admin != null ? src.Admin.Phone :
					src.Customer != null ? src.Customer.Phone : null))
				.ForMember(dest => dest.Image, opt => opt.MapFrom(src =>
					src.Admin != null ? src.Admin.Image :
					src.Customer != null ? src.Customer.Image : null))
				.ForMember(dest => dest.Dob, opt => opt.MapFrom(src =>
					src.Admin != null ? src.Admin.Dob :
					src.Customer != null && src.Customer.Dob != null ? src.Customer.Dob : null))
				.ForMember(dest => dest.Address, opt => opt.MapFrom(src =>
					src.Customer != null && src.Customer.Address != null ? src.Customer.Address : null));


			CreateMap<Product, ProductViewDto>().ReverseMap();

			CreateMap<Product, ProductDetailViewDto>()
				.ForMember(dest => dest.CategoryName, opt => opt.MapFrom(src =>
					src.Category != null ? src.Category.Name : null));


			CreateMap<Feedback, FeedbackViewDto>()
				.ForMember(dest => dest.Image, opt => opt.MapFrom(src =>
					src.User != null && src.User.Customer != null ? src.User.Customer.Image : Constants.Default_Avatar.CUSTOMER))
				.ForMember(dest => dest.Name, opt => opt.MapFrom(src =>
					src.User != null && src.User.Customer != null ? src.User.Customer.Name : "Guest"));


			CreateMap<OrderDetail, OrderDetailViewDto>()
				.ForMember(dest => dest.MainImage, opt => opt.MapFrom(src =>
					src.Product != null ? src.Product.MainImage : null))
				.ForMember(dest => dest.Name, opt => opt.MapFrom(src =>
					src.Product != null ? src.Product.Name : null));

		}
	}
}
