function addAntiForgeryToken(data) {
    data.__RequestVerificationToken = $('[name=__RequestVerificationToken]').val();
    return data;
};

function carregaItens(url, param) {
    $.post(url, addAntiForgeryToken(param), function (response) {
        if (response) {

            var table = $('#grid-cadastro').find('tbody');

            table.empty();

            for (var i = 0; i < response.Lista.length; i++) {
                table.append(criarLinhaGrid(response.Lista[i]));
            }
        }
    });
};

function formatarMensagem(mensagens) {
    var ret = '';
    for (var i = 0; i < mensagens.length; i++) {
        ret += '<li>' + mensagens[i] + '</li>';
    }
    return '<ul>' + ret + '</ul>';
};

function getId(btn) {
    var id = btn.closest('tr').attr('data-id');
    return id;
};

$(document).on('click', '#btn-incluir', function () {
    window.location = urlCadastro;
})
.on('click', '.btn-alterar', function () {

    var id = getId($(this));
    window.location = urlCadastro + '/' + id;

 })
.on('click', '.btn-excluir', function () {

    var id = getId($(this)),
        tr = $(this).closest('tr');

    bootbox.confirm({
        message: "Deseja realmente excluir o " + tituloPage + "?",
        buttons: {
            confirm: {
                label: 'Sim',
                className: 'btn-success'
            },
            cancel: {
                label: 'Não',
                className: 'btn-primary'
            }
        },
        callback: function (result) {
            if (result) {

                url = urlExcluir;
                param = { 'id': id };

                $.post(url, addAntiForgeryToken(param), function (response) {
                    if (response) {
                        tr.remove();
                    } else {
                        bootbox.alert({
                            size: "small",
                            title: "Não foi possível remover o item",
                            message: "Não foi possível remover o item, recarrege a página.",
                            callback: function () { }
                        })
                    }
                })
            }
        }
    });
}).on('click', '#btn-salvar', function () {
    url = urlSalvar;
    param = getDadosForm();
   
    $.post(url, addAntiForgeryToken(param), function (response) {

        if (response.Resultado == "OK") {
            window.location.href = urlBase;
        } else if (response.Resultado == "ERRO") {
            $('#mensagem-erro').show();
        } else {
            $('#mensagem-aviso1').empty();
            $('#mensagem-aviso1').append('<b>Atenção!</b><br/><br/>' + formatarMensagem(response.Mensagens));
            $('#mensagem-aviso1').show();
            $('#mensagem-erro').hide();
        }
    })

})
.on('click', '.page-item', function () {
    var btn = $(this),
        page = btn.text(),
        quantItens = $('#tam-pagina').val(),
        url = urlGrupoPagina,
        param = { pagina: page, quantItens: quantItens };

    carregaItens(url, param);

    btn.siblings().removeClass('active');

    btn.addClass('active');

})
.ready(function () {
    $('#modal-cadastro').hide();
});

function getNewVal(item) {
    var tamPagina = item.value,
        param = { quant: tamPagina },
        url = urlMudaQuantItens;

        carregaItens(url, param);
}