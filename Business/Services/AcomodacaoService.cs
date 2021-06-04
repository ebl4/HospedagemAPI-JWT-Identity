using Domain.Model;
using Persistence.Context;
using System.Collections.Generic;
using System.Linq;

namespace Business
{
    public class AcomodacaoService
    {
        private CatalogoDbContext _context;

        public AcomodacaoService(CatalogoDbContext context)
        {
            _context = context;
        }

        public Acomodacao Obter(string nome)
        {
            nome = nome?.Trim();
            if (string.IsNullOrWhiteSpace(nome))
            {
                return _context.Acomodacoes.Where(a => a.Nome == nome).FirstOrDefault();
            }
            else
                return null;
        }

        public IEnumerable<Acomodacao> ListarTodos()
        {
            return _context.Acomodacoes.OrderBy(p => p.Nome).ToList();
        }

        public Resultado Incluir(Acomodacao dadosAcomodacao)
        {
            Resultado resultado = DadosValidos(dadosAcomodacao);
            resultado.Acao = "Inclusão de Acomodação";

            if(resultado.Inconsistencias.Count == 0)
            {
                _context.Acomodacoes.Add(dadosAcomodacao);
                _context.SaveChanges();
            }

            return resultado;
        }

        private Resultado DadosValidos(Acomodacao acomodacao)
        {
            var resultado = new Resultado();
            if (acomodacao == null) resultado.Inconsistencias.Add("Preencha os Dados da Acomodação");
            else
            {
                if (string.IsNullOrWhiteSpace(acomodacao.Local)) resultado.Inconsistencias.Add("Preencha o Local");
                if (string.IsNullOrWhiteSpace(acomodacao.Tipo)) resultado.Inconsistencias.Add("Preencha o Tipo da Acomodação");
                if (string.IsNullOrWhiteSpace(acomodacao.Area)) resultado.Inconsistencias.Add("Preencha a Área da Acomodação");
                if (acomodacao.Diaria <= 0) resultado.Inconsistencias.Add("O valor da diária deve ser maior que zero");
            }

            return resultado;
        }
    }
}
