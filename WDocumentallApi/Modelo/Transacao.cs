using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Modelo
{
    public enum Transacao
    {
        SolicitacaoDeRegistro = 1,
        AlterarSolicitacaoDeRegistro,
        AlterarStatusSolicitacaoRegistro,
        SolicitacaoDeRegistroDeCustas,
        SolicitacaoDeRegistroDeExigencias
    }
}
