﻿using Domain.IntegrationsCore.CustomValidations;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Domain.LinxMicrovix.Outbound.WebService.Entites.LinxMicrovix
{
    public class LinxRespostaVenda
    {
        [Column(TypeName = "datetime")]
        public DateTime? lastupdateon { get; private set; }

        [Key]
        [Column(TypeName = "int")]
        public Int32? id_resposta_venda { get; private set; }

        [Column(TypeName = "int")]
        public Int32? portal { get; private set; }

        [Column(TypeName = "varchar(100)")]
        [LengthValidation(length: 100, propertyName: "descricao_resposta")]
        public string? descricao_resposta { get; private set; }

        [Column(TypeName = "int")]
        public Int32? id_pergunta_venda { get; private set; }

        [Column(TypeName = "bigint")]
        public Int64? timestamp { get; private set; }

        public LinxRespostaVenda() { }

        public LinxRespostaVenda(
            List<ValidationResult> listValidations,
            string? portal,
            string? id_resposta_venda,
            string? descricao_resposta,
            string? id_pergunta_venda,
            string? timestamp
        )
        {
            lastupdateon = DateTime.Now;

            this.portal =
                ConvertToInt32Validation.IsValid(portal, "portal", listValidations) ?
                Convert.ToInt32(portal) :
                0;

            this.id_resposta_venda =
                ConvertToInt32Validation.IsValid(id_resposta_venda, "id_resposta_venda", listValidations) ?
                Convert.ToInt32(id_resposta_venda) :
                0;

            this.id_pergunta_venda =
                ConvertToInt32Validation.IsValid(id_pergunta_venda, "id_pergunta_venda", listValidations) ?
                Convert.ToInt32(id_pergunta_venda) :
                0;

            this.timestamp =
                ConvertToInt64Validation.IsValid(timestamp, "timestamp", listValidations) ?
                Convert.ToInt64(timestamp) :
                0;

            this.descricao_resposta = descricao_resposta;
        }
    }
}
