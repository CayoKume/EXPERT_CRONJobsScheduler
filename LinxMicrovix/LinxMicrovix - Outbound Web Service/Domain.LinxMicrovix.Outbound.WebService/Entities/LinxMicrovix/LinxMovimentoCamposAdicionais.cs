﻿using Domain.IntegrationsCore.CustomValidations;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Domain.LinxMicrovix.Outbound.WebService.Entites.LinxMicrovix
{
    public class LinxMovimentoCamposAdicionais
    {
        [Column(TypeName = "datetime")]
        public DateTime? lastupdateon { get; private set; }

        [Key]
        [Column(TypeName = "int")]
        public Int32? id_ordservprod { get; private set; }

        [Column(TypeName = "int")]
        public Int32? transacao { get; private set; }
        
        [Column(TypeName = "varchar(100)")]
        [LengthValidation(length: 100, propertyName: "paciente")]
        public string? paciente { get; private set; }
        
        [Column(TypeName = "varchar(40)")]
        [LengthValidation(length: 40, propertyName: "codigo_gerencial")]
        public string? codigo_gerencial { get; private set; }
        
        [Column(TypeName = "bigint")]
        public Int64? timestamp { get; private set; }

        public LinxMovimentoCamposAdicionais() { }

        public LinxMovimentoCamposAdicionais(
            List<ValidationResult> listValidations,
            string? id_ordservprod,
            string? transacao,
            string? paciente,
            string? codigo_gerencial,
            string? timestamp
        )
        {
            lastupdateon = DateTime.Now;

            this.id_ordservprod =
                ConvertToInt32Validation.IsValid(id_ordservprod, "id_ordservprod", listValidations) ?
                Convert.ToInt32(id_ordservprod) :
                0;

            this.transacao =
                ConvertToInt32Validation.IsValid(transacao, "transacao", listValidations) ?
                Convert.ToInt32(transacao) :
                0;

            this.timestamp =
                ConvertToInt64Validation.IsValid(timestamp, "timestamp", listValidations) ?
                Convert.ToInt64(timestamp) :
                0;

            this.paciente = paciente;
            this.codigo_gerencial = codigo_gerencial;
        }
    }
}
