$(document).ready(function () {
    $(".cnpj").mask('00.000.000/0000-00', { reverse: true });
    $(".data").mask('00/00/0000');
    $("#txt-cnpj").focus();
})

$(document).on('blur', '.cnpj', function () {
    if ($(this).val().length < 18) {
        setMensagem('1', '<b> Atencao!</b><br/><br/> Preencha o CNPJ!', $(this));
    } else {
        if (!validarCNPJ($(this).val())) {
            setMensagem('1', '<b>Atencao!</b><br/><br/> CNPJ Invalido!', $(this));
        } else {
            $('#mensagem-aviso1').hide();
        }       
    }
})

$(document).on('blur', '.email', function () {
    if (!validateEmail($(this).val())) {
        $(this).val('');
    }
})

function validateEmail($email) {
    var emailReg = /^([\w-\.]+@([\w-]+\.)+[\w-]{2,4})?$/;
    return emailReg.test($email);
}

$(document).on('blur', '.data', function () {
    validaData($(this));
});

$('.telefone').each(function (i, el) {
    $('#' + el.id).mask("(00)00000-0000");
})

function aplicaMascara(event) {
    var $element = $('#' + this.id);
    $(this).off('blur');
    $element.unmask();
    if (this.value.replace(/\D/g, '').length > 10) {
        $element.mask("(00)00000-0000");
    } else {
        $element.mask("(00)0000-00009");
    }
    $(this).on('blur', updateMask);
}

$('.telefone').on('blur', aplicaMascara);

function validaData(data) {
    if (!isDate(data.val())) {
        data.val('');
    } 
}

function setMensagem(id, mensagem, elemento) {
    idMensagem = '#mensagem-aviso'.concat(id);
    $(idMensagem).empty();
    $(idMensagem).append(mensagem);
    $(idMensagem).show();
    elemento.val('');
    elemento.focus();
}

function isDate(txtDate)
{
    var currVal = txtDate;
    if (currVal == '')
        return false;

    var rxDatePattern = /^(\d{1,2})(\/|-)(\d{1,2})(\/|-)(\d{4})$/;

    var dtArray = currVal.match(rxDatePattern);

    if (dtArray == null)
       return false;
    dtMonth = dtArray[3];
    dtDay = dtArray[1];
    dtYear = dtArray[5];

    if (dtMonth < 1 || dtMonth > 12)
       return false;
  else if (dtDay < 1 || dtDay > 31)
    return false;
  else if ((dtMonth == 4 || dtMonth == 6 || dtMonth == 9 || dtMonth == 11) && dtDay == 31)
      return false;
  else if (dtMonth == 2)
    {
        var isleap = (dtYear % 4 == 0 && (dtYear % 100 != 0 || dtYear % 400 == 0));
        if (dtDay > 29 || (dtDay == 29 && !isleap))
        return false;
    }
    return true;
}


function validarCNPJ(cnpj) {

    cnpj = cnpj.replace(/[^\d]+/g, '');

    if (cnpj == '') return false;

    if (cnpj.length != 14)
        return false;

    // Elimina CNPJs invalidos conhecidos
    if (cnpj == "00000000000000" ||
        cnpj == "11111111111111" ||
        cnpj == "22222222222222" ||
        cnpj == "33333333333333" ||
        cnpj == "44444444444444" ||
        cnpj == "55555555555555" ||
        cnpj == "66666666666666" ||
        cnpj == "77777777777777" ||
        cnpj == "88888888888888" ||
        cnpj == "99999999999999")
        return false;

    // Valida DVs
    tamanho = cnpj.length - 2
    numeros = cnpj.substring(0, tamanho);
    digitos = cnpj.substring(tamanho);
    soma = 0;
    pos = tamanho - 7;
    for (i = tamanho; i >= 1; i--) {
        soma += numeros.charAt(tamanho - i) * pos--;
        if (pos < 2)
            pos = 9;
    }
    resultado = soma % 11 < 2 ? 0 : 11 - soma % 11;
    if (resultado != digitos.charAt(0))
        return false;

    tamanho = tamanho + 1;
    numeros = cnpj.substring(0, tamanho);
    soma = 0;
    pos = tamanho - 7;
    for (i = tamanho; i >= 1; i--) {
        soma += numeros.charAt(tamanho - i) * pos--;
        if (pos < 2)
            pos = 9;
    }
    resultado = soma % 11 < 2 ? 0 : 11 - soma % 11;
    if (resultado != digitos.charAt(1))
        return false;

    return true;

}
