package mx.intellcomm.myapplication.data

import android.content.Context
import mx.intellcomm.myapplication.data.model.UsuarioDTO
import mx.intellcomm.myapplication.data.model.RespuestaValidacionUsuarioDTO


class LoginRepository(val context: Context, val dataSource: LoginDataSource = LoginDataSource(context)) {

    var user: RespuestaValidacionUsuarioDTO? = null
        private set

    val isLoggedIn: Boolean
        get() = user != null

    init {
        // If user credentials will be cached in local storage, it is recommended it be encrypted
        // @see https://developer.android.com/training/articles/keystore
        user = null
    }

    fun logout() {
        user = null
        dataSource.logout()
    }

    suspend fun login(username: String, password: String): RespuestaValidacionUsuarioDTO {

        val result = dataSource.login(username, password)

        setLoggedInUser(result)

        return result
    }

    private fun setLoggedInUser(loggedInUser: RespuestaValidacionUsuarioDTO) {
        this.user = loggedInUser

    }
}