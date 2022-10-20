using System.ComponentModel.DataAnnotations;

namespace CadCaminhoes.MVC.Helpers
{
    public class AnoAtualAttribute : ValidationAttribute
    {
        int anoAtual;
        public AnoAtualAttribute() 
        {
            anoAtual = DateTimeOffset.Now.Year;
            ErrorMessage = $"somente o ano {anoAtual} é permitido.";
        }

        public override bool IsValid(object value)
        {
            int ano = Convert.ToInt32(value);
            return ano == anoAtual;
        }
    }
    public class AnoAtualMaisProxAttribute : ValidationAttribute
    {
        int anoAtual;
        public AnoAtualMaisProxAttribute()
        {
            anoAtual = DateTimeOffset.Now.Year;
            ErrorMessage = $"somente o ano {anoAtual} e {anoAtual + 1} são permitidos.";
        }

        public override bool IsValid(object value)
        {
            int ano = Convert.ToInt32(value);
            return ano == anoAtual || ano == anoAtual + 1;
        }
    }


    public class MedelosPermitidosAttribute : ValidationAttribute
    {
        string[] modelos;
        public MedelosPermitidosAttribute()
        {
            modelos = new string[] { "FH", "FM" };
            ErrorMessage = $"modelo não permitido.";
        }

        public override bool IsValid(object value)
        {
            string modelo = Convert.ToString(value);
            return modelos.Contains(modelo);
        }
    }
}
