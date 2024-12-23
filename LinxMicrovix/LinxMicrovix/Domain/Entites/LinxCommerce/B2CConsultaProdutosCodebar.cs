﻿using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace LinxMicrovix_Outbound_Web_Service.Domain.Entites.LinxCommerce
{
    public class B2CConsultaProdutosCodebar
    {
        [Column(TypeName = "datetime")]
        public DateTime? lastupdateon { get; private set; }

        [Key]
        [Column(TypeName = "bigint")]
        public Int64? codigoproduto { get; private set; }

        [Column(TypeName = "varchar(20)")]
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
        public string? tipo_codebar { get; private set; }

        [Column(TypeName = "int")]
        public Int32? portal { get; private set; }

        public B2CConsultaProdutosCodebar() { }

        public B2CConsultaProdutosCodebar(
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
                String.IsNullOrEmpty(codigoproduto) ? 0
                : Convert.ToInt64(codigoproduto);

            this.codebar =
                String.IsNullOrEmpty(codebar) ? ""
                : codebar.Substring(
                    0,
                    codebar.Length > 20 ? 20
                    : codebar.Length
                );

            this.id_produtos_codebar =
                String.IsNullOrEmpty(id_produtos_codebar) ? 0
                : Convert.ToInt32(id_produtos_codebar);

            this.principal =
                String.IsNullOrEmpty(principal) ? 0
                : Convert.ToInt32(principal);

            this.empresa =
                String.IsNullOrEmpty(empresa) ? 0
                : Convert.ToInt32(empresa);

            this.tipo_codebar =
                String.IsNullOrEmpty(tipo_codebar) ? ""
                : tipo_codebar.Substring(
                    0,
                    tipo_codebar.Length > 20 ? 20
                    : tipo_codebar.Length
                );

            this.timestamp =
                String.IsNullOrEmpty(timestamp) ? 0
                : Convert.ToInt64(timestamp);

            this.portal =
                String.IsNullOrEmpty(portal) ? 0
                : Convert.ToInt32(portal);
        }
    }
}
