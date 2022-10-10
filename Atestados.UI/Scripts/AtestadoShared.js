// Este archivo contiene todos los métodos JS y AJAX para agregar y eliminar
// autores y archivos cuando se está realizando CRUD a un atestado.

var archCont = 0;

// Declarar las variables utilizadas por el modal de autores solo si es necesario.
if (hasAutores) {
    var autorCont = 0;
    var per = 100;
    var modal = document.getElementById('modalAutores');
    var checkbox = document.getElementById('AutoresEq');
    var hiddenCheck = document.getElementById('hiddenCheck');
    var autoresCheck = document.getElementById('AutoresCheck');

    /*
     * Si el checkbox de porcentaje equitativo se activa, este valor no se le pasa
     * al controlador correctamente por estar dentro del modal. Por lo tanto, se
     * agregó un checkbox adicional que está escondido en medio del formulario. Si
     * el checkbox de autores equitativos se activa, entonces el checkbox escondido
     * también. De esta forma el controlador recibe el valor del checkbox escondido
     * y sí funciona la validación del formulario.
     */
    hiddenCheck.onchange = function () {
        document.getElementById('porcentaje').disabled = this.checked;
        document.getElementById('porcentaje_funcionario').disabled = this.checked;
        checkbox.checked = this.checked;
    };

    // Si se está editando un atestado, suponer que cumple con el check de autores.
    if (editMode) {
        autoresCheck.checked = true;
    }
}


function isEmail(e) {
    var filter = /^\s*[\w\-\+_]+(\.[\w\-\+_]+)*\@[\w\-\+_]+\.[\w\-\+_]+(\.[\w\-\+_]+)*\s*$/;
    return String(e).search(filter) != -1;
}

function validPercentage(percentage) {
    return (per - percentage) >= 0 && percentage >= 0;
}
//Agregar Criterios
//$('#criterioAgregar').click(function () { });
// Agregar funcionarios como autores al libro.
$('#funcionarioAgregar').click(function () {
    var usuario = new Object();
    var funcionario = null;

    usuario.Email = $('#email_funcionario').val();

    $.ajax({
        type: 'POST',
        url: `${baseUrl}/checkFuncionario`,
        async: false,
        contentType: 'application/json; charset=utf-8',
        dataType: 'json',
        data: JSON.stringify(usuario),
        success: function (response) {
            funcionario = JSON.parse(response.usuario)
        }
    });

    if (!funcionario) {
        alert("Funcionario no encontrado");
        return;
    }

    // Validar si el porcentaje ingresado es correcto.
    if (!checkbox.checked && $('#porcentaje_funcionario').val() == "") {
        alert("Debe ingresar un porcentaje");
        return;
    }
    if (!checkbox.checked && !validPercentage(parseInt($('#porcentaje_funcionario').val()))) {
        alert("Se ha ingresado un porcentaje inválido");
        return;
    }

    // Crear el objeto del autor con la información ingresada.
    var autor = new Object();
    autor.Email = usuario.Email;
    autor.esFuncionario = true;
    autor.porcEquitativo = false;
    autor.Porcentaje = $('#porcentaje_funcionario').val();
    autor.numAutor = ++autorCont;

    // Si está marcada la opción de porcentaje equitativo.
    if (autor != null && isEmail(autor.Email) && checkbox.checked) {
        autor.porcEquitativo = true;
        $.ajax({
            type: 'POST',
            url: `${baseUrl}/agregarFuncionario`,
            async: false,
            contentType: 'application/json; charset=utf-8',
            dataType: 'html',
            data: JSON.stringify(autor),
            success: function (result) {
                $("#autoresTabla").html(result);
            }
        });
        // Marcar el requerimiento de al menos un autor como cumplido.
        autoresCheck.checked = true;
        // No se pueden mezclar los autores de forma equitativa y no equitativa. 
        hiddenCheck.disabled = true;
    }
    // Si no está marcada la opción de porcentaje equitativo.
    else if (autor != null && isEmail(autor.Email)) {
        per = per - parseInt(autor.Porcentaje);
        $.ajax({
            type: 'POST',
            url: `${baseUrl}/agregarFuncionario`,
            async: false,
            contentType: 'application/json; charset=utf-8',
            dataType: 'html',
            data: JSON.stringify(autor),
            success: function (result) {
                $("#autoresTabla").html(result);
            }
        });
        // Marcar el requerimiento de al menos un autor como cumplido.
        autoresCheck.checked = true;
        // No se pueden mezclar los autores de forma equitativa y no equitativa. 
        hiddenCheck.disabled = true;
    }
    clearAuthorForm();
});

// Agregar un agente externo como autor.
$('#autorAgregar').click(function () {

    // Validar que el porcentaje ingresado es correcto.
    if (!checkbox.checked && $('#porcentaje').val() == "") {
        alert("Debe ingresar un porcentaje");
        return;
    }
    if (!checkbox.checked && !validPercentage(parseInt($('#porcentaje').val()))) {
        alert("Se ha ingresado un porcentaje inválido");
        return;
    }

    // Crear el objeto del autor con la información ingresada.
    var autor = new Object();
    autor.Nombre = $('#nombre').val();
    autor.PrimerApellido = $('#apellido1').val();
    autor.SegundoApellido = $('#apellido2').val();
    autor.Email = $('#email').val();
    autor.esFuncionario = false;
    autor.porcEquitativo = false;
    autor.Porcentaje = $('#porcentaje').val()
    autor.numAutor = ++autorCont;

    // Si está marcada la opción de porcentaje equitativo.
    if (autor != null && isEmail(autor.Email) && checkbox.checked) {
        autor.porcEquitativo = true;
        $.ajax({
            type: 'POST',
            url: `${baseUrl}/agregarAutor`,
            async: false,
            contentType: 'application/json; charset=utf-8',
            dataType: 'html',
            data: JSON.stringify(autor),
            success: function (result) {
                $("#autoresTabla").html(result);
            }
        });
        // Marcar el requerimiento de al menos un autor como cumplido.
        autoresCheck.checked = true;
        // No se pueden mezclar los autores de forma equitativa y no equitativa. 
        hiddenCheck.disabled = true;
    }
    // Si no está marcada la opción de porcentaje equitativo.
    else if (autor != null && isEmail(autor.Email)) {
        per = per - parseInt(autor.Porcentaje);
        $.ajax({
            type: 'POST',
            url: `${baseUrl}/agregarAutor`,
            async: false,
            contentType: 'application/json; charset=utf-8',
            dataType: 'html',
            data: JSON.stringify(autor),
            success: function (result) {
                $("#autoresTabla").html(result);
            }
        });
        // Marcar el requerimiento de al menos un autor como cumplido.
        autoresCheck.checked = true;
        // No se pueden mezclar los autores de forma equitativa y no equitativa. 
        hiddenCheck.disabled = true;
    }
    clearAuthorForm();
})

// Quitar texto de todos los campos del formulario de autores.
function clearAuthorForm() {
    $('#nombre').val('');
    $('#apellido1').val('');
    $('#apellido2').val('');
    $('#email').val('');
    $('#porcentaje').val('');
    $('#email_funcionario').val('');
    $('#porcentaje_funcionario').val('');
}

// Borrar autores de la tabla.
$('#autoresTabla').on('click', '.remove', function () {

    // Encontrar el número de autor que 
    var $row = $(this).closest("tr");  
    var $text = $row.find(".numAut").text();

    // Crear el objeto que se le pasa al controlador.
    var autor = new Object()
    autor.numAutor = $text;

    $.ajax({
        type: 'POST',
        url: `${baseUrl}/borrarAutor`,
        async: false,
        contentType: 'application/json; charset=utf-8',
        dataType: 'html',
        data: JSON.stringify(autor),
        success: function (result) {
            $("#autoresTabla").html(result);
        }
    });
    // Calcular el porcentaje restante una vez removido el autor.
    if (!checkbox.checked) {
        $.ajax({
            type: 'POST',
            url: `${baseUrl}/calcularPorcentajes`,
            async: false,
            contentType: 'application/json; charset=utf-8',
            dataType: 'json',
            success: function (response) {
                p = JSON.parse(response.p);
                per = p;
                if (per == 100) {
                    // Volver a poner el check de menos de un autor.
                    autoresCheck.checked = false;
                    // Volver a habilitar el check para autores equitativos.
                    hiddenCheck.disabled = false;

                }
            }
    });
    }
});

// Subir el archivo.
$('#subirArchivo').submit(function (e) {
    e.preventDefault();
    var data = new FormData(this);
    if (jQuery('#archivo').val().length != '') {
        // Primero se le manda el número de archivo al controlador.
        $.ajax({
            type: 'GET',
            url: `${baseUrl}/enviarArchCont`,
            dataType: 'json',
            async: false,
            cache: false,
            data: { 'num': ++archCont },
        });
        // Luego se le manda el archivo al controlador.
        $.ajax({
            url: `${baseUrl}/subirArchivo`,
            data: data,
            cache: false,
            contentType: false,
            processData: false,
            type: 'POST',
            success: function (result) {
                $('#archivo').val('');
                $("#archivosDiv").html(result);
            }
        });
    }
})

// Descargar un archivo.
$('#archivosDiv').on('click', '.dload', function () {

    // Encontrar el número de archivo.
    var $row = $(this).closest("tr");  
    var $text = $row.find(".numArch").text();

    //Descargar el archivo
    window.location = `${baseUrl}/descargarArchivo?numArch=` + $text;
});

// Borrar arhcivos de la tabla.
$('#archivosDiv').on('click', '.rm-archivo', function () {

    // Encontrar el número de autor que 
    var $row = $(this).closest("tr");  
    var $text = $row.find(".numArch").text();

    // Crear el objeto que se le pasa al controlador.
    var arch = new Object()
    arch.numArchivo = $text;

    $.ajax({
        type: 'POST',
        url: `${baseUrl}/borrarArchivo`,
        async: false,
        contentType: 'application/json; charset=utf-8',
        dataType: 'html',
        data: JSON.stringify(arch),
        success: function (result) {
            $("#archivosDiv").html(result);
        }
    });
    
});

