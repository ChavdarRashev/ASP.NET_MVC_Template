using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace FileManagement.Models.ViewModels
{
    public class InputFileModel
    {
        [System.ComponentModel.DataAnnotations.Required]
        [Display(Name = "Файл")]
        public IFormFile FormFile { get; set; }        
    }
}
