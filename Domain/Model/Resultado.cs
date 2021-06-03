using System.Collections.Generic;

namespace Domain.Model
{
    public class Resultado
    {
        public string Acao { get; set; }
        public List<string> Inconsistencias { get; } = new List<string>();
        public bool Sucesso
        {
            get { return Inconsistencias == null || Inconsistencias.Count == 0; }
        }
    }
}
