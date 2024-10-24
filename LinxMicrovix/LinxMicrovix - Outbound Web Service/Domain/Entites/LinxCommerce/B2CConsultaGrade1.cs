﻿using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace LinxMicrovix_Outbound_Web_Service.Domain.Entites.LinxCommerce
{
    public class B2CConsultaGrade1
    {
        [Column(TypeName = "datetime")]
        public DateTime? lastupdateon { get; private set; }

        [Key]
        [Column(TypeName = "int")]
        public Int32? codigo_grade1 { get; private set; }

        [Column(TypeName = "varchar(100)")]
        public string? nome_grade1 { get; private set; }

        [Column(TypeName = "bigint")]
        public Int64? timestamp { get; private set; }

        [Column(TypeName = "int")]
        public Int32? portal { get; private set; }

        public B2CConsultaGrade1() { }

        public B2CConsultaGrade1(
            string? codigo_grade1,
            string? nome_grade1,
            string? timestamp,
            string? portal
        )
        {
            lastupdateon = DateTime.Now;

            this.codigo_grade1 =
                codigo_grade1 == String.Empty ? 0
                : Convert.ToInt32(codigo_grade1);

            this.nome_grade1 =
                nome_grade1 == String.Empty ? ""
                : nome_grade1.Substring(
                    0,
                    nome_grade1.Length > 100 ? 100
                    : nome_grade1.Length
                );

            this.timestamp =
                timestamp == String.Empty ? 0
                : Convert.ToInt64(timestamp);

            this.portal =
                portal == String.Empty ? 0
                : Convert.ToInt32(portal);
        }
    }
}
