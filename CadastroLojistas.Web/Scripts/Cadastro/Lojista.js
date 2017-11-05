
function dadosForm(dados) {
    return '<td>' + dados.CNPJ + '</td>' +
           '<td>' + dados.Fantasia + '</td>';
}

function getDadosInclusao() {
    return { Id: 0, Sigla: '', Nome: '', Ativo: true };
}

function setLinhaParam(linha, param) {
    linha.eq(0).html(param.Nome).end()
       .eq(1).html(param.Ativo ? 'Sim' : 'Não');
}

