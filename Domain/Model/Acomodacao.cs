namespace Domain.Model
{
    public class Acomodacao
    {
        public string Nome { get; private set; }
        public string Tipo { get; private set; }
        public decimal Diaria { get; private set; }
        public string Area { get; private set; }
        public string Local { get; private set; }

        public Acomodacao(string nome, string tipo, decimal diaria, string area, string local)
        {
            Nome = nome;
            Tipo = tipo;
            Diaria = diaria;
            Area = area;
            Local = local;
        }

    }
}
