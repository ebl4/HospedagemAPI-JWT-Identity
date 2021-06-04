namespace Domain.Model
{
    public class Acomodacao
    {
        private string nome;
        private string tipo;
        private decimal diaria;
        private string area;
        private string local;
        private int id;

        public int Id { get => id; set => id = value; }
        public string Nome { get => nome; set => nome = value; }
        public string Tipo { get => tipo; set => tipo = value; }
        public decimal Diaria { get => diaria; set => diaria = value; }
        public string Area { get => area; set => area = value; }
        public string Local { get => local; set => local = value; }

    }
}
