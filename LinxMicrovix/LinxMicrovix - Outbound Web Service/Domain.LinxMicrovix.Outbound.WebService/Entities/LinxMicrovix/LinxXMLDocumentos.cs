﻿using Domain.IntegrationsCore.CustomValidations;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Globalization;

namespace Domain.LinxMicrovix.Outbound.WebService.Entites.LinxMicrovix
{
    public class LinxXMLDocumentos
    {
        [Column(TypeName = "datetime")]
        public DateTime? lastupdateon { get; private set; }

        [Column(TypeName = "int")]
        public Int32? portal { get; private set; }

        [Column(TypeName = "varchar(14)")]
        [LengthValidation(length: 14, propertyName: "cnpj_emp")]
        public string? cnpj_emp { get; private set; }

        [Column(TypeName = "int")]
        public Int32? documento { get; private set; }

        [Column(TypeName = "varchar(10)")]
        [LengthValidation(length: 10, propertyName: "serie")]
        public string? serie { get; private set; }

        [Column(TypeName = "datetime")]
        public DateTime? data_emissao { get; private set; }

        [Key]
        [Column(TypeName = "varchar(44)")]
        [LengthValidation(length: 44, propertyName: "chave_nfe")]
        public string? chave_nfe { get; private set; }

        [Column(TypeName = "int")]
        public Int32? situacao { get; private set; }
        
        [Column(TypeName = "varchar(max)")]
        public string? xml { get; private set; }

        [Column(TypeName = "bit")]
        public bool? excluido { get; private set; }

        [Column(TypeName = "uniqueidentifier")]
        public Guid? identificador_microvix { get; private set; }
        
        [Column(TypeName = "datetime")]
        public DateTime? dt_insert { get; private set; }

        [Column(TypeName = "bigint")]
        public Int64? timestamp { get; private set; }
        
        [Column(TypeName = "varchar(15)")]
        [LengthValidation(length: 15, propertyName: "nProtCanc")]
        public string? nProtCanc { get; private set; }
        
        [Column(TypeName = "varchar(15)")]
        [LengthValidation(length: 15, propertyName: "nProtInut")]
        public string? nProtInut { get; private set; }
        
        [Column(TypeName = "varchar(max)")]
        public string? xmlDistribuicao { get; private set; }
        
        [Column(TypeName = "varchar(15)")]
        [LengthValidation(length: 15, propertyName: "nProtDeneg")]
        public string? nProtDeneg { get; private set; }

        [Column(TypeName = "varchar(5)")]
        [LengthValidation(length: 5, propertyName: "cStat")]
        public string? cStat { get; private set; }
        
        [Column(TypeName = "int")]
        public Int32? id_nfe { get; private set; }
        
        [Column(TypeName = "int")]
        public Int32? cod_cliente { get; private set; }

        public LinxXMLDocumentos() { }

        public LinxXMLDocumentos(
            List<ValidationResult> listValidations,
            string? portal,
            string? cnpj_emp,
            string? documento,
            string? serie,
            string? data_emissao,
            string? chave_nfe,
            string? situacao,
            string? xml,
            string? excluido,
            string? identificador_microvix,
            string? dt_insert,
            string? timestamp,
            string? nProtCanc,
            string? nProtInut,
            string? xmlDistribuicao,
            string? nProtDeneg,
            string? cStat,
            string? id_nfe,
            string? cod_cliente
        )
        {
            lastupdateon = DateTime.Now;

            this.portal =
                ConvertToInt32Validation.IsValid(portal, "portal", listValidations) ?
                Convert.ToInt32(portal) :
                0;

            this.documento =
                ConvertToInt32Validation.IsValid(documento, "documento", listValidations) ?
                Convert.ToInt32(documento) :
                0;

            this.situacao =
                ConvertToInt32Validation.IsValid(situacao, "situacao", listValidations) ?
                Convert.ToInt32(situacao) :
                0;

            this.id_nfe =
                ConvertToInt32Validation.IsValid(id_nfe, "id_nfe", listValidations) ?
                Convert.ToInt32(id_nfe) :
                0;

            this.cod_cliente =
                ConvertToInt32Validation.IsValid(cod_cliente, "cod_cliente", listValidations) ?
                Convert.ToInt32(cod_cliente) :
                0;

            this.timestamp =
                ConvertToInt64Validation.IsValid(timestamp, "timestamp", listValidations) ?
                Convert.ToInt64(timestamp) :
                0;

            this.identificador_microvix =
                String.IsNullOrEmpty(identificador_microvix) ? null
                : Guid.Parse(identificador_microvix);

            this.excluido =
                ConvertToBooleanValidation.IsValid(excluido, "excluido", listValidations) ?
                Convert.ToBoolean(excluido) :
                false;

            this.data_emissao =
                ConvertToDateTimeValidation.IsValid(data_emissao, "data_emissao", listValidations) ?
                Convert.ToDateTime(data_emissao) :
                new DateTime(1990, 01, 01, 00, 00, 00, new CultureInfo("en-US").Calendar);

            this.dt_insert =
               ConvertToDateTimeValidation.IsValid(dt_insert, "dt_insert", listValidations) ?
               Convert.ToDateTime(dt_insert) :
               new DateTime(1990, 01, 01, 00, 00, 00, new CultureInfo("en-US").Calendar);

            this.cStat = cStat;
            this.nProtInut = nProtInut;
            this.nProtDeneg = nProtDeneg;
            this.xmlDistribuicao = xmlDistribuicao;
            this.xml = xml;
            this.nProtCanc = nProtCanc;
            this.chave_nfe = chave_nfe;
            this.serie = serie;
            this.cnpj_emp = cnpj_emp;
        }
    }
}
