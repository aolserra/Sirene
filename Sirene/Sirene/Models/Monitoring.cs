using System;

namespace Sirene.Models
{
    public class Monitoring
    {
        public string Id { get; set; }

        public string Nome { get; set; }

        public bool IsOnline { get; set; }

        public string Resposta { get; set; }

        public bool TemPessoasComProbMobilidade { get; set; }

        public bool QtdPessoasComProbMobilidade { get; set; }
    }
}