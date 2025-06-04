package mx.intellcomm.myapplication.data

import android.content.Context
import mx.intellcomm.myapplication.data.model.PeticionInicioSesionDTO
import mx.intellcomm.myapplication.data.model.RespuestaValidacionUsuarioDTO
import mx.intellcomm.myapplication.data.model.UsuarioDTO
import mx.intellcomm.myapplication.service.LoginService
import java.io.IOException

/**
 * Class that handles authentication w/ login credentials and retrieves user information.
 */
class LoginDataSource(val context: Context) {

    private val loginService = LoginService.create(context)

    suspend fun login(username: String, password: String): RespuestaValidacionUsuarioDTO {
        return try {
            val response = loginService.login(PeticionInicioSesionDTO(username, password))
            if (response.isSuccessful) {
                return response.body()!!
            } else {
                return RespuestaValidacionUsuarioDTO("", null, false)
            }
        } catch (e: Exception) {
            return RespuestaValidacionUsuarioDTO(e.toString(), null, false)
        }
    }

    fun logout() {

    }
}