﻿using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace LinxMicrovix_Outbound_Web_Service.Domain.Entites.LinxCommerce
{
    public class B2CConsultaProdutosPalavrasChavePesquisa
    {
        [Column(TypeName = "datetime")]
        public DateTime? lastupdateon { get; set; }

        [Column(TypeName = "int")]
        public Int32? portal { get; set; }

        [Key]
        [Column(TypeName = "int")]
        public Int32? id_b2c_palavras_chave_pesquisa_produtos { get; set; }

        [Column(TypeName = "int")]
        public Int32? id_b2c_palavras_chave_pesquisa { get; set; }

        [Column(TypeName = "bigint")]
        public Int64? codigoproduto { get; set; }

        [Column(TypeName = "bigint")]
        public Int64? timestamp { get; set; }

        [Column(TypeName = "varchar(300)")]
        public string? descricao_b2c_palavras_chave_pesquisa { get; set; }
    }
}
