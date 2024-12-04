﻿using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using Domain.IntegrationsCore.CustomValidations;

namespace Domain.LinxMicrovix.Outbound.WebService.Entites.LinxMicrovix
{
    public class LinxSerialVenda
    {
        [Column(TypeName = "datetime")]
        public DateTime? lastupdateon { get; private set; }

        [Key]
        [Column(TypeName = "int")]
        public Int32? id_serial_venda { get; private set; }

        [Column(TypeName = "int")]
        public Int32? portal { get; private set; }
        
        [Column(TypeName = "int")]
        public Int32? transacao { get; private set; }

        [Column(TypeName = "bigint")]
        public Int64? codigoproduto { get; private set; }

        [Column(TypeName = "varchar(50)")]
        [LengthValidation(length: 50, propertyName: "serial")]
        public string? serial { get; private set; }
        
        [Column(TypeName = "bigint")]
        public Int64? timestamp { get; private set; }

        public LinxSerialVenda() { }

        public LinxSerialVenda(
            List<ValidationResult> listValidations,
            string? id_serial_venda,
            string? portal,
            string? transacao,
            string? codigoproduto,
            string? serial,
            string? timestamp
        )
        {
            lastupdateon = DateTime.Now;

            this.portal =
                ConvertToInt32Validation.IsValid(portal, "portal", listValidations) ?
                Convert.ToInt32(portal) :
                0;

            this.id_serial_venda =
                ConvertToInt32Validation.IsValid(id_serial_venda, "id_serial_venda", listValidations) ?
                Convert.ToInt32(id_serial_venda) :
                0;

            this.transacao =
                ConvertToInt32Validation.IsValid(transacao, "transacao", listValidations) ?
                Convert.ToInt32(transacao) :
                0;

            this.timestamp =
                ConvertToInt64Validation.IsValid(timestamp, "timestamp", listValidations) ?
                Convert.ToInt64(timestamp) :
                0;

            this.codigoproduto =
                ConvertToInt64Validation.IsValid(codigoproduto, "codigoproduto", listValidations) ?
                Convert.ToInt64(codigoproduto) :
                0;

            this.serial = serial;
        }
    }
}
