using System;
using System.CodeDom;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using API.Models;

namespace API.ViewModels
{
    /// <summary>
    /// Пользователь
    /// </summary>
    public class UserViewModelGet
    {

        public int Id { get; set; }
        /// <summary>
        /// Электропочта
        /// </summary>
        public string Email { get; set; }

        /// <summary>
        /// Человеческое имя
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Номер телефона
        /// </summary>
        public string Phone { get; set; }

        /// <summary>
        /// Аватарка
        /// </summary>
        public virtual string AvatarUrl{ get; set; }

        /// <summary>
        /// Превью аватарки
        /// </summary>
        public virtual string AvatarUrlThumb { get; set; }

        /// <summary>
        /// Название роли
        /// </summary>
        public string RoleName { get; set; }

    }

    public class UserViewModelShortGet
    {
        public int Id { get; set; }
        /// <summary>
        /// Электропочта
        /// </summary>
        public string Email { get; set; }

        /// <summary>
        /// Человеческое имя
        /// </summary>
        public string Name { get; set; }
    }

    /// <summary>
    /// Пользователь
    /// </summary>
    public class UserViewModelPut
    {
        /// <summary>
        /// Человеческое имя
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Номер телефона
        /// </summary>
        public string Phone { get; set; }

        /// <summary>
        /// Роль
        /// </summary>
        public Role Role { get; set; }

        /// <summary>
        /// ID формы. По этому идентификатору связываются временные файлы
        /// с активностями. После сохранения активности, все TempFile с указанным FormId
        /// превратятся в Picture и привяжутся в данной активности.
        /// </summary>
        public string FormId { get; set; }

        [Required]
        public string Email { get; set; }
    }

    /// <summary>
    /// Регистрация или аутентификация
    /// </summary>
    public class UserRegisterViewModel
    {
        /// <summary>
        /// Почта пользователя
        /// </summary>
        [Required(AllowEmptyStrings = false)]
        public string Email { get; set; }
    }
}