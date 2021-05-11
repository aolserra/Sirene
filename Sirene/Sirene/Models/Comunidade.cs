using System;

namespace Sirene.Models
{
    public class Comunidade
    {
        public string Id { get; set; }

        public string CodigoComunidade { get; set; }

        public string NomeComunidade { get; set; }

        public string ResponsavelComunidade { get; set; }

        public string BairroComunidade { get; set; }

        public string CidadeComunidade { get; set; }

        public string UFComunidade { get; set; }

        public DateTime DataCriacaoComunidade { get; set; }

        public DateTime DataAlteracaoComunidade { get; set; }

        public string LogAlteracaoComunidade { get; set; }
    }
}