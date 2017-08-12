
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
                cfg.CreateMap<PageViewDTO<Order>, PageView<OrderShortViewModelGet>>();
                cfg.CreateMap<Order, OrderShortViewModelGet>();
                cfg.CreateMap<Order, OrderViewModelGet>();


                // ViewModels to Models
                cfg.CreateMap<UserViewModelPut, User>();
                cfg.CreateMap<OrderViewModelPost, Order>();
            });
        }
    }
}