package mx.intellcomm.myapplication.data.model


data class UsuarioDTO(val id: Int,
                      val correoElectronico: String,
                      val paterno: String,
                      val materno: String,
                      val nombre: String,
                      val puesto: String,
                      val activo: Boolean
)