package mx.intellcomm.myapplication.data.model

/**
 * Data class that captures user information for logged in users retrieved from LoginRepository
 */
data class RespuestaValidacionUsuarioDTO(
    val token: String,
    val usuario: UsuarioDTO?,
    val correcto: Boolean
)