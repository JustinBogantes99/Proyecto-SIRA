var CODIGO_EXITO = 0
var CODIGO_SESSION_EXPIRA = -99;
var CODIGO_NULOS = -98;
var ErrorGeneral = 'Ha ocurrido un problema, inténtalo de nuevo.'
var usuarioConsultarGlob = '';
var codUsuarioConsultarGlob = '';


$(document).ready(function () {
    var lParticipantes = [];
    $.LoadingOverlaySetup({ image: rootDirImage + "/Content/img/anim_tec.gif", maxSize: "128px", minSize: "75px", resizeInterval: 0, size: "100%" });

    //Llamado en carga inicial
    //Tomar la fecha de hoy del cliente
    var now = new Date();
    var day = ("0" + now.getDate()).slice(-2);
    var month = ("0" + (now.getMonth() + 1)).slice(-2);
    var monthAfter = ("0" + (now.getMonth() + 2)).slice(-2);
    var today = now.getFullYear() + "-" + (month) + "-" + (day);

    Consultar();



    

   

});


function Consultar() {

    var idConfiguracion = "1";

    $("#tablaResultadosBusqueda > tbody").empty();
    $.LoadingOverlay("show");
    $.ajax({
        type: 'POST',
        dataType: 'json',
        url: rootDirImage + 'Home/Consultar',
        data: { 'idConfiguracion': idConfiguracion},
        success: function (data) {
            if (data == CODIGO_SESSION_EXPIRA) {
                location.href = $("#dirAppIniLogin").val();
            }
            else if (data == CODIGO_SESSION_EXPIRA) {
                $('#popFallo').modal('show');
                $('#popFallo .popMensaje').html(MensajesDatosNulos);
            }
            else if (data == null) {
                $('#popFallo').modal('show');
                $('#popFallo .popMensaje').html(ErrorGeneral);
            }
            else if (data.respuesta.CodigoRespuesta != CODIGO_EXITO) {

                if (data.respuesta.MensajeRespuesta.length <= 0) {
                    $('#popFallo').modal('show');
                    $('#popFallo .popMensaje').html(ErrorGeneral);
                }
                else {
                    $('#popFallo').modal('show');
                    $('#popFallo .popMensaje').html(data.respuesta.MensajeRespuesta);
                }
            }
            else {

                for (var i = 0; i < data.respuesta.ObjetoRespuesta.length; i++) {
                    var obj = data.respuesta.ObjetoRespuesta[i];

                    var row =
                        "<tr>" +
                        "<td class='oculto tiId'>" + obj.Idconfiguracion + "</td>" +
                        "<td class='tiTipoConfig'>" + obj.TipoConfiguracion + "</td>" +
                        "<td class='tiNombre'>" + obj.Nombre + "</td>" +
                        "<td class='tiValor'>" + obj.Valor + "</td>" +
                        "</tr>";
                    $("#listado").find("table tbody").append(row);

                }

            }
            $.LoadingOverlay("hide");
        },
        error: function (XMLHttpRequest, textStatus, errorThrown) {
            $('#popFallo').modal('show');
            $('#popFallo .popMensaje').html(ErrorGeneral);
            $.LoadingOverlay("hide");
        }
    });
}




//---------------------------CONSULTAR UN FUNCIONARIO-------------------------------------------//
$("#btnAdminBuscarFuncionario").click(function () {
    var usuarioTec = $('#txtUsuarioTecConsulta').val();

    if (usuarioTec.length <= 0) {
        $("#tablaResultadosBusquedaAdminConsulta > tbody").empty();
        return false;
    }

    if (usuarioTec.length <= 3 && usuarioTec.length > 0) {
        $('#popFallo').modal('show');
        $('#popFallo .popMensaje').html('Debes indicar mínimo 4 caracteres para poder buscar.');
        return false;
    }

    $("#tablaResultadosBusquedaAdminConsulta > tbody").empty();
    $.LoadingOverlay("show");
    $.ajax({
        type: 'POST',
        dataType: 'json',
        url: rootDirImage + 'Home/ListaUsuarios',
        data: { "correoElectronico": usuarioTec },
        success: function (data) {
            if (data == CODIGO_SESSION_EXPIRA) {
                location.href = $("#dirAppIniLogin").val();
            }
            else if (data == CODIGO_SESSION_EXPIRA) {
                $('#popFallo').modal('show');
                $('#popFallo .popMensaje').html(MensajesDatosNulos);
            }
            else if (data.CodigoRespuesta != CODIGO_EXITO) {
                $('#popFallo').modal('show');
                $('#popFallo .popMensaje').html(ErrorGeneral);
            }

            for (var i = 0; i < data.listado.length; i++) {
                var obj = data.listado[i];
                var rowUsuarios =
                    "<tr>" +
                    "<td class='oculto tiCodUsuario'>" + obj.IdUsuario + "</td>" +
                    "<td class='tiNombre'>" + obj.Nombre + "</td>" +
                    "<td class='tiApellidos'>" + obj.Apellido1 + " " + obj.Apellido2 + "</td>" +
                    "<td class='tiCorreo'>" + obj.CorreoPrincipal + "</td>" +
                    "<td><input type='radio' class='chkUsarUsuarioImpersonal form-check-input' title='Escoger Usuario' name='usugen' id='chkUsarUsuarioImpersonal'/></td>" + " " +
                    "</tr>";
                $("#ListadoBusquedaAdminConsulta").find("table tbody").append(rowUsuarios);
            }
            $.LoadingOverlay("hide");
        },
        error: function (XMLHttpRequest, textStatus, errorThrown) {
            $('#popFallo').modal('show');
            $('#popFallo .popMensaje').html(ErrorGeneral);
            $.LoadingOverlay("hide");
        }
    });
});


