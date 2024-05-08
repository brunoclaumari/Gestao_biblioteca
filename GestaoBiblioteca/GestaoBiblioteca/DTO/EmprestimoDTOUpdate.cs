using GestaoBiblioteca.Enums;

namespace GestaoBiblioteca.DTO
{
    public class EmprestimoDTOUpdate : EmprestimoPadraoDTO// : EmprestimoDTOEntrada
    {
        //public DateTime DataEmprestimo { get; set; }


        //public DateTime DataDevolucao { get; set; }

        public EnumEmprestimoStatus StatusEmprestimo { get; set; }
    }
}
