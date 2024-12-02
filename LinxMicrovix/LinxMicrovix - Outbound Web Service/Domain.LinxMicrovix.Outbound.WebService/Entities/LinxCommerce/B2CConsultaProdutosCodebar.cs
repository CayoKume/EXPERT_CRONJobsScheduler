﻿using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using Domain.IntegrationsCore.CustomValidations;

namespace Domain.LinxMicrovix.Outbound.WebService.Entites.LinxCommerce
{
    public class B2CConsultaProdutosCodebar
    {
        [Column(TypeName = "datetime")]
        public DateTime? lastupdateon { get; private set; }

        [Key]
        [Column(TypeName = "bigint")]
        public Int64? codigoproduto { get; private set; }

        [Column(TypeName = "varchar(20)")]
        [LengthValidation(length: 20, propertyName: "codebar")]
        public string? codebar { get; private set; }

        [Key]
        [Column(TypeName = "int")]
        public Int32? id_produtos_codebar { get; private set; }

        [Column(TypeName = "bit")]
        public Int32? principal { get; private set; }

        [Column(TypeName = "int")]
        public Int32? empresa { get; private set; }

        [Column(TypeName = "bigint")]
        public Int64? timestamp { get; private set; }

        [Column(TypeName = "varchar(20)")]
        [LengthValidation(length: 20, propertyName: "tipo_codebar")]
        public string? tipo_codebar { get; private set; }

        [Column(TypeName = "int")]
        public Int32? portal { get; private set; }

        public B2CConsultaProdutosCodebar() { }

        public B2CConsultaProdutosCodebar(
            List<ValidationResult> listValidations,
            string? codigoproduto,
            string? codebar,
            string? id_produtos_codebar,
            string? principal,
            string? empresa,
            string? timestamp,
            string? tipo_codebar,
            string? portal
        )
        {
            lastupdateon = DateTime.Now;

            this.codigoproduto =
                ConvertToInt64Validation.IsValid(codigoproduto, "codigoproduto", listValidations) ?
                Convert.ToInt64(codigoproduto) :
                0;

            this.id_produtos_codebar =
                ConvertToInt32Validation.IsValid(id_produtos_codebar, "id_produtos_codebar", listValidations) ?
                Convert.ToInt32(id_produtos_codebar) :
                0;

            this.principal =
                ConvertToInt32Validation.IsValid(principal, "principal", listValidations) ?
                Convert.ToInt32(principal) :
                0;

            this.empresa =
                ConvertToInt32Validation.IsValid(empresa, "empresa", listValidations) ?
                Convert.ToInt32(empresa) :
                0;

            this.portal =
                ConvertToInt32Validation.IsValid(portal, "portal", listValidations) ?
                Convert.ToInt32(portal) :
                0;

            this.timestamp =
                ConvertToInt64Validation.IsValid(timestamp, "timestamp", listValidations) ?
                Convert.ToInt64(timestamp) :
                0;

            this.tipo_codebar = tipo_codebar;
            this.codebar = codebar;
        }
    }
}
