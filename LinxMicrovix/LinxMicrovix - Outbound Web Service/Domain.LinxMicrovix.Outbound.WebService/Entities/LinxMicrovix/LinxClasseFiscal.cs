﻿using Domain.IntegrationsCore.CustomValidations;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Domain.LinxMicrovix.Outbound.WebService.Entites.LinxMicrovix
{
    public class LinxClasseFiscal
    {
        [Column(TypeName = "datetime")]
        public DateTime? lastupdateon { get; private set; }

        [Column(TypeName = "int")]
        public Int32 portal { get; private set; }

        [Key]
        [Column(TypeName = "int")]
        public Int32 id_classe_fiscal { get; private set; }

        [Column(TypeName = "varchar(150)")]
        [LengthValidation(length: 150, propertyName: "descricao")]
        public string? descricao { get; private set; }

        [Column(TypeName = "bit")]
        public bool excluido { get; private set; }

        [Column(TypeName = "bigint")]
        public Int64 timestamp { get; private set; }

        public LinxClasseFiscal() { }

        public LinxClasseFiscal(
            List<ValidationResult> listValidations,
            string? portal,
            string? id_classe_fiscal,
            string? descricao,
            string? excluido,
            string? timestamp
        )
        {
            lastupdateon = DateTime.Now;

            this.id_classe_fiscal =
                ConvertToInt32Validation.IsValid(id_classe_fiscal, "id_classe_fiscal", listValidations) ?
                Convert.ToInt32(id_classe_fiscal) :
                0;

            this.excluido =
                ConvertToBooleanValidation.IsValid(excluido, "excluido", listValidations) ?
                Convert.ToBoolean(excluido) :
                false;

            this.portal =
                ConvertToInt32Validation.IsValid(portal, "portal", listValidations) ?
                Convert.ToInt32(portal) :
                0;

            this.timestamp =
                ConvertToInt64Validation.IsValid(timestamp, "timestamp", listValidations) ?
                Convert.ToInt64(timestamp) :
                0;

            this.descricao = descricao;
        }
    }
}
