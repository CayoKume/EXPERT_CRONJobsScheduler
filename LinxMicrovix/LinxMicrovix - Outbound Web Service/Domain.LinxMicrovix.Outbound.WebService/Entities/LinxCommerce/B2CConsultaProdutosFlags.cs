﻿using Domain.IntegrationsCore.CustomValidations;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Domain.LinxMicrovix.Outbound.WebService.Entites.LinxCommerce
{
    public class B2CConsultaProdutosFlags
    {
        [Column(TypeName = "datetime")]
        public DateTime? lastupdateon { get; private set; }

        [Column(TypeName = "int")]
        public Int32? portal { get; private set; }

        [Key]
        [Column(TypeName = "int")]
        public Int32? id_b2c_flags_produtos { get; private set; }

        [Column(TypeName = "int")]
        public Int32? id_b2c_flags { get; private set; }

        [Column(TypeName = "bigint")]
        public Int64? codigoproduto { get; private set; }

        [Column(TypeName = "bigint")]
        public Int64? timestamp { get; private set; }

        [Column(TypeName = "varchar(300)")]
        [LengthValidation(length: 300, propertyName: "descricao_b2c_flags")]
        public string? descricao_b2c_flags { get; private set; }

        public B2CConsultaProdutosFlags() { }

        public B2CConsultaProdutosFlags(
            List<ValidationResult> listValidations,
            string? portal,
            string? id_b2c_flags_produtos,
            string? id_b2c_flags,
            string? codigoproduto,
            string? timestamp,
            string? descricao_b2c_flags
        )
        {
            lastupdateon = DateTime.Now;

            this.id_b2c_flags_produtos =
                String.IsNullOrEmpty(id_b2c_flags_produtos) ? 0
                : Convert.ToInt32(id_b2c_flags_produtos);

            this.id_b2c_flags =
                String.IsNullOrEmpty(id_b2c_flags) ? 0
                : Convert.ToInt32(id_b2c_flags);

            this.codigoproduto =
                String.IsNullOrEmpty(codigoproduto) ? 0
                : Convert.ToInt64(codigoproduto);

            this.descricao_b2c_flags = descricao_b2c_flags;

            this.portal =
                ConvertToInt32Validation.IsValid(portal, "portal", listValidations) ?
                Convert.ToInt32(portal) :
                0;

            this.timestamp =
                ConvertToInt64Validation.IsValid(timestamp, "timestamp", listValidations) ?
                Convert.ToInt64(timestamp) :
                0;
        }
    }
}
