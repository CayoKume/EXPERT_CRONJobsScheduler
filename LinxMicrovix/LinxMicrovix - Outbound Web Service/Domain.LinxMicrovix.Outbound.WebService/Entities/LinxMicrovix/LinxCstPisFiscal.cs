﻿using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using Domain.IntegrationsCore.CustomValidations;
using System.Globalization;

namespace Domain.LinxMicrovix.Outbound.WebService.Entites.LinxMicrovix
{
    public class LinxCstPisFiscal
    {
        [Column(TypeName = "datetime")]
        public DateTime? lastupdateon { get; private set; }

        [Column(TypeName = "int")]
        public Int32? portal { get; private set; }

        [Key]
        [Column(TypeName = "int")]
        public Int32? id_cst_pis_fiscal { get; private set; }

        [Column(TypeName = "varchar(4)")]
        [LengthValidation(length: 4, propertyName: "cst_pis_fiscal")]
        public string? cst_pis_fiscal { get; private set; }
        
        [Column(TypeName = "varchar(150)")]
        [LengthValidation(length: 150, propertyName: "descricao")]
        public string? descricao { get; private set; }
        
        [Column(TypeName = "bit")]
        public bool? excluido { get; private set; }
        
        [Column(TypeName = "datetime")]
        public DateTime? inicio_vigencia { get; private set; }
        
        [Column(TypeName = "datetime")]
        public DateTime? termino_vigencia { get; private set; }
        
        [Column(TypeName = "bigint")]
        public Int64? timestamp { get; private set; }

        public LinxCstPisFiscal() { }

        public LinxCstPisFiscal(
            List<ValidationResult> listValidations,
            string? portal,
            string? id_cst_pis_fiscal,
            string? cst_pis_fiscal,
            string? descricao,
            string? excluido,
            string? inicio_vigencia,
            string? termino_vigencia,
            string? timestamp
        )
        {
            lastupdateon = DateTime.Now;

            this.id_cst_pis_fiscal =
                ConvertToInt32Validation.IsValid(id_cst_pis_fiscal, "id_cst_pis_fiscal", listValidations) ?
                Convert.ToInt32(id_cst_pis_fiscal) :
                0;

            this.excluido =
                ConvertToBooleanValidation.IsValid(excluido, "excluido", listValidations) ?
                Convert.ToBoolean(excluido) :
                false;

            this.inicio_vigencia =
                ConvertToDateTimeValidation.IsValid(inicio_vigencia, "inicio_vigencia", listValidations) ?
                Convert.ToDateTime(inicio_vigencia) :
                new DateTime(1990, 01, 01, 00, 00, 00, new CultureInfo("en-US").Calendar);

            this.termino_vigencia =
                ConvertToDateTimeValidation.IsValid(termino_vigencia, "termino_vigencia", listValidations) ?
                Convert.ToDateTime(termino_vigencia) :
                new DateTime(1990, 01, 01, 00, 00, 00, new CultureInfo("en-US").Calendar);

            this.portal =
                ConvertToInt32Validation.IsValid(portal, "portal", listValidations) ?
                Convert.ToInt32(portal) :
                0;

            this.timestamp =
                ConvertToInt64Validation.IsValid(timestamp, "timestamp", listValidations) ?
                Convert.ToInt64(timestamp) :
                0;

            this.cst_pis_fiscal = cst_pis_fiscal;
            this.descricao = descricao;
        }
    }
}
