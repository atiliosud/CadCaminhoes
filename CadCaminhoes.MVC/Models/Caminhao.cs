using System.ComponentModel.DataAnnotations;
using CadCaminhoes.MVC.Helpers;
using Microsoft.AspNetCore.Identity;

namespace CadCaminhoes.MVC.Models
{
    public class Caminhao
    {

        [Key]
        public Guid Id { get; set; } = Guid.NewGuid();

        [Display(Name = "Modelo")]
        [Required]
        [MedelosPermitidos]
        public string Modelo { get; set; } = string.Empty;


        [Display(Name = "Ano de Fabricação")]
        [Required]
        [AnoAtual]
        public int FabricacaoAno { get; set; }


        [Display(Name = "Ano Modelo")]
        [Required]
        [AnoAtualMaisProx]
        public int ModeloAno { get; set; }

        [Display(Name = "Criado por")]
        [Required]
        public string CreateById { get; set; } = string.Empty;

        [Display(Name = "Criado por")]
        public IdentityUser? CreateBy { get; set; }

        [Display(Name = "Criado às")]
        [Required]
        [DataType(DataType.Date)]
        public DateTimeOffset CreateAt { get; set; }
    }
}