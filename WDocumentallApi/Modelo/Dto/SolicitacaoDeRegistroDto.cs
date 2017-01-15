using System;
using System.Collections.Generic;

namespace Modelo.Dto
{
    public class SolicitacaoDeRegistroDto
    {
        public int IdSolicitacaoDeRegistro { get; set; }
        public string Nome { get; set; }
        public int CPF { get; set; }
        public DateTime DataSolicitacao { get; set; }
        public Nullable<decimal> ValorDeclarado { get; set; }
        public Nullable<int> QuantidadePagina { get; set; }
        public string IDCertisign { get; set; }
        public int IdStatusSolicitacao { get; set; }

        public virtual StatusSolicitacao StatusSolicitacao { get; set; }
        public virtual IList<DocumentoDto> Documentos { get; set; }
        public virtual IList<SolicitacaoDeRegistroCustasDto> SolicitacaoDeRegistrosCustas { get; set; }
        public virtual IList<SolicitacaoDeRegistroExigenciasDto> SolicitacaoDeRegistrosExigencias { get; set; }

    }
}