using System;

namespace Sirene.Models
{
	public class Usuario
	{
		public string Id { get; set; }

		public string IdAccess { get; set; }

		public string ConnectionID { get; set; }

		public bool IsOnline { get; set; }

		public string CodigoUsuario { get; set; }

		public string NomeUsuario { get; set; }

		public string ComunidadeUsuario { get; set; }

		public string IdComunidadeUsu { get; set; }

		public string CodigoComunidadeUsu { get; set; }

		public string PerfilUsuario { get; set; }

		public bool TemPessoasComProbMobilidade { get; set; }

		public int QtdPessoasComProbMobilidade { get; set; }

		public string RespostaNotifUsu { get; set; }

		public DateTime DataCriacaoUsuario { get; set; }

		public DateTime DataAlteracaoUsuario { get; set; }

		public string LogAlteracaoUsuario { get; set; }
	}
}