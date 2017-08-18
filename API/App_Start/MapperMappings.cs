
using API.Models;
using API.ViewModels;
using AutoMapper;
using Models.Entities;
using Models.Operations;

namespace API
{
    /// <summary>
    /// Маппинг для Automapper
    /// </summary>
    public class MapperMappings
    {
        /// <summary>
        /// Главный метод
        /// </summary>
        public static void Map()
        {
            Mapper.Initialize(cfg => {
                
                // Model to ViewModels
                cfg.CreateMap<User, UserViewModelPut>();
                cfg.CreateMap<User, UserViewModelGet>();
                cfg.CreateMap<User, UserViewModelShortGet>();
                cfg.CreateMap<PageViewDTO<Order>, PageView<OrderShortViewModelGet>>();
                cfg.CreateMap<Order, OrderShortViewModelGet>();
                cfg.CreateMap<Order, OrderViewModelGet>();
                cfg.CreateMap<PageViewDTO<Comment>, PageView<CommentViewModelGet>>();
                cfg.CreateMap<Comment, CommentViewModelGet>();
                cfg.CreateMap<Payment, PaymentViewModelGet>();
                cfg.CreateMap<Payment, PaymentViewModelShortGet>();
                cfg.CreateMap<Payment, PaymentViewModelShortGet>();
                cfg.CreateMap<PageViewDTO<Payment>, PageView<PaymentViewModelShortGet>>();
                cfg.CreateMap<PageViewDTO<Payment>, PageView<PaymentViewModelGet>>();



                // ViewModels to Models
                cfg.CreateMap<UserViewModelPut, User>();
                cfg.CreateMap<OrderViewModelPost, Order>();
                cfg.CreateMap<CommentViewModelGet, Comment>();
                cfg.CreateMap<PageView<CommentViewModelGet>, PageViewDTO<Comment>>();
                cfg.CreateMap<PaymentViewModelPost, Payment>();
            });
        }
    }
}