﻿using Domain.IntegrationsCore.CustomValidations;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Domain.LinxMicrovix.Outbound.WebService.Entites.LinxMicrovix
{
    public class LinxMovimentoRemessasAcertosItens
    {
        [Column(TypeName = "datetime")]
        public DateTime? lastupdateon { get; private set; }

        [Key]
        [Column(TypeName = "int")]
        public Int32? id_remessas_acertos { get; private set; }

        [Column(TypeName = "int")]
        public Int32? portal { get; private set; }
        
        [Column(TypeName = "int")]
        public Int32? empresa { get; private set; }
        
        [Column(TypeName = "int")]
        public Int32? transacao { get; private set; }
        
        [Column(TypeName = "decimal(10,2)")]
        public decimal? qtde_total { get; private set; }
        
        [Column(TypeName = "int")]
        public Int32? id_remessa_operacoes { get; private set; }
        
        [Column(TypeName = "int")]
        public Int32? id_remessas_itens { get; private set; }
        
        [Column(TypeName = "bigint")]
        public Int64? timestamp { get; private set; }

        public LinxMovimentoRemessasAcertosItens() { }

        public LinxMovimentoRemessasAcertosItens(
            List<ValidationResult> listValidations,
            string? id_remessas_acertos,
            string? portal,
            string? empresa,
            string? transacao,
            string? qtde_total,
            string? id_remessa_operacoes,
            string? id_remessas_itens,
            string? timestamp
        )
        {
            lastupdateon = DateTime.Now;

            this.portal =
                ConvertToInt32Validation.IsValid(portal, "portal", listValidations) ?
                Convert.ToInt32(portal) :
                0;

            this.empresa =
                ConvertToInt32Validation.IsValid(empresa, "empresa", listValidations) ?
                Convert.ToInt32(empresa) :
                0;

            this.id_remessas_acertos =
                ConvertToInt32Validation.IsValid(id_remessas_acertos, "id_remessas_acertos", listValidations) ?
                Convert.ToInt32(id_remessas_acertos) :
                0;

            this.transacao =
                ConvertToInt32Validation.IsValid(transacao, "transacao", listValidations) ?
                Convert.ToInt32(transacao) :
                0;

            this.id_remessa_operacoes =
                ConvertToInt32Validation.IsValid(id_remessa_operacoes, "id_remessa_operacoes", listValidations) ?
                Convert.ToInt32(id_remessa_operacoes) :
                0;

            this.id_remessas_itens =
                ConvertToInt32Validation.IsValid(id_remessas_itens, "id_remessas_itens", listValidations) ?
                Convert.ToInt32(id_remessas_itens) :
                0;

            this.qtde_total =
                ConvertToDecimalValidation.IsValid(qtde_total, "qtde_total", listValidations) ?
                Convert.ToDecimal(qtde_total) :
                0;

            this.timestamp =
                ConvertToInt64Validation.IsValid(timestamp, "timestamp", listValidations) ?
                Convert.ToInt64(timestamp) :
                0;
        }
    }
}
