namespace GestaoBiblioteca.DTO
{
    public class EmprestimoDTOUpdate: EmprestimoDTOEntrada
    {
        public DateTime DataEmprestimo { get; set; }


        public DateTime DataDevolucao { get; set; }
    }
}
