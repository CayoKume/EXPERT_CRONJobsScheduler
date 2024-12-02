﻿using Domain.IntegrationsCore.CustomValidations;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Domain.LinxMicrovix.Outbound.WebService.Entites.LinxCommerce
{
    public class B2CConsultaPedidosItens
    {
        [Column(TypeName = "datetime")]
        public DateTime? lastupdateon { get; private set; }

        [Key]
        [Column(TypeName = "int")]
        public Int32? id_pedido_item { get; private set; }

        [Key]
        [Column(TypeName = "int")]
        public Int32? id_pedido { get; private set; }

        [Key]
        [Column(TypeName = "bigint")]
        public Int64? codigoproduto { get; private set; }

        [Column(TypeName = "int")]
        public Int32? quantidade { get; private set; }

        [Column(TypeName = "decimal(10,2)")]
        public decimal? vl_unitario { get; private set; }

        [Column(TypeName = "bigint")]
        public Int64? timestamp { get; private set; }

        [Column(TypeName = "int")]
        public Int32? portal { get; private set; }

        public B2CConsultaPedidosItens() { }

        public B2CConsultaPedidosItens(
            List<ValidationResult> listValidations,
            string? id_pedido_item,
            string? id_pedido,
            string? codigoproduto,
            string? quantidade,
            string? vl_unitario,
            string? timestamp,
            string? portal
        )
        {
            lastupdateon = DateTime.Now;

            this.id_pedido_item =
                ConvertToInt32Validation.IsValid(id_pedido_item, "id_pedido_item", listValidations) ?
                Convert.ToInt32(id_pedido_item) :
                0;

            this.id_pedido =
                ConvertToInt32Validation.IsValid(id_pedido, "id_pedido", listValidations) ?
                Convert.ToInt32(id_pedido) :
                0;

            this.codigoproduto =
                ConvertToInt64Validation.IsValid(codigoproduto, "codigoproduto", listValidations) ?
                Convert.ToInt64(codigoproduto) :
                0;

            this.quantidade =
                ConvertToInt32Validation.IsValid(quantidade, "quantidade", listValidations) ?
                Convert.ToInt32(quantidade) :
                0;

            this.vl_unitario =
                ConvertToDecimalValidation.IsValid(vl_unitario, "vl_unitario", listValidations) ?
                Convert.ToDecimal(vl_unitario) :
                0;

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
