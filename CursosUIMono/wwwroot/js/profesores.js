function agregarProfesor() {
    limpiarFormularioAgregar();
    $('#agregarProfesorModal').modal('show');        
}    


function limpiarFormularioAgregar() {
    $("#formaAgregar #nombre").val("");
    $("#formaAgregar #email").val("");
    $("#formaAgregar #telefono").val("");
    $("#formaAgregar #especializacion").val("");
    $("#formaAgregar #id").val("0");        
}

function editarProfesor(id) {
    $.ajax({
        type: "GET",
        url: "/api/Profesores/"+id,
        dataType: "json",
        contentType: "application/json",        
        success: function (respuesta) {
            if (respuesta.exito) {
                cargarDatosProfesor(respuesta.datos);
                mostrarDialogoEditarProfesor();                
            } else {
                alert("Error al agregar el profesor.");
            }
        },
        error: function () {
            alert("Error en la solicitud.");
        }
    });    
}

function cargarDatosProfesor(profesor) {        
    $("#formaEditar #id").val(profesor.id);
    $("#formaEditar #nombre").val(profesor.nombre);
    $("#formaEditar #email").val(profesor.email);
    $("#formaEditar #telefono").val(profesor.telefono);
    $("#formaEditar #especializacion").val(profesor.especializacion);
}


function mostrarDialogoEditarProfesor() {
    $('#editarProfesorPanel').offcanvas('show');
}    


function guardarDatosAgregarProfesor() {
    if (validarDatosAgregar()) {
        var forma = {
            nombre: $("#formaAgregar #nombre").val(),
            email: $("#formaAgregar #email").val(),
            telefono: $("#formaAgregar #telefono").val(),
            especializacion: $("#formaAgregar #especializacion").val()
        };
        $.ajax({
            type: "POST",
            url: "/api/Profesores",
            dataType: "json",
            contentType: "application/json",
            data: JSON.stringify(forma),
            success: function (respuesta) {
                if (respuesta.exito) {
                    alert("Profesor agregado exitosamente.");
                    location.reload();
                } else {
                    alert("Error al agregar el profesor.");
                }
            },
            error: function () {
                alert("Error en la solicitud.");
            }
        });
    } else {
        alert("Por favor, complete todos los campos requeridos.");
    }
}

function validarDatosAgregar() {        
    var form = $("#formaAgregar");
    if (form.valid()) {            
        return true
    } else {            
        return false;
    }
}

function guardarDatosEditarProfesor() {
    if (validarDatosEditar()) {        
        var forma = {
            id: $("#formaEditar #id").val(),
            nombre: $("#formaEditar #nombre").val(),
            email: $("#formaEditar #email").val(),
            telefono: $("#formaEditar #telefono").val(),
            especializacion: $("#formaEditar #especializacion").val()
        };
        $.ajax({
            type: "PUT",
            url: "/api/Profesores/"+forma.id,
            dataType: "json",
            contentType: "application/json",
            data: JSON.stringify(forma),
            success: function (respuesta) {
                if (respuesta.exito) {
                    alert("Profesor modificado exitosamente.");
                    location.reload();
                } else {
                    alert("Error al modificar el profesor.");
                }
            },
            error: function () {
                alert("Error en la solicitud.");
            }
        });
    } else {
        alert("Por favor, complete todos los campos requeridos.");
    }
}

function validarDatosEditar() {        
    var form = $("#formaEditar");         
    if (form.valid()) {            
        return true
    } else {            
        return false;
    }
}

function eliminarProfesor(id, nombre) {
    $("#formaEliminar #id").val(id);
    $("#nombreProfesorEliminar").html(nombre);
    $("#eliminarProfesorModal").modal("show");   
}

function guardarDatosEliminarProfesor() {
    
    let id = $("#formaEliminar #id").val();        
    $.ajax({
        type: "DELETE",
        url: "/api/Profesores/"+id,
        dataType: "json",
        contentType: "application/json",        
        success: function (respuesta) {
            if (respuesta.exito) {
                alert("Profesor eliminado exitosamente.");
                location.reload();
            } else {
                alert("Error al eliminar el profesor.");
            }
        },
        error: function () {
            alert("Error en la solicitud.");
        }
    });    
}