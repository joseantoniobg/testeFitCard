function getDadosForm() {

    var dataCad;

    if ($('#txt-data-cadastro').val() != '') {
        var from = $("#txt-data-cadastro").val().split("/");
        dataCad = new Date(from[2], from[1] - 1, from[0]).toISOString();
    }

    return {
        IdLojista: $('#txt-idlojista').val(),
        CNPJ: $('#txt-cnpj').val(),
        Razao: $('#txt-razao').val(),
        Fantasia: $('#txt-fantasia').val(),
        Email: $('#txt-email').val(),
        Telefone: $('#txt-telefone').val(),
        Celular: $('#txt-celular').val(),
        Endereco: $('#txt-endereco').val(),
        Numero: $('#txt-numero').val(),
        Complemento: $('#txt-complemento').val(),
        Bairro: $('#txt-bairro').val(),
        Cidade: { IdCidade: $('#cb-cidade').val(), Cidade: '', Estado: '' },
        Status: $('#opt-ativo').is(':checked'),
        DataCadastro: dataCad,
        Categoria: { IdCategoria: $('#cb-categoria').val(), Categoria: '' }
    }
}

$(document).on('blur', '#txt-razao', function () {
    if ($(this).val() == '') {
        setMensagem('1', '<b> Atencao!</b><br/><br/> Preencha a Razao Social!', $(this));
        $(this).focus();
    } else {
        $('#mensagem-aviso1').hide();
    }
})
