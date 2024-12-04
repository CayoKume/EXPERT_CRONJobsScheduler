﻿using Domain.IntegrationsCore.CustomValidations;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Globalization;

namespace Domain.LinxMicrovix.Outbound.WebService.Entites.LinxMicrovix
{
  public class LinxMovimento
  {
        [Column(TypeName = "datetime")]
        public DateTime? lastupdateon { get; private set; }

        [Column(TypeName = "int")]
		public Int32? portal { get; private set; }

        [Column(TypeName = "varchar(14)")]
        [LengthValidation(length: 14, propertyName: "cnpj_emp")]
        public string? cnpj_emp { get; private set; }
		
        [Column(TypeName = "int")]
        public Int32? transacao { get; private set; }
		
        [Column(TypeName = "int")]
        public Int32? usuario { get; private set; }
		
        [Column(TypeName = "int")]
        public Int32? documento { get; private set; }
		
        [Column(TypeName = "varchar(44)")]
        [LengthValidation(length: 44, propertyName: "chave_nf")]
        public string? chave_nf { get; private set; }
		
        [Column(TypeName = "int")]
        public Int32? ecf { get; private set; }
		
        [Column(TypeName = "varchar(30)")]
        [LengthValidation(length: 30, propertyName: "numero_serie_ecf")]
        public string? numero_serie_ecf { get; private set; }
		
        [Column(TypeName = "int")]
        public Int32? modelo_nf { get; private set; }

        [Column(TypeName = "datetime")]
        public DateTime? data_documento { get; private set; }

        [Column(TypeName = "datetime")]
        public DateTime? data_lancamento { get; private set; }
		
        [Column(TypeName = "int")]
        public Int32? codigo_cliente { get; private set; }
		
        [Column(TypeName = "varchar(10)")]
        [LengthValidation(length: 10, propertyName: "serie")]
        public string? serie { get; private set; }
		
        [Column(TypeName = "varchar(300)")]
        [LengthValidation(length: 300, propertyName: "desc_cfop")]
        public string? desc_cfop { get; private set; }
		
        [Column(TypeName = "varchar(5)")]
        [LengthValidation(length: 5, propertyName: "id_cfop")]
        public string? id_cfop { get; private set; }
		
        [Column(TypeName = "int")]
        public Int32? cod_vendedor { get; private set; }
		
        [Column(TypeName = "decimal(10,2)")]
        public decimal? quantidade { get; private set; }
		
        [Column(TypeName = "decimal(10,2)")]
        public decimal? preco_custo { get; private set; }
		
        [Column(TypeName = "decimal(10,2)")]
        public decimal? valor_liquido { get; private set; }
		
        [Column(TypeName = "decimal(10,2)")]
        public decimal? desconto { get; private set; }
		
        [Column(TypeName = "varchar(4)")]
        [LengthValidation(length: 4, propertyName: "cst_icms")]
        public string? cst_icms { get; private set; }
		
        [Column(TypeName = "varchar(4)")]
        [LengthValidation(length: 4, propertyName: "cst_pis")]
        public string? cst_pis { get; private set; }
		
        [Column(TypeName = "varchar(4)")]
        [LengthValidation(length: 4, propertyName: "cst_cofins")]
        public string? cst_cofins { get; private set; }
		
        [Column(TypeName = "varchar(4)")]
        [LengthValidation(length: 4, propertyName: "cst_ipi")]
        public string? cst_ipi { get; private set; }
		
        [Column(TypeName = "decimal(10,2)")]
        public decimal? valor_icms { get; private set; }
		
        [Column(TypeName = "decimal(10,2)")]
        public decimal? aliquota_icms { get; private set; }
		
        [Column(TypeName = "decimal(10,2)")]
        public decimal? base_icms { get; private set; }
		
        [Column(TypeName = "decimal(10,2)")]
        public decimal? valor_pis { get; private set; }
		
        [Column(TypeName = "decimal(10,2)")]
        public decimal? aliquota_pis { get; private set; }
		
        [Column(TypeName = "decimal(10,2)")]
        public decimal? base_pis { get; private set; }
		
        [Column(TypeName = "decimal(10,2)")]
        public decimal? valor_cofins { get; private set; }
		
        [Column(TypeName = "decimal(10,2)")]
        public decimal? aliquota_cofins { get; private set; }
		
        [Column(TypeName = "decimal(10,2)")]
        public decimal? base_cofins { get; private set; }
		
        [Column(TypeName = "decimal(10,2)")]
        public decimal? valor_icms_st { get; private set; }
		
        [Column(TypeName = "decimal(10,2)")]
        public decimal? aliquota_icms_st { get; private set; }
		
        [Column(TypeName = "decimal(10,2)")]
        public decimal? base_icms_st { get; private set; }
		
        [Column(TypeName = "decimal(10,2)")]
        public decimal? valor_ipi { get; private set; }
		
        [Column(TypeName = "decimal(10,2)")]
        public decimal? aliquota_ipi { get; private set; }
		
        [Column(TypeName = "decimal(10,2)")]
        public decimal? base_ipi { get; private set; }
		
        [Column(TypeName = "decimal(10,2)")]
        public decimal? valor_total { get; private set; }
		
        [Column(TypeName = "bit")]
        public bool? forma_dinheiro { get; private set; }
		
        [Column(TypeName = "decimal(10,2)")]
        public decimal? total_dinheiro { get; private set; }
		
        [Column(TypeName = "bit")]
        public bool? forma_cheque { get; private set; }
		
        [Column(TypeName = "decimal(10,2)")]
        public decimal? total_cheque { get; private set; }
		
        [Column(TypeName = "bit")]
        public bool? forma_cartao { get; private set; }
		
        [Column(TypeName = "decimal(10,2)")]
        public decimal? total_cartao { get; private set; }
		
        [Column(TypeName = "bit")]
        public bool? forma_crediario { get; private set; }

        [Column(TypeName = "decimal(10,2)")]
        public decimal? total_crediario { get; private set; }

        [Column(TypeName = "bit")]
        public bool? forma_convenio { get; private set; }
		
        [Column(TypeName = "decimal(10,2)")]
        public decimal? total_convenio { get; private set; }
		
        [Column(TypeName = "decimal(10,2)")]
        public decimal? frete { get; private set; }
		
        [Column(TypeName = "char(2)")]
        [LengthValidation(length: 2, propertyName: "operacao")]
        public string? operacao { get; private set; }
		
        [Column(TypeName = "char(1)")]
        [LengthValidation(length: 1, propertyName: "tipo_transacao")]
        public string? tipo_transacao { get; private set; }

        [Column(TypeName = "bigint")]
        public Int64? cod_produto { get; private set; }
		
        [Column(TypeName = "varchar(20)")]
        [LengthValidation(length: 20, propertyName: "cod_barra")]
        public string? cod_barra { get; private set; }
		
        [Column(TypeName = "char(1)")]
        [LengthValidation(length: 1, propertyName: "cancelado")]
        public string? cancelado { get; private set; }
		
        [Column(TypeName = "char(1)")]
        [LengthValidation(length: 1, propertyName: "excluido")]
        public string? excluido { get; private set; }
		
        [Column(TypeName = "char(1)")]
        [LengthValidation(length: 1, propertyName: "soma_relatorio")]
        public string? soma_relatorio { get; private set; }

        [Column(TypeName = "uniqueidentifier")]
        public Guid? identificador { get; private set; }
		
        [Column(TypeName = "varchar(100)")]
        [LengthValidation(length: 100, propertyName: "deposito")]
        public string? deposito { get; private set; }
		
        [Column(TypeName = "varchar(max)")]
        public string? obs { get; private set; }

        [Column(TypeName = "decimal(10,2)")]
        public decimal? preco_unitario { get; private set; }
		
        [Column(TypeName = "char(5)")]
        [LengthValidation(length: 5, propertyName: "hora_lancamento")]
        public string? hora_lancamento { get; private set; }
		
        [Column(TypeName = "varchar(60)")]
        [LengthValidation(length: 60, propertyName: "natureza_operacao")]
        public string? natureza_operacao { get; private set; }
		
        [Column(TypeName = "int")]
        public Int32? tabela_preco { get; private set; }
		
        [Column(TypeName = "varchar(50)")]
        [LengthValidation(length: 50, propertyName: "nome_tabela_preco")]
        public string? nome_tabela_preco { get; private set; }
		
        [Column(TypeName = "int")]
        public Int32? cod_sefaz_situacao { get; private set; }
		
        [Column(TypeName = "varchar(30)")]
        [LengthValidation(length: 30, propertyName: "desc_sefaz_situacao")]
        public string? desc_sefaz_situacao { get; private set; }
		
        [Column(TypeName = "varchar(15)")]
        [LengthValidation(length: 15, propertyName: "protocolo_aut_nfe")]
        public string? protocolo_aut_nfe { get; private set; }
		
        [Column(TypeName = "datetime")]
        public DateTime? dt_update { get; private set; }
		
        [Column(TypeName = "bit")]
        public bool? forma_cheque_prazo { get; private set; }
		
        [Column(TypeName = "decimal(10,2)")]
        public decimal? total_cheque_prazo { get; private set; }
		
        [Column(TypeName = "char(10)")]
        [LengthValidation(length: 10, propertyName: "cod_natureza_operacao")]
        public string? cod_natureza_operacao { get; private set; }
		
        [Column(TypeName = "decimal(10,2)")]
        public decimal? preco_tabela_epoca { get; private set; }
		
        [Column(TypeName = "decimal(10,2)")]
        public decimal? desconto_total_item { get; private set; }
		
        [Column(TypeName = "char(1)")]
        [LengthValidation(length: 1, propertyName: "conferido")]
        public string? conferido { get; private set; }
		
        [Column(TypeName = "int")]
        public Int32? transacao_pedido_venda { get; private set; }
		
        [Column(TypeName = "varchar(5)")]
        [LengthValidation(length: 5, propertyName: "codigo_modelo_nf")]
        public string? codigo_modelo_nf { get; private set; }
		
        [Column(TypeName = "decimal(10,2)")]
        public decimal? acrescimo { get; private set; }
		
        [Column(TypeName = "bit")]
        public bool? mob_checkout { get; private set; }
		
        [Column(TypeName = "decimal(10,2)")]
        public decimal? aliquota_iss { get; private set; }
		
        [Column(TypeName = "decimal(10,2)")]
        public decimal? base_iss { get; private set; }

        [Column(TypeName = "int")]
        public Int32? ordem { get; private set; }

        [Column(TypeName = "int")]
        public Int32? codigo_rotina_origem { get; private set; }
		
        [Column(TypeName = "bigint")]
        public Int64? timestamp { get; private set; }
		
        [Column(TypeName = "decimal(10,2)")]
        public decimal? troco { get; private set; }

        [Column(TypeName = "int")]
        public Int32? transportador { get; private set; }
		
        [Column(TypeName = "decimal(10,2)")]
        public decimal? icms_aliquota_desonerado { get; private set; }
		
        [Column(TypeName = "decimal(10,2)")]
        public decimal? icms_valor_desonerado_item { get; private set; }
		
        [Column(TypeName = "int")]
        public Int32? empresa { get; private set; }
		
        [Column(TypeName = "decimal(10,2)")]
        public decimal? desconto_item { get; private set; }
		
        [Column(TypeName = "decimal(10,2)")]
        public decimal? aliq_iss { get; private set; }
		
        [Column(TypeName = "decimal(10,2)")]
        public decimal? iss_base_item { get; private set; }
		
        [Column(TypeName = "decimal(10,2)")]
        public decimal? despesas { get; private set; }
		
        [Column(TypeName = "decimal(10,2)")]
        public decimal? seguro_total_item { get; private set; }
		
        [Column(TypeName = "decimal(10,2)")]
        public decimal? acrescimo_total_item { get; private set; }
		
        [Column(TypeName = "decimal(10,2)")]
        public decimal? despesas_total_item { get; private set; }
		
        [Column(TypeName = "bit")]
        public bool? forma_pix { get; private set; }
		
        [Column(TypeName = "decimal(10,2)")]
        public decimal? total_pix { get; private set; }
		
        [Column(TypeName = "bit")]
        public bool? forma_deposito_bancario { get; private set; }
		
        [Column(TypeName = "decimal(10,2)")]
        public decimal? total_deposito_bancario { get; private set; }

        [Column(TypeName = "int")]
        public Int32? id_venda_produto_b2c { get; private set; }
		
        [Column(TypeName = "bit")]
        public bool? item_promocional { get; private set; }

        [Column(TypeName = "bit")]
        public bool? acrescimo_item { get; private set; }
		
        [Column(TypeName = "decimal(10,2)")]
        public decimal? icms_st_antecipado_aliquota { get; private set; }
		
        [Column(TypeName = "decimal(10,2)")]
        public decimal? icms_st_antecipado_margem { get; private set; }
		
        [Column(TypeName = "decimal(10,2)")]
        public decimal? icms_st_antecipado_percentual_reducao { get; private set; }
		
        [Column(TypeName = "decimal(10,2)")]
        public decimal? icms_st_antecipado_valor_item { get; private set; }
		
        [Column(TypeName = "decimal(10,2)")]
        public decimal? icms_base_desonerado_item { get; private set; }
		
        [Column(TypeName = "varchar(5)")]
        [LengthValidation(length: 5, propertyName: "codigo_status_nfe")]
        public string? codigo_status_nfe { get; private set; }

        public LinxMovimento() { }

        public LinxMovimento(
            List<ValidationResult> listValidations,
            string? portal,
            string? cnpj_emp,
            string? transacao,
            string? usuario,
            string? documento,
            string? chave_nf,
            string? ecf,
            string? numero_serie_ecf,
            string? modelo_nf,
            string? data_documento,
            string? data_lancamento,
            string? codigo_cliente,
            string? serie,
            string? desc_cfop,
            string? id_cfop,
            string? cod_vendedor,
            string? quantidade,
            string? preco_custo,
            string? valor_liquido,
            string? desconto,
            string? cst_icms,
            string? cst_pis,
            string? cst_cofins,
            string? cst_ipi,
            string? valor_icms,
            string? aliquota_icms,
            string? base_icms,
            string? valor_pis,
            string? aliquota_pis,
            string? base_pis,
            string? valor_cofins,
            string? aliquota_cofins,
            string? base_cofins,
            string? valor_icms_st,
            string? aliquota_icms_st,
            string? base_icms_st,
            string? valor_ipi,
            string? aliquota_ipi,
            string? base_ipi,
            string? valor_total,
            string? forma_dinheiro,
            string? total_dinheiro,
            string? forma_cheque,
            string? total_cheque,
            string? forma_cartao,
            string? total_cartao,
            string? total_crediario,
            string? forma_crediario,
            string? forma_convenio,
            string? total_convenio,
            string? frete,
            string? operacao,
            string? tipo_transacao,
            string? cod_produto,
            string? cod_barra,
            string? cancelado,
            string? excluido,
            string? soma_relatorio,
            string? identificador,
            string? deposito,
            string? obs,
            string? preco_unitario,
            string? hora_lancamento,
            string? natureza_operacao,
            string? tabela_preco,
            string? nome_tabela_preco,
            string? cod_sefaz_situacao,
            string? desc_sefaz_situacao,
            string? protocolo_aut_nfe,
            string? dt_update,
            string? forma_cheque_prazo,
            string? total_cheque_prazo,
            string? cod_natureza_operacao,
            string? preco_tabela_epoca,
            string? desconto_total_item,
            string? conferido,
            string? transacao_pedido_venda,
            string? codigo_modelo_nf,
            string? acrescimo,
            string? mob_checkout,
            string? aliquota_iss,
            string? base_iss,
            string? ordem,
            string? codigo_rotina_origem,
            string? timestamp,
            string? troco,
            string? transportador,
            string? icms_aliquota_desonerado,
            string? icms_valor_desonerado_item,
            string? empresa,
            string? desconto_item,
            string? aliq_iss,
            string? iss_base_item,
            string? despesas,
            string? seguro_total_item,
            string? acrescimo_total_item,
            string? despesas_total_item,
            string? forma_pix,
            string? total_pix,
            string? forma_deposito_bancario,
            string? total_deposito_bancario,
            string? id_venda_produto_b2c,
            string? item_promocional,
            string? acrescimo_item,
            string? icms_st_antecipado_aliquota,
            string? icms_st_antecipado_margem,
            string? icms_st_antecipado_percentual_reducao,
            string? icms_st_antecipado_valor_item,
            string? icms_base_desonerado_item,
            string? codigo_status_nfe
		)
        {
            lastupdateon = DateTime.Now;

            this.portal =
                ConvertToInt32Validation.IsValid(portal, "portal", listValidations) ?
                Convert.ToInt32(portal) :
                0;

            this.transacao =
                ConvertToInt32Validation.IsValid(transacao, "transacao", listValidations) ?
                Convert.ToInt32(transacao) :
                0;

            this.usuario =
                ConvertToInt32Validation.IsValid(usuario, "usuario", listValidations) ?
                Convert.ToInt32(usuario) :
                0;

            this.documento =
                ConvertToInt32Validation.IsValid(documento, "documento", listValidations) ?
                Convert.ToInt32(documento) :
                0;

            this.ecf =
                ConvertToInt32Validation.IsValid(ecf, "ecf", listValidations) ?
                Convert.ToInt32(ecf) :
                0;

            this.modelo_nf =
                ConvertToInt32Validation.IsValid(modelo_nf, "modelo_nf", listValidations) ?
                Convert.ToInt32(modelo_nf) :
                0;

            this.codigo_cliente =
                ConvertToInt32Validation.IsValid(codigo_cliente, "codigo_cliente", listValidations) ?
                Convert.ToInt32(codigo_cliente) :
                0;

            this.cod_vendedor =
                ConvertToInt32Validation.IsValid(cod_vendedor, "cod_vendedor", listValidations) ?
                Convert.ToInt32(cod_vendedor) :
                0;

            this.tabela_preco =
                ConvertToInt32Validation.IsValid(tabela_preco, "tabela_preco", listValidations) ?
                Convert.ToInt32(tabela_preco) :
                0;

            this.cod_sefaz_situacao =
                ConvertToInt32Validation.IsValid(cod_sefaz_situacao, "cod_sefaz_situacao", listValidations) ?
                Convert.ToInt32(cod_sefaz_situacao) :
                0;

            this.transacao_pedido_venda =
                ConvertToInt32Validation.IsValid(transacao_pedido_venda, "transacao_pedido_venda", listValidations) ?
                Convert.ToInt32(transacao_pedido_venda) :
                0;

            this.ordem =
                ConvertToInt32Validation.IsValid(ordem, "ordem", listValidations) ?
                Convert.ToInt32(ordem) :
                0;

            this.codigo_rotina_origem =
                ConvertToInt32Validation.IsValid(codigo_rotina_origem, "codigo_rotina_origem", listValidations) ?
                Convert.ToInt32(codigo_rotina_origem) :
                0;

            this.transportador =
                ConvertToInt32Validation.IsValid(transportador, "transportador", listValidations) ?
                Convert.ToInt32(transportador) :
                0;

            this.empresa =
                ConvertToInt32Validation.IsValid(empresa, "empresa", listValidations) ?
                Convert.ToInt32(empresa) :
                0;

            this.id_venda_produto_b2c =
                ConvertToInt32Validation.IsValid(id_venda_produto_b2c, "id_venda_produto_b2c", listValidations) ?
                Convert.ToInt32(id_venda_produto_b2c) :
                0;

            this.timestamp =
                ConvertToInt64Validation.IsValid(timestamp, "timestamp", listValidations) ?
                Convert.ToInt64(timestamp) :
                0;

            this.cod_produto =
                ConvertToInt64Validation.IsValid(cod_produto, "cod_produto", listValidations) ?
                Convert.ToInt64(cod_produto) :
                0;

            this.data_documento =
                ConvertToDateTimeValidation.IsValid(data_documento, "data_documento", listValidations) ?
                Convert.ToDateTime(data_documento) :
                new DateTime(1990, 01, 01, 00, 00, 00, new CultureInfo("en-US").Calendar);

            this.data_lancamento =
                ConvertToDateTimeValidation.IsValid(data_lancamento, "data_lancamento", listValidations) ?
                Convert.ToDateTime(data_lancamento) :
                new DateTime(1990, 01, 01, 00, 00, 00, new CultureInfo("en-US").Calendar);

            this.dt_update =
                ConvertToDateTimeValidation.IsValid(dt_update, "dt_update", listValidations) ?
                Convert.ToDateTime(dt_update) :
                new DateTime(1990, 01, 01, 00, 00, 00, new CultureInfo("en-US").Calendar);

            this.quantidade =
                ConvertToDecimalValidation.IsValid(quantidade, "quantidade", listValidations) ?
                Convert.ToDecimal(quantidade) :
                0;

            this.preco_custo =
                ConvertToDecimalValidation.IsValid(preco_custo, "preco_custo", listValidations) ?
                Convert.ToDecimal(preco_custo) :
                0;

            this.valor_liquido =
                ConvertToDecimalValidation.IsValid(valor_liquido, "valor_liquido", listValidations) ?
                Convert.ToDecimal(valor_liquido) :
                0;

            this.desconto =
                ConvertToDecimalValidation.IsValid(desconto, "desconto", listValidations) ?
                Convert.ToDecimal(desconto) :
                0;

            this.valor_icms =
                ConvertToDecimalValidation.IsValid(valor_icms, "valor_icms", listValidations) ?
                Convert.ToDecimal(valor_icms) :
                0;

            this.aliquota_icms =
                ConvertToDecimalValidation.IsValid(aliquota_icms, "aliquota_icms", listValidations) ?
                Convert.ToDecimal(aliquota_icms) :
                0;

            this.base_icms =
                ConvertToDecimalValidation.IsValid(base_icms, "base_icms", listValidations) ?
                Convert.ToDecimal(base_icms) :
                0;

            this.valor_pis =
                ConvertToDecimalValidation.IsValid(valor_pis, "valor_pis", listValidations) ?
                Convert.ToDecimal(valor_pis) :
                0;

            this.aliquota_pis =
                ConvertToDecimalValidation.IsValid(aliquota_pis, "aliquota_pis", listValidations) ?
                Convert.ToDecimal(aliquota_pis) :
                0;

            this.base_pis =
                ConvertToDecimalValidation.IsValid(base_pis, "base_pis", listValidations) ?
                Convert.ToDecimal(base_pis) :
                0;

            this.valor_cofins =
                ConvertToDecimalValidation.IsValid(valor_cofins, "valor_cofins", listValidations) ?
                Convert.ToDecimal(valor_cofins) :
                0;

            this.aliquota_cofins =
                ConvertToDecimalValidation.IsValid(aliquota_cofins, "aliquota_cofins", listValidations) ?
                Convert.ToDecimal(aliquota_cofins) :
                0;

            this.base_cofins =
                ConvertToDecimalValidation.IsValid(base_cofins, "base_cofins", listValidations) ?
                Convert.ToDecimal(base_cofins) :
                0;

            this.valor_icms_st =
                ConvertToDecimalValidation.IsValid(valor_icms_st, "valor_icms_st", listValidations) ?
                Convert.ToDecimal(valor_icms_st) :
                0;

            this.base_icms_st =
                ConvertToDecimalValidation.IsValid(base_icms_st, "base_icms_st", listValidations) ?
                Convert.ToDecimal(base_icms_st) :
                0;

            this.aliquota_icms_st =
                ConvertToDecimalValidation.IsValid(aliquota_icms_st, "aliquota_icms_st", listValidations) ?
                Convert.ToDecimal(aliquota_icms_st) :
                0;

            this.valor_ipi =
                ConvertToDecimalValidation.IsValid(valor_ipi, "valor_ipi", listValidations) ?
                Convert.ToDecimal(valor_ipi) :
                0;

            this.aliquota_ipi =
                ConvertToDecimalValidation.IsValid(aliquota_ipi, "aliquota_ipi", listValidations) ?
                Convert.ToDecimal(aliquota_ipi) :
                0;

            this.base_ipi =
                ConvertToDecimalValidation.IsValid(base_ipi, "base_ipi", listValidations) ?
                Convert.ToDecimal(base_ipi) :
                0;

            this.valor_total =
                ConvertToDecimalValidation.IsValid(valor_total, "valor_total", listValidations) ?
                Convert.ToDecimal(valor_total) :
                0;

            this.total_dinheiro =
                ConvertToDecimalValidation.IsValid(total_dinheiro, "total_dinheiro", listValidations) ?
                Convert.ToDecimal(total_dinheiro) :
                0;

            this.total_cheque =
                ConvertToDecimalValidation.IsValid(total_cheque, "total_cheque", listValidations) ?
                Convert.ToDecimal(total_cheque) :
                0;

            this.total_cartao =
                ConvertToDecimalValidation.IsValid(total_cartao, "total_cartao", listValidations) ?
                Convert.ToDecimal(total_cartao) :
                0;

            this.total_crediario =
                ConvertToDecimalValidation.IsValid(total_crediario, "total_crediario", listValidations) ?
                Convert.ToDecimal(total_crediario) :
                0;

            this.total_convenio =
                ConvertToDecimalValidation.IsValid(total_convenio, "total_convenio", listValidations) ?
                Convert.ToDecimal(total_convenio) :
                0;

            this.frete =
                ConvertToDecimalValidation.IsValid(frete, "frete", listValidations) ?
                Convert.ToDecimal(frete) :
                0;

            this.preco_unitario =
                ConvertToDecimalValidation.IsValid(preco_unitario, "preco_unitario", listValidations) ?
                Convert.ToDecimal(preco_unitario) :
                0;

            this.total_cheque_prazo =
                ConvertToDecimalValidation.IsValid(total_cheque_prazo, "total_cheque_prazo", listValidations) ?
                Convert.ToDecimal(total_cheque_prazo) :
                0;

            this.preco_tabela_epoca =
                ConvertToDecimalValidation.IsValid(preco_tabela_epoca, "preco_tabela_epoca", listValidations) ?
                Convert.ToDecimal(preco_tabela_epoca) :
                0;

            this.desconto_total_item =
                ConvertToDecimalValidation.IsValid(desconto_total_item, "desconto_total_item", listValidations) ?
                Convert.ToDecimal(desconto_total_item) :
                0;

            this.acrescimo =
                ConvertToDecimalValidation.IsValid(acrescimo, "acrescimo", listValidations) ?
                Convert.ToDecimal(acrescimo) :
                0;

            this.aliquota_iss =
                ConvertToDecimalValidation.IsValid(aliquota_iss, "aliquota_iss", listValidations) ?
                Convert.ToDecimal(aliquota_iss) :
                0;

            this.base_iss =
                ConvertToDecimalValidation.IsValid(base_iss, "base_iss", listValidations) ?
                Convert.ToDecimal(base_iss) :
                0;

            this.troco =
                ConvertToDecimalValidation.IsValid(troco, "troco", listValidations) ?
                Convert.ToDecimal(troco) :
                0;

            this.icms_aliquota_desonerado =
                ConvertToDecimalValidation.IsValid(icms_aliquota_desonerado, "icms_aliquota_desonerado", listValidations) ?
                Convert.ToDecimal(icms_aliquota_desonerado) :
                0;

            this.icms_valor_desonerado_item =
                ConvertToDecimalValidation.IsValid(icms_valor_desonerado_item, "icms_valor_desonerado_item", listValidations) ?
                Convert.ToDecimal(icms_valor_desonerado_item) :
                0;

            this.desconto_item =
                ConvertToDecimalValidation.IsValid(desconto_item, "desconto_item", listValidations) ?
                Convert.ToDecimal(desconto_item) :
                0;

            this.aliq_iss =
                ConvertToDecimalValidation.IsValid(aliq_iss, "aliq_iss", listValidations) ?
                Convert.ToDecimal(aliq_iss) :
                0;

            this.iss_base_item =
                ConvertToDecimalValidation.IsValid(iss_base_item, "iss_base_item", listValidations) ?
                Convert.ToDecimal(iss_base_item) :
                0;

            this.despesas =
                ConvertToDecimalValidation.IsValid(despesas, "despesas", listValidations) ?
                Convert.ToDecimal(despesas) :
                0;

            this.seguro_total_item =
                ConvertToDecimalValidation.IsValid(seguro_total_item, "seguro_total_item", listValidations) ?
                Convert.ToDecimal(seguro_total_item) :
                0;

            this.acrescimo_total_item =
                ConvertToDecimalValidation.IsValid(acrescimo_total_item, "acrescimo_total_item", listValidations) ?
                Convert.ToDecimal(acrescimo_total_item) :
                0;

            this.despesas_total_item =
                ConvertToDecimalValidation.IsValid(despesas_total_item, "despesas_total_item", listValidations) ?
                Convert.ToDecimal(despesas_total_item) :
                0;

            this.total_pix =
                ConvertToDecimalValidation.IsValid(total_pix, "total_pix", listValidations) ?
                Convert.ToDecimal(total_pix) :
                0;

            this.total_deposito_bancario =
                ConvertToDecimalValidation.IsValid(total_deposito_bancario, "total_deposito_bancario", listValidations) ?
                Convert.ToDecimal(total_deposito_bancario) :
                0;

            this.icms_st_antecipado_aliquota =
                ConvertToDecimalValidation.IsValid(icms_st_antecipado_aliquota, "icms_st_antecipado_aliquota", listValidations) ?
                Convert.ToDecimal(icms_st_antecipado_aliquota) :
                0;

            this.icms_st_antecipado_margem =
                ConvertToDecimalValidation.IsValid(icms_st_antecipado_margem, "icms_st_antecipado_margem", listValidations) ?
                Convert.ToDecimal(icms_st_antecipado_margem) :
                0;

            this.icms_st_antecipado_percentual_reducao =
                ConvertToDecimalValidation.IsValid(icms_st_antecipado_percentual_reducao, "icms_st_antecipado_percentual_reducao", listValidations) ?
                Convert.ToDecimal(icms_st_antecipado_percentual_reducao) :
                0;

            this.icms_st_antecipado_valor_item =
                ConvertToDecimalValidation.IsValid(icms_st_antecipado_valor_item, "icms_st_antecipado_valor_item", listValidations) ?
                Convert.ToDecimal(icms_st_antecipado_valor_item) :
                0;

            this.icms_base_desonerado_item =
                ConvertToDecimalValidation.IsValid(icms_base_desonerado_item, "icms_base_desonerado_item", listValidations) ?
                Convert.ToDecimal(icms_base_desonerado_item) :
                0;

            this.forma_dinheiro =
                ConvertToBooleanValidation.IsValid(forma_dinheiro, "forma_dinheiro", listValidations) ?
                Convert.ToBoolean(forma_dinheiro) :
                false;

            this.forma_cheque =
                ConvertToBooleanValidation.IsValid(forma_cheque, "forma_cheque", listValidations) ?
                Convert.ToBoolean(forma_cheque) :
                false;

            this.forma_cartao =
                ConvertToBooleanValidation.IsValid(forma_cartao, "forma_cartao", listValidations) ?
                Convert.ToBoolean(forma_cartao) :
                false;

            this.forma_crediario =
                ConvertToBooleanValidation.IsValid(forma_crediario, "forma_crediario", listValidations) ?
                Convert.ToBoolean(forma_crediario) :
                false;

            this.forma_convenio =
                ConvertToBooleanValidation.IsValid(forma_convenio, "forma_convenio", listValidations) ?
                Convert.ToBoolean(forma_convenio) :
                false;

            this.forma_cheque_prazo =
                ConvertToBooleanValidation.IsValid(forma_cheque_prazo, "forma_cheque_prazo", listValidations) ?
                Convert.ToBoolean(forma_cheque_prazo) :
                false;

            this.mob_checkout =
                ConvertToBooleanValidation.IsValid(mob_checkout, "mob_checkout", listValidations) ?
                Convert.ToBoolean(mob_checkout) :
                false;

            this.forma_pix =
                ConvertToBooleanValidation.IsValid(forma_pix, "forma_pix", listValidations) ?
                Convert.ToBoolean(forma_pix) :
                false;

            this.forma_deposito_bancario =
                ConvertToBooleanValidation.IsValid(forma_deposito_bancario, "forma_deposito_bancario", listValidations) ?
                Convert.ToBoolean(forma_deposito_bancario) :
                false;

            this.item_promocional =
                ConvertToBooleanValidation.IsValid(item_promocional, "item_promocional", listValidations) ?
                Convert.ToBoolean(item_promocional) :
                false;

            this.acrescimo_item =
                ConvertToBooleanValidation.IsValid(acrescimo_item, "acrescimo_item", listValidations) ?
                Convert.ToBoolean(acrescimo_item) :
                false;

            this.identificador =
                String.IsNullOrEmpty(identificador) ? null
                : Guid.Parse(identificador);

            this.cnpj_emp = cnpj_emp;
            this.chave_nf = chave_nf;
            this.numero_serie_ecf = numero_serie_ecf;
            this.serie = serie;
            this.desc_cfop = desc_cfop;
            this.id_cfop = id_cfop;
            this.cst_icms = cst_icms;
            this.cst_pis = cst_pis;
            this.cst_cofins = cst_cofins;
            this.cst_ipi = cst_ipi;
            this.operacao = operacao;
            this.cod_barra = cod_barra;
            this.tipo_transacao = tipo_transacao;
            this.cancelado = cancelado;
            this.excluido = excluido;
            this.soma_relatorio = soma_relatorio;
            this.deposito = deposito;
            this.obs = obs;
            this.hora_lancamento = hora_lancamento;
            this.natureza_operacao = natureza_operacao;
            this.nome_tabela_preco = nome_tabela_preco;
            this.desc_sefaz_situacao = desc_sefaz_situacao;
            this.protocolo_aut_nfe = protocolo_aut_nfe;
            this.cod_natureza_operacao = cod_natureza_operacao;
            this.conferido = conferido;
            this.codigo_modelo_nf = codigo_modelo_nf;
            this.codigo_status_nfe = codigo_status_nfe;
        }
    }
}
