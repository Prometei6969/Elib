using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

namespace ELib.Models
{
    [Authorize]
    public class PublicationViewModel
    {
        public PublicationViewModel()
        {
            Files = new List<HttpPostedFileBase>();
        }

        public List<HttpPostedFileBase> Files { get; set; }


        // ID публикации
        [HiddenInput(DisplayValue = false)]
        public int Id { get; set; }
        // название публикации
        [StringLength(60, MinimumLength = 3)]
        [Required(ErrorMessage = "Не указано название публикации")]
        [Display(Name = "Название")]
        public string Name { get; set; }
        // автор 
        // цена
        [Required(ErrorMessage = "Не указан год публикации")]
        [RegularExpression(@"[0-9]\d{3}", ErrorMessage = "Год должен быть задан как ХХХХ")]
        [Display(Name = "Год")]
        [Range(2019, 2100)]
        public int Year { get; set; }

        [Required(ErrorMessage = "Не указан автор публикации")]
        [Display(Name = "Автор")]
        public string Author { get; set; }

        // тема
        [Required(ErrorMessage = "Не указана тема публикации")]
        [Display(Name = "Тема")]
        public string Theme { get; set; }

        [Required(ErrorMessage = "Не указана аннотация")]
        [Display(Name = "Аннотация")]
        public string Annotation { get; set; }

        public string Nickname { get; set; }
        public int Token { get; set; }
        public string Path { get; set; }
        public string FileName { get; set; }
    }

 

    public class ShowPublicationModel
    {
        public int? Id { get; set; }
        public bool IsUser { get; set; }

        [Display(Name = "Название")]
        public string Name { get; set; }

        [Display(Name = "Автор")]
        public string Author { get; set; }

        [Display(Name = "Год")]
        public int Year { get; set; }

        [Display(Name = "Тема")]
        public string Theme { get; set; }

        [Display(Name = "Аннотация")]
        public string Annotation { get; set; }


    }

    public class PublicationModel
    {

        public int Id { get; set; }

        [Display(Name = "Название")]
        public string Name { get; set; }

        [Display(Name = "Автор")]
        public string Author { get; set; }

        [Display(Name = "Год")]
        public int Year { get; set; }

        [Display(Name = "Тема")]
        public string Theme { get; set; }

        [Display(Name = "Аннотация")]
        public string Annotation { get; set; }

        public string Nickname { get; set; }
 
        public int Token { get; set; }
        public string Path { get; set; }
        public string FileName { get; set; }

    }


    public class ThemeTable
    {
        public string Id { get; set; }
        public string Theme { get; set; }
    }
}
