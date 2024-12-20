﻿using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Domain.LinxMicrovix.Outbound.WebService.Entites.LinxMicrovix
{
    public class LinxMetodos
    {
        [Column(TypeName = "datetime")]
        public DateTime? lastupdateon { get; private set; }

        [Key]
        [Column(TypeName = "int")]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int? methodID { get; set; }

        [Column(TypeName = "varchar(max)")]
        public string? Retorno { get; set; }

        public LinxMetodos() { }

        public LinxMetodos(
            string? Retorno
        )
        {
            lastupdateon = DateTime.Now;
            this.Retorno = Retorno;
        }
    }
}
