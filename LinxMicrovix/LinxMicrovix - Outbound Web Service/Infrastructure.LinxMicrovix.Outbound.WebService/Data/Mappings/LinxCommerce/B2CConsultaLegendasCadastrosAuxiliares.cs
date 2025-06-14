using Infrastructure.IntegrationsCore.Data.Schemas;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using Domain.LinxMicrovix.Outbound.WebService.Entities.LinxCommerce;
using Domain.IntegrationsCore.Entities.Enums;
using Infrastructure.IntegrationsCore.Data.Extensions;
using Domain.LinxMicrovix.Outbound.WebService.Entities.LinxMicrovix;

namespace Infrastructure.LinxMicrovix.Outbound.WebService.Data.Mappings.LinxCommerce
{
    public class B2CConsultaLegendasCadastrosAuxiliaresMap : IEntityTypeConfiguration<B2CConsultaLegendasCadastrosAuxiliares>
    {
        public void Configure(EntityTypeBuilder<B2CConsultaLegendasCadastrosAuxiliares> builder)
        {
            var schema = SchemaContext.GetSchema(typeof(B2CConsultaLegendasCadastrosAuxiliares));

            builder.ToTable("B2CConsultaLegendasCadastrosAuxiliares");

            if (schema == "linx_microvix_commerce")
            {
                builder.HasKey(e => e.empresa);
                builder.Ignore(e => e.id);
            }
            else
            {
                builder.HasKey(e => e.id);

                builder.Property(e => e.id)
                    .HasColumnType("int")
                    .ValueGeneratedOnAdd();
            }
            
            builder.Property(e => e.lastupdateon)
                .HasProviderColumnType(EnumTableColumnType.DateTime);

            builder.Property(e => e.empresa)
                .HasColumnType("int");

            builder.Property(e => e.legenda_setor)
                .HasColumnType("varchar(20)");

            builder.Property(e => e.legenda_linha)
                .HasColumnType("varchar(20)");

            builder.Property(e => e.legenda_marca)
                .HasColumnType("varchar(20)");

            builder.Property(e => e.legenda_colecao)
                .HasColumnType("varchar(20)");

            builder.Property(e => e.legenda_grade1)
                .HasColumnType("varchar(20)");

            builder.Property(e => e.legenda_grade2)
                .HasColumnType("varchar(20)");

            builder.Property(e => e.legenda_espessura)
                .HasColumnType("varchar(20)");

            builder.Property(e => e.legenda_classificacao)
                .HasColumnType("varchar(20)");

            builder.Property(e => e.timestamp)
                .HasColumnType("bigint");
        }
    }
}
