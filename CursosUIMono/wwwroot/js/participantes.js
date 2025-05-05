var tablaParticipantes;
$(document).ready(function () {
    tablaParticipantes = new DataTable("#tablaParticipantes", {
        language: {
            url: '//cdn.datatables.net/plug-ins/2.2.2/i18n/es-ES.json',            
        },
        ajax: '/api/Participantes/data',
        columns: [
            { data: 'nombre', searchable:true },
            { data: 'email',searchable:false },
            { data: 'telefono',searchable:false },
            { data: 'opciones',searchable:false }
        ]
    });
    
});
    

function agregarParticipante() {
    limpiarFormularioAgregar();
    $('#agregarParticipanteModal').modal('show');        
}    


function limpiarFormularioAgregar() {
    $("#formaAgregar #nombre").val("");
    $("#formaAgregar #email").val("");
    $("#formaAgregar #telefono").val("");    
    $("#formaAgregar #id").val("0");        
}

function editarParticipante(id) {
    $.ajax({
        type: "GET",
        url: "/api/Participantes/"+id,
        dataType: "json",
        contentType: "application/json",        
        success: function (respuesta) {
            if (respuesta.exito) {
                cargarDatosParticipante(respuesta.datos);
                mostrarDialogoEditarParticipante();                
            } else {
                alert("Error al agregar el participante.");
            }
        },
        error: function () {
            alert("Error en la solicitud.");
        }
    });    
}

function cargarDatosParticipante(participante) {        
    $("#formaEditar #id").val(participante.id);
    $("#formaEditar #nombre").val(participante.nombre);
    $("#formaEditar #email").val(participante.email);
    $("#formaEditar #telefono").val(participante.telefono);
    $("#formaEditar #especializacion").val(participante.especializacion);
}


function mostrarDialogoEditarParticipante() {
    $('#editarParticipantePanel').offcanvas('show');
}    


function guardarDatosAgregarParticipante() {
    if (validarDatosAgregar()) {
        var forma = {
            nombre: $("#formaAgregar #nombre").val(),
            email: $("#formaAgregar #email").val(),
            telefono: $("#formaAgregar #telefono").val(),
            especializacion: $("#formaAgregar #especializacion").val()
        };
        $.ajax({
            type: "POST",
            url: "/api/Participantes",
            dataType: "json",
            contentType: "application/json",
            data: JSON.stringify(forma),
            success: function (respuesta) {
                if (respuesta.exito) {
                    alert("Participante agregado exitosamente.");
                    tablaParticipantes.ajax.reload();
                    $('#agregarParticipanteModal').modal('hide');
                } else {
                    alert("Error al agregar el participante.");
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

function guardarDatosEditarParticipante() {
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
            url: "/api/Participantes/"+forma.id,
            dataType: "json",
            contentType: "application/json",
            data: JSON.stringify(forma),
            success: function (respuesta) {
                if (respuesta.exito) {
                    alert("Participante modificado exitosamente.");
                    tablaParticipantes.ajax.reload();
                    $('#editarParticipantePanel').offcanvas('hide');
                } else {
                    alert("Error al modificar el participante.");
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

function eliminarParticipante(id, nombre) {
    $("#formaEliminar #id").val(id);
    $("#nombreParticipanteEliminar").html(nombre);
    $("#eliminarParticipanteModal").modal("show");   
}

function guardarDatosEliminarParticipante() {
    
    let id = $("#formaEliminar #id").val();        
    $.ajax({
        type: "DELETE",
        url: "/api/Participantes/"+id,
        dataType: "json",
        contentType: "application/json",        
        success: function (respuesta) {
            if (respuesta.exito) {
                alert("Participante eliminado exitosamente.");
                tablaParticipantes.ajax.reload();
                $('#agregarParticipanteModal').modal('hide');
            } else {
                alert("Error al eliminar el participante.");
            }
        },
        error: function () {
            alert("Error en la solicitud.");
        }
    });    
}