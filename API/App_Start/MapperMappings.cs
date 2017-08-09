
using API.Models;
using API.ViewModels;
using AutoMapper;
using Models.Entities;

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
                cfg.CreateMap<Picture, PictureViewModelGet>();
                cfg.CreateMap<Picture, PictureViewModelShortGet>();
                cfg.CreateMap<TempFile, TempFileViewModelGet>();



                // ViewModels to Models
                cfg.CreateMap<UserViewModelPut, User>();
                cfg.CreateMap<PictureViewModelGet, Picture>();
                cfg.CreateMap<TempFileViewModelPost, TempFile>();
            });
        }
    }
}