var agregandoCurso = false;
var tablaCursos;
$(document).ready(function () {
    tablaCursos = new DataTable("#tablaCursos", {
        language: {
            url: '//cdn.datatables.net/plug-ins/2.2.2/i18n/es-ES.json',            
        }
        
    });
    
});

function agregarCurso() {
    agregandoCurso = true;
    limpiarFormularioAgregar();
    $('#agregarCursoModal').modal('show');        
}    

function limpiarFormularioAgregar() {
    $("#formaAgregar #nombre").val("");
    $("#formaAgregar #descripcion").val("");
    $("#formaAgregar #precio").val("");
    $("#formaAgregar #profesor").val("");
    $("#formaAgregar #fechaInicio").val("");
    $("#formaAgregar #fechaTermino").val("");
    $("#formaAgregar #id").val("0");        
}

function editarCurso(id) {
    $.ajax({
        type: "GET",
        url: "/api/Cursos/"+id,
        dataType: "json",
        contentType: "application/json",        
        success: function (respuesta) {
            if (respuesta.exito) {
                agregandoCurso = false;
                cargarDatosCurso(respuesta.datos);
                mostrarDialogoEditarCurso();                
            } else {
                alert("Error al agregar el profesor.");
            }
        },
        error: function () {
            alert("Error en la solicitud.");
        }
    });    
}

function cargarDatosCurso(curso) {        
    $("#formaAgregar #id").val(curso.id);
    $("#formaAgregar #nombre").val(curso.nombre);
    $("#formaAgregar #descripcion").val(curso.descripcion);
    $("#formaAgregar #precio").val(curso.precio);
    $("#formaAgregar #profesor").val(curso.profesor);
    if (curso.fechaInicio != null) 
        $("#formaAgregar #fechaInicio").val(curso.fechaInicio.substring(0, 10));
    if (curso.fechaTermino != null) 
        $("#formaAgregar #fechaTermino").val(curso.fechaTermino.substring(0, 10));
}


function mostrarDialogoEditarCurso() {
    $('#agregarCursoModal').modal('show');
}    


function guardarDatosCurso() {
    if (validarDatos()) {
        var forma = {
            id: $("#formaAgregar #id").val(),
            nombre: $("#formaAgregar #nombre").val(),
            descripcion: $("#formaAgregar #descripcion").val(),
            profesor: $("#formaAgregar #profesor").val(),
            idProfesor: $("#formaAgregar #profesor").val(),
            precio: $("#formaAgregar #precio").val(),
            fechaInicio: $("#formaAgregar #fechaInicio").val(),
            fechaTermino: $("#formaAgregar #fechaTermino").val()
        };
        if (agregandoCurso) {
            verbo = "POST";
            url = "/api/Cursos";
            mnensaje = "Curso agregado exitosamente.";
        } else {
            verbo = "PUT";
            url = "/api/Cursos/"+forma.id;
            mnensaje = "Curso modificado exitosamente.";
        }
        $.ajax({
            type: verbo,
            url: url,
            dataType: "json",
            contentType: "application/json",
            data: JSON.stringify(forma),
            success: function (respuesta) {
                if (respuesta.exito) {
                    alert(mnensaje);
                    location.reload();
                } else {
                    alert("Error al gurdar el curso.");
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

function validarDatos() {        
    var form = $("#formaAgregar");
    if (form.valid()) {            
        return true
    } else {            
        return false;
    }
}



function eliminarCurso(id, nombre) {
    $("#formaEliminar #id").val(id);
    $("#nombreCursoEliminar").html(nombre);
    $("#eliminarCursoModal").modal("show");   
}

function guardarDatosEliminarCurso() {
    
    let id = $("#formaEliminar #id").val();        
    $.ajax({
        type: "DELETE",
        url: "/api/Cursos/"+id,
        dataType: "json",
        contentType: "application/json",        
        success: function (respuesta) {
            if (respuesta.exito) {
                alert("Curso eliminado exitosamente.");
                location.reload();
            } else {
                alert("Error el eliminar el curso.");
            }
        },
        error: function () {
            alert("Error en la solicitud.");
        }
    });    
}

function asignarCursoImagen(idCurso) {
    $("#formaAsignarImagen #idCurso").val(idCurso);    
    $.ajax({
        type: "GET",
        url: "/api/CursoImagen/Existe/"+idCurso,
        processData: false, 
        contentType: false,    
        success: function (respuesta) {
            if (respuesta.exito) {
                $("#formaAsignarImagen #imagen").attr("src", respuesta.datos);    
                $("#formaAsignarImagen #imagen").css("display", "block");
            } else {
                $("#formaAsignarImagen #imagen").attr("src", "");    
                $("#formaAsignarImagen #imagen").css("display", "none");
            }
            $('#asignarImagenModal').modal('show');        
        },
        error: function () {
            alert("Error en la solicitud.");
        }
    });    

        
    
}    

function guardarImagenCurso() {
    var archivo = $("#formaAsignarImagen #archivoImagen")[0].files[0];
    
    if (!archivo) {
        alert("Por favor, seleccione un archivo.");
        return;
    }
    let idCurso = $("#formaAsignarImagen #idCurso").val();        
    var formData = new FormData();
    formData.append("archivo", archivo);     

    $.ajax({
        type: "POST",
        url: "/api/CursoImagen/"+idCurso,
        processData: false, 
        contentType: false,
        data: formData,
        success: function (respuesta) {
            if (respuesta.exito) {
                alert("Imagen de Curso guardada exitosamente.");
                location.reload();
            } else {
                alert("Error el eliminar el curso.");
            }
        },
        error: function () {
            alert("Error en la solicitud.");
        }
    });    
}