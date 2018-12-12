using AutoMapper;
using HttpListener.BusinessLayer.Infrastructure.Models;
using HttpListener.DataLayer.Infrastructure.Models;

namespace HttpListener.BusinessLayer.MapperConfigurations
{
    /// <summary>
    /// Represents a <see cref="HttpListenerMappingProfile"/> class.
    /// </summary>
    public class HttpListenerMappingProfile : Profile
    {
        public HttpListenerMappingProfile()
        {
            CreateMap<OrderView, Order>()
                .ForMember(d => d.Customer, opt => opt.Ignore())
                .ForMember(d => d.Employee, opt => opt.Ignore())
                .ForMember(d => d.Order_Details, opt => opt.Ignore())
                .ForMember(d => d.Shipper, opt => opt.Ignore())
                .ReverseMap();
        }
    }
}
