﻿using Domain.IntegrationsCore.CustomValidations;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Domain.LinxMicrovix.Outbound.WebService.Entites.LinxMicrovix
{
    public class LinxClientesFornecCamposAdicionais
    {
        [Column(TypeName = "datetime")]
        public DateTime? lastupdateon { get; private set; }

        [Column(TypeName = "int")]
        public Int32 portal { get; private set; }

        [Key]
        [Column(TypeName = "int")]
        public Int32 cod_cliente { get; private set; }

        [Column(TypeName = "varchar(50)")]
        [LengthValidation(length: 50, propertyName: "campo")]
        public string? campo { get; private set; }

        [Column(TypeName = "varchar(100)")]
        [LengthValidation(length: 100, propertyName: "valor")]
        public string? valor { get; private set; }

        public LinxClientesFornecCamposAdicionais() { }

        public LinxClientesFornecCamposAdicionais(
            List<ValidationResult> listValidations,
            string? portal,
            string? cod_cliente,
            string? campo,
            string? valor
        )
        {
            lastupdateon = DateTime.Now;

            this.cod_cliente =
                ConvertToInt32Validation.IsValid(cod_cliente, "cod_cliente", listValidations) ?
                Convert.ToInt32(cod_cliente) :
                0;

            this.portal =
                ConvertToInt32Validation.IsValid(portal, "portal", listValidations) ?
                Convert.ToInt32(portal) :
                0;

            this.campo = campo;
            this.valor = valor;
        }
    }
}
