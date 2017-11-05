function setDadosForm(dados) {
    $('#txt-nome').val(dados.Nome);
    $('#txt-login').val(dados.Login);
    $('#id-cadastro').val(dados.Id);
}

function setFocusForm() {
    $('#txt-nome').focus();
}

function dadosForm(dados) {
    return '<td>' + dados.Nome + '</td>' +
           '<td>' + dados.DataCadastro + '</td>' +
           '<td>' + dados.Login + '</td>';
}

function getDadosInclusao() {
    return { Id: 0, Nome: '', Ativo: true };
}

function getDadosForm() {
    return {
        Id: $('#id-cadastro').val(),
        Nome: $('#txt-nome').val(),
        Login: $('#txt-login').val()
    }
}

function setLinhaParam(linha, param) {
    linha.eq(0).html(param.Nome).end()
        .eq(1).html(param.DataCadastro)
        .eq(2).html(param.Login);
}

