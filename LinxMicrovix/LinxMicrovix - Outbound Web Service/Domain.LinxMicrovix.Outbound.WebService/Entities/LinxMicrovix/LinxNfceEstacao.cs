﻿using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using Domain.IntegrationsCore.CustomValidations;

namespace Domain.LinxMicrovix.Outbound.WebService.Entites.LinxMicrovix
{
    public class LinxNfceEstacao
    {
        [Column(TypeName = "datetime")]
        public DateTime? lastupdateon { get; private set; }

        [Key]
        [Column(TypeName = "int")]
        public Int32? id_nfce_estacao { get; private set; }

        [Column(TypeName = "int")]
        public Int32? empresa { get; private set; }
        
        [Column(TypeName = "varchar(50)")]
        [LengthValidation(length: 50, propertyName: "descricao")]
        public string? descricao { get; private set; }
        
        [Column(TypeName = "varchar(20)")]
        [LengthValidation(length: 20, propertyName: "numero_pdv_tef")]
        public string? numero_pdv_tef { get; private set; }
        
        [Column(TypeName = "bit")]
        public bool? ativo { get; private set; }
        
        [Column(TypeName = "bigint")]
        public Int64? timestamp { get; private set; }

        public LinxNfceEstacao() { }

        public LinxNfceEstacao(
            List<ValidationResult> listValidations,
            string? id_nfce_estacao,
            string? empresa,
            string? descricao,
            string? numero_pdv_tef,
            string? ativo,
            string? timestamp
        )
        {
            lastupdateon = DateTime.Now;

            this.id_nfce_estacao =
                ConvertToInt32Validation.IsValid(id_nfce_estacao, "id_nfce_estacao", listValidations) ?
                Convert.ToInt32(id_nfce_estacao) :
                0;

            this.empresa =
                ConvertToInt32Validation.IsValid(empresa, "empresa", listValidations) ?
                Convert.ToInt32(empresa) :
                0;

            this.ativo =
                ConvertToBooleanValidation.IsValid(ativo, "ativo", listValidations) ?
                Convert.ToBoolean(ativo) :
                false;

            this.timestamp =
                ConvertToInt64Validation.IsValid(timestamp, "timestamp", listValidations) ?
                Convert.ToInt64(timestamp) :
                0;

            this.descricao = descricao;
            this.numero_pdv_tef = numero_pdv_tef;
        }
    }
}
